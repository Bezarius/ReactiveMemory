using System.Numerics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Security.Cryptography;
using ReactiveMemory.Benchmark.DataAccess.Models;
using System.Diagnostics;
using System;
using System.Reflection;

namespace ReactiveMemory.Benchmark
{

    public class ChangesMediatorFactory : IChangesMediatorFactory
    {
        public IChangesMediator<TElement> Create<TElement>()
        {
            return new ObservableWrapper<TElement>();
        }
    }

    public class ObservableWrapper<T> : IChangesMediator<T>
    {

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(EntityChange<T> value) { }

        public IDisposable Subscribe(IObserver<EntityChange<T>> observer)
        {
            return null;
        }
    }

    public class ReactiveMemoryOpBench
    {

        [Params(10, 100, 1000)]
        public int PersonsCount;

        private static Random Random = new Random();


        public static Person[] GenerateRandomPersons(int count)
        {
            Random random = new Random();
            Person[] persons = new Person[count];

            for (int i = 0; i < count; i++)
            {
                Person person = new Person
                {
                    Age = random.Next(1, 100),
                    Gender = (Gender)random.Next(0, Enum.GetValues(typeof(Gender)).Length),
                    Name = GenerateRandomName(),
                    PersonId = i + 1
                };

                persons[i] = person;
            }

            return persons;
        }

        public static string GenerateRandomName()
        {
            string[] names = { "John", "Jane", "Michael", "Emily", "William", "Sophia", "Robert", "Olivia", "David", "Ava", "James", "Mia" };

            Random random = new Random();
            int index = random.Next(0, names.Length);
            return names[index];
        }

        private DbContext _ctx;
        private Dictionary<int, Person> _personsDict;
        private DbContext _ctx2;
        private int[] _ids;


        [GlobalSetup]
        public void GlobalSetup()
        {
            Console.WriteLine($"[GlobalSetup]");
            var builder = new DatabaseBuilder();
            var persons = GenerateRandomPersons(PersonsCount);
            builder.Append(persons);
            _ctx = new DbContext(builder.Build(), null);
            _ids = _ctx.Database.PersonTable.All
                .Select(x => x.PersonId)
                .ToArray()
                .Shuffle(Random)
                .Shuffle(Random);

            _personsDict = persons.ToDictionary(x => x.PersonId, x => x);
        }

        [Benchmark]
        public void TestTransactionOverhead()
        {
            _ctx.BeginTransaction();
            _ctx.Commit();
        }

        [Benchmark]
        public void TestFind()
        {
            Random random = new Random();
            for (int i = 0; i < PersonsCount; i++)
            {
                int index = random.Next(1, PersonsCount);
                var person = _ctx.Database.PersonTable.FindByPersonId(index);
            }
        }

        [Benchmark]
        public void TestDictFind()
        {
            Random random = new Random();
            for (int i = 0; i < PersonsCount; i++)
            {
                int index = random.Next(1, PersonsCount);
                var person = _personsDict[index];
            }
        }

        [Benchmark]
        public void TestFindAndUpdate()
        {
            var sourcePerson = new Person
            {
                Age = 100500,
                Gender = Gender.Male,
                Name = "Default",
                PersonId = 0
            };
            _ctx.BeginTransaction();
            var t = _ctx.Transaction;
            for (int i = 0; i < PersonsCount; i++)
            {
                t.Diff(sourcePerson with
                {
                    PersonId = i + 1
                });
            }
            _ctx.Commit();
        }

        [Benchmark]
        public void TestFindAndUpdate2()
        {
            var sourcePerson = new Person
            {
                Age = 100500,
                Gender = Gender.Male,
                Name = "Default",
                PersonId = 0
            };
            _ctx.BeginTransaction();
            var t = _ctx.Transaction;
            for (int i = 0; i < PersonsCount; i++)
            {
                t.Diff(new Person[] {sourcePerson with
                {
                    PersonId = i + 1
                } });
            }
            _ctx.Commit();
        }


        [Benchmark]
        public void TestFindAndUpdateStruct()
        {
            var sourcePerson = new PersonStruct
            {
                Age = 100500,
                Gender = Gender.Male,
                Name = "Default",
                PersonId = 0
            };
            _ctx.BeginTransaction();
            var t = _ctx.Transaction;
            for (int i = 0; i < PersonsCount; i++)
            {
                t.Diff(sourcePerson with
                {
                    PersonId = i + 1
                });
            }
            _ctx.Commit();
        }

        [Benchmark]
        public void TestFindAndUpdateWithRollback()
        {
            var sourcePerson = new Person
            {
                Age = 100500,
                Gender = Gender.Male,
                Name = "Default",
                PersonId = 0
            };
            _ctx.BeginTransaction();
            var t = _ctx.Transaction;
            for (int i = 0; i < PersonsCount; i++)
            {
                t.Diff(sourcePerson with
                {
                    PersonId = i + 1
                });
            }
            _ctx.Rollback();
        }


        [Benchmark]
        public void TestFindAndUpdateDict()
        {
            var sourcePerson = new Person
            {
                Age = 100500,
                Gender = Gender.Male,
                Name = "Default",
                PersonId = 0
            };
            for (int i = 0; i < PersonsCount; i++)
            {
                _personsDict[i] = sourcePerson with
                {
                    PersonId = i + 1
                };
            }
        }

        [Benchmark]
        public void TestRandomRemove()
        {
            _ctx.BeginTransaction();
            var t = _ctx.Transaction;
            for (int i = 0; i < PersonsCount; i++)
            {
                t.RemovePerson(_ids[i]);
            }
            _ctx.Commit();
        }

        [Benchmark]
        public void TestRandomRemoveDict()
        {
            for (int i = 0; i < PersonsCount; i++)
            {
                _personsDict.Remove(_ids[i]);
            }
        }

        [Benchmark]
        public void TestSeqRemove()
        {
            _ctx.BeginTransaction();
            var t = _ctx.Transaction;
            for (int i = 0; i < PersonsCount; i++)
            {
                t.RemovePerson(i);
            }
            _ctx.Commit();
        }

        [Benchmark]
        public void TestSeqRemoveDict()
        {
            for (int i = 0; i < PersonsCount; i++)
            {
                _personsDict.Remove(i);
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<ReactiveMemoryOpBench>();
            //var test = new ReactiveMemoryOpBench();
            //test.PersonsCount = 1000000;
            //test.GlobalSetup();
            //test.TestFindAndUpdate();
            //test.TestRandomRemove();
        }
    }

    public static class ArrayExtensions
    {
        public static T[] Shuffle<T>(this T[] array, Random rng)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
            return array;
        }
    }
}