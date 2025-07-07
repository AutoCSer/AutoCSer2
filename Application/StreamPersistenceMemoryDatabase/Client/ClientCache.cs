using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Log stream persistence memory database client cache for client singleton
    /// 日志流持久化内存数据库客户端缓存，用于客户端单例
    /// </summary>
    /// <typeparam name="NT">Basic service operation client interface type
    /// 服务基础操作客户端接口类型</typeparam>
    /// <typeparam name="ET">Command the client socket event type
    /// 命令客户端套接字事件类型</typeparam>
    public class ClientCache<NT, ET> : ClientTaskCache<NT>
        where NT : class, IServiceNodeClientNode
        where ET : CommandClientSocketEventTask<ET>
    {
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        public readonly AutoCSer.Net.CommandClientSocketEventCache<ET> Client;
        /// <summary>
        /// Log stream persistence memory database client access lock
        /// 日志流持久化内存数据库客户端访问锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim clientLock;
        /// <summary>
        /// Create a log stream persistence in-memory database client instance
        /// 创建日志流持久化内存数据库客户端实例
        /// </summary>
        private readonly Func<ET, IStreamPersistenceMemoryDatabaseClientSocketEvent> createClient;
        /// <summary>
        /// Log stream persistence memory database client cache for client singleton
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="client">Command client socket events
        /// 命令客户端套接字事件</param>
        /// <param name="createClient">Create a log stream persistence in-memory database client instance
        /// 创建日志流持久化内存数据库客户端实例</param>
        public ClientCache(AutoCSer.Net.CommandClientSocketEventCache<ET> client, Func<ET, IStreamPersistenceMemoryDatabaseClientSocketEvent> createClient)
        {
            Client = client;
            this.createClient = createClient;
            clientLock = new System.Threading.SemaphoreSlim(1, 1);
        }
        /// <summary>
        /// Log stream persistence memory database client cache for client singleton
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="client">Command client</param>
        /// <param name="createClient">Create a log stream persistence in-memory database client instance
        /// 创建日志流持久化内存数据库客户端实例</param>
        public ClientCache(AutoCSer.Net.ICommandClient client, Func<ET, IStreamPersistenceMemoryDatabaseClientSocketEvent> createClient) : this(new AutoCSer.Net.CommandClientSocketEventCache<ET>(client), createClient) { }
        /// <summary>
        /// Log stream persistence memory database client cache for client singleton
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="config">Command client configuration
        /// 命令客户端配置</param>
        /// <param name="createClient">Create a log stream persistence in-memory database client instance
        /// 创建日志流持久化内存数据库客户端实例</param>
        public ClientCache(AutoCSer.Net.CommandClientConfig config, Func<ET, IStreamPersistenceMemoryDatabaseClientSocketEvent> createClient) : this(new AutoCSer.Net.CommandClient(config), createClient) { }
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
            var socketEvent = await Client.SocketEvent.Wait();
            if (socketEvent != null)
            {
                if (clientTask != null) return clientTask.Result;
                await clientLock.WaitAsync();
                try
                {
                    if (clientTask != null) return clientTask.Result;
                    StreamPersistenceMemoryDatabaseClient<NT> clientNode = new StreamPersistenceMemoryDatabaseClient<NT>(createClient(socketEvent));
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
    public sealed class ClientCache<T> : ClientCache<IServiceNodeClientNode, T>
        where T : CommandClientSocketEventTask<T>
    {
        /// <summary>
        /// Log stream persistence memory database client cache for client singleton
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="client">Command client socket events
        /// 命令客户端套接字事件</param>
        /// <param name="createClient">Create a log stream persistence in-memory database client instance
        /// 创建日志流持久化内存数据库客户端实例</param>
        public ClientCache(AutoCSer.Net.CommandClientSocketEventCache<T> client, Func<T, IStreamPersistenceMemoryDatabaseClientSocketEvent> createClient) : base(client, createClient) { }
        /// <summary>
        /// Log stream persistence memory database client cache for client singleton
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="client">Command client</param>
        /// <param name="createClient">Create a log stream persistence in-memory database client instance
        /// 创建日志流持久化内存数据库客户端实例</param>
        public ClientCache(AutoCSer.Net.ICommandClient client, Func<T, IStreamPersistenceMemoryDatabaseClientSocketEvent> createClient) : base(client, createClient) { }
        /// <summary>
        /// Log stream persistence memory database client cache for client singleton
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="config">Command client configuration
        /// 命令客户端配置</param>
        /// <param name="createClient">Create a log stream persistence in-memory database client instance
        /// 创建日志流持久化内存数据库客户端实例</param>
        public ClientCache(AutoCSer.Net.CommandClientConfig config, Func<T, IStreamPersistenceMemoryDatabaseClientSocketEvent> createClient) : base(config, createClient) { }
    }
}
