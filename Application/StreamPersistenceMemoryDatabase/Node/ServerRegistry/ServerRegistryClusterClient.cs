using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 集群客户端
    /// </summary>
    /// <typeparam name="T">集群客户端节点类型</typeparam>
    public abstract class ServerRegistryClusterClient<T> : ServerRegistryLogClient, IDisposable
        where T : ClusterClient<T>
    {
        /// <summary>
        /// 客户端数组
        /// </summary>
        protected LeftArray<T> clientArray;
        /// <summary>
        /// 客户端集合
        /// </summary>
        protected readonly Dictionary<long, T> clients;
        /// <summary>
        /// 客户端访问锁
        /// </summary>
        protected readonly object clientLock;
        /// <summary>
        /// 集群客户端
        /// </summary>
        /// <param name="node">服务注册节点</param>
        /// <param name="serverName">服务名称</param>
        protected ServerRegistryClusterClient(StreamPersistenceMemoryDatabaseClientNodeCache<IServerRegistryNodeClientNode> node, string serverName) : base(node, serverName)
        {
            clientArray = new LeftArray<T>(0);
            clients = DictionaryCreator.CreateLong<T>();
            clientLock = new object();
        }
        /// <summary>
        /// 释放资源
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
        /// 服务日志回调委托
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
        /// 移除客户端
        /// </summary>
        /// <param name="sessionID">服务会话标识ID</param>
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
        /// 移除客户端
        /// </summary>
        /// <param name="client"></param>
        protected abstract void onRemoved(T client);
        /// <summary>
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
