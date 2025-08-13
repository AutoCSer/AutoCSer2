using AutoCSer.Extensions;
using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// Task queue controller service
    /// Task 队列控制器服务
    /// </summary>
    public abstract class CommandServerTaskQueueService
    {
        /// <summary>
        /// The queue for asynchronous server calls
        /// 服务端异步调用队列
        /// </summary>
        public readonly CommandServerCallTaskQueue Queue;
        /// <summary>
        /// The socket of the currently executing task
        /// 当前执行任务套接字
        /// </summary>
        public CommandServerSocket Socket { get; internal set; }
        /// <summary>
        /// Task queue controller service
        /// Task 队列控制器服务
        /// </summary>
        /// <param name="task"></param>
        protected CommandServerTaskQueueService(CommandServerCallTaskQueueNode task)
        {
            Socket = task.GetSocketQueue(out Queue);
        }

        /// <summary>
        /// Create the controller task queue
        /// 创建控制器 Task 队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <param name="task"></param>
        /// <param name="key"></param>
        /// <param name="getQueue"></param>
        /// <returns></returns>
        public static T CreateTaskQueue<T, KT>(CommandServerCallTaskQueueNode task, ref KT key, Func<CommandServerCallTaskQueueNode, KT, T> getQueue)
            where KT : IEquatable<KT>
        {
            T queue = getQueue(task, key);
            task.Queue.TaskQueue = (queue as CommandServerTaskQueueService).notNull();
            return queue;
        }
        /// <summary>
        /// Create the controller task queue
        /// 创建控制器 Task 队列委托
        /// </summary>
        /// <param name="task"></param>
        /// <param name="key"></param>
        /// <param name="getQueue"></param>
        /// <returns></returns>
        internal delegate T CreateTaskQueueDelegate<T, KT>(CommandServerCallTaskQueueNode task, ref KT key, Func<CommandServerCallTaskQueueNode, KT, T> getQueue)
            where KT : IEquatable<KT>;
    }
    /// <summary>
    /// Task queue controller service
    /// Task 队列控制器服务
    /// </summary>
    /// <typeparam name="T">Queue keyword type
    /// 队列关键字类型</typeparam>
    public abstract class CommandServerTaskQueueService<T> : CommandServerTaskQueueService
        where T : IEquatable<T>
    {
        /// <summary>
        /// Queue keyword
        /// 队列关键字
        /// </summary>
        public readonly T Key;
        /// <summary>
        /// Task queue controller service
        /// Task 队列控制器服务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="key"></param>
        public CommandServerTaskQueueService(CommandServerCallTaskQueueNode task, T key) : base(task)
        {
            Key = key;
        }
    }
}
