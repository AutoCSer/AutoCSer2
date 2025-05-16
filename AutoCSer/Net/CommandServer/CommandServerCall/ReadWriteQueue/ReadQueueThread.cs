using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 读写队列读操作线程
    /// </summary>
    internal sealed class ReadQueueThread : AutoCSer.Threading.Link<ReadQueueThread>
    {
        /// <summary>
        /// 服务端同步读写队列
        /// </summary>
        private readonly CommandServerCallWriteQueue queue;
        /// <summary>
        /// 线程句柄
        /// </summary>
        private readonly System.Threading.Thread threadHandle;
        /// <summary>
        /// 等待事件
        /// </summary>
        internal OnceAutoWaitHandle WaitHandle;
        /// <summary>
        /// 当前分配任务
        /// </summary>
        private ReadWriteQueueNode node;
        /// <summary>
        /// 读写队列读操作线程
        /// </summary>
        /// <param name="queue"></param>
        internal ReadQueueThread(CommandServerCallWriteQueue queue)
        {
            this.queue = queue;
            node = NullReadWriteQueueNode.Null;
            WaitHandle.Set(this);
            threadHandle = new System.Threading.Thread(run, AutoCSer.Threading.ThreadPool.TinyStackSize);
            threadHandle.IsBackground = true;
            threadHandle.Start();
        }
        /// <summary>
        /// 任务线程处理
        /// </summary>
        private void run()
        {
            do
            {
                WaitHandle.Wait();
                if (!queue.IsClose)
                {
                    var node = this.node;
                    do
                    {
                        try
                        {
                            node.RunTask();
                        }
                        catch (Exception exception)
                        {
                            node.OnException(queue, exception);
                        }
                        node = queue.GetRead();
                    }
                    while (node != null);
                    this.node = NullReadWriteQueueNode.Null;
                }
                else
                {
                    queue.CloseReadThread();
                    return;
                }
            }
            while (queue.Free(this));
        }
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
}
