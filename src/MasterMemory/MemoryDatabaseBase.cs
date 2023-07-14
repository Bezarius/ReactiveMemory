using ReactiveMemory.Internal;
using MessagePack;
using MessagePack.Formatters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using ReactiveMemory.Validation;
using System.Collections;

namespace ReactiveMemory
{
    internal static class TypeIdGenerator
    {
        private static int _index;

        public static int Index => ++_index;
    }

    internal static class TypeId<T>
    {
        // ReSharper disable once StaticMemberInGenericType
        public static int Id { get; } = TypeIdGenerator.Index;
    }


    public interface IChangesQueue<TElement>
    {
        IObservable<EntityChange<TElement>> OnChange { get; }
        void EnqueueAdd(TElement added);
        void EnqueueRemove(TElement element);
        void EnqueueUpdate(TElement updated, TElement old);
    }

    public interface IChangesMediator<TElement> : IObserver<EntityChange<TElement>>, IObservable<EntityChange<TElement>>
    {

    }

    public interface IChangesMediatorFactory
    {
        IChangesMediator<TElement> Create<TElement>();
    }

    public class ChangesConveyor
    {
        private readonly SparseSet<IDbChangesPublisher> _dbChangesPublishers = new SparseSet<IDbChangesPublisher>();
        private readonly Queue<IDbChangesPublisher> _publishersQueue = new Queue<IDbChangesPublisher>();
        private readonly IChangesMediatorFactory _changesMediatorFactory;

        public ChangesConveyor(IChangesMediatorFactory changesMediatorFactory)
        {
            _changesMediatorFactory = changesMediatorFactory;
        }

        public IChangesQueue<TElement> GetQueue<TElement>()
        {
            var typeId = TypeId<TElement>.Id;
            if (!_dbChangesPublishers.Contains(typeId))
            {
                _dbChangesPublishers.Add(typeId, new ChangesQueue<TElement>(this, _changesMediatorFactory.Create<TElement>()));
            }
            var publisher = _dbChangesPublishers[typeId];
            return publisher as IChangesQueue<TElement>;
        }
        
        internal void Enqueue(IDbChangesPublisher publisher)
        {
            _publishersQueue.Enqueue(publisher);
        }

        public void Publish()
        {
            while (_publishersQueue.Count > 0)
            {
                var publisher = _publishersQueue.Dequeue();
                publisher.PublishNext();
            }
        }
    }

    public interface IDbChangesPublisher
    {
        void PublishNext();
    }

    public class ChangesQueue<TEntity> : IDbChangesPublisher, IChangesQueue<TEntity>
    {
        public IObservable<EntityChange<TEntity>> OnChange => _observer;

        private readonly ChangesConveyor _changesConveyor;
        private readonly Queue<EntityChange<TEntity>> _entityChanges = new Queue<EntityChange<TEntity>>();
        private readonly IChangesMediator<TEntity> _observer;

        public ChangesQueue(ChangesConveyor changesConveyor, IChangesMediator<TEntity> observer)
        {
            _changesConveyor = changesConveyor;
            _observer = observer;
        }

        public void PublishNext()
        {
            var change = _entityChanges.Dequeue();
            _observer.OnNext(change);
        }

        public void EnqueueAdd(TEntity added)
        {
            _entityChanges.Enqueue(new EntityChange<TEntity>(EEntityChangeType.Add, added));
            _changesConveyor.Enqueue(this);
        }

        public void EnqueueRemove(TEntity element)
        {
            _entityChanges.Enqueue(new EntityChange<TEntity>(EEntityChangeType.Remove, element));
            _changesConveyor.Enqueue(this);
        }

        public void EnqueueUpdate(TEntity updated, TEntity old)
        {
            _entityChanges.Enqueue(new EntityChange<TEntity>(EEntityChangeType.Update, updated, old));
            _changesConveyor.Enqueue(this);
        }
    }

    public enum EEntityChangeType
    {
        Add,
        Update,
        Remove
    }

    public readonly struct EntityChange<TEntity>
    {
        public EEntityChangeType Change { get; }
        public TEntity Entity { get; }
        public TEntity Old { get; }

        public EntityChange(EEntityChangeType change, TEntity entity)
        {
            Change = change;
            Entity = entity;
            Old = default;
        }

        public EntityChange(EEntityChangeType change, TEntity entity, TEntity old)
        {
            Change = change;
            Entity = entity;
            Old = old;
        }
    }


    public abstract class MemoryDatabaseBase
    {
        public IObservable<EntityChange<TEntity>> OnChange<TEntity>() => ChangesConveyor?.GetQueue<TEntity>().OnChange;
        

        public readonly ChangesConveyor ChangesConveyor;

        protected MemoryDatabaseBase(ChangesConveyor changesConveyor)
        {
            ChangesConveyor = changesConveyor;
        }

