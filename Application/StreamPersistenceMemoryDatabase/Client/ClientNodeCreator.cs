using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
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
        internal static readonly Type[] NodeConstructorParameterTypes = new Type[] { typeof(string), typeof(Func<NodeIndex, string, NodeInfo, Task<ResponseResult<NodeIndex>>>), typeof(StreamPersistenceMemoryDatabaseClient), typeof(NodeIndex), typeof(bool) };
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal static readonly Func<ClientNode, int, Task<ResponseResult>> StreamPersistenceMemoryDatabaseClientCall = StreamPersistenceMemoryDatabaseClient.Call;
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo StreamPersistenceMemoryDatabaseClientCallInputOutputMethod = typeof(StreamPersistenceMemoryDatabaseClient).GetMethod(nameof(StreamPersistenceMemoryDatabaseClient.CallInputOutput), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal static readonly MethodInfo StreamPersistenceMemoryDatabaseClientInputKeepCallbackMethod = typeof(StreamPersistenceMemoryDatabaseClient).GetMethod(nameof(StreamPersistenceMemoryDatabaseClient.InputKeepCallback), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
    }
    /// <summary>
    /// 生成客户端节点
    /// </summary>
    /// <typeparam name="T">节点接口类型</typeparam>
    internal static class ClientNodeCreator<T> where T : class
    {
        /// <summary>
        /// 创建客户端节点
        /// </summary>
        /// <param name="key">节点全局关键字</param>
        /// <param name="creator">创建节点操作对象委托</param>
        /// <param name="client">日志流持久化内存数据库客户端</param>
        /// <param name="index">节点索引信息</param>
        /// <param name="isPersistenceCallbackExceptionRenewNode">服务端节点产生持久化成功但是执行异常状态时 PersistenceCallbackException 节点将不可操作直到该异常被修复并重启服务端，该参数设置为 true 则在调用发生该异常以后自动删除该服务端节点并重新创建新节点避免该节点长时间不可使用的情况，代价是历史数据将全部丢失</param>
        /// <returns></returns>
        internal static T Create(string key, Func<NodeIndex, string, NodeInfo, Task<ResponseResult<NodeIndex>>> creator, StreamPersistenceMemoryDatabaseClient client, NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
        {
            if (creatorException == null)
            {
                T node = ClientNodeCreator<T>.creator(key, creator, client, index, isPersistenceCallbackExceptionRenewNode);
                if (creatorMessages == null) return node;
                AutoCSer.LogHelper.DebugIgnoreException($"{typeof(T).fullName()} 节点客户端生成警告\r\n{string.Join("\r\n", creatorMessages)}");
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
        internal static NodeInfo GetNodeInfo(out Exception exception)
        {
            exception = creatorException;
            if (creatorMessages == null) return nodeInfo;
            AutoCSer.LogHelper.DebugIgnoreException($"{typeof(T).fullName()} 节点客户端生成警告\r\n{string.Join("\r\n", creatorMessages)}");
            creatorMessages = null;
            return nodeInfo;
        }
        /// <summary>
        /// 服务端信息
        /// </summary>
        private static readonly NodeInfo nodeInfo;
        /// <summary>
        /// 创建客户端节点委托
        /// </summary>
        private static readonly Func<string, Func<NodeIndex, string, NodeInfo, Task<ResponseResult<NodeIndex>>>, StreamPersistenceMemoryDatabaseClient, NodeIndex, bool, T> creator;
        /// <summary>
        /// 节点构造错误
        /// </summary>
        private static readonly Exception creatorException;
        /// <summary>
        /// 节点构造提示信息
        /// </summary>
        private static string[] creatorMessages;
        static ClientNodeCreator()
        {
            Type type = typeof(T), serverType = null;
            try
            {
                string error = NodeType.CheckType(type);
                if (error != null)
                {
                    creatorException = new Exception(error);
                    return;
                }
                ClientNodeAttribute attribute = (ClientNodeAttribute)type.GetCustomAttribute(typeof(ClientNodeAttribute), false) ?? ClientNode.DefaultAttribute;
                serverType = attribute.ServerNodeType;
                if (serverType == null)
                {
                    creatorException = new Exception($"{type.fullName()} 客户端节点没有找到匹配服务端节点接口类型 {typeof(ClientNodeAttribute).fullName()}.{nameof(attribute.ServerNodeType)}");
                    return;
                }
                if (serverType.IsGenericTypeDefinition) serverType = serverType.MakeGenericType(type.GetGenericArguments());
                NodeType nodeType = new NodeType(serverType);
                ClientNodeMethod[] methods;
                if (!nodeType.GetClientMethods(type, ref creatorException, ref creatorMessages, out methods)) return;

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
                constructorGenerator.Emit(OpCodes.Call, typeof(ClientNode<T>).GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, ClientNodeCreator.NodeConstructorParameterTypes, null));
                constructorGenerator.Emit(OpCodes.Ret);
                #endregion
                #endregion
                foreach (ClientNodeMethod method in methods)
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
                            LocalBuilder inputParameterLocalBuilder = null;
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
                            switch (method.CallType)
                            {
                                case CallTypeEnum.Call:
                                    methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientCall.Method);
                                    break;
                                case CallTypeEnum.CallOutput:
                                    if (method.IsSimpleDeserializeParamter) methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientSimpleDeserializeCallOutputDelegate.Method);
                                    else methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientCallOutputDelegate.Method);
                                    break;
                                case CallTypeEnum.CallInput:
                                    if(method.IsSimpleSerializeParamter) methodGenerator.call(StructGenericType.Get(method.InputParameterType.Type).StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputDelegate.Method);
                                    else methodGenerator.call(StructGenericType.Get(method.InputParameterType.Type).StreamPersistenceMemoryDatabaseClientCallInputDelegate.Method);
                                    break;
                                case CallTypeEnum.CallInputOutput:
                                    methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientCallInputOutputMethod.MakeGenericMethod(method.InputParameterType.Type, method.ReturnValueType));
                                    break;
                                case CallTypeEnum.SendOnly:
                                    if (method.IsSimpleSerializeParamter) methodGenerator.call(StructGenericType.Get(method.InputParameterType.Type).StreamPersistenceMemoryDatabaseClientSimpleSerializeSendOnlyDelegate.Method);
                                    else methodGenerator.call(StructGenericType.Get(method.InputParameterType.Type).StreamPersistenceMemoryDatabaseClientSendOnlyDelegate.Method);
                                    break;
                                case CallTypeEnum.KeepCallback:
                                    if (method.IsSimpleDeserializeParamter) methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientSimpleDeserializeKeepCallbackDelegate.Method);
                                    else methodGenerator.call(GenericType.Get(method.ReturnValueType).StreamPersistenceMemoryDatabaseClientKeepCallbackDelegate.Method);
                                    break;
                                case CallTypeEnum.InputKeepCallback:
                                    methodGenerator.call(ClientNodeCreator.StreamPersistenceMemoryDatabaseClientInputKeepCallbackMethod.MakeGenericMethod(method.InputParameterType.Type, method.ReturnValueType));
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
                callConstructorGenerator.Emit(OpCodes.Newobj, creatorType.GetConstructor(ClientNodeCreator.NodeConstructorParameterTypes));
                callConstructorGenerator.Emit(OpCodes.Ret);
                creator = (Func<string, Func<NodeIndex, string, NodeInfo, Task<ResponseResult<NodeIndex>>>, StreamPersistenceMemoryDatabaseClient, NodeIndex, bool, T>)dynamicMethod.CreateDelegate(typeof(Func<string, Func<NodeIndex, string, NodeInfo, Task<ResponseResult<NodeIndex>>>, StreamPersistenceMemoryDatabaseClient, NodeIndex, bool, T>));
                nodeInfo = new NodeInfo(serverType);
            }
            catch (Exception exception)
            {
                creatorException = new Exception($"{serverType?.fullName()} 客户端节点 {type.fullName()} 生成失败", exception);
            }
        }
