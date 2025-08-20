using System;
using System.Runtime.InteropServices;

namespace AutoCSer
{
    /// <summary>
    /// System.Numerics.Complex
    /// </summary>
    [AutoCSer.CodeGenerator.JsonSerialize]
    [StructLayout(LayoutKind.Sequential, Size = sizeof(double) * 2)]
    internal partial struct SerializeComplex
    {
        /// <summary>
        /// 实数
        /// </summary>
        public double Real;
        /// <summary>
        /// 虚数
        /// </summary>
        public double Imaginary;
    }
}
