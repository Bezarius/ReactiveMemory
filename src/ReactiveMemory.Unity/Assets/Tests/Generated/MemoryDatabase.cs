// <auto-generated />
#pragma warning disable CS0105
using MessagePack;
using ReactiveMemory.Tests.TestStructures;
using ReactiveMemory.Tests;
using ReactiveMemory.Validation;
using ReactiveMemory;
using System.Collections.Generic;
using System.Text;
using System;
using ReactiveMemory.Test.Tables;

namespace ReactiveMemory.Test
{
   public interface IMemoryDatabase
   {
        public IObservable<EntityChange<TEntity>> OnChange<TEntity>();
        public FailTable FailTable { get; }
        public ItemMasterTable ItemMasterTable { get; }
        public ItemMasterEmptyValidateTable ItemMasterEmptyValidateTable { get; }
        public PersonModelTable PersonModelTable { get; }
        public QuestMasterTable QuestMasterTable { get; }
        public QuestMasterEmptyValidateTable QuestMasterEmptyValidateTable { get; }
        public SampleTable SampleTable { get; }
        public SequentialCheckMasterTable SequentialCheckMasterTable { get; }
        public SingleMasterTable SingleMasterTable { get; }
        public SkillMasterTable SkillMasterTable { get; }
        public TestMasterTable TestMasterTable { get; }
        public UserLevelTable UserLevelTable { get; }
   }

   public sealed class MemoryDatabase : MemoryDatabaseBase, IMemoryDatabase
   {
        public FailTable FailTable { get; private set; }
        public ItemMasterTable ItemMasterTable { get; private set; }
        public ItemMasterEmptyValidateTable ItemMasterEmptyValidateTable { get; private set; }
        public PersonModelTable PersonModelTable { get; private set; }
        public QuestMasterTable QuestMasterTable { get; private set; }
        public QuestMasterEmptyValidateTable QuestMasterEmptyValidateTable { get; private set; }
        public SampleTable SampleTable { get; private set; }
        public SequentialCheckMasterTable SequentialCheckMasterTable { get; private set; }
        public SingleMasterTable SingleMasterTable { get; private set; }
        public SkillMasterTable SkillMasterTable { get; private set; }
        public TestMasterTable TestMasterTable { get; private set; }
        public UserLevelTable UserLevelTable { get; private set; }

        public MemoryDatabase(
            FailTable FailTable,
            ItemMasterTable ItemMasterTable,
            ItemMasterEmptyValidateTable ItemMasterEmptyValidateTable,
            PersonModelTable PersonModelTable,
            QuestMasterTable QuestMasterTable,
            QuestMasterEmptyValidateTable QuestMasterEmptyValidateTable,
            SampleTable SampleTable,
            SequentialCheckMasterTable SequentialCheckMasterTable,
            SingleMasterTable SingleMasterTable,
            SkillMasterTable SkillMasterTable,
            TestMasterTable TestMasterTable,
            UserLevelTable UserLevelTable
        , ChangesConveyor changesConveyor) : base(changesConveyor)
        {
            this.FailTable = FailTable;
            this.ItemMasterTable = ItemMasterTable;
            this.ItemMasterEmptyValidateTable = ItemMasterEmptyValidateTable;
            this.PersonModelTable = PersonModelTable;
            this.QuestMasterTable = QuestMasterTable;
            this.QuestMasterEmptyValidateTable = QuestMasterEmptyValidateTable;
            this.SampleTable = SampleTable;
            this.SequentialCheckMasterTable = SequentialCheckMasterTable;
            this.SingleMasterTable = SingleMasterTable;
            this.SkillMasterTable = SkillMasterTable;
            this.TestMasterTable = TestMasterTable;
            this.UserLevelTable = UserLevelTable;
        }

