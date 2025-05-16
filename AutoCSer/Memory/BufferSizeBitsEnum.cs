using System;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 缓冲区字节大小二进制位数
    /// </summary>
    public enum BufferSizeBitsEnum : byte
    {
        /// <summary>
        /// 256B
        /// </summary>
        Byte256 = 8,
        /// <summary>
        /// 512B
        /// </summary>
        Byte512,
        /// <summary>
        /// 1KB
        /// </summary>
        Kilobyte,
        /// <summary>
        /// 2KB
        /// </summary>
        Kilobyte2,
        /// <summary>
        /// 4KB
        /// </summary>
        Kilobyte4,
        /// <summary>
        /// 8KB
        /// </summary>
        Kilobyte8,
        /// <summary>
        /// 16KB
        /// </summary>
        Kilobyte16,
        /// <summary>
        /// 32KB
        /// </summary>
        Kilobyte32,
        /// <summary>
        /// 64KB
        /// </summary>
        Kilobyte64,
        /// <summary>
        /// 128KB
        /// </summary>
        Kilobyte128,
    }
}
