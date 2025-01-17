using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 注册服务客户端
    /// </summary>
    public abstract class ServerRegistryLogClient
    {
        /// <summary>
        /// 服务注册节点
        /// </summary>
        public readonly StreamPersistenceMemoryDatabaseClientNodeCache<IServerRegistryNodeClientNode> NodeCache;
        /// <summary>
        /// 服务名称
        /// </summary>
        public readonly string ServerName;
        /// <summary>
        /// 服务日志保持回调
        /// </summary>
#if NetStandard21
        protected AutoCSer.Net.CommandKeepCallback? keepCallback;
#else
        protected AutoCSer.Net.CommandKeepCallback keepCallback;
#endif
        /// <summary>
        /// 是否已经添加注册服务客户端
        /// </summary>
        public bool IsAppendClient { get; internal set; }
        /// <summary>
        /// 服务日志回调委托
        /// </summary>
        /// <returns></returns>
        public abstract Task LogCallback();
        /// <summary>
        /// 注册服务客户端
        /// </summary>
        /// <param name="node">服务注册节点</param>
        /// <param name="serverName">服务名称</param>
        protected ServerRegistryLogClient(StreamPersistenceMemoryDatabaseClientNodeCache<IServerRegistryNodeClientNode> node, string serverName)
        {
            this.NodeCache = node;
            this.ServerName = serverName;
        }
        /// <summary>
        /// 服务日志回调
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
        /// 服务日志回调
        /// </summary>
        /// <param name="log"></param>
        protected abstract void logCallback(ServerRegistryLog log);
    }
}
