using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// The CPU cache is filled with data blocks. By default, it is a 64b application, and the filling is 7 * 8 = 56b
    /// CPU 高速缓存填充数据块，默认为 64b 应用，填充 7 * 8 = 56b
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct CpuCachePad
    {
        private readonly ulong pad0;
        private readonly ulong pad1;
        private readonly ulong pad2;
        private readonly ulong pad3;
        private readonly ulong pad4;
        private readonly ulong pad5;
        private readonly ulong pad6;
    }
}
