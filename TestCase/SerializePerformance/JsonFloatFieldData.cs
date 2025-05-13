using System;

namespace AutoCSer.TestCase.SerializePerformance
{
    /// <summary>
    /// 二进制混杂 JSON 测试数据
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.RandomObject]
#endif
    [AutoCSer.BinarySerialize(IsMixJsonSerialize = true, IsReferenceMember = false)]
    partial class JsonFloatFieldData : FloatFieldData
    {
    }
}
