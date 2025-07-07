using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry
{
    /// <summary>
    /// Cluster client node
    /// 集群客户端节点
    /// </summary>
    public abstract class ClusterClient
    {
        /// <summary>
        /// Server Registration Log
        /// 服务注册日志
        /// </summary>
        public readonly ServerRegistryLog Log;
        /// <summary>
        /// Index position of the client array
        /// 客户端数组索引位置
        /// </summary>
        internal int ClientIndex;
        /// <summary>
        /// Has the client been closed
        /// 客户端是否已经关闭
        /// </summary>
        public bool IsClosed { get { return ClientIndex == int.MinValue; } }
        /// <summary>
        /// Is the client connection being checked
        /// 是否正在检查客户端连接
        /// </summary>
        protected int isCheck;
        /// <summary>
        /// Is the server registration log being checked
        /// 是否正在检查服务注册日志
        /// </summary>
        protected int isCheckLog;
        /// <summary>
        /// Cluster client node
        /// 集群客户端节点
        /// </summary>
        /// <param name="log">Server Registration Log
        /// 服务注册日志</param>
        protected ClusterClient(ServerRegistryLog log)
        {
            this.Log = log;
            ClientIndex = -1;
        }
        /// <summary>
        /// Get the client connection
        /// 获取客户端连接
        /// </summary>
        /// <returns></returns>
        protected abstract Task<bool> getSocket();
        /// <summary>
        /// Close the client
        /// 关闭客户端
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Close()
        {
            ClientIndex = int.MinValue;
            close();
        }
        /// <summary>
        /// Close the client
        /// 关闭客户端
        /// </summary>
        protected abstract void close();
    }
    /// <summary>
    /// Cluster client node
    /// 集群客户端节点
    /// </summary>
    /// <typeparam name="T">Cluster client node type
    /// 集群客户端节点类型</typeparam>
    public abstract class ClusterClient<T> : ClusterClient
        where T : ClusterClient<T>
    {
        /// <summary>
        /// Cluster client scheduling
        /// 集群客户端调度
        /// </summary>
        protected readonly ServerRegistryClusterClient<T> serverRegistryClusterClient;
        /// <summary>
        /// Cluster client node
        /// 集群客户端节点
        /// </summary>
        /// <param name="clusterClient">Cluster client scheduling
        /// 集群客户端调度</param>
        /// <param name="log">Server Registration Log
        /// 服务注册日志</param>
        protected ClusterClient(ServerRegistryClusterClient<T> clusterClient, ServerRegistryLog log) : base(log)
        {
            this.serverRegistryClusterClient = clusterClient;
        }
        /// <summary>
        /// Check the client connection
        /// 检查客户端连接
        /// </summary>
        /// <returns></returns>
        protected async Task check()
        {
            if (System.Threading.Interlocked.Exchange(ref isCheck, 1) == 0)
            {
                await AutoCSer.Threading.SwitchAwaiter.Default;
                bool isAppend = false;
                try
                {
                    if (await getSocket())
                    {
                        serverRegistryClusterClient.Append((T)this);
                        isAppend = true;
                    }
                }
                finally
                {
                    if (!isAppend)
                    {
                        serverRegistryClusterClient.remove(Log.SessionID);
                        CheckLog();
                    }
                }
            }
        }
        /// <summary>
        /// Get the server registration logs that need to be inspected
        /// 获取需要检查的服务注册日志
        /// </summary>
        /// <returns></returns>
        public void CheckLog()
        {
            if (System.Threading.Interlocked.CompareExchange(ref isCheckLog, 1, 0) == 0) checkLog().NotWait();
        }
        /// <summary>
        /// Check the online status of the server
        /// 检查服务在线状态
        /// </summary>
        /// <returns></returns>
        private async Task checkLog()
        {
            try
            {
                ResponseResult<IServerRegistryNodeClientNode> node = await serverRegistryClusterClient.NodeCache.GetSynchronousNode();
                if (node.Value != null) node.Value.Check(Log.SessionID, Log.ServerName).Discard();
            }
            finally
            {
                await Task.Delay(1000);
                System.Threading.Interlocked.Exchange(ref isCheckLog, 0);
            }
        }
    }
}
