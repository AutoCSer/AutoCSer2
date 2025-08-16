using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
#if !AOT
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata;
#endif

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
#if !AOT
    /// <summary>
    /// 生成客户端节点
    /// </summary>
    internal static class LocalClientNodeCreator
    {
        /// <summary>
        /// 客户端节点方法构造函数参数
        /// </summary>
        internal static readonly Type[] NodeConstructorParameterTypes = new Type[] { typeof(string), typeof(Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>>), typeof(LocalClient), typeof(NodeIndex), typeof(bool) };
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly Func<LocalClientNode, int, bool, LocalServiceQueueNode<LocalResult>> LocalServiceCallNodeCreate = LocalServiceCallNode.Create;
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly Action<LocalClientNode, int, Action<LocalResult>, bool> LocalServiceCallbackNodeCreate = LocalServiceCallbackNode.Create;
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo LocalServiceCallInputOutputNodeCreateMethod = typeof(LocalServiceCallInputOutputNode).GetMethod(nameof(LocalServiceCallInputOutputNode.Create), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).notNull();
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo LocalServiceCallbackInputOutputNodeCreateMethod = typeof(LocalServiceCallbackInputOutputNode).GetMethod(nameof(LocalServiceCallbackInputOutputNode.Create), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).notNull();
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo LocalServiceInputKeepCallbackEnumeratorNodeCreateMethod = typeof(LocalServiceInputKeepCallbackEnumeratorNode).GetMethod(nameof(LocalServiceInputKeepCallbackEnumeratorNode.Create), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).notNull();
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo LocalServiceInputKeepCallbackNodeCreateMethod = typeof(LocalServiceInputKeepCallbackNode).GetMethod(nameof(LocalServiceInputKeepCallbackNode.Create), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).notNull();
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo LocalServiceTwoStageCallbackNodeCreateMethod = typeof(LocalServiceTwoStageCallbackNode).GetMethod(nameof(LocalServiceTwoStageCallbackNode.Create), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).notNull();
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo LocalServiceInputTwoStageCallbackNodeCreateMethod = typeof(LocalServiceInputTwoStageCallbackNode).GetMethod(nameof(LocalServiceInputTwoStageCallbackNode.Create), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).notNull();
    }
#endif
    /// <summary>
    /// 生成客户端节点
    /// </summary>
    /// <typeparam name="T">Client node interface type
    /// 客户端节点接口类型</typeparam>
    internal static class LocalClientNodeCreator<T> where T : class
    {
        /// <summary>
        /// 创建客户端节点
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="creator">A delegate to create a node operation object
        /// 创建节点操作对象委托</param>
        /// <param name="client">Log stream persistence in-memory database local client
        /// 日志流持久化内存数据库本地客户端</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns></returns>
#if NetStandard21
        internal static T Create(string key, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>>? creator, LocalClient client, NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
#else
        internal static T Create(string key, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>> creator, LocalClient client, NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
#endif
        {
            if (creatorException == null)
            {
                T node = LocalClientNodeCreator<T>.creator(key, creator, client, index, isPersistenceCallbackExceptionRenewNode);
                if (creatorMessages == null) return node;
                AutoCSer.LogHelper.DebugIgnoreException(Culture.Configuration.Default.GetClientNodeCreatorWarning(typeof(T), creatorMessages));
                creatorMessages = null;
                return node;
            }
            throw creatorException;
        }
        /// <summary>
        /// 获取服务端节点信息
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
#if NetStandard21
        internal static NodeInfo? GetNodeInfo(out Exception? exception)
#else
        internal static NodeInfo GetNodeInfo(out Exception exception)
#endif
        {
            exception = creatorException;
            if (creatorMessages == null) return nodeInfo;
            AutoCSer.LogHelper.DebugIgnoreException(Culture.Configuration.Default.GetClientNodeCreatorWarning(typeof(T), creatorMessages));
            creatorMessages = null;
            return nodeInfo;
        }
        /// <summary>
        /// 服务端信息
        /// </summary>
#if NetStandard21
        private static readonly NodeInfo? nodeInfo;
#else
        private static readonly NodeInfo nodeInfo;
#endif
        /// <summary>
        /// 创建客户端节点委托
        /// </summary>
#if NetStandard21
        [AllowNull]
        private static readonly Func<string, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>>?, LocalClient, NodeIndex, bool, T> creator;
#else
        private static readonly Func<string, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>>, LocalClient, NodeIndex, bool, T> creator;
#endif
        /// <summary>
        /// 节点构造错误
        /// </summary>
#if NetStandard21
        private static readonly Exception? creatorException;
#else
        private static readonly Exception creatorException;
#endif
        /// <summary>
        /// 节点构造提示信息
        /// </summary>
#if NetStandard21
        private static string[]? creatorMessages;
#else
        private static string[] creatorMessages;
#endif
        static LocalClientNodeCreator()
        {
            Type type = typeof(T);
            var serverType = typeof(Type);
            try
            {
#if AOT
                var attribute = type.GetCustomAttribute<ClientNodeAttribute>();
                if (attribute?.ClientNodeType != null)
                {
                    var method = attribute.ClientNodeType.GetMethod(ClientNodeAttribute.LocalClientNodeConstructorMethodName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, new Type[] { typeof(string), typeof(Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>>), typeof(LocalClient), typeof(NodeIndex), typeof(bool) });
                    if (method != null && !method.IsGenericMethod && method.ReturnType == type)
                    {
                        creator = (Func<string, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>>?, LocalClient, NodeIndex, bool, T>)method.CreateDelegate(typeof(Func<string, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>>?, LocalClient, NodeIndex, bool, T>));
                        nodeInfo = new NodeInfo(serverType = attribute.ServerNodeType);
                        return;
                    }
                    throw new MissingMethodException(attribute.ClientNodeType.fullName(), ClientNodeAttribute.LocalClientNodeConstructorMethodName);
                }
                throw new MissingMemberException(type.fullName(), typeof(ClientNodeAttribute).fullName());
#else
                var error = NodeType.CheckType(type);
                if (error != null)
                {
                    creatorException = new Exception(error);
                    return;
                }
                ClientNodeAttribute attribute = type.GetCustomAttribute<ClientNodeAttribute>(false) ?? ClientNode.DefaultAttribute;
                serverType = attribute.ServerNodeType;
                if (serverType == null)
                {
                    creatorException = new Exception(Culture.Configuration.Default.GetClientNodeCreatorNotMatchType(type));
                    return;
                }
                if (serverType.IsGenericTypeDefinition) serverType = serverType.MakeGenericType(type.GetGenericArguments());
                NodeType nodeType = new NodeType(serverType);
#if NetStandard21
                ClientNodeMethod?[] methods;
#else
                ClientNodeMethod[] methods;
#endif
                if (!nodeType.GetClientMethods(type, ref creatorException, ref creatorMessages, out methods, true)) return;

                TypeBuilder typeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(AutoCSer.Common.NamePrefix + ".CommandService.StreamPersistenceMemoryDatabase.LocalClientNode." + type.FullName, TypeAttributes.Class | TypeAttributes.Sealed, typeof(LocalClientNode<T>), new Type[] { type });
                #region 构造函数
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, LocalClientNodeCreator.NodeConstructorParameterTypes);
                ILGenerator constructorGenerator = constructorBuilder.GetILGenerator();
                #region base(client, index)
                constructorGenerator.Emit(OpCodes.Ldarg_0);
                constructorGenerator.Emit(OpCodes.Ldarg_1);
                constructorGenerator.Emit(OpCodes.Ldarg_2);
                constructorGenerator.Emit(OpCodes.Ldarg_3);
                constructorGenerator.ldarg(4);
                constructorGenerator.ldarg(5);
                constructorGenerator.Emit(OpCodes.Call, typeof(LocalClientNode<T>).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, LocalClientNodeCreator.NodeConstructorParameterTypes, null).notNull());
                constructorGenerator.ret();
                #endregion
                #endregion
                foreach (var method in methods)
                {
                    if (method != null)
                    {
                        MethodBuilder methodBuilder = typeBuilder.DefineMethod(method.Method.Name, MethodAttributes.Private | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final, method.Method.ReturnType, method.Parameters.getArray(parameter => parameter.ParameterType));
                        typeBuilder.DefineMethodOverride(methodBuilder, method.Method);
                        ILGenerator methodGenerator = methodBuilder.GetILGenerator();
                        if (method.Error == null)
                        {
                            #region return LocalServiceCallInputNode.Create(this, 0, new p0 { key = key, value = value });
                            methodGenerator.Emit(OpCodes.Ldarg_0);
                            methodGenerator.int32(method.MethodIndex);
                            #region p0 inputParameter = new p0 { Value = Value, Ref = Ref };
                            var inputParameterLocalBuilder = default(LocalBuilder);
                            if (method.InputParameterType != null)
                            {
                                inputParameterLocalBuilder = methodGenerator.DeclareLocal(method.InputParameterType.Type);
                                if (method.InputParameterType.IsInitobj)
                                {
                                    methodGenerator.Emit(OpCodes.Ldloca_S, inputParameterLocalBuilder);
                                    methodGenerator.Emit(OpCodes.Initobj, method.InputParameterType.Type);
                                }
                                method.SetInputParameter(methodGenerator, inputParameterLocalBuilder);
                                methodGenerator.Emit(OpCodes.Ldloc_S, inputParameterLocalBuilder);
                            }
                            #endregion
                            if (method.IsCallback)
                            {
                                switch (method.CallType)
                                {
                                    case CallTypeEnum.TwoStageCallback:
                                    case CallTypeEnum.InputTwoStageCallback:
                                        methodGenerator.ldarg(method.Parameters.Length - 1);
                                        break;
                                }
                                methodGenerator.ldarg(method.Parameters.Length);
                            }
                            switch (method.CallType)
                            {
                                case CallTypeEnum.Call:
                                    methodGenerator.int32(method.QueueNodeType == Net.CommandServer.ReadWriteNodeTypeEnum.Read ? 0 : 1);
                                    if (method.IsCallback) methodGenerator.call(LocalClientNodeCreator.LocalServiceCallbackNodeCreate.Method);
                                    else methodGenerator.call(LocalClientNodeCreator.LocalServiceCallNodeCreate.Method);
                                    break;
                                case CallTypeEnum.CallOutput:
                                    methodGenerator.int32(method.QueueNodeType == Net.CommandServer.ReadWriteNodeTypeEnum.Read ? 0 : 1);
                                    if (method.IsCallback) methodGenerator.call(GenericType.Get(method.ReturnValueType).LocalServiceCallbackOutputNodeCreateDelegate.Method);
                                    else methodGenerator.call(GenericType.Get(method.ReturnValueType).LocalServiceCallOutputNodeCreateDelegate.Method);
                                    break;
                                case CallTypeEnum.CallInput:
                                    if (method.IsCallback) methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).LocalServiceCallbackInputNodeCreateDelegate.Method);
                                    else methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).LocalServiceCallInputNodeCreateDelegate.Method);
                                    break;
                                case CallTypeEnum.CallInputOutput:
                                    if (method.IsCallback) methodGenerator.call(LocalClientNodeCreator.LocalServiceCallbackInputOutputNodeCreateMethod.MakeGenericMethod(method.ReturnValueType, method.InputParameterType.notNull().Type));
                                    else methodGenerator.call(LocalClientNodeCreator.LocalServiceCallInputOutputNodeCreateMethod.MakeGenericMethod(method.ReturnValueType, method.InputParameterType.notNull().Type));
                                    break;
                                case CallTypeEnum.SendOnly:
                                    methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).LocalServiceSendOnlyNodeCreateDelegate.Method);
                                    break;
                                case CallTypeEnum.KeepCallback:
                                    methodGenerator.int32(method.QueueNodeType == Net.CommandServer.ReadWriteNodeTypeEnum.Read ? 0 : 1);
                                    if (method.IsCallback) methodGenerator.call(GenericType.Get(method.ReturnValueType).LocalServiceKeepCallbackNodeCreateDelegate.Method);
                                    else methodGenerator.call(GenericType.Get(method.ReturnValueType).LocalServiceKeepCallbackEnumeratorNodeCreateDelegate.Method);
                                    break;
                                case CallTypeEnum.InputKeepCallback:
                                    if (method.IsCallback) methodGenerator.call(LocalClientNodeCreator.LocalServiceInputKeepCallbackNodeCreateMethod.MakeGenericMethod(method.ReturnValueType, method.InputParameterType.notNull().Type));
                                    else methodGenerator.call(LocalClientNodeCreator.LocalServiceInputKeepCallbackEnumeratorNodeCreateMethod.MakeGenericMethod(method.ReturnValueType, method.InputParameterType.notNull().Type));
                                    break;
                                case CallTypeEnum.TwoStageCallback:
                                    methodGenerator.int32(method.QueueNodeType == Net.CommandServer.ReadWriteNodeTypeEnum.Read ? 0 : 1);
                                    methodGenerator.call(LocalClientNodeCreator.LocalServiceTwoStageCallbackNodeCreateMethod.MakeGenericMethod(method.TwoStageReturnValueType, method.ReturnValueType));
                                    break;
                                case CallTypeEnum.InputTwoStageCallback:
                                    methodGenerator.call(LocalClientNodeCreator.LocalServiceInputTwoStageCallbackNodeCreateMethod.MakeGenericMethod(method.TwoStageReturnValueType, method.ReturnValueType, method.InputParameterType.notNull().Type));
                                    break;
                            }
                            #endregion
                        }
                        else
                        {
                            methodGenerator.ldstr(method.Error);
                            methodGenerator.call(AutoCSer.Net.CommandServer.ClientInterfaceController.ClientInterfaceMethodThrowException.Method);
                            methodGenerator.loadNull();
                        }
                        methodGenerator.ret();
                    }
                }
                Type creatorType = typeBuilder.CreateType();

                DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "CallConstructor", type, LocalClientNodeCreator.NodeConstructorParameterTypes, creatorType, true);
                ILGenerator callConstructorGenerator = dynamicMethod.GetILGenerator();
                callConstructorGenerator.Emit(OpCodes.Ldarg_0);
                callConstructorGenerator.Emit(OpCodes.Ldarg_1);
                callConstructorGenerator.Emit(OpCodes.Ldarg_2);
                callConstructorGenerator.Emit(OpCodes.Ldarg_3);
                callConstructorGenerator.ldarg(4);
                callConstructorGenerator.Emit(OpCodes.Newobj, creatorType.GetConstructor(LocalClientNodeCreator.NodeConstructorParameterTypes).notNull());
                callConstructorGenerator.ret();
