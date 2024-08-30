using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 消息标记
    /// </summary>
    [Flags]
    internal enum MessageFlagsEnum : byte
    {
        /// <summary>
        /// 无标记
        /// </summary>
        None = 0,
        /// <summary>
        /// 已完成
        /// </summary>
        Completed = 1,
        /// <summary>
        /// 处理失败
        /// </summary>
        Failed = 2,
        /// <summary>
        /// 超时
        /// </summary>
        Timeout = 4,
        /// <summary>
        /// 处理失败（包括超时）
        /// </summary>
        FailedOrTimeout = Failed | Timeout,
        /// <summary>
        /// 快照结束
        /// </summary>
        SnapshotEnd = 0x80,
    }
}
