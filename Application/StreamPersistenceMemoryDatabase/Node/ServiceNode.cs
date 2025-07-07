using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Reflection;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Service basic operation node
    /// 服务基础操作节点
    /// </summary>
    public class ServiceNode : IServiceNode
    {
        /// <summary>
        /// Identification of basic service nodes
        /// 基础服务节点标识
        /// </summary>
        internal static readonly NodeIndex ServiceNodeIndex = new NodeIndex(0, 1);

        /// <summary>
        /// Log stream persistence memory database service
        /// 日志流持久化内存数据库服务
        /// </summary>
        public readonly StreamPersistenceMemoryDatabaseService Service;
        /// <summary>
        /// Whether it is necessary to call AutoCSer.Common.Config.CheckRemoteType to check the validity of the remote type
        /// 是否需要调用 AutoCSer.Common.Config.CheckRemoteType 检查远程类型的合法性
        /// </summary>
        protected readonly bool isCheckRemoveType;
        /// <summary>
        /// Service basic operation node
        /// 服务基础操作节点
        /// </summary>
        /// <param name="service"></param>
        public ServiceNode(StreamPersistenceMemoryDatabaseService service)
        {
            this.Service = service;
            isCheckRemoveType = !(service is LocalService);
        }
        /// <summary>
        /// Delete the node
        /// 删除节点
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <returns>Returning false indicates that the node was not found
        /// 返回 false 表示没有找到节点</returns>
        public virtual bool RemoveNode(NodeIndex index)
        {
            return Service.RemoveNode(index);
        }
        /// <summary>
        /// Delete the node
        /// 删除节点
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <returns>Returning false indicates that the node was not found
        /// 返回 false 表示没有找到节点</returns>
        public virtual bool RemoveNodeByKey(string key)
        {
            return Service.RemoveNode(key);
        }
        /// <summary>
        /// Create server-side nodes (persistence is not supported; only nodes that support snapshots support persistence)
        /// 创建服务端节点（不支持持久化，只有支持快照的节点才支持持久化）
        /// </summary>
        /// <typeparam name="T">Node interface type
        /// 节点接口类型</typeparam>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="getNode">Get the node operation object
        /// 获取节点操作对象</param>
        /// <param name="isPersistence">By default, false indicates pure memory mode, and data will be lost when the service is restarted
        /// 默认为 false 表示纯内存模式，在重启服务时数据将丢失</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateNode<T>(NodeIndex index, string key, NodeInfo nodeInfo, Func<T> getNode, bool isPersistence = false) where T : class
        {
            try
            {
                if (nodeInfo == null) return new NodeIndex(CallStateEnum.NullNodeInfo);
                if (!Service.IsLoaded) Service.LoadCreateNode(index, key, nodeInfo);
                if (!typeof(T).IsInterface) return new NodeIndex(CallStateEnum.OnlySupportInterface);
                ServerNodeCreator nodeCreator = Service.GetNodeCreator<T>();
                if (nodeCreator == null) return new NodeIndex(CallStateEnum.NotFoundNodeCreator);
                NodeIndex nodeIndex = Service.CheckCreateNodeIndex(index, key, nodeInfo);
                if (nodeIndex.Index < 0 || !nodeIndex.GetFree()) return nodeIndex;
                return new ServerNode<T>(Service, nodeIndex, key, getNode(), isPersistence).Index;
            }
            finally { Service.RemoveFreeIndex(index); }
        }
        /// <summary>
        /// Create server-side nodes (persistence is not supported; only nodes that support snapshots support persistence)
        /// 创建服务端节点（不支持持久化，只有支持快照的节点才支持持久化）
        /// </summary>
        /// <typeparam name="T">Node interface type
        /// 节点接口类型</typeparam>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns></returns>
        public virtual ValueResult<NodeIndex> CreateNodeBeforePersistence<T>(CreateNodeIndex index, string key, NodeInfo nodeInfo) where T : class
        {
            if (!typeof(T).IsInterface) return new NodeIndex(CallStateEnum.OnlySupportInterface);
            if (nodeInfo == null) return new NodeIndex(CallStateEnum.NullNodeInfo);
            ServerNodeCreator nodeCreator = Service.GetNodeCreator<T>();
            if (nodeCreator == null) return new NodeIndex(CallStateEnum.NotFoundNodeCreator);
            ValueResult<NodeIndex> result = Service.GetNodeIndexBeforePersistence(key, nodeInfo, true);
            if (result.IsValue)
            {
                if (result.Value.GetState() != CallStateEnum.Success || !result.Value.GetFree()) return result;
                index.Index = result.Value;
            }
            else index.Index = Service.CreateNodeIndex();
            return default(ValueResult<NodeIndex>);
        }
        /// <summary>
        /// Create server-side nodes (persistence is not supported; only nodes that support snapshots support persistence)
        /// 创建服务端节点（不支持持久化，只有支持快照的节点才支持持久化）
        /// </summary>
        /// <typeparam name="T">Node interface type
        /// 节点接口类型</typeparam>
        /// <param name="createNodeIndex">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="getNode">Get the node operation object
        /// 获取节点操作对象</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateNode<T>(CreateNodeIndex createNodeIndex, string key, NodeInfo nodeInfo, Func<T> getNode) where T : class
        {
            NodeIndex index = createNodeIndex.Index;
            try
            {
                if (!Service.IsLoaded) Service.LoadCreateNode(index, key, nodeInfo);
                return new ServerNode<T>(Service, index, key, getNode()).Index;
            }
            finally { Service.RemoveFreeIndex(index); }
        }
        /// <summary>
        /// Create server-side nodes that support the snapshot interface (it is necessary to ensure that the operation node object implements the snapshot interface)
        /// 创建支持快照接口的服务端节点（必须保证操作节点对象实现快照接口）
        /// </summary>
        /// <typeparam name="T">Node interface type
        /// 节点接口类型</typeparam>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="getNode">Get the node operation object (must ensure that the operation node object implements the snapshot interface)
        /// 获取节点操作对象（必须保证操作节点对象实现快照接口）</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateSnapshotNode<T>(NodeIndex index, string key, NodeInfo nodeInfo, Func<T> getNode)
            where T : class
        {
            var disposable = default(IDisposable);
            try
            {
                if (nodeInfo == null) return new NodeIndex(CallStateEnum.NullNodeInfo);
                if (!Service.IsLoaded) Service.LoadCreateNode(index, key, nodeInfo);
                if (!typeof(T).IsInterface) return new NodeIndex(CallStateEnum.OnlySupportInterface);
                ServerNodeCreator nodeCreator = Service.GetNodeCreator<T>();
                if (nodeCreator == null) return new NodeIndex(CallStateEnum.NotFoundNodeCreator);
                T node = getNode();
                disposable = node as IDisposable;
                CallStateEnum state = nodeCreator.CheckSnapshotNode(node.GetType());
                if (state != CallStateEnum.Success) return new NodeIndex(state);
                NodeIndex nodeIndex = Service.CheckCreateNodeIndex(index, key, nodeInfo);
                if (nodeIndex.Index < 0 || !nodeIndex.GetFree()) return nodeIndex;
                index = new ServerSnapshotNode<T>(Service, nodeIndex, key, node, Service.CurrentCallIsPersistence).Index;
                disposable = null;
                return index;
            }
            finally
            {
                Service.RemoveFreeIndex(index);
                disposable?.Dispose();
            }
        }
        /// <summary>
        /// Create server-side nodes that support the snapshot interface (it is necessary to ensure that the operation node object implements the snapshot interface)
        /// 创建支持快照接口的服务端节点（必须保证操作节点对象实现快照接口）
        /// </summary>
        /// <typeparam name="T">Node interface type
        /// 节点接口类型</typeparam>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="nodeCreator">Generate server-side node
        /// 生成服务端节点</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
#if NetStandard21
        public virtual ValueResult<NodeIndex> CreateSnapshotNodeBeforePersistence<T>(CreateNodeIndex index, string key, NodeInfo nodeInfo, out ServerNodeCreator? nodeCreator)
#else
        public virtual ValueResult<NodeIndex> CreateSnapshotNodeBeforePersistence<T>(CreateNodeIndex index, string key, NodeInfo nodeInfo, out ServerNodeCreator nodeCreator)
#endif
            where T : class
        {
            if (!typeof(T).IsInterface)
            {
                nodeCreator = null;
                return new NodeIndex(CallStateEnum.OnlySupportInterface);
            }
            if (nodeInfo == null)
            {
                nodeCreator = null;
                return new NodeIndex(CallStateEnum.NullNodeInfo);
            }
            nodeCreator = Service.GetNodeCreator<T>();
            if (nodeCreator == null) return new NodeIndex(CallStateEnum.NotFoundNodeCreator);
            ValueResult<NodeIndex> result = Service.GetNodeIndexBeforePersistence(key, nodeInfo, true);
            if (result.IsValue)
            {
                if (result.Value.GetState() != CallStateEnum.Success || !result.Value.GetFree()) return result;
                index.Index = result.Value;
            }
            else index.Index = Service.CreateNodeIndex();
            return default(ValueResult<NodeIndex>);
        }
        /// <summary>
        /// Create server-side nodes that support the snapshot interface (it is necessary to ensure that the operation node object implements the snapshot interface)
        /// 创建支持快照接口的服务端节点（必须保证操作节点对象实现快照接口）
        /// </summary>
        /// <typeparam name="T">Node interface type
        /// 节点接口类型</typeparam>
        /// <param name="createNodeIndex">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="nodeCreator">Generate server-side node
        /// 生成服务端节点</param>
        /// <param name="getNode">Get the node operation object (must ensure that the operation node object implements the snapshot interface)
        /// 获取节点操作对象（必须保证操作节点对象实现快照接口）</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateSnapshotNode<T>(CreateNodeIndex createNodeIndex, string key, NodeInfo nodeInfo, ServerNodeCreator nodeCreator, Func<T> getNode)
            where T : class
        {
            var disposable = default(IDisposable);
            NodeIndex index = createNodeIndex.Index;
            try
            {
                if (!Service.IsLoaded) Service.LoadCreateNode(index, key, nodeInfo);
                T node = getNode();
                disposable = node as IDisposable;
                CallStateEnum state = nodeCreator.CheckSnapshotNode(node.GetType());
                if (state != CallStateEnum.Success) return new NodeIndex(state);
                index = new ServerSnapshotNode<T>(Service, index, key, node, Service.CurrentCallIsPersistence).Index;
                disposable = null;
                return index;
            }
            finally
            {
                Service.RemoveFreeIndex(index);
                disposable?.Dispose();
            }
        }
#if !AOT
        /// <summary>
        /// Create a server registration node IServerRegistryNode
        /// 创建服务注册节点 IServerRegistryNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="loadTimeoutSeconds">Cold start session timeout seconds
        /// 冷启动会话超时秒数</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateServerRegistryNode(NodeIndex index, string key, NodeInfo nodeInfo, int loadTimeoutSeconds)
        {
            return CreateSnapshotNode<IServerRegistryNode>(index, key, nodeInfo, () => new ServerRegistryNode(loadTimeoutSeconds));
        }
        /// <summary>
        /// Create a service process daemon node IProcessGuardNode
        /// 创建服务进程守护节点 IProcessGuardNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateProcessGuardNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<IProcessGuardNode>(index, key, nodeInfo, () => new ProcessGuardNode());
        }
        /// <summary>
        /// Create a message processing node IMessageNode{ServerByteArrayMessage}
        /// 创建消息处理节点 IMessageNode{ServerByteArrayMessage}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="arraySize">The size of the message array being processed
        /// 正在处理的消息数组大小</param>
        /// <param name="timeoutSeconds">The number of seconds of message processing timeout
        /// 消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">Check the interval in seconds for message timeouts
        /// 消息超时检查间隔秒数</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateServerByteArrayMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds)
        {
            return CreateSnapshotNode<IMessageNode<ServerByteArrayMessage>>(index, key, nodeInfo, () => new MessageNode<ServerByteArrayMessage>(arraySize, timeoutSeconds, checkTimeoutSeconds));
        }
        /// <summary>
        /// Create a message processing node IMessageNode{T}
        /// 创建消息处理节点 IMessageNode{T}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="messageType">Message data type
        /// 消息数据类型</param>
        /// <param name="arraySize">The size of the message array being processed
        /// 正在处理的消息数组大小</param>
        /// <param name="timeoutSeconds">The number of seconds of message processing timeout
        /// 消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">Check the interval in seconds for message timeouts
        /// 消息超时检查间隔秒数</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType messageType, int arraySize, int timeoutSeconds, int checkTimeoutSeconds)
        {
            var type = default(Type);
            if (messageType.TryGet(out type, isCheckRemoveType))
            {
                if (typeof(Message<>).MakeGenericType(type).IsAssignableFrom(type))
                {
                    return new MessageNodeCreator(this, index, key, nodeInfo, arraySize, timeoutSeconds, checkTimeoutSeconds).Create(type);
                }
                return new NodeIndex(CallStateEnum.RemoteTypeError);
            }
            return new NodeIndex(CallStateEnum.NotFoundRemoteType);
        }
        /// <summary>
        /// Create distributed lock nodes IDistributedLockNode{KT}
        /// 创建分布式锁节点 IDistributedLockNode{KT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateDistributedLockNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType)
        {
            var type = default(Type);
            CallStateEnum state = getEquatableType(ref keyType, ref type);
            if (state == CallStateEnum.Success)
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.EquatableGenericType.Get(type.notNull()).CreateDistributedLockNode(this, index, key, nodeInfo);
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// Create a dictionary node IByteArrayFragmentDictionaryNode{KT}
        /// 创建字典节点 IByteArrayFragmentDictionaryNode{KT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateByteArrayFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType)
        {
            var type = default(Type);
            CallStateEnum state = getEquatableType(ref keyType, ref type);
            if (state == CallStateEnum.Success)
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.EquatableGenericType.Get(type.notNull()).CreateByteArrayFragmentDictionaryNode(this, index, key, nodeInfo);
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// Get the type IEquatable{T}
        /// 获取 IEquatable{T} 类型
        /// </summary>
        /// <param name="remoteType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
#if NetStandard21
        protected CallStateEnum getEquatableType(ref AutoCSer.Reflection.RemoteType remoteType, ref Type? type)
#else
        protected CallStateEnum getEquatableType(ref AutoCSer.Reflection.RemoteType remoteType, ref Type type)
#endif
        {
            if (remoteType.TryGet(out type, isCheckRemoveType))
            {
                if (typeof(IEquatable<>).MakeGenericType(type).IsAssignableFrom(type)) return CallStateEnum.Success;
                return CallStateEnum.RemoteTypeError;
            }
            return CallStateEnum.NotFoundRemoteType;
        }
        /// <summary>
        /// Get the type IEquatable{T}
        /// 获取 IEquatable{T} 类型
        /// </summary>
        /// <param name="remoteType"></param>
        /// <param name="type"></param>
        /// <param name="remoteType2"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
#if NetStandard21
        protected CallStateEnum getEquatableType(ref AutoCSer.Reflection.RemoteType remoteType, ref Type? type, ref AutoCSer.Reflection.RemoteType remoteType2, ref Type? type2)
#else
        protected CallStateEnum getEquatableType(ref AutoCSer.Reflection.RemoteType remoteType, ref Type type, ref AutoCSer.Reflection.RemoteType remoteType2, ref Type type2)
#endif
        {
            CallStateEnum state = getEquatableType(ref remoteType, ref type);
            if (state == CallStateEnum.Success)
            {
                if (remoteType2.TryGet(out type2, isCheckRemoveType)) return CallStateEnum.Success;
                return CallStateEnum.NotFoundRemoteType;
            }
            return state;
        }
        /// <summary>
        /// Get the type IEquatable{T}
        /// 获取 IEquatable{T} 类型
        /// </summary>
        /// <param name="remoteType"></param>
        /// <param name="type"></param>
        /// <param name="remoteType2"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
#if NetStandard21
        protected CallStateEnum getEquatableType2(ref AutoCSer.Reflection.RemoteType remoteType, ref Type? type, ref AutoCSer.Reflection.RemoteType remoteType2, ref Type? type2)
#else
        protected CallStateEnum getEquatableType2(ref AutoCSer.Reflection.RemoteType remoteType, ref Type type, ref AutoCSer.Reflection.RemoteType remoteType2, ref Type type2)
#endif
        {
            CallStateEnum state = getEquatableType(ref remoteType, ref type);
            if (state == CallStateEnum.Success) return getEquatableType(ref remoteType2, ref type2);
            return state;
        }
        /// <summary>
        /// Create a dictionary node IFragmentDictionaryNode{KT,VT}
        /// 创建字典节点 IFragmentDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="valueType">Data type</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType)
        {
            var type = default(Type);
            var type2 = default(Type);
            CallStateEnum state = getEquatableType(ref keyType, ref type, ref valueType, ref type2);
            if (state == CallStateEnum.Success)
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.EquatableGenericType2.Get(type.notNull(), type2.notNull()).CreateFragmentDictionaryNode(this, index, key, nodeInfo);
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// Create a dictionary node IByteArrayDictionaryNode{KT}
        /// 创建字典节点 IByteArrayDictionaryNode{KT}
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
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateByteArrayDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity, ReusableDictionaryGroupTypeEnum groupType)
        {
            var type = default(Type);
            CallStateEnum state = getEquatableType(ref keyType, ref type);
            if (state == CallStateEnum.Success)
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.EquatableGenericType.Get(type.notNull()).CreateByteArrayDictionaryNode(this, index, key, nodeInfo, capacity, groupType);
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// Create a dictionary node IDictionaryNode{KT,VT}
        /// 创建字典节点 IDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="valueType">Data type</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType, int capacity, ReusableDictionaryGroupTypeEnum groupType)
        {
            var type = default(Type);
            var type2 = default(Type);
            CallStateEnum state = getEquatableType(ref keyType, ref type, ref valueType, ref type2);
            if (state == CallStateEnum.Success)
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.EquatableGenericType2.Get(type.notNull(), type2.notNull()).CreateDictionaryNode(this, index, key, nodeInfo, capacity, groupType);
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// Get the type IComparable{T}
        /// 获取 IComparable{T} 类型
        /// </summary>
        /// <param name="remoteType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
#if NetStandard21
        protected CallStateEnum getComparableType(ref AutoCSer.Reflection.RemoteType remoteType, ref Type? type)
#else
        protected CallStateEnum getComparableType(ref AutoCSer.Reflection.RemoteType remoteType, ref Type type)
#endif
        {
            if (remoteType.TryGet(out type, isCheckRemoveType))
            {
                if (typeof(IComparable<>).MakeGenericType(type).IsAssignableFrom(type)) return CallStateEnum.Success;
                return CallStateEnum.RemoteTypeError;
            }
            return CallStateEnum.NotFoundRemoteType;
        }
        /// <summary>
        /// Get the type IComparable{T}
        /// 获取 IComparable{T} 类型
        /// </summary>
        /// <param name="remoteType"></param>
        /// <param name="type"></param>
        /// <param name="remoteType2"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
#if NetStandard21
        protected CallStateEnum getComparableType(ref AutoCSer.Reflection.RemoteType remoteType, ref Type? type, ref AutoCSer.Reflection.RemoteType remoteType2, ref Type? type2)
#else
        protected CallStateEnum getComparableType(ref AutoCSer.Reflection.RemoteType remoteType, ref Type type, ref AutoCSer.Reflection.RemoteType remoteType2, ref Type type2)
#endif
        {
            CallStateEnum state = getComparableType(ref remoteType, ref type);
            if (state == CallStateEnum.Success)
            {
                if (remoteType2.TryGet(out type2, isCheckRemoveType)) return CallStateEnum.Success;
                return CallStateEnum.NotFoundRemoteType;
            }
            return state;
        }
        /// <summary>
        /// Create a binary search tree node ISearchTreeDictionaryNode{KT,VT}
        /// 创建二叉搜索树节点 ISearchTreeDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="valueType">Data type</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateSearchTreeDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType)
        {
            var type = default(Type);
            var type2 = default(Type);
            CallStateEnum state = getComparableType(ref keyType, ref type, ref valueType, ref type2);
            if (state == CallStateEnum.Success)
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.ComparableGenericType2.Get(type.notNull(), type2.notNull()).CreateSearchTreeDictionaryNode(this, index, key, nodeInfo);
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// Create a sorting dictionary node ISortedDictionaryNode{KT,VT}
        /// 创建排序字典节点 ISortedDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="valueType">Data type</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateSortedDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType)
        {
            var type = default(Type);
            var type2 = default(Type);
            CallStateEnum state = getComparableType(ref keyType, ref type, ref valueType, ref type2);
            if (state == CallStateEnum.Success)
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.ComparableGenericType2.Get(type.notNull(), type2.notNull()).CreateSortedDictionaryNode(this, index, key, nodeInfo);
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// Create a sorting list node ISortedListNode{KT,VT}
        /// 创建排序列表节点 ISortedListNode{KT,VT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="valueType">Data type</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateSortedListNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType, int capacity)
        {
            var type = default(Type);
            var type2 = default(Type);
            CallStateEnum state = getComparableType(ref keyType, ref type, ref valueType, ref type2);
            if (state == CallStateEnum.Success)
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.ComparableGenericType2.Get(type.notNull(), type2.notNull()).CreateSortedListNode(this, index, key, nodeInfo, capacity);
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// Create a 256 base fragment hash table node IFragmentHashSetNode{KT}
        /// 创建 256 基分片哈希表节点 IFragmentHashSetNode{KT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateFragmentHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType)
        {
            var type = default(Type);
            CallStateEnum state = getEquatableType(ref keyType, ref type);
            if (state == CallStateEnum.Success)
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.EquatableGenericType.Get(type.notNull()).CreateFragmentHashSetNode(this, index, key, nodeInfo);
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// Create a hash table node IHashSetNode{KT}
        /// 创建哈希表节点 IHashSetNode{KT}
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
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity, ReusableDictionaryGroupTypeEnum groupType)
        {
            var type = default(Type);
            CallStateEnum state = getEquatableType(ref keyType, ref type);
            if (state == CallStateEnum.Success)
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.EquatableGenericType.Get(type.notNull()).CreateHashSetNode(this, index, key, nodeInfo, capacity, groupType);
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// Create a binary search tree collection node ISearchTreeSetNode{KT}
        /// 创建二叉搜索树集合节点 ISearchTreeSetNode{KT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateSearchTreeSetNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType)
        {
            var type = default(Type);
            CallStateEnum state = getComparableType(ref keyType, ref type);
            if (state == CallStateEnum.Success)
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.ComparableGenericType.Get(type.notNull()).CreateSearchTreeSetNode(this, index, key, nodeInfo);
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// Create sorted collection node ISortedSetNode{KT}
        /// 创建排序集合节点 ISortedSetNode{KT}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateSortedSetNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType)
        {
            var type = default(Type);
            CallStateEnum state = getComparableType(ref keyType, ref type);
            if (state == CallStateEnum.Success)
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.ComparableGenericType.Get(type.notNull()).CreateSortedSetNode(this, index, key, nodeInfo);
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// Create a queue node IQueueNode{T} (First in, first Out)
        /// 创建队列节点（先进先出） IQueueNode{T}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Container initialization size
        /// 关键字类型</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateQueueNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity)
        {
            var type = default(Type);
            if (keyType.TryGet(out type, isCheckRemoveType))
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.GenericType.Get(type.notNull()).CreateQueueNode(this, index, key, nodeInfo, capacity);
            }
            return new NodeIndex(CallStateEnum.NotFoundRemoteType);
        }
        /// <summary>
        /// Create a stack node IStackNode{T} (Last in, first out)
        /// 创建栈节点（后进先出） IStackNode{T}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Container initialization size
        /// 关键字类型</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateStackNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity)
        {
            var type = default(Type);
            if (keyType.TryGet(out type, isCheckRemoveType))
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.GenericType.Get(type.notNull()).CreateStackNode(this, index, key, nodeInfo, capacity);
            }
            return new NodeIndex(CallStateEnum.NotFoundRemoteType);
        }
        /// <summary>
        /// Create a array node ILeftArrayNode{T}
        /// 创建数组节点 ILeftArrayNode{T}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Container initialization size
        /// 关键字类型</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateLeftArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity)
        {
            var type = default(Type);
            if (keyType.TryGet(out type, isCheckRemoveType))
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.GenericType.Get(type.notNull()).CreateLeftArrayNode(this, index, key, nodeInfo, capacity);
            }
            return new NodeIndex(CallStateEnum.NotFoundRemoteType);
        }
        /// <summary>
        /// Create a array node IArrayNode{T}
        /// 创建数组节点 IArrayNode{T}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="keyType">Keyword type
        /// 关键字类型</param>
        /// <param name="length">Array length</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateArrayNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int length)
        {
            var type = default(Type);
            if (keyType.TryGet(out type, isCheckRemoveType))
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.GenericType.Get(type.notNull()).CreateArrayNode(this, index, key, nodeInfo, length);
            }
            return new NodeIndex(CallStateEnum.NotFoundRemoteType);
        }
        /// <summary>
        /// Create a dictionary node IHashBytesFragmentDictionaryNode
        /// 创建字典节点 IHashBytesFragmentDictionaryNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateHashBytesFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<IHashBytesFragmentDictionaryNode>(index, key, nodeInfo, () => new HashBytesFragmentDictionaryNode());
        }
        /// <summary>
        /// Create a dictionary node IHashBytesDictionaryNode
        /// 创建字典节点 IHashBytesDictionaryNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        /// <param name="groupType">Reusable dictionary recombination operation type
        /// 可重用字典重组操作类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateHashBytesDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity, ReusableDictionaryGroupTypeEnum groupType)
        {
            return CreateSnapshotNode<IHashBytesDictionaryNode>(index, key, nodeInfo, () => new HashBytesDictionaryNode(capacity, groupType));
        }
        /// <summary>
        /// Create a queue node IByteArrayQueueNode (First in, first Out)
        /// 创建队列节点（先进先出） IByteArrayQueueNode
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
        public virtual NodeIndex CreateByteArrayQueueNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return CreateSnapshotNode<IByteArrayQueueNode>(index, key, nodeInfo, () => new ByteArrayQueueNode(capacity));
        }
        /// <summary>
        /// Create a stack node IByteArrayStackNode (Last in, first out)
        /// 创建栈节点（后进先出） IByteArrayStackNode
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
        public virtual NodeIndex CreateByteArrayStackNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return CreateSnapshotNode<IByteArrayStackNode>(index, key, nodeInfo, () => new ByteArrayStackNode(capacity));
        }
        /// <summary>
        /// Create an archive node only IOnlyPersistenceNode{T}
        /// 创建仅存档节点 IOnlyPersistenceNode{T}
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="valueType">Archive data type
        /// 存档数据类型</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateOnlyPersistenceNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType valueType)
        {
            var type = default(Type);
            if (valueType.TryGet(out type, isCheckRemoveType))
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.GenericType.Get(type.notNull()).CreateOnlyPersistenceNode(this, index, key, nodeInfo);
            }
            return new NodeIndex(CallStateEnum.NotFoundRemoteType);
        }
#endif
        /// <summary>
        /// Creat a multi-hash bitmap client synchronization filter node IManyHashBitMapClientFilterNode
        /// 创建多哈希位图客户端同步过滤节点 IManyHashBitMapClientFilterNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="size">Bitmap size (number of bits)
        /// 位图大小（位数量）</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateManyHashBitMapClientFilterNode(NodeIndex index, string key, NodeInfo nodeInfo, int size)
        {
            return CreateSnapshotNode<IManyHashBitMapClientFilterNode>(index, key, nodeInfo, () => new ManyHashBitMapClientFilterNode(size));
        }
        /// <summary>
        /// Creat a multi-hash bitmap filter node IManyHashBitMapFilterNode
        /// 创建多哈希位图过滤节点 IManyHashBitMapFilterNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="size">Bitmap size (number of bits)
        /// 位图大小（位数量）</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateManyHashBitMapFilterNode(NodeIndex index, string key, NodeInfo nodeInfo, int size)
        {
            return CreateSnapshotNode<IManyHashBitMapFilterNode>(index, key, nodeInfo, () => new ManyHashBitMapFilterNode(size));
        }
        /// <summary>
        /// Create a 64-bit auto-increment identity node IIdentityGeneratorNode
        /// 创建 64 位自增ID 节点 IIdentityGeneratorNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="identity">Initial Allocation identity
        /// 起始分配 ID</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateIdentityGeneratorNode(NodeIndex index, string key, NodeInfo nodeInfo, long identity)
        {
            return CreateSnapshotNode<IIdentityGeneratorNode>(index, key, nodeInfo, () => new IdentityGeneratorNode(identity));
        }
        /// <summary>
        /// Create a bitmap node IBitmapNode
        /// 创建位图节点 IBitmapNode
        /// </summary>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="nodeInfo">Server-side node information
        /// 服务端节点信息</param>
        /// <param name="capacity">The number of binary bits
        /// 二进制位数量</param>
        /// <returns>Node identifier, there have been a node is returned directly
        /// 节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateBitmapNode(NodeIndex index, string key, NodeInfo nodeInfo, uint capacity)
        {
            return CreateSnapshotNode<IBitmapNode>(index, key, nodeInfo, () => new BitmapNode(capacity));
        }

        /// <summary>
        /// Create the basic operation nodes of the service
        /// 创建服务基础操作节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static ServerNode<T> CreateServiceNode<T>(StreamPersistenceMemoryDatabaseService service, T target)
             where T : class, IServiceNode
        {
            return new ServerNode<T>(service, ServiceNodeIndex, string.Empty, target);
        }
    }
}
