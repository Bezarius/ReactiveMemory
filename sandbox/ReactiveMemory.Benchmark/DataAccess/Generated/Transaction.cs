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
        public void ReplaceAll(System.Collections.Generic.IList<PersonStruct> data);
        public void RemovePersonStruct(int key);
        public void RemovePersonStruct(int[] keys);
        public void Diff(PersonStruct addOrReplaceData);
        public void Diff(PersonStruct[] addOrReplaceData);
   }

   public sealed class Transaction : TransactionBase, ITransaction
   {

        public MemoryDatabase Database
        {
            get
            {
                if(_rebuildIsNeeded)
                {
                    Commit();
                }
                return memory;
            }
        }

        private MemoryDatabase memory;

        private IChangesQueue<Monster> _MonsterChangeTracker;
        private IChangesQueue<Person> _PersonChangeTracker;
        private IChangesQueue<PersonStruct> _PersonStructChangeTracker;
 

        private Monster[] _MonsterChanges;
        private Person[] _PersonChanges;
        private PersonStruct[] _PersonStructChanges;
 

        private bool _rebuildIsNeeded;

        public Transaction(MemoryDatabase memory)
        {
            this.memory = memory;
            _MonsterChangeTracker = this.memory.ChangesConveyor.GetQueue<Monster>();
            _PersonChangeTracker = this.memory.ChangesConveyor.GetQueue<Person>();
            _PersonStructChangeTracker = this.memory.ChangesConveyor.GetQueue<PersonStruct>();
 
        }

        public MemoryDatabase Commit()
        {
            if(!_rebuildIsNeeded)
            {
                return memory;
            }
            MonsterTable MonsterTable;
            if(_MonsterChanges != null)
            {
                MonsterTable = new MonsterTable(CloneAndSortBy(_MonsterChanges, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default));
                _MonsterChanges = null;
            }
            else
            {
                MonsterTable = memory.MonsterTable;
            }
            PersonTable PersonTable;
            if(_PersonChanges != null)
            {
                PersonTable = new PersonTable(CloneAndSortBy(_PersonChanges, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default));
                _PersonChanges = null;
            }
            else
            {
                PersonTable = memory.PersonTable;
            }
            PersonStructTable PersonStructTable;
            if(_PersonStructChanges != null)
            {
                PersonStructTable = new PersonStructTable(CloneAndSortBy(_PersonStructChanges, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default));
                _PersonStructChanges = null;
            }
            else
            {
                PersonStructTable = memory.PersonStructTable;
            }
 
            memory = new MemoryDatabase(
                MonsterTable,
                PersonTable,
                PersonStructTable,
 
                memory.ChangesConveyor             
            );
            _rebuildIsNeeded = false;
            return memory;
        }

        public void ReplaceAll(System.Collections.Generic.IList<Monster> data)
        {
            _MonsterChanges = CloneAndSortBy(data, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default);
            _rebuildIsNeeded = true;
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
            _rebuildIsNeeded = true;
        }


        public void RemoveMonster(int[] keys)
        {
            if(_MonsterChanges == null)
            {
                _MonsterChanges = RemoveCore(memory.MonsterTable.GetRawDataUnsafe(), keys, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default, _MonsterChangeTracker);
            }
            else
            {
                _MonsterChanges = RemoveCore(_MonsterChanges, keys, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default, _MonsterChangeTracker);
            }
            _rebuildIsNeeded = true;
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
            _rebuildIsNeeded = true;
        }

        public void Diff(Monster[] addOrReplaceData)
        {
            if(_MonsterChanges == null)
            {
                _MonsterChanges = DiffCore(memory.MonsterTable.GetRawDataUnsafe(), addOrReplaceData, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default, _MonsterChangeTracker, true);  
            }
            else
            {
                _MonsterChanges = DiffCore(_MonsterChanges, addOrReplaceData, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default, _MonsterChangeTracker, false);  
            }
            _rebuildIsNeeded = true;
        }

        public void ReplaceAll(System.Collections.Generic.IList<Person> data)
        {
            _PersonChanges = CloneAndSortBy(data, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default);
            _rebuildIsNeeded = true;
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
            _rebuildIsNeeded = true;
        }


        public void RemovePerson(int[] keys)
        {
            if(_PersonChanges == null)
            {
                _PersonChanges = RemoveCore(memory.PersonTable.GetRawDataUnsafe(), keys, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonChangeTracker);
            }
            else
            {
                _PersonChanges = RemoveCore(_PersonChanges, keys, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonChangeTracker);
            }
            _rebuildIsNeeded = true;
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
            _rebuildIsNeeded = true;
        }

        public void Diff(Person[] addOrReplaceData)
        {
            if(_PersonChanges == null)
            {
                _PersonChanges = DiffCore(memory.PersonTable.GetRawDataUnsafe(), addOrReplaceData, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonChangeTracker, true);  
            }
            else
            {
                _PersonChanges = DiffCore(_PersonChanges, addOrReplaceData, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonChangeTracker, false);  
            }
            _rebuildIsNeeded = true;
        }

        public void ReplaceAll(System.Collections.Generic.IList<PersonStruct> data)
        {
            _PersonStructChanges = CloneAndSortBy(data, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default);
            _rebuildIsNeeded = true;
        }

        
        public void RemovePersonStruct(int key)
        {
            if(_PersonStructChanges == null)
            {
                _PersonStructChanges = RemoveCore(memory.PersonStructTable.GetRawDataUnsafe(), key, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonStructChangeTracker);
            }
            else
            {
                _PersonStructChanges = RemoveCore(_PersonStructChanges, key, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonStructChangeTracker);
            }
            _rebuildIsNeeded = true;
        }


        public void RemovePersonStruct(int[] keys)
        {
            if(_PersonStructChanges == null)
            {
                _PersonStructChanges = RemoveCore(memory.PersonStructTable.GetRawDataUnsafe(), keys, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonStructChangeTracker);
            }
            else
            {
                _PersonStructChanges = RemoveCore(_PersonStructChanges, keys, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonStructChangeTracker);
            }
            _rebuildIsNeeded = true;
        }

        public void Diff(PersonStruct addOrReplaceData)
        {
            if(_PersonStructChanges == null)
            {
                _PersonStructChanges = DiffCore(memory.PersonStructTable.GetRawDataUnsafe(), addOrReplaceData, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonStructChangeTracker, true);
            }
            else
            {
                _PersonStructChanges = DiffCore(_PersonStructChanges, addOrReplaceData, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonStructChangeTracker, false);
            }
            _rebuildIsNeeded = true;
        }

        public void Diff(PersonStruct[] addOrReplaceData)
        {
            if(_PersonStructChanges == null)
            {
                _PersonStructChanges = DiffCore(memory.PersonStructTable.GetRawDataUnsafe(), addOrReplaceData, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonStructChangeTracker, true);  
            }
            else
            {
                _PersonStructChanges = DiffCore(_PersonStructChanges, addOrReplaceData, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default, _PersonStructChangeTracker, false);  
            }
            _rebuildIsNeeded = true;
        }

    }
}