using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Matrix4x4
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = sizeof(float) * 16)]
    internal struct Matrix4x4Union
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal SerializeMatrix4x4 SerializeValue;
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal System.Numerics.Matrix4x4 Matrix4x4;
    }
}
