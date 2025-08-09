using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端异步调用队列任务
    /// </summary>
    public sealed class CommandServerCallTaskQueueCustomTask : CommandServerCallTaskQueueCustomTaskNode, INotifyCompletion
    {
        /// <summary>
        /// 服务端异步调用队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous"></param>
        internal CommandServerCallTaskQueueCustomTask(Func<CommandServerCallTaskQueue, Task> getTask, bool isSynchronous) : base(getTask, isSynchronous) { }
        /// <summary>
        /// 服务端异步调用队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous"></param>
        internal CommandServerCallTaskQueueCustomTask(Func<Task> getTask, bool isSynchronous) : base(getTask, isSynchronous) { }
        /// <summary>
        /// Wait for the return result of the task execution
        /// 等待任务执行返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task Wait()
        {
            await this;
        }
        /// <summary>
        /// Gets the result of the task execution (Only for supporting await)
        /// 获取任务执行返回结果（仅用于支持 await）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void GetResult()
        {
            if (exception != null) throw exception;
        }
        /// <summary>
        /// Set asynchronous callback
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandServerCallTaskQueueCustomTask GetAwaiter()
        {
            return this;
        }
    }
    /// <summary>
    /// 服务端异步调用队列任务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CommandServerCallTaskQueueCustomTask<T> : CommandServerCallTaskQueueCustomTaskNode<T>, INotifyCompletion
    {
        /// <summary>
        /// 服务端异步调用队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous"></param>
        internal CommandServerCallTaskQueueCustomTask(Func<CommandServerCallTaskQueue, Task<T>> getTask, bool isSynchronous) : base(getTask, isSynchronous) { }
        /// <summary>
        /// 服务端异步调用队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous"></param>
        internal CommandServerCallTaskQueueCustomTask(Func<Task<T>> getTask, bool isSynchronous) : base(getTask, isSynchronous) { }
        /// <summary>
        /// Wait for the return result of the task execution
        /// 等待任务执行返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public async Task<T> Wait()
        {
            return await this;
        }
        /// <summary>
        /// Gets the result of the task execution, return the default value before the result is returned (Only for supporting await)
        /// 获取任务执行返回结果，结果未返回之前则返回默认值（仅用于支持 await）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T GetResult()
        {
            if (exception != null) throw exception;
            return value;
        }
        /// <summary>
        /// Set asynchronous callback
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommandServerCallTaskQueueCustomTask<T> GetAwaiter()
        {
            return this;
        }
    }
}
