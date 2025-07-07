using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Net
{
    /// <summary>
    /// 服务端异步调用队列任务
    /// </summary>
    public sealed class CommandServerCallTaskQueueExceptionTask : CommandServerCallTaskQueueCustomTaskNode, INotifyCompletion
    {
        /// <summary>
        /// 服务端异步调用队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous"></param>
        internal CommandServerCallTaskQueueExceptionTask(Func<CommandServerCallTaskQueue, Task> getTask, bool isSynchronous) : base(getTask, isSynchronous) { }
        /// <summary>
        /// 服务端异步调用队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous"></param>
        internal CommandServerCallTaskQueueExceptionTask(Func<Task> getTask, bool isSynchronous) : base(getTask, isSynchronous) { }

        /// <summary>
        /// Wait for the return result of the task execution
        /// 等待任务执行返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public async Task<Exception?> Wait()
#else
        public async Task<Exception> Wait()
#endif
        {
            return await this;
        }
        /// <summary>
        /// Gets the result of the task execution
        /// 获取任务执行返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Exception? GetResult()
#else
        public Exception GetResult()
#endif
        {
            return exception;
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
        public CommandServerCallTaskQueueExceptionTask GetAwaiter()
        {
            return this;
        }
    }
    /// <summary>
    /// 服务端异步调用队列任务
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class CommandServerCallTaskQueueExceptionTask<T> : CommandServerCallTaskQueueCustomTaskNode<T>, INotifyCompletion
    {
        /// <summary>
        /// 服务端异步调用队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous"></param>
        internal CommandServerCallTaskQueueExceptionTask(Func<CommandServerCallTaskQueue, Task<T>> getTask, bool isSynchronous) : base(getTask, isSynchronous) { }
        /// <summary>
        /// 服务端异步调用队列任务
        /// </summary>
        /// <param name="getTask"></param>
        /// <param name="isSynchronous"></param>
        internal CommandServerCallTaskQueueExceptionTask(Func<Task<T>> getTask, bool isSynchronous) : base(getTask, isSynchronous) { }

        /// <summary>
        /// Wait for the return result of the task execution
        /// 等待任务执行返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public async Task<KeyValue<T?, Exception?>> Wait()
#else
        public async Task<KeyValue<T, Exception>> Wait()
#endif
        {
            return await this;
        }
        /// <summary>
        /// Gets the result of the task execution
        /// 获取任务执行返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public KeyValue<T?, Exception?> GetResult()
#else
        public KeyValue<T, Exception> GetResult()
#endif
        {
#if NetStandard21
            return new KeyValue<T?, Exception?>(value, exception);
#else
            return new KeyValue<T, Exception>(value, exception);
#endif
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
        public CommandServerCallTaskQueueExceptionTask<T> GetAwaiter()
        {
            return this;
        }
    }
}
