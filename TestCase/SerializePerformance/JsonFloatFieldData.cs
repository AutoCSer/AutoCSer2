using System;

namespace AutoCSer.TestCase.SerializePerformance
{
    /// <summary>
    /// 二进制混杂 JSON 测试数据
    /// </summary>
    [AutoCSer.BinarySerialize(IsMixJsonSerialize = true)]
    class JsonFloatFieldData : FloatFieldData
    {
    }
}
