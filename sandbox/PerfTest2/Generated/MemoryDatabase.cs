// <auto-generated />
#pragma warning disable CS0105
using LiteDB;
using MessagePack;
using ReactiveMemory.Validation;
using ReactiveMemory;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using TestPerfLiteDB;
using TestPerfLiteDB.Tables;

namespace TestPerfLiteDB
{
   public interface IMemoryDatabase
   {
        public IObservable<EntityChange<TEntity>> OnChange<TEntity>();
        public TestDocTable TestDocTable { get; }
   }

   public sealed class MemoryDatabase : MemoryDatabaseBase, IMemoryDatabase
   {
        public TestDocTable TestDocTable { get; private set; }

        public MemoryDatabase(
            TestDocTable TestDocTable
        , ChangesConveyor changesConveyor) : base(changesConveyor)
        {
            this.TestDocTable = TestDocTable;
        }

        public MemoryDatabase(byte[] databaseBinary, IChangesMediatorFactory changesMediatorFactory = null, bool internString = true, MessagePack.IFormatterResolver formatterResolver = null, int maxDegreeOfParallelism = 1)
            : base(databaseBinary, changesMediatorFactory, internString, formatterResolver, maxDegreeOfParallelism)
        {
        }

        public MemoryDatabase(byte[] databaseBinary, ChangesConveyor changesConveyor, bool internString = true, MessagePack.IFormatterResolver formatterResolver = null, int maxDegreeOfParallelism = 1)
            : base(databaseBinary, changesConveyor, internString, formatterResolver, maxDegreeOfParallelism)
        {
        }

        protected override void Init(Dictionary<string, (int offset, int count)> header, System.ReadOnlyMemory<byte> databaseBinary, MessagePack.MessagePackSerializerOptions options, int maxDegreeOfParallelism)
        {
            if(maxDegreeOfParallelism == 1)
            {
                InitSequential(header, databaseBinary, options, maxDegreeOfParallelism);
            }
            else
            {
                InitParallel(header, databaseBinary, options, maxDegreeOfParallelism);
            }
        }

        void InitSequential(Dictionary<string, (int offset, int count)> header, System.ReadOnlyMemory<byte> databaseBinary, MessagePack.MessagePackSerializerOptions options, int maxDegreeOfParallelism)
        {
            this.TestDocTable = ExtractTableData<TestDoc, TestDocTable>(header, databaseBinary, options, xs => new TestDocTable(xs));
        }

        void InitParallel(Dictionary<string, (int offset, int count)> header, System.ReadOnlyMemory<byte> databaseBinary, MessagePack.MessagePackSerializerOptions options, int maxDegreeOfParallelism)
        {
            var extracts = new Action[]
            {
                () => this.TestDocTable = ExtractTableData<TestDoc, TestDocTable>(header, databaseBinary, options, xs => new TestDocTable(xs)),
            };
            
            System.Threading.Tasks.Parallel.Invoke(new System.Threading.Tasks.ParallelOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            }, extracts);
        }

        public Transaction BeginTransaction()
        {
            return new Transaction(this);
        }

        public DatabaseBuilder ToDatabaseBuilder()
        {
            var builder = new DatabaseBuilder();
            builder.Append(this.TestDocTable.GetRawDataUnsafe());
            return builder;
        }

        public DatabaseBuilder ToDatabaseBuilder(MessagePack.IFormatterResolver resolver)
        {
            var builder = new DatabaseBuilder(resolver);
            builder.Append(this.TestDocTable.GetRawDataUnsafe());
            return builder;
        }

#if !DISABLE_MASTERMEMORY_VALIDATOR

        public ValidateResult Validate()
        {
            var result = new ValidateResult();
            var database = new ValidationDatabase(new object[]
            {
                TestDocTable,
            });

            ((ITableUniqueValidate)TestDocTable).ValidateUnique(result);
            ValidateTable(TestDocTable.All, database, "id", TestDocTable.PrimaryKeySelector, result);

            return result;
        }

#endif

        static ReactiveMemory.Meta.MetaDatabase metaTable;

        public static object GetTable(MemoryDatabase db, string tableName)
        {
            switch (tableName)
            {
                case "TestDoc":
                    return db.TestDocTable;
                
                default:
                    return null;
            }
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static ReactiveMemory.Meta.MetaDatabase GetMetaDatabase()
        {
            if (metaTable != null) return metaTable;

            var dict = new Dictionary<string, ReactiveMemory.Meta.MetaTable>();
            dict.Add("TestDoc", TestPerfLiteDB.Tables.TestDocTable.CreateMetaTable());

            metaTable = new ReactiveMemory.Meta.MetaDatabase(dict);
            return metaTable;
        }

#endif
    }
}