        public MemoryDatabaseBase(byte[] databaseBinary, IChangesMediatorFactory changesMediatorFactory, bool internString = true,
            IFormatterResolver formatterResolver = null, int maxDegreeOfParallelism = 1) : this(new ChangesConveyor(changesMediatorFactory))
        {
            var reader = new MessagePackReader(databaseBinary);
            var formatter = new DictionaryFormatter<string, (int, int)>();

            var header = formatter.Deserialize(ref reader, HeaderFormatterResolver.StandardOptions);
            var resolver = formatterResolver ?? MessagePackSerializer.DefaultOptions.Resolver;
            if (internString)
            {
                resolver = new InternStringResolver(resolver);
            }

            if (maxDegreeOfParallelism < 1)
            {
                maxDegreeOfParallelism = 1;
            }

            Init(header, databaseBinary.AsMemory((int)reader.Consumed),
                MessagePackSerializer.DefaultOptions.WithResolver(resolver)
                    .WithCompression(MessagePackCompression.Lz4Block), maxDegreeOfParallelism);
        }

        protected static TView ExtractTableData<T, TView>(Dictionary<string, (int offset, int count)> header,
            ReadOnlyMemory<byte> databaseBinary, MessagePackSerializerOptions options, Func<T[], TView> createView)
        {
            var tableName = typeof(T).GetCustomAttribute<MemoryTableAttribute>();
            if (tableName == null)
                throw new InvalidOperationException("Type is not annotated MemoryTableAttribute. Type:" +
                                                    typeof(T).FullName);

            if (header.TryGetValue(tableName.TableName, out var segment))
            {
                var data = MessagePackSerializer.Deserialize<T[]>(databaseBinary.Slice(segment.offset, segment.count),
                    options);
                return createView(data);
            }
            else
            {
                // return empty
                var data = Array.Empty<T>();
                return createView(data);
            }
        }

        protected abstract void Init(Dictionary<string, (int offset, int count)> header,
            ReadOnlyMemory<byte> databaseBinary, MessagePackSerializerOptions options, int maxDegreeOfParallelism);

        public static TableInfo[] GetTableInfo(byte[] databaseBinary, bool storeTableData = true)
        {
            var formatter = new DictionaryFormatter<string, (int, int)>();
            var reader = new MessagePackReader(databaseBinary);
            var header = formatter.Deserialize(ref reader, HeaderFormatterResolver.StandardOptions);

            return header.Select(x =>
                new TableInfo(x.Key, x.Value.Item2, storeTableData ? databaseBinary : null, x.Value.Item1)).ToArray();
        }

        protected void ValidateTable<TElement>(IReadOnlyList<TElement> table, ValidationDatabase database,
            string pkName, Delegate pkSelector, ValidateResult result)
        {
            var onceCalled = new System.Runtime.CompilerServices.StrongBox<bool>(false);
            foreach (var item in table)
            {
                if (item is IValidatable<TElement> validatable)
                {
                    var validator = new Validator<TElement>(database, item, result, onceCalled, pkName, pkSelector);
                    validatable.Validate(validator);
                }
            }
        }
    }

    /// <summary>
    /// Diagnostic info of ReactiveMemory's table.
    /// </summary>
    public class TableInfo
    {
        public string TableName { get; }
        public int Size { get; }
        byte[] binaryData;

        public TableInfo(string tableName, int size, byte[] rawBinary, int offset)
        {
            TableName = tableName;
            Size = size;
            if (rawBinary != null)
            {
                this.binaryData = new byte[size];
                Array.Copy(rawBinary, offset, binaryData, 0, size);
            }
        }

        public string DumpAsJson()
        {
            return DumpAsJson(MessagePackSerializer.DefaultOptions);
        }

        public string DumpAsJson(MessagePackSerializerOptions options)
        {
            if (binaryData == null)
            {
                throw new InvalidOperationException(
                    "DumpAsJson can only call from GetTableInfo(storeTableData = true).");
            }

            return MessagePackSerializer.ConvertToJson(binaryData,
                options.WithCompression(MessagePackCompression.Lz4Block));
        }
    }

    public class SparseSet<T> : IEnumerable
    {
        private T[] _dense;
        private int[] _sparse;
        private int _count;

        public SparseSet(int capacity = 10)
        {
            _dense = new T[capacity];
            _sparse = new int[capacity];
            _count = 0;
        }

        public int Count => _count;

        public void Add(int objectId, T item)
        {
            if (Contains(objectId))
                return;

            if (objectId + 1 >= _sparse.Length)
                Array.Resize(ref _sparse, objectId + 1);

            if (_dense.Length <= _count)
                Array.Resize(ref _dense, _dense.Length * 2);

            _dense[_count] = item;
            _sparse[objectId] = _count;

            _count++;
        }

        public T this[int objectId]
        {
            get
            {
                if (Contains(objectId))
                    return _dense[_sparse[objectId]];
                throw new ArgumentException($"Object with index {objectId} does not exist in the SparseSet.");
            }
        }


        public void Remove(int objectId)
        {
            if (!Contains(objectId))
                return;

            var itemIndex = _sparse[objectId];
            var lastItem = _dense[_count - 1];

            _dense[itemIndex] = lastItem;
            _sparse[objectId] = itemIndex;

            _dense[_count - 1] = default(T);
            _sparse[objectId] = 0;

            _count--;
        }

        public bool Contains(int objectId)
        {
            return objectId < _sparse.Length && _sparse[objectId] != 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < _count; i++)
            {
                yield return _dense[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}