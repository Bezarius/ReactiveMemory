// <auto-generated />
#pragma warning disable CS0105
using LiteDB;
using MessagePack;
using ReactiveMemory.Validation;
using ReactiveMemory;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using TestPerfLiteDB;
using TestPerfLiteDB.Tables;

namespace TestPerfLiteDB
{
   public sealed class DatabaseBuilder : DatabaseBuilderBase
   {
        public DatabaseBuilder() : this(null) { }
        public DatabaseBuilder(MessagePack.IFormatterResolver resolver) : base(resolver) { }

        public DatabaseBuilder Append(System.Collections.Generic.IEnumerable<TestDoc> dataSource)
        {
            AppendCore(dataSource, x => x.id, System.Collections.Generic.Comparer<int>.Default);
            return this;
        }

    }
}