using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化内存数据库本地服务队列节点 await T
    /// </summary>
    /// <typeparam name="T">返回结果类型</typeparam>
    [AutoCSer.CodeGenerator.AwaitResultType]
    public abstract class LocalServiceQueueNode<T> : QueueTaskNode, INotifyCompletion
    {
        /// <summary>
        /// 日志流持久化内存数据库本地服务
        /// </summary>
        protected readonly LocalService service;
        /// <summary>
        /// 异步回调
        /// </summary>
        private Action continuation;
        /// <summary>
        /// 返回结果
        /// </summary>
        protected T result;
        /// <summary>
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get; protected set; }
        /// <summary>
        /// 本地服务获取节点标识
        /// </summary>
        /// <param name="service">日志流持久化内存数据库本地服务</param>
        internal LocalServiceQueueNode(LocalService service)
        {
            this.service = service;
        }
        /// <summary>
        /// 完成处理
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void completed()
        {
            IsCompleted = true;
            if (continuation != null || System.Threading.Interlocked.CompareExchange(ref continuation, Common.EmptyAction, null) != null)
            {
                if (!service.IsSynchronousCallback) Task.Run(continuation);
                else continuation();
            }
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
        /// 获取 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LocalServiceQueueNode<T> GetAwaiter()
        {
            return this;
        }
        /// <summary>
        /// 添加到队列
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal LocalServiceQueueNode<T> AppendQueue()
        {
            service.CommandServerCallQueue.AddOnly(this);
            return this;
        }
        /// <summary>
        /// 获取命令调用结果
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T GetResult()
        {
            return result;
        }
        /// <summary>
        /// await 支持
        /// </summary>
        /// <returns></returns>
        public async Task<T> Wait()
        {
            return await this;
        }
    }
}
