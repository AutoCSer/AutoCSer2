using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Used for await
    /// 用于 await
    /// </summary>
    public sealed class LocalServiceReturnValue : INotifyCompletion
    {
        /// <summary>
        /// Return value node
        /// 返回值节点
        /// </summary>
        public readonly LocalServiceQueueNode<LocalResult> Node;
        /// <summary>
        /// Asynchronous callback
        /// 异步回调
        /// </summary>
#if NetStandard21
        private Action? continuation;
#else
        private Action continuation;
#endif
        /// <summary>
        /// Completed status
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get { return Node.IsCompleted; } }
#if AOT
        /// <summary>
        /// Error message
        /// </summary>
        public string? ErrorMessage { get { return Node.Result.ErrorMessage; } }
#endif
        /// <summary>
        /// Call status
        /// 调用状态
        /// </summary>
        public CallStateEnum State { get { return Node.Result.CallState; } }
        /// <summary>
        /// Whether errors and exceptions are ignored
        /// 是否忽略错误与异常
        /// </summary>
        private readonly bool isIgnoreError;
        /// <summary>
        /// Used for await
        /// 用于 await
        /// </summary>
        /// <param name="node">Return value node
        /// 返回值节点</param>
        /// <param name="isIgnoreError">Whether errors and exceptions are ignored
        /// 是否忽略错误与异常</param>
        internal LocalServiceReturnValue(LocalServiceQueueNode<LocalResult> node, bool isIgnoreError)
        {
            Node = node;
            this.isIgnoreError = isIgnoreError;
            if (!node.IsCompleted) node.OnCompleted(onCompleted);
            else continuation = Common.EmptyAction;
        }
        /// <summary>
        /// Complete the callback operation
        /// 完成回调操作
        /// </summary>
        private void onCompleted()
        {
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, AutoCSer.Common.EmptyAction, null) != null) continuation();
        }
        /// <summary>
        /// Set asynchronous callback
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// Wait for the command call to return the result
        /// 等待命令调用返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public async Task Wait()
#else
        public async Task Wait()
#endif
        {
            await this;
        }
        /// <summary>
        /// Get the result of the command call, return an error message before the result is returned (Only for supporting await)
        /// 获取命令调用结果，结果未返回之前则返回错误信息（仅用于支持 await）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void GetResult()
#else
        public void GetResult()
#endif
        {
            Node.Result.GetValue(isIgnoreError);
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LocalServiceReturnValue GetAwaiter()
        {
            return this;
        }
    }
    /// <summary>
    /// Return value (await T)
    /// 返回值（await T）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LocalServiceReturnValue<T> : INotifyCompletion
    {
        /// <summary>
        /// Return value node
        /// 返回值节点
        /// </summary>
        public readonly LocalServiceQueueNode<LocalResult<T>> Node;
        /// <summary>
        /// Asynchronous callback
        /// 异步回调
        /// </summary>
#if NetStandard21
        private Action? continuation;
#else
        private Action continuation;
#endif
        /// <summary>
        /// Completed status
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get { return Node.IsCompleted; } }
        /// <summary>
        /// Exception information
        /// 异常信息
        /// </summary>
#if NetStandard21
        public Exception? Exception { get { return Node.Result.Exception; } }
#else
        public Exception Exception { get { return Node.Result.Exception; } }
#endif
        /// <summary>
        /// Call status
        /// 调用状态
        /// </summary>
        public CallStateEnum State { get { return Node.Result.CallState;  } }
        /// <summary>
        /// Return value
        /// </summary>
#if NetStandard21
        public T? ReturnValue { get { return Node.Result.Value; } }
#else
        public T ReturnValue { get { return Node.Result.Value; } }
#endif
        /// <summary>
        /// Whether errors and exceptions are ignored
        /// 是否忽略错误与异常
        /// </summary>
        private readonly bool isIgnoreError;
        /// <summary>
        /// Return value (await T)
        /// 返回值（await T）
        /// </summary>
        /// <param name="node">Return value node
        /// 返回值节点</param>
        /// <param name="isIgnoreError">Whether errors and exceptions are ignored
        /// 是否忽略错误与异常</param>
        internal LocalServiceReturnValue(LocalServiceQueueNode<LocalResult<T>> node, bool isIgnoreError)
        {
            Node = node;
            this.isIgnoreError = isIgnoreError;
            if (!node.IsCompleted) node.OnCompleted(onCompleted);
            else continuation = Common.EmptyAction;
        }
        /// <summary>
        /// Complete the callback operation
        /// 完成回调操作
        /// </summary>
        private void onCompleted()
        {
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, AutoCSer.Common.EmptyAction, null) != null) continuation();
        }
        /// <summary>
        /// Set asynchronous callback
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void OnCompleted(Action continuation)
        {
            if (System.Threading.Interlocked.CompareExchange(ref this.continuation, continuation, null) != null) continuation();
        }
        /// <summary>
        /// Wait for the command call to return the result
        /// 等待命令调用返回结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public async Task<T?> Wait()
#else
        public async Task<T> Wait()
#endif
        {
            return await this;
        }
        /// <summary>
        /// Get the result of the command call, return an error message before the result is returned (Only for supporting await)
        /// 获取命令调用结果，结果未返回之前则返回错误信息（仅用于支持 await）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public T? GetResult()
#else
        public T GetResult()
#endif
        {
            return Node.Result.GetValue(isIgnoreError);
        }
        /// <summary>
        /// Get the awaiter object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LocalServiceReturnValue<T> GetAwaiter()
        {
            return this;
        }
    }
}
