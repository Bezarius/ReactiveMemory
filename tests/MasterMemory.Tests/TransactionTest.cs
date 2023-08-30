using FluentAssertions;
using MessagePack;
using ReactiveMemory.Tests.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ReactiveMemory.Tests
{
    public class TransactionTest
    {
        public TransactionTest()
        {
            MessagePackSerializer.DefaultOptions = MessagePackSerializer.DefaultOptions.WithResolver(MessagePackResolver.Instance);
        }

        Sample[] CreateData()
        {
            // Id = Unique, PK
            // FirstName + LastName = Unique
            var data = new[]
            {
                new Sample { Id = 5, Age = 19, FirstName = "aaa", LastName = "foo" },
                new Sample { Id = 6, Age = 29, FirstName = "bbb", LastName = "foo" },
                new Sample { Id = 7, Age = 39, FirstName = "ccc", LastName = "foo" },
                new Sample { Id = 8, Age = 49, FirstName = "ddd", LastName = "foo" },
                new Sample { Id = 1, Age = 59, FirstName = "eee", LastName = "foo" },
                new Sample { Id = 2, Age = 89, FirstName = "aaa", LastName = "bar" },
                new Sample { Id = 3, Age = 79, FirstName = "be", LastName = "de" },
                new Sample { Id = 4, Age = 89, FirstName = "aaa", LastName = "tako" },
                new Sample { Id = 9, Age = 99, FirstName = "aaa", LastName = "ika" },
                new Sample { Id = 10, Age = 9, FirstName = "eee", LastName = "baz" },
            };
            return data;
        }

        SampleTable CreateTable(Sample[] data)
        {
            return new MemoryDatabase(new DatabaseBuilder().Append(data).Build()).SampleTable;
        }

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

        public DbContext GetContext()
        {
            var builder = new DatabaseBuilder();
            builder.Append(CreateData());

            var bin = builder.Build();
            var ctx = new DbContext(bin, new ChangesMediatorFactory());
            return ctx;
        }

        [Fact]
        public void TestDelete()
        {
            var ctx = GetContext();
            var t = ctx.BeginTransaction();
            t.RemoveSample(9);
            t.RemoveSample(10);
            ctx.Commit();

            ctx.Database.SampleTable.All.Count.Should().Be(8);
            ctx.Database.SampleTable.All.Where(x=> x.Id > 8).Count().Should().Be(0);
        }

        [Fact]
        public void TestUpdate()
        {
            var ctx = GetContext();
            var sample = ctx.Database.SampleTable.FindById(9);
            var t = ctx.BeginTransaction();
            t.Diff(sample with
            {
                Age = 98
            });
            ctx.Commit();
            ctx.Database.SampleTable.FindById(9).Age.Should().Be(98);
        }

        [Fact]
        public void TestAccessInsideTransaction()
        {
            var ctx = GetContext();
            var t = ctx.BeginTransaction();
            t.Diff(new Sample
            {
                Id = 99,
                Age = 99,
                FirstName = "99",
                LastName = "99"
            });
            ctx.Database.SampleTable.TryFindById(99, out var sample);
            sample.Should().NotBeNull();
            ctx.Commit();
            ctx.Database.SampleTable.TryFindById(99, out var sample2);
            sample2.Should().NotBeNull();
        }
    }
}