#if NetStandard21
                creator = (Func<string, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>>?, LocalClient, NodeIndex, bool, T>)dynamicMethod.CreateDelegate(typeof(Func<string, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>>?, LocalClient, NodeIndex, bool, T>));
#else
                creator = (Func<string, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>>, LocalClient, NodeIndex, bool, T>)dynamicMethod.CreateDelegate(typeof(Func<string, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>>, LocalClient, NodeIndex, bool, T>));
#endif
                nodeInfo = new NodeInfo(serverType);
#endif
            }
            catch (Exception exception)
            {
                creatorException = new Exception(Culture.Configuration.Default.GetClientNodeCreatorException(type, serverType), exception);
            }
        }
#if DEBUG && NetStandard21
        public interface IDictionary<KT, VT>
            where KT : IEquatable<KT>
        {
            LocalServiceQueueNode<LocalResult> Clear();
            LocalServiceQueueNode<LocalResult> Add(KT key, VT value);
            LocalServiceQueueNode<LocalResult> Set(KT key, VT value);
            LocalServiceQueueNode<LocalResult<VT>> Get(KT key);
        }
        internal sealed class Dictionary<KT, VT> : LocalClientNode<IDictionary<KT, VT>>, IDictionary<KT, VT>
            where KT : IEquatable<KT>
        {
            public Dictionary(string key, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>> creator, LocalClient client, NodeIndex index, bool isPersistenceCallbackExceptionRenewNode) : base(key, creator, client, index, isPersistenceCallbackExceptionRenewNode) { }
            private struct p0
            {
                public KT key;
                public VT value;
            }
            private struct p1
            {
                public KT key;
            }
            LocalServiceQueueNode<LocalResult> IDictionary<KT, VT>.Clear()
            {
                return LocalServiceCallNode.Create(this, 0, false);
            }
            LocalServiceQueueNode<LocalResult> IDictionary<KT, VT>.Add(KT key, VT value)
            {
                return LocalServiceCallInputNode.Create(this, 0, new p0 { key = key, value = value });
            }
            LocalServiceQueueNode<LocalResult> IDictionary<KT, VT>.Set(KT key, VT value)
            {
                return LocalServiceCallInputNode.Create(this, 1, new p0 { key = key, value = value });
            }
            LocalServiceQueueNode<LocalResult<VT>> IDictionary<KT, VT>.Get(KT key)
            {
                return LocalServiceCallInputOutputNode.Create<VT, p1>(this, 2, new p1 { key = key });
            }
            public static IDictionary<KT, VT> Create(string key, Func<NodeIndex, string, NodeInfo, LocalServiceQueueNode<LocalResult<NodeIndex>>> creator, LocalClient client, NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
            {
                return new Dictionary<KT, VT>(key, creator, client, index, isPersistenceCallbackExceptionRenewNode);
            }
        }
#endif
    }
}
