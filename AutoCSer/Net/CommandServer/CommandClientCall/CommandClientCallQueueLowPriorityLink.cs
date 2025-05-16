using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 客户端执行低优先级队列
    /// </summary>
    public sealed class CommandClientCallQueueLowPriorityLink : CommandClientCallQueueNode
    {
        /// <summary>
        /// 客户端执行队列
        /// </summary>
        private readonly CommandClientCallQueue queue;
        /// <summary>
        /// 任务队列
        /// </summary>
        private LinkStack<CommandClientCallQueueNode> nodeQueue;
        /// <summary>
        /// 首节点
        /// </summary>
#if NetStandard21
        private CommandClientCallQueueNode? head;
#else
        private CommandClientCallQueueNode head;
#endif
        /// <summary>
        /// 是否已经添加到队列
        /// </summary>
        private int isQueue;
        /// <summary>
        /// 任务队列链表节点
        /// </summary>
        /// <param name="queue">任务队列</param>
        internal CommandClientCallQueueLowPriorityLink(CommandClientCallQueue queue)
        {
            this.queue = queue;
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Add(CommandClientCallQueueNode node)
        {
            if (node != null) add(node);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        private void add(CommandClientCallQueueNode node)
        {
            if (nodeQueue.IsPushHead(node) && System.Threading.Interlocked.CompareExchange(ref isQueue, 1, 0) == 0) queue.Add(this);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Add(CommandClientCallQueueLowPriorityLink queue, CommandClientCallQueueNode node)
        {
            queue.add(node);
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="queue"></param>
        public override void RunTask(CommandClientCallQueue queue)
        {
            var node = head ?? nodeQueue.GetQueue().notNull();
            if ((head = node.GetLinkNextClear()) != null)
            {
                try
                {
                    node.RunTask(queue);
                }
                finally { queue.Add(this); }
            }
            else
            {
                try
                {
                    node.RunTask(queue);
                }
                finally
                {
                    if (!nodeQueue.IsEmpty) queue.Add(this);
                    else
                    {
                        do
                        {
                            System.Threading.Interlocked.Exchange(ref isQueue, 0);
                            if (nodeQueue.IsEmpty || System.Threading.Interlocked.CompareExchange(ref isQueue, 1, 0) != 0) break;
                            if (!nodeQueue.IsEmpty)
                            {
                                queue.Add(this);
                                break;
                            }
                        }
                        while (true);
                    }
                }
            }
        }
        ///// <summary>
        ///// 执行任务
        ///// </summary>
        ///// <param name="queue"></param>
        //public override void RunTask(CommandClientCallQueue queue)
        //{
        //    var node = head.notNull();
        //    var next = node.LinkNext;
        //    if (next == null)
        //    {
        //        queueLock.EnterYield();
        //        head = next = head.notNull().LinkNext;
        //        queueLock.Exit();
        //        node.LinkNext = null;
        //        try
        //        {
        //            node.RunTask(queue);
        //        }
        //        finally
        //        {
        //            if (next != null) queue.Add(this);
        //        }
        //    }
        //    else
        //    {
        //        head = next;
        //        node.LinkNext = null;
        //        try
        //        {
        //            node.RunTask(queue);
        //        }
        //        finally { queue.Add(this); }
        //    }
        //}
    }
}
