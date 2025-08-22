using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// The log stream persists the in-memory database local service queue node, and await T returns the call result
    /// 日志流持久化内存数据库本地服务队列节点，await T 返回调用结果
    /// </summary>
    /// <typeparam name="T">Return the data type of the result
    /// 返回结果数据类型</typeparam>
    [AutoCSer.CodeGenerator.AwaitResultType]
    public abstract class LocalServiceQueueNode<T> : ReadWriteQueueNode, INotifyCompletion
    {
        /// <summary>
        /// Log stream persistence memory database local service
        /// 日志流持久化内存数据库本地服务
        /// </summary>
        protected readonly LocalService service;
        /// <summary>
        /// Asynchronous callback
        /// 异步回调
        /// </summary>
#if NetStandard21
        protected Action? continuation;
#else
        protected Action continuation;
#endif
        /// <summary>
        /// Return result
        /// 返回结果
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        internal T Result;
        /// <summary>
        /// Completed status
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; protected set; }
        /// <summary>
        /// The log stream persists the in-memory database local service queue node
        /// 日志流持久化内存数据库本地服务队列节点
        /// </summary>
        /// <param name="service">Log stream persistence memory database local service
        /// 日志流持久化内存数据库本地服务</param>
        internal LocalServiceQueueNode(LocalService service)
        {
            this.service = service;
        }
        /// <summary>
        /// Task call completion processing
        /// 任务调用完成处理
        /// </summary>
        /// <param name="isSynchronousCallback"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void completed(bool isSynchronousCallback)
        {
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
            {
                if (isSynchronousCallback) continuation();
                else Task.Run(continuation);
            }
        }
        /// <summary>
        /// Task call completion processing
        /// 任务调用完成处理
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void completed()
        {
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
            {
                Task.Run(continuation);
            }
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
        /// await support
        /// await 支持
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LocalServiceQueueNode<T> GetAwaiter()
        {
            return this;
        }
        /// <summary>
        /// Add to the queue
        /// 添加到队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal LocalServiceQueueNode<T> AppendWrite()
        {
            service.CommandServerCallQueue.AppendWriteOnly(this);
            return this;
        }
        /// <summary>
        /// Only for supporting await
        /// 仅用于支持 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T GetResult()
        {
            return Result;
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
    }
}
