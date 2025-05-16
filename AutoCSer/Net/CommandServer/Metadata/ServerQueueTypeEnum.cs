using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 队列类型
    /// </summary>
    internal enum ServerQueueTypeEnum : byte
    {
        /// <summary>
        /// 无队列
        /// </summary>
        None,
        /// <summary>
        /// 异步队列
        /// </summary>
        TaskQueue,
        /// <summary>
        /// 同步线程队列
        /// </summary>
        Queue,
        /// <summary>
        /// 并行并发读的同步队列
        /// </summary>
        ConcurrencyReadQueue,
        /// <summary>
        /// 读写队列
        /// </summary>
        ReadWriteQueue,
    }
}
