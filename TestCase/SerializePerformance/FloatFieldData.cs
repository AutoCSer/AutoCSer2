using System;

#pragma warning disable
namespace AutoCSer.TestCase.SerializePerformance
{
    /// <summary>
    /// 浮点数字段测试数据
    /// </summary>
    [AutoCSer.CodeGenerator.JsonSerialize]
    [AutoCSer.CodeGenerator.XmlSerialize]
    [AutoCSer.CodeGenerator.RandomObject]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [AutoCSer.JsonSerialize(CheckLoopReference = false)]
    partial class FloatFieldData : FieldData
    {
        //public Half Half;
        public float Float;
        public double Double;
        public decimal Decimal;
        //public Half? HalfNull;
        public float? FloatNull;
        public double? DoubleNull;
        public decimal? DecimalNull;
    }
}
