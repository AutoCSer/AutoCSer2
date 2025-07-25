﻿using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// The low-priority queue of the server synchronization thread
    /// 服务端同步线程低优先级队列
    /// </summary>
    public sealed class CommandServerCallLowPriorityQueue : CommandServerCallQueueNode
    {
        /// <summary>
        /// The queue of the server synchronization thread
        /// 服务端同步线程队列
        /// </summary>
        public readonly CommandServerCallQueue Queue;
        /// <summary>
        /// Task queue
        /// 任务队列
        /// </summary>
        private LinkStack<QueueTaskNode> nodeQueue;
        /// <summary>
        /// Head node
        /// </summary>
#if NetStandard21
        private QueueTaskNode? head;
#else
        private QueueTaskNode head;
#endif
        /// <summary>
        /// The current task execution node
        /// 当前执行任务节点
        /// </summary>
        private QueueTaskNode currentTask;
        /// <summary>
        /// Has it been added to the queue
        /// 是否已经添加到队列
        /// </summary>
        private int isQueue;
        /// <summary>
        /// The low-priority queue of the server synchronization thread
        /// 服务端同步线程低优先级队列
        /// </summary>
        /// <param name="queue"></param>
        internal CommandServerCallLowPriorityQueue(CommandServerCallQueue queue)
        {
            this.Queue = queue;
            currentTask = ActionQueueTaskNode.Empty;
        }
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        /// <param name="node"></param>
        public void Add(CommandServerCallQueueCustomNode node)
        {
            if (node.CheckQueue()) add(node);
            else throw new Exception("node.isQueue is true");
        }
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void AddOnly(CommandServerCallQueueNode node)
        {
            if (node != null) add(node);
        }
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        /// <param name="node"></param>
        private void add(QueueTaskNode node)
        {
            if (nodeQueue.IsPushHead(node) && System.Threading.Interlocked.CompareExchange(ref isQueue, 1, 0) == 0) Queue.AddOnly(this);
        }
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Add(CommandServerCallLowPriorityQueue queue, CommandServerCallQueueNode node)
        {
            queue.add(node);
        }
        /// <summary>
        /// Add the task node
        /// 添加任务节点
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
        /// Execute the task
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
        /// Server-side queue timeout notification
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
