using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 二维秒级定时委托任务节点
    /// </summary>
    internal sealed class SecondTimerArrayActionNode : SecondTimerArrayNode
    {
        /// <summary>
        /// 委托任务
        /// </summary>
        private Action task;
        /// <summary>
        /// 二维秒级定时委托任务节点
        /// </summary>
        /// <param name="task">委托任务</param>
        /// <param name="taskArray">二维定时任务数组</param>
        /// <param name="timeoutSeconds">第一次执行任务间隔的秒数</param>
        /// <param name="keepMode">定时任务继续模式</param>
        /// <param name="keepSeconds">继续执行间隔秒数，0 表示不继续执行</param>
        internal SecondTimerArrayActionNode(Action task, SecondTimerArray taskArray, int timeoutSeconds, SecondTimerKeepModeEnum keepMode, int keepSeconds)
            : base(taskArray, timeoutSeconds, keepMode, keepSeconds)
        {
            this.task = task;
        }
        /// <summary>
        /// Trigger the timed operation
        /// 触发定时操作
        /// </summary>
        protected internal override void OnTimer()
        {
            task();
        }
    }
}
