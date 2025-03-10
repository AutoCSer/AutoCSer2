using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 并发队列（相当于读写锁）
    /// </summary>
    public sealed class ConcurrencyQueue
    {
        /// <summary>
        /// 读操作队列首节点
        /// </summary>
#if NetStandard21
        private ConcurrencyQueueTaskNode? head;
#else
        private ConcurrencyQueueTaskNode head;
#endif
        /// <summary>
        /// 读操作队列尾节点
        /// </summary>
#if NetStandard21
        private ConcurrencyQueueTaskNode? end;
#else
        private ConcurrencyQueueTaskNode end;
#endif
        /// <summary>
        /// 最大读取操作并发数量
        /// </summary>
        private readonly int maxConcurrency;
        /// <summary>
        /// 当前读取操作并发数量
        /// </summary>
        private int currentConcurrency;
        /// <summary>
        /// 队列访问锁
        /// </summary>
        private OnceAutoWaitHandle queueLock;
        /// <summary>
        /// 并发操作锁
        /// </summary>
        private readonly object concurrencyLock;
        /// <summary>
        /// 是否存在写操作等待队列访问锁
        /// </summary>
        private bool isWaitQueue;
        /// <summary>
        /// 并发队列（相当于读写锁）
        /// </summary>
        /// <param name="maxConcurrency">最大读取操作并发数量</param>
        public ConcurrencyQueue(int maxConcurrency)
        {
            this.maxConcurrency = maxConcurrency > 0 ? maxConcurrency : AutoCSer.Common.ProcessorCount;
            queueLock.Set(new object(), 1);
            concurrencyLock = new object();
        }
        /// <summary>
        /// 写操作等待队列（必须在数据库执行队列中调用）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WaitQueue()
        {
            isWaitQueue = true;
            queueLock.Wait();
        }
        /// <summary>
        /// 写操作释放队列（必须在数据库执行队列中调用）
        /// </summary>
        public void ReleaseQueue()
        {
            isWaitQueue = false;
            if (head != null)
            {
                queueLock.IsWait = 0;
                Monitor.Enter(concurrencyLock);
                try
                {
                    do
                    {
                        ++currentConcurrency;
                        AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(head.RunTask);
                        head = head.LinkNext;
                        if (head == null)
                        {
                            end = null;
                            return;
                        }
                    }
                    while (currentConcurrency != maxConcurrency);
                }
                finally { Monitor.Exit(concurrencyLock); }
            }
            else queueLock.IsWait = 1;
        }
        /// <summary>
        /// 添加读操作（必须在数据库执行队列中调用）
        /// </summary>
        /// <param name="node"></param>
        public void Push(ConcurrencyQueueTaskNode node)
        {
            bool isRunTask = false;
            Monitor.Enter(concurrencyLock);
            if (currentConcurrency != maxConcurrency)
            {
                if (currentConcurrency == 0) queueLock.IsWait = 0;
                isRunTask = true;
                ++currentConcurrency;
            }
            else
            {
                if (end == null) head = node;
                else end.LinkNext = node;
                end = node;
            }
            Monitor.Exit(concurrencyLock);
            if (isRunTask) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(node.RunTask);
        }
        /// <summary>
        /// 尝试从队列中执行下一个任务
        /// </summary>
        internal void CheckNext()
        {
            var node = default(ConcurrencyQueueTaskNode);
            bool isSetQueueLock = false;
            Monitor.Enter(concurrencyLock);
            if (!isWaitQueue && head != null)
            {
                node = head;
                head = head.LinkNext;
                if (head == null) end = null;
            }
            else if (--currentConcurrency == 0) isSetQueueLock = true;
            Monitor.Exit(concurrencyLock);
            if (isSetQueueLock) queueLock.Set();
            else if (node != null) AutoCSer.Threading.ThreadPool.TinyBackground.FastStart(node.RunTask);
        }
    }
}
