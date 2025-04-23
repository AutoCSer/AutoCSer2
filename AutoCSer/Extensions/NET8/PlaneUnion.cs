using System;
using System.Runtime.InteropServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Plane
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = sizeof(float) * 4)]
    internal struct PlaneUnion
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal SerializePlane SerializeValue;
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal System.Numerics.Plane Plane;
    }
}
