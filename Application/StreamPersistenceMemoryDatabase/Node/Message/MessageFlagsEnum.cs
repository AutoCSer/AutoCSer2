using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Message flags
    /// 消息标记
    /// </summary>
    [Flags]
    internal enum MessageFlagsEnum : byte
    {
        /// <summary>
        /// No flag
        /// 无标记
        /// </summary>
        None = 0,
        /// <summary>
        /// Completed
        /// 已完成
        /// </summary>
        Completed = 1,
        /// <summary>
        /// Processing failed
        /// 处理失败
        /// </summary>
        Failed = 2,
        /// <summary>
        /// Timeout
        /// 超时
        /// </summary>
        Timeout = 4,
        /// <summary>
        /// Processing failure (including timeout)
        /// 处理失败（包括超时）
        /// </summary>
        FailedOrTimeout = Failed | Timeout,
        /// <summary>
        /// The snapshot ends
        /// 快照结束
        /// </summary>
        SnapshotEnd = 0x80,
    }
}
