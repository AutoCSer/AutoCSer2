using System;

namespace AutoCSer.TestCase.Data
{
    /// <summary>
    /// 二进制混杂 JSON 序列化
    /// </summary>
    [AutoCSer.BinarySerialize(IsMixJsonSerialize = true)]
    internal class JsonEmpty
    {
    }
}
