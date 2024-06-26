// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
#pragma warning disable CS1591 // document public APIs

#pragma warning disable SA1129 // Do not use default value type constructor
#pragma warning disable SA1309 // Field names should not begin with underscore
#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1649 // File name should match first type name

namespace ReactiveMemory.Tests.Formatters.ReactiveMemory.Tests
{
    public sealed class SampleFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::ReactiveMemory.Tests.Sample>
    {
        // Id
        private static global::System.ReadOnlySpan<byte> GetSpan_Id() => new byte[1 + 2] { 162, 73, 100 };
        // Age
        private static global::System.ReadOnlySpan<byte> GetSpan_Age() => new byte[1 + 3] { 163, 65, 103, 101 };
        // FirstName
        private static global::System.ReadOnlySpan<byte> GetSpan_FirstName() => new byte[1 + 9] { 169, 70, 105, 114, 115, 116, 78, 97, 109, 101 };
        // LastName
        private static global::System.ReadOnlySpan<byte> GetSpan_LastName() => new byte[1 + 8] { 168, 76, 97, 115, 116, 78, 97, 109, 101 };

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::ReactiveMemory.Tests.Sample value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
                return;
            }

            var formatterResolver = options.Resolver;
            writer.WriteMapHeader(4);
            writer.WriteRaw(GetSpan_Id());
            writer.Write(value.Id);
            writer.WriteRaw(GetSpan_Age());
            writer.Write(value.Age);
            writer.WriteRaw(GetSpan_FirstName());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<string>(formatterResolver).Serialize(ref writer, value.FirstName, options);
            writer.WriteRaw(GetSpan_LastName());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<string>(formatterResolver).Serialize(ref writer, value.LastName, options);
        }

        public global::ReactiveMemory.Tests.Sample Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            var formatterResolver = options.Resolver;
            var length = reader.ReadMapHeader();
            var __Id__ = default(int);
            var __Age__ = default(int);
            var __FirstName__ = default(string);
            var __LastName__ = default(string);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.Internal.CodeGenHelpers.ReadStringSpan(ref reader);
                switch (stringKey.Length)
                {
                    default:
                    FAIL:
                      reader.Skip();
                      continue;
                    case 2:
                        if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 25673UL) { goto FAIL; }

                        __Id__ = reader.ReadInt32();
                        continue;
                    case 3:
                        if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 6645569UL) { goto FAIL; }

                        __Age__ = reader.ReadInt32();
                        continue;
                    case 9:
                        if (!global::System.MemoryExtensions.SequenceEqual(stringKey, GetSpan_FirstName().Slice(1))) { goto FAIL; }

                        __FirstName__ = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<string>(formatterResolver).Deserialize(ref reader, options);
                        continue;
                    case 8:
                        if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 7308604759881245004UL) { goto FAIL; }

                        __LastName__ = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<string>(formatterResolver).Deserialize(ref reader, options);
                        continue;

                }
            }

            var ____result = new global::ReactiveMemory.Tests.Sample(__Id__, __Age__, __FirstName__, __LastName__);
            reader.Depth--;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1129 // Do not use default value type constructor
#pragma warning restore SA1309 // Field names should not begin with underscore
#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1403 // File may only contain a single namespace
#pragma warning restore SA1649 // File name should match first type name