        public MemoryDatabase(byte[] databaseBinary, IChangesMediatorFactory changesMediatorFactory, bool internString = true, MessagePack.IFormatterResolver formatterResolver = null, int maxDegreeOfParallelism = 1)
            : base(databaseBinary, changesMediatorFactory, internString, formatterResolver, maxDegreeOfParallelism)
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
            this.FailTable = ExtractTableData<Fail, FailTable>(header, databaseBinary, options, xs => new FailTable(xs));
            this.ItemMasterTable = ExtractTableData<ItemMaster, ItemMasterTable>(header, databaseBinary, options, xs => new ItemMasterTable(xs));
            this.ItemMasterEmptyValidateTable = ExtractTableData<ItemMasterEmptyValidate, ItemMasterEmptyValidateTable>(header, databaseBinary, options, xs => new ItemMasterEmptyValidateTable(xs));
            this.PersonModelTable = ExtractTableData<PersonModel, PersonModelTable>(header, databaseBinary, options, xs => new PersonModelTable(xs));
            this.QuestMasterTable = ExtractTableData<QuestMaster, QuestMasterTable>(header, databaseBinary, options, xs => new QuestMasterTable(xs));
            this.QuestMasterEmptyValidateTable = ExtractTableData<QuestMasterEmptyValidate, QuestMasterEmptyValidateTable>(header, databaseBinary, options, xs => new QuestMasterEmptyValidateTable(xs));
            this.SampleTable = ExtractTableData<Sample, SampleTable>(header, databaseBinary, options, xs => new SampleTable(xs));
            this.SequentialCheckMasterTable = ExtractTableData<SequentialCheckMaster, SequentialCheckMasterTable>(header, databaseBinary, options, xs => new SequentialCheckMasterTable(xs));
            this.SingleMasterTable = ExtractTableData<SingleMaster, SingleMasterTable>(header, databaseBinary, options, xs => new SingleMasterTable(xs));
            this.SkillMasterTable = ExtractTableData<SkillMaster, SkillMasterTable>(header, databaseBinary, options, xs => new SkillMasterTable(xs));
            this.TestMasterTable = ExtractTableData<TestMaster, TestMasterTable>(header, databaseBinary, options, xs => new TestMasterTable(xs));
            this.UserLevelTable = ExtractTableData<UserLevel, UserLevelTable>(header, databaseBinary, options, xs => new UserLevelTable(xs));
        }

