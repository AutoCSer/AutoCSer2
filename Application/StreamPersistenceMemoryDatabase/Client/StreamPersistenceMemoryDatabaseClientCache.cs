using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 日志流持久化内存数据库客户端缓存，用于客户端单例
    /// </summary>
    /// <typeparam name="T">服务基础操作客户端接口类型</typeparam>
    public abstract class StreamPersistenceMemoryDatabaseClientCache<T>
        where T : class, IServiceNodeClientNode
    {
        /// <summary>
        /// 日志流持久化内存数据库客户端
        /// </summary>
#if NetStandard21
        protected Task<StreamPersistenceMemoryDatabaseClient<T>?>? clientTask;
#else
        protected Task<StreamPersistenceMemoryDatabaseClient<T>> clientTask;
#endif
        /// <summary>
        /// 获取日志流持久化内存数据库客户端
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Task<StreamPersistenceMemoryDatabaseClient<T>?> GetClient()
#else
        public Task<StreamPersistenceMemoryDatabaseClient<T>> GetClient()
#endif
        {
            return clientTask ?? getClient();
        }
        /// <summary>
        /// 获取日志流持久化内存数据库客户端
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        protected abstract Task<StreamPersistenceMemoryDatabaseClient<T>?> getClient();
#else
        protected abstract Task<StreamPersistenceMemoryDatabaseClient<T>> getClient();
#endif
        /// <summary>
        /// 获取日志流持久化内存数据库客户端节点
        /// </summary>
        /// <typeparam name="NT">客户端节点类型</typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public StreamPersistenceMemoryDatabaseClientNodeCache<NT> CreateNode<NT>(Func<StreamPersistenceMemoryDatabaseClient<T>, Task<ResponseResult<NT>>> getNodeTask)
            where NT : class
        {
            return new StreamPersistenceMemoryDatabaseClientNodeCache<NT, T>(this, getNodeTask);
        }
    }
    /// <summary>
    /// 日志流持久化内存数据库客户端缓存，用于客户端单例
    /// </summary>
    /// <typeparam name="NT">服务基础操作客户端接口类型</typeparam>
    /// <typeparam name="ET">命令客户端套接字事件类型</typeparam>
    public sealed class StreamPersistenceMemoryDatabaseClientCache<NT, ET> : StreamPersistenceMemoryDatabaseClientCache<NT>
        where NT : class, IServiceNodeClientNode
        where ET : CommandClientSocketEventTask<ET>, IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        public readonly AutoCSer.Net.CommandClientSocketEventCache<ET> ClientCache;
        /// <summary>
        /// 日志流持久化内存数据库客户端访问锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim clientLock;
        /// <summary>
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="client">命令客户端套接字事件</param>
        public StreamPersistenceMemoryDatabaseClientCache(AutoCSer.Net.CommandClientSocketEventCache<ET> client)
        {
            ClientCache = client;
            clientLock = new System.Threading.SemaphoreSlim(1, 1);
        }
        /// <summary>
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="client">命令客户端</param>
        public StreamPersistenceMemoryDatabaseClientCache(AutoCSer.Net.CommandClient client) : this(new AutoCSer.Net.CommandClientSocketEventCache<ET>(client)) { }
        /// <summary>
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="config">命令客户端配置</param>
        public StreamPersistenceMemoryDatabaseClientCache(AutoCSer.Net.CommandClientConfig config) : this(new AutoCSer.Net.CommandClient(config)) { }
        /// <summary>
        /// 获取日志流持久化内存数据库客户端
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        protected override async Task<StreamPersistenceMemoryDatabaseClient<NT>?> getClient()
#else
        protected override async Task<StreamPersistenceMemoryDatabaseClient<NT>> getClient()
#endif
        {
            var socketEvent = await ClientCache.SocketEvent.Wait();
            if (socketEvent != null)
            {
                if (clientTask != null) return clientTask.Result;
                await clientLock.WaitAsync();
                try
                {
                    if (clientTask != null) return clientTask.Result;
                    StreamPersistenceMemoryDatabaseClient<NT> clientNode = new StreamPersistenceMemoryDatabaseClient<NT>(socketEvent);
                    clientTask = AutoCSer.Common.GetCompletedTask(clientNode);
                    return clientNode;
                }
                finally { clientLock.Release(); }
            }
            return null;
        }
    }
}
