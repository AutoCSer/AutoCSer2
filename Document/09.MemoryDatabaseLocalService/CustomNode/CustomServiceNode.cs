using System;

namespace AutoCSer.Document.MemoryDatabaseLocalService.CustomNode
{
    /// <summary>
    /// Customize the basic service node for adding custom nodes to create API methods
    /// 自定义基础服务节点，用于添加自定义节点创建 API 方法
    /// </summary>
    public sealed class CustomServiceNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServiceNode, ICustomServiceNode
    {
        /// <summary>
        /// Customize the basic service node
        /// 自定义基础服务节点
        /// </summary>
        /// <param name="service">Log stream persistence memory database service
        /// 日志流持久化内存数据库服务</param>
        public CustomServiceNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.LocalService service) : base(service) { }
        /// <summary>
        /// Create a counter node ICounterNode
        /// 创建计数器节点 ICounterNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateCounterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<ICounterNode>(index, key, nodeInfo, () => new CounterNode());
        }
    }
}
