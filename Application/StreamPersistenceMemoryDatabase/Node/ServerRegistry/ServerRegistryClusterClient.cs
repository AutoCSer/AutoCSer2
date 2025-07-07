using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Cluster client scheduling
    /// 集群客户端调度
    /// </summary>
    /// <typeparam name="T">Cluster client node type
    /// 集群客户端节点类型</typeparam>
    public abstract class ServerRegistryClusterClient<T> : ServerRegistryLogClient, IDisposable
        where T : ClusterClient<T>
    {
        /// <summary>
        /// Client array
        /// 客户端数组
        /// </summary>
        protected LeftArray<T> clientArray;
        /// <summary>
        /// Client collection
        /// 客户端集合
        /// </summary>
        protected readonly Dictionary<long, T> clients;
        /// <summary>
        /// Client access lock
        /// 客户端访问锁
        /// </summary>
        protected readonly object clientLock;
        /// <summary>
        /// Cluster client scheduling
        /// 集群客户端调度
        /// </summary>
        /// <param name="node">The client node for server registration
        /// 服务注册客户端节点</param>
        /// <param name="serverName">Server name
        /// 服务名称</param>
        protected ServerRegistryClusterClient(StreamPersistenceMemoryDatabaseClientNodeCache<IServerRegistryNodeClientNode> node, string serverName) : base(node, serverName)
        {
            clientArray = new LeftArray<T>(0);
            clients = DictionaryCreator.CreateLong<T>();
            clientLock = new object();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        public virtual void Dispose()
        {
            clientArray.Reserve = 1;
            Monitor.Enter(clientLock);
            try
            {
                keepCallback?.Dispose();
                foreach (T client in clients.Values) client.Close();
                clients.Clear();
                clientArray.Clear();
            }
            finally { Monitor.Exit(clientLock); }
        }
        /// <summary>
        /// Server registration log callback
        /// 服务注册日志回调
        /// </summary>
        /// <returns></returns>
        public override async Task LogCallback()
        {
            var node = await NodeCache.GetSynchronousNode();
            if (node.IsSuccess && clientArray.Reserve == 0)
            {
                var keepCallback = await node.Value.notNull().LogCallback(ServerName, logCallback);
                if (keepCallback != null)
                {
                    var oldKeepCallback = this.keepCallback;
                    this.keepCallback = keepCallback;
                    oldKeepCallback?.Dispose();
                    if (clientArray.Reserve != 0) keepCallback.Dispose();
                }
            }
        }
        /// <summary>
        /// Remove the client
        /// 移除客户端
        /// </summary>
        /// <param name="sessionID">Server session identity
        /// 服务会话标识</param>
        protected internal void remove(long sessionID)
        {
            var client = default(T);
            Monitor.Enter(clientLock);
            if (clients.Remove(sessionID, out client))
            {
                try
                {
                    onRemoved(client);
                    int index = client.ClientIndex;
                    if (index >= 0)
                    {
                        clientArray.RemoveAtToEnd(index);
                        if (index != clientArray.Count) clientArray[index].ClientIndex = index;
                    }
                    client.Close();
                }
                finally { Monitor.Exit(clientLock); }
            }
            else Monitor.Exit(clientLock);
        }
        /// <summary>
        /// Remove the client
        /// 移除客户端
        /// </summary>
        /// <param name="client"></param>
        protected abstract void onRemoved(T client);
        /// <summary>
        /// Add the client
        /// 添加客户端
        /// </summary>
        /// <param name="client"></param>
        internal void Append(T client)
        {
            Monitor.Enter(clientLock);
            if (!client.IsClosed && clientArray.Reserve == 0)
            {
                client.ClientIndex = clientArray.Count;
                try
                {
                    clientArray.Add(client);
                }
                finally { Monitor.Exit(clientLock); }
            }
            else Monitor.Exit(clientLock);
        }
    }
}
