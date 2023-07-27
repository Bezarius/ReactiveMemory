using MessagePack;
using ReactiveMemory;
using ReactiveMemory.Tests;
using ReactiveMemory.Tests.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Analytics;
using UnityEngine.TestTools;
using Xunit;
using UniRx;
using FluentAssertions;

namespace ReactiveMemory.Tests
{
    /*
     *  var bytes = builder.Build();

            // build db from bytes
            var ctx = new DbContext(bytes, new ChangesMediatorFactory());
            // subscribe on any person change
            ctx.Database.OnChange<Person>().Subscribe( x=>
            {
                // do some logic on person change 
                ...
            });
            // search person by id
            var person = ctx.Database.PersonTable.FindByPersonId(1);
            var malePersonsOfAge25 = ctx.Database.PersonTable.FindByGenderAndAge((Gender.Male, 25));
            // start edit
            ctx.BeginTransaction();
            // remove person
            ctx.Transaction.RemovePerson(person.PersonId);
            // or update
            person.Age *= 2;
            ctx.Transaction.Diff(person);
            // save changes
            ctx.Commit(); // subscriptions will fire only on commit
            // or discard changes
            ctx.Rollback(); */


    public class DbContextTest
    {
        public DbContextTest()
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

        DbContext CreateDbContext(Sample[] data)
        {
            return new DbContext(new DatabaseBuilder().Append(data).Build(), changesMediatorFactory: UniRxSubjectFactory.Default);
        }


        [Fact]
        public void CommitTest()
        {
            var ctx = CreateDbContext(CreateData());
            var targetId = 5;
            bool flag = false;
            ctx.Database.OnChange<Sample>().Subscribe(x =>
            {
                if (x.Change == EEntityChangeType.Update && x.Entity.Id == 5) flag = true;
            });

            ctx.BeginTransaction();
            var p = ctx.Database.SampleTable.FindById(targetId);
            var p2 = new Sample { Id = targetId, Age = 24, FirstName = "aaa", LastName = "foo" };
            ctx.Transaction.Diff(p2);
            flag.Should().BeFalse();
            ctx.Commit();
            flag.Should().BeTrue();
        }

        [Fact]
        public void RollbackTest()
        {
            var ctx = CreateDbContext(CreateData());
            var targetId = 5;
            bool flag = false;
            ctx.Database.OnChange<Sample>().Subscribe(x =>
            {
                if (x.Change == EEntityChangeType.Update && x.Entity.Id == 5) flag = true;
            });

            ctx.BeginTransaction();
            var p = ctx.Database.SampleTable.FindById(targetId);
            var p2 = new Sample { Id = targetId, Age = 24, FirstName = "aaa", LastName = "foo" };
            ctx.Transaction.Diff(p2);
            flag.Should().BeFalse();
            ctx.Rollback();
            flag.Should().BeFalse();
            var p3 = ctx.Database.SampleTable.FindById(targetId);
            p3.Age.Should().Be(19);
        }
    }
}
