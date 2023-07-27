// <auto-generated />
#pragma warning disable CS0105
using MessagePack;
using ReactiveMemory.Tests.TestStructures;
using ReactiveMemory.Validation;
using ReactiveMemory;
using System.Collections.Generic;
using System.Text;
using System;

namespace ReactiveMemory.Test.Tables
{
   public sealed partial class FailTable : TableBase<Fail>, ITableUniqueValidate
   {
        public Func<Fail, int> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<Fail, int> primaryIndexSelector;


        public FailTable(Fail[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.Id;
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Fail FindById(int key)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].Id;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { return data[mid]; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            return ThrowKeyNotFound(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool TryFindById(int key, out Fail result)
        {
            var lo = 0;
            var hi = data.Length - 1;
            while (lo <= hi)
            {
                var mid = (int)(((uint)hi + (uint)lo) >> 1);
                var selected = data[mid].Id;
                var found = (selected < key) ? -1 : (selected > key) ? 1 : 0;
                if (found == 0) { result = data[mid]; return true; }
                if (found < 0) { lo = mid + 1; }
                else { hi = mid - 1; }
            }
            result = default;
            return false;
        }

        public Fail FindClosestById(int key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, key, selectLower);
        }

        public RangeView<Fail> FindRangeById(int min, int max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
#if !DISABLE_MASTERMEMORY_VALIDATOR

            ValidateUniqueCore(data, primaryIndexSelector, "Id", resultSet);       

#endif
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static ReactiveMemory.Meta.MetaTable CreateMetaTable()
        {
            return new ReactiveMemory.Meta.MetaTable(typeof(Fail), typeof(FailTable), "fail",
                new ReactiveMemory.Meta.MetaProperty[]
                {
                    new ReactiveMemory.Meta.MetaProperty(typeof(Fail).GetProperty("Id")),
                },
                new ReactiveMemory.Meta.MetaIndex[]{
                    new ReactiveMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Fail).GetProperty("Id"),
                    }, true, true, System.Collections.Generic.Comparer<int>.Default),
                });
        }

#endif
    }
}