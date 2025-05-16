using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 任务添加状态
    /// </summary>
    internal enum SecondTimerAppendTaskStateEnum : byte
    {
        /// <summary>
        /// 已经同步处理完成
        /// </summary>
        Completed,
        /// <summary>
        /// 执行之后添加新的定时任务
        /// </summary>
        After,
        /// <summary>
        /// 添加任务
        /// </summary>
        AppendTaskArray,
        /// <summary>
        /// 触发定时操作
        /// </summary>
        OnTimer,
    }
}
