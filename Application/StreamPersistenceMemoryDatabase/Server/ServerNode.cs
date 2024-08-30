using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
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
        internal readonly MethodParameter CreateNodeMethodParameter;
        /// <summary>
        /// 快照事务关系节点集合
        /// </summary>
        internal Dictionary<HashString, ServerNode> SnapshotTransactionNodes;
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
            if (nodeCreator == null) throw new InvalidOperationException($"节点 {GetType().fullName()} 创建失败");
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
        /// 服务端节点移除后处理
        /// </summary>
        internal virtual void OnRemoved()
        {
            IsRemoved = true;
            if (SnapshotTransactionNodes != null)
            {
                HashString key = Key;
                foreach (ServerNode node in SnapshotTransactionNodes.Values) node.SnapshotTransactionNodes.Remove(key);
            }
        }
        /// <summary>
        /// 根据节点全局关键字获取服务端节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal ServerNode GetSnapshotTransactionNode(ref HashString key)
        {
            ServerNode node;
            return SnapshotTransactionNodes != null && SnapshotTransactionNodes.TryGetValue(key, out node) ? node : null;
        }
        /// <summary>
        /// 添加快照事务关系节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="node"></param>
        internal void AppendSnapshotTransaction(ref HashString key, ServerNode node)
        {
            ++NodeCreator.Service.SnapshotTransactionNodeVersion;
            if (SnapshotTransactionNodes == null) SnapshotTransactionNodes = new Dictionary<HashString, ServerNode>();
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
            if (SnapshotTransactionNodes == null) SnapshotTransactionNodes = new Dictionary<HashString, ServerNode>();
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
        internal InputMethodParameter CreateInputMethodParameter(int methodIndex, out CallStateEnum state)
        {
            if ((uint)methodIndex < (uint)NodeCreator.Methods.Length)
            {
                Method method = NodeCreator.Methods[methodIndex];
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
        internal CallStateEnum Call(int methodIndex, ref CommandServerCallback<CallStateEnum> callback)
        {
            if ((uint)methodIndex < (uint)NodeCreator.Methods.Length)
            {
                Method method = NodeCreator.Methods[methodIndex];
                if (method != null)
                {
                    if (method.CallType == CallTypeEnum.Call)
                    {
                        CallMethod callMethod = (CallMethod)method;
                        if (method.IsClientCall)
                        {
                            CommandServerSocketSessionObjectService service = NodeCreator.Service;
                            if (method.IsPersistence)
                            {
                                if (IsPersistence && !service.IsMaster) return CallStateEnum.OnlyMaster;
                                CallMethodParameter callMethodParameter = null;
                                if (method.BeforePersistenceMethodIndex >= 0)
                                {
                                    service.CurrentMethodParameter = callMethodParameter = new BeforePersistenceCallMethodParameter(this, callMethod, callback);
                                    if (!((CallOutputMethod)NodeCreator.Methods[method.BeforePersistenceMethodIndex]).CallBeforePersistence(this)) return CallStateEnum.Success;
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
        internal CallStateEnum CallOutput(int methodIndex, ref CommandServerCallback<ResponseParameter> callback)
        {
            if ((uint)methodIndex < (uint)NodeCreator.Methods.Length)
            {
                Method method = NodeCreator.Methods[methodIndex];
                if (method != null)
                {
                    switch (method.CallType)
                    {
                        case CallTypeEnum.CallOutput:
                        case CallTypeEnum.Callback:
                            CallOutputMethod callOutputMethod = (CallOutputMethod)method;
                            if (method.IsClientCall)
                            {
                                CommandServerSocketSessionObjectService service = NodeCreator.Service;
                                if (method.IsPersistence)
                                {
                                    if (IsPersistence && !service.IsMaster) return CallStateEnum.OnlyMaster;
                                    CallOutputMethodParameter callOutputMethodParameter = null;
                                    if (method.BeforePersistenceMethodIndex >= 0)
                                    {
                                        service.CurrentMethodParameter = callOutputMethodParameter = new BeforePersistenceCallOutputMethodParameter(this, callOutputMethod, callback);
                                        ValueResult<ResponseParameter> value = ((CallOutputMethod)NodeCreator.Methods[method.BeforePersistenceMethodIndex]).CallOutputBeforePersistence(this);
                                        if (value.IsValue)
                                        {
                                            callback?.Callback(value.Value);
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
        internal CallStateEnum KeepCallback(int methodIndex, ref CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            if ((uint)methodIndex < (uint)NodeCreator.Methods.Length)
            {
                Method method = NodeCreator.Methods[methodIndex];
                if (method != null)
                {
                    switch (method.CallType)
                    {
                        case CallTypeEnum.KeepCallback:
                        case CallTypeEnum.Enumerable:
                            KeepCallbackMethod keepCallbackMethod = (KeepCallbackMethod)method;
                            if (method.IsClientCall)
                            {
                                CommandServerSocketSessionObjectService service = NodeCreator.Service;
                                if (method.IsPersistence)
                                {
                                    if (IsPersistence && !service.IsMaster) return CallStateEnum.OnlyMaster;
                                    KeepCallbackMethodParameter keepCallbackMethodParameter = null;
                                    if (method.BeforePersistenceMethodIndex >= 0)
                                    {
                                        service.CurrentMethodParameter = keepCallbackMethodParameter = new BeforePersistenceKeepCallbackMethodParameter(this, keepCallbackMethod, callback);
                                        ValueResult<ResponseParameter> value = ((CallOutputMethod)NodeCreator.Methods[method.BeforePersistenceMethodIndex]).CallOutputBeforePersistence(this);
                                        if (value.IsValue)
                                        {
                                            callback?.CallbackCancelKeep(value.Value.CreateKeepCallback());
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
        /// 获取快照数据集合
        /// </summary>
        /// <returns>是否成功</returns>
        internal virtual bool SetSnapshotArray()
        {
            Rebuilding = false;
            return false;
        }
        /// <summary>
        /// 持久化重建
        /// </summary>
        /// <param name="rebuilder"></param>
        internal virtual void Rebuild(PersistenceRebuilder rebuilder) { }
        /// <summary>
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <param name="rawAssembly"></param>
        /// <param name="method">必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <param name="methodAttribute"></param>
        /// <param name="callback"></param>
        public abstract void Repair(byte[] rawAssembly, MethodInfo method, ServerMethodAttribute methodAttribute, ref CommandServerCallback<CallStateEnum> callback);
        /// <summary>
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <param name="rawAssembly"></param>
        /// <param name="method">必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <param name="methodAttribute"></param>
        /// <param name="callback"></param>
        public abstract void Bind(byte[] rawAssembly, MethodInfo method, ServerMethodAttribute methodAttribute, ref CommandServerCallback<CallStateEnum> callback);

        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        /// <param name="values"></param>
        /// <returns>快照数据集合</returns>
        internal static LeftArray<KeyValue<KT, VT>> GetSnapshotArray<KT, VT>(ICollection<KeyValuePair<KT, VT>> values)
        {
            if (values.Count == 0) return new LeftArray<KeyValue<KT, VT>>(0);
            LeftArray<KeyValue<KT, VT>> array = new LeftArray<KeyValue<KT, VT>>(values.Count);
            foreach (KeyValuePair<KT, VT> value in values) array.Array[array.Length++].Set(value.Key, value.Value);
            return array;
        }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        /// <param name="count"></param>
        /// <param name="values"></param>
        /// <returns>快照数据集合</returns>
        internal static LeftArray<T> GetSnapshotArray<T>(int count, IEnumerable<T> values)
        {
            if (count == 0) return new LeftArray<T>(0);
            LeftArray<T> array = new LeftArray<T>(count);
            foreach (T value in values) array.Array[array.Length++] = value;
            return array;
        }
        /// <summary>
        /// 获取二叉搜索树快照数据集合
        /// </summary>
        /// <param name="count"></param>
        /// <param name="values"></param>
        /// <returns>快照数据集合</returns>
        internal static LeftArray<T> GetSearchTreeSnapshotArray<T>(int count, IEnumerable<T> values)
        {
            LeftArray<T> array = GetSnapshotArray(count, values);
            if (array.Length < 2) return array;
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
            return new LeftArray<T>(treeArray);
        }

        /// <summary>
        /// 默认节点接口配置
        /// </summary>
        internal static readonly ServerNodeAttribute DefaultAttribute = new ServerNodeAttribute();
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
            INode<T> node = target as INode<T>;
            if (node != null)
            {
                try
                {
                    T newTarget = node.StreamPersistenceMemoryDatabaseServiceLoaded();
                    if (newTarget != null) target = newTarget;
                }
                catch (Exception exception)
                {
                    SetPersistenceCallbackException();
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
            INode<T> node = target as INode<T>;
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
            return ServerNodeCreator<T>.MethodParameterCreator(this) as MethodParameterCreator<T>;
        }
        /// <summary>
        /// 修复接口方法错误，强制覆盖原接口方法调用，除了第一个参数为操作节点对象，方法定义必须一致
        /// </summary>
        /// <param name="rawAssembly"></param>
        /// <param name="method">必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号</param>
        /// <param name="methodAttribute"></param>
        /// <param name="callback"></param>
        public override void Repair(byte[] rawAssembly, MethodInfo method, ServerMethodAttribute methodAttribute, ref CommandServerCallback<CallStateEnum> callback)
        {
            NodeCreator.Repair<T>(rawAssembly, method, methodAttribute, callback).NotWait();
            callback = null;
        }
        /// <summary>
        /// 绑定新方法，用于动态增加接口功能，新增方法编号初始状态必须为空闲状态
        /// </summary>
        /// <param name="rawAssembly"></param>
        /// <param name="method">必须是静态方法，第一个参数必须是操作节点接口类型，必须使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerMethodAttribute.MethodIndex 配置方法编号与其他必要配置信息</param>
        /// <param name="methodAttribute"></param>
        /// <param name="callback"></param>
        public override void Bind(byte[] rawAssembly, MethodInfo method, ServerMethodAttribute methodAttribute, ref CommandServerCallback<CallStateEnum> callback)
        {
            NodeCreator.Bind<T>(rawAssembly, method, methodAttribute, callback).NotWait();
            callback = null;
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
    /// <summary>
    /// 支持快照的服务端节点
    /// </summary>
    /// <typeparam name="T">节点接口类型</typeparam>
    /// <typeparam name="ST">快照数据类型</typeparam>
    public sealed class ServerNode<T, ST> : ServerNode<T>
    {
        /// <summary>
        /// 当前节点是否支持重建
        /// </summary>
        internal override bool IsRebuild { get { return IsPersistence; } }
        /// <summary>
        /// 快照数据集合
        /// </summary>
        private LeftArray<ST> snapshotArray;
        /// <summary>
        /// 服务端节点
        /// </summary>
        /// <param name="service"></param>
        /// <param name="index"></param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="target"></param>
        /// <param name="isPersistence">默认为 true 表示持久化为数据库，设置为 false 为纯内存模式在重启服务是数据将丢失</param>
        internal ServerNode(StreamPersistenceMemoryDatabaseService service, NodeIndex index, string key, T target, bool isPersistence = true) : base(service, index, key, checkTarget(target), isPersistence)
        {
        }
        /// <summary>
        /// 检查操作节点对象是否实现快照接口
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private static T checkTarget(T target)
        {
            if (target is ISnapshot<ST>) return target;
            throw new InvalidCastException($"服务端节点类型 {target.GetType().fullName()} 缺少快照接口实现 {typeof(ISnapshot<ST>).fullName()}");
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        internal override void Loaded()
        {
            INode<T> node = target as INode<T>;
            if (node != null)
            {
                try
                {
                    T newTarget = node.StreamPersistenceMemoryDatabaseServiceLoaded();
                    if (newTarget != null) checkTarget(target = newTarget);
                }
                catch (Exception exception)
                {
                    SetPersistenceCallbackException();
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
            }
        }
        /// <summary>
        /// 关闭重建
        /// </summary>
        internal override void CloseRebuild()
        {
            Rebuilding = false;
            snapshotArray.SetEmpty();
        }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        /// <returns>是否成功</returns>
        internal override bool SetSnapshotArray()
        {
            Rebuilding = false;
            if (!IsLoadException)
            {
                snapshotArray = (target as ISnapshot<ST>).GetSnapshotArray();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 持久化重建
        /// </summary>
        /// <param name="rebuilder"></param>
        internal override void Rebuild(PersistenceRebuilder rebuilder)
        {
            LeftArray<ST> array = snapshotArray;
            snapshotArray.SetEmpty();
            rebuilder.Rebuild(ref array);
        }
    }
}
