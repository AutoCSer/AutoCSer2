using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 日志流持久化内存数据库客户端缓存，用于客户端单例
    /// </summary>
    /// <typeparam name="NT">服务基础操作客户端接口类型</typeparam>
    /// <typeparam name="ET">命令客户端套接字事件类型</typeparam>
    public sealed class ClientCache<NT, ET> : StreamPersistenceMemoryDatabaseClientCache<NT>
        where NT : class, IServiceNodeClientNode
        where ET : CommandClientSocketEventTask<ET>
    {
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        public readonly AutoCSer.Net.CommandClientSocketEventCache<ET> Client;
        /// <summary>
        /// 日志流持久化内存数据库客户端访问锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim clientLock;
        /// <summary>
        /// 创建日志流持久化内存数据库客户端接口
        /// </summary>
        private readonly Func<ET, IStreamPersistenceMemoryDatabaseClientSocketEvent> createClient;
        /// <summary>
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="client">命令客户端套接字事件</param>
        /// <param name="createClient">创建日志流持久化内存数据库客户端接口</param>
        public ClientCache(AutoCSer.Net.CommandClientSocketEventCache<ET> client, Func<ET, IStreamPersistenceMemoryDatabaseClientSocketEvent> createClient)
        {
            Client = client;
            this.createClient = createClient;
            clientLock = new System.Threading.SemaphoreSlim(1, 1);
        }
        /// <summary>
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="client">命令客户端</param>
        /// <param name="createClient">创建日志流持久化内存数据库客户端接口</param>
        public ClientCache(AutoCSer.Net.ICommandClient client, Func<ET, IStreamPersistenceMemoryDatabaseClientSocketEvent> createClient) : this(new AutoCSer.Net.CommandClientSocketEventCache<ET>(client), createClient) { }
        /// <summary>
        /// 日志流持久化内存数据库客户端缓存，用于客户端单例
        /// </summary>
        /// <param name="config">命令客户端配置</param>
        /// <param name="createClient">创建日志流持久化内存数据库客户端接口</param>
        public ClientCache(AutoCSer.Net.CommandClientConfig config, Func<ET, IStreamPersistenceMemoryDatabaseClientSocketEvent> createClient) : this(new AutoCSer.Net.CommandClient(config), createClient) { }
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
}
