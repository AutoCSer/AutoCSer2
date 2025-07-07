using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 生成客户端节点
    /// </summary>
    internal static class ClientNodeCreator
    {
        /// <summary>
        /// 客户端节点方法构造函数参数
        /// </summary>
        internal static readonly Type[] NodeConstructorParameterTypes = new Type[] { typeof(string), typeof(Func<NodeIndex, string, NodeInfo, ResponseParameterAwaiter<NodeIndex>>), typeof(StreamPersistenceMemoryDatabaseClient), typeof(NodeIndex), typeof(bool) };
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly Func<ClientNode, int, ResponseResultAwaiter> StreamPersistenceMemoryDatabaseClientCall = StreamPersistenceMemoryDatabaseClient.Call;
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly Func<ClientNode, int, ResponseResultAwaiter> StreamPersistenceMemoryDatabaseClientCallWrite = StreamPersistenceMemoryDatabaseClient.CallWrite;
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly Func<ClientNode, int, Action<ResponseResult>, AutoCSer.Net.CallbackCommand> StreamPersistenceMemoryDatabaseClientCallCommand = StreamPersistenceMemoryDatabaseClient.CallCommand;
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly Func<ClientNode, int, Action<ResponseResult>, AutoCSer.Net.CallbackCommand> StreamPersistenceMemoryDatabaseClientCallWriteCommand = StreamPersistenceMemoryDatabaseClient.CallWriteCommand;
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo StreamPersistenceMemoryDatabaseClientCallInputOutputMethod = typeof(StreamPersistenceMemoryDatabaseClient).GetMethod(nameof(StreamPersistenceMemoryDatabaseClient.CallInputOutput), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).notNull();
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo StreamPersistenceMemoryDatabaseClientCallInputOutputWriteMethod = typeof(StreamPersistenceMemoryDatabaseClient).GetMethod(nameof(StreamPersistenceMemoryDatabaseClient.CallInputOutputWrite), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).notNull();
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo StreamPersistenceMemoryDatabaseClientCallInputOutputCommandMethod = typeof(StreamPersistenceMemoryDatabaseClient).GetMethod(nameof(StreamPersistenceMemoryDatabaseClient.CallInputOutputCommand), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).notNull();
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo StreamPersistenceMemoryDatabaseClientCallInputOutputWriteCommandMethod = typeof(StreamPersistenceMemoryDatabaseClient).GetMethod(nameof(StreamPersistenceMemoryDatabaseClient.CallInputOutputWriteCommand), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).notNull();
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo StreamPersistenceMemoryDatabaseClientInputKeepCallbackMethod = typeof(StreamPersistenceMemoryDatabaseClient).GetMethod(nameof(StreamPersistenceMemoryDatabaseClient.InputKeepCallback), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).notNull();
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo StreamPersistenceMemoryDatabaseClientInputKeepCallbackWriteMethod = typeof(StreamPersistenceMemoryDatabaseClient).GetMethod(nameof(StreamPersistenceMemoryDatabaseClient.InputKeepCallbackWrite), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).notNull();
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo StreamPersistenceMemoryDatabaseClientInputKeepCallbackCommandMethod = typeof(StreamPersistenceMemoryDatabaseClient).GetMethod(nameof(StreamPersistenceMemoryDatabaseClient.InputKeepCallbackCommand), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).notNull();
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo StreamPersistenceMemoryDatabaseClientInputKeepCallbackWriteCommandMethod = typeof(StreamPersistenceMemoryDatabaseClient).GetMethod(nameof(StreamPersistenceMemoryDatabaseClient.InputKeepCallbackWriteCommand), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).notNull();
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly Func<ClientNode, int, ResponseParameter, ResponseParameterAwaiter<ResponseParameter>> StreamPersistenceMemoryDatabaseClientCallOutputResponseParameter = StreamPersistenceMemoryDatabaseClient.CallOutputResponseParameter;
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly Func<ClientNode, int, ResponseParameter, ResponseParameterAwaiter<ResponseParameter>> StreamPersistenceMemoryDatabaseClientCallOutputWriteResponseParameter = StreamPersistenceMemoryDatabaseClient.CallOutputWriteResponseParameter;
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly Func<ClientNode, int, ResponseParameter, Action<ResponseResult<ResponseParameter>>, AutoCSer.Net.CallbackCommand> StreamPersistenceMemoryDatabaseClientCallOutputCommandResponseParameter = StreamPersistenceMemoryDatabaseClient.CallOutputCommandResponseParameter;
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly Func<ClientNode, int, ResponseParameter, Action<ResponseResult<ResponseParameter>>, AutoCSer.Net.CallbackCommand> StreamPersistenceMemoryDatabaseClientCallOutputWriteCommandResponseParameter = StreamPersistenceMemoryDatabaseClient.CallOutputWriteCommandResponseParameter;
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly Func<ClientNode, int, ResponseParameterSerializer, Task<KeepCallbackResponse<ResponseParameterSerializer>>> StreamPersistenceMemoryDatabaseClientKeepCallbackResponseParameter = StreamPersistenceMemoryDatabaseClient.KeepCallbackResponseParameter;
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly Func<ClientNode, int, ResponseParameterSerializer, Task<KeepCallbackResponse<ResponseParameterSerializer>>> StreamPersistenceMemoryDatabaseClientKeepCallbackWriteResponseParameter = StreamPersistenceMemoryDatabaseClient.KeepCallbackWriteResponseParameter;
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly Func<ClientNode, int, ResponseParameterSerializer, Action<ResponseResult<ResponseParameterSerializer>, AutoCSer.Net.KeepCallbackCommand>, AutoCSer.Net.KeepCallbackCommand> StreamPersistenceMemoryDatabaseClientKeepCallbackCommandResponseParameter = StreamPersistenceMemoryDatabaseClient.KeepCallbackCommandResponseParameter;
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal static readonly Func<ClientNode, int, ResponseParameterSerializer, Action<ResponseResult<ResponseParameterSerializer>, AutoCSer.Net.KeepCallbackCommand>, AutoCSer.Net.KeepCallbackCommand> StreamPersistenceMemoryDatabaseClientKeepCallbackWriteCommandResponseParameter = StreamPersistenceMemoryDatabaseClient.KeepCallbackWriteCommandResponseParameter;
    }
    /// <summary>
    /// 生成客户端节点
    /// </summary>
    /// <typeparam name="T">Client node interface type
    /// 客户端节点接口类型</typeparam>
    internal static class ClientNodeCreator<T> where T : class
    {
        /// <summary>
        /// 创建客户端节点
        /// </summary>
        /// <param name="key">Node global keyword
        /// 节点全局关键字</param>
        /// <param name="creator">创建节点操作对象委托</param>
        /// <param name="client">日志流持久化内存数据库客户端</param>
        /// <param name="index">Node index information
        /// 节点索引信息</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">Default to false said persistence service node produces success but PersistenceCallbackException when performing a abnormal state node will not operate until the anomalies have been restored and restart the server; If set to true, the server node will be automatically deleted and a new node will be recreated after the exception occurs during the call to avoid the situation where the node is unavailable for a long time. The cost is that all historical data will be lost
        /// 默认为 false 表示服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端；设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns></returns>
#if NetStandard21
        internal static T Create(string key, Func<NodeIndex, string, NodeInfo, ResponseParameterAwaiter<NodeIndex>>? creator, StreamPersistenceMemoryDatabaseClient client, NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
#else
        internal static T Create(string key, Func<NodeIndex, string, NodeInfo, ResponseParameterAwaiter<NodeIndex>> creator, StreamPersistenceMemoryDatabaseClient client, NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
#endif
        {
            if (creatorException == null)
            {
                T node = ClientNodeCreator<T>.creator(key, creator, client, index, isPersistenceCallbackExceptionRenewNode);
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
            if (creatorMessages == null) return NodeInfo;
            AutoCSer.LogHelper.DebugIgnoreException(Culture.Configuration.Default.GetClientNodeCreatorWarning(typeof(T), creatorMessages));
            creatorMessages = null;
            return NodeInfo;
        }
        /// <summary>
        /// 服务端信息
        /// </summary>
#if NetStandard21
        internal static readonly NodeInfo? NodeInfo;
#else
        internal static readonly NodeInfo NodeInfo;
#endif
        /// <summary>
        /// 创建客户端节点委托
        /// </summary>
#if NetStandard21
        [AllowNull]
        private static readonly Func<string, Func<NodeIndex, string, NodeInfo, ResponseParameterAwaiter<NodeIndex>>?, StreamPersistenceMemoryDatabaseClient, NodeIndex, bool, T> creator;
#else
        private static readonly Func<string, Func<NodeIndex, string, NodeInfo, ResponseParameterAwaiter<NodeIndex>>, StreamPersistenceMemoryDatabaseClient, NodeIndex, bool, T> creator;
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
        static ClientNodeCreator()
        {
            Type type = typeof(T);
            var serverType = typeof(Type);
            try
            {
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
                if (!nodeType.GetClientMethods(type, ref creatorException, ref creatorMessages, out methods, false)) return;

                TypeBuilder typeBuilder = AutoCSer.Reflection.Emit.Module.Builder.DefineType(AutoCSer.Common.NamePrefix + ".CommandService.StreamPersistenceMemoryDatabase.ClientNode." + type.FullName, TypeAttributes.Class | TypeAttributes.Sealed, typeof(ClientNode<T>), new Type[] { type });
                #region 构造函数
                ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, ClientNodeCreator.NodeConstructorParameterTypes);
                ILGenerator constructorGenerator = constructorBuilder.GetILGenerator();
                #region base(client, index)
                constructorGenerator.Emit(OpCodes.Ldarg_0);
                constructorGenerator.Emit(OpCodes.Ldarg_1);
                constructorGenerator.Emit(OpCodes.Ldarg_2);
                constructorGenerator.Emit(OpCodes.Ldarg_3);
                constructorGenerator.ldarg(4);
                constructorGenerator.ldarg(5);
                constructorGenerator.Emit(OpCodes.Call, typeof(ClientNode<T>).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, ClientNodeCreator.NodeConstructorParameterTypes, null).notNull());
                constructorGenerator.Emit(OpCodes.Ret);
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
                            #region return StreamPersistenceMemoryDatabaseClient.CallInput(this, 0, new p0 { key = key, value = value });
                            methodGenerator.Emit(OpCodes.Ldarg_0);
                            methodGenerator.int32(method.MethodIndex);
                            if (method.IsReturnResponseParameter) methodGenerator.Emit(OpCodes.Ldarg_1);
                            switch (method.CallType)
                            {
                                case CallTypeEnum.CallInputOutput:
                                case CallTypeEnum.InputKeepCallback:
                                    MethodFlagsEnum flags = MethodFlagsEnum.None;
                                    if (method.IsSimpleSerializeParamter) flags |= MethodFlagsEnum.IsSimpleSerializeParamter;
                                    if (method.IsSimpleDeserializeParamter) flags |= MethodFlagsEnum.IsSimpleDeserializeParamter;
                                    methodGenerator.int32((byte)flags);
                                    break;
                            }
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
                            if (method.IsCallback) methodGenerator.ldarg(method.Parameters.Length);
                            switch (method.CallType)
                            {
                                case CallTypeEnum.Call:
                                    if (method.QueueNodeType == Net.CommandServer.ReadWriteNodeTypeEnum.Read)
                                    {
                                        if (method.IsCallback) methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientCallCommand.Method);
                                        else methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientCall.Method);
                                    }
                                    else
                                    {
                                        if (method.IsCallback) methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientCallWriteCommand.Method);
                                        else methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientCallWrite.Method);
                                    }
                                    break;
                                case CallTypeEnum.CallOutput:
                                    if (method.QueueNodeType == Net.CommandServer.ReadWriteNodeTypeEnum.Read)
                                    {
                                        if (method.IsCallback)
                                        {
                                            if (method.IsReturnResponseParameter) methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientCallOutputCommandResponseParameter.Method);
                                            else if (method.IsSimpleDeserializeParamter) methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientSimpleDeserializeCallOutputCommandDelegate.Method);
                                            else methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientCallOutputCommandDelegate.Method);
                                        }
                                        else
                                        {
                                            if (method.IsReturnResponseParameter) methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientCallOutputResponseParameter.Method);
                                            else if (method.IsSimpleDeserializeParamter) methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientSimpleDeserializeCallOutputDelegate.Method);
                                            else methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientCallOutputDelegate.Method);
                                        }
                                    }
                                    else
                                    {
                                        if (method.IsCallback)
                                        {
                                            if (method.IsReturnResponseParameter) methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientCallOutputWriteCommandResponseParameter.Method);
                                            else if (method.IsSimpleDeserializeParamter) methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientSimpleDeserializeCallOutputWriteCommandDelegate.Method);
                                            else methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientCallOutputWriteCommandDelegate.Method);
                                        }
                                        else
                                        {
                                            if (method.IsReturnResponseParameter) methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientCallOutputWriteResponseParameter.Method);
                                            else if (method.IsSimpleDeserializeParamter) methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientSimpleDeserializeCallOutputWriteDelegate.Method);
                                            else methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientCallOutputWriteDelegate.Method);
                                        }
                                    }
                                    break;
                                case CallTypeEnum.CallInput:
                                    if (method.QueueNodeType == Net.CommandServer.ReadWriteNodeTypeEnum.Read)
                                    {
                                        if (method.IsCallback)
                                        {
                                            if (method.IsSimpleSerializeParamter) methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputCommandDelegate.Method);
                                            else methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientCallInputCommandDelegate.Method);
                                        }
                                        else
                                        {
                                            if (method.IsSimpleSerializeParamter) methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputDelegate.Method);
                                            else methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientCallInputDelegate.Method);
                                        }
                                    }
                                    else
                                    {
                                        if (method.IsCallback)
                                        {
                                            if (method.IsSimpleSerializeParamter) methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputWriteCommandDelegate.Method);
                                            else methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientCallInputWriteCommandDelegate.Method);
                                        }
                                        else
                                        {
                                            if (method.IsSimpleSerializeParamter) methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputWriteDelegate.Method);
                                            else methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientCallInputWriteDelegate.Method);
                                        }
                                    }
                                    break;
                                case CallTypeEnum.CallInputOutput:
                                    if (method.QueueNodeType == Net.CommandServer.ReadWriteNodeTypeEnum.Read)
                                    {
                                        if (method.IsCallback)
                                        {
                                            if (method.IsReturnResponseParameter) methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientCallInputOutputCommandResponseParameterDelegate.Method);
                                            else methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientCallInputOutputCommandMethod.MakeGenericMethod(method.InputParameterType.notNull().Type, method.ReturnValueType));
                                        }
                                        else
                                        {
                                            if (method.IsReturnResponseParameter) methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientCallInputOutputResponseParameterDelegate.Method);
                                            else methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientCallInputOutputMethod.MakeGenericMethod(method.InputParameterType.notNull().Type, method.ReturnValueType));
                                        }
                                    }
                                    else
                                    {
                                        if (method.IsCallback)
                                        {
                                            if (method.IsReturnResponseParameter) methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientCallInputOutputWriteCommandResponseParameterDelegate.Method);
                                            else methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientCallInputOutputWriteCommandMethod.MakeGenericMethod(method.InputParameterType.notNull().Type, method.ReturnValueType));
                                        }
                                        else
                                        {
                                            if (method.IsReturnResponseParameter) methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientCallInputOutputWriteResponseParameterDelegate.Method);
                                            else methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientCallInputOutputWriteMethod.MakeGenericMethod(method.InputParameterType.notNull().Type, method.ReturnValueType));
                                        }
                                    }
                                    break;
                                case CallTypeEnum.SendOnly:
                                    if (method.QueueNodeType == Net.CommandServer.ReadWriteNodeTypeEnum.Read)
                                    {
                                        if (method.IsSimpleSerializeParamter) methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientSimpleSerializeSendOnlyDelegate.Method);
                                        else methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientSendOnlyDelegate.Method);
                                    }
                                    else
                                    {
                                        if (method.IsSimpleSerializeParamter) methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientSimpleSerializeSendOnlyWriteDelegate.Method);
                                        else methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientSendOnlyWriteDelegate.Method);
                                    }
                                    break;
                                case CallTypeEnum.KeepCallback:
                                    if (method.QueueNodeType == Net.CommandServer.ReadWriteNodeTypeEnum.Read)
                                    {
                                        if (method.IsCallback)
                                        {
                                            if (method.IsReturnResponseParameter) methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientKeepCallbackCommandResponseParameter.Method);
                                            else if (method.IsSimpleDeserializeParamter) methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientSimpleDeserializeKeepCallbackCommandDelegate.Method);
                                            else methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientKeepCallbackCommandDelegate.Method);
                                        }
                                        else
                                        {
                                            if (method.IsReturnResponseParameter) methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientKeepCallbackResponseParameter.Method);
                                            else if (method.IsSimpleDeserializeParamter) methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientSimpleDeserializeKeepCallbackDelegate.Method);
                                            else methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientKeepCallbackDelegate.Method);
                                        }
                                    }
                                    else
                                    {
                                        if (method.IsCallback)
                                        {
                                            if (method.IsReturnResponseParameter) methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientKeepCallbackWriteCommandResponseParameter.Method);
                                            else if (method.IsSimpleDeserializeParamter) methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientSimpleDeserializeKeepCallbackWriteCommandDelegate.Method);
                                            else methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientKeepCallbackWriteCommandDelegate.Method);
                                        }
                                        else
                                        {
                                            if (method.IsReturnResponseParameter) methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientKeepCallbackWriteResponseParameter.Method);
                                            else if (method.IsSimpleDeserializeParamter) methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientSimpleDeserializeKeepCallbackWriteDelegate.Method);
                                            else methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientKeepCallbackWriteDelegate.Method);
                                        }
                                    }
                                    break;
                                case CallTypeEnum.InputKeepCallback:
                                    if (method.QueueNodeType == Net.CommandServer.ReadWriteNodeTypeEnum.Read)
                                    {
                                        if (method.IsCallback)
                                        {
                                            if (method.IsReturnResponseParameter) methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientInputKeepCallbackCommandResponseParameterDelegate.Method);
                                            else methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientInputKeepCallbackCommandMethod.MakeGenericMethod(method.InputParameterType.notNull().Type, method.ReturnValueType));
                                        }
                                        else
                                        {
                                            if (method.IsReturnResponseParameter) methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientInputKeepCallbackResponseParameterDelegate.Method);
                                            else methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientInputKeepCallbackMethod.MakeGenericMethod(method.InputParameterType.notNull().Type, method.ReturnValueType));
                                        }
                                    }
                                    else
                                    {
                                        if (method.IsCallback)
                                        {
                                            if (method.IsReturnResponseParameter) methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientInputKeepCallbackWriteCommandResponseParameterDelegate.Method);
                                            else methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientInputKeepCallbackWriteCommandMethod.MakeGenericMethod(method.InputParameterType.notNull().Type, method.ReturnValueType));
                                        }
                                        else
                                        {
                                            if (method.IsReturnResponseParameter) methodGenerator.call(StructGenericType.Get(method.InputParameterType.notNull().Type).StreamPersistenceMemoryDatabaseClientInputKeepCallbackWriteResponseParameterDelegate.Method);
                                            else methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientInputKeepCallbackWriteMethod.MakeGenericMethod(method.InputParameterType.notNull().Type, method.ReturnValueType));
                                        }
                                    }
                                    break;
                            }
                            #endregion
                        }
                        else
                        {
                            methodGenerator.ldstr(method.Error);
                            methodGenerator.call(AutoCSer.Net.CommandServer.ClientInterfaceController.ClientInterfaceMethodThrowException.Method);
                            methodGenerator.Emit(OpCodes.Ldnull);
                        }
                        methodGenerator.Emit(OpCodes.Ret);
                    }
                }
                Type creatorType = typeBuilder.CreateType();

                DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "CallConstructor", type, ClientNodeCreator.NodeConstructorParameterTypes, creatorType, true);
                ILGenerator callConstructorGenerator = dynamicMethod.GetILGenerator();
                callConstructorGenerator.Emit(OpCodes.Ldarg_0);
                callConstructorGenerator.Emit(OpCodes.Ldarg_1);
                callConstructorGenerator.Emit(OpCodes.Ldarg_2);
                callConstructorGenerator.Emit(OpCodes.Ldarg_3);
                callConstructorGenerator.ldarg(4);
                callConstructorGenerator.Emit(OpCodes.Newobj, creatorType.GetConstructor(ClientNodeCreator.NodeConstructorParameterTypes).notNull());
                callConstructorGenerator.Emit(OpCodes.Ret);
