using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;

namespace AutoCSer.Net
{
    /// <summary>
    /// The queue for asynchronous server calls (mainly used for reading the queue's memory cache status, except for the initialization of the queue context, try to avoid IO blocking operations as much as possible; Dirty read database operations should be handled using ordinary concurrent tasks instead of read-write queue operations.
    /// 服务端异步调用队列（主要用于读取队列内存缓存状态，除了队列上下文初始化尽量不要有 IO 阻塞操作；脏读数据库操作应该使用普通并发任务处理，不应该使用读写队列操作）
    /// </summary>
    public abstract class CommandServerCallTaskQueue : CommandServerCallTaskLowPriorityQueue
    {
        /// <summary>
        /// Queue context asynchronous lock
        /// 队列上下文异步锁
        /// </summary>
#if DEBUG && NetStandard21
        [AllowNull]
#endif
        private SemaphoreSlimLock contextLock;
        /// <summary>
        /// Default empty queue
        /// 默认空队列
        /// </summary>
        internal CommandServerCallTaskQueue() { }
        /// <summary>
        /// The queue for asynchronous server calls (mainly used for reading the queue's memory cache status, except for the initialization of the queue context, try to avoid IO blocking operations as much as possible; Dirty read database operations should be handled using ordinary concurrent tasks instead of read-write queue operations.
        /// 服务端异步调用队列（主要用于读取队列内存缓存状态，除了队列上下文初始化尽量不要有 IO 阻塞操作；脏读数据库操作应该使用普通并发任务处理，不应该使用读写队列操作）
        /// </summary>
        /// <param name="queueSet"></param>
        /// <param name="isReside"></param>
        public CommandServerCallTaskQueue(CommandServerCallTaskQueueSet queueSet, bool isReside) : base(queueSet, isReside)
        {
            if (MaxConcurrent > 1) contextLock = new SemaphoreSlimLock(1);
        }
        /// <summary>
        /// The queue for asynchronous server calls (mainly used for reading the queue's memory cache status, except for the initialization of the queue context, try to avoid IO blocking operations as much as possible; Dirty read database operations should be handled using ordinary concurrent tasks instead of read-write queue operations.
        /// 服务端异步调用队列（主要用于读取队列内存缓存状态，除了队列上下文初始化尽量不要有 IO 阻塞操作；脏读数据库操作应该使用普通并发任务处理，不应该使用读写队列操作）
        /// </summary>
        /// <param name="controller"></param>
        protected CommandServerCallTaskQueue(CommandServerController controller) : base(controller)
        {
            if (MaxConcurrent > 1) contextLock = new SemaphoreSlimLock(1);
        }
        /// <summary>
        /// Enter the queue context asynchronous lock (used in the case where multiple read tasks concurrently operate ContextObject)
        /// 进入队列上下文异步锁（用于多个读取任务并发操作 ContextObject 的情况）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task EnterContext(
#if DEBUG
#if NetStandard21
             [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#else
             [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0
#endif
#endif
            )
        {
            if (MaxConcurrent > 1)
            {
#if DEBUG
                return contextLock.EnterAsync(callerMemberName, callerFilePath, callerLineNumber);
#else
                return contextLock.EnterAsync();
#endif
            }
            return AutoCSer.Common.CompletedTask;
        }
        /// <summary>
        /// Release the asynchronous lock of the queue context (used in the case where multiple read tasks concurrently operate the context object)
        /// 释放队列上下文异步锁（用于多个读取任务并发操作 ContextObject 的情况）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void ExitContext()
        {
            if (MaxConcurrent > 1) contextLock.Exit();
        }
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        /// <param name="value"></param>
        public void Add(CommandServerCallTaskQueueNode value)
        {
            if (object.ReferenceEquals(value.Queue, CommandServerControllerCallTaskQueue.Null))
            {
                AddOnly(value);
                return;
            }
            throw new Exception("value.Queue != null");
        }
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        /// <param name="value"></param>
        internal void AddOnly(CommandServerCallTaskQueueNode value)
        {
            value.Queue = this;
            AppendTaskSeconds = SecondTimer.CurrentSeconds;
            if (Queue.IsPushHead(value))
            {
                //Console.Write('A');
                if (System.Threading.Interlocked.CompareExchange(ref isRunTask, 1, 0) == 0)
                {
                    //Console.Write('B');
                    WaitType = CallTaskQueueWaitTypeEnum.Queue;
                    //currentTask = Queue.GetClear();
                    //WaitType = CallTaskQueueWaitType.None;
                    Task.Run((Action)run);
                    //run();
                    return;
                }
                //if (WaitType == CallTaskQueueWaitTypeEnum.Queue && checkWait()) wait();
                if (WaitType == CallTaskQueueWaitTypeEnum.Queue) Task.Run((Action)checkQueueWait);
            }
        }
        /// <summary>
        /// Try to obtain the ownership of the task
        /// 尝试获取任务所有权
        /// </summary>
        private void checkQueueWait()
        {
            if (checkWait()) wait();
        }
    }
    /// <summary>
    /// The queue for asynchronous server calls
    /// 服务端异步调用队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CommandServerCallTaskQueue<T> : CommandServerCallTaskLowPriorityQueue<T>
        where T : IEquatable<T>
    {
        /// <summary>
        /// The next queue waiting to be deleted
        /// 下一个等待删除队列
        /// </summary>
#if NetStandard21
        internal CommandServerCallTaskQueue<T>? RemoveNext;
#else
        internal CommandServerCallTaskQueue<T> RemoveNext;
#endif
        /// <summary>
        /// Waiting time for deletion
        /// 等待删除时间
        /// </summary>
        internal long RemoveSeconds;
        /// <summary>
        /// Gets the queue keyword string
        /// 获取队列关键字字符串
        /// </summary>
        public override string KeyString { get { return Key.ToString().notNull(); } }
        /// <summary>
        /// Default empty queue
        /// 默认空队列
        /// </summary>
        private CommandServerCallTaskQueue() { }
        /// <summary>
        /// The queue for asynchronous server calls
        /// 服务端异步调用队列
        /// </summary>
        /// <param name="queueSet"></param>
        /// <param name="isReside"></param>
        /// <param name="key"></param>
        public CommandServerCallTaskQueue(CommandServerCallTaskQueueSet<T> queueSet, bool isReside, T key)
            : base(queueSet, isReside, key)
        {
        }
        ///// <summary>
        ///// 释放队列驻留内存计数
        ///// </summary>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public void ReleaseReside()
        //{
        //    if (System.Threading.Interlocked.Decrement(ref resideCount) == 0 && isRunTask == 0) queueSet.AppendRemove(this);
        //}
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        /// <param name="value"></param>
        internal void AddOnlyReside(CommandServerCallTaskQueueNode value)
        {
            System.Threading.Interlocked.Increment(ref resideCount);
            try
            {
                AddOnly(value);
            }
            finally
            {
                if (System.Threading.Interlocked.Decrement(ref resideCount) == 0 && isRunTask == 0) queueSet.AppendRemove(this);
            }
        }
        /// <summary>
        /// Add low-priority task
        /// 添加低优先级任务
        /// </summary>
        /// <param name="value"></param>
        internal void AddLowPriorityOnlyReside(CommandServerCallTaskQueueNode value)
        {
            System.Threading.Interlocked.Increment(ref resideCount);
            try
            {
                AddLowPriorityOnly(value);
            }
            finally
            {
                if (System.Threading.Interlocked.Decrement(ref resideCount) == 0 && isRunTask == 0) queueSet.AppendRemove(this);
            }
        }
        /// <summary>
        /// Add to the deletion queue
        /// 添加到删除队列
        /// </summary>
        protected override void appendRemove()
        {
            queueSet.AppendRemove(this);
        }
        /// <summary>
        /// Set the waiting time for deletion
        /// 设置等待删除时间
        /// </summary>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        internal bool SetRemoveSeconds(int timeoutSeconds)
        {
            if (RemoveSeconds == 0)
            {
                if (CheckRemove())
                {
                    AppendTaskSeconds = 0;
                    RemoveSeconds = SecondTimer.CurrentSeconds + timeoutSeconds;
                    return true;
                }
            }
            else
            {
                AppendTaskSeconds = 0;
                RemoveSeconds = SecondTimer.CurrentSeconds + timeoutSeconds;
            }
            return false;
        }
        /// <summary>
        /// Determine whether the queue can be deleted
        /// 判断队列是否可以删除
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal int CheckRemove(out CommandServerCallTaskQueue<T>? next)
#else
        internal int CheckRemove(out CommandServerCallTaskQueue<T> next)
#endif
        {
            next = RemoveNext;
            RemoveNext = null;
            if (AppendTaskSeconds == 0)
            {
                if (SecondTimer.CurrentSeconds >= RemoveSeconds) return 0;
                return 1;
            }
            RemoveSeconds = 0;
            return 2;
        }
        /// <summary>
        /// Determine whether the queue can be deleted
        /// 判断队列是否可以删除
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckRemove()
        {
            return (resideCount | isRunTask) == 0 && Queue.IsEmpty && IsEmptyLowPriorityQueue;
        }

        /// <summary>
        /// Default empty queue
        /// 默认空队列
        /// </summary>
        internal static readonly CommandServerCallTaskQueue<T> Null = new CommandServerCallTaskQueue<T>();
    }
}
