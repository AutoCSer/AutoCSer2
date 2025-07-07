using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// The client scheduling of the registration server
    /// 注册服务客户端调度
    /// </summary>
    public abstract class ServerRegistryLogClient
    {
        /// <summary>
        /// Server registration node
        /// 服务注册节点
        /// </summary>
        public readonly StreamPersistenceMemoryDatabaseClientNodeCache<IServerRegistryNodeClientNode> NodeCache;
        /// <summary>
        /// Server name
        /// 服务名称
        /// </summary>
        public readonly string ServerName;
        /// <summary>
        /// Keep callback of get service logs
        /// 服务日志保持回调
        /// </summary>
#if NetStandard21
        protected AutoCSer.Net.CommandKeepCallback? keepCallback;
#else
        protected AutoCSer.Net.CommandKeepCallback keepCallback;
#endif
        /// <summary>
        /// Has the client for the registration server been added
        /// 是否已经添加注册服务客户端
        /// </summary>
        public bool IsAppendClient { get; internal set; }
        /// <summary>
        /// Server registration log callback
        /// 服务注册日志回调
        /// </summary>
        /// <returns></returns>
        public abstract Task LogCallback();
        /// <summary>
        /// The client scheduling of the registration server
        /// 注册服务客户端调度
        /// </summary>
        /// <param name="node">The client node for server registration
        /// 服务注册客户端节点</param>
        /// <param name="serverName">Server name
        /// 服务名称</param>
        protected ServerRegistryLogClient(StreamPersistenceMemoryDatabaseClientNodeCache<IServerRegistryNodeClientNode> node, string serverName)
        {
            this.NodeCache = node;
            this.ServerName = serverName;
        }
        /// <summary>
        /// Server registration log callback
        /// 服务注册日志回调
        /// </summary>
        /// <param name="result"></param>
        /// <param name="command"></param>
        protected void logCallback(ResponseResult<ServerRegistryLog> result, AutoCSer.Net.KeepCallbackCommand command)
        {
            if (result.IsSuccess)
            {
                var log = result.Value;
                if (log != null) logCallback(log);
            }
        }
        /// <summary>
        /// Server registration log callback
        /// 服务注册日志回调
        /// </summary>
        /// <param name="log"></param>
        protected abstract void logCallback(ServerRegistryLog log);
    }
}
