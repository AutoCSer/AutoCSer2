using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 异步回调 await 包装
    /// </summary>
    public sealed class AwaitCallBack : INotifyCompletion
    {
        /// <summary>
        /// await 异步回调
        /// </summary>
#if NetStandard21
        private Action? continuation;
#else
        private Action continuation;
#endif
        /// <summary>
        /// 回调是否调用 Task.Run 处理处理
        /// </summary>
        private readonly bool isTaskRun;
        /// <summary>
        /// Completed status
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// 异步回调 await 包装
        /// </summary>
        /// <param name="isTaskRun">默认为 false 同步回调，设置为 true 调用 Task.Run 处理</param>
        public AwaitCallBack(bool isTaskRun = false)
        {
            this.isTaskRun = isTaskRun;
        }
        /// <summary>
        /// await support
        /// await 支持
        /// </summary>
        /// <returns></returns>
        public async Task Wait()
        {
            await this;
        }
        /// <summary>
        /// await support
        /// await 支持
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void GetResult() { }
        /// <summary>
        /// await support
        /// await 支持
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AwaitCallBack GetAwaiter()
        {
            return this;
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
        /// 异步回调包装
        /// </summary>
        public void Callback()
        {
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
            {
                if (isTaskRun) Task.Run(continuation);
                else continuation();
            }
        }
    }
    /// <summary>
    /// 异步回调 await 包装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class AwaitCallBack<T> : INotifyCompletion
    {
        /// <summary>
        /// await 异步回调
        /// </summary>
#if NetStandard21
        private Action? continuation;
#else
        private Action continuation;
#endif
        /// <summary>
        /// Return value
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private T returnValue;
        /// <summary>
        /// 回调是否调用 Task.Run 处理处理
        /// </summary>
        private readonly bool isTaskRun;
        /// <summary>
        /// Completed status
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// 异步回调 await 包装
        /// </summary>
        /// <param name="isTaskRun">默认为 false 同步回调，设置为 true 调用 Task.Run 处理</param>
        public AwaitCallBack(bool isTaskRun = false) 
        {
            this.isTaskRun = isTaskRun;
        }
        /// <summary>
        /// await support
        /// await 支持
        /// </summary>
        /// <returns></returns>
        public async Task<T> Wait()
        {
            return await this;
        }
        /// <summary>
        /// await support
        /// await 支持
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T GetResult()
        {
            return returnValue;
        }
        /// <summary>
        /// await support
        /// await 支持
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AwaitCallBack<T> GetAwaiter()
        {
            return this;
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
        /// Asynchronous callback
        /// 异步回调
        /// </summary>
        /// <returns></returns>
        public void Callback(T value)
        {
            returnValue = value;
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
            {
                if (isTaskRun) Task.Run(continuation);
                else continuation();
            }
        }
    }
}
