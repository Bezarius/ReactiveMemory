﻿// <auto-generated />
using MessagePack;
using ReactiveMemory.Benchmark.Tables;
using ReactiveMemory.Validation;

namespace ReactiveMemory.Benchmark
{
    public interface IMemoryDatabase1
    {
        TestDocTable TestDocTable { get; }

        Transaction BeginTransaction();
        DatabaseBuilder ToDatabaseBuilder();
        DatabaseBuilder ToDatabaseBuilder(IFormatterResolver resolver);
        ValidateResult Validate();
    }
}