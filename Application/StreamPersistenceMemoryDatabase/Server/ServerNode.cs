using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata;
using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端节点
    /// </summary>
    public abstract class ServerNode
    {
        /// <summary>
        /// 节点索引信息
        /// </summary>
        public readonly NodeIndex Index;
        /// <summary>
        /// 生成服务端节点
        /// </summary>
        internal readonly ServerNodeCreator NodeCreator;
        /// <summary>
        /// 节点全局关键字
        /// </summary>
        public readonly string Key;
        /// <summary>
        /// 创建节点参数
        /// </summary>
#if NetStandard21
        internal readonly MethodParameter? CreateNodeMethodParameter;
#else
        internal readonly MethodParameter CreateNodeMethodParameter;
#endif
        /// <summary>
        /// 快照事务关系节点集合
        /// </summary>
#if NetStandard21
        internal Dictionary<string, ServerNode>? SnapshotTransactionNodes;
#else
        internal Dictionary<string, ServerNode> SnapshotTransactionNodes;
#endif
        /// <summary>
        /// 快照事务关系节点数量
        /// </summary>
        internal int SnapshotTransactionNodeCount
        {
            get { return SnapshotTransactionNodes == null ? 0 : SnapshotTransactionNodes.Count; }
        }
        /// <summary>
        /// 是否持久化，设置为 false 为纯内存模式在重启服务是数据将丢失
        /// </summary>
        internal readonly bool IsPersistence;
        /// <summary>
        /// 节点调用状态
        /// </summary>
        private CallStateEnum callState;
        /// <summary>
        /// 节点调用状态
        /// </summary>
        internal CallStateEnum CallState
        {
            get
            {
                return callState == CallStateEnum.Success ? NodeCreator.State : callState;
            }
        }
        /// <summary>
        /// 是否重建中，设置为 true 表示忽略持久化回调参数，设置为 false 则需要将回调参数复制添加到待持久化队列中
        /// </summary>
        internal bool Rebuilding;
        /// <summary>
        /// 是否初始化加载执行异常节点
        /// </summary>
        internal bool IsLoadException;
        /// <summary>
        /// 持久化回调是否存在数据更新，节点数据更新以后应该将该状态修改为 true
        /// </summary>
        public bool IsPersistenceCallbackChanged;
        /// <summary>
        /// 当前节点是否支持重建
        /// </summary>
        internal virtual bool IsRebuild { get { return false; } }
        /// <summary>
        /// 当前节点是否已被移除
        /// </summary>
        internal bool IsRemoved;
        /// <summary>
        /// 服务端节点
        /// </summary>
        /// <param name="nodeCreator"></param>
        /// <param name="index"></param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="isPersistence">是否持久化，设置为 false 为纯内存模式在重启服务是数据将丢失</param>
        internal ServerNode(ServerNodeCreator nodeCreator, NodeIndex index, string key, bool isPersistence)
        {
            if (nodeCreator == null) throw new InvalidOperationException(Culture.Configuration.Default.GetServerNodeCreateFailed(GetType()));
            NodeCreator = nodeCreator;
            callState = CallStateEnum.Success;
            Key = key;
            Index = index;
            IsPersistence = isPersistence || index.Equals(ServiceNode.ServiceNodeIndex);
            CreateNodeMethodParameter = nodeCreator.Service.AppendNode(this, key)?.Clone();
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        internal abstract void Loaded();
        /// <summary>
        /// 释放节点资源
        /// </summary>
        internal abstract void NodeDispose();
        /// <summary>
        /// 服务端节点移除后处理
        /// </summary>
        internal virtual void OnRemoved()
        {
            IsRemoved = true;
            if (SnapshotTransactionNodes != null)
            {
                foreach (ServerNode node in SnapshotTransactionNodes.Values) node.SnapshotTransactionNodes.notNull().Remove(Key);
            }
        }
        /// <summary>
        /// 根据节点全局关键字获取服务端节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal ServerNode? GetSnapshotTransactionNode(string key)
#else
        internal ServerNode GetSnapshotTransactionNode(string key)
#endif
        {
            var node = default(ServerNode);
            return SnapshotTransactionNodes != null && SnapshotTransactionNodes.TryGetValue(key, out node) ? node : null;
        }
        /// <summary>
        /// 添加快照事务关系节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="node"></param>
        internal void AppendSnapshotTransaction(string key, ServerNode node)
        {
            ++NodeCreator.Service.SnapshotTransactionNodeVersion;
            if (SnapshotTransactionNodes == null) SnapshotTransactionNodes = DictionaryCreator.CreateAny<string, ServerNode>();
            SnapshotTransactionNodes.Add(key, node);
            node.appendSnapshotTransaction(this);
        }
        /// <summary>
        /// 添加快照事务关系节点
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void appendSnapshotTransaction(ServerNode node)
        {
            if (SnapshotTransactionNodes == null) SnapshotTransactionNodes = DictionaryCreator.CreateAny<string, ServerNode>();
            SnapshotTransactionNodes.Add(node.Key, node);
        }
        /// <summary>
        /// 设置持久化回调错误
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetPersistenceCallbackException()
        {
            if (Index.Index != 0) callState = CallStateEnum.PersistenceCallbackException;
        }
        /// <summary>
        /// 设置初始化加载持久化数据执行异常
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetLoadException()
        {
            IsLoadException = true;
            SetPersistenceCallbackException();
        }
        /// <summary>
        /// 创建方法调用参数信息
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <param name="state"></param>
        /// <returns></returns>
#if NetStandard21
        internal InputMethodParameter? CreateInputMethodParameter(int methodIndex, out CallStateEnum state)
#else
        internal InputMethodParameter CreateInputMethodParameter(int methodIndex, out CallStateEnum state)
#endif
        {
            if ((uint)methodIndex < (uint)NodeCreator.Methods.Length)
            {
                var method = NodeCreator.Methods[methodIndex];
                if (method != null)
                {
                    state = CallStateEnum.Success;
                    switch (method.CallType)
                    {
                        case CallTypeEnum.CallInput: return ((CallInputMethod)method).CreateParameter(this);
                        case CallTypeEnum.CallInputOutput:
                        case CallTypeEnum.InputCallback:
                            return ((CallInputOutputMethod)method).CreateParameter(this);
                        case CallTypeEnum.SendOnly: return ((SendOnlyMethod)method).CreateParameter(this);
                        case CallTypeEnum.InputKeepCallback:
                        case CallTypeEnum.InputEnumerable:
                            return ((InputKeepCallbackMethod)method).CreateParameter(this);
                        default: state = CallStateEnum.CallTypeNotMatch; return null;
                    }
                }
            }
            state = CallStateEnum.NotFoundMethod;
            return null;
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
#if NetStandard21
        internal CallStateEnum Call(int methodIndex, [MaybeNull] ref CommandServerCallback<CallStateEnum> callback)
#else
        internal CallStateEnum Call(int methodIndex, ref CommandServerCallback<CallStateEnum> callback)
#endif
        {
            if ((uint)methodIndex < (uint)NodeCreator.Methods.Length)
            {
                var method = NodeCreator.Methods[methodIndex];
                if (method != null)
                {
                    if (method.CallType == CallTypeEnum.Call)
                    {
                        CallMethod callMethod = (CallMethod)method;
                        if (method.IsClientCall)
                        {
                            StreamPersistenceMemoryDatabaseServiceBase service = NodeCreator.Service;
                            if (method.IsPersistence)
                            {
                                if (IsPersistence && !service.IsMaster) return CallStateEnum.OnlyMaster;
                                var callMethodParameter = default(CallMethodParameter);
                                if (method.BeforePersistenceMethodIndex >= 0)
                                {
                                    service.CurrentMethodParameter = callMethodParameter = new BeforePersistenceCallMethodParameter(this, callMethod, callback);
                                    if (!((CallOutputMethod)NodeCreator.Methods[method.BeforePersistenceMethodIndex].notNull()).CallBeforePersistence(this)) return CallStateEnum.Success;
                                }
                                if (IsPersistence)
                                {
                                    service.PushPersistenceMethodParameter(callMethodParameter ?? new CallMethodParameter(this, callMethod, callback), ref callback);
                                    return CallStateEnum.Success;
                                }
                            }
                            service.CurrentCallIsPersistence = false;
                            callMethod.Call(this, ref callback);
                            return CallStateEnum.Success;
                        }
                        return CallStateEnum.NotAllowClientCall;
                    }
                    return CallStateEnum.CallTypeNotMatch;
                }
            }
            return CallStateEnum.NotFoundMethod;
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
#if NetStandard21
        internal CallStateEnum CallOutput(int methodIndex, [MaybeNull] ref CommandServerCallback<ResponseParameter> callback)
#else
        internal CallStateEnum CallOutput(int methodIndex, ref CommandServerCallback<ResponseParameter> callback)
#endif
        {
            if ((uint)methodIndex < (uint)NodeCreator.Methods.Length)
            {
                var method = NodeCreator.Methods[methodIndex];
                if (method != null)
                {
                    switch (method.CallType)
                    {
                        case CallTypeEnum.CallOutput:
                        case CallTypeEnum.Callback:
                            CallOutputMethod callOutputMethod = (CallOutputMethod)method;
                            if (method.IsClientCall)
                            {
                                StreamPersistenceMemoryDatabaseServiceBase service = NodeCreator.Service;
                                if (method.IsPersistence)
                                {
                                    if (IsPersistence && !service.IsMaster) return CallStateEnum.OnlyMaster;
                                    var callOutputMethodParameter = default(CallOutputMethodParameter);
                                    if (method.BeforePersistenceMethodIndex >= 0)
                                    {
                                        service.CurrentMethodParameter = callOutputMethodParameter = new BeforePersistenceCallOutputMethodParameter(this, callOutputMethod, callback);
                                        ValueResult<ResponseParameter> value = ((CallOutputMethod)NodeCreator.Methods[method.BeforePersistenceMethodIndex].notNull()).CallOutputBeforePersistence(this);
                                        if (value.IsValue)
                                        {
                                            callback?.SynchronousCallback(value.Value);
                                            callback = null;
                                            return CallStateEnum.Success;
                                        }
                                    }
                                    if (IsPersistence)
                                    {
                                        service.PushPersistenceMethodParameter(callOutputMethodParameter ?? new CallOutputMethodParameter(this, callOutputMethod, callback), ref callback);
                                        return CallStateEnum.Success;
                                    }
                                }
                                service.CurrentCallIsPersistence = false;
                                callOutputMethod.CallOutput(this, ref callback);
                                return CallStateEnum.Success;
                            }
                            return CallStateEnum.NotAllowClientCall;
                        default: return CallStateEnum.CallTypeNotMatch;
                    }
                }
            }
            return CallStateEnum.NotFoundMethod;
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
#if NetStandard21
        internal CallStateEnum KeepCallback(int methodIndex, [MaybeNull] ref CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
#else
        internal CallStateEnum KeepCallback(int methodIndex, ref CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
#endif
        {
            if ((uint)methodIndex < (uint)NodeCreator.Methods.Length)
            {
                var method = NodeCreator.Methods[methodIndex];
                if (method != null)
                {
                    switch (method.CallType)
                    {
                        case CallTypeEnum.KeepCallback:
                        case CallTypeEnum.Enumerable:
                            KeepCallbackMethod keepCallbackMethod = (KeepCallbackMethod)method;
                            if (method.IsClientCall)
                            {
                                StreamPersistenceMemoryDatabaseServiceBase service = NodeCreator.Service;
                                if (method.IsPersistence)
                                {
                                    if (IsPersistence && !service.IsMaster) return CallStateEnum.OnlyMaster;
                                    var keepCallbackMethodParameter = default(KeepCallbackMethodParameter);
                                    if (method.BeforePersistenceMethodIndex >= 0)
                                    {
                                        service.CurrentMethodParameter = keepCallbackMethodParameter = new BeforePersistenceKeepCallbackMethodParameter(this, keepCallbackMethod, callback);
                                        ValueResult<ResponseParameter> value = ((CallOutputMethod)NodeCreator.Methods[method.BeforePersistenceMethodIndex].notNull()).CallOutputBeforePersistence(this);
                                        if (value.IsValue)
                                        {
                                            callback?.VirtualCallbackCancelKeep(value.Value.CreateKeepCallback());
                                            callback = null;
                                            return CallStateEnum.Success;
                                        }
                                    }
                                    if (IsPersistence)
                                    {
                                        service.PushPersistenceMethodParameter(keepCallbackMethodParameter ?? new KeepCallbackMethodParameter(this, keepCallbackMethod, callback), ref callback);
                                        return CallStateEnum.Success;
                                    }
                                }
                                service.CurrentCallIsPersistence = false;
                                keepCallbackMethod.KeepCallback(this, ref callback);
                                return CallStateEnum.Success;
                            }
                            return CallStateEnum.NotAllowClientCall;
                        default: return CallStateEnum.CallTypeNotMatch;
                    }
                }
            }
            return CallStateEnum.NotFoundMethod;
        }
        /// <summary>
        /// 关闭重建
        /// </summary>
        internal virtual void CloseRebuild() { Rebuilding = false; }
        /// <summary>
        /// 检查快照重建状态
        /// </summary>
        /// <returns></returns>
        internal virtual bool CheckSnapshot()
        {
            Rebuilding = false;
            return false;
        }
        /// <summary>
        /// 预申请快照容器数组
        /// </summary>
        internal virtual void GetSnapshotArray() { }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        /// <returns>是否成功</returns>
        internal virtual bool GetSnapshotResult()
        {
            return false;
        }
        /// <summary>
        /// 持久化重建
        /// </summary>
        /// <param name="rebuilder"></param>
        /// <returns></returns>
        internal virtual bool Rebuild(PersistenceRebuilder rebuilder) { return false; }
        /// <summary>
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <param name="rawAssembly"></param>
        /// <param name="method">必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <param name="methodAttribute"></param>
        /// <param name="callback"></param>
        public abstract void Repair(byte[] rawAssembly, MethodInfo method, ServerMethodAttribute methodAttribute, CommandServerCallback<CallStateEnum> callback);
        /// <summary>
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <param name="rawAssembly"></param>
        /// <param name="method">必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <param name="methodAttribute"></param>
        /// <param name="callback"></param>
        public abstract void Bind(byte[] rawAssembly, MethodInfo method, ServerMethodAttribute methodAttribute, CommandServerCallback<CallStateEnum> callback);

        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        /// <param name="values"></param>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <returns>快照数据信息</returns>
        public static SnapshotResult<KeyValue<KT, VT>> GetSnapshotResult<KT, VT>(ICollection<KeyValuePair<KT, VT>> values, KeyValue<KT, VT>[] snapshotArray)
        {
            if (values.Count == 0) return new SnapshotResult<KeyValue<KT, VT>>(0);
            SnapshotResult<KeyValue<KT, VT>> result = new SnapshotResult<KeyValue<KT, VT>>(values.Count, snapshotArray.Length);
            foreach (KeyValuePair<KT, VT> value in values)
            {
                if (result.Count != snapshotArray.Length) snapshotArray[result.Count++].Set(value.Key, value.Value);
                else result.Array.Array[result.Array.Length++].Set(value.Key, value.Value);
            }
            return result;
        }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        /// <param name="values"></param>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <returns>快照数据信息</returns>
        public static SnapshotResult<KeyValue<KT, VT>> GetSnapshotResult<KT, VT>(ICollection<KeyValuePair<KT, CheckSnapshotCloneObject<VT>>> values, KeyValue<KT, VT>[] snapshotArray)
            where VT : SnapshotCloneObject<VT>
        {
            if (values.Count == 0) return new SnapshotResult<KeyValue<KT, VT>>(0);
            SnapshotResult<KeyValue<KT, VT>> result = new SnapshotResult<KeyValue<KT, VT>>(values.Count, snapshotArray.Length);
            foreach (KeyValuePair<KT, CheckSnapshotCloneObject<VT>> value in values)
            {
                if (result.Count != snapshotArray.Length) snapshotArray[result.Count++].Set(value.Key, value.Value.NoCheckNotNull());
                else result.Array.Array[result.Array.Length++].Set(value.Key, value.Value.NoCheckNotNull());
            }
            return result;
        }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        /// <param name="values"></param>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <returns>快照数据信息</returns>
        internal static SnapshotResult<KeyValue<byte[], VT>> GetSnapshotResult<VT>(ICollection<KeyValuePair<HashBytes, VT>> values, KeyValue<byte[], VT>[] snapshotArray)
        {
            if (values.Count == 0) return new SnapshotResult<KeyValue<byte[], VT>>(0);
            SnapshotResult<KeyValue<byte[], VT>> result = new SnapshotResult<KeyValue<byte[], VT>>(values.Count, snapshotArray.Length);
            foreach (KeyValuePair<HashBytes, VT> value in values)
            {
                if (result.Count != snapshotArray.Length) snapshotArray[result.Count++].Set(value.Key.SubArray.Array, value.Value);
                else result.Array.Array[result.Array.Length++].Set(value.Key.SubArray.Array, value.Value);
            }
            return result;
        }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        /// <param name="values"></param>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <returns>快照数据信息</returns>
        internal static SnapshotResult<KeyValue<byte[], VT>> GetSnapshotResult<VT>(ICollection<KeyValuePair<HashBytes, CheckSnapshotCloneObject<VT>>> values, KeyValue<byte[], VT>[] snapshotArray)
            where VT : SnapshotCloneObject<VT>
        {
            if (values.Count == 0) return new SnapshotResult<KeyValue<byte[], VT>>(0);
            SnapshotResult<KeyValue<byte[], VT>> result = new SnapshotResult<KeyValue<byte[], VT>>(values.Count, snapshotArray.Length);
            foreach (KeyValuePair<HashBytes, CheckSnapshotCloneObject<VT>> value in values)
            {
                if (result.Count != snapshotArray.Length) snapshotArray[result.Count++].Set(value.Key.SubArray.Array, value.Value.NoCheckNotNull());
                else result.Array.Array[result.Array.Length++].Set(value.Key.SubArray.Array, value.Value.NoCheckNotNull());
            }
            return result;
        }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        /// <param name="values"></param>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <returns>快照数据信息</returns>
        internal static SnapshotResult<KeyValue<KT, VT>> GetSnapshotResult<KT, VT>(FragmentDictionary256<KT, VT> values, KeyValue<KT, VT>[] snapshotArray)
            where KT : IEquatable<KT>
        {
            if (values.Count == 0) return new SnapshotResult<KeyValue<KT, VT>>(0);
            SnapshotResult<KeyValue<KT, VT>> result = new SnapshotResult<KeyValue<KT, VT>>(values.Count, snapshotArray.Length);
            foreach (KeyValuePair<KT, VT> value in values.KeyValues)
            {
                if (result.Count != snapshotArray.Length) snapshotArray[result.Count++].Set(value.Key, value.Value);
                else result.Array.Array[result.Array.Length++].Set(value.Key, value.Value);
            }
            return result;
        }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        /// <param name="values"></param>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <returns>快照数据信息</returns>
        internal static SnapshotResult<KeyValue<byte[], VT>> GetSnapshotResult<VT>(HashBytesFragmentDictionary256<VT> values, KeyValue<byte[], VT>[] snapshotArray)
        {
            if (values.Count == 0) return new SnapshotResult<KeyValue<byte[], VT>>(0);
            SnapshotResult<KeyValue<byte[], VT>> result = new SnapshotResult<KeyValue<byte[], VT>>(values.Count, snapshotArray.Length);
            foreach (KeyValuePair<HashBytes, VT> value in values.KeyValues)
            {
                if (result.Count != snapshotArray.Length) snapshotArray[result.Count++].Set(value.Key.SubArray.Array, value.Value);
                else result.Array.Array[result.Array.Length++].Set(value.Key.SubArray.Array, value.Value);
            }
            return result;
        }
        ///// <summary>
        ///// 获取快照数据集合
        ///// </summary>
        ///// <param name="values"></param>
        ///// <param name="snapshotArray"></param>
        ///// <param name="count"></param>
        ///// <returns>快照数据集合</returns>
        //internal static LeftArray<T> GetSnapshotResult<T>(IEnumerable<T> values, T[] snapshotArray, int count)
        //{
        //    LeftArray<T> leftArray = count > snapshotArray.Length ? new LeftArray<T>(count) : new LeftArray<T>(0, snapshotArray);
        //    foreach (T value in values) leftArray.Add(value);
        //    return leftArray;
        //}
        /// <summary>
        /// 获取二叉搜索树快照数据集合
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        internal static void SetSearchTreeSnapshotResult<T>(ref LeftArray<T> array, ref LeftArray<T> newArray)
        {
            array.Add(ref newArray);
            newArray.SetEmpty();
            if (array.Length < 2) return;

            T[] treeArray = new T[array.Length];
            KeyValue<int, int>[] framents = new KeyValue<int, int>[array.Length];
            framents[0].Set(0, array.Length);
            int framentIndex = 0, framentEndIndex = 1, treeArrayIndex = 0;
            do
            {
                KeyValue<int, int> frament = framents[framentIndex++];
                switch (frament.Value)
                {
                    case 1:
                        treeArray[treeArrayIndex++] = array.Array[frament.Key];
                        break;
                    case 2:
                        treeArray[treeArrayIndex++] = array.Array[frament.Key + 1];
                        framents[framentEndIndex++].Set(frament.Key, 1);
                        break;
                    default:
                        int leftCount = frament.Value >> 1, middle = frament.Key + leftCount;
                        treeArray[treeArrayIndex++] = array.Array[middle];
                        framents[framentEndIndex++].Set(frament.Key, leftCount);
                        framents[framentEndIndex++].Set(middle + 1, frament.Value - (leftCount + 1));
                        break;
                }
            }
            while (framentIndex != framentEndIndex);
            array.Set(treeArray);
        }

        /// <summary>
        /// 默认节点接口配置
        /// </summary>
        internal static readonly ServerNodeAttribute DefaultAttribute = new ServerNodeAttribute();
        /// <summary>
        /// 默认节点接口配置
        /// </summary>
        internal static readonly ServerNodeMethodIndexAttribute DefaultMethodIndexAttribute = new ServerNodeMethodIndexAttribute();
    }
    /// <summary>
    /// 服务端节点
    /// </summary>
    /// <typeparam name="T">节点接口类型</typeparam>
    public class ServerNode<T> : ServerNode
    {
        /// <summary>
        /// 操作节点对象
        /// </summary>
        protected T target;
        /// <summary>
        /// 操作节点对象
        /// </summary>
        public T Target { get { return target; } }
        /// <summary>
        /// 服务端节点
        /// </summary>
        /// <param name="service"></param>
        /// <param name="index"></param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="target"></param>
        /// <param name="isPersistence">默认为 true 表示持久化为数据库，设置为 false 为纯内存模式在重启服务是数据将丢失</param>
        protected ServerNode(StreamPersistenceMemoryDatabaseService service, NodeIndex index, string key, T target, bool isPersistence) : base(service.GetNodeCreator<T>(), index, key, isPersistence)
        {
            this.target = target;
            (target as INode<T>)?.SetContext(this);
            if (service.IsLoaded) Loaded();
        }
        /// <summary>
        /// 服务端节点（除了 服务基础操作节点 以外，该调用不支持节点持久化，只有支持快照的节点才支持持久化）
        /// </summary>
        /// <param name="service"></param>
        /// <param name="index"></param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="target"></param>
        public ServerNode(StreamPersistenceMemoryDatabaseService service, NodeIndex index, string key, T target) : this(service, index, key, target, false) { }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        internal override void Loaded()
        {
            var node = target as INode<T>;
            if (node != null)
            {
                try
                {
                    var newTarget = node.StreamPersistenceMemoryDatabaseServiceLoaded();
                    if (newTarget != null && !object.ReferenceEquals(target, newTarget)) target = checkNewTarget(newTarget);
                }
                catch (Exception exception)
                {
                    SetPersistenceCallbackException();
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
            }
        }
        /// <summary>
        /// 检查操作节点对象是否合法
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected virtual T checkNewTarget(T target)
        {
            return target;
        }
        /// <summary>
        /// 释放节点资源
        /// </summary>
        internal override void NodeDispose()
        {
            var node = target as INode;
            if (node != null)
            {
                try
                {
                    node.StreamPersistenceMemoryDatabaseServiceDisposable();
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
            }
        }
        /// <summary>
        /// 服务端节点移除后处理
        /// </summary>
        internal override void OnRemoved()
        {
            base.OnRemoved();
            var node = target as INode;
            if (node != null)
            {
                try
                {
                    node.StreamPersistenceMemoryDatabaseServiceNodeOnRemoved();
                }
                catch(Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
            }
        }
        /// <summary>
        /// 创建调用方法与参数信息，派生自类型 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameterCreator
        /// </summary>
        /// <returns>创建调用方法与参数信息</returns>
        public MethodParameterCreator<T> CreateMethodParameterCreator()
        {
            return (ServerNodeCreator<T>.MethodParameterCreator(this) as MethodParameterCreator<T>).notNull();
        }
        /// <summary>
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <param name="rawAssembly"></param>
        /// <param name="method">必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <param name="methodAttribute"></param>
        /// <param name="callback"></param>
        public override void Repair(byte[] rawAssembly, MethodInfo method, ServerMethodAttribute methodAttribute, CommandServerCallback<CallStateEnum> callback)
        {
            NodeCreator.Repair<T>(rawAssembly, method, methodAttribute, callback).NotWait();
        }
        /// <summary>
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <param name="rawAssembly"></param>
        /// <param name="method">必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <param name="methodAttribute"></param>
        /// <param name="callback"></param>
        public override void Bind(byte[] rawAssembly, MethodInfo method, ServerMethodAttribute methodAttribute, CommandServerCallback<CallStateEnum> callback)
        {
            NodeCreator.Bind<T>(rawAssembly, method, methodAttribute, callback).NotWait();
        }

        /// <summary>
        /// 获取操作节点对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static T GetTarget(ServerNode<T> node)
        {
            return node.target;
        }
    }
}
