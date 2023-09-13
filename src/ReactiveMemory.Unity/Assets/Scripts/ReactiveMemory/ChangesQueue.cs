using System;
using System.Collections.Generic;

namespace ReactiveMemory
{
    public class ChangesQueue<TEntity> : IDbChangesPublisher, IChangesQueue<TEntity>
    {
        public IObservable<EntityChange<TEntity>> OnChange => _observer;

        private readonly ChangesConveyor _changesConveyor;
        private readonly Queue<EntityChange<TEntity>> _entityChanges = new();
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

        public void Clear()
        {
            _entityChanges.Clear();
        }

        public void OnCompleted()
        {
            _observer.OnCompleted();
        }

        public Action Prepare()
        {
            var change = _entityChanges.Dequeue();
            return () => _observer.OnNext(change);
        }
    }
}