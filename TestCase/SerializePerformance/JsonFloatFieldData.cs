using System;

namespace AutoCSer.TestCase.SerializePerformance
{
    /// <summary>
    /// JSON 混杂二进制 测试数据
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.RandomObject]
#endif
    [AutoCSer.BinarySerialize(IsJsonMix = true, IsReferenceMember = false)]
    partial class JsonFloatFieldData : FloatFieldData
    {
    }
}
