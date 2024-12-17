using ReactiveMemory.Tests.TestStructures;
using FluentAssertions;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using ReactiveMemory.Tests;

namespace ReactiveMemory.Tests
{
    public class ValidatorTest
    {
        readonly Xunit.Abstractions.ITestOutputHelper output;

#if UNITY_2018_3_OR_NEWER
        public ValidatorTest()
        {
            this.output = new Xunit.Abstractions.DebugLogTestOutputHelper();
            MessagePackSerializer.DefaultOptions = MessagePackSerializer.DefaultOptions.WithResolver(MessagePackResolver.Instance);
        }
#else
        public ValidatorTest(Xunit.Abstractions.ITestOutputHelper output)
        {
            this.output = output;
            MessagePackSerializer.DefaultOptions = MessagePackSerializer.DefaultOptions.WithResolver(MessagePackResolver.Instance);
        }
#endif

        MemoryDatabase CreateDatabase(Fail[] data1)
        {

            var bin = new DatabaseBuilder()
                .Append(data1)
                .Build();

            return new MemoryDatabase(bin, internString: false, changesMediatorFactory: UniRxSubjectFactory.Default);
        }

        MemoryDatabase CreateDatabase(SingleMaster[] data1)
        {

            var bin = new DatabaseBuilder()
                .Append(data1)
                .Build();

            return new MemoryDatabase(bin, internString: false, changesMediatorFactory: UniRxSubjectFactory.Default);
        }

        MemoryDatabase CreateDatabase(SequentialCheckMaster[] data1)
        {

            var bin = new DatabaseBuilder()
                .Append(data1)
                .Build();

            return new MemoryDatabase(bin, internString: false, changesMediatorFactory: UniRxSubjectFactory.Default);
        }

        MemoryDatabase CreateDatabase(QuestMaster[] data1, ItemMaster[] data2)
        {

            var bin = new DatabaseBuilder()
                .Append(data1)
                .Append(data2)
                .Build();

            return new MemoryDatabase(bin, internString: false, changesMediatorFactory: UniRxSubjectFactory.Default);
        }

        MemoryDatabase CreateDatabase(QuestMasterEmptyValidate[] data1, ItemMasterEmptyValidate[] data2)
        {

            var bin = new DatabaseBuilder()
                .Append(data1)
                .Append(data2)
                .Build();

            return new MemoryDatabase(bin, internString: false, changesMediatorFactory: UniRxSubjectFactory.Default);
        }

        [Fact]
        public void Empty()
        {
            var validateResult = CreateDatabase(new QuestMaster[]
            {
            }, new ItemMaster[]
            {
            }).Validate();

            validateResult.IsValidationFailed.Should().BeFalse();
            validateResult.FailedResults.Count.Should().Be(0);
        }

        [Fact]
        public void PKUnique()
        {
            var validateResult = CreateDatabase(
                new QuestMasterEmptyValidate[]
                {
                    new QuestMasterEmptyValidate (1, "", 0, 0),
                    new QuestMasterEmptyValidate (2, "", 0, 0),
                    new QuestMasterEmptyValidate (1, "", 0, 0),
                    new QuestMasterEmptyValidate (4, "", 0, 0),
                    new QuestMasterEmptyValidate (4, "", 0, 0),
                },
                new ItemMasterEmptyValidate[]
                {
                    new ItemMasterEmptyValidate (1),
                    new ItemMasterEmptyValidate (2),
                    new ItemMasterEmptyValidate (2),
                }).Validate();
            output.WriteLine(validateResult.FormatFailedResults());

            validateResult.IsValidationFailed.Should().BeTrue();
            validateResult.FailedResults.Count.Should().Be(3); // Q:1,4 + I:2
            var faileds = validateResult.FailedResults.OrderBy(x => x.Message).ToArray();

            faileds[0].Message.Should().Be("Unique failed: ItemId, value = 2");
            faileds[1].Message.Should().Be("Unique failed: QuestId, value = 1");
            faileds[2].Message.Should().Be("Unique failed: QuestId, value = 4");
        }

        // test IValidator

        /*
        public interface IValidator<T>
        {
            ValidatableSet<T> GetTableSet();
            ReferenceSet<T, TRef> GetReferenceSet<TRef>();
            void Validate(Expression<Func<T, bool>> predicate);
            void Validate(Func<T, bool> predicate, string message);
            void ValidateAction(Expression<Func<bool>> predicate);
            void ValidateAction(Func<bool> predicate, string message);
            void Fail(string message);
            bool CallOnce();
        }

        ReferenceSet.Exists
        ValidatableSet.Unique
        ValidatableSet.Sequential
    */

        [Fact]
        public void Exists()
        {
            var validateResult = CreateDatabase(new QuestMaster[]
            {
                new QuestMaster (1,"foo", 1, 1 ),
                new QuestMaster(2,  "bar", 3,3  ),
                new QuestMaster (3,  "baz" , 2, 2),
                new QuestMaster (4, "tako", 5, 5),
                new QuestMaster (5, "nano", 4, 4),
            }, new ItemMaster[]
            {
                new ItemMaster (1),
                new ItemMaster (2),
                new ItemMaster (3),
            }).Validate();
            output.WriteLine(validateResult.FormatFailedResults());
            validateResult.IsValidationFailed.Should().BeTrue();

            validateResult.FailedResults[0].Message.Should().Be("Exists failed: QuestMaster.RewardItemId -> ItemMaster.ItemId, value = 5, PK(QuestId) = 4");
            validateResult.FailedResults[1].Message.Should().Be("Exists failed: QuestMaster.RewardItemId -> ItemMaster.ItemId, value = 4, PK(QuestId) = 5");
        }

