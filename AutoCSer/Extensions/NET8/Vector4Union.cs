using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Vector4
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = sizeof(float) * 4)]
    internal struct Vector4Union
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal SerializeVector4 SerializeValue;
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal System.Numerics.Vector4 Vector4;
    }
}
