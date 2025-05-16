using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 服务端异步调用队列等待类型
    /// </summary>
    internal enum CallTaskQueueWaitTypeEnum : byte
    {
        /// <summary>
        /// 当可增加并发任务数量为 0 时，等待增加并发任务数量
        /// </summary>
        Concurrent,
        /// <summary>
        /// 等待所有未完成任务执行完以后执行低优先级任务
        /// </summary>
        LowPriority,
        /// <summary>
        /// 等待低优先级任务完成
        /// </summary>
        RunLowPriority,
        /// <summary>
        /// 当没有新任务并且存在未完成任务时，等待新任务继续执行，或者等待未完成任务执行完以后释放执行任务标志
        /// </summary>
        Queue,
        ///// <summary>
        ///// 无等待，直接执行当前任务
        ///// </summary>
        //None
    }
}
