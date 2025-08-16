using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class StructGenericType : AutoCSer.Metadata.GenericTypeCache<StructGenericType>
    {
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallInputDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallInputWriteDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallInputCommandDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallInputWriteCommandDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallInputOutputResponseParameterDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallInputOutputWriteResponseParameterDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallInputOutputCommandResponseParameterDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallInputOutputWriteCommandResponseParameterDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientInputKeepCallbackResponseParameterDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientInputKeepCallbackWriteResponseParameterDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientInputKeepCallbackCommandResponseParameterDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientInputKeepCallbackWriteCommandResponseParameterDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputWriteDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputCommandDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputWriteCommandDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSendOnlyDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSendOnlyWriteDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeSendOnlyDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeSendOnlyWriteDelegate { get; }
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
        internal abstract Delegate MethodParameterCreatorCreateCallInputMethodParameterDelegate { get; }
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
        internal abstract Delegate MethodParameterCreatorCreateCallInputOutputMethodParameterDelegate { get; }
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
        internal abstract Delegate MethodParameterCreatorCreateCallInputOutputCallbackMethodParameterDelegate { get; }
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
        internal abstract Delegate MethodParameterCreatorCreateSendOnlyMethodParameterDelegate { get; }
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
        internal abstract Delegate MethodParameterCreatorCreateInputKeepCallbackMethodParameterDelegate { get; }
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
        internal abstract Delegate MethodParameterCreatorCreateInputTwoStageCallbackMethodParameterDelegate { get; }

        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate LocalServiceCallInputNodeCreateDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate LocalServiceCallbackInputNodeCreateDelegate { get; }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate LocalServiceSendOnlyNodeCreateDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static StructGenericType create<T>() where T :struct
        {
            return new StructGenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static StructGenericType? lastGenericType;
#else
        protected static StructGenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static StructGenericType Get(Type type)
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
    internal sealed class StructGenericType<T> : StructGenericType
        where T : struct
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputDelegate { get { return (Func<ClientNode, int, T, ResponseResultAwaiter>)StreamPersistenceMemoryDatabaseClient.CallInput<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputWriteDelegate { get { return (Func<ClientNode, int, T, ResponseResultAwaiter>)StreamPersistenceMemoryDatabaseClient.CallInputWrite<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputCommandDelegate { get { return (Func<ClientNode, int, T, Action<ResponseResult>, AutoCSer.Net.CallbackCommand>)StreamPersistenceMemoryDatabaseClient.CallInputCommand<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputWriteCommandDelegate { get { return (Func<ClientNode, int, T, Action<ResponseResult>, AutoCSer.Net.CallbackCommand>)StreamPersistenceMemoryDatabaseClient.CallInputWriteCommand<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputOutputResponseParameterDelegate { get { return (Func<ClientNode, int, ResponseParameter, MethodFlagsEnum, T, ResponseParameterAwaiter<ResponseParameter>>)StreamPersistenceMemoryDatabaseClient.CallInputOutputResponseParameter<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputOutputWriteResponseParameterDelegate { get { return (Func<ClientNode, int, ResponseParameter, MethodFlagsEnum, T, ResponseParameterAwaiter<ResponseParameter>>)StreamPersistenceMemoryDatabaseClient.CallInputOutputWriteResponseParameter<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputOutputCommandResponseParameterDelegate { get { return (Func<ClientNode, int, ResponseParameter, MethodFlagsEnum, T, Action<ResponseResult<ResponseParameter>>, AutoCSer.Net.CallbackCommand>)StreamPersistenceMemoryDatabaseClient.CallInputOutputCommandResponseParameter<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputOutputWriteCommandResponseParameterDelegate { get { return (Func<ClientNode, int, ResponseParameter, MethodFlagsEnum, T, Action<ResponseResult<ResponseParameter>>, AutoCSer.Net.CallbackCommand>)StreamPersistenceMemoryDatabaseClient.CallInputOutputWriteCommandResponseParameter<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientInputKeepCallbackResponseParameterDelegate { get { return (Func<ClientNode, int, ResponseParameterSerializer, MethodFlagsEnum, T, Task<KeepCallbackResponse<ResponseParameterSerializer>>>)StreamPersistenceMemoryDatabaseClient.InputKeepCallbackResponseParameter<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientInputKeepCallbackWriteResponseParameterDelegate { get { return (Func<ClientNode, int, ResponseParameterSerializer, MethodFlagsEnum, T, Task<KeepCallbackResponse<ResponseParameterSerializer>>>)StreamPersistenceMemoryDatabaseClient.InputKeepCallbackWriteResponseParameter<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientInputKeepCallbackCommandResponseParameterDelegate { get { return (Func<ClientNode, int, ResponseParameterSerializer, MethodFlagsEnum, T, Action<ResponseResult<ResponseParameterSerializer>, AutoCSer.Net.KeepCallbackCommand>, AutoCSer.Net.KeepCallbackCommand>)StreamPersistenceMemoryDatabaseClient.InputKeepCallbackCommandResponseParameter<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientInputKeepCallbackWriteCommandResponseParameterDelegate { get { return (Func<ClientNode, int, ResponseParameterSerializer, MethodFlagsEnum, T, Action<ResponseResult<ResponseParameterSerializer>, AutoCSer.Net.KeepCallbackCommand>, AutoCSer.Net.KeepCallbackCommand>)StreamPersistenceMemoryDatabaseClient.InputKeepCallbackWriteCommandResponseParameter<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputDelegate { get { return (Func<ClientNode, int, T, ResponseResultAwaiter>)StreamPersistenceMemoryDatabaseClient.SimpleSerializeCallInput<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputWriteDelegate { get { return (Func<ClientNode, int, T, ResponseResultAwaiter>)StreamPersistenceMemoryDatabaseClient.SimpleSerializeCallInputWrite<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputCommandDelegate { get { return (Func<ClientNode, int, T, Action<ResponseResult>, AutoCSer.Net.CallbackCommand>)StreamPersistenceMemoryDatabaseClient.SimpleSerializeCallInputCommand<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputWriteCommandDelegate { get { return (Func<ClientNode, int, T, Action<ResponseResult>, AutoCSer.Net.CallbackCommand>)StreamPersistenceMemoryDatabaseClient.SimpleSerializeCallInputWriteCommand<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSendOnlyDelegate { get { return (Func<ClientNode, int, T, SendOnlyCommand>)StreamPersistenceMemoryDatabaseClient.SendOnly<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSendOnlyWriteDelegate { get { return (Func<ClientNode, int, T, SendOnlyCommand>)StreamPersistenceMemoryDatabaseClient.SendOnlyWrite<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeSendOnlyDelegate { get { return (Func<ClientNode, int, T, SendOnlyCommand>)StreamPersistenceMemoryDatabaseClient.SimpleSerializeSendOnly<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeSendOnlyWriteDelegate { get { return (Func<ClientNode, int, T, SendOnlyCommand>)StreamPersistenceMemoryDatabaseClient.SimpleSerializeSendOnlyWrite<T>; } }
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
        internal override Delegate MethodParameterCreatorCreateCallInputMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T>)MethodParameterCreator.CreateCallInputMethodParameter<T>; } }
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
        internal override Delegate MethodParameterCreatorCreateCallInputOutputMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T>)MethodParameterCreator.CreateCallInputOutputMethodParameter<T>; } }
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
#if NetStandard21
        internal override Delegate MethodParameterCreatorCreateCallInputOutputCallbackMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T, CommandServerCallback<ResponseParameter>?>)MethodParameterCreator.CreateCallInputOutputCallbackMethodParameter<T>; } }
#else
        internal override Delegate MethodParameterCreatorCreateCallInputOutputCallbackMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T, CommandServerCallback<ResponseParameter>>)MethodParameterCreator.CreateCallInputOutputCallbackMethodParameter<T>; } }
#endif
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
        internal override Delegate MethodParameterCreatorCreateSendOnlyMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T>)MethodParameterCreator.CreateSendOnlyMethodParameter<T>; } }
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
#if NetStandard21
        internal override Delegate MethodParameterCreatorCreateInputKeepCallbackMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T, CommandServerKeepCallback<KeepCallbackResponseParameter>?>)MethodParameterCreator.CreateInputKeepCallbackMethodParameter<T>; } }
#else
        internal override Delegate MethodParameterCreatorCreateInputKeepCallbackMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T, CommandServerKeepCallback<KeepCallbackResponseParameter>>)MethodParameterCreator.CreateInputKeepCallbackMethodParameter<T>; } }
#endif
        /// <summary>
        /// Create the calling method and parameter information
        /// 创建调用方法与参数信息
        /// </summary>
#if NetStandard21
        internal override Delegate MethodParameterCreatorCreateInputTwoStageCallbackMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T, CommandServerCallback<ResponseParameter>?, CommandServerKeepCallback<KeepCallbackResponseParameter>?>)MethodParameterCreator.CreateInputTwoStageCallbackMethodParameter<T>; } }
#else
        internal override Delegate MethodParameterCreatorCreateInputTwoStageCallbackMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T, CommandServerCallback<ResponseParameter>, CommandServerKeepCallback<KeepCallbackResponseParameter>>)MethodParameterCreator.CreateInputTwoStageCallbackMethodParameter<T>; } }
#endif

        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate LocalServiceCallInputNodeCreateDelegate { get { return (Func<LocalClientNode, int, T, LocalServiceQueueNode<LocalResult>>)LocalServiceCallInputNode.Create<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        internal override Delegate LocalServiceCallbackInputNodeCreateDelegate { get { return (Action<LocalClientNode, int, T, Action<LocalResult>>)LocalServiceCallbackInputNode.Create<T>; } }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
#if NetStandard21
        internal override Delegate LocalServiceSendOnlyNodeCreateDelegate { get { return (Func<LocalClientNode, int, T, MethodParameter?>)LocalServiceSendOnlyNode.Create<T>; } }
#else
        internal override Delegate LocalServiceSendOnlyNodeCreateDelegate { get { return (Func<LocalClientNode, int, T, MethodParameter>)LocalServiceSendOnlyNode.Create<T>; } }
#endif
    }
}
