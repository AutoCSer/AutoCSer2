using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoCSer.Extensions;
using AutoCSer.Threading;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端异步调用队列集合
    /// </summary>
    public abstract class CommandServerCallTaskQueueSet : SecondTimerTaskArrayNode
    {
        /// <summary>
        /// 命令服务
        /// </summary>
        internal readonly CommandListener Server;
        /// <summary>
        /// 队列集合访问锁
        /// </summary>
        protected readonly object queueLock = new object();
        /// <summary>
        /// Task 队列服务控制器创建器
        /// </summary>
#if NetStandard21
        private CommandServerInterfaceControllerCreator? controllerCreator;
#else
        private CommandServerInterfaceControllerCreator controllerCreator;
#endif
        /// <summary>
        /// 异步队列最大读并发任务数量
        /// </summary>
        internal readonly int QueueMaxConcurrent;
        /// <summary>
        /// 异步队列写操作等待读取操作任务数量
        /// </summary>
        internal readonly int QueueWaitCount;
        /// <summary>
        /// 异步队列驻留超时秒数，等待指定时间以后没有新任务再删除，负数表示永久驻留内存
        /// </summary>
        protected readonly int timeoutSeconds;
        /// <summary>
        /// 是否检查队列执行超时
        /// </summary>
        internal readonly bool CheckTaskTimeout;
        /// <summary>
        /// 服务端执行队列
        /// </summary>
        /// <param name="server"></param>
        /// <param name="checkTaskTimeout"></param>
        /// <param name="timeoutSeconds"></param>
        protected CommandServerCallTaskQueueSet(CommandListener server, bool checkTaskTimeout, int timeoutSeconds)
            : base(SecondTimer.TaskArray, timeoutSeconds, SecondTimerTaskThreadModeEnum.Synchronous, SecondTimerKeepModeEnum.After, timeoutSeconds)
        {
            Server = server;
            QueueMaxConcurrent = Math.Max(server.Config.TaskQueueMaxConcurrent, 1);
            QueueWaitCount = Math.Max(server.Config.TaskQueueWaitCount, 1);
            CheckTaskTimeout = checkTaskTimeout;
            this.timeoutSeconds = timeoutSeconds;
            if (KeepSeconds > 0) AppendTaskArray();
        }
        /// <summary>
        /// 关闭服务端异步调用队列
        /// </summary>
        internal abstract void Close();
        /// <summary>
        /// 队列任务执行超时检查
        /// </summary>
        /// <param name="keepSeconds"></param>
        internal abstract Task CheckTaskTimeoutAsync(int keepSeconds);
        /// <summary>
        /// 设置 Task 队列服务控制器创建器
        /// </summary>
        /// <param name="controllerCreator"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal CommandServerInterfaceControllerCreator? Set(CommandServerInterfaceControllerCreator controllerCreator)
#else
        internal CommandServerInterfaceControllerCreator Set(CommandServerInterfaceControllerCreator controllerCreator)
#endif
        {
            if (this.controllerCreator == null)
            {
                this.controllerCreator = controllerCreator;
                return null;
            }
            return this.controllerCreator;
        }
    }
    /// <summary>
    /// 服务端异步调用队列集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandServerCallTaskQueueSet<T> : CommandServerCallTaskQueueSet
        where T : IEquatable<T>
    {
        /// <summary>
        /// 队列集合
        /// </summary>
        protected readonly Dictionary<T, CommandServerCallTaskQueue<T>> queues = DictionaryCreator<T>.Create<CommandServerCallTaskQueue<T>>();
        /// <summary>
        /// 最后一次访问队列
        /// </summary>
        private CommandServerCallTaskQueue<T> lastQueue;
        /// <summary>
        /// 等待删除的队列链表首节点
        /// </summary>
        private CommandServerCallTaskQueue<T> removeHead;
        /// <summary>
        /// 等待删除的队列链表尾首节点
        /// </summary>
        private CommandServerCallTaskQueue<T> removeEnd;
        /// <summary>
        /// 服务端执行队列
        /// </summary>
        /// <param name="server"></param>
        /// <param name="checkTaskTimeout">是否检查队列执行超时</param>
        /// <param name="timeoutSeconds">异步队列驻留超时秒数，等待指定时间以后没有新任务再删除，负数表示永久驻留内存</param>
        public CommandServerCallTaskQueueSet(CommandListener server, bool checkTaskTimeout, int timeoutSeconds) : base(server, checkTaskTimeout, timeoutSeconds)
        {
            removeHead = removeEnd = lastQueue = CommandServerCallTaskQueue<T>.Null;
        }
        /// <summary>
        /// 服务端执行队列
        /// </summary>
        /// <param name="server"></param>
        /// <param name="checkTaskTimeout">是否检查队列执行超时</param>
        public CommandServerCallTaskQueueSet(CommandListener server, bool checkTaskTimeout)
            : base(server, checkTaskTimeout, typeAttribute == null ? server.Config.TaskQueueTimeoutSeconds : typeAttribute.TimeoutSeconds)
        {
            removeHead = removeEnd = lastQueue = CommandServerCallTaskQueue<T>.Null;
        }
        /// <summary>
        /// 创建异步队列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual CommandServerCallTaskQueue<T> createQueue(T key)
        {
            return new CommandServerCallTaskQueue<T>(this, timeoutSeconds < 0, key);
        }
        /// <summary>
        /// 获取队列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
#if NetStandard21
        private CommandServerCallTaskQueue<T>? get(T key)
#else
        private CommandServerCallTaskQueue<T> get(T key)
#endif
        {
            var queue = default(CommandServerCallTaskQueue<T>);
            System.Threading.Monitor.Enter(queueLock);
            try
            {
                if (queues.TryGetValue(key, out queue))
                {
                    lastQueue = queue;
                }
                else if (!Server.IsDisposed)
                {
                    queues.Add(key, queue = createQueue(key));
                    lastQueue = queue;
                }
            }
            finally { System.Threading.Monitor.Exit(queueLock); }
            return queue;
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <param name="key"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        internal CommandClientReturnTypeEnum Add(T key, CommandServerCallTaskQueueNode task)
        {
            var queue = lastQueue;
            if (!object.ReferenceEquals(queue, CommandServerCallTaskQueue<T>.Null) && queue.Key.Equals(key))
            {
                if (!Server.IsDisposed)
                {
                    if (timeoutSeconds >= 0) queue.AddOnlyReside(task);
                    else queue.AddOnly(task);
                    return CommandClientReturnTypeEnum.Success;
                }
                return CommandClientReturnTypeEnum.ServerDisposed;
            }
            if (Server.Config.CheckTaskQueueKey(key))
            {
                queue = get(key);
                if (queue != null)
                {
                    if (timeoutSeconds >= 0) queue.AddOnlyReside(task);
                    else queue.AddOnly(task);
                    return CommandClientReturnTypeEnum.Success;
                }
                return CommandClientReturnTypeEnum.ServerDisposed;
            }
            return CommandClientReturnTypeEnum.NotSupportTaskQueueKey;
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        public void Add(T key, Func<CommandServerCallTaskQueue, Task> getTask)
        {
            if (getTask == null) throw new ArgumentNullException();
            Add(key, new CommandServerCallTaskQueueCustomTask(getTask, true));
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        public void Add(T key, Func<CommandServerCallTaskQueue, T, Task> getTask)
        {
            if (getTask == null) throw new ArgumentNullException();
            Add(key, new CommandServerCallTaskQueueCustomTask(new CommandServer.CommandServerCallTaskQueueKeyFunc<T>(getTask, key).GetTask, true));
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueCustomTask AddTask(T key, Func<CommandServerCallTaskQueue, Task> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueCustomTask task = new CommandServerCallTaskQueueCustomTask(getTask, isSynchronous);
            Add(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueCustomTask AddTask(T key, Func<CommandServerCallTaskQueue, T, Task> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueCustomTask task = new CommandServerCallTaskQueueCustomTask(new CommandServer.CommandServerCallTaskQueueKeyFunc<T>(getTask, key).GetTask, isSynchronous);
            Add(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueExceptionTask AddExceptionTask(T key, Func<CommandServerCallTaskQueue, Task> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueExceptionTask task = new CommandServerCallTaskQueueExceptionTask(getTask, isSynchronous);
            Add(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueExceptionTask AddExceptionTask(T key, Func<CommandServerCallTaskQueue, T, Task> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueExceptionTask task = new CommandServerCallTaskQueueExceptionTask(new CommandServer.CommandServerCallTaskQueueKeyFunc<T>(getTask, key).GetTask, isSynchronous);
            Add(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <typeparam name="TT"></typeparam>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueCustomTask<TT> AddTask<TT>(T key, Func<CommandServerCallTaskQueue, Task<TT>> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueCustomTask<TT> task = new CommandServerCallTaskQueueCustomTask<TT>(getTask, isSynchronous);
            Add(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <typeparam name="TT"></typeparam>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueCustomTask<TT> AddTask<TT>(T key, Func<CommandServerCallTaskQueue, T, Task<TT>> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueCustomTask<TT> task = new CommandServerCallTaskQueueCustomTask<TT>(new CommandServer.CommandServerCallTaskQueueKeyFunc<T, TT>(getTask, key).GetTask, isSynchronous);
            Add(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <typeparam name="TT"></typeparam>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueExceptionTask<TT> AddExceptionTask<TT>(T key, Func<CommandServerCallTaskQueue, Task<TT>> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueExceptionTask<TT> task = new CommandServerCallTaskQueueExceptionTask<TT>(getTask, isSynchronous);
            Add(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <typeparam name="TT"></typeparam>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueExceptionTask<TT> AddExceptionTask<TT>(T key, Func<CommandServerCallTaskQueue, T, Task<TT>> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueExceptionTask<TT> task = new CommandServerCallTaskQueueExceptionTask<TT>(new CommandServer.CommandServerCallTaskQueueKeyFunc<T, TT>(getTask, key).GetTask, isSynchronous);
            Add(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        public void Add(T key, Func<Task> getTask)
        {
            if (getTask == null) throw new ArgumentNullException();
            Add(key, new CommandServerCallTaskQueueCustomTask(getTask, true));
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueCustomTask AddTask(T key, Func<Task> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueCustomTask task = new CommandServerCallTaskQueueCustomTask(getTask, isSynchronous);
            Add(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueExceptionTask AddExceptionTask(T key, Func<Task> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueExceptionTask task = new CommandServerCallTaskQueueExceptionTask(getTask, isSynchronous);
            Add(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <typeparam name="TT"></typeparam>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueCustomTask<TT> AddTask<TT>(T key, Func<Task<TT>> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueCustomTask<TT> task = new CommandServerCallTaskQueueCustomTask<TT>(getTask, isSynchronous);
            Add(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务
        /// </summary>
        /// <typeparam name="TT"></typeparam>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueExceptionTask<TT> AddExceptionTask<TT>(T key, Func<Task<TT>> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueExceptionTask<TT> task = new CommandServerCallTaskQueueExceptionTask<TT>(getTask, isSynchronous);
            Add(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务（低优先级）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        internal CommandClientReturnTypeEnum AddLowPriority(T key, CommandServerCallTaskQueueNode task)
        {
            var queue = lastQueue;
            if (!object.ReferenceEquals(queue, CommandServerCallTaskQueue<T>.Null) && queue.Key.Equals(key))
            {
                if (!Server.IsDisposed)
                {
                    if (timeoutSeconds >= 0) queue.AddLowPriorityOnlyReside(task);
                    else queue.AddLowPriorityOnly(task);
                    return CommandClientReturnTypeEnum.Success;
                }
                return CommandClientReturnTypeEnum.ServerDisposed;
            }
            if (Server.Config.CheckTaskQueueKey(key))
            {
                queue = get(key);
                if (queue != null)
                {
                    if (timeoutSeconds >= 0) queue.AddLowPriorityOnlyReside(task);
                    else queue.AddLowPriorityOnly(task);
                    return CommandClientReturnTypeEnum.Success;
                }
                return CommandClientReturnTypeEnum.ServerDisposed;
            }
            return CommandClientReturnTypeEnum.NotSupportTaskQueueKey;
        }
        /// <summary>
        /// 添加队列任务（低优先级）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        public void AddLowPriority(T key, Func<CommandServerCallTaskQueue, Task> getTask)
        {
            if (getTask == null) throw new ArgumentNullException();
            AddLowPriority(key, new CommandServerCallTaskQueueCustomTask(getTask, true));
        }
        /// <summary>
        /// 添加队列任务（低优先级）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        public void AddLowPriority(T key, Func<CommandServerCallTaskQueue, T, Task> getTask)
        {
            if (getTask == null) throw new ArgumentNullException();
            AddLowPriority(key, new CommandServerCallTaskQueueCustomTask(new CommandServer.CommandServerCallTaskQueueKeyFunc<T>(getTask, key).GetTask, true));
        }
        /// <summary>
        /// 添加队列任务（低优先级）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueCustomTask AddLowPriorityTask(T key, Func<CommandServerCallTaskQueue, Task> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueCustomTask task = new CommandServerCallTaskQueueCustomTask(getTask, isSynchronous);
            AddLowPriority(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务（低优先级）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueCustomTask AddLowPriorityTask(T key, Func<CommandServerCallTaskQueue, T, Task> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueCustomTask task = new CommandServerCallTaskQueueCustomTask(new CommandServer.CommandServerCallTaskQueueKeyFunc<T>(getTask, key).GetTask, isSynchronous);
            AddLowPriority(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务（低优先级）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueExceptionTask AddLowPriorityExceptionTask(T key, Func<CommandServerCallTaskQueue, Task> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueExceptionTask task = new CommandServerCallTaskQueueExceptionTask(getTask, isSynchronous);
            AddLowPriority(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务（低优先级）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueExceptionTask AddLowPriorityExceptionTask(T key, Func<CommandServerCallTaskQueue, T, Task> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueExceptionTask task = new CommandServerCallTaskQueueExceptionTask(new CommandServer.CommandServerCallTaskQueueKeyFunc<T>(getTask, key).GetTask, isSynchronous);
            AddLowPriority(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务（低优先级）
        /// </summary>
        /// <typeparam name="TT"></typeparam>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueCustomTask<TT> AddLowPriorityTask<TT>(T key, Func<CommandServerCallTaskQueue, Task<TT>> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueCustomTask<TT> task = new CommandServerCallTaskQueueCustomTask<TT>(getTask, isSynchronous);
            AddLowPriority(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务（低优先级）
        /// </summary>
        /// <typeparam name="TT"></typeparam>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueCustomTask<TT> AddLowPriorityTask<TT>(T key, Func<CommandServerCallTaskQueue, T, Task<TT>> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueCustomTask<TT> task = new CommandServerCallTaskQueueCustomTask<TT>(new CommandServer.CommandServerCallTaskQueueKeyFunc<T, TT>(getTask, key).GetTask, isSynchronous);
            AddLowPriority(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务（低优先级）
        /// </summary>
        /// <typeparam name="TT"></typeparam>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueExceptionTask<TT> AddLowPriorityExceptionTask<TT>(T key, Func<CommandServerCallTaskQueue, Task<TT>> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueExceptionTask<TT> task = new CommandServerCallTaskQueueExceptionTask<TT>(getTask, isSynchronous);
            AddLowPriority(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务（低优先级）
        /// </summary>
        /// <typeparam name="TT"></typeparam>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueExceptionTask<TT> AddLowPriorityExceptionTask<TT>(T key, Func<CommandServerCallTaskQueue, T, Task<TT>> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueExceptionTask<TT> task = new CommandServerCallTaskQueueExceptionTask<TT>(new CommandServer.CommandServerCallTaskQueueKeyFunc<T, TT>(getTask, key).GetTask, isSynchronous);
            AddLowPriority(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务（低优先级）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        public void AddLowPriority(T key, Func<Task> getTask)
        {
            if (getTask == null) throw new ArgumentNullException();
            AddLowPriority(key, new CommandServerCallTaskQueueCustomTask(getTask, true));
        }
        /// <summary>
        /// 添加队列任务（低优先级）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueCustomTask AddLowPriorityTask(T key, Func<Task> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueCustomTask task = new CommandServerCallTaskQueueCustomTask(getTask, isSynchronous);
            AddLowPriority(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务（低优先级）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueExceptionTask AddLowPriorityExceptionTask(T key, Func<Task> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueExceptionTask task = new CommandServerCallTaskQueueExceptionTask(getTask, isSynchronous);
            AddLowPriority(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务（低优先级）
        /// </summary>
        /// <typeparam name="TT"></typeparam>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueCustomTask<TT> AddLowPriorityTask<TT>(T key, Func<Task<TT>> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueCustomTask<TT> task = new CommandServerCallTaskQueueCustomTask<TT>(getTask, isSynchronous);
            AddLowPriority(key, task);
            return task;
        }
        /// <summary>
        /// 添加队列任务（低优先级）
        /// </summary>
        /// <typeparam name="TT"></typeparam>
        /// <param name="key"></param>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous">是否同步执行回调，默认为 false 表示任务执行回调，设置为 true 可能阻塞队列执行</param>
        /// <returns>可 await 任务对象</returns>
        public CommandServerCallTaskQueueExceptionTask<TT> AddLowPriorityExceptionTask<TT>(T key, Func<Task<TT>> getTask, bool isSynchronous = false)
        {
            if (getTask == null) throw new ArgumentNullException();
            CommandServerCallTaskQueueExceptionTask<TT> task = new CommandServerCallTaskQueueExceptionTask<TT>(getTask, isSynchronous);
            AddLowPriority(key, task);
            return task;
        }
        /// <summary>
        /// 关闭服务端异步调用队列
        /// </summary>
        internal override void Close()
        {
            System.Threading.Monitor.Enter(queueLock);
            try
            {
                queues.Clear();
            }
            finally
            {
                System.Threading.Monitor.Exit(queueLock);
                onClosed();
            }
        }
        /// <summary>
        /// 关闭队列处理
        /// </summary>
        protected virtual void onClosed() { }
        /// <summary>
        /// 队列执行超时检查
        /// </summary>
        /// <param name="keepSeconds"></param>
        internal override async Task CheckTaskTimeoutAsync(int keepSeconds)
        {
            foreach (CommandServerCallTaskQueue queue in queues.Values) await queue.CheckTaskTimeoutAsync(keepSeconds);
        }
        /// <summary>
        /// 添加到过期删除队列
        /// </summary>
        /// <param name="queue"></param>
        internal void AppendRemove(CommandServerCallTaskQueue<T> queue)
        {
            if (KeepSeconds > 0)
            {
                if (queue.RemoveSeconds == 0)
                {
                    System.Threading.Monitor.Enter(queueLock);
                    if (queue.SetRemoveSeconds(timeoutSeconds))
                    {
                        if (!object.ReferenceEquals(removeHead, CommandServerCallTaskQueue<T>.Null)) removeEnd.RemoveNext = queue;
                        else removeHead = queue;
                        removeEnd = queue;
                    }
                    System.Threading.Monitor.Exit(queueLock);
                }
            }
            else
            {
                bool isRemoved = false;
                System.Threading.Monitor.Enter(queueLock);
                try
                {
                    if (queue.CheckRemove())
                    {
                        queues.Remove(queue.Key);
                        if (object.ReferenceEquals(lastQueue, queue)) lastQueue = CommandServerCallTaskQueue<T>.Null;
                        isRemoved = true;
                    }
                }
                finally
                {
                    System.Threading.Monitor.Exit(queueLock);
                    if (isRemoved)
                    {
                        if (object.ReferenceEquals(lastQueue, queue)) lastQueue = CommandServerCallTaskQueue<T>.Null;
                        onRemoved(queue);
                    }
                }
            }
        }
        /// <summary>
        /// 队列删除以后的处理
        /// </summary>
        /// <param name="queue"></param>
        protected virtual void onRemoved(CommandServerCallTaskQueue<T> queue) { }
        /// <summary>
        /// 队列过期删除检查
        /// </summary>
        protected internal override void OnTimer()
        {
            if (!object.ReferenceEquals(removeHead, CommandServerCallTaskQueue<T>.Null))
            {
                System.Threading.Monitor.Enter(queueLock);
                var head = removeHead;
                removeEnd = CommandServerCallTaskQueue<T>.Null;
                removeHead = CommandServerCallTaskQueue<T>.Null;
                System.Threading.Monitor.Exit(queueLock);

                var next = default(CommandServerCallTaskQueue<T>);
                var removedHead = default(CommandServerCallTaskQueue<T>);
                var removedEnd = default(CommandServerCallTaskQueue<T>);
                do
                {
                    bool isRemoved = false;
                    System.Threading.Monitor.Enter(queueLock);
                    switch (head.CheckRemove(out next))
                    {
                        case 0:
                            try
                            {
                                queues.Remove(head.Key);
                                isRemoved = true;
                            }
                            catch { }
                            break;
                        case 1:
                            if (!object.ReferenceEquals(removeHead, CommandServerCallTaskQueue<T>.Null)) removeEnd.RemoveNext = head;
                            else removeHead = head;
                            removeEnd = head;
                            break;
                    }
                    System.Threading.Monitor.Exit(queueLock);
                    if (isRemoved)
                    {
                        if (removedHead != null) removedEnd.notNull().RemoveNext = head;
                        else removedHead = head;
                        removedEnd = head;
                    }
                }
                while ((head = next) != null);

                while (removedHead != null)
                {
                    try
                    {
                        onRemoved(removedHead);
                    }
                    catch (Exception exception)
                    {
                        Server.Log.ExceptionIgnoreException(exception, null, LogLevelEnum.AutoCSer | LogLevelEnum.Exception);
                    }
                    removedHead = removedHead.RemoveNext;
                }
            }
        }

        /// <summary>
        /// 异步任务队列关键字类型自定义属性
        /// </summary>
#if NetStandard21
        private static readonly CommandServerCallTaskQueueTypeAttribute? typeAttribute;
#else
        private static readonly CommandServerCallTaskQueueTypeAttribute typeAttribute;
#endif

        static CommandServerCallTaskQueueSet()
        {
            typeAttribute = typeof(T).GetCustomAttribute<CommandServerCallTaskQueueTypeAttribute>(false);
        }
    }
}
