using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// System.Numerics.Complex
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.XmlSerialize]
#endif
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
