using System;

namespace AutoCSer.TestCase.SerializePerformance
{
    /// <summary>
    /// 二进制混杂 JSON 测试数据
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.BinarySerialize]
    [AutoCSer.CodeGenerator.JsonSerialize]
    [AutoCSer.CodeGenerator.DefaultConstructor]
    [AutoCSer.CodeGenerator.RandomObject]
#endif
    [AutoCSer.BinarySerialize(IsMixJsonSerialize = true)]
    partial class JsonFloatPropertyData : FloatPropertyData
    {
    }
}