        void InitParallel(Dictionary<string, (int offset, int count)> header, System.ReadOnlyMemory<byte> databaseBinary, MessagePack.MessagePackSerializerOptions options, int maxDegreeOfParallelism)
        {
            var extracts = new Action[]
            {
                () => this.FailTable = ExtractTableData<Fail, FailTable>(header, databaseBinary, options, xs => new FailTable(xs)),
                () => this.ItemMasterTable = ExtractTableData<ItemMaster, ItemMasterTable>(header, databaseBinary, options, xs => new ItemMasterTable(xs)),
                () => this.ItemMasterEmptyValidateTable = ExtractTableData<ItemMasterEmptyValidate, ItemMasterEmptyValidateTable>(header, databaseBinary, options, xs => new ItemMasterEmptyValidateTable(xs)),
                () => this.PersonModelTable = ExtractTableData<PersonModel, PersonModelTable>(header, databaseBinary, options, xs => new PersonModelTable(xs)),
                () => this.QuestMasterTable = ExtractTableData<QuestMaster, QuestMasterTable>(header, databaseBinary, options, xs => new QuestMasterTable(xs)),
                () => this.QuestMasterEmptyValidateTable = ExtractTableData<QuestMasterEmptyValidate, QuestMasterEmptyValidateTable>(header, databaseBinary, options, xs => new QuestMasterEmptyValidateTable(xs)),
                () => this.SampleTable = ExtractTableData<Sample, SampleTable>(header, databaseBinary, options, xs => new SampleTable(xs)),
                () => this.SequentialCheckMasterTable = ExtractTableData<SequentialCheckMaster, SequentialCheckMasterTable>(header, databaseBinary, options, xs => new SequentialCheckMasterTable(xs)),
                () => this.SingleMasterTable = ExtractTableData<SingleMaster, SingleMasterTable>(header, databaseBinary, options, xs => new SingleMasterTable(xs)),
                () => this.SkillMasterTable = ExtractTableData<SkillMaster, SkillMasterTable>(header, databaseBinary, options, xs => new SkillMasterTable(xs)),
                () => this.TestMasterTable = ExtractTableData<TestMaster, TestMasterTable>(header, databaseBinary, options, xs => new TestMasterTable(xs)),
                () => this.UserLevelTable = ExtractTableData<UserLevel, UserLevelTable>(header, databaseBinary, options, xs => new UserLevelTable(xs)),
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
            builder.Append(this.FailTable.GetRawDataUnsafe());
            builder.Append(this.ItemMasterTable.GetRawDataUnsafe());
            builder.Append(this.ItemMasterEmptyValidateTable.GetRawDataUnsafe());
            builder.Append(this.PersonModelTable.GetRawDataUnsafe());
            builder.Append(this.QuestMasterTable.GetRawDataUnsafe());
            builder.Append(this.QuestMasterEmptyValidateTable.GetRawDataUnsafe());
            builder.Append(this.SampleTable.GetRawDataUnsafe());
            builder.Append(this.SequentialCheckMasterTable.GetRawDataUnsafe());
            builder.Append(this.SingleMasterTable.GetRawDataUnsafe());
            builder.Append(this.SkillMasterTable.GetRawDataUnsafe());
            builder.Append(this.TestMasterTable.GetRawDataUnsafe());
            builder.Append(this.UserLevelTable.GetRawDataUnsafe());
            return builder;
        }

        public DatabaseBuilder ToDatabaseBuilder(MessagePack.IFormatterResolver resolver)
        {
            var builder = new DatabaseBuilder(resolver);
            builder.Append(this.FailTable.GetRawDataUnsafe());
            builder.Append(this.ItemMasterTable.GetRawDataUnsafe());
            builder.Append(this.ItemMasterEmptyValidateTable.GetRawDataUnsafe());
            builder.Append(this.PersonModelTable.GetRawDataUnsafe());
            builder.Append(this.QuestMasterTable.GetRawDataUnsafe());
            builder.Append(this.QuestMasterEmptyValidateTable.GetRawDataUnsafe());
            builder.Append(this.SampleTable.GetRawDataUnsafe());
            builder.Append(this.SequentialCheckMasterTable.GetRawDataUnsafe());
            builder.Append(this.SingleMasterTable.GetRawDataUnsafe());
            builder.Append(this.SkillMasterTable.GetRawDataUnsafe());
            builder.Append(this.TestMasterTable.GetRawDataUnsafe());
            builder.Append(this.UserLevelTable.GetRawDataUnsafe());
            return builder;
        }

#if !DISABLE_MASTERMEMORY_VALIDATOR

        public ValidateResult Validate()
        {
            var result = new ValidateResult();
            var database = new ValidationDatabase(new object[]
            {
                FailTable,
                ItemMasterTable,
                ItemMasterEmptyValidateTable,
                PersonModelTable,
                QuestMasterTable,
                QuestMasterEmptyValidateTable,
                SampleTable,
                SequentialCheckMasterTable,
                SingleMasterTable,
                SkillMasterTable,
                TestMasterTable,
                UserLevelTable,
            });

            ((ITableUniqueValidate)FailTable).ValidateUnique(result);
            ValidateTable(FailTable.All, database, "Id", FailTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)ItemMasterTable).ValidateUnique(result);
            ValidateTable(ItemMasterTable.All, database, "ItemId", ItemMasterTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)ItemMasterEmptyValidateTable).ValidateUnique(result);
            ValidateTable(ItemMasterEmptyValidateTable.All, database, "ItemId", ItemMasterEmptyValidateTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)PersonModelTable).ValidateUnique(result);
            ValidateTable(PersonModelTable.All, database, "RandomId", PersonModelTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)QuestMasterTable).ValidateUnique(result);
            ValidateTable(QuestMasterTable.All, database, "QuestId", QuestMasterTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)QuestMasterEmptyValidateTable).ValidateUnique(result);
            ValidateTable(QuestMasterEmptyValidateTable.All, database, "QuestId", QuestMasterEmptyValidateTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)SampleTable).ValidateUnique(result);
            ValidateTable(SampleTable.All, database, "Id", SampleTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)SequentialCheckMasterTable).ValidateUnique(result);
            ValidateTable(SequentialCheckMasterTable.All, database, "Id", SequentialCheckMasterTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)SingleMasterTable).ValidateUnique(result);
            ValidateTable(SingleMasterTable.All, database, "Id", SingleMasterTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)SkillMasterTable).ValidateUnique(result);
            ValidateTable(SkillMasterTable.All, database, "(SkillId, SkillLevel)", SkillMasterTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)TestMasterTable).ValidateUnique(result);
            ValidateTable(TestMasterTable.All, database, "TestID", TestMasterTable.PrimaryKeySelector, result);
            ((ITableUniqueValidate)UserLevelTable).ValidateUnique(result);
            ValidateTable(UserLevelTable.All, database, "Level", UserLevelTable.PrimaryKeySelector, result);

            return result;
        }

