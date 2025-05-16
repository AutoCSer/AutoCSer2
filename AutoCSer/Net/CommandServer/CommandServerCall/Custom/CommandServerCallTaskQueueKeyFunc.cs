using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 服务端异步调用队列委托包装
    /// </summary>
    /// <typeparam name="KT"></typeparam>
    internal sealed class CommandServerCallTaskQueueKeyFunc<KT>
    {
        /// <summary>
        /// 队列委托
        /// </summary>
        private readonly Func<CommandServerCallTaskQueue, KT, Task> getTask;
        /// <summary>
        /// 关键字
        /// </summary>
        private readonly KT key;
        /// <summary>
        /// 服务端异步调用队列委托包装
        /// </summary>
        /// <param name="getTask">队列委托</param>
        /// <param name="key">关键字</param>
        internal CommandServerCallTaskQueueKeyFunc(Func<CommandServerCallTaskQueue, KT, Task> getTask, KT key)
        {
            this.getTask = getTask;
            this.key = key;
        }
        /// <summary>
        /// 队列委托包装
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Task GetTask(CommandServerCallTaskQueue queue)
        {
            return getTask(queue, key);
        }
    }
    /// <summary>
    /// 服务端异步调用队列委托包装
    /// </summary>
    /// <typeparam name="KT"></typeparam>
    /// <typeparam name="T"></typeparam>
    internal sealed class CommandServerCallTaskQueueKeyFunc<KT, T>
    {
        /// <summary>
        /// 队列委托
        /// </summary>
        private readonly Func<CommandServerCallTaskQueue, KT, Task<T>> getTask;
        /// <summary>
        /// 关键字
        /// </summary>
        private readonly KT key;
        /// <summary>
        /// 服务端异步调用队列委托包装
        /// </summary>
        /// <param name="getTask">队列委托</param>
        /// <param name="key">关键字</param>
        internal CommandServerCallTaskQueueKeyFunc(Func<CommandServerCallTaskQueue, KT, Task<T>> getTask, KT key)
        {
            this.getTask = getTask;
            this.key = key;
        }
        /// <summary>
        /// 队列委托包装
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Task<T> GetTask(CommandServerCallTaskQueue queue)
        {
            return getTask(queue, key);
        }
    }
}
