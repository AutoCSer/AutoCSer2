using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务获取节点标识
    /// </summary>
    internal sealed class LocalServiceGetNodeIndex : LocalServiceQueueNode<NodeIndex>
    {
        /// <summary>
        /// 节点全局关键字
        /// </summary>
        private readonly string key;
        /// <summary>
        /// 节点信息
        /// </summary>
        private readonly NodeInfo nodeInfo;
        /// <summary>
        /// 本地服务获取节点标识
        /// </summary>
        /// <param name="service">日志流持久化内存数据库本地服务</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        internal LocalServiceGetNodeIndex(LocalService service, string key, NodeInfo nodeInfo) : base(service)
        {
            this.key = key;
            this.nodeInfo = nodeInfo;
        }
        /// <summary>
        /// 获取节点标识
        /// </summary>
        public override void RunTask()
        {
            result = service.GetNodeIndex(key, nodeInfo, true);
            completed();
        }
    }
}
