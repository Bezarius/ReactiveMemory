// <auto-generated />
#pragma warning disable CS0105
using MessagePack;
using ReactiveMemory.Tests.TestStructures;
using ReactiveMemory.Validation;
using ReactiveMemory;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace ReactiveMemory.Tests.Tables
{
   public sealed partial class QuestMasterTable : TableBase<QuestMaster>, ITableUniqueValidate
   {
        public Func<QuestMaster, int> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<QuestMaster, int> primaryIndexSelector;


        public QuestMasterTable(QuestMaster[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.QuestId;
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public QuestMaster FindByQuestId(int key)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].QuestId;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { return data[mid]; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            return ThrowKeyNotFound(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryFindByQuestId(int key, out QuestMaster result)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].QuestId;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { result = data[mid]; return true; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            result = default;
            return false;
        }

        public QuestMaster FindClosestByQuestId(int key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, key, selectLower);
        }

        public RangeView<QuestMaster> FindRangeByQuestId(int min, int max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
#if !DISABLE_MASTERMEMORY_VALIDATOR

            ValidateUniqueCore(data, primaryIndexSelector, "QuestId", resultSet);       

#endif
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static ReactiveMemory.Meta.MetaTable CreateMetaTable()
        {
            return new ReactiveMemory.Meta.MetaTable(typeof(QuestMaster), typeof(QuestMasterTable), "quest_master",
                new ReactiveMemory.Meta.MetaProperty[]
                {
                    new ReactiveMemory.Meta.MetaProperty(typeof(QuestMaster).GetProperty("QuestId")),
                    new ReactiveMemory.Meta.MetaProperty(typeof(QuestMaster).GetProperty("Name")),
                    new ReactiveMemory.Meta.MetaProperty(typeof(QuestMaster).GetProperty("RewardItemId")),
                    new ReactiveMemory.Meta.MetaProperty(typeof(QuestMaster).GetProperty("Cost")),
                },
                new ReactiveMemory.Meta.MetaIndex[]{
                    new ReactiveMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(QuestMaster).GetProperty("QuestId"),
                    }, true, true, System.Collections.Generic.Comparer<int>.Default),
                });
        }

#endif
    }
}