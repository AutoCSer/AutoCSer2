using System;

namespace AutoCSer.TestCase.SerializePerformance
{
    /// <summary>
    /// JSON 混杂二进制 测试数据
    /// </summary>
    [AutoCSer.CodeGenerator.RandomObject]
    [AutoCSer.BinarySerialize(IsJsonMix = true, IsReferenceMember = false)]
    partial class JsonFloatPropertyData : FloatPropertyData
    {
    }
}
