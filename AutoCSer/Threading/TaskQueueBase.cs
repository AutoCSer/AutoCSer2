using AutoCSer.Extensions;
using System;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 任务队列
    /// </summary>
    public abstract class TaskQueueBase : IDisposable
    {
        /// <summary>
        /// 线程句柄
        /// </summary>
        protected readonly System.Threading.Thread threadHandle;
        /// <summary>
        /// 等待事件
        /// </summary>
        internal readonly System.Threading.AutoResetEvent WaitHandle;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        protected volatile bool isDisposed;
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            WaitHandle.setDispose();
        }
        /// <summary>
        /// 任务队列
        /// </summary>
        public TaskQueueBase()
        {
            WaitHandle = new System.Threading.AutoResetEvent(false);
            threadHandle = new System.Threading.Thread(run, AutoCSer.Threading.ThreadPool.TinyStackSize);
            threadHandle.IsBackground = true;
            threadHandle.Start();
        }
        /// <summary>
        /// 任务线程处理
        /// </summary>
        protected abstract void run();
    }
}
