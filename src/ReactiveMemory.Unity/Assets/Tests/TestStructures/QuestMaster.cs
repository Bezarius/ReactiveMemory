using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveMemory.Tests.TestStructures
{
    [MemoryTable("quest_master"), MessagePackObject(true)]
    public class QuestMaster : IValidatable<QuestMaster>
    {
        [PrimaryKey]
        public int QuestId { get; set; }
        public string Name { get; set; }
        public int RewardItemId { get; set; }
        public int Cost { get; set; }

        public void Validate(IValidator<QuestMaster> validator)
        {
            var itemMaster = validator.GetReferenceSet<ItemMaster>();

            itemMaster.Exists(x => x.RewardItemId, x => x.ItemId);

            validator.Validate(x => x.Cost <= 100);
            validator.Validate(x => x.Cost >= 0, ">= 0!!!");

            validator.ValidateAction(() => this.Cost <= 1000);
            validator.ValidateAction(() => this.Cost >= -90, ">= -90!!!");

            if (validator.CallOnce())
            {
                var quests = validator.GetTableSet();
                quests.Unique(x => x.Name);
            }
        }

        public QuestMaster(int QuestId, string Name, int RewardItemId, int Cost)
        {
            this.QuestId = QuestId;
            this.Name = Name;
            this.RewardItemId = RewardItemId;
            this.Cost = Cost;
        }
    }

    [MemoryTable("item_master"), MessagePackObject(true)]
    public class ItemMaster : IValidatable<ItemMaster>
    {
        [PrimaryKey]
        public int ItemId { get; set; }

        public void Validate(IValidator<ItemMaster> validator)
        {
        }

        public ItemMaster(int ItemId)
        {
            this.ItemId = ItemId;
        }
    }

    [MemoryTable("quest_master_empty"), MessagePackObject(true)]
    public class QuestMasterEmptyValidate
    {
        [PrimaryKey]
        public int QuestId { get; set; }
        public string Name { get; set; }
        public int RewardItemId { get; set; }
        public int Cost { get; set; }

        public QuestMasterEmptyValidate(int QuestId, string Name, int RewardItemId, int Cost)
        {
            this.QuestId = QuestId;
            this.Name = Name;
            this.RewardItemId = RewardItemId;
            this.Cost = Cost;
        }
    }

    [MemoryTable("item_master_empty"), MessagePackObject(true)]
    public class ItemMasterEmptyValidate
    {
        [PrimaryKey]
        public int ItemId { get; set; }

        public ItemMasterEmptyValidate(int ItemId)
        {
            this.ItemId = ItemId;
        }
    }

    [MemoryTable("sequantial_master"), MessagePackObject(true)]
    public class SequentialCheckMaster : IValidatable<SequentialCheckMaster>
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int Cost { get; set; }

        public void Validate(IValidator<SequentialCheckMaster> validator)
        {
            if (validator.CallOnce())
            {
                var set = validator.GetTableSet();

                set.Sequential(x => x.Id);
                set.Sequential(x => x.Cost, true);
            }
        }

        public SequentialCheckMaster(int Id, int Cost)
        {
            this.Id = Id;
            this.Cost = Cost;
        }
    }

    [MemoryTable("single_master"), MessagePackObject(true)]
    public class SingleMaster : IValidatable<SingleMaster>
    {
        public static int CalledValidateCount;
        public static int CalledOnceCount;

        [PrimaryKey]
        public int Id { get; set; }

        public void Validate(IValidator<SingleMaster> validator)
        {
            CalledValidateCount++;
            if (validator.CallOnce())
            {
                CalledOnceCount++;
            }
        }

        public SingleMaster(int Id)
        {
            this.Id = Id;
        }
    }

    [MemoryTable("fail"), MessagePackObject(true)]
    public class Fail : IValidatable<Fail>
    {
        [PrimaryKey]
        public int Id { get; set; }

        public void Validate(IValidator<Fail> validator)
        {
            validator.Fail("Failed Id:" + Id);
        }

        public Fail(int Id)
        {
            this.Id = Id;
        }
    }
}
