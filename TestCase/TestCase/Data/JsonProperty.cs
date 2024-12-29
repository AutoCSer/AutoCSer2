using System;

namespace AutoCSer.TestCase.Data
{
    /// <summary>
    /// 二进制混杂 JSON 序列化
    /// </summary>
    [AutoCSer.AOT.Preserve(AllMembers = true)]
    [AutoCSer.BinarySerialize(IsMixJsonSerialize = true)]
    internal class JsonProperty
    {
    }
}
