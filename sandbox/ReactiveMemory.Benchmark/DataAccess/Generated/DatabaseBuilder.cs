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
   public sealed class DatabaseBuilder : DatabaseBuilderBase
   {
        public DatabaseBuilder() : this(null) { }
        public DatabaseBuilder(MessagePack.IFormatterResolver resolver) : base(resolver) { }

        public DatabaseBuilder Append(System.Collections.Generic.IEnumerable<Monster> dataSource)
        {
            AppendCore(dataSource, x => x.MonsterId, System.Collections.Generic.Comparer<int>.Default);
            return this;
        }

        public DatabaseBuilder Append(System.Collections.Generic.IEnumerable<Person> dataSource)
        {
            AppendCore(dataSource, x => x.PersonId, System.Collections.Generic.Comparer<int>.Default);
            return this;
        }

    }
}