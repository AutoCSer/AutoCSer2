using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 二维秒级定时同步任务节点
    /// </summary>
    public class SecondTimerArrayNode : DoubleLink<SecondTimerArrayNode>
    {
        /// <summary>
        /// 二维定时任务数组
        /// </summary>
        private readonly SecondTimerArray taskArray;
        /// <summary>
        /// 继续执行间隔秒数，小于等于 0 表示不继续执行
        /// </summary>
        protected internal int KeepSeconds;
        /// <summary>
        /// 定时任务继续模式
        /// </summary>
        internal SecondTimerKeepModeEnum KeepMode;
        /// <summary>
        /// 超时
        /// </summary>
        internal long TimeoutSeconds;
        /// <summary>
        /// 二维秒级定时任务节点
        /// </summary>
        protected SecondTimerArrayNode() 
        {
            taskArray = AutoCSer.Threading.SecondTimer.InternalTaskArray;
        }
        /// <summary>
        /// 二维秒级定时任务节点
        /// </summary>
        /// <param name="taskArray">二维定时任务数组</param>
        /// <param name="timeoutSeconds">第一次执行任务间隔的秒数</param>
        /// <param name="keepMode">定时任务继续模式</param>
        /// <param name="keepSeconds">继续执行间隔秒数，0 表示不继续执行</param>
        internal SecondTimerArrayNode(SecondTimerArray taskArray, int timeoutSeconds, SecondTimerKeepModeEnum keepMode, int keepSeconds)
        {
            this.taskArray = taskArray;
            this.KeepSeconds = Math.Max(keepSeconds, 0);
            TimeoutSeconds = SecondTimer.CurrentSeconds + Math.Max(timeoutSeconds, 0);
            this.KeepMode = keepMode;
        }
        /// <summary>
        /// 二维秒级定时任务节点
        /// </summary>
        /// <param name="taskArray">二维定时任务数组</param>
        /// <param name="keepMode">定时任务继续模式</param>
        internal SecondTimerArrayNode(SecondTimerArray taskArray, SecondTimerKeepModeEnum keepMode)
        {
            this.taskArray = taskArray;
            this.KeepMode = keepMode;
        }
        /// <summary>
        /// 二维秒级定时任务节点
        /// </summary>
        /// <param name="taskArray">二维定时任务数组</param>
        /// <param name="timeoutSeconds">执行任务时间</param>
        internal SecondTimerArrayNode(SecondTimerArray taskArray, long timeoutSeconds)
        {
            this.taskArray = taskArray;
            TimeoutSeconds = timeoutSeconds + SecondTimer.CurrentSeconds;
        }
        /// <summary>
        /// 获取下一个超时秒计数
        /// </summary>
        /// <returns>0 表示不再继续</returns>
        protected virtual long getNextTimeoutSeconds()
        {
            if (KeepSeconds <= 0) return 0;
            if (KeepMode == SecondTimerKeepModeEnum.Before) return TimeoutSeconds + KeepSeconds;
            return SecondTimer.CurrentSeconds + KeepSeconds;
        }
        /// <summary>
        /// 任务添加到二维定时任务数组
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AppendTaskArray()
        {
            taskArray.Append(this);
        }
        /// <summary>
        /// 任务添加到二维定时任务数组
        /// </summary>
        /// <param name="keepSeconds">继续执行间隔秒数</param>
        internal void AppendTaskArray(int keepSeconds)
        {
            this.KeepSeconds = Math.Max(keepSeconds, 0);
            TimeoutSeconds = SecondTimer.CurrentSeconds + this.KeepSeconds;
            taskArray.Append(this);
        }
        /// <summary>
        /// 添加任务直接触发定时操作
        /// </summary>
        internal void AppendCall()
        {
            switch (KeepMode)
            {
                case SecondTimerKeepModeEnum.After: After(); return;
                case SecondTimerKeepModeEnum.Before:
                    TimeoutSeconds = getNextTimeoutSeconds();
                    if (TimeoutSeconds != 0) taskArray.Append(this);
                    OnTimer();
                    return;
            }
        }
        /// <summary>
        /// 执行之后添加新的定时任务
        /// </summary>
        internal void After()
        {
            try
            {
                OnTimer();
            }
            finally
            {
                TimeoutSeconds = getNextTimeoutSeconds();
                if (TimeoutSeconds != 0) taskArray.Append(this);
            }
        }
        /// <summary>
        /// 触发定时任务并返回下一个节点
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal void Call(ref SecondTimerArrayNode? next)
#else
        internal void Call(ref SecondTimerArrayNode next)
#endif
        {
            next = DoubleLinkNext;
            switch (KeepMode)
            {
                case SecondTimerKeepModeEnum.After:
                    ResetDoubleLink();
                    After();
                    return;
                case SecondTimerKeepModeEnum.Before:
                    TimeoutSeconds = getNextTimeoutSeconds();
                    if (TimeoutSeconds != 0)
                    {
                        ResetDoubleLink();
                        taskArray.Append(this);
                    }
                    OnTimer();
                    return;
            }
        }
        /// <summary>
        /// 触发定时操作
        /// </summary>
        protected internal virtual void OnTimer() { }

        /// <summary>
        /// 默认空节点
        /// </summary>
        internal static readonly SecondTimerArrayNode Null = new SecondTimerArrayNode();
    }
}
