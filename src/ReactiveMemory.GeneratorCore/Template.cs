﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveMemory.GeneratorCore
{
    public partial class DatabaseBuilderTemplate
    {
        public string Namespace { get; set; }
        public string Using { get; set; }
        public string PrefixClassName { get; set; }
        public GenerationContext[] GenerationContexts { get; set; }

        public string ClassName => PrefixClassName + "DatabaseBuilder";
    }

    public partial class MemoryDatabaseTemplate
    {
        public string Namespace { get; set; }
        public string Using { get; set; }
        public string PrefixClassName { get; set; }
        public GenerationContext[] GenerationContexts { get; set; }
        public string ClassName => PrefixClassName + "MemoryDatabase";
    }

    public partial class MetaMemoryDatabaseTemplate
    {
        public string Namespace { get; set; }
        public string Using { get; set; }
        public string PrefixClassName { get; set; }
        public GenerationContext[] GenerationContexts { get; set; }
        public string ClassName => PrefixClassName + "MetaMemoryDatabase";
    }

    public partial class TransactionTemplate
    {
        public string Namespace { get; set; }
        public string Using { get; set; }
        public string PrefixClassName { get; set; }
        public GenerationContext[] GenerationContexts { get; set; }
        public string ClassName => PrefixClassName + "Transaction";
    }

    public partial class MessagePackResolverTemplate
    {
        public string Namespace { get; set; }
        public string Using { get; set; }
        public string PrefixClassName { get; set; }
        public GenerationContext[] GenerationContexts { get; set; }
        public string ClassName => PrefixClassName + "ReactiveMemoryResolver";
    }

    public partial class TableTemplate
    {
        public string Namespace { get; set; }
        public string Using { get; set; }
        public string PrefixClassName { get; set; }
        public GenerationContext GenerationContext { get; set; }

        public bool ThrowKeyIfNotFound { get; set; }
    }

    public partial class DataBaseContextTemplate
    {
        public string Namespace { get; set; }
        public string Using { get; set; }
        public string PrefixClassName { get; set; }
        public GenerationContext[] GenerationContexts { get; set; }
        public string ClassName => PrefixClassName + "DbContext";
        public string DatabaseBuilderClassName { get; set; }
        public string MemoryDatabaseClassName { get; set; }
        public string TransactionClassName { get; set; }
    }
}
