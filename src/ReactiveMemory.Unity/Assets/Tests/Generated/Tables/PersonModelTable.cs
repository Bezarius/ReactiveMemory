// <auto-generated />
#pragma warning disable CS0105
using ReactiveMemory.Tests.TestStructures;
using ReactiveMemory.Validation;
using ReactiveMemory;
using MessagePack;
using System.Collections.Generic;
using System.Text;
using System;

namespace ReactiveMemory.Tests.Tables
{
   public sealed partial class PersonModelTable : TableBase<PersonModel>, ITableUniqueValidate
   {
        public Func<PersonModel, string> PrimaryKeySelector => primaryIndexSelector;
        readonly Func<PersonModel, string> primaryIndexSelector;

        readonly PersonModel[] secondaryIndex0;
        readonly Func<PersonModel, string> secondaryIndex0Selector;
        readonly PersonModel[] secondaryIndex1;
        readonly Func<PersonModel, (string FirstName, string LastName)> secondaryIndex1Selector;
        readonly PersonModel[] secondaryIndex2;
        readonly Func<PersonModel, string> secondaryIndex2Selector;

        public PersonModelTable(PersonModel[] sortedData)
            : base(sortedData)
        {
            this.primaryIndexSelector = x => x.RandomId;
            this.secondaryIndex0Selector = x => x.LastName;
            this.secondaryIndex0 = CloneAndSortBy(this.secondaryIndex0Selector, System.StringComparer.Ordinal);
            this.secondaryIndex1Selector = x => (x.FirstName, x.LastName);
            this.secondaryIndex1 = CloneAndSortBy(this.secondaryIndex1Selector, System.Collections.Generic.Comparer<(string FirstName, string LastName)>.Default);
            this.secondaryIndex2Selector = x => x.FirstName;
            this.secondaryIndex2 = CloneAndSortBy(this.secondaryIndex2Selector, System.StringComparer.Ordinal);
            OnAfterConstruct();
        }

        partial void OnAfterConstruct();

        public RangeView<PersonModel> SortByLastName => new RangeView<PersonModel>(secondaryIndex0, 0, secondaryIndex0.Length - 1, true);
        public RangeView<PersonModel> SortByFirstNameAndLastName => new RangeView<PersonModel>(secondaryIndex1, 0, secondaryIndex1.Length - 1, true);
        public RangeView<PersonModel> SortByFirstName => new RangeView<PersonModel>(secondaryIndex2, 0, secondaryIndex2.Length - 1, true);

        public PersonModel FindByRandomId(string key)
        {
            return FindUniqueCore(data, primaryIndexSelector, System.StringComparer.Ordinal, key, true);
        }
        
        public bool TryFindByRandomId(string key, out PersonModel result)
        {
            return TryFindUniqueCore(data, primaryIndexSelector, System.StringComparer.Ordinal, key, out result);
        }

        public PersonModel FindClosestByRandomId(string key, bool selectLower = true)
        {
            return FindUniqueClosestCore(data, primaryIndexSelector, System.StringComparer.Ordinal, key, selectLower);
        }

        public RangeView<PersonModel> FindRangeByRandomId(string min, string max, bool ascendant = true)
        {
            return FindUniqueRangeCore(data, primaryIndexSelector, System.StringComparer.Ordinal, min, max, ascendant);
        }

        public RangeView<PersonModel> FindByLastName(string key)
        {
            return FindManyCore(secondaryIndex0, secondaryIndex0Selector, System.StringComparer.Ordinal, key);
        }

        public RangeView<PersonModel> FindClosestByLastName(string key, bool selectLower = true)
        {
            return FindManyClosestCore(secondaryIndex0, secondaryIndex0Selector, System.StringComparer.Ordinal, key, selectLower);
        }

        public RangeView<PersonModel> FindRangeByLastName(string min, string max, bool ascendant = true)
        {
            return FindManyRangeCore(secondaryIndex0, secondaryIndex0Selector, System.StringComparer.Ordinal, min, max, ascendant);
        }

        public RangeView<PersonModel> FindByFirstNameAndLastName((string FirstName, string LastName) key)
        {
            return FindManyCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<(string FirstName, string LastName)>.Default, key);
        }

        public RangeView<PersonModel> FindClosestByFirstNameAndLastName((string FirstName, string LastName) key, bool selectLower = true)
        {
            return FindManyClosestCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<(string FirstName, string LastName)>.Default, key, selectLower);
        }

        public RangeView<PersonModel> FindRangeByFirstNameAndLastName((string FirstName, string LastName) min, (string FirstName, string LastName) max, bool ascendant = true)
        {
            return FindManyRangeCore(secondaryIndex1, secondaryIndex1Selector, System.Collections.Generic.Comparer<(string FirstName, string LastName)>.Default, min, max, ascendant);
        }

        public RangeView<PersonModel> FindByFirstName(string key)
        {
            return FindManyCore(secondaryIndex2, secondaryIndex2Selector, System.StringComparer.Ordinal, key);
        }

        public RangeView<PersonModel> FindClosestByFirstName(string key, bool selectLower = true)
        {
            return FindManyClosestCore(secondaryIndex2, secondaryIndex2Selector, System.StringComparer.Ordinal, key, selectLower);
        }

        public RangeView<PersonModel> FindRangeByFirstName(string min, string max, bool ascendant = true)
        {
            return FindManyRangeCore(secondaryIndex2, secondaryIndex2Selector, System.StringComparer.Ordinal, min, max, ascendant);
        }


        void ITableUniqueValidate.ValidateUnique(ValidateResult resultSet)
        {
#if !DISABLE_MASTERMEMORY_VALIDATOR

            ValidateUniqueCore(data, primaryIndexSelector, "RandomId", resultSet);       

#endif
        }

#if !DISABLE_MASTERMEMORY_METADATABASE

        public static ReactiveMemory.Meta.MetaTable CreateMetaTable()
        {
            return new ReactiveMemory.Meta.MetaTable(typeof(PersonModel), typeof(PersonModelTable), "people",
                new ReactiveMemory.Meta.MetaProperty[]
                {
                    new ReactiveMemory.Meta.MetaProperty(typeof(PersonModel).GetProperty("LastName")),
                    new ReactiveMemory.Meta.MetaProperty(typeof(PersonModel).GetProperty("FirstName")),
                    new ReactiveMemory.Meta.MetaProperty(typeof(PersonModel).GetProperty("RandomId")),
                },
                new ReactiveMemory.Meta.MetaIndex[]{
                    new ReactiveMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(PersonModel).GetProperty("RandomId"),
                    }, true, true, System.StringComparer.Ordinal),
                    new ReactiveMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(PersonModel).GetProperty("LastName"),
                    }, false, false, System.StringComparer.Ordinal),
                    new ReactiveMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(PersonModel).GetProperty("FirstName"),
                        typeof(PersonModel).GetProperty("LastName"),
                    }, false, false, System.Collections.Generic.Comparer<(string FirstName, string LastName)>.Default),
                    new ReactiveMemory.Meta.MetaIndex(new System.Reflection.PropertyInfo[] {
                        typeof(PersonModel).GetProperty("FirstName"),
                    }, false, false, System.StringComparer.Ordinal),
                });
        }

#endif
    }
}