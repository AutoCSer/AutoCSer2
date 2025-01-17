using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.ServerRegistry.MessageNodeClusterClient
{
    /// <summary>
    /// 集群服务客户端
    /// </summary>
    internal sealed class ServerRegistryClusterClient : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryClusterClient<ClusterClient>
    {
        /// <summary>
        /// 没有找到有效客户端节点
        /// </summary>
        private readonly Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage>>> notFoundClient;
        /// <summary>
        /// 客户端连续获取次数，避免频繁切换（非精确计数）
        /// </summary>
        private readonly int getCount;
        /// <summary>
        /// 上一次获取客户端数组索引位置
        /// </summary>
        private int getIndex;
        /// <summary>
        /// 最后一次获取的客户端
        /// </summary>
        private ClusterClient? lastClient;
        /// <summary>
        /// 当前客户端允许获取次数（非精确计数）
        /// </summary>
        private volatile int freeCount;
        /// <summary>
        /// 集群服务客户端
        /// </summary>
        /// <param name="getCount"></param>
        internal ServerRegistryClusterClient(int getCount = 1 << 10) : base(ServerRegistryLogCommandClientSocketEvent.ServerRegistryNodeCache, nameof(AutoCSer.Document.ServerRegistry.MessageNodeCluster))
        {
            this.getCount = Math.Max(getCount - 1, 0);
            getIndex = -1;
            notFoundClient = Task.FromResult(new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage>>(AutoCSer.Net.CommandClientReturnTypeEnum.Unknown, "没有找到有效客户端节点"));
            ServerRegistryLogCommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.ClientCache.SocketEvent.Append(this).NotWait();
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            ServerRegistryLogCommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.ClientCache.SocketEvent.Remove(this).NotWait();
            base.Dispose();
        }
        /// <summary>
        /// 服务日志回调
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
        /// 获取一个节点
        /// </summary>
        /// <returns></returns>
        public Task<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IMessageNodeClientNode<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage>>> GetNode()
        {
            var client = getClient();
            return client != null ? client.NodeCache.GetNode() : getNode();
        }
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <returns></returns>
        private ClusterClient? getClient()
        {
            var client = lastClient;
            if (client == null)
            {
                Monitor.Enter(clientLock);
                try
                {
                    client = lastClient;
                    if (client != null)
                    {
                        --freeCount;
                        return client;
                    }
                    if (clientArray.Count == 0) return null;
                    nextClient();
                    client = lastClient;
                }
                finally { Monitor.Exit(clientLock); }
                return client;
            }
            if (--freeCount < 0)
            {
                Monitor.Enter(clientLock);
                try
                {
                    if (freeCount < 0)
                    {
                        if (clientArray.Count == 0) return null;
                        nextClient();
                    }
                    client = lastClient;
                }
                finally { Monitor.Exit(clientLock); }
            }
            return client;
        }
        /// <summary>
        /// 下一个客户端
        /// </summary>
        private void nextClient()
        {
            if (++getIndex >= clientArray.Count) getIndex = 0;
            lastClient = clientArray[getIndex];
            freeCount = getCount;
        }
        /// <summary>
        /// 获取一个节点
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
        /// 移除客户端
        /// </summary>
        /// <param name="client"></param>
        protected override void onRemoved(ClusterClient client)
        {
            if (object.ReferenceEquals(lastClient, client)) lastClient = null;
        }

        /// <summary>
        /// 集群服务客户端单例
        /// </summary>
        public static readonly ServerRegistryClusterClient Client = new ServerRegistryClusterClient(1 << 4);
        /// <summary>
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
                    var result = await node.Value.notNull().AppendMessage(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerByteArrayMessage.JsonSerialize(new Data.TestClass { Int = value, String = value.toString() }));
                    if (result.IsSuccess) Console.Write('+');
                    else ConsoleWriteQueue.Breakpoint($"AppendMessage {result.ReturnType}.{result.CallState}");
                }
                else Console.Write('!');
                await Task.Delay(1);
            }
        }
    }
}
