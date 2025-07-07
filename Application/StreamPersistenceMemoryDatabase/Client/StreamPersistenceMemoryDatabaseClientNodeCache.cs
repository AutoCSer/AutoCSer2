using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// Log stream persistence memory database client node cache for client singleton
    /// 日志流持久化内存数据库客户端节点缓存，用于客户端单例
    /// </summary>
    /// <typeparam name="T">Client node type
    /// 客户端节点类型</typeparam>
    public abstract class StreamPersistenceMemoryDatabaseClientNodeCache<T>
        where T : class
    {
        /// <summary>
        /// Client node
        /// 客户端节点
        /// </summary>
#if NetStandard21
        protected Task<ResponseResult<T>>? nodeTask;
#else
        protected Task<ResponseResult<T>> nodeTask;
#endif
        /// <summary>
        /// The client node of the IO thread synchronization callback
        /// IO 线程同步回调客户端节点
        /// </summary>
#if NetStandard21
        protected Task<ResponseResult<T>>? synchronousNodeTask;
#else
        protected Task<ResponseResult<T>> synchronousNodeTask;
#endif
        /// <summary>
        /// Client node access lock
        /// 客户端节点访问锁
        /// </summary>
        protected readonly System.Threading.SemaphoreSlim nodeLock = new System.Threading.SemaphoreSlim(1, 1);
        /// <summary>
        /// Get the client node
        /// 获取客户端节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<T>> GetNode()
        {
            return nodeTask ?? getNode();
        }
        /// <summary>
        /// Get the client node
        /// 获取客户端节点
        /// </summary>
        /// <returns></returns>
        protected abstract Task<ResponseResult<T>> getNode();
        /// <summary>
        /// Get the client node of the IO thread synchronous callback, node call await subsequent operation does not allow the existence of synchronization blocking logic or long CPU operation
        /// 获取 IO 线程同步回调客户端节点，节点调用 await 后续操作不允许存在同步阻塞逻辑或者长时间占用 CPU 运算
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<T>> GetSynchronousNode()
        {
            return synchronousNodeTask ?? getSynchronousNode();
        }
        /// <summary>
        /// Get the client node for the IO thread synchronous callback
        /// 获取 IO 线程同步回调客户端节点
        /// </summary>
        /// <returns></returns>
        private async Task<ResponseResult<T>> getSynchronousNode()
        {
            ResponseResult<T> node = await GetNode();
            if (node.IsSuccess)
            {
                if (synchronousNodeTask != null) return synchronousNodeTask.Result;
                node = ClientNode<T>.GetSynchronousCallback(node.Value.notNull());
                synchronousNodeTask = Task.FromResult(node);
            }
            return node;
        }
    }
    /// <summary>
    /// Log stream persistence memory database client node cache for client singleton
    /// 日志流持久化内存数据库客户端节点缓存，用于客户端单例
    /// </summary>
    /// <typeparam name="NT">Client node type
    /// 客户端节点类型</typeparam>
    /// <typeparam name="ST">Basic service operation client interface type
    /// 服务基础操作客户端接口类型</typeparam>
    public sealed class StreamPersistenceMemoryDatabaseClientNodeCache<NT, ST> : StreamPersistenceMemoryDatabaseClientNodeCache<NT>
        where ST : class, IServiceNodeClientNode
        where NT : class
    {
        /// <summary>
        /// Log stream persistence memory database client cache for client singleton
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        public readonly ClientTaskCache<ST> Client;
        /// <summary>
        /// Get client node delegate
        /// 获取客户端节点委托
        /// </summary>
        private readonly Func<StreamPersistenceMemoryDatabaseClient<ST>, Task<ResponseResult<NT>>> getNodeTask;
        /// <summary>
        /// Log stream persistence memory database client node cache for client singleton
        /// 日志流持久化内存数据库客户端节点缓存，用于客户端单例
        /// </summary>
        /// <param name="client">Log stream persistence memory database client cache
        /// 日志流持久化内存数据库客户端缓存</param>
        /// <param name="getNode">Get client node delegate
        /// 获取客户端节点委托</param>
        internal StreamPersistenceMemoryDatabaseClientNodeCache(ClientTaskCache<ST> client, Func<StreamPersistenceMemoryDatabaseClient<ST>, Task<ResponseResult<NT>>> getNode)
        {
            this.Client = client;
            getNodeTask = getNode;
        }
        /// <summary>
        /// Get the client node
        /// 获取客户端节点
        /// </summary>
        /// <returns></returns>
        protected override async Task<ResponseResult<NT>> getNode()
        {
            var client = await this.Client.GetClient();
            if (client != null)
            {
                if (nodeTask != null) return nodeTask.Result;
                await nodeLock.WaitAsync();
                try
                {
                    if (nodeTask != null) return nodeTask.Result;
                    ResponseResult<NT> node = await getNodeTask(client);
                    if (node.IsSuccess) nodeTask = Task.FromResult(node);
                    return node;
                }
                finally { nodeLock.Release(); }
            }
            return CallStateEnum.Unknown;
        }
    }
}
