using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions.Threading
{
    /// <summary>
    /// 并发任务
    /// </summary>
    /// <typeparam name="T">获取任务参数类型</typeparam>
    public sealed class EnumerableValueTask<T>
    {
        /// <summary>
        /// 获取任务参数集合
        /// </summary>
        private readonly IEnumerable<T> source;
        /// <summary>
        /// 获取任务委托
        /// </summary>
        private readonly Func<T, ValueTask> getTask;
        /// <summary>
        /// 任务并发锁
        /// </summary>
        private readonly SemaphoreSlim taskLock;
        /// <summary>
        /// 任务完成等待锁
        /// </summary>
        private readonly SemaphoreSlim waitLock;
        /// <summary>
        /// 当前启动任务数量
        /// </summary>
        private int taskCount;
        /// <summary>
        /// 当前完成任务数量
        /// </summary>
        private int completedCount;
        /// <summary>
        /// /当前完成任务数量
        /// </summary>
        public int CompletedCount { get { return completedCount - 1; } }
        /// <summary>
        /// 异常任务数量
        /// </summary>
        private int errorCount;
        /// <summary>
        /// 异常任务数量
        /// </summary>
        public int ErrorCount { get { return errorCount; } }
        /// <summary>
        /// 是否已经取消任务
        /// </summary>
        private bool isCancel;
        /// <summary>
        /// 并发任务
        /// </summary>
        /// <param name="source">获取任务参数集合</param>
        /// <param name="taskCount">最大并发任务数量</param>
        /// <param name="getTask">获取任务委托</param>
        internal EnumerableValueTask(IEnumerable<T> source, int taskCount, Func<T, ValueTask> getTask)
        {
            if (source == null || getTask == null) throw new ArgumentNullException();
            taskCount = Math.Max(taskCount, 1);
            taskLock = new SemaphoreSlim(taskCount, taskCount);
            waitLock = new SemaphoreSlim(0, 1);
            this.source = source;
            this.getTask = getTask;
        }
        /// <summary>
        /// 取消任务
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Cancel()
        {
            isCancel = true;
        }
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <returns></returns>
        internal async Task<EnumerableValueTask<T>> Start()
        {
            bool isRun = true;
            taskCount = 1;
            try
            {
                foreach (T value in source)
                {
                    if (!isCancel)
                    {
                        await taskLock.WaitAsync();
                        ++taskCount;
                        isRun = false;
                        run(value).NotWait();
                        isRun = true;
                    }
                }
            }
            finally
            {
                if (!isRun) --taskCount;
                if (Interlocked.Increment(ref completedCount) < taskCount) await waitLock.WaitAsync();
            }
            return this;
        }
        /// <summary>
        /// 运行任务
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private async Task run(T value)
        {
            bool isTask = false;
            try
            {
                await getTask(value);
                isTask = true;
            }
            finally
            {
                taskLock.Release();
                if (!isTask) Interlocked.Increment(ref errorCount);
                if (Interlocked.Increment(ref completedCount) >= taskCount) waitLock.Release();
            }
        }
    }
}
