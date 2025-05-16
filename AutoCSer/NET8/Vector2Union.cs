using System;
using System.Runtime.InteropServices;

namespace AutoCSer
{
    /// <summary>
    /// Vector2
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = sizeof(float) * 2)]
    internal struct Vector2Union
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal SerializeVector2 SerializeValue;
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal System.Numerics.Vector2 Vector2;
    }
}
