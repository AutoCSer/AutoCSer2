using System;
using System.Runtime.InteropServices;

namespace AutoCSer
{
    /// <summary>
    /// System.Numerics.Complex
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = sizeof(double) * 2)]
    internal struct ComplexUnion
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal SerializeComplex SerializeValue;
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal System.Numerics.Complex Complex;
    }
}
