using System;

namespace AutoCSer.Document.MemoryDatabaseLocalService.CustomNode
{
    /// <summary>
    /// 自定义基础服务接口
    /// </summary>
    [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode(IsClient = false, IsLocalClient = true)]
    public partial interface ICustomServiceNode : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNode
    {
        /// <summary>
        /// 创建计数器节点 ICounterNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateCounterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
    }
}
