using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 定时任务继续模式
    /// </summary>
    public enum SecondTimerKeepModeEnum : byte
    {
        /// <summary>
        /// 执行之前添加新的定时任务
        /// </summary>
        Before,
        /// <summary>
        /// 执行之后添加新的定时任务
        /// </summary>
        After,
    }
}
