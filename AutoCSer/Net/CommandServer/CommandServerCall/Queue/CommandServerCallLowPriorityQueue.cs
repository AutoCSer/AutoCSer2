using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端执行低优先级队列
    /// </summary>
    public sealed class CommandServerCallLowPriorityQueue : CommandServerCallQueueNode
    {
        /// <summary>
        /// 服务端执行队列
        /// </summary>
        public readonly CommandServerCallQueue Queue;
        /// <summary>
        /// 任务队列
        /// </summary>
        private LinkStack<QueueTaskNode> nodeQueue;
        /// <summary>
        /// 首节点
        /// </summary>
#if NetStandard21
        private QueueTaskNode? head;
#else
        private QueueTaskNode head;
#endif
        /// <summary>
        /// 当前执行任务
        /// </summary>
        private QueueTaskNode currentTask;
        /// <summary>
        /// 是否已经添加到队列
        /// </summary>
        private int isQueue;
        /// <summary>
        /// 任务队列链表节点
        /// </summary>
        /// <param name="queue">任务队列</param>
        internal CommandServerCallLowPriorityQueue(CommandServerCallQueue queue)
        {
            this.Queue = queue;
            currentTask = ActionQueueTaskNode.Empty;
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        public void Add(CommandServerCallQueueCustomNode node)
        {
            if (node.CheckQueue()) add(node);
            else throw new Exception("node.isQueue is true");
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AddOnly(CommandServerCallQueueNode node)
        {
            if (node != null) add(node);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        private void add(QueueTaskNode node)
        {
            if (nodeQueue.IsPushHead(node) && System.Threading.Interlocked.CompareExchange(ref isQueue, 1, 0) == 0) Queue.AddOnly(this);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Add(CommandServerCallLowPriorityQueue queue, CommandServerCallQueueNode node)
        {
            queue.add(node);
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CommandClientReturnTypeEnum AddIsDeserialize(CommandServerCallLowPriorityQueue queue, CommandServerCallQueueNode node)
        {
            if (node.IsDeserialize)
            {
                queue.add(node);
                return CommandClientReturnTypeEnum.Success;
            }
            return CommandClientReturnTypeEnum.ServerDeserializeError;
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        public override void RunTask()
        {
            currentTask = head ?? nodeQueue.GetQueue().notNull();
            if ((head = currentTask.GetLinkNextClear()) != null)
            {
                try
                {
                    currentTask.ClearLinkRunTask();
                }
                finally { Queue.AddOnly(this); }
            }
            else
            {
                try
                {
                    currentTask.ClearLinkRunTask();
                }
                finally
                {
                    if (!nodeQueue.IsEmpty) Queue.AddOnly(this);
                    else
                    {
                        do
                        {
                            System.Threading.Interlocked.Exchange(ref isQueue, 0);
                            if (nodeQueue.IsEmpty || System.Threading.Interlocked.CompareExchange(ref isQueue, 1, 0) != 0) break;
                            if (!nodeQueue.IsEmpty)
                            {
                                Queue.AddOnly(this);
                                break;
                            }
                        }
                        while (true);
                    }
                }
            }
        }
        /// <summary>
        /// 服务端队列超时通知
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        internal override Task OnTimeout(CommandServerCallQueue queue, long seconds)
        {
            QueueTaskNode currentTask = this.currentTask;
            if (currentTask != null) return currentTask.OnTimeout(queue, seconds);
            return AutoCSer.Common.CompletedTask;
        }
    }
}
