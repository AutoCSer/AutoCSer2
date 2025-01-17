using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry
{
    /// <summary>
    /// 集群客户端节点
    /// </summary>
    /// <typeparam name="T">集群客户端节点类型</typeparam>
    public abstract class ClusterClient<T> where T : ClusterClient<T>
    {
        /// <summary>
        /// 集群服务客户端
        /// </summary>
        protected readonly ServerRegistryClusterClient<T> serverRegistryClusterClient;
        /// <summary>
        /// 服务注册日志
        /// </summary>
        public readonly ServerRegistryLog Log;
        /// <summary>
        /// 客户端数组索引位置
        /// </summary>
        internal int ClientIndex;
        /// <summary>
        /// 客户端是否已经关闭
        /// </summary>
        public bool IsClosed { get { return ClientIndex == int.MinValue; } }
        /// <summary>
        /// 是否正在检查客户端连接
        /// </summary>
        private int isCheck;
        /// <summary>
        /// 内存数据库集群客户端
        /// </summary>
        /// <param name="clusterClient">集群服务客户端</param>
        /// <param name="log">服务注册日志</param>
        protected ClusterClient(ServerRegistryClusterClient<T> clusterClient, ServerRegistryLog log)
        {
            this.serverRegistryClusterClient = clusterClient;
            this.Log = log;
            ClientIndex = -1;
        }
        /// <summary>
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
                    if (!isAppend) serverRegistryClusterClient.remove(Log.SessionID);
                }
            }
        }
        /// <summary>
        /// 获取客户端连接
        /// </summary>
        /// <returns></returns>
        protected abstract Task<bool> getSocket();
        /// <summary>
        /// 关闭客户端
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Close()
        {
            ClientIndex = int.MinValue;
            close();
        }
        /// <summary>
        /// 关闭客户端
        /// </summary>
        protected abstract void close();
    }
}
