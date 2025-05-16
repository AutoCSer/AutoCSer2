using System;
using System.Runtime.InteropServices;

namespace AutoCSer
{
    /// <summary>
    /// Int128 / UInt128
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = sizeof(ulong) * 2)]
    internal struct Int128Union
    {
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal SerializeInt128 SerializeValue;
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal UInt128 UInt128;
        /// <summary>
        /// 
        /// </summary>
        [FieldOffset(0)]
        internal Int128 Int128;
    }
}
