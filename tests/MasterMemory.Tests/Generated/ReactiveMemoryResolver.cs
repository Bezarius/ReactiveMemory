// <auto-generated />
#pragma warning disable CS0105
using ReactiveMemory.Tests.TestStructures;
using System;

namespace ReactiveMemory.Tests
{
    public class ReactiveMemoryResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new ReactiveMemoryResolver();

        ReactiveMemoryResolver()
        {

        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> formatter;

            static FormatterCache()
            {
                var f = MasterMemoryResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class MasterMemoryResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static MasterMemoryResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(12)
            {
                {typeof(Fail[]), 0 },
                {typeof(ItemMaster[]), 1 },
                {typeof(ItemMasterEmptyValidate[]), 2 },
                {typeof(PersonModel[]), 3 },
                {typeof(QuestMaster[]), 4 },
                {typeof(QuestMasterEmptyValidate[]), 5 },
                {typeof(Sample[]), 6 },
                {typeof(SequentialCheckMaster[]), 7 },
                {typeof(SingleMaster[]), 8 },
                {typeof(SkillMaster[]), 9 },
                {typeof(TestMaster[]), 10 },
                {typeof(UserLevel[]), 11 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new MessagePack.Formatters.ArrayFormatter<Fail>();
                case 1: return new MessagePack.Formatters.ArrayFormatter<ItemMaster>();
                case 2: return new MessagePack.Formatters.ArrayFormatter<ItemMasterEmptyValidate>();
                case 3: return new MessagePack.Formatters.ArrayFormatter<PersonModel>();
                case 4: return new MessagePack.Formatters.ArrayFormatter<QuestMaster>();
                case 5: return new MessagePack.Formatters.ArrayFormatter<QuestMasterEmptyValidate>();
                case 6: return new MessagePack.Formatters.ArrayFormatter<Sample>();
                case 7: return new MessagePack.Formatters.ArrayFormatter<SequentialCheckMaster>();
                case 8: return new MessagePack.Formatters.ArrayFormatter<SingleMaster>();
                case 9: return new MessagePack.Formatters.ArrayFormatter<SkillMaster>();
                case 10: return new MessagePack.Formatters.ArrayFormatter<TestMaster>();
                case 11: return new MessagePack.Formatters.ArrayFormatter<UserLevel>();
                default: return null;
            }
        }
    }
}