using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 服务端输出创建参数
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit, Size = sizeof(int) * 4)]
    internal struct ServerBuildInfo
    {
        /// <summary>
        /// 发送数据缓冲区字节大小
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(0)]
        internal int SendBufferSize;
        /// <summary>
        /// 当前已经创建输出数量
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(4)]
        internal int Count;

        /// <summary>
        /// 是否需要发送数据
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(8)]
        internal int IsSend;
        /// <summary>
        /// 数据是否需要发送数据
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(8)]
        internal byte isFullSend;
        /// <summary>
        /// 是否需要关闭
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(9)]
        internal bool IsClose;

        /// <summary>
        /// 是否错误
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(12)]
        internal bool IsError;
        /// <summary>
        /// 是否创建了新的缓冲区
        /// </summary>
        [System.Runtime.InteropServices.FieldOffset(13)]
        internal byte IsNewBuffer;
        /// <summary>
        /// 清除数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Clear()
        {
            Count = IsSend = 0;
        }
    }
}