#if NetStandard21
                creator = (Func<string, Func<NodeIndex, string, NodeInfo, ResponseParameterAwaiter<NodeIndex>>?, StreamPersistenceMemoryDatabaseClient, NodeIndex, bool, T>)dynamicMethod.CreateDelegate(typeof(Func<string, Func<NodeIndex, string, NodeInfo, ResponseParameterAwaiter<NodeIndex>>?, StreamPersistenceMemoryDatabaseClient, NodeIndex, bool, T>));
#else
                creator = (Func<string, Func<NodeIndex, string, NodeInfo, ResponseParameterAwaiter<NodeIndex>>, StreamPersistenceMemoryDatabaseClient, NodeIndex, bool, T>)dynamicMethod.CreateDelegate(typeof(Func<string, Func<NodeIndex, string, NodeInfo, ResponseParameterAwaiter<NodeIndex>>, StreamPersistenceMemoryDatabaseClient, NodeIndex, bool, T>));
#endif
                NodeInfo = new NodeInfo(serverType);
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
            ResponseParameterAwaiter Clear();
            ResponseParameterAwaiter Add(KT key, VT value);
            ResponseParameterAwaiter Set(KT key, VT value);
            ResponseParameterAwaiter<VT> Get(KT key);
        }
        internal sealed class Dictionary<KT, VT> : ClientNode<IDictionary<KT, VT>>, IDictionary<KT, VT>
            where KT : IEquatable<KT>
        {
            public Dictionary(string key, Func<NodeIndex, string, NodeInfo, ResponseParameterAwaiter<NodeIndex>> creator, StreamPersistenceMemoryDatabaseClient client, NodeIndex index, bool isPersistenceCallbackExceptionRenewNode) : base(key, creator, client, index, isPersistenceCallbackExceptionRenewNode) { }
            private struct p0
            {
                public KT key;
                public VT value;
            }
            private struct p1
            {
                public KT key;
            }
            ResponseParameterAwaiter IDictionary<KT, VT>.Clear()
            {
                return StreamPersistenceMemoryDatabaseClient.Call(this, 0);
            }
            ResponseParameterAwaiter IDictionary<KT, VT>.Add(KT key, VT value)
            {
                return StreamPersistenceMemoryDatabaseClient.CallInput(this, 0, new p0 { key = key, value = value });
            }
            ResponseParameterAwaiter IDictionary<KT, VT>.Set(KT key, VT value)
            {
                return StreamPersistenceMemoryDatabaseClient.CallInput(this, 1, new p0 { key = key, value = value });
            }
            ResponseParameterAwaiter<VT> IDictionary<KT, VT>.Get(KT key)
            {
                return StreamPersistenceMemoryDatabaseClient.CallInputOutput<p1, VT>(this, 2, 0, new p1 { key = key });
            }
            public static IDictionary<KT, VT> Create(string key, Func<NodeIndex, string, NodeInfo, ResponseParameterAwaiter<NodeIndex>> creator, StreamPersistenceMemoryDatabaseClient client, NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
            {
                return new Dictionary<KT, VT>(key, creator, client, index, isPersistenceCallbackExceptionRenewNode);
            }
        }
#endif
    }
}
