using System;

namespace AutoCSer.Document.MemoryDatabaseLocalService.CustomNode
{
    /// <summary>
    /// Customize the basic service node interface
    /// 自定义基础服务节点接口
    /// </summary>
    [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode(IsClient = false, IsLocalClient = true)]
    public partial interface ICustomServiceNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNode
    {
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
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateCounterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
    }
}
