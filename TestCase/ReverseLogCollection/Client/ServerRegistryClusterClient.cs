using AutoCSer.Extensions;
using AutoCSer.TestCase.ReverseLogCollectionCommon;
using System;
using System.Threading;

namespace AutoCSer.TestCase.ReverseLogCollectionClient
{
    /// <summary>
    /// Cluster client scheduling
    /// 集群客户端调度
    /// </summary>
    internal sealed class ServerRegistryClusterClient : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryClusterClient<ReverseLogCollectionClusterClient>
    {
        /// <summary>
        /// Cluster client scheduling
        /// 集群客户端调度
        /// </summary>
        internal ServerRegistryClusterClient() : base(ServerRegistryLogCommandClientSocketEvent.ServerRegistryNodeCache, LogInfo.ServerName)
        {
            ServerRegistryLogCommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.ClientCache.SocketEvent.Append(this).NotWait();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public override void Dispose()
        {
            ServerRegistryLogCommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.ClientCache.SocketEvent.Remove(this).NotWait();
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
                    Monitor.Enter(clientLock);
                    try
                    {
                        if (!clients.TryGetValue(log.SessionID, out var client)) clients.Add(log.SessionID, client = new ReverseLogCollectionClusterClient(this, log));
                    }
                    finally { Monitor.Exit(clientLock); }
                    break;
                case AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryOperationTypeEnum.Logout:
                case AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryOperationTypeEnum.LostContact:
                    remove(log.SessionID);
                    break;
            }
        }
        /// <summary>
        /// Remove the client
        /// 移除客户端
        /// </summary>
        /// <param name="client"></param>
        protected override void onRemoved(ReverseLogCollectionClusterClient client) { }
    }
}
