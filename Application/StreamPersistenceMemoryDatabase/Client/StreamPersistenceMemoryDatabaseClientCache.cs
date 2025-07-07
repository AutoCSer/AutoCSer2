using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// Log stream persistence memory database client cache for client singleton
    /// 日志流持久化内存数据库客户端缓存，用于客户端单例
    /// </summary>
    /// <typeparam name="NT">Basic service operation client interface type
    /// 服务基础操作客户端接口类型</typeparam>
    /// <typeparam name="ET">Command the client socket event type
    /// 命令客户端套接字事件类型</typeparam>
    public class StreamPersistenceMemoryDatabaseClientCache<NT, ET> : ClientTaskCache<NT>
        where NT : class, IServiceNodeClientNode
        where ET : CommandClientSocketEventTask<ET>, IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        public readonly AutoCSer.Net.CommandClientSocketEventCache<ET> ClientCache;
        /// <summary>
        /// Log stream persistence memory database client access lock
        /// 日志流持久化内存数据库客户端访问锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim clientLock;
        /// <summary>
        /// Log stream persistence memory database client cache for client singleton
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="client">Command client socket events
        /// 命令客户端套接字事件</param>
        public StreamPersistenceMemoryDatabaseClientCache(AutoCSer.Net.CommandClientSocketEventCache<ET> client)
        {
            ClientCache = client;
            clientLock = new System.Threading.SemaphoreSlim(1, 1);
        }
        /// <summary>
        /// Log stream persistence memory database client cache for client singleton
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="client">Command client</param>
        public StreamPersistenceMemoryDatabaseClientCache(AutoCSer.Net.ICommandClient client) : this(new AutoCSer.Net.CommandClientSocketEventCache<ET>(client)) { }
        /// <summary>
        /// Log stream persistence memory database client cache for client singleton
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="config">Command client configuration
        /// 命令客户端配置</param>
        public StreamPersistenceMemoryDatabaseClientCache(AutoCSer.Net.CommandClientConfig config) : this(new AutoCSer.Net.CommandClient(config)) { }
        /// <summary>
        /// Get the log stream persistent memory database client
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
    /// <summary>
    /// Log stream persistence memory database client cache for client singleton
    /// 日志流持久化内存数据库客户端缓存，用于客户端单例
    /// </summary>
    /// <typeparam name="T">Command the client socket event type
    /// 命令客户端套接字事件类型</typeparam>
    public sealed class StreamPersistenceMemoryDatabaseClientCache<T> : StreamPersistenceMemoryDatabaseClientCache<IServiceNodeClientNode, T>
        where T : CommandClientSocketEventTask<T>, IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// Log stream persistence memory database client cache for client singleton
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="client">Command client socket events
        /// 命令客户端套接字事件</param>
        public StreamPersistenceMemoryDatabaseClientCache(AutoCSer.Net.CommandClientSocketEventCache<T> client) : base(client) { }
        /// <summary>
        /// Log stream persistence memory database client cache for client singleton
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="client">Command client</param>
        public StreamPersistenceMemoryDatabaseClientCache(AutoCSer.Net.ICommandClient client) : base(client) { }
        /// <summary>
        /// Log stream persistence memory database client cache for client singleton
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="config">Command client configuration
        /// 命令客户端配置</param>
        public StreamPersistenceMemoryDatabaseClientCache(AutoCSer.Net.CommandClientConfig config) : base(config) { }
    }
}
