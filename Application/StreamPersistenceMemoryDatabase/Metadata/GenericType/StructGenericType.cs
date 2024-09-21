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
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSendOnlyDelegate { get; }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal abstract Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeSendOnlyDelegate { get; }
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
        protected static StructGenericType lastGenericType;
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static StructGenericType Get(Type type)
        {
            StructGenericType value = lastGenericType;
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
        internal override Delegate StreamPersistenceMemoryDatabaseClientCallInputDelegate { get { return (Func<ClientNode, int, T, Task<ResponseResult>>)StreamPersistenceMemoryDatabaseClient.CallInput<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeCallInputDelegate { get { return (Func<ClientNode, int, T, Task<ResponseResult>>)StreamPersistenceMemoryDatabaseClient.SimpleSerializeCallInput<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSendOnlyDelegate { get { return (Func<ClientNode, int, T, SendOnlyCommand>)StreamPersistenceMemoryDatabaseClient.SendOnly<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate StreamPersistenceMemoryDatabaseClientSimpleSerializeSendOnlyDelegate { get { return (Func<ClientNode, int, T, SendOnlyCommand>)StreamPersistenceMemoryDatabaseClient.SimpleSerializeSendOnly<T>; } }
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
        internal override Delegate MethodParameterCreatorCreateSendOnlyMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T>)MethodParameterCreator.CreateSendOnlyMethodParameter<T>; } }
        /// <summary>
        /// 创建调用方法与参数信息
        /// </summary>
        internal override Delegate MethodParameterCreatorCreateInputKeepCallbackMethodParameterDelegate { get { return (Action<MethodParameterCreator, int, T>)MethodParameterCreator.CreateInputKeepCallbackMethodParameter<T>; } }

        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate LocalServiceCallInputNodeCreateDelegate { get { return (Func<LocalClientNode, int, T, LocalServiceQueueNode<ResponseResult>>)LocalServiceCallInputNode.Create<T>; } }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        internal override Delegate LocalServiceSendOnlyNodeCreateDelegate { get { return (Func<LocalClientNode, int, T, MethodParameter>)LocalServiceSendOnlyNode.Create<T>; } }
    }
}
