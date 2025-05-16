using System;
using System.Runtime.InteropServices;

namespace AutoCSer
{
    /// <summary>
    /// Int128 / UInt128
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.JsonSerialize]
#endif
    [StructLayout(LayoutKind.Sequential, Size = sizeof(ulong) * 2)]
    internal partial struct SerializeInt128
    {
        /// <summary>
        /// 低 64b
        /// </summary>
        public ulong Lower;
        /// <summary>
        /// 高 64b
        /// </summary>
        public ulong Upper;
    }
}
