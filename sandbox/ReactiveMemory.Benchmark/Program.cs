using System.Numerics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Security.Cryptography;
using ReactiveMemory.Benchmark.DataAccess.Models;
using System.Diagnostics;
using System;

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

        public void OnCompleted() {  }

        public void OnError(Exception error){ }

        public void OnNext(EntityChange<T> value) {  }

        public IDisposable Subscribe(IObserver<EntityChange<T>> observer)
        {
            return null;
        }
    }

    public class ReactiveMemoryOpBench
    {

        const int PersonsCount = 10000;

        private static Random Random = new Random();


        public static Person[] GenerateRandomPersons(int N)
        {
            Random random = new Random();
            Person[] persons = new Person[N];

            for (int i = 0; i < N; i++)
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
        private DbContext _ctx2;

        [DebuggerNonUserCode]
        public ReactiveMemoryOpBench()
        {
            var builder = new DatabaseBuilder();
            builder.Append(GenerateRandomPersons(PersonsCount));
            _ctx = new DbContext(builder.Build(), null);
            _ctx.BeginTransaction();
            _ctx.Commit();
            //_ctx2 = new DbContext(builder.Build(), null, "SHA256");
        }

        
        [Benchmark]
        public void TestFind()
        {
            Random random = new Random();

            _ctx.BeginTransaction();
            var t = _ctx.Transaction;
            for (int i = 0; i < PersonsCount / 10; i++)
            {
                int index = random.Next(1, PersonsCount);
                var person = _ctx.Database.PersonTable.FindByPersonId(index);
            }
            _ctx.Commit();
        }

        [Benchmark]
        public void TestFindAndUpdate()
        {
            Random random = new Random();
            
            _ctx.BeginTransaction();
            var t = _ctx.Transaction;
            for(int i = 0; i < PersonsCount / 10; i++)
            {
                int index = random.Next(1, PersonsCount);
                var person = _ctx.Database.PersonTable.FindByPersonId(index);
                if (person != null)
                {
                    person = person with
                    {
                        Age = random.Next(1, 100)
                    };
                    t.Diff(person);
                }
            }
            _ctx.Commit();
        }

        [Benchmark]
        public void TestRandomRemove()
        {
            Random random = new Random();

            _ctx.BeginTransaction();
            var t = _ctx.Transaction;
            var set = new HashSet<int>();
            for (int i = 0; i < PersonsCount / 10; i++)
            {
                int index = random.Next(1, PersonsCount);
                if (!set.Contains(index))
                {
                    set.Add(index);
                    t.RemovePerson(index);
                }
            }
            _ctx.Commit();
        }

        /*
        [Benchmark]
        public void TestFindAndDiff2()
        {
            Random random = new Random();

            _ctx.BeginTransaction();
            var t = _ctx.Transaction;
            for (int i = 0; i < PersonsCount / 10; i++)
            {
                int index = random.Next(1, PersonsCount);
                var person = _ctx.Database.PersonTable.FindByPersonId(index);
                if (person != null)
                {
                    person = person with
                    {
                        Age = random.Next(1, 100)
                    };
                    t.Diff(new[] { person });
                }
            }
            _ctx.Commit();
        }*/


        /*
        [Benchmark]
        public void TestFindAndDiffSha256()
        {
            Random random = new Random();

            _ctx2.BeginTransaction();
            var t = _ctx2.Transaction;
            for (int i = 0; i < PersonsCount / 10; i++)
            {
                int index = random.Next(1, PersonsCount);
                var person = _ctx2.Database.PersonTable.FindByPersonId(index);
                person = person with
                {
                    Age = random.Next(1, 100)
                };
                t.Diff(person);
            }
        }*/
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<ReactiveMemoryOpBench>();
            //var test = new ReactiveMemoryOpBench();
            //test.TestRandomRemove();
        }
    }
}