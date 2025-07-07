using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// The server asynchronously calls the low-priority queue (mainly used for write operations, transaction read operations, and updating the queue memory cache status)
    /// 服务端异步调用低优先级队列（主要用于写操作、事务读操作、更新队列内存缓存状态）
    /// </summary>
    public abstract class CommandServerCallTaskLowPriorityQueue
    {
        /// <summary>
        /// Task queue
        /// 任务队列
        /// </summary>
        internal LinkStack<CommandServerCallTaskQueueNode> Queue;
        /// <summary>
        /// Low-priority task queue
        /// 低优先级任务队列
        /// </summary>
        internal LinkStack<CommandServerCallTaskQueueNode> LowPriorityQueue;
        /// <summary>
        /// Low-priority task queue node
        /// 低优先级任务队列节点
        /// </summary>
#if NetStandard21
        private CommandServerCallTaskQueueNode? lowPriorityQueue;
#else
        private CommandServerCallTaskQueueNode lowPriorityQueue;
#endif
        /// <summary>
        /// Whether the queue of low-priority tasks is empty
        /// 低优先级任务队列是否为空
        /// </summary>
        internal bool IsEmptyLowPriorityQueue
        {
            get { return lowPriorityQueue == null && LowPriorityQueue.IsEmpty; }
        }
        /// <summary>
        /// Execute timeout to check the task list
        /// 执行超时检查任务链表
        /// </summary>
        private LinkStack<CommandServerCallTaskQueueNode> timeoutLink;
        /// <summary>
        /// Command server to listen
        /// 命令服务端监听
        /// </summary>
        public readonly CommandListener Server;
        /// <summary>
        /// Gets the queue keyword string
        /// 获取队列关键字字符串
        /// </summary>
        public abstract string KeyString { get; }
        /// <summary>
        /// Queue custom context object
        /// 队列自定义上下文对象
        /// </summary>
#if NetStandard21
        public object? ContextObject;
#else
        public object ContextObject;
#endif
        /// <summary>
        /// Task queue controller service
        /// Task 队列控制器服务
        /// </summary>
#if NetStandard21
        internal CommandServerTaskQueueService? TaskQueue;
#else
        internal CommandServerTaskQueueService TaskQueue;
#endif
        /// <summary>
        /// The maximum number of concurrent reading tasks
        /// 最大读并发任务数量
        /// </summary>
        public readonly int MaxConcurrent;
        /// <summary>
        /// Write operations wait for the number of read operation tasks
        /// 写操作等待读取操作任务数量
        /// </summary>
        public readonly int LowPriorityWaitCount;
        /// <summary>
        /// The number of concurrent tasks can be increased
        /// 可增加并发任务数量
        /// </summary>
        private int canConcurrentCount;
        /// <summary>
        /// The current number of tasks waiting for execution in the low-priority queue is less than or equal to 0 to trigger the execution of tasks in the low-priority queue
        /// 当前低优先级队列等待任务执行数量，小于等于 0 触发低优先级队列任务执行
        /// </summary>
        private int currentLowPriorityWaitCount;
        /// <summary>
        /// Is any task is running
        /// 是否正在运行任务
        /// </summary>
        protected int isRunTask;
        /// <summary>
        /// Wait for the task to end and access the lock
        /// 等待任务结束访问锁
        /// </summary>
        private int waitLock = 1;
        /// <summary>
        /// The time of the last task addition
        /// 最后添加任务时间
        /// </summary>
        protected long AppendTaskSeconds;
        /// <summary>
        /// Queue residency application count
        /// 队列驻留申请计数
        /// </summary>
        protected int resideCount;
        /// <summary>
        /// The queue for asynchronous server calls waiting type
        /// 服务端异步调用队列等待类型
        /// </summary>
        internal CallTaskQueueWaitTypeEnum WaitType;
        /// <summary>
        /// Whether it is necessary to check the queue execution timeout
        /// 是否需要检查队列执行超时
        /// </summary>
        private readonly bool checkTimeout;
        /// <summary>
        /// Whether the queue resides in memory by default
        /// 队列是否默认驻留内存
        /// </summary>
        protected readonly bool isReside;
        /// <summary>
        /// The current task execution node
        /// 当前执行任务节点
        /// </summary>
#if NetStandard21
        protected CommandServerCallTaskQueueNode? currentTask;
#else
        protected CommandServerCallTaskQueueNode currentTask;
#endif
        /// <summary>
        /// The next task ready for execution
        /// 下一个准备执行的任务
        /// </summary>
#if NetStandard21
        private CommandServerCallTaskQueueNode? nextTask;
#else
        private CommandServerCallTaskQueueNode nextTask;
#endif
        /// <summary>
        /// Default empty queue
        /// 默认空队列
        /// </summary>
        internal CommandServerCallTaskLowPriorityQueue()
        {
            Server = CommandListener.Null;
        }
        /// <summary>
        /// The queue for asynchronous server calls
        /// 服务端异步调用队列
        /// </summary>
        /// <param name="queueSet"></param>
        /// <param name="isReside"></param>
        protected CommandServerCallTaskLowPriorityQueue(CommandServerCallTaskQueueSet queueSet, bool isReside)
        {
            Server = queueSet.Server;
            canConcurrentCount = MaxConcurrent = queueSet.QueueMaxConcurrent;
            currentLowPriorityWaitCount = LowPriorityWaitCount = queueSet.QueueWaitCount;
            checkTimeout = queueSet.CheckTaskTimeout;
            this.isReside = isReside;
        }
        /// <summary>
        /// The queue for asynchronous server calls
        /// 服务端异步调用队列
        /// </summary>
        /// <param name="controller"></param>
        protected CommandServerCallTaskLowPriorityQueue(CommandServerController controller)
        {
            Server = controller.Server;
            canConcurrentCount = MaxConcurrent = controller.Attribute.TaskQueueMaxConcurrent;
            currentLowPriorityWaitCount = LowPriorityWaitCount = Math.Max(controller.Attribute.TaskQueueWaitCount, 1);
            isReside = true;
        }
        /// <summary>
        /// Start executing tasks
        /// 开始执行任务
        /// </summary>
        protected void run()
        {
            switch (WaitType)
            {
                case CallTaskQueueWaitTypeEnum.Concurrent: goto LOW;
                case CallTaskQueueWaitTypeEnum.LowPriority:
                    if (!runLowPriority()) return;
                    goto NEXT;
                case CallTaskQueueWaitTypeEnum.RunLowPriority:
                    goto NEXT;
                case CallTaskQueueWaitTypeEnum.Queue:
                    goto HEAD;
            }
        START:
            --currentLowPriorityWaitCount;
            var task = currentTask.notNull();
            if (!task.RunTask(out nextTask))
            {
                //Console.Write('C');
                if (checkTimeout)
                {
                    task.LinkNext = null;
                    timeoutLink.Push(task);
                }
                //Console.Write('-');
                if (System.Threading.Interlocked.Decrement(ref canConcurrentCount) == 0)
                {
                    WaitType = CallTaskQueueWaitTypeEnum.Concurrent;
                    wait();
                    return;
                }
            }
            else if (Server.IsDisposed) return;
            LOW:
            if (currentLowPriorityWaitCount <= 0 && !IsEmptyLowPriorityQueue)
            {
                if (canConcurrentCount != MaxConcurrent)
                {
                    WaitType = CallTaskQueueWaitTypeEnum.LowPriority;
                    wait();
                    return;
                }
                if (!runLowPriority()) return;
            }
        NEXT:
            if (nextTask != null)
            {
                currentTask = nextTask;
                goto START;
            }
        HEAD:
            currentTask = Queue.GetQueue();
            if (currentTask != null) goto START;
            if (!IsEmptyLowPriorityQueue)
            {
                if (canConcurrentCount != MaxConcurrent)
                {
                    WaitType = CallTaskQueueWaitTypeEnum.LowPriority;
                    wait();
                    return;
                }
                if (!runLowPriority()) return;
                goto HEAD;
            }

            if (canConcurrentCount != MaxConcurrent)
            {
                WaitType = CallTaskQueueWaitTypeEnum.Queue;
                wait();
                return;
            }

            System.Threading.Interlocked.Exchange(ref isRunTask, 0);
            if ((!Queue.IsEmpty || !IsEmptyLowPriorityQueue) && System.Threading.Interlocked.CompareExchange(ref isRunTask, 1, 0) == 0)
            {
                goto HEAD;
            }
            if (resideCount == 0 && !isReside) appendRemove();
            //Console.Write('D');
        }
        /// <summary>
        /// Run low-priority tasks
        /// 运行低优先级任务
        /// </summary>
        /// <returns>Whether the next task needs to be carried out
        /// 是否需要继续执行下一个任务</returns>
#if NET8
        [MemberNotNull(nameof(currentTask))]
#endif
        private bool runLowPriority()
        {
#if DEBUG
            if (canConcurrentCount != MaxConcurrent)
            {
                Server.Log.ErrorIgnoreException($"{KeyString} 低优先级任务并发数量检查错误 {canConcurrentCount} <> {MaxConcurrent} 可能导致读写任务并发执行", LogLevelEnum.Error | LogLevelEnum.Fatal);
            }
#endif
            currentLowPriorityWaitCount = LowPriorityWaitCount;
            currentTask = popLowPriority();
            if (!currentTask.LowPriorityRunTask())
            {
                //Console.Write('d');
                if (checkTimeout)
                {
                    currentTask.LinkNext = null;
                    timeoutLink.Push(currentTask);
                }
                //Console.Write('-');
                System.Threading.Interlocked.Decrement(ref canConcurrentCount);
                WaitType = CallTaskQueueWaitTypeEnum.RunLowPriority;
                wait();
                return false;
            }
            return !Server.IsDisposed;
        }
        /// <summary>
        /// Release the task ownership and attempt to obtain the task ownership
        /// 释放任务所有权并尝试获取任务所有权
        /// </summary>
        protected void wait()
        {
            do
            {
                System.Threading.Interlocked.Exchange(ref waitLock, 0);
                switch (WaitType)
                {
                    case CallTaskQueueWaitTypeEnum.Concurrent:
                        if (canConcurrentCount == 0) return;
                        break;
                    case CallTaskQueueWaitTypeEnum.LowPriority:
                    case CallTaskQueueWaitTypeEnum.RunLowPriority:
                        if (canConcurrentCount != MaxConcurrent)
                        {
                            //Console.Write('e');
                            return;
                        }
                        break;
                    case CallTaskQueueWaitTypeEnum.Queue:
                        if (Queue.IsEmpty && IsEmptyLowPriorityQueue && canConcurrentCount != MaxConcurrent) return;
                        break;
                }
            }
            while (checkWait());
        }
        /// <summary>
        /// Try to obtain the ownership of the task
        /// 尝试获取任务所有权
        /// </summary>
        /// <returns></returns>
        protected bool checkWait()
        {
            if (System.Threading.Interlocked.CompareExchange(ref waitLock, 1, 0) == 0)
            {
                switch (WaitType)
                {
                    case CallTaskQueueWaitTypeEnum.Concurrent:
                        if (canConcurrentCount != 0)
                        {
                            run();
                            return false;
                        }
                        return true;
                    case CallTaskQueueWaitTypeEnum.LowPriority:
                    case CallTaskQueueWaitTypeEnum.RunLowPriority:
                        if (canConcurrentCount == MaxConcurrent)
                        {
                            run();
                            return false;
                        }
                        return true;
                    case CallTaskQueueWaitTypeEnum.Queue:
                        if (canConcurrentCount == MaxConcurrent)
                        {
                            run();
                            return false;
                        }
                        if (!Queue.IsEmpty || !IsEmptyLowPriorityQueue)
                        {
                            run();
                            return false;
                        }
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Try to obtain the ownership of the task after it is completed
        /// 任务完成以后尝试获取任务所有权
        /// </summary>
        /// <param name="task"></param>
        internal void OnCompleted(CommandServerCallTaskQueueNode task)
        {
            //Console.Write('E');
            if (!Server.IsDisposed && System.Threading.Interlocked.Exchange(ref task.RunSeconds, 0) != 0)
            {
                //Console.Write('+');
                int canConcurrentCount = System.Threading.Interlocked.Increment(ref this.canConcurrentCount);
                switch (WaitType)
                {
                    case CallTaskQueueWaitTypeEnum.LowPriority:
                    //case CallTaskQueueWaitType.RunLowPriority:
                    case CallTaskQueueWaitTypeEnum.Queue:
                        if (canConcurrentCount != MaxConcurrent)
                        {
                            //Console.Write('F');
                            return;
                        }
                        break;
                    case CallTaskQueueWaitTypeEnum.RunLowPriority:
                        if (canConcurrentCount != MaxConcurrent)
                        {
                            Server.Log.ErrorIgnoreException($"{KeyString} 低优先级任务完成以后并发数量检查错误 {canConcurrentCount} <> {MaxConcurrent} 可能导致队列执行中断", LogLevelEnum.Error | LogLevelEnum.Fatal);
                            //Console.Write('F');
                            return;
                        }
                        break;
                }
                if (checkWait()) wait();
            }
        }
        /// <summary>
        /// Add to the deletion queue
        /// 添加到删除队列
        /// </summary>
        protected abstract void appendRemove();
        /// <summary>
        /// Task execution timeout check
        /// 任务执行超时检查
        /// </summary>
        /// <param name="keepSeconds"></param>
        /// <returns></returns>
        internal async Task CheckTaskTimeoutAsync(int keepSeconds)
        {
            if (!timeoutLink.IsEmpty)
            {
                long timeoutSeconds = SecondTimer.CurrentSeconds - keepSeconds;
                var head = timeoutLink.Get().notNull();
                var next = default(CommandServerCallTaskQueueNode);
                do
                {
                    try
                    {
                        do
                        {
                            long runSeconds = head.GetRunSeconds(out next);
                            if (runSeconds != 0)
                            {
                                timeoutLink.Push(head);
                                if (runSeconds < timeoutSeconds) await head.OnTimeout(SecondTimer.CurrentSeconds - runSeconds);
                            }
                            head = next;
                        }
                        while (head != null);
                        break;
                    }
                    catch (Exception exception)
                    {
                        await Server.Config.Log.Exception(exception, null, LogLevelEnum.AutoCSer | LogLevelEnum.Exception);
                    }
                    head = next;
                }
                while (head != null);
            }
        }
        /// <summary>
        /// Pop up low-priority task
        /// 弹出低优先级任务
        /// </summary>
        /// <returns></returns>
        private CommandServerCallTaskQueueNode popLowPriority()
        {
            var node = lowPriorityQueue ?? LowPriorityQueue.GetQueue().notNull();
            lowPriorityQueue = node.LinkNext;
            return node;
        }
        /// <summary>
        /// Add low-priority task
        /// 添加低优先级任务
        /// </summary>
        /// <param name="value"></param>
        public void AddLowPriority(CommandServerCallTaskQueueNode value)
        {
            if (object.ReferenceEquals(value.Queue, CommandServerControllerCallTaskQueue.Null)) AddLowPriorityOnly(value);
            else throw new Exception("value.Queue != null");
        }
        /// <summary>
        /// Add low-priority task
        /// 添加低优先级任务
        /// </summary>
        /// <param name="value"></param>
        internal void AddLowPriorityOnly(CommandServerCallTaskQueueNode value)
        {
            value.Queue = (CommandServerCallTaskQueue)this;
            AppendTaskSeconds = SecondTimer.CurrentSeconds;
            if (LowPriorityQueue.IsPushHead(value) && lowPriorityQueue == null)
            {
                //Console.Write('a');
                if (System.Threading.Interlocked.CompareExchange(ref isRunTask, 1, 0) == 0)
                {
                    //Console.Write('b');
                    WaitType = CallTaskQueueWaitTypeEnum.Queue;
                    run();
                    return;
                }
                if (WaitType == CallTaskQueueWaitTypeEnum.Queue && checkWait()) wait();
            }
        }
    }
    /// <summary>
    /// The server asynchronously calls the low-priority queue
    /// 服务端异步调用低优先级队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommandServerCallTaskLowPriorityQueue<T> : CommandServerCallTaskQueue
        where T : IEquatable<T>
    {
        /// <summary>
        /// The collection of asynchronous call queues on the server side
        /// 服务端异步调用队列集合
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        protected readonly CommandServerCallTaskQueueSet<T> queueSet;
        /// <summary>
        /// Queue keyword
        /// 队列关键字
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        public readonly T Key;
        /// <summary>
        /// Default empty queue
        /// 默认空队列
        /// </summary>
        internal CommandServerCallTaskLowPriorityQueue() { }
        /// <summary>
        /// The queue for asynchronous server calls
        /// 服务端异步调用队列
        /// </summary>
        /// <param name="queueSet"></param>
        /// <param name="isReside"></param>
        /// <param name="key"></param>
        public CommandServerCallTaskLowPriorityQueue(CommandServerCallTaskQueueSet<T> queueSet, bool isReside, T key)
            : base(queueSet, isReside)
        {
            this.queueSet = queueSet;
            Key = key;
        }
    }
}
