using AutoCSer.Net;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class GenericType : AutoCSer.Metadata.GenericTypeCache<GenericType>
    {
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallOutputDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallOutputWriteDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallOutputCommandDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallOutputWriteCommandDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientKeepCallbackDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientKeepCallbackWriteDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientKeepCallbackCommandDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientKeepCallbackWriteCommandDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeCallOutputDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeCallOutputWriteDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeCallOutputCommandDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeCallOutputWriteCommandDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeKeepCallbackDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeKeepCallbackWriteDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeKeepCallbackCommandDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeKeepCallbackWriteCommandDelegate { get; }
        /// <summary>
        /// 创建回调对象
        /// </summary>
        internal abstract Delegate CreateMethodCallbackDelegate { get; }
        /// <summary>
        /// 创建回调对象
        /// </summary>
        internal abstract Delegate CreateMethodParameterCallbackDelegate { get; }
        /// <summary>
        /// 创建回调对象
        /// </summary>
        internal abstract Delegate CreateMethodKeepCallbackDelegate { get; }
        /// <summary>
        /// 创建回调对象
        /// </summary>
        internal abstract Delegate CreateMethodParameterKeepCallbackDelegate { get; }
        /// <summary>
        /// 获取服务接口回调委托
        /// </summary>
        internal abstract Delegate MethodCallbackGetCallbackDelegate { get; }
        /// <summary>
        /// 添加待加载修复方法节点
        /// </summary>
        internal abstract Action<StreamPersistenceMemoryDatabaseServiceBase, DirectoryInfo, RepairNodeMethodDirectory> AppendRepairNodeMethodLoader { get; }

        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate LocalServiceCallOutputNodeCreateDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate LocalServiceCallbackOutputNodeCreateDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate LocalServiceKeepCallbackEnumeratorNodeCreateDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate LocalServiceKeepCallbackNodeCreateDelegate { get; }

        /// <summary>
        /// 创建队列节点（先进先出） IQueueNode{T}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal abstract NodeIndex CreateQueueNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建栈节点（后进先出） IStackNode{T}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal abstract NodeIndex CreateStackNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建数组节点 ILeftArrayNode{T}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal abstract NodeIndex CreateLeftArrayNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo, int capacity);
        /// <summary>
        /// 创建数组节点 IArrayNode{T}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="length">数组长度</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal abstract NodeIndex CreateArrayNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo, int length);
        /// <summary>
        /// 快照接口节点
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal abstract SnapshotNode CreateSnapshotNode(object target);
        /// <summary>
        /// 快照接口节点
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal abstract SnapshotNode CreateEnumerableSnapshotNode(object target);

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static GenericType create<T>()
        {
            return new GenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static GenericType? lastGenericType;
#else
        protected static GenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static GenericType Get(Type type)
        {
            var value = lastGenericType;
            if (value?.CurrentType == type) return value;
            value = get(type);
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class GenericType<T> : GenericType
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallOutputDelegate { get { return (Func<ClientNode, int, ResponseParameterAwaiter<T>>)StreamPersistenceMemoryDatabaseClient.CallOutput<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallOutputWriteDelegate { get { return (Func<ClientNode, int, ResponseParameterAwaiter<T>>)StreamPersistenceMemoryDatabaseClient.CallOutputWrite<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallOutputCommandDelegate { get { return (Func<ClientNode, int, Action<ResponseResult<T>>, AutoCSer.Net.CallbackCommand>)StreamPersistenceMemoryDatabaseClient.CallOutputCommand<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallOutputWriteCommandDelegate { get { return (Func<ClientNode, int, Action<ResponseResult<T>>, AutoCSer.Net.CallbackCommand>)StreamPersistenceMemoryDatabaseClient.CallOutputWriteCommand<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientKeepCallbackDelegate { get { return (Func<ClientNode, int, Task<KeepCallbackResponse<T>>>)StreamPersistenceMemoryDatabaseClient.KeepCallback<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientKeepCallbackWriteDelegate { get { return (Func<ClientNode, int, Task<KeepCallbackResponse<T>>>)StreamPersistenceMemoryDatabaseClient.KeepCallbackWrite<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientKeepCallbackCommandDelegate { get { return (Func<ClientNode, int, Action<ResponseResult<T>, AutoCSer.Net.KeepCallbackCommand>, AutoCSer.Net.KeepCallbackCommand>)StreamPersistenceMemoryDatabaseClient.KeepCallbackCommand<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientKeepCallbackWriteCommandDelegate { get { return (Func<ClientNode, int, Action<ResponseResult<T>, AutoCSer.Net.KeepCallbackCommand>, AutoCSer.Net.KeepCallbackCommand>)StreamPersistenceMemoryDatabaseClient.KeepCallbackWriteCommand<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeCallOutputDelegate { get { return (Func<ClientNode, int, ResponseParameterAwaiter<T>>)StreamPersistenceMemoryDatabaseClient.SimpleDeserializeCallOutput<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeCallOutputWriteDelegate { get { return (Func<ClientNode, int, ResponseParameterAwaiter<T>>)StreamPersistenceMemoryDatabaseClient.SimpleDeserializeCallOutputWrite<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeCallOutputCommandDelegate { get { return (Func<ClientNode, int, Action<ResponseResult<T>>, AutoCSer.Net.CallbackCommand>)StreamPersistenceMemoryDatabaseClient.SimpleDeserializeCallOutputCommand<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeCallOutputWriteCommandDelegate { get { return (Func<ClientNode, int, Action<ResponseResult<T>>, AutoCSer.Net.CallbackCommand>)StreamPersistenceMemoryDatabaseClient.SimpleDeserializeCallOutputWriteCommand<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeKeepCallbackDelegate { get { return (Func<ClientNode, int, Task<KeepCallbackResponse<T>>>)StreamPersistenceMemoryDatabaseClient.SimpleDeserializeKeepCallback<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeKeepCallbackWriteDelegate { get { return (Func<ClientNode, int, Task<KeepCallbackResponse<T>>>)StreamPersistenceMemoryDatabaseClient.SimpleDeserializeKeepCallbackWrite<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeKeepCallbackCommandDelegate { get { return (Func<ClientNode, int, Action<ResponseResult<T>, AutoCSer.Net.KeepCallbackCommand>, AutoCSer.Net.KeepCallbackCommand>)StreamPersistenceMemoryDatabaseClient.SimpleDeserializeKeepCallbackCommand<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleDeserializeKeepCallbackWriteCommandDelegate { get { return (Func<ClientNode, int, Action<ResponseResult<T>, AutoCSer.Net.KeepCallbackCommand>, AutoCSer.Net.KeepCallbackCommand>)StreamPersistenceMemoryDatabaseClient.SimpleDeserializeKeepCallbackWriteCommand<T>; } }
        /// <summary>
        /// 创建回调对象
        /// </summary>
        internal override Delegate CreateMethodCallbackDelegate { get { return (MethodCallback<T>.CreateDelegate)MethodCallback<T>.Create; } }
        /// <summary>
        /// 创建回调对象
        /// </summary>
        internal override Delegate CreateMethodParameterCallbackDelegate { get { return (Func<CallInputOutputMethodParameter, MethodCallback<T>>)MethodCallback<T>.Create; } }
        /// <summary>
        /// 创建回调对象
        /// </summary>
        internal override Delegate CreateMethodKeepCallbackDelegate { get { return (MethodKeepCallback<T>.CreateDelegate)MethodKeepCallback<T>.Create; } }
        /// <summary>
        /// 创建回调对象
        /// </summary>
        internal override Delegate CreateMethodParameterKeepCallbackDelegate { get { return (Func<InputKeepCallbackMethodParameter, MethodKeepCallback<T>>)MethodKeepCallback<T>.Create; } }
        /// <summary>
        /// 获取服务接口回调委托
        /// </summary>
#if NetStandard21
        internal override Delegate MethodCallbackGetCallbackDelegate { get { return (Func<MethodCallback<T>, CommandServerCallback<ResponseParameter>?>)MethodCallback<T>.GetCallback; } }
#else
        internal override Delegate MethodCallbackGetCallbackDelegate { get { return (Func<MethodCallback<T>, CommandServerCallback<ResponseParameter>>)MethodCallback<T>.GetCallback; } }
#endif
        /// <summary>
        /// 添加待加载修复方法节点
        /// </summary>
        internal override Action<StreamPersistenceMemoryDatabaseServiceBase, DirectoryInfo, RepairNodeMethodDirectory> AppendRepairNodeMethodLoader { get { return StreamPersistenceMemoryDatabaseServiceBase.AppendRepairNodeMethodLoader<T>; } }

        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate LocalServiceCallOutputNodeCreateDelegate { get { return (Func<LocalClientNode, int, bool, LocalServiceQueueNode<LocalResult<T>>>)LocalServiceCallOutputNode<T>.Create; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate LocalServiceCallbackOutputNodeCreateDelegate { get { return (Action<LocalClientNode, int, Action<LocalResult<T>>, bool>)LocalServiceCallbackOutputNode<T>.Create; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate LocalServiceKeepCallbackEnumeratorNodeCreateDelegate { get { return (Func<LocalClientNode, int, bool, LocalServiceQueueNode<LocalKeepCallback<T>>>)LocalServiceKeepCallbackEnumeratorNode<T>.Create; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate LocalServiceKeepCallbackNodeCreateDelegate { get { return (Func<LocalClientNode, int, Action<LocalResult<T>>, bool, LocalServiceQueueNode<IDisposable>>)LocalServiceKeepCallbackNode<T>.Create; } }

        /// <summary>
        /// 创建队列节点（先进先出） IQueueNode{T}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal override NodeIndex CreateQueueNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return node.CreateSnapshotNode<IQueueNode<T>>(index, key, nodeInfo, () => new QueueNode<T>(capacity));
        }
        /// <summary>
        /// 创建栈节点（后进先出） IStackNode{T}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal override NodeIndex CreateStackNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return node.CreateSnapshotNode<IStackNode<T>>(index, key, nodeInfo, () => new StackNode<T>(capacity));
        }
        /// <summary>
        /// 创建数组节点 ILeftArrayNode{T}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal override NodeIndex CreateLeftArrayNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return node.CreateSnapshotNode<ILeftArrayNode<T>>(index, key, nodeInfo, () => new LeftArrayNode<T>(capacity));
        }
        /// <summary>
        /// 创建数组节点 IArrayNode{T}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="length">数组长度</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal override NodeIndex CreateArrayNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo, int length)
        {
            return node.CreateSnapshotNode<IArrayNode<T>>(index, key, nodeInfo, () => new ArrayNode<T>(length));
        }
        /// <summary>
        /// 快照接口节点
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal override SnapshotNode CreateSnapshotNode(object target)
        {
            return new SnapshotNode<T>((ISnapshot<T>)target);
        }
        /// <summary>
        /// 快照接口节点
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal override SnapshotNode CreateEnumerableSnapshotNode(object target)
        {
            return new EnumerableSnapshotNode<T>((IEnumerableSnapshot<T>)target);
        }
    }
}
