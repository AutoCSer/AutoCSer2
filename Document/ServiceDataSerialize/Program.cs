using System;

namespace AutoCSer.Document.ServiceDataSerialize
{
    /// <summary>
    /// https://zhuanlan.zhihu.com/p/8762985779
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"{nameof(BinarySerialize)}.{nameof(BinarySerialize.LoopReference)} {BinarySerialize.LoopReference.TestCase()}");
            Console.WriteLine($"{nameof(BinarySerialize)}.{nameof(BinarySerialize.DisabledReference)} {BinarySerialize.DisabledReference.TestCase()}");
            Console.WriteLine($"{nameof(BinarySerialize)}.{nameof(BinarySerialize.PublicInstanceField)} {BinarySerialize.PublicInstanceField.TestCase()}");
            Console.WriteLine($"{nameof(BinarySerialize)}.{nameof(BinarySerialize.Property)} {BinarySerialize.Property.TestCase()}");
            Console.WriteLine($"{nameof(BinarySerialize)}.{nameof(BinarySerialize.IgnoreMember)} {BinarySerialize.IgnoreMember.TestCase()}");
            Console.WriteLine($"{nameof(BinarySerialize)}.{nameof(BinarySerialize.AnonymousType)} {BinarySerialize.AnonymousType.TestCase()}");
            Console.WriteLine($"{nameof(BinarySerialize)}.{nameof(BinarySerialize.MemberMap)} {BinarySerialize.MemberMap.TestCase()}");
            Console.WriteLine($"{nameof(BinarySerialize)}.{nameof(BinarySerialize.DisabledMemberMap)} {BinarySerialize.DisabledMemberMap.TestCase()}");
            Console.WriteLine($"{nameof(BinarySerialize)}.{nameof(BinarySerialize.BaseType)} {BinarySerialize.BaseType.TestCase()}");
            Console.WriteLine($"{nameof(BinarySerialize)}.{nameof(BinarySerialize.RealType)} {BinarySerialize.RealType.TestCase()}");
            Console.WriteLine($"{nameof(BinarySerialize)}.{nameof(BinarySerialize.GlobalVersion)} {BinarySerialize.GlobalVersion.TestCase()}");
            Console.WriteLine($"{nameof(BinarySerialize)}.{nameof(BinarySerialize.JsonMember)} {BinarySerialize.JsonMember.TestCase()}");
            Console.WriteLine($"{nameof(BinarySerialize)}.{nameof(BinarySerialize.Custom)} {BinarySerialize.Custom.TestCase()}");

            Console.WriteLine($"{nameof(JsonSerialize)}.{nameof(JsonSerialize.PublicInstanceField)} {JsonSerialize.PublicInstanceField.TestCase()}");
            Console.WriteLine($"{nameof(JsonSerialize)}.{nameof(JsonSerialize.AnonymousType)} {JsonSerialize.AnonymousType.TestCase()}");
            Console.WriteLine($"{nameof(JsonSerialize)}.{nameof(JsonSerialize.IgnoreMember)} {JsonSerialize.IgnoreMember.TestCase()}");
            Console.WriteLine($"{nameof(JsonSerialize)}.{nameof(JsonSerialize.MemberMap)} {JsonSerialize.MemberMap.TestCase()}");
            Console.WriteLine($"{nameof(JsonSerialize)}.{nameof(JsonSerialize.Custom)} {JsonSerialize.Custom.TestCase()}");
            Console.WriteLine($"{nameof(JsonSerialize)}.{nameof(JsonSerialize.BaseType)} {JsonSerialize.BaseType.TestCase()}");
            Console.WriteLine($"{nameof(JsonSerialize)}.{nameof(JsonSerialize.JsonNode)} {JsonSerialize.JsonNode.TestCase()}");
        }
    }
}
