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
        /// 回调处理是否启动线程
        /// </summary>
        private readonly bool isThreadPool;
        /// <summary>
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// 异步回调 await 包装
        /// </summary>
        /// <param name="isThreadPool">回调处理是否启动线程，回调无阻塞的情况应该设置为 false 直接同步回调</param>
        public AwaitCallBack(bool isThreadPool = true)
        {
            this.isThreadPool = isThreadPool;
        }
        /// <summary>
        /// await 支持
        /// </summary>
        /// <returns></returns>
        public async Task Wait()
        {
            await this;
        }
        /// <summary>
        /// await 支持
        /// </summary>
        /// <returns></returns>
        public void GetResult() { }
        /// <summary>
        /// await 支持
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AwaitCallBack GetAwaiter()
        {
            return this;
        }
        /// <summary>
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
                if (isThreadPool) AutoCSer.Threading.ThreadPool.TinyBackground.Start(continuation);
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
        /// 返回值
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private T returnValue;
        /// <summary>
        /// 回调处理是否启动线程
        /// </summary>
        private readonly bool isThreadPool;
        /// <summary>
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        /// 异步回调 await 包装
        /// </summary>
        /// <param name="isThreadPool">回调处理是否启动线程，回调无阻塞的情况应该设置为 false 直接同步回调</param>
        public AwaitCallBack(bool isThreadPool = true) 
        {
            this.isThreadPool = isThreadPool;
        }
        /// <summary>
        /// await 支持
        /// </summary>
        /// <returns></returns>
        public async Task<T> Wait()
        {
            return await this;
        }
        /// <summary>
        /// await 支持
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T GetResult()
        {
            return returnValue;
        }
        /// <summary>
        /// await 支持
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public AwaitCallBack<T> GetAwaiter()
        {
            return this;
        }
        /// <summary>
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <returns></returns>
        public void Callback(T value)
        {
            returnValue = value;
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
            {
                if (isThreadPool) AutoCSer.Threading.ThreadPool.TinyBackground.Start(continuation);
                else continuation();
            }
        }
    }
}
