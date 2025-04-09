using System;

namespace AutoCSer.TestCase.Data
{
    /// <summary>
    /// 二进制混杂 JSON 序列化
    /// </summary>
    [AutoCSer.CodeGenerator.JsonSerialize]
    [AutoCSer.CodeGenerator.BinarySerialize]
    [AutoCSer.AOT.Preserve(AllMembers = true)]
    [AutoCSer.BinarySerialize(IsMixJsonSerialize = true)]
    internal partial class JsonField : Field
    {
    }
}
