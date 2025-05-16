using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// 服务端异步调用队列委托包装
    /// </summary>
    internal sealed class CommandServerCallTaskQueueFunc
    {
        /// <summary>
        /// 队列委托
        /// </summary>
        private readonly Func<Task> getTask;
        /// <summary>
        /// 服务端异步调用队列委托包装
        /// </summary>
        /// <param name="getTask">队列委托</param>
        internal CommandServerCallTaskQueueFunc(Func<Task> getTask)
        {
            this.getTask = getTask;
        }
        /// <summary>
        /// 队列委托包装
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Task GetTask(CommandServerCallTaskQueue queue)
        {
            return getTask();
        }
    }
    /// <summary>
    /// 服务端异步调用队列委托包装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class CommandServerCallTaskQueueFunc<T>
    {
        /// <summary>
        /// 队列委托
        /// </summary>
        private readonly Func<Task<T>> getTask;
        /// <summary>
        /// 服务端异步调用队列委托包装
        /// </summary>
        /// <param name="getTask">队列委托</param>
        internal CommandServerCallTaskQueueFunc(Func<Task<T>> getTask)
        {
            this.getTask = getTask;
        }
        /// <summary>
        /// 队列委托包装
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Task<T> GetTask(CommandServerCallTaskQueue queue)
        {
            return getTask();
        }
    }
}
