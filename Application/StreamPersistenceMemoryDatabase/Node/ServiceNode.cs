using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Reflection;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务基础操作
    /// </summary>
    public class ServiceNode : IServiceNode
    {
        /// <summary>
        /// 创建服务基础操作节点标识
        /// </summary>
        internal static readonly NodeIndex ServiceNodeIndex = new NodeIndex(0, 1);

        /// <summary>
        /// 日志流持久化内存数据库服务端
        /// </summary>
        internal readonly StreamPersistenceMemoryDatabaseService Service;
        /// <summary>
        /// 是否通过 AutoCSer.Common.Config.CheckRemoteType 检查远程类型的合法性
        /// </summary>
        protected readonly bool isCheckRemoveType;
        /// <summary>
        /// 服务基础操作
        /// </summary>
        /// <param name="service"></param>
        public ServiceNode(StreamPersistenceMemoryDatabaseService service)
        {
            this.Service = service;
            isCheckRemoveType = !(service is LocalService);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
        public virtual bool RemoveNode(NodeIndex index)
        {
            return Service.RemoveNode(index);
        }
        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <returns>是否成功删除节点，否则表示没有找到节点</returns>
        public virtual bool RemoveNodeByKey(string key)
        {
            return Service.RemoveNode(key);
        }
        /// <summary>
        /// 创建服务端节点（不支持持久化，只有支持快照的节点才支持持久化）
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="getNode">获取节点操作对象</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateNode<T>(NodeIndex index, string key, NodeInfo nodeInfo, Func<T> getNode) where T : class
        {
            try
            {
                if (!Service.IsLoaded) Service.LoadCreateNode(index, key, ref nodeInfo);
                if (!typeof(T).IsInterface) return new NodeIndex(CallStateEnum.OnlySupportInterface);
                ServerNodeCreator nodeCreator = Service.GetNodeCreator<T>();
                if (nodeCreator == null) return new NodeIndex(CallStateEnum.NotFoundNodeCreator);
                NodeIndex nodeIndex = Service.CheckCreateNodeIndex(index, key, ref nodeInfo);
                if (nodeIndex.Index < 0 || !nodeIndex.GetFree()) return nodeIndex;
                return new ServerNode<T>(Service, nodeIndex, key, getNode()).Index;
            }
            finally { Service.RemoveFreeIndex(index); }
        }
        /// <summary>
        /// 创建服务端节点（不支持持久化，只有支持快照的节点才支持持久化）
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns></returns>
        public virtual ValueResult<NodeIndex> CreateNodeBeforePersistence<T>(CreateNodeIndex index, string key, NodeInfo nodeInfo) where T : class
        {
            if (!typeof(T).IsInterface) return new NodeIndex(CallStateEnum.OnlySupportInterface);
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
        /// 创建服务端节点（不支持持久化，只有支持快照的节点才支持持久化）
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <param name="createNodeIndex">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="getNode">获取节点操作对象</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateNode<T>(CreateNodeIndex createNodeIndex, string key, NodeInfo nodeInfo, Func<T> getNode) where T : class
        {
            NodeIndex index = createNodeIndex.Index;
            try
            {
                if (!Service.IsLoaded) Service.LoadCreateNode(index, key, ref nodeInfo);
                return new ServerNode<T>(Service, index, key, getNode()).Index;
            }
            finally { Service.RemoveFreeIndex(index); }
        }
        /// <summary>
        /// 创建支持快照接口的服务端节点（必须保证操作节点对象实现快照接口）
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="getNode">获取节点操作对象（必须保证操作节点对象实现快照接口）</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateSnapshotNode<T>(NodeIndex index, string key, NodeInfo nodeInfo, Func<T> getNode)
            where T : class
        {
            var disposable = default(IDisposable);
            try
            {
                if (!Service.IsLoaded) Service.LoadCreateNode(index, key, ref nodeInfo);
                if (!typeof(T).IsInterface) return new NodeIndex(CallStateEnum.OnlySupportInterface);
                ServerNodeCreator nodeCreator = Service.GetNodeCreator<T>();
                if (nodeCreator == null) return new NodeIndex(CallStateEnum.NotFoundNodeCreator);
                T node = getNode();
                disposable = node as IDisposable;
                CallStateEnum state = nodeCreator.CheckSnapshotNode(node.GetType());
                if (state != CallStateEnum.Success) return new NodeIndex(state);
                NodeIndex nodeIndex = Service.CheckCreateNodeIndex(index, key, ref nodeInfo);
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
        /// 创建支持快照接口的服务端节点（必须保证操作节点对象实现快照接口）
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="nodeCreator">生成服务端节点</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
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
        /// 创建支持快照接口的服务端节点（必须保证操作节点对象实现快照接口）
        /// </summary>
        /// <typeparam name="T">节点接口类型</typeparam>
        /// <param name="createNodeIndex">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="nodeCreator">生成服务端节点</param>
        /// <param name="getNode">获取节点操作对象（必须保证操作节点对象实现快照接口）</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateSnapshotNode<T>(CreateNodeIndex createNodeIndex, string key, NodeInfo nodeInfo, ServerNodeCreator nodeCreator, Func<T> getNode)
            where T : class
        {
            var disposable = default(IDisposable);
            NodeIndex index = createNodeIndex.Index;
            try
            {
                if (!Service.IsLoaded) Service.LoadCreateNode(index, key, ref nodeInfo);
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

        ///// <summary>
        ///// 创建支持快照的服务端节点 参数检查
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="ST"></typeparam>
        ///// <param name="index"></param>
        ///// <param name="key"></param>
        ///// <param name="nodeInfo"></param>
        ///// <param name="nodeIndex"></param>
        ///// <returns>返回 true 表示直接返回无需继续操作</returns>
        //protected virtual bool checkCreateNode<T, ST>(NodeIndex index, string key, NodeInfo nodeInfo, out NodeIndex nodeIndex)
        //{
        //    if (!Service.IsLoaded) Service.LoadCreateNode(index, key);
        //    ServerNodeCreator nodeCreator = Service.GetNodeCreator<T>();
        //    if (nodeCreator == null)
        //    {
        //        nodeIndex = new NodeIndex(CallStateEnum.NotFoundNodeCreator);
        //        return true;
        //    }
        //    if (nodeCreator.SnapshotType != typeof(ST))
        //    {
        //        nodeIndex = new NodeIndex(CallStateEnum.SnapshotTypeNotMatch);
        //        return true;
        //    }
        //    nodeIndex = Service.CheckCreateNodeIndex(index, key, ref nodeInfo);
        //    return nodeIndex.Index < 0 || !nodeIndex.GetFree();
        //}
        ///// <summary>
        ///// 创建支持快照的服务端节点
        ///// </summary>
        ///// <typeparam name="T">节点接口类型</typeparam>
        ///// <typeparam name="NT">节点接口操作对象类型</typeparam>
        ///// <typeparam name="ST">快照数据类型</typeparam>
        ///// <param name="index">节点索引信息</param>
        ///// <param name="key">节点全局关键字</param>
        ///// <param name="nodeInfo">节点信息</param>
        ///// <param name="getNode">获取节点操作对象</param>
        ///// <returns>节点标识，已经存在节点则直接返回</returns>
        //public virtual NodeIndex CreateNode<T, NT, ST>(NodeIndex index, string key, NodeInfo nodeInfo, Func<NT> getNode)
        //    where T : class
        //    where NT : T, ISnapshot<ST>
        //{
        //    try
        //    {
        //        NodeIndex nodeIndex;
        //        if (checkCreateNode<T, ST>(index, key, nodeInfo, out nodeIndex)) return nodeIndex;
        //        return new ServerNode<T, ST>(Service, nodeIndex, key, getNode(), Service.CurrentCallIsPersistence).Index;
        //    }
        //    finally { Service.RemoveFreeIndex(index); }
        //}
        ///// <summary>
        ///// 创建支持快照的服务端节点（必须保证操作节点对象实现快照接口）
        ///// </summary>
        ///// <typeparam name="T">节点接口类型</typeparam>
        ///// <typeparam name="ST">快照数据类型</typeparam>
        ///// <param name="index">节点索引信息</param>
        ///// <param name="key">节点全局关键字</param>
        ///// <param name="nodeInfo">节点信息</param>
        ///// <param name="getNode">获取节点操作对象（必须保证操作节点对象实现快照接口）</param>
        ///// <returns>节点标识，已经存在节点则直接返回</returns>
        //public virtual NodeIndex CreateNode<T, ST>(NodeIndex index, string key, NodeInfo nodeInfo, Func<T> getNode)
        //    where T : class
        //{
        //    try
        //    {
        //        NodeIndex nodeIndex;
        //        if (checkCreateNode<T, ST>(index, key, nodeInfo, out nodeIndex)) return nodeIndex;
        //        return new ServerNode<T, ST>(Service, nodeIndex, key, getNode(), Service.CurrentCallIsPersistence).Index;
        //    }
        //    finally { Service.RemoveFreeIndex(index); }
        //}
        ///// <summary>
        ///// 创建支持快照克隆的服务端节点
        ///// </summary>
        ///// <typeparam name="T">节点接口类型</typeparam>
        ///// <typeparam name="NT">节点接口操作对象类型</typeparam>
        ///// <typeparam name="ST">快照数据类型</typeparam>
        ///// <param name="index">节点索引信息</param>
        ///// <param name="key">节点全局关键字</param>
        ///// <param name="nodeInfo">节点信息</param>
        ///// <param name="getNode">获取节点操作对象</param>
        ///// <returns>节点标识，已经存在节点则直接返回</returns>
        //protected virtual NodeIndex createSnapshotCloneNode<T, NT, ST>(NodeIndex index, string key, NodeInfo nodeInfo, Func<NT> getNode)
        //    where T : class
        //    where NT : T, ISnapshot<ST>
        //    where ST : SnapshotCloneObject<ST>
        //{
        //    try
        //    {
        //        NodeIndex nodeIndex;
        //        if (checkCreateNode<T, ST>(index, key, nodeInfo, out nodeIndex)) return nodeIndex;
        //        return new ServerSnapshotCloneNode<T, ST>(Service, nodeIndex, key, getNode(), Service.CurrentCallIsPersistence).Index;
        //    }
        //    finally { Service.RemoveFreeIndex(index); }
        //}
        ///// <summary>
        ///// 创建支持快照克隆的服务端节点（必须保证操作节点对象实现快照接口）
        ///// </summary>
        ///// <typeparam name="T">节点接口类型</typeparam>
        ///// <typeparam name="ST">快照数据类型</typeparam>
        ///// <param name="index">节点索引信息</param>
        ///// <param name="key">节点全局关键字</param>
        ///// <param name="nodeInfo">节点信息</param>
        ///// <param name="getNode">获取节点操作对象（必须保证操作节点对象实现快照接口）</param>
        ///// <returns>节点标识，已经存在节点则直接返回</returns>
        //protected virtual NodeIndex createSnapshotCloneNode<T, ST>(NodeIndex index, string key, NodeInfo nodeInfo, Func<T> getNode)
        //    where T : class
        //    where ST : SnapshotCloneObject<ST>
        //{
        //    try
        //    {
        //        NodeIndex nodeIndex;
        //        if (checkCreateNode<T, ST>(index, key, nodeInfo, out nodeIndex)) return nodeIndex;
        //        return new ServerSnapshotCloneNode<T, ST>(Service, nodeIndex, key, getNode(), Service.CurrentCallIsPersistence).Index;
        //    }
        //    finally { Service.RemoveFreeIndex(index); }
        //}

        /// <summary>
        /// 创建服务注册节点 IServerRegistryNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="loadTimeoutSeconds">冷启动会话超时秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateServerRegistryNode(NodeIndex index, string key, NodeInfo nodeInfo, int loadTimeoutSeconds)
        {
            return CreateSnapshotNode<IServerRegistryNode>(index, key, nodeInfo, () => new ServerRegistryNode(loadTimeoutSeconds));
        }
        /// <summary>
        /// 创建服务进程守护节点 IProcessGuardNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateProcessGuardNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<IProcessGuardNode>(index, key, nodeInfo, () => new ProcessGuardNode());
        }
        /// <summary>
        /// 创建消息处理节点 MessageNode{ServerByteArrayMessage}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateServerByteArrayMessageNode(NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds)
        {
            return CreateSnapshotNode(index, key, nodeInfo, () => MessageNode<ServerByteArrayMessage>.Create(Service, arraySize, timeoutSeconds, checkTimeoutSeconds));
        }
        /// <summary>
        /// 创建消息处理节点 MessageNode{T}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="messageType">消息数据类型</param>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
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
        /// 创建分布式锁节点 DistributedLockNode{KT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
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
        /// 多哈希位图客户端同步过滤节点 IManyHashBitMapClientFilterNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="size">位图大小（位数量）</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateManyHashBitMapClientFilterNode(NodeIndex index, string key, NodeInfo nodeInfo, int size)
        {
            return CreateSnapshotNode<IManyHashBitMapClientFilterNode>(index, key, nodeInfo, () => new ManyHashBitMapClientFilterNode(size));
        }
        /// <summary>
        /// 创建多哈希位图过滤节点 IManyHashBitMapFilterNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="size">位图大小（位数量）</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateManyHashBitMapFilterNode(NodeIndex index, string key, NodeInfo nodeInfo, int size)
        {
            return CreateSnapshotNode<IManyHashBitMapFilterNode>(index, key, nodeInfo, () => new ManyHashBitMapFilterNode(size));
        }
        /// <summary>
        /// 创建字典节点 IHashBytesFragmentDictionaryNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateHashBytesFragmentDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return CreateSnapshotNode<IHashBytesFragmentDictionaryNode>(index, key, nodeInfo, () => new HashBytesFragmentDictionaryNode());
        }
        /// <summary>
        /// 创建字典节点 IByteArrayFragmentDictionaryNode{KT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
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
        /// 创建字典节点 FragmentDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="valueType">数据类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
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
        /// 创建字典节点 HashBytesDictionaryNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateHashBytesDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return CreateSnapshotNode<IHashBytesDictionaryNode>(index, key, nodeInfo, () => new HashBytesDictionaryNode(capacity));
        }
        /// <summary>
        /// 创建字典节点 ByteArrayDictionaryNode{KT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateByteArrayDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, int capacity)
        {
            var type = default(Type);
            CallStateEnum state = getEquatableType(ref keyType, ref type);
            if (state == CallStateEnum.Success)
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.EquatableGenericType.Get(type.notNull()).CreateByteArrayDictionaryNode(this, index, key, nodeInfo, capacity);
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// 创建字典节点 DictionaryNode{KT,VT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="valueType">数据类型</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateDictionaryNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType, AutoCSer.Reflection.RemoteType valueType, int capacity)
        {
            var type = default(Type);
            var type2 = default(Type);
            CallStateEnum state = getEquatableType(ref keyType, ref type, ref valueType, ref type2);
            if (state == CallStateEnum.Success)
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.EquatableGenericType2.Get(type.notNull(), type2.notNull()).CreateDictionaryNode(this, index, key, nodeInfo, capacity);
            }
            return new NodeIndex(state);
        }
        /// <summary>
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
        /// 创建二叉搜索树节点 SearchTreeDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="valueType">数据类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
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
        /// 创建排序字典节点 SortedDictionaryNode{KT,VT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="valueType">数据类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
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
        /// 创建排序列表节点 SortedListNode{KT,VT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="valueType">数据类型</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
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
        /// 创建 256 基分片哈希表节点 FragmentHashSetNode{KT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
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
        /// 创建哈希表节点 HashSetNode{KT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateHashSetNode(NodeIndex index, string key, NodeInfo nodeInfo, AutoCSer.Reflection.RemoteType keyType)
        {
            var type = default(Type);
            CallStateEnum state = getEquatableType(ref keyType, ref type);
            if (state == CallStateEnum.Success)
            {
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata.EquatableGenericType.Get(type.notNull()).CreateHashSetNode(this, index, key, nodeInfo);
            }
            return new NodeIndex(state);
        }
        /// <summary>
        /// 创建二叉搜索树集合节点 SearchTreeSetNode{KT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
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
        /// 创建排序集合节点 SortedSetNode{KT}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
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
        /// 创建队列节点（先进先出） ByteArrayQueueNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateByteArrayQueueNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return CreateSnapshotNode<IByteArrayQueueNode>(index, key, nodeInfo, () => new ByteArrayQueueNode(capacity));
        }
        /// <summary>
        /// 创建队列节点（先进先出） QueueNode{T}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
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
        /// 创建栈节点（后进先出） ByteArrayStackNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateByteArrayStackNode(NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return CreateSnapshotNode<IByteArrayStackNode>(index, key, nodeInfo, () => new ByteArrayStackNode(capacity));
        }
        /// <summary>
        /// 创建栈节点（后进先出） StackNode{T}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
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
        /// 创建数组节点 LeftArrayNode{T}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
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
        /// 创建数组节点 ArrayNode{T}
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="keyType">关键字类型</param>
        /// <param name="length">数组长度</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
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
        /// 创建 64 位自增ID 节点 IdentityGeneratorNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="identity">起始分配 ID</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateIdentityGeneratorNode(NodeIndex index, string key, NodeInfo nodeInfo, long identity)
        {
            return CreateSnapshotNode<IIdentityGeneratorNode>(index, key, nodeInfo, () => new IdentityGeneratorNode(identity));
        }
        /// <summary>
        /// 创建位图节点 BitmapNode
        /// </summary>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">二进制位数量</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        public virtual NodeIndex CreateBitmapNode(NodeIndex index, string key, NodeInfo nodeInfo, uint capacity)
        {
            return CreateSnapshotNode<IBitmapNode>(index, key, nodeInfo, () => new BitmapNode(capacity));
        }

        /// <summary>
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
