using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 读写队列读操作线程
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal abstract class ConcurrencyReadWriteQueueThread<T> where T : CommandServerCallReadWriteQueue
    {
        /// <summary>
        /// 服务端同步读写队列
        /// </summary>
        protected readonly T queue;
        /// <summary>
        /// 线程句柄
        /// </summary>
        private readonly System.Threading.Thread threadHandle;
        /// <summary>
        /// 等待事件
        /// </summary>
        internal readonly System.Threading.AutoResetEvent WaitHandle;
        /// <summary>
        /// 当前分配任务
        /// </summary>
        protected ReadWriteQueueNode node;
        /// <summary>
        /// 读写队列读操作线程
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="isNull"></param>
        internal ConcurrencyReadWriteQueueThread(T queue, bool isNull)
        {
            this.queue = queue;
            node = NullReadWriteQueueNode.Null;
            WaitHandle = new System.Threading.AutoResetEvent(false);
            if (!isNull)
            {
                threadHandle = new System.Threading.Thread(run, AutoCSer.Threading.ThreadPool.TinyStackSize);
                threadHandle.IsBackground = true;
                threadHandle.Start();
            }
#if NetStandard21
            else
            {
                threadHandle = AutoCSer.Threading.ThreadPool.BackgroundExitThread.Handle;
            }
#endif
        }
        /// <summary>
        /// 任务线程处理
        /// </summary>
        protected abstract void run();
        /// <summary>
        /// 分配任务
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(ReadWriteQueueNode node)
        {
            this.node = node;
            WaitHandle.Set();
        }
    }
    /// <summary>
    /// 并发读操作线程
    /// </summary>
    internal sealed class ConcurrencyReadWriteQueueThread : ConcurrencyReadWriteQueueThread<CommandServerCallWriteQueue>
    {
        /// <summary>
        /// 读写队列读操作线程
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="isNull"></param>
        internal ConcurrencyReadWriteQueueThread(CommandServerCallWriteQueue queue, bool isNull = false) : base(queue, isNull) { }
        /// <summary>
        /// 任务线程处理
        /// </summary>
        protected override void run()
        {
            do
            {
                WaitHandle.WaitOne();
                if (!queue.IsClose)
                {
                    try
                    {
                        node.RunTask();
                    }
                    catch (Exception exception)
                    {
                        node.OnException(queue, exception);
                    }
                    node = NullReadWriteQueueNode.Null;
                }
                else
                {
                    queue.CloseConcurrencyReadThread();
                    return;
                }
            }
            while (queue.Free(this));
        }
    }
}
