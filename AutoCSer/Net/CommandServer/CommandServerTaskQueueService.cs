using AutoCSer.Extensions;
using System;

namespace AutoCSer.Net
{
    /// <summary>
    /// 命令服务 Task 队列服务实例
    /// </summary>
    public abstract class CommandServerTaskQueueService
    {
        /// <summary>
        /// 服务端异步调用队列
        /// </summary>
        public readonly CommandServerCallTaskQueue Queue;
        /// <summary>
        /// 当前执行任务套接字
        /// </summary>
        public CommandServerSocket Socket { get; internal set; }
        /// <summary>
        /// 命令服务 Task 队列
        /// </summary>
        /// <param name="task"></param>
        protected CommandServerTaskQueueService(CommandServerCallTaskQueueNode task)
        {
            Socket = task.GetSocket(out Queue);
        }

        /// <summary>
        /// 创建命令服务 Task 队列
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
        /// 创建命令服务 Task 队列委托
        /// </summary>
        /// <param name="task"></param>
        /// <param name="key"></param>
        /// <param name="getQueue"></param>
        /// <returns></returns>
        internal delegate T CreateTaskQueueDelegate<T, KT>(CommandServerCallTaskQueueNode task, ref KT key, Func<CommandServerCallTaskQueueNode, KT, T> getQueue)
            where KT : IEquatable<KT>;
    }
    /// <summary>
    /// 命令服务 Task 队列服务实例
    /// </summary>
    /// <typeparam name="T">队列关键字类型</typeparam>
    public abstract class CommandServerTaskQueueService<T> : CommandServerTaskQueueService
        where T : IEquatable<T>
    {
        /// <summary>
        /// 队列关键字
        /// </summary>
        public readonly T Key;
        /// <summary>
        /// 命令服务 Task 队列
        /// </summary>
        /// <param name="task"></param>
        /// <param name="key"></param>
        public CommandServerTaskQueueService(CommandServerCallTaskQueueNode task, T key) : base(task)
        {
            Key = key;
        }
    }
}
