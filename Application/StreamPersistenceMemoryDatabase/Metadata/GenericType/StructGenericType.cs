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
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallInputDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallInputWriteDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallInputCommandDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallInputWriteCommandDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallInputOutputResponseParameterDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallInputOutputWriteResponseParameterDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallInputOutputCommandResponseParameterDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientCallInputOutputWriteCommandResponseParameterDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientInputKeepCallbackResponseParameterDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientInputKeepCallbackWriteResponseParameterDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientInputKeepCallbackCommandResponseParameterDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientInputKeepCallbackWriteCommandResponseParameterDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputWriteDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputCommandDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputWriteCommandDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSendOnlyDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSendOnlyWriteDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeSendOnlyDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeSendOnlyWriteDelegate { get; }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        internal abstract Delegate MethodParameterCreatorCreateCallInputMethodParameterDelegate { get; }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        internal abstract Delegate MethodParameterCreatorCreateCallInputOutputMethodParameterDelegate { get; }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        internal abstract Delegate MethodParameterCreatorCreateCallInputOutputCallbackMethodParameterDelegate { get; }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        internal abstract Delegate MethodParameterCreatorCreateSendOnlyMethodParameterDelegate { get; }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        internal abstract Delegate MethodParameterCreatorCreateInputKeepCallbackMethodParameterDelegate { get; }

        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate LocalServiceCallInputNodeCreateDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate LocalServiceCallbackInputNodeCreateDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate LocalServiceSendOnlyNodeCreateDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
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
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputDelegate { get { return (Func<ClientNode, int, T, ResponseResultAwaiter>)StreamPersistenceMemoryDatabaseClient.CallInput<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputWriteDelegate { get { return (Func<ClientNode, int, T, ResponseResultAwaiter>)StreamPersistenceMemoryDatabaseClient.CallInputWrite<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputCommandDelegate { get { return (Func<ClientNode, int, T, Action<ResponseResult>, AutoCSer.Net.CallbackCommand>)StreamPersistenceMemoryDatabaseClient.CallInputCommand<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputWriteCommandDelegate { get { return (Func<ClientNode, int, T, Action<ResponseResult>, AutoCSer.Net.CallbackCommand>)StreamPersistenceMemoryDatabaseClient.CallInputWriteCommand<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputOutputResponseParameterDelegate { get { return (Func<ClientNode, int, ResponseParameter, MethodFlagsEnum, T, ResponseParameterAwaiter<ResponseParameter>>)StreamPersistenceMemoryDatabaseClient.CallInputOutputResponseParameter<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputOutputWriteResponseParameterDelegate { get { return (Func<ClientNode, int, ResponseParameter, MethodFlagsEnum, T, ResponseParameterAwaiter<ResponseParameter>>)StreamPersistenceMemoryDatabaseClient.CallInputOutputWriteResponseParameter<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputOutputCommandResponseParameterDelegate { get { return (Func<ClientNode, int, ResponseParameter, MethodFlagsEnum, T, Action<ResponseResult<ResponseParameter>>, AutoCSer.Net.CallbackCommand>)StreamPersistenceMemoryDatabaseClient.CallInputOutputCommandResponseParameter<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputOutputWriteCommandResponseParameterDelegate { get { return (Func<ClientNode, int, ResponseParameter, MethodFlagsEnum, T, Action<ResponseResult<ResponseParameter>>, AutoCSer.Net.CallbackCommand>)StreamPersistenceMemoryDatabaseClient.CallInputOutputWriteCommandResponseParameter<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientInputKeepCallbackResponseParameterDelegate { get { return (Func<ClientNode, int, ResponseParameterSerializer, MethodFlagsEnum, T, Task<KeepCallbackResponse<ResponseParameterSerializer>>>)StreamPersistenceMemoryDatabaseClient.InputKeepCallbackResponseParameter<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientInputKeepCallbackWriteResponseParameterDelegate { get { return (Func<ClientNode, int, ResponseParameterSerializer, MethodFlagsEnum, T, Task<KeepCallbackResponse<ResponseParameterSerializer>>>)StreamPersistenceMemoryDatabaseClient.InputKeepCallbackWriteResponseParameter<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientInputKeepCallbackCommandResponseParameterDelegate { get { return (Func<ClientNode, int, ResponseParameterSerializer, MethodFlagsEnum, T, Action<ResponseResult<ResponseParameterSerializer>, AutoCSer.Net.KeepCallbackCommand>, AutoCSer.Net.KeepCallbackCommand>)StreamPersistenceMemoryDatabaseClient.InputKeepCallbackCommandResponseParameter<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientInputKeepCallbackWriteCommandResponseParameterDelegate { get { return (Func<ClientNode, int, ResponseParameterSerializer, MethodFlagsEnum, T, Action<ResponseResult<ResponseParameterSerializer>, AutoCSer.Net.KeepCallbackCommand>, AutoCSer.Net.KeepCallbackCommand>)StreamPersistenceMemoryDatabaseClient.InputKeepCallbackWriteCommandResponseParameter<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputDelegate { get { return (Func<ClientNode, int, T, ResponseResultAwaiter>)StreamPersistenceMemoryDatabaseClient.SimpleSerializeCallInput<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputWriteDelegate { get { return (Func<ClientNode, int, T, ResponseResultAwaiter>)StreamPersistenceMemoryDatabaseClient.SimpleSerializeCallInputWrite<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputCommandDelegate { get { return (Func<ClientNode, int, T, Action<ResponseResult>, AutoCSer.Net.CallbackCommand>)StreamPersistenceMemoryDatabaseClient.SimpleSerializeCallInputCommand<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputWriteCommandDelegate { get { return (Func<ClientNode, int, T, Action<ResponseResult>, AutoCSer.Net.CallbackCommand>)StreamPersistenceMemoryDatabaseClient.SimpleSerializeCallInputWriteCommand<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSendOnlyDelegate { get { return (Func<ClientNode, int, T, SendOnlyCommand>)StreamPersistenceMemoryDatabaseClient.SendOnly<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSendOnlyWriteDelegate { get { return (Func<ClientNode, int, T, SendOnlyCommand>)StreamPersistenceMemoryDatabaseClient.SendOnlyWrite<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeSendOnlyDelegate { get { return (Func<ClientNode, int, T, SendOnlyCommand>)StreamPersistenceMemoryDatabaseClient.SimpleSerializeSendOnly<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeSendOnlyWriteDelegate { get { return (Func<ClientNode, int, T, SendOnlyCommand>)StreamPersistenceMemoryDatabaseClient.SimpleSerializeSendOnlyWrite<T>; } }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        internal override Delegate MethodParameterCreatorCreateCallInputMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T>)MethodParameterCreator.CreateCallInputMethodParameter<T>; } }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        internal override Delegate MethodParameterCreatorCreateCallInputOutputMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T>)MethodParameterCreator.CreateCallInputOutputMethodParameter<T>; } }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
#if NetStandard21
        internal override Delegate MethodParameterCreatorCreateCallInputOutputCallbackMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T, CommandServerCallback<ResponseParameter>?>)MethodParameterCreator.CreateCallInputOutputCallbackMethodParameter<T>; } }
#else
        internal override Delegate MethodParameterCreatorCreateCallInputOutputCallbackMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T, CommandServerCallback<ResponseParameter>>)MethodParameterCreator.CreateCallInputOutputCallbackMethodParameter<T>; } }
#endif
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        internal override Delegate MethodParameterCreatorCreateSendOnlyMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T>)MethodParameterCreator.CreateSendOnlyMethodParameter<T>; } }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        internal override Delegate MethodParameterCreatorCreateInputKeepCallbackMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T>)MethodParameterCreator.CreateInputKeepCallbackMethodParameter<T>; } }

        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate LocalServiceCallInputNodeCreateDelegate { get { return (Func<LocalClientNode, int, T, LocalServiceQueueNode<LocalResult>>)LocalServiceCallInputNode.Create<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate LocalServiceCallbackInputNodeCreateDelegate { get { return (Action<LocalClientNode, int, T, Action<LocalResult>>)LocalServiceCallbackInputNode.Create<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
#if NetStandard21
        internal override Delegate LocalServiceSendOnlyNodeCreateDelegate { get { return (Func<LocalClientNode, int, T, MethodParameter?>)LocalServiceSendOnlyNode.Create<T>; } }
#else
        internal override Delegate LocalServiceSendOnlyNodeCreateDelegate { get { return (Func<LocalClientNode, int, T, MethodParameter>)LocalServiceSendOnlyNode.Create<T>; } }
#endif
    }
}
