using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 二维秒级定时任务节点
    /// </summary>
    public class SecondTimerTaskArrayNode : DoubleLink<SecondTimerTaskArrayNode>
    {
        /// <summary>
        /// 二维定时任务数组
        /// </summary>
        private readonly SecondTimerTaskArray taskArray;
        /// <summary>
        /// 继续执行间隔秒数，小于等于 0 表示不继续执行
        /// </summary>
        protected internal int KeepSeconds;
        /// <summary>
        /// 执行任务的线程模式
        /// </summary>
        private readonly SecondTimerTaskThreadModeEnum threadMode;
        /// <summary>
        /// 定时任务继续模式
        /// </summary>
        protected internal SecondTimerKeepModeEnum KeepMode;
        /// <summary>
        /// 是否已经添加任务到任务数组
        /// </summary>
        private bool isTryAppended;
        /// <summary>
        /// 超时秒计数
        /// </summary>
        internal long TimeoutSeconds;
        /// <summary>
        /// 二维秒级定时任务节点
        /// </summary>
        protected SecondTimerTaskArrayNode() 
        {
            taskArray = SecondTimer.TaskArray;
        }
        /// <summary>
        /// 二维秒级定时任务节点
        /// </summary>
        /// <param name="taskArray">二维定时任务数组</param>
        /// <param name="threadMode">执行任务的线程模式</param>
        /// <param name="KeepMode">定时任务继续模式</param>
        public SecondTimerTaskArrayNode(SecondTimerTaskArray taskArray, SecondTimerTaskThreadModeEnum threadMode, SecondTimerKeepModeEnum KeepMode = SecondTimerKeepModeEnum.Before)
        {
            this.taskArray = taskArray;
            this.threadMode = threadMode;
            this.KeepMode = KeepMode;
        }
        /// <summary>
        /// 二维秒级定时任务节点
        /// </summary>
        /// <param name="taskArray">二维定时任务数组</param>
        /// <param name="timeoutSeconds">第一次执行任务间隔的秒数</param>
        /// <param name="threadMode">执行任务的线程模式</param>
        /// <param name="KeepMode">定时任务继续模式</param>
        /// <param name="keepSeconds">继续执行间隔秒数，0 表示不继续执行</param>
        public SecondTimerTaskArrayNode(SecondTimerTaskArray taskArray, int timeoutSeconds, SecondTimerTaskThreadModeEnum threadMode, SecondTimerKeepModeEnum KeepMode = SecondTimerKeepModeEnum.Before, int keepSeconds = 0)
        {
            this.KeepSeconds = Math.Max(keepSeconds, 0);
            this.taskArray = taskArray;
            TimeoutSeconds = SecondTimer.CurrentSeconds + Math.Max(timeoutSeconds, 0);
            this.threadMode = threadMode;
            this.KeepMode = KeepMode;
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
        /// <param name="keepSeconds">继续执行间隔秒数</param>
        public async Task<bool> TryAppendTaskArrayAsync(int keepSeconds)
        {
            this.KeepSeconds = Math.Max(keepSeconds, 0);
            TimeoutSeconds = SecondTimer.CurrentSeconds + this.KeepSeconds;
            return await TryAppendTaskArrayAsync();
        }
        /// <summary>
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
                if (TimeoutSeconds != 0) AppendTaskArray();
            }
        }
        /// <summary>
        /// 执行之后添加新的定时任务
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
        /// 触发定时操作
        /// </summary>
        /// <returns></returns>
        protected internal virtual void OnTimer() { }
        /// <summary>
        /// 触发定时操作
        /// </summary>
        /// <returns></returns>
        protected internal virtual Task OnTimerAsync() { return AutoCSer.Common.CompletedTask; }

        /// <summary>
        /// 默认空节点
        /// </summary>
        internal static readonly SecondTimerTaskArrayNode Null = new SecondTimerTaskArrayNode();
    }
}