#if DEBUG
        public interface IDictionary<KT, VT>
            where KT : IEquatable<KT>
        {
            Task<ResponseResult> Clear();
            Task<ResponseResult> Add(KT key, VT value);
            Task<ResponseResult> Set(KT key, VT value);
            Task<ResponseResult<VT>> Get(KT key);
        }
        internal sealed class Dictionary<KT, VT> : ClientNode<IDictionary<KT, VT>>, IDictionary<KT, VT>
            where KT : IEquatable<KT>
        {
            public Dictionary(string key, Func<NodeIndex, string, NodeInfo, Task<ResponseResult<NodeIndex>>> creator, StreamPersistenceMemoryDatabaseClient client, NodeIndex index, bool isPersistenceCallbackExceptionRenewNode) : base(key, creator, client, index, isPersistenceCallbackExceptionRenewNode) { }
            private struct p0
            {
                public KT key;
                public VT value;
            }
            private struct p1
            {
                public KT key;
            }
            Task<ResponseResult> IDictionary<KT, VT>.Clear()
            {
                return StreamPersistenceMemoryDatabaseClient.Call(this, 0);
            }
            Task<ResponseResult> IDictionary<KT, VT>.Add(KT key, VT value)
            {
                return StreamPersistenceMemoryDatabaseClient.CallInput(this, 0, new p0 { key = key, value = value });
            }
            Task<ResponseResult> IDictionary<KT, VT>.Set(KT key, VT value)
            {
                return StreamPersistenceMemoryDatabaseClient.CallInput(this, 1, new p0 { key = key, value = value });
            }
            Task<ResponseResult<VT>> IDictionary<KT, VT>.Get(KT key)
            {
                return StreamPersistenceMemoryDatabaseClient.CallInputOutput<p1, VT>(this, 2, 0, new p1 { key = key });
            }
            public static IDictionary<KT, VT> Create(string key, Func<NodeIndex, string, NodeInfo, Task<ResponseResult<NodeIndex>>> creator, StreamPersistenceMemoryDatabaseClient client, NodeIndex index, bool isPersistenceCallbackExceptionRenewNode)
            {
                return new Dictionary<KT, VT>(key, creator, client, index, isPersistenceCallbackExceptionRenewNode);
            }
        }
#endif
    }
}
