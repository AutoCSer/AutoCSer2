using System;

#pragma warning disable
namespace AutoCSer.TestCase.SerializePerformance
{
    /// <summary>
    /// JSON 序列化属性测试数据
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.BinarySerialize]
    [AutoCSer.CodeGenerator.JsonSerialize]
    [AutoCSer.CodeGenerator.XmlSerialize]
    [AutoCSer.CodeGenerator.DefaultConstructor]
    [AutoCSer.CodeGenerator.RandomObject]
#endif
    [AutoCSer.JsonSerialize(CheckLoopReference = false)]
    partial class FloatPropertyData : PropertyData
    {
        //public Half Half { get; set; }
        public float Float { get; set; }
        public double Double { get; set; }
        public decimal Decimal { get; set; }
        //public Half? HalfNull { get; set; }
        public float? FloatNull { get; set; }
        public double? DoubleNull { get; set; }
        public decimal? DecimalNull { get; set; }
    }
}
