using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 执行任务的线程模式
    /// </summary>
    public enum SecondTimerTaskThreadModeEnum : byte
    {
        /// <summary>
        /// 阻塞定时线程同步执行 OnTimer，适用于无阻塞快速结束任务避免线程调度
        /// </summary>
        Synchronous,
        /// <summary>
        /// await 阻塞定时线程同步执行 OnTimerAsync，适用于无阻塞快速结束任务避免线程调度
        /// </summary>
        WaitTask,
        /// <summary>
        /// 调用 AutoCSer.Threading.CatchTask.Add 执行 OnTimerAsync
        /// </summary>
        AddCatchTask,
    }
}
