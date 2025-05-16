using System;
using System.Runtime.InteropServices;

namespace AutoCSer
{
    /// <summary>
    /// Half / ushort
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = sizeof(ushort))]
    internal struct HalfUnion
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal Half Half;
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal ushort UShort;
    }
}
