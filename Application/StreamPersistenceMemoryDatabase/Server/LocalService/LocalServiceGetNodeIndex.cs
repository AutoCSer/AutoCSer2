using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// The local service get the node identity
    /// 本地服务获取节点标识
    /// </summary>
    internal sealed class LocalServiceGetNodeIndex : LocalServiceQueueNode<NodeIndex>
    {
        /// <summary>
        /// Node global keyword
        /// 节点全局关键字
        /// </summary>
        private readonly string key;
        /// <summary>
        /// 节点信息
        /// </summary>
        private readonly NodeInfo nodeInfo;
        /// <summary>
        /// The local service get the node identity
        /// 本地服务获取节点标识
        /// </summary>
        /// <param name="service">Log stream persistence memory database local service
        /// 日志流持久化内存数据库本地服务</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        internal LocalServiceGetNodeIndex(LocalService service, string key, NodeInfo nodeInfo) : base(service)
        {
            this.key = key;
            this.nodeInfo = nodeInfo;
        }
        /// <summary>
        /// Get node identity
        /// 获取节点标识
        /// </summary>
        public override void RunTask()
        {
            result = service.GetNodeIndex(key, nodeInfo, true);
            completed();
        }
    }
}
