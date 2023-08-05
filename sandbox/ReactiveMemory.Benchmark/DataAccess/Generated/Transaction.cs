// <auto-generated />
#pragma warning disable CS0105
using MessagePack;
using ReactiveMemory.Benchmark.DataAccess.Models;
using ReactiveMemory.Validation;
using ReactiveMemory;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using ReactiveMemory.Benchmark.Tables;

namespace ReactiveMemory.Benchmark
{
   public interface ITransaction
   {
        public void ReplaceAll(System.Collections.Generic.IList<Monster> data);
        public void RemoveMonster(int key);
        public void RemoveMonster(int[] keys);
        public void Diff(Monster addOrReplaceData);
        public void Diff(Monster[] addOrReplaceData);
        public void ReplaceAll(System.Collections.Generic.IList<Person> data);
        public void RemovePerson(int key);
        public void RemovePerson(int[] keys);
        public void Diff(Person addOrReplaceData);
        public void Diff(Person[] addOrReplaceData);
   }

   public sealed class Transaction : TransactionBase, ITransaction
   {
        MemoryDatabase memory;

        private IChangesQueue<Monster> _MonsterChangeTracker;
        private IChangesQueue<Person> _PersonChangeTracker;
 

        private Monster[] _MonsterChanges;
        private Person[] _PersonChanges;
 

        public Transaction(MemoryDatabase memory)
        {
            this.memory = memory;
            _MonsterChangeTracker = this.memory.ChangesConveyor.GetQueue<Monster>();
            _PersonChangeTracker = this.memory.ChangesConveyor.GetQueue<Person>();
 
        }

        public MemoryDatabase Commit()
        {
            MonsterTable MonsterTable;
            if(_MonsterChanges != null)
            {
                MonsterTable = new MonsterTable(CloneAndSortBy(_MonsterChanges, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default));
            }
            else
            {
                MonsterTable = memory.MonsterTable;
            }
            PersonTable PersonTable;
            if(_PersonChanges != null)
            {
                PersonTable = new PersonTable(CloneAndSortBy(_PersonChanges, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default));
            }
            else
            {
                PersonTable = memory.PersonTable;
            }
 
            memory = new MemoryDatabase(
                MonsterTable,
                PersonTable,
 
                memory.ChangesConveyor             
            );
            memory.ChangesConveyor.Publish();
            return memory;
        }

        public void ReplaceAll(System.Collections.Generic.IList<Monster> data)
        {
            var newData = CloneAndSortBy(data, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default);
            var table = new MonsterTable(newData);
            memory = new MemoryDatabase(
                table,
                memory.PersonTable,
 
                memory.ChangesConveyor            
            );
        }

        
        public void RemoveMonster(int key)
        {
            if(_MonsterChanges == null)
            {
                _MonsterChanges = RemoveCore(memory.MonsterTable.GetRawDataUnsafe(), key, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default, _MonsterChangeTracker);
            }
            else
            {
                _MonsterChanges = RemoveCore(_MonsterChanges, key, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default, _MonsterChangeTracker);
            }
        }


        public void RemoveMonster(int[] keys)
        {
            var data = RemoveCore(memory.MonsterTable.GetRawDataUnsafe(), keys, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default, _MonsterChangeTracker);
            var newData = CloneAndSortBy(data, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default);
            var table = new MonsterTable(newData);
            memory = new MemoryDatabase(
                table,
                memory.PersonTable,
 
                memory.ChangesConveyor             
            );
        }

        public void Diff(Monster addOrReplaceData)
        {
            if(_MonsterChanges == null)
            {
                _MonsterChanges = DiffCore(memory.MonsterTable.GetRawDataUnsafe(), addOrReplaceData, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default, _MonsterChangeTracker, true);
            }
            else
            {
                _MonsterChanges = DiffCore(_MonsterChanges, addOrReplaceData, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default, _MonsterChangeTracker, false);
            }
        }

        public void Diff(Monster[] addOrReplaceData)
        {
            if(_MonsterChanges == null)
            {
                _MonsterChanges = DiffCore(memory.MonsterTable.GetRawDataUnsafe(), addOrReplaceData, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default, _MonsterChangeTracker).ToArray();  
            }
            else
            {
                _MonsterChanges = DiffCore(_MonsterChanges, addOrReplaceData, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default, _MonsterChangeTracker).ToArray();  
            }
        }

        public void ReplaceAll(System.Collections.Generic.IList<Person> data)
        {
            var newData = CloneAndSortBy(data, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default);
            var table = new PersonTable(newData);
            memory = new MemoryDatabase(
                memory.MonsterTable,
                table,
 
                memory.ChangesConveyor            
            );
        }

        
        public void RemovePerson(int key)
        {
            if(_PersonChanges == null)
            {
                _PersonChanges = RemoveCore(memory.PersonTable.GetRawDataUnsafe(), key, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonChangeTracker);
            }
            else
            {
                _PersonChanges = RemoveCore(_PersonChanges, key, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonChangeTracker);
            }
        }


        public void RemovePerson(int[] keys)
        {
            var data = RemoveCore(memory.PersonTable.GetRawDataUnsafe(), keys, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonChangeTracker);
            var newData = CloneAndSortBy(data, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default);
            var table = new PersonTable(newData);
            memory = new MemoryDatabase(
                memory.MonsterTable,
                table,
 
                memory.ChangesConveyor             
            );
        }

        public void Diff(Person addOrReplaceData)
        {
            if(_PersonChanges == null)
            {
                _PersonChanges = DiffCore(memory.PersonTable.GetRawDataUnsafe(), addOrReplaceData, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonChangeTracker, true);
            }
            else
            {
                _PersonChanges = DiffCore(_PersonChanges, addOrReplaceData, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonChangeTracker, false);
            }
        }

        public void Diff(Person[] addOrReplaceData)
        {
            if(_PersonChanges == null)
            {
                _PersonChanges = DiffCore(memory.PersonTable.GetRawDataUnsafe(), addOrReplaceData, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonChangeTracker).ToArray();  
            }
            else
            {
                _PersonChanges = DiffCore(_PersonChanges, addOrReplaceData, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonChangeTracker).ToArray();  
            }
        }

    }
}