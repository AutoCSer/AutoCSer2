using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Document.ServerRegistry.MessageNodeClusterClient
{
    /// <summary>
    /// In-memory database cluster client
    /// 内存数据库集群客户端
    /// </summary>
    internal sealed class ClusterClient : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry.ClusterClient<ClusterClient>
    {
        /// <summary>
        /// Log streams persist the in-memory database client cache
        /// 日志流持久化内存数据库客户端缓存
        /// </summary>
        private readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<CommandClientSocketEvent> client;
        /// <summary>
        /// Message node caching
        /// 消息节点缓存
        /// </summary>
        internal readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage>> NodeCache;
        /// <summary>
        /// Client JSON message client consumer
        /// 客户端 JSON 消息客户端消费者
        /// </summary>
        private MessageConsumer? messageConsumer;
        /// <summary>
        /// Determine whether the socket has been closed
        /// 判断套接字是否已经关闭
        /// </summary>
        internal bool IsSocketClosed
        {
            get { return client.ClientCache.Client.IsSocketClosed; }
        }
        /// <summary>
        /// In-memory database cluster client
        /// 内存数据库集群客户端
        /// </summary>
        /// <param name="serverRegistryClusterClient">Cluster service client scheduling
        /// 集群服务客户端调度</param>
        /// <param name="log">Server Registration Log
        /// 服务注册日志</param>
        internal ClusterClient(ServerRegistryClusterClient serverRegistryClusterClient, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryLog log) : base(serverRegistryClusterClient, log)
        {
            client = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<CommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
            {
                Host = new AutoCSer.Net.HostEndPoint(log.Port, log.Host),
                GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client),
            });
            NodeCache = client.CreateNode(client => client.GetOrCreateServerByteArrayMessageNode(nameof(AutoCSer.Document.ServerRegistry.MessageNodeClusterClient)));
            check().AutoCSerExtensions().Catch();
        }
        /// <summary>
        /// Get the client connection
        /// 获取客户端连接
        /// </summary>
        /// <returns></returns>
        protected override async Task<bool> getSocket()
        {
            var socket = await client.ClientCache.Client.GetSocketEvent();
            if (socket != null)
            {
                var node = await NodeCache.GetNode();
                if (node.IsSuccess)
                {
                    Console.WriteLine($"New client {Log.SessionID}");
                    //Each client of the cluster service needs to create a message consumer
                    //集群服务的每一个客户端都需要创建一个消息消费者
                    messageConsumer = new MessageConsumer(client.ClientCache.Client, node.Value.AutoCSerClassGenericTypeExtensions().NotNull());
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Close the client
        /// 关闭客户端
        /// </summary>
        protected override void close()
        {
            Console.WriteLine($"Close client {Log.SessionID}");
            client.ClientCache.Client.Dispose();
        }
        /// <summary>
        /// Determine whether the socket has been closed
        /// 判断套接字是否已经关闭
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckSocketClosed()
        {
            if (IsSocketClosed)
            {
                CheckLog();
                return true;
            }
            return false;
        }
    }
}
