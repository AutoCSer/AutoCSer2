using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.ServerRegistry.MessageNodeClusterClient
{
    /// <summary>
    /// Cluster client scheduling
    /// 集群客户端调度
    /// </summary>
    internal sealed class ServerRegistryClusterClient : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryClusterClient<ClusterClient>
    {
        /// <summary>
        /// No valid client nodes were found
        /// 没有找到有效客户端节点
        /// </summary>
        private readonly Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage>>> notFoundClient;
        /// <summary>
        /// The client continuously acquires the number of times to avoid frequent switching (non-precise counting)
        /// 客户端连续获取次数，避免频繁切换（非精确计数）
        /// </summary>
        private readonly int getCount;
        /// <summary>
        /// The index position of the client array was obtained last time
        /// 上一次获取客户端数组索引位置
        /// </summary>
        private int getIndex;
        /// <summary>
        /// The last client obtained
        /// 最后一次获取的客户端
        /// </summary>
        private ClusterClient? lastClient;
        /// <summary>
        /// The current number of times the client is allowed to obtain (non-precise count)
        /// 当前客户端允许获取次数（非精确计数）
        /// </summary>
        private volatile int freeCount;
        /// <summary>
        /// Cluster client scheduling
        /// 集群客户端调度
        /// </summary>
        /// <param name="getCount"></param>
        internal ServerRegistryClusterClient(int getCount = 1 << 10) : base(ServerRegistryLogCommandClientSocketEvent.ServerRegistryNodeCache, nameof(AutoCSer.Document.ServerRegistry.MessageNodeCluster))
        {
            this.getCount = Math.Max(getCount - 1, 0);
            getIndex = -1;
            notFoundClient = Task.FromResult(new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage>>(AutoCSer.Net.CommandClientReturnTypeEnum.Unknown, "没有找到有效客户端节点"));
            ServerRegistryLogCommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.ClientCache.SocketEvent.Append(this).AutoCSerNotWait();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public override void Dispose()
        {
            ServerRegistryLogCommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.ClientCache.SocketEvent.Remove(this).AutoCSerNotWait();
            base.Dispose();
        }
        /// <summary>
        /// Server registration log callback
        /// 服务注册日志回调
        /// </summary>
        /// <param name="log"></param>
        protected override void logCallback(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryLog log)
        {
            switch (log.OperationType)
            {
                case AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryOperationTypeEnum.ClusterNode:
                    Console.WriteLine($"{log.OperationType} + {log.SessionID} {log.Host}:{log.Port}");
                    Monitor.Enter(clientLock);
                    try
                    {
                        if (!clients.TryGetValue(log.SessionID, out var client)) clients.Add(log.SessionID, client = new ClusterClient(this, log));
                    }
                    finally { Monitor.Exit(clientLock); }
                    break;
                case AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryOperationTypeEnum.Logout:
                case AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryOperationTypeEnum.LostContact:
                    Console.WriteLine($"{log.OperationType} - {log.SessionID}");
                    remove(log.SessionID);
                    break;
            }
        }
        /// <summary>
        /// Get a message client node
        /// 获取一个消息客户端节点
        /// </summary>
        /// <returns></returns>
        public Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage>>> GetNode()
        {
            var client = getClient();
            return client != null ? client.NodeCache.GetNode() : getNode();
        }
        /// <summary>
        /// Get the client
        /// 获取客户端
        /// </summary>
        /// <returns></returns>
        private ClusterClient? getClient()
        {
            var client = lastClient;
            return --freeCount >= 0 && client != null && !client.IsSocketClosed ? client : getNextClient();
        }
        /// <summary>
        /// Get the next client
        /// 获取下一个客户端
        /// </summary>
        /// <returns></returns>
        private ClusterClient? getNextClient()
        {
            Monitor.Enter(clientLock);
            try
            {
                var client = lastClient;
                if (--freeCount >= 0 && client != null && !client.CheckSocketClosed()) return client;
                if (clientArray.Count == 0) return lastClient = null;
                if (++getIndex >= clientArray.Count) getIndex = 0;
                lastClient = clientArray[getIndex];
                if (lastClient.CheckSocketClosed())
                {
                    int index = getIndex;
                    do
                    {
                        if (++getIndex >= clientArray.Count) getIndex = 0;
                        if (getIndex != index) lastClient = clientArray[getIndex];
                        else return lastClient = null;
                    }
                    while (lastClient.CheckSocketClosed());
                }
                freeCount = getCount;
                return lastClient;
            }
            finally { Monitor.Exit(clientLock); }
        }
        /// <summary>
        /// Get a message client node
        /// 获取一个消息客户端节点
        /// </summary>
        /// <returns></returns>
        private async Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage>>> getNode()
        {
            if (!IsAppendClient) await ServerRegistryLogCommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.ClientCache.SocketEvent.Append(this);
            var client = getClient();
            if (client != null) return await client.NodeCache.GetNode();
            return notFoundClient.Result;
        }
        /// <summary>
        /// Remove the client
        /// 移除客户端
        /// </summary>
        /// <param name="client"></param>
        protected override void onRemoved(ClusterClient client)
        {
            if (object.ReferenceEquals(lastClient, client)) lastClient = null;
        }

        /// <summary>
        /// Cluster server client singleton (In practice, getCount should be set to at least 1000 or more)
        /// 集群服务客户端单例（实战中 getCount 至少应该设置为 1000 以上）
        /// </summary>
        public static readonly ServerRegistryClusterClient Client = new ServerRegistryClusterClient(1 << 4);
        /// <summary>
        /// Message node cluster client producer test
        /// 消息节点集群客户端生产者测试
        /// </summary>
        /// <returns></returns>
        public static async Task Test()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            for (long value = 0; value != long.MaxValue; ++value)
            {
                var node = await Client.GetNode();
                if (node.IsSuccess)
                {
                    var result = await node.Value.AutoCSerClassGenericTypeExtensions().NotNull().AppendMessage(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage.JsonSerialize(new Data.TestClass { Int = value, String = value.AutoCSerExtensions().ToString() }));
                    if (result.IsSuccess) Console.Write('+');
                    else ConsoleWriteQueue.Breakpoint($"AppendMessage {result.ReturnType}.{result.CallState}");
                }
                else Console.Write('!');
                await Task.Delay(1);
            }
        }
    }
}
