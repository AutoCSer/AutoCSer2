using System;

namespace AutoCSer.TestCase.Data
{
    /// <summary>
    /// 二进制混杂 JSON 序列化
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.MemberCopy]
    [AutoCSer.CodeGenerator.RandomObject]
    [AutoCSer.CodeGenerator.FieldEquals]
#endif
    [AutoCSer.BinarySerialize(IsMixJsonSerialize = true)]
    internal partial class JsonProperty : Property
    {
    }
}