        [Fact]
        public void Unique()
        {
            var validateResult = CreateDatabase(new QuestMaster[]
            {
                new QuestMaster(1, "foo", 0, 0),
                new QuestMaster(2, "baz", 0, 0),
                new QuestMaster(3, "baz", 0, 0),
                new QuestMaster(4, "nano", 0, 0),
                new QuestMaster(5, "nano", 0, 0),
            }, new ItemMaster[]
            {
                new ItemMaster(0)
            }).Validate();

            output.WriteLine(validateResult.FormatFailedResults());
            validateResult.IsValidationFailed.Should().BeTrue();

            // Fix the expected failed results by correcting the QuestId values
            validateResult.FailedResults[0].Message.Should().Be("Unique failed: .Name, value = baz, PK(QuestId) = 3");
            validateResult.FailedResults[1].Message.Should().Be("Unique failed: .Name, value = nano, PK(QuestId) = 5");
        }

        [Fact]
        public void Sequential()
        {
            {
                var validateResult = CreateDatabase(new SequentialCheckMaster[]
                {
                    new SequentialCheckMaster (1,10),
                    new SequentialCheckMaster (2, 11),
                    new SequentialCheckMaster (3, 11),
                    new SequentialCheckMaster (4, 12),
                }).Validate();
                output.WriteLine(validateResult.FormatFailedResults());
                validateResult.IsValidationFailed.Should().BeFalse();
            }
            {
                var validateResult = CreateDatabase(new SequentialCheckMaster[]
                {
                    new SequentialCheckMaster (1, 10),
                    new SequentialCheckMaster (2, 11),
                    new SequentialCheckMaster (3, 11),
                    new SequentialCheckMaster (5, 13),
                }).Validate();
                output.WriteLine(validateResult.FormatFailedResults());
                validateResult.IsValidationFailed.Should().BeTrue();

                validateResult.FailedResults[0].Message.Should().Be("Sequential failed: .Id, value = (3, 5), PK(Id) = 5");
                validateResult.FailedResults[1].Message.Should().Be("Sequential failed: .Cost, value = (11, 13), PK(Id) = 5");
            }
        }

        [Fact]
        public void CallOnce()
        {
            _ = CreateDatabase(new SingleMaster[]
            {
                new SingleMaster (1),
                new SingleMaster (2),
                new SingleMaster (3)    ,
                new SingleMaster (4),
            }).Validate();


            SingleMaster.CalledValidateCount.Should().Be(4);
            SingleMaster.CalledOnceCount.Should().Be(1);
        }

        [Fact]
        public void Validate()
        {
            var validateResult = CreateDatabase(new QuestMaster[]
            {
                new QuestMaster (1,"foo",1,-1 ),
                new QuestMaster ( 2, "bar",3, 99 ),
                new QuestMaster (3, "baz", 2, 100),
                new QuestMaster (4, "tao", 3, 101),
                new QuestMaster (5, "nao", 3, 33),
            }, new ItemMaster[]
            {
                new ItemMaster (1),
                new ItemMaster (2),
                new ItemMaster (3),
            }).Validate();
            output.WriteLine(validateResult.FormatFailedResults());
            validateResult.IsValidationFailed.Should().BeTrue();

            validateResult.FailedResults[0].Message.Should().Be("Validate failed: >= 0!!!, PK(QuestId) = 1");
            validateResult.FailedResults[1].Message.Should().Be("Validate failed: (this.Cost <= 100), Cost = 101, PK(QuestId) = 4");
        }

        [Fact]
        public void ValidateAction()
        {
            var validateResult = CreateDatabase(new QuestMaster[]
             {
                new QuestMaster ( 1,   "foo",1, -100 ),
                new QuestMaster (  2, "bar",  3, 99 ),
                new QuestMaster (3, "baz", 2, 100),
                new QuestMaster (4, "tao", 3, 1001),
                new QuestMaster (5, "nao", 3, 33),
             }, new ItemMaster[]
             {
                new ItemMaster (1),
                new ItemMaster (2),
                new ItemMaster (3),
             }).Validate();
            output.WriteLine(validateResult.FormatFailedResults());
            validateResult.IsValidationFailed.Should().BeTrue();

            var results = validateResult.FailedResults.Select(x => x.Message).Where(x => x.Contains("ValidateAction faile")).ToArray();

            results[0].Should().Be("ValidateAction failed: >= -90!!!, PK(QuestId) = 1");
            results[1].Should().Be("ValidateAction failed: (value(ReactiveMemory.Tests.TestStructures.QuestMaster).Cost <= 1000), PK(QuestId) = 4");
        }

        [Fact]
        public void Fail()
        {
            var validateResult = CreateDatabase(new Fail[]
            {
                new Fail (1),
                new Fail (2),
                new Fail (3),
            }).Validate();
            output.WriteLine(validateResult.FormatFailedResults());
            validateResult.IsValidationFailed.Should().BeTrue();

            var msg = validateResult.FailedResults.Select(x => x.Message).ToArray();
            msg[0].Should().Be("Failed Id:1, PK(Id) = 1");
            msg[1].Should().Be("Failed Id:2, PK(Id) = 2");
            msg[2].Should().Be("Failed Id:3, PK(Id) = 3");
        }
    }
}
