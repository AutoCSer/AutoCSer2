using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 并发读操作线程
    /// </summary>
    internal sealed class ConcurrencyReadQueueThread : ConcurrencyReadWriteQueueThread<CommandServerCallConcurrencyReadWriteQueue>
    {
        /// <summary>
        /// 并发读操作线程
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="isNull"></param>
        internal ConcurrencyReadQueueThread(CommandServerCallConcurrencyReadWriteQueue queue, bool isNull = false) : base(queue, isNull) { }
        /// <summary>
        /// 任务线程处理
        /// </summary>
        protected override void run()
        {
            do
            {
                WaitHandle.Wait();
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
                    queue.CloseReadThread();
                    return;
                }
            }
            while (queue.Free(this));
        }
    }
}
