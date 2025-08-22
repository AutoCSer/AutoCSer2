using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// API encapsulation nodes that directly return values
    /// 直接返回值的 API 封装节点
    /// </summary>
    /// <typeparam name="T">Client node interface type
    /// 客户端节点接口类型</typeparam>
    public abstract class ClientReturnValueNode<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// Client node
        /// 客户端节点
        /// </summary>
#if NetStandard21
        protected T? node;
#else
        protected T node;
#endif
        /// <summary>
        /// 获取客户端节点等待锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim waitLock;
        /// <summary>
        /// Error message
        /// </summary>
#if NetStandard21
        private string? errorMessage;
#else
        private string errorMessage;
#endif
        /// <summary>
        /// Whether errors and exceptions are ignored
        /// 是否忽略错误与异常
        /// </summary>
        protected readonly bool isIgnoreError;
        /// <summary>
        /// The return type of the call
        /// 调用返回类型
        /// </summary>
        private CommandClientReturnTypeEnum returnType;
        /// <summary>
        /// Call status
        /// 调用状态
        /// </summary>
        private CallStateEnum callState;
        /// <summary>
        /// 是否需要释放节点等待锁
        /// </summary>
        private bool isReleaseWaitLock;
        /// <summary>
        /// Whether resources have been released
        /// 是否已经释放资源
        /// </summary>
        private bool isDisposed;
        /// <summary>
        /// API encapsulation nodes that directly return values
        /// 直接返回值的 API 封装节点
        /// </summary>
        /// <param name="node">Log stream persistence memory database client node cache for client singleton
        /// 日志流持久化内存数据库客户端节点缓存，用于客户端单例</param>
        /// <param name="isIgnoreError">A default value of false indicates that exceptions and error messages are not ignored
        /// 默认为 false 表示忽略异常与错误信息</param>
        /// <param name="isSynchronousCallback">The default value of false indicates that the IO thread synchronization callback is not used; otherwise, the subsequent operations of the API call await are not allowed to have synchronization blocking logic or long-term CPU occupation operations
        /// 默认为 false 表示不使用 IO 线程同步回调，否则 API 调用 await 后续操作不允许存在同步阻塞逻辑或者长时间占用 CPU 运算</param>
        public ClientReturnValueNode(StreamPersistenceMemoryDatabaseClientNodeCache<T> node, bool isIgnoreError = false, bool isSynchronousCallback = false)
        {
            this.isIgnoreError = isIgnoreError;
            returnType = AutoCSer.Net.CommandClientReturnTypeEnum.WaitConnect;
            callState = CallStateEnum.ClientLoadUnfinished;
            isReleaseWaitLock = true;
            waitLock = new System.Threading.SemaphoreSlim(0, 1);
           getNode(node, isSynchronousCallback).Catch();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            returnType = AutoCSer.Net.CommandClientReturnTypeEnum.ClientDisposed;
            callState = CallStateEnum.ClientDisposed;
        }
        /// <summary>
        /// Get the client node
        /// 获取客户端节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isSynchronousCallback"></param>
        /// <returns></returns>
        private async System.Threading.Tasks.Task getNode(StreamPersistenceMemoryDatabaseClientNodeCache<T> node, bool isSynchronousCallback)
        {
            try
            {
                if (isSynchronousCallback)
                {
                    do
                    {
                        if (getNode(node, await node.GetSynchronousNode())) return;
                        await System.Threading.Tasks.Task.Delay(1);
                    }
                    while (!isDisposed);
                }
                else
                {
                    do
                    {
                        if (getNode(node, await node.GetNode())) return;
                        await System.Threading.Tasks.Task.Delay(1);
                    }
                    while (!isDisposed);
                }
            }
            finally { releaseWaitLock(); }
        }
        /// <summary>
        /// Get the client node
        /// 获取客户端节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="result"></param>
        /// <returns>Whether to end the program
        /// 是否结束程序</returns>
        private bool getNode(StreamPersistenceMemoryDatabaseClientNodeCache<T> node, ResponseResult<T> result)
        {
            if (result.IsSuccess)
            {
                this.node = result.Value;
                returnType = AutoCSer.Net.CommandClientReturnTypeEnum.Success;
                callState = CallStateEnum.Success;
                releaseWaitLock();
                return true;
            }
            returnType = result.ReturnType;
            callState = result.CallState;
            errorMessage = result.ErrorMessage;
            releaseWaitLock();
            if (node.IsDisposed)
            {
                Dispose();
                return true;
            }
            return isDisposed;
        }
        /// <summary>
        /// 释放节点等待锁
        /// </summary>
        private void releaseWaitLock()
        {
            if (isReleaseWaitLock)
            {
                isReleaseWaitLock = false;
                waitLock.Release();
            }
        }
        /// <summary>
        /// Get the client node
        /// 获取客户端节点
        /// </summary>
        /// <returns></returns>
        public ResponseResult<T> GetNodeResult()
        {
            if (node != null) return node;
            if (returnType == CommandClientReturnTypeEnum.Success) return new ResponseResult<T>(callState);
            return new ResponseResult<T>(returnType, errorMessage);
        }
        /// <summary>
        /// Get the client node
        /// 获取客户端节点
        /// </summary>
        /// <returns></returns>
        public Task<ResponseResult<T>> GetNodeResultAsync()
        {
            return !isReleaseWaitLock ? Task.FromResult(GetNodeResult()) : getNodeResultAsync();
        }
        /// <summary>
        /// Get the client node
        /// 获取客户端节点
        /// </summary>
        /// <returns></returns>
        private async Task<ResponseResult<T>> getNodeResultAsync()
        {
            await waitLock.WaitAsync();
            try
            {
                return GetNodeResult();
            }
            finally { waitLock.Release(); }
        }
    }
}
