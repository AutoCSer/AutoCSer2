using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 任务队列
    /// </summary>
    public sealed class TaskQueue : IDisposable
    {
        /// <summary>
        /// 线程句柄
        /// </summary>
        private readonly System.Threading.Thread threadHandle;
        /// <summary>
        /// 任务队列
        /// </summary>
        private Link<QueueTaskNode>.YieldQueue queue;
        /// <summary>
        /// 等待事件
        /// </summary>
        private OnceAutoWaitHandle waitHandle;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private bool isDisposed;
        /// <summary>
        /// 任务队列
        /// </summary>
        public TaskQueue()
        {
            waitHandle.Set(this);
            threadHandle = new System.Threading.Thread(run, AutoCSer.Threading.ThreadPool.TinyStackSize);
            threadHandle.IsBackground = true;
            threadHandle.Start();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            if (queue.IsEmpty) waitHandle.Set();
        }
        /// <summary>
        /// 任务线程处理
        /// </summary>
        private void run()
        {
            do
            {
                waitHandle.Wait();
                if (isDisposed) return;
                var value = queue.GetClear();
                do
                {
                    try
                    {
                        while (value != null)
                        {
                            value.RunTask(ref value);
                            if (isDisposed) return;
                        }
                        break;
                    }
                    catch (Exception exception)
                    {
                        AutoCSer.LogHelper.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception | LogLevelEnum.AutoCSer);
                    }
                }
                while (value != null);
            }
            while (!isDisposed);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AddOnly(QueueTaskNode node)
        {
            if (queue.IsPushHead(node)) waitHandle.Set();
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        public void Add(CommandServerCallQueueCustomNode node)
        {
            if (node.CheckQueue()) AddOnly(node);
            else throw new Exception("node.isQueue is true");
        }
    }
}
