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

namespace ReactiveMemory.Test.Formatters.ReactiveMemory.Tests.TestStructures
{
    public sealed class SingleMasterFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::ReactiveMemory.Tests.TestStructures.SingleMaster>
    {
        // Id
        private static global::System.ReadOnlySpan<byte> GetSpan_Id() => new byte[1 + 2] { 162, 73, 100 };

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::ReactiveMemory.Tests.TestStructures.SingleMaster value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
                return;
            }

            writer.WriteMapHeader(1);
            writer.WriteRaw(GetSpan_Id());
            writer.Write(value.Id);
        }

        public global::ReactiveMemory.Tests.TestStructures.SingleMaster Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            var length = reader.ReadMapHeader();
            var __Id__ = default(int);

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

                }
            }

            var ____result = new global::ReactiveMemory.Tests.TestStructures.SingleMaster(__Id__);
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
