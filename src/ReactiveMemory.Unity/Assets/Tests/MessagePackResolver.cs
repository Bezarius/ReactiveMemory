﻿using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;
using ReactiveMemory.Tests.Resolvers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveMemory.Tests
{
    public class MessagePackResolver : IFormatterResolver
    {
        public static IFormatterResolver Instance = new MessagePackResolver();

        MessagePackResolver()
        {

        }

        public IMessagePackFormatter<T> GetFormatter<T>()
        {
            return ReactiveMemoryResolver.Instance.GetFormatter<T>()
                ?? GeneratedResolver.Instance.GetFormatter<T>()
                ?? StandardResolver.Instance.GetFormatter<T>();
        }
    }
}
