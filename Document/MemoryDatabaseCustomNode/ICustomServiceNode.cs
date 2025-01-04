using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// 自定义基础服务接口
    /// </summary>
    [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode]
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
        /// <summary>
        /// 创建字典计数器节点 IDictionaryCounterNode{T}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateDictionaryCounterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
        /// <summary>
        /// 创建支持快照克隆的字典计数器节点 IDictionarySnapshotCloneCounterNode{T}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateDictionarySnapshotCloneCounterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
        /// <summary>
        /// 创建持久化前置检查示例节点 IBeforePersistenceNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">初始化容器尺寸</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateBeforePersistenceNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
    }
}