#endif

        static ReactiveMemory.Meta.MetaDatabase metaTable;

        public static object GetTable(MemoryDatabase db, string tableName)
        {
            switch (tableName)
            {
                case "fail":
                    return db.FailTable;
                case "item_master":
                    return db.ItemMasterTable;
                case "item_master_empty":
                    return db.ItemMasterEmptyValidateTable;
                case "people":
                    return db.PersonModelTable;
                case "quest_master":
                    return db.QuestMasterTable;
                case "quest_master_empty":
                    return db.QuestMasterEmptyValidateTable;
                case "s_a_m_p_l_e":
                    return db.SampleTable;
                case "sequantial_master":
                    return db.SequentialCheckMasterTable;
                case "single_master":
                    return db.SingleMasterTable;
                case "skillmaster":
                    return db.SkillMasterTable;
                case "TestMaster":
                    return db.TestMasterTable;
                case "UserLevel":
                    return db.UserLevelTable;
                
                default:
                    return null;
            }
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static ReactiveMemory.Meta.MetaDatabase GetMetaDatabase()
        {
            if (metaTable != null) return metaTable;

            var dict = new Dictionary<string, ReactiveMemory.Meta.MetaTable>();
            dict.Add("fail", ReactiveMemory.Test.Tables.FailTable.CreateMetaTable());
            dict.Add("item_master", ReactiveMemory.Test.Tables.ItemMasterTable.CreateMetaTable());
            dict.Add("item_master_empty", ReactiveMemory.Test.Tables.ItemMasterEmptyValidateTable.CreateMetaTable());
            dict.Add("people", ReactiveMemory.Test.Tables.PersonModelTable.CreateMetaTable());
            dict.Add("quest_master", ReactiveMemory.Test.Tables.QuestMasterTable.CreateMetaTable());
            dict.Add("quest_master_empty", ReactiveMemory.Test.Tables.QuestMasterEmptyValidateTable.CreateMetaTable());
            dict.Add("s_a_m_p_l_e", ReactiveMemory.Test.Tables.SampleTable.CreateMetaTable());
            dict.Add("sequantial_master", ReactiveMemory.Test.Tables.SequentialCheckMasterTable.CreateMetaTable());
            dict.Add("single_master", ReactiveMemory.Test.Tables.SingleMasterTable.CreateMetaTable());
            dict.Add("skillmaster", ReactiveMemory.Test.Tables.SkillMasterTable.CreateMetaTable());
            dict.Add("TestMaster", ReactiveMemory.Test.Tables.TestMasterTable.CreateMetaTable());
            dict.Add("UserLevel", ReactiveMemory.Test.Tables.UserLevelTable.CreateMetaTable());

            metaTable = new ReactiveMemory.Meta.MetaDatabase(dict);
            return metaTable;
        }

#endif
    }
}