using System;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    /// <summary>
    /// Customize the basic service node interface
    /// 自定义基础服务节点接口
    /// </summary>
    [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode(IsReturnValueNode = false)]
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
        /// <summary>
        /// Create a dictionary counter node IDictionaryCounterNode{T}
        /// 创建字典计数器节点 IDictionaryCounterNode{T}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateDictionaryCounterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
        /// <summary>
        /// Create a dictionary counter node that supports snapshot cloning IDictionarySnapshotCloneCounterNode{T}
        /// 创建支持快照克隆的字典计数器节点 IDictionarySnapshotCloneCounterNode{T}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateDictionarySnapshotCloneCounterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity);
        /// <summary>
        /// Create a sample node for persistent pre-checking IBeforePersistenceNode
        /// 创建持久化前置检查示例节点 IBeforePersistenceNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateBeforePersistenceNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// Create a API sample node for initialize and load the persistent data ILoadPersistenceNode
        /// 创建初始化加载持久化数据 API 示例节点 ILoadPersistenceNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateLoadPersistenceNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo);
    }
}
