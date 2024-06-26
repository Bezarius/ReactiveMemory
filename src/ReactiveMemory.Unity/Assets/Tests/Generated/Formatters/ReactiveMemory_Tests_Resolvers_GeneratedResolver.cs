// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
#pragma warning disable CS1591 // document public APIs

#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1649 // File name should match first type name

namespace ReactiveMemory.Tests.Resolvers
{
    public class GeneratedResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new GeneratedResolver();

        private GeneratedResolver()
        {
        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.Formatter;
        }

        private static class FormatterCache<T>
        {
            internal static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> Formatter;

            static FormatterCache()
            {
                var f = GeneratedResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    Formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class GeneratedResolverGetFormatterHelper
    {
        private static readonly global::System.Collections.Generic.Dictionary<global::System.Type, int> lookup;

        static GeneratedResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<global::System.Type, int>(12)
            {
                { typeof(global::ReactiveMemory.Tests.Sample), 0 },
                { typeof(global::ReactiveMemory.Tests.SkillMaster), 1 },
                { typeof(global::ReactiveMemory.Tests.TestStructures.Fail), 2 },
                { typeof(global::ReactiveMemory.Tests.TestStructures.ItemMaster), 3 },
                { typeof(global::ReactiveMemory.Tests.TestStructures.ItemMasterEmptyValidate), 4 },
                { typeof(global::ReactiveMemory.Tests.TestStructures.PersonModel), 5 },
                { typeof(global::ReactiveMemory.Tests.TestStructures.QuestMaster), 6 },
                { typeof(global::ReactiveMemory.Tests.TestStructures.QuestMasterEmptyValidate), 7 },
                { typeof(global::ReactiveMemory.Tests.TestStructures.SequentialCheckMaster), 8 },
                { typeof(global::ReactiveMemory.Tests.TestStructures.SingleMaster), 9 },
                { typeof(global::ReactiveMemory.Tests.TestStructures.TestMaster), 10 },
                { typeof(global::ReactiveMemory.Tests.UserLevel), 11 },
            };
        }

        internal static object GetFormatter(global::System.Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key))
            {
                return null;
            }

            switch (key)
            {
                case 0: return new ReactiveMemory.Tests.Formatters.ReactiveMemory.Tests.SampleFormatter();
                case 1: return new ReactiveMemory.Tests.Formatters.ReactiveMemory.Tests.SkillMasterFormatter();
                case 2: return new ReactiveMemory.Tests.Formatters.ReactiveMemory.Tests.TestStructures.FailFormatter();
                case 3: return new ReactiveMemory.Tests.Formatters.ReactiveMemory.Tests.TestStructures.ItemMasterFormatter();
                case 4: return new ReactiveMemory.Tests.Formatters.ReactiveMemory.Tests.TestStructures.ItemMasterEmptyValidateFormatter();
                case 5: return new ReactiveMemory.Tests.Formatters.ReactiveMemory.Tests.TestStructures.PersonModelFormatter();
                case 6: return new ReactiveMemory.Tests.Formatters.ReactiveMemory.Tests.TestStructures.QuestMasterFormatter();
                case 7: return new ReactiveMemory.Tests.Formatters.ReactiveMemory.Tests.TestStructures.QuestMasterEmptyValidateFormatter();
                case 8: return new ReactiveMemory.Tests.Formatters.ReactiveMemory.Tests.TestStructures.SequentialCheckMasterFormatter();
                case 9: return new ReactiveMemory.Tests.Formatters.ReactiveMemory.Tests.TestStructures.SingleMasterFormatter();
                case 10: return new ReactiveMemory.Tests.Formatters.ReactiveMemory.Tests.TestStructures.TestMasterFormatter();
                case 11: return new ReactiveMemory.Tests.Formatters.ReactiveMemory.Tests.UserLevelFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1649 // File name should match first type name
