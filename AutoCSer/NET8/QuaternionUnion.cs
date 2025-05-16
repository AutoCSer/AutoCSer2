using System;
using System.Runtime.InteropServices;

namespace AutoCSer
{
    /// <summary>
    /// Quaternion
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = sizeof(float) * 4)]
    internal struct QuaternionUnion
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal SerializeQuaternion SerializeValue;
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal System.Numerics.Quaternion Quaternion;
    }
}
