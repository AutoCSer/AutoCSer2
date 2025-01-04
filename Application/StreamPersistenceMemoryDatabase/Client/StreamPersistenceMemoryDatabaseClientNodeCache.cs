using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 日志流持久化内存数据库客户端节点缓存，用于客户端单例
    /// </summary>
    /// <typeparam name="T">客户端节点类型</typeparam>
    public abstract class StreamPersistenceMemoryDatabaseClientNodeCache<T>
        where T : class
    {
        /// <summary>
        /// 客户端节点
        /// </summary>
#if NetStandard21
        protected Task<ResponseResult<T>>? nodeTask;
#else
        protected Task<ResponseResult<T>> nodeTask;
#endif
        /// <summary>
        /// IO 线程同步回调客户端节点
        /// </summary>
#if NetStandard21
        protected Task<ResponseResult<T>>? synchronousNodeTask;
#else
        protected Task<ResponseResult<T>> synchronousNodeTask;
#endif
        /// <summary>
        /// 客户端节点访问锁
        /// </summary>
        protected readonly System.Threading.SemaphoreSlim nodeLock = new System.Threading.SemaphoreSlim(1, 1);
        /// <summary>
        /// 获取客户端节点
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<T>> GetNode()
        {
            return nodeTask ?? getNode();
        }
        /// <summary>
        /// 获取客户端节点
        /// </summary>
        /// <returns></returns>
        protected abstract Task<ResponseResult<T>> getNode();
        /// <summary>
        /// 获取 IO 线程同步回调客户端节点，节点调用 await 后续操作不允许存在同步阻塞逻辑或者长时间占用 CPU 运算
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<ResponseResult<T>> GetSynchronousNode()
        {
            return synchronousNodeTask ?? getSynchronousNode();
        }
        /// <summary>
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
    /// 日志流持久化内存数据库客户端节点缓存，用于客户端单例
    /// </summary>
    /// <typeparam name="NT">客户端节点类型</typeparam>
    /// <typeparam name="ST">服务基础操作客户端接口类型</typeparam>
    public sealed class StreamPersistenceMemoryDatabaseClientNodeCache<NT, ST> : StreamPersistenceMemoryDatabaseClientNodeCache<NT>
        where ST : class, IServiceNodeClientNode
        where NT : class
    {
        /// <summary>
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        private readonly StreamPersistenceMemoryDatabaseClientCache<ST> client;
        /// <summary>
        /// 获取客户端节点委托
        /// </summary>
        private readonly Func<StreamPersistenceMemoryDatabaseClient<ST>, Task<ResponseResult<NT>>> getNodeTask;
        /// <summary>
        /// 日志流持久化内存数据库客户端节点缓存，用于客户端单例
        /// </summary>
        /// <param name="client">日志流持久化内存数据库客户端缓存</param>
        /// <param name="getNode">获取客户端节点委托</param>
        internal StreamPersistenceMemoryDatabaseClientNodeCache(StreamPersistenceMemoryDatabaseClientCache<ST> client, Func<StreamPersistenceMemoryDatabaseClient<ST>, Task<ResponseResult<NT>>> getNode)
        {
            this.client = client;
            getNodeTask = getNode;
        }
        /// <summary>
        /// 获取客户端节点
        /// </summary>
        /// <returns></returns>
        protected override async Task<ResponseResult<NT>> getNode()
        {
            var client = await this.client.GetClient();
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
