using AutoCSer.Extensions;
using System;
using System.Reflection;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
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
        public CustomServiceNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService service) : base(service) { }
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
        public AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateDictionaryCounterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity)
        {
            var type = default(Type);
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum state = getEquatableType(ref keyType, ref type);
            if (state == AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum.Success)
            {
                MethodInfo method = typeof(CustomServiceNode).GetMethod(nameof(createDictionaryCounterNode), BindingFlags.Instance | BindingFlags.NonPublic).AutoCSerExtensions().NotNull();
                return (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex)method.MakeGenericMethod(type.AutoCSerExtensions().NotNull()).Invoke(this, new object[] { index, key, nodeInfo, capacity }).AutoCSerExtensions().NotNull();
            }
            return new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex(state);
        }
        /// <summary>
        /// Create a dictionary counter node IDictionaryCounterNode{T}
        /// 创建字典计数器节点 IDictionaryCounterNode{T}
        /// </summary>
        /// <typeparam name="T"></typeparam>
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
        private AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex createDictionaryCounterNode<T>(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity)
            where T : IEquatable<T>
        {
            return CreateSnapshotNode<IDictionaryCounterNode<T>>(index, key, nodeInfo, () => new DictionaryCounterNode<T>(capacity));
        }
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
        public AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateDictionarySnapshotCloneCounterNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity)
        {
            var type = default(Type);
            AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum state = getEquatableType(ref keyType, ref type);
            if (state == AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum.Success)
            {
                MethodInfo method = typeof(CustomServiceNode).GetMethod(nameof(createDictionarySnapshotCloneCounterNode), BindingFlags.Instance | BindingFlags.NonPublic).AutoCSerExtensions().NotNull();
                return (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex)method.MakeGenericMethod(type.AutoCSerExtensions().NotNull()).Invoke(this, new object[] { index, key, nodeInfo, capacity }).AutoCSerExtensions().NotNull();
            }
            return new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex(state);
        }
        /// <summary>
        /// Create a dictionary counter node that supports snapshot cloning IDictionarySnapshotCloneCounterNode{T}
        /// 创建支持快照克隆的字典计数器节点 IDictionarySnapshotCloneCounterNode{T}
        /// </summary>
        /// <typeparam name="T"></typeparam>
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
        private AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex createDictionarySnapshotCloneCounterNode<T>(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity)
            where T : IEquatable<T>
        {
            return CreateSnapshotNode<IDictionarySnapshotCloneCounterNode<T>>(index, key, nodeInfo, () => new DictionarySnapshotCloneCounterNode<T>(capacity));
        }
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
        public AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateBeforePersistenceNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo, int capacity)
        {
            return CreateSnapshotNode<IBeforePersistenceNode>(index, key, nodeInfo, () => new BeforePersistenceNode(capacity));
        }
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
        public AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex CreateLoadPersistenceNode(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeIndex index, string key, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<ILoadPersistenceNode>(index, key, nodeInfo, () => new LoadPersistenceNode());
        }
    }
}
