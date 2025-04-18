using System;

#pragma warning disable
namespace AutoCSer.TestCase.Data
{
    /// <summary>
    /// 浮点数测试
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.JsonSerialize]
    [AutoCSer.CodeGenerator.XmlSerialize]
    [AutoCSer.CodeGenerator.DefaultConstructor]
    [AutoCSer.CodeGenerator.FieldEquals]
#endif
    public partial class Float
    {
        //public Half HalfMin = Half.MinValue;
        //public Half HalfMax = Half.MaxValue;
        //public Half HalfNaN = Half.NaN;
        //public Half HalfPositiveInfinity = Half.PositiveInfinity;
        //public Half HalfNegativeInfinity = Half.NegativeInfinity;

        public float FloatMin = float.MinValue;
        public float FloatMax = float.MaxValue;
        public float FloatNaN = float.NaN;
        public float FloatPositiveInfinity = float.PositiveInfinity;
        public float FloatNegativeInfinity = float.NegativeInfinity;

        public double DoubleMin = double.MinValue;
        public double DoubleMax = double.MaxValue;
        public double DoubleNaN = double.NaN;
        public double DoublePositiveInfinity = double.PositiveInfinity;
        public double DoubleNegativeInfinity = double.NegativeInfinity;

        public double Double0 = 1.7976931348623150E+308;
        public double Double1 = 1.7976931348623151E+308;
        public double Double2 = 1.7976931348623152E+308;
        public double Double3 = 1.7976931348623153E+308;
        public double Double4 = 1.7976931348623154E+308;
        public double Double5 = 1.7976931348623155E+308;
        public double Double6 = 1.7976931348623156E+308;
        public double Double7 = 1.7976931348623157E+308;

        public double DoubleNegative0 = -1.7976931348623150E+308;
        public double DoubleNegative1 = -1.7976931348623151E+308;
        public double DoubleNegative2 = -1.7976931348623152E+308;
        public double DoubleNegative3 = -1.7976931348623153E+308;
        public double DoubleNegative4 = -1.7976931348623154E+308;
        public double DoubleNegative5 = -1.7976931348623155E+308;
        public double DoubleNegative6 = -1.7976931348623156E+308;
        public double DoubleNegative7 = -1.7976931348623157E+308;
    }
}
