using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// Two-dimensional second-level timing task node
    /// 二维秒级定时任务节点
    /// </summary>
    public class SecondTimerTaskArrayNode : DoubleLink<SecondTimerTaskArrayNode>
    {
        /// <summary>
        /// Two-dimensional array of scheduled tasks
        /// 二维定时任务数组
        /// </summary>
        private readonly SecondTimerTaskArray taskArray;
        /// <summary>
        /// The number of seconds between the continuation of execution. If it is less than or equal to 0, it indicates that the execution will not continue
        /// 继续执行间隔秒数，小于等于 0 表示不继续执行
        /// </summary>
        protected internal int KeepSeconds;
        /// <summary>
        /// The thread mode for executing tasks
        /// 执行任务的线程模式
        /// </summary>
        private readonly SecondTimerTaskThreadModeEnum threadMode;
        /// <summary>
        /// The scheduled task continues to execute mode
        /// 定时任务继续执行模式
        /// </summary>
        protected internal SecondTimerKeepModeEnum KeepMode;
        /// <summary>
        /// Has the task been added to the task array
        /// 是否已经添加任务到任务数组
        /// </summary>
        private bool isTryAppended;
        /// <summary>
        /// Internal reserved fields of AutoCSer
        /// AutoCSer 内部保留字段
        /// </summary>
        internal byte Reserved;
        /// <summary>
        /// Timeout second count
        /// 超时秒计数
        /// </summary>
        internal long TimeoutSeconds;
        /// <summary>
        /// Two-dimensional second-level timing task node
        /// 二维秒级定时任务节点
        /// </summary>
        protected SecondTimerTaskArrayNode() 
        {
            taskArray = SecondTimer.TaskArray;
        }
        /// <summary>
        /// Two-dimensional second-level timing task node
        /// 二维秒级定时任务节点
        /// </summary>
        /// <param name="taskArray">Two-dimensional array of scheduled tasks
        /// 二维定时任务数组</param>
        /// <param name="threadMode">The thread mode for executing tasks
        /// 执行任务的线程模式</param>
        /// <param name="KeepMode">The scheduled task continues to execute mode
        /// 定时任务继续执行模式</param>
        public SecondTimerTaskArrayNode(SecondTimerTaskArray taskArray, SecondTimerTaskThreadModeEnum threadMode, SecondTimerKeepModeEnum KeepMode = SecondTimerKeepModeEnum.Before)
        {
            this.taskArray = taskArray;
            this.threadMode = threadMode;
            this.KeepMode = KeepMode;
        }
        /// <summary>
        /// Two-dimensional second-level timing task node
        /// 二维秒级定时任务节点
        /// </summary>
        /// <param name="taskArray">Two-dimensional array of scheduled tasks
        /// 二维定时任务数组</param>
        /// <param name="timeoutSeconds">The number of seconds between the first task execution
        /// 第一次执行任务间隔的秒数</param>
        /// <param name="threadMode">The thread mode for executing tasks
        /// 执行任务的线程模式</param>
        /// <param name="KeepMode">The scheduled task continues to execute mode
        /// 定时任务继续执行模式</param>
        /// <param name="keepSeconds">The number of seconds for the continuation of execution, with 0 indicating no continuation of execution
        /// 继续执行间隔秒数，0 表示不继续执行</param>
        public SecondTimerTaskArrayNode(SecondTimerTaskArray taskArray, int timeoutSeconds, SecondTimerTaskThreadModeEnum threadMode, SecondTimerKeepModeEnum KeepMode = SecondTimerKeepModeEnum.Before, int keepSeconds = 0)
        {
            this.KeepSeconds = Math.Max(keepSeconds, 0);
            this.taskArray = taskArray;
            TimeoutSeconds = SecondTimer.CurrentSeconds + Math.Max(timeoutSeconds, 0);
            this.threadMode = threadMode;
            this.KeepMode = KeepMode;
        }
        /// <summary>
        /// Get the next timeout second count
        /// 获取下一个超时秒计数
        /// </summary>
        /// <returns>0 indicates no further continuation
        /// 0 表示不再继续</returns>
        protected virtual long getNextTimeoutSeconds()
        {
            if (KeepSeconds <= 0) return 0;
            if (KeepMode == SecondTimerKeepModeEnum.Before) return TimeoutSeconds + KeepSeconds;
            return SecondTimer.CurrentSeconds + KeepSeconds;
        }
        /// <summary>
        /// The task is added to the two-dimensional scheduled task array
        /// 任务添加到二维定时任务数组
        /// </summary>
        /// <param name="keepSeconds">Continue the execution interval in seconds
        /// 继续执行间隔秒数</param>
        public async Task<bool> TryAppendTaskArrayAsync(int keepSeconds)
        {
            this.KeepSeconds = Math.Max(keepSeconds, 0);
            TimeoutSeconds = SecondTimer.CurrentSeconds + this.KeepSeconds;
            return await TryAppendTaskArrayAsync();
        }
        /// <summary>
        /// Try to add to the task array
        /// 尝试添加到任务数组
        /// </summary>
        /// <returns></returns>
        public TaskCastAwaiter<bool> TryAppendTaskArrayAsync()
        {
            if (!isTryAppended)
            {
                isTryAppended = true;
                if (DoubleLinkNext == null && DoubleLinkPrevious == null)
                {
                    SecondTimerAppendTaskStateEnum appendTaskState = TryAppendTaskArray();
                    if (appendTaskState != SecondTimerAppendTaskStateEnum.Completed) return new TaskReturnAwaiter<bool>(AppendTaskArrayAsync(appendTaskState), true);
                    return AutoCSer.Common.GetCompletedAwaiter(true);
                }
            }
            return CompletedTaskCastAwaiter<bool>.Default;
        }
        /// <summary>
        /// Try to add to the task array
        /// 尝试添加到任务数组
        /// </summary>
        /// <returns></returns>
        internal SecondTimerAppendTaskStateEnum TryAppendTaskArray()
        {
            isTryAppended = true;
            if (!taskArray.Append(this))
            {
                switch (KeepMode)
                {
                    case SecondTimerKeepModeEnum.After:
                        switch (threadMode)
                        {
                            case SecondTimerTaskThreadModeEnum.Synchronous: After(); return SecondTimerAppendTaskStateEnum.Completed;
                            case SecondTimerTaskThreadModeEnum.WaitTask:  return SecondTimerAppendTaskStateEnum.After;
                            case SecondTimerTaskThreadModeEnum.AddCatchTask: AfterAsync().Catch(); return SecondTimerAppendTaskStateEnum.Completed;
                                //case SecondTimerTaskThreadModeEnum.AddCatchTask: yieldAfter().Catch(); return SecondTimerAppendTaskStateEnum.Completed;
                        }
                        return SecondTimerAppendTaskStateEnum.Completed;
                    case SecondTimerKeepModeEnum.Before:
                        TimeoutSeconds = getNextTimeoutSeconds();
                        if (TimeoutSeconds != 0) return SecondTimerAppendTaskStateEnum.AppendTaskArray;
                        switch (threadMode)
                        {
                            case SecondTimerTaskThreadModeEnum.Synchronous: OnTimer(); return SecondTimerAppendTaskStateEnum.Completed;
                            case SecondTimerTaskThreadModeEnum.WaitTask: return SecondTimerAppendTaskStateEnum.OnTimer;
                            case SecondTimerTaskThreadModeEnum.AddCatchTask: OnTimerAsync().Catch(); return SecondTimerAppendTaskStateEnum.Completed;
                                //case SecondTimerTaskThreadModeEnum.AddCatchTask: yieldOnTimer().Catch(); return SecondTimerAppendTaskStateEnum.Completed;
                        }
                        return SecondTimerAppendTaskStateEnum.Completed;
                }
            }
            return SecondTimerAppendTaskStateEnum.Completed;
        }
        ///// <summary>
        ///// 任务添加到二维定时任务数组
        ///// </summary>
        ///// <param name="appendTaskState"></param>
        ///// <returns></returns>
        //private async Task<bool> appendTaskArrayAsync(SecondTimerAppendTaskStateEnum appendTaskState)
        //{
        //    await AppendTaskArrayAsync(appendTaskState);
        //    return true;
        //}
        /// <summary>
        /// The task is added to the two-dimensional scheduled task array
        /// 任务添加到二维定时任务数组
        /// </summary>
        /// <param name="appendTaskState"></param>
        /// <returns></returns>
        internal async Task AppendTaskArrayAsync(SecondTimerAppendTaskStateEnum appendTaskState)
        {
            switch (appendTaskState)
            {
                case SecondTimerAppendTaskStateEnum.After: await AfterAsync(); return;
                case SecondTimerAppendTaskStateEnum.AppendTaskArray:
                    appendTaskState = TryAppendTaskArray();
                    if (appendTaskState != SecondTimerAppendTaskStateEnum.Completed)
                    {
                        //await Task.Yield();
                        await AppendTaskArrayAsync(appendTaskState);
                    }
                    switch (threadMode)
                    {
                        case SecondTimerTaskThreadModeEnum.Synchronous: OnTimer(); return;
                        case SecondTimerTaskThreadModeEnum.WaitTask:
                            //if (appendTaskState == SecondTimerAppendTaskStateEnum.Completed) await Task.Yield();
                            await OnTimerAsync();
                            return;
                        case SecondTimerTaskThreadModeEnum.AddCatchTask: OnTimerAsync().Catch(); return;
                            //case SecondTimerTaskThreadModeEnum.AddCatchTask: yieldOnTimer().Catch(); return;
                    }
                    return;
                case SecondTimerAppendTaskStateEnum.OnTimer: await OnTimerAsync(); return;
            }
        }
        /// <summary>
        /// Try to add to the task array
        /// 尝试添加到任务数组
        /// </summary>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public bool TryAppendTaskArray(int timeoutSeconds)
        {
            if (!isTryAppended)
            {
                isTryAppended = true;
                if (DoubleLinkNext == null && DoubleLinkPrevious == null)
                {
                    TimeoutSeconds = SecondTimer.CurrentSeconds + timeoutSeconds;
                    AppendTaskArray();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// The task is added to the two-dimensional scheduled task array
        /// 任务添加到二维定时任务数组
        /// </summary>
        /// <returns></returns>
        internal void AppendTaskArray()
        {
            isTryAppended = true;
            if (!taskArray.Append(this))
            {
                switch (KeepMode)
                {
                    case SecondTimerKeepModeEnum.After:
                        switch (threadMode)
                        {
                            case SecondTimerTaskThreadModeEnum.Synchronous: After(); return;
                            case SecondTimerTaskThreadModeEnum.WaitTask:
                            case SecondTimerTaskThreadModeEnum.AddCatchTask: AfterAsync().Catch(); return;
                                //case SecondTimerTaskThreadModeEnum.AddCatchTask: yieldAfter().Catch(); return;
                        }
                        return;
                    case SecondTimerKeepModeEnum.Before:
                        TimeoutSeconds = getNextTimeoutSeconds();
                        if (TimeoutSeconds != 0) AppendTaskArray();
                        switch (threadMode)
                        {
                            case SecondTimerTaskThreadModeEnum.Synchronous: OnTimer(); return;
                            case SecondTimerTaskThreadModeEnum.WaitTask:
                            case SecondTimerTaskThreadModeEnum.AddCatchTask: OnTimerAsync().Catch(); return;
                                //case SecondTimerTaskThreadModeEnum.AddCatchTask: yieldOnTimer().Catch(); return;
                        }
                        return;
                }
            }
        }
        ///// <summary>
        ///// 执行之后添加新的定时任务
        ///// </summary>
        ///// <returns></returns>
        //private async Task yieldAfter()
        //{
        //    await Task.Yield();
        //    await AfterAsync();
        //}
        /// <summary>
        /// Add a new scheduled task after the task is executed
        /// 任务执行之后添加新的定时任务
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
                if (TimeoutSeconds != 0) AppendTaskArray();
            }
        }
        /// <summary>
        /// Add a new scheduled task after the task is executed
        /// 任务执行之后添加新的定时任务
        /// </summary>
        /// <returns></returns>
        internal async Task AfterAsync()
        {
            try
            {
                await OnTimerAsync();
            }
            finally
            {
                TimeoutSeconds = getNextTimeoutSeconds();
                if (TimeoutSeconds != 0)
                {
                    SecondTimerAppendTaskStateEnum appendTaskState = TryAppendTaskArray();
                    if (appendTaskState != SecondTimerAppendTaskStateEnum.Completed) await AppendTaskArrayAsync(appendTaskState);
                }
            }
        }
        /// <summary>
        /// Try to trigger the scheduled task
        /// 尝试触发定时任务
        /// </summary>
        /// <param name="appendTaskState"></param>
        /// <returns></returns>
        internal SecondTimerAppendTaskStateEnum TryCall(ref SecondTimerAppendTaskStateEnum appendTaskState)
        {
            switch (KeepMode)
            {
                case SecondTimerKeepModeEnum.After:
                    ResetDoubleLink();
                    switch (threadMode)
                    {
                        case SecondTimerTaskThreadModeEnum.Synchronous: OnTimer(); return SecondTimerAppendTaskStateEnum.Completed;
                        case SecondTimerTaskThreadModeEnum.WaitTask: return SecondTimerAppendTaskStateEnum.After;
                        case SecondTimerTaskThreadModeEnum.AddCatchTask: AfterAsync().Catch(); return SecondTimerAppendTaskStateEnum.Completed;
                            //case SecondTimerTaskThreadModeEnum.AddCatchTask: yieldAfter().Catch(); return SecondTimerAppendTaskStateEnum.Completed;
                    }
                    return SecondTimerAppendTaskStateEnum.Completed;
                case SecondTimerKeepModeEnum.Before:
                    TimeoutSeconds = getNextTimeoutSeconds();
                    if (TimeoutSeconds != 0)
                    {
                        ResetDoubleLink();
                        appendTaskState = TryAppendTaskArray();
                        if (appendTaskState != SecondTimerAppendTaskStateEnum.Completed) return SecondTimerAppendTaskStateEnum.AppendTaskArray;
                    }
                    switch (threadMode)
                    {
                        case SecondTimerTaskThreadModeEnum.Synchronous: OnTimer(); return SecondTimerAppendTaskStateEnum.Completed;
                        case SecondTimerTaskThreadModeEnum.WaitTask: return SecondTimerAppendTaskStateEnum.OnTimer;
                        case SecondTimerTaskThreadModeEnum.AddCatchTask: OnTimerAsync().Catch(); return SecondTimerAppendTaskStateEnum.Completed;
                            //case SecondTimerTaskThreadModeEnum.AddCatchTask: yieldOnTimer().Catch(); return SecondTimerAppendTaskStateEnum.Completed;
                    }
                    return SecondTimerAppendTaskStateEnum.Completed;
            }
            return SecondTimerAppendTaskStateEnum.Completed;
        }
        /// <summary>
        /// Trigger the scheduled task
        /// 触发定时任务
        /// </summary>
        /// <param name="callTaskState"></param>
        /// <param name="appendTaskState"></param>
        /// <returns></returns>
        internal async Task Call(SecondTimerAppendTaskStateEnum callTaskState, SecondTimerAppendTaskStateEnum appendTaskState)
        {
            switch (callTaskState)
            {
                case SecondTimerAppendTaskStateEnum.After: await AfterAsync(); return;
                case SecondTimerAppendTaskStateEnum.AppendTaskArray:
                    await AppendTaskArrayAsync(appendTaskState);
                    switch (threadMode)
                    {
                        case SecondTimerTaskThreadModeEnum.Synchronous: OnTimer(); return;
                        case SecondTimerTaskThreadModeEnum.WaitTask: await OnTimerAsync(); return;
                        case SecondTimerTaskThreadModeEnum.AddCatchTask: OnTimerAsync().Catch(); return;
                            //case SecondTimerTaskThreadModeEnum.AddCatchTask: yieldOnTimer().Catch(); return;
                    }
                    return;
                case SecondTimerAppendTaskStateEnum.OnTimer: await OnTimerAsync(); return;
            }
        }
        ///// <summary>
        ///// 触发定时操作
        ///// </summary>
        ///// <returns></returns>
        //private async Task yieldOnTimer()
        //{
        //    await Task.Yield();
        //    await OnTimerAsync();
        //}
        /// <summary>
        /// Trigger the timed operation
        /// 触发定时操作
        /// </summary>
        /// <returns></returns>
        protected internal virtual void OnTimer() { }
        /// <summary>
        /// Trigger the timed operation
        /// 触发定时操作
        /// </summary>
        /// <returns></returns>
        protected internal virtual Task OnTimerAsync() { return AutoCSer.Common.CompletedTask; }

        /// <summary>
        /// Default empty node
        /// </summary>
        internal static readonly SecondTimerTaskArrayNode Null = new SecondTimerTaskArrayNode();
    }
}
