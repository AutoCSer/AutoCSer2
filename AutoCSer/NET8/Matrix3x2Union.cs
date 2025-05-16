using System;
using System.Runtime.InteropServices;

namespace AutoCSer
{
    /// <summary>
    /// Matrix3x2
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = sizeof(float) * 6)]
    internal struct Matrix3x2Union
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal SerializeMatrix3x2 SerializeValue;
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal System.Numerics.Matrix3x2 Matrix3x2;
    }
}
