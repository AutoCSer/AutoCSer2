using System;

#pragma warning disable
namespace AutoCSer.TestCase.SerializePerformance
{
    /// <summary>
    /// JSON 序列化属性测试数据
    /// </summary>
    [AutoCSer.JsonSerialize(CheckLoopReference = false)]
    class FloatPropertyData : PropertyData
    {
        public double Double { get; set; }
        public float Float { get; set; }
        public decimal Decimal { get; set; }
        public double? DoubleNull { get; set; }
        public float? FloatNull { get; set; }
        public decimal? DecimalNull { get; set; }
    }
}
#pragma warning restore
