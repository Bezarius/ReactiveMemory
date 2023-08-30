// <auto-generated />
#pragma warning disable CS0105
using MessagePack;
using ReactiveMemory.Tests;
using ReactiveMemory.Validation;
using ReactiveMemory;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace ReactiveMemory.Tests.Tables
{
   public sealed partial class SampleTable : TableBase<Sample>, ITableUniqueValidate
   {
        public Func<Sample, int> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<Sample, int> primaryIndexSelector;

        private Sample[] secondaryIndex1;
        private Func<Sample, (int Id, int Age, string FirstName, string LastName)> secondaryIndex1Selector;
        private Sample[] secondaryIndex2;
        private Func<Sample, (int Id, int Age)> secondaryIndex2Selector;
        private Sample[] secondaryIndex3;
        private Func<Sample, (int Id, int Age, string FirstName)> secondaryIndex3Selector;
        private Sample[] secondaryIndex5;
        private Func<Sample, int> secondaryIndex5Selector;
        private Sample[] secondaryIndex6;
        private Func<Sample, (string FirstName, int Age)> secondaryIndex6Selector;
        private Sample[] secondaryIndex0;
        private Func<Sample, (string FirstName, string LastName)> secondaryIndex0Selector;
        private Sample[] secondaryIndex4;
        private Func<Sample, string> secondaryIndex4Selector;

        public SampleTable(Sample[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.Id;
            var tasks = new List<Task>();
            tasks.Add(Task.Run(() =>
            {
                this.secondaryIndex1Selector = x => (x.Id, x.Age, x.FirstName, x.LastName);
                this.secondaryIndex1 = CloneAndSortBy(this.secondaryIndex1Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName, string LastName)>.Default);
            }));
            tasks.Add(Task.Run(() =>
            {
                this.secondaryIndex2Selector = x => (x.Id, x.Age);
                this.secondaryIndex2 = CloneAndSortBy(this.secondaryIndex2Selector, System.Collections.Generic.Comparer<(int Id, int Age)>.Default);
            }));
            tasks.Add(Task.Run(() =>
            {
                this.secondaryIndex3Selector = x => (x.Id, x.Age, x.FirstName);
                this.secondaryIndex3 = CloneAndSortBy(this.secondaryIndex3Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName)>.Default);
            }));
            tasks.Add(Task.Run(() =>
            {
                this.secondaryIndex5Selector = x => x.Age;
                this.secondaryIndex5 = CloneAndSortBy(this.secondaryIndex5Selector, System.Collections.Generic.Comparer<int>.Default);
            }));
            tasks.Add(Task.Run(() =>
            {
                this.secondaryIndex6Selector = x => (x.FirstName, x.Age);
                this.secondaryIndex6 = CloneAndSortBy(this.secondaryIndex6Selector, System.Collections.Generic.Comparer<(string FirstName, int Age)>.Default);
            }));
            tasks.Add(Task.Run(() =>
            {
                this.secondaryIndex0Selector = x => (x.FirstName, x.LastName);
                this.secondaryIndex0 = CloneAndSortBy(this.secondaryIndex0Selector, System.Collections.Generic.Comparer<(string FirstName, string LastName)>.Default);
            }));
            tasks.Add(Task.Run(() =>
            {
                this.secondaryIndex4Selector = x => x.FirstName;
                this.secondaryIndex4 = CloneAndSortBy(this.secondaryIndex4Selector, System.StringComparer.Ordinal);
            }));
            Task.WhenAll(tasks).Wait();
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();

        public RangeView<Sample> SortByIdAndAgeAndFirstNameAndLastName => new RangeView<Sample>(secondaryIndex1, 0, secondaryIndex1.Length - 1, true);
        public RangeView<Sample> SortByIdAndAge => new RangeView<Sample>(secondaryIndex2, 0, secondaryIndex2.Length - 1, true);
        public RangeView<Sample> SortByIdAndAgeAndFirstName => new RangeView<Sample>(secondaryIndex3, 0, secondaryIndex3.Length - 1, true);
        public RangeView<Sample> SortByAge => new RangeView<Sample>(secondaryIndex5, 0, secondaryIndex5.Length - 1, true);
        public RangeView<Sample> SortByFirstNameAndAge => new RangeView<Sample>(secondaryIndex6, 0, secondaryIndex6.Length - 1, true);
        public RangeView<Sample> SortByFirstNameAndLastName => new RangeView<Sample>(secondaryIndex0, 0, secondaryIndex0.Length - 1, true);
        public RangeView<Sample> SortByFirstName => new RangeView<Sample>(secondaryIndex4, 0, secondaryIndex4.Length - 1, true);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Sample FindById(int key)
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
        public bool TryFindById(int key, out Sample result)
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

        public Sample FindClosestById(int key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, key, selectLower);
        }

        public RangeView<Sample> FindRangeById(int min, int max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.Collections.Generic.Comparer<int>.Default, min, max, ascendant);
        }

        public Sample FindByIdAndAgeAndFirstNameAndLastName((int Id, int Age, string FirstName, string LastName) key)
        {
            return FindUniqueCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName, string LastName)>.Default, key, true);
        }
        
        public bool TryFindByIdAndAgeAndFirstNameAndLastName((int Id, int Age, string FirstName, string LastName) key, out Sample result)
        {
            return TryFindUniqueCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName, string LastName)>.Default, key, out result);
        }

        public Sample FindClosestByIdAndAgeAndFirstNameAndLastName((int Id, int Age, string FirstName, string LastName) key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName, string LastName)>.Default, key, selectLower);
        }

        public RangeView<Sample> FindRangeByIdAndAgeAndFirstNameAndLastName((int Id, int Age, string FirstName, string LastName) min, (int Id, int Age, string FirstName, string LastName) max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName, string LastName)>.Default, min, max, ascendant);
        }

        public Sample FindByIdAndAge((int Id, int Age) key)
        {
            return FindUniqueCore(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<(int Id, int Age)>.Default, key, true);
        }
        
        public bool TryFindByIdAndAge((int Id, int Age) key, out Sample result)
        {
            return TryFindUniqueCore(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<(int Id, int Age)>.Default, key, out result);
        }

        public Sample FindClosestByIdAndAge((int Id, int Age) key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<(int Id, int Age)>.Default, key, selectLower);
        }

        public RangeView<Sample> FindRangeByIdAndAge((int Id, int Age) min, (int Id, int Age) max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex2, secondaryIndex2Selector, System.Collections.Generic.Comparer<(int Id, int Age)>.Default, min, max, ascendant);
        }

        public Sample FindByIdAndAgeAndFirstName((int Id, int Age, string FirstName) key)
        {
            return FindUniqueCore(secondaryIndex3, secondaryIndex3Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName)>.Default, key, true);
        }
        
        public bool TryFindByIdAndAgeAndFirstName((int Id, int Age, string FirstName) key, out Sample result)
        {
            return TryFindUniqueCore(secondaryIndex3, secondaryIndex3Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName)>.Default, key, out result);
        }

        public Sample FindClosestByIdAndAgeAndFirstName((int Id, int Age, string FirstName) key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex3, secondaryIndex3Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName)>.Default, key, selectLower);
        }

        public RangeView<Sample> FindRangeByIdAndAgeAndFirstName((int Id, int Age, string FirstName) min, (int Id, int Age, string FirstName) max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex3, secondaryIndex3Selector, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName)>.Default, min, max, ascendant);
        }

        public RangeView<Sample> FindByAge(int key)
        {
            return FindManyCore(secondaryIndex5, secondaryIndex5Selector, System.Collections.Generic.Comparer<int>.Default, key);
        }

        public RangeView<Sample> FindClosestByAge(int key, bool selectLower = true)
        {
            return FindManyClosestCore(secondaryIndex5, secondaryIndex5Selector, System.Collections.Generic.Comparer<int>.Default, key, selectLower);
        }

        public RangeView<Sample> FindRangeByAge(int min, int max, bool ascendant = true)
        {
            return FindManyRangeCore(secondaryIndex5, secondaryIndex5Selector, System.Collections.Generic.Comparer<int>.Default, min, max, ascendant);
        }

        public RangeView<Sample> FindByFirstNameAndAge((string FirstName, int Age) key)
        {
            return FindManyCore(secondaryIndex6, secondaryIndex6Selector, System.Collections.Generic.Comparer<(string FirstName, int Age)>.Default, key);
        }

        public RangeView<Sample> FindClosestByFirstNameAndAge((string FirstName, int Age) key, bool selectLower = true)
        {
            return FindManyClosestCore(secondaryIndex6, secondaryIndex6Selector, System.Collections.Generic.Comparer<(string FirstName, int Age)>.Default, key, selectLower);
        }

        public RangeView<Sample> FindRangeByFirstNameAndAge((string FirstName, int Age) min, (string FirstName, int Age) max, bool ascendant = true)
        {
            return FindManyRangeCore(secondaryIndex6, secondaryIndex6Selector, System.Collections.Generic.Comparer<(string FirstName, int Age)>.Default, min, max, ascendant);
        }

        public Sample FindByFirstNameAndLastName((string FirstName, string LastName) key)
        {
            return FindUniqueCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<(string FirstName, string LastName)>.Default, key, true);
        }
        
        public bool TryFindByFirstNameAndLastName((string FirstName, string LastName) key, out Sample result)
        {
            return TryFindUniqueCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<(string FirstName, string LastName)>.Default, key, out result);
        }

        public Sample FindClosestByFirstNameAndLastName((string FirstName, string LastName) key, bool selectLower = true)
        {
            return FindUniqueClosestCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<(string FirstName, string LastName)>.Default, key, selectLower);
        }

        public RangeView<Sample> FindRangeByFirstNameAndLastName((string FirstName, string LastName) min, (string FirstName, string LastName) max, bool ascendant = true)
        {
            return FindUniqueRangeCore(secondaryIndex0, secondaryIndex0Selector, System.Collections.Generic.Comparer<(string FirstName, string LastName)>.Default, min, max, ascendant);
        }

        public RangeView<Sample> FindByFirstName(string key)
        {
            return FindManyCore(secondaryIndex4, secondaryIndex4Selector, System.StringComparer.Ordinal, key);
        }

        public RangeView<Sample> FindClosestByFirstName(string key, bool selectLower = true)
        {
            return FindManyClosestCore(secondaryIndex4, secondaryIndex4Selector, System.StringComparer.Ordinal, key, selectLower);
        }

        public RangeView<Sample> FindRangeByFirstName(string min, string max, bool ascendant = true)
        {
            return FindManyRangeCore(secondaryIndex4, secondaryIndex4Selector, System.StringComparer.Ordinal, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
#if !DISABLE_MASTERMEMORY_VALIDATOR

            ValidateUniqueCore(data, primaryIndexSelector, "Id", resultSet);       
            ValidateUniqueCore(secondaryIndex1, secondaryIndex1Selector, "(Id, Age, FirstName, LastName)", resultSet);       
            ValidateUniqueCore(secondaryIndex2, secondaryIndex2Selector, "(Id, Age)", resultSet);       
            ValidateUniqueCore(secondaryIndex3, secondaryIndex3Selector, "(Id, Age, FirstName)", resultSet);       
            ValidateUniqueCore(secondaryIndex0, secondaryIndex0Selector, "(FirstName, LastName)", resultSet);       

#endif
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static ReactiveMemory.Meta.MetaTable CreateMetaTable()
        {
            return new ReactiveMemory.Meta.MetaTable(typeof(Sample), typeof(SampleTable), "s_a_m_p_l_e",
                new ReactiveMemory.Meta.MetaProperty[]
                {
                    new ReactiveMemory.Meta.MetaProperty(typeof(Sample).GetProperty("Id")),
                    new ReactiveMemory.Meta.MetaProperty(typeof(Sample).GetProperty("Age")),
                    new ReactiveMemory.Meta.MetaProperty(typeof(Sample).GetProperty("FirstName")),
                    new ReactiveMemory.Meta.MetaProperty(typeof(Sample).GetProperty("LastName")),
                },
                new ReactiveMemory.Meta.MetaIndex[]{
                    new ReactiveMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Sample).GetProperty("Id"),
                    }, true, true, System.Collections.Generic.Comparer<int>.Default),
                    new ReactiveMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Sample).GetProperty("Id"),
                        typeof(Sample).GetProperty("Age"),
                        typeof(Sample).GetProperty("FirstName"),
                        typeof(Sample).GetProperty("LastName"),
                    }, false, true, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName, string LastName)>.Default),
                    new ReactiveMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Sample).GetProperty("Id"),
                        typeof(Sample).GetProperty("Age"),
                    }, false, true, System.Collections.Generic.Comparer<(int Id, int Age)>.Default),
                    new ReactiveMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Sample).GetProperty("Id"),
                        typeof(Sample).GetProperty("Age"),
                        typeof(Sample).GetProperty("FirstName"),
                    }, false, true, System.Collections.Generic.Comparer<(int Id, int Age, string FirstName)>.Default),
                    new ReactiveMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Sample).GetProperty("Age"),
                    }, false, false, System.Collections.Generic.Comparer<int>.Default),
                    new ReactiveMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Sample).GetProperty("FirstName"),
                        typeof(Sample).GetProperty("Age"),
                    }, false, false, System.Collections.Generic.Comparer<(string FirstName, int Age)>.Default),
                    new ReactiveMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Sample).GetProperty("FirstName"),
                        typeof(Sample).GetProperty("LastName"),
                    }, false, true, System.Collections.Generic.Comparer<(string FirstName, string LastName)>.Default),
                    new ReactiveMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(Sample).GetProperty("FirstName"),
                    }, false, false, System.StringComparer.Ordinal),
                });
        }

#endif
    }
}