using System;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 非托管内存数据流切换数据缓冲区信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct UnmanagedStreamExchangeBuffer
    {
        /// <summary>
        /// 非托管内存池指针
        /// </summary>
        internal UnmanagedPoolPointer Data;
        /// <summary>
        /// 是否非托管内存数据
        /// </summary>
        internal bool IsUnmanaged;
        /// <summary>
        /// 是否允许扩展缓存区大小
        /// </summary>
        internal bool CanResize;
        /// <summary>
        /// 在不允许扩展缓存区大小的情况下是否产生了扩展操作
        /// </summary>
        internal bool IsResizeError;
        /// <summary>
        /// 字符串二进制序列化直接复制内存数据
        /// </summary>
        internal bool IsSerializeCopyString;
        /// <summary>
        /// 非托管内存数据流切换数据缓冲区信息
        /// </summary>
        /// <param name="stream"></param>
        internal UnmanagedStreamExchangeBuffer(UnmanagedStreamBase stream)
        {
            Data = stream.Data;
            IsUnmanaged = stream.IsUnmanaged;
            CanResize = stream.CanResize;
            IsResizeError = stream.IsResizeError;
            IsSerializeCopyString = stream.IsSerializeCopyString;
        }
    }
}
