using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Vector3
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = sizeof(float) * 3)]
    internal struct Vector3Union
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal SerializeVector3 SerializeValue;
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal System.Numerics.Vector3 Vector3;
    }
}
