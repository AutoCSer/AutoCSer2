﻿using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 调用方法与参数信息
    /// </summary>
    public abstract class CallInputOutputMethodParameter : InputMethodParameter
    {
        /// <summary>
        /// Server node method information
        /// 服务端节点方法信息
        /// </summary>
        internal CallInputOutputMethod Method;
        /// <summary>
        /// 调用回调
        /// </summary>
#if NetStandard21
        internal CommandServerCallback<ResponseParameter>? callback;
#else
        internal CommandServerCallback<ResponseParameter> callback;
#endif
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        public CallInputOutputMethodParameter(ServerNode node, CallInputOutputMethod method) : base(node)
        {
            this.Method = method;
        }
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
#if NetStandard21
        internal CallStateEnum CallInputOutput([MaybeNull] ref CommandServerCallback<ResponseParameter> callback)
#else
        internal CallStateEnum CallInputOutput(ref CommandServerCallback<ResponseParameter> callback)
#endif
        {
            if (Method.IsClientCall)
            {
                this.callback = callback;
                StreamPersistenceMemoryDatabaseServiceBase service = Node.NodeCreator.Service;
                if (Method.IsPersistence)
                {
                    if (Node.CallState != CallStateEnum.Success) return Node.CallState;
                    if (Node.IsPersistence && !service.IsMaster) return CallStateEnum.OnlyMaster;
                    if (Method.BeforePersistenceMethodIndex >= 0)
                    {
                        CallInputOutputMethod beforePersistenceMethod = (CallInputOutputMethod)Node.NodeCreator.Methods[Method.BeforePersistenceMethodIndex].notNull();
                        BeforePersistenceMethodParameter = CreateBeforePersistenceMethodParameter(beforePersistenceMethod);
                        service.CurrentMethodParameter = BeforePersistenceMethodParameter;
                        ValueResult<ResponseParameter> value = beforePersistenceMethod.CallOutputBeforePersistence(BeforePersistenceMethodParameter);
                        if (value.IsValue)
                        {
                            callback = null;
                            this.callback?.SynchronousCallback(value.Value);
                            return CallStateEnum.Success;
                        }
                    }
                    if (Node.IsPersistence) service.PushPersistenceMethodParameter(this, ref callback);
                    else
                    {
                        service.CommandServerCallQueue.AppendWriteOnly(new MethodParameterPersistenceCallback(this));
                        callback = null;
                    }
                    return CallStateEnum.Success;
                }
                callback = null;
                try
                {
                    service.SetCurrentMethodParameter(this);
                    Method.CallInputOutput(this);
                }
                finally
                {
                    this.callback?.SynchronousCallback(ResponseParameter.CallStates[(byte)CallStateEnum.Unknown]);
                }
                return CallStateEnum.Success;
            }
            return CallStateEnum.NotAllowClientCall;
        }
        /// <summary>
        /// 持久化回调
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal override MethodParameter? PersistenceCallback()
#else
        internal override MethodParameter PersistenceCallback()
#endif
        {
            if (this.callback != null)
            {
                if (Node.IsPersistence)
                {
                    try
                    {
                        Node.IsPersistenceCallbackChanged = false;
                        Method.CallInputOutput(this);
                    }
                    finally
                    {
                        var callback = this.callback;
                        StreamPersistenceMemoryDatabaseServiceBase service = Node.NodeCreator.Service;
                        var rebuilder = service.Rebuilder;
                        if (callback == null)
                        {
                            if (rebuilder != null && !Node.Rebuilding)
                            {
                                IsPersistenceCallback = true;
                                rebuilder.PushQueue(this);
                            }
                        }
                        else
                        {
                            this.callback = null;
                            bool isPersistenceCallbackException = true;
                            try
                            {
                                if (Method.IsIgnorePersistenceCallbackException && !Node.IsPersistenceCallbackChanged)
                                {
                                    service.WritePersistenceCallbackExceptionPosition(persistenceCallbackExceptionPosition);
                                    rebuilder = null;
                                    isPersistenceCallbackException = false;
                                    callback.SynchronousCallback(ResponseParameter.CallStates[(byte)CallStateEnum.IgnorePersistenceCallbackException]);
                                }
                                else callback.SynchronousCallback(ResponseParameter.CallStates[(byte)CallStateEnum.PersistenceCallbackException]);
                            }
                            finally
                            {
                                if (isPersistenceCallbackException) Node.SetPersistenceCallbackException();
                                if (rebuilder != null && !Node.Rebuilding) rebuilder.PushQueue(this);
                            }
                        }
                    }
                }
                else Method.CallInputOutput(this);
            }
            return LinkNext;
        }
        /// <summary>
        /// 持久化异常回调
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
#if NetStandard21
        internal override MethodParameter? PersistenceCallback(CallStateEnum state)
#else
        internal override MethodParameter PersistenceCallback(CallStateEnum state)
#endif
        {
            var callback = this.callback;
            if (callback != null)
            {
                this.callback = null;
                callback.Callback(ResponseParameter.CallStates[(byte)state]);
            }
            return LinkNext;
        }
        /// <summary>
        /// 调用回调
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        internal void SuccessCallback<T>(T value)
        {
            var callback = this.callback;
            if (callback != null)
            {
                this.callback = null;
                bool isCallback = false;
                try
                {
                    ResponseParameter responseParameter = ResponseParameter.Create(value, Method.Flags);
                    isCallback = true;
                    callback.SynchronousCallback(responseParameter);
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
                finally
                {
                    if (!isCallback) callback.Socket.DisposeSocket();
                }
            }
        }
        /// <summary>
        /// 调用回调
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodParameter"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Callback<T>(CallInputOutputMethodParameter methodParameter, T value)
        {
            methodParameter.SuccessCallback(value);
        }
        /// <summary>
        /// 调用回调
        /// </summary>
        /// <param name="value"></param>
        internal void SuccessCallback(ResponseParameter value)
        {
            var callback = this.callback;
            if (callback != null)
            {
                this.callback = null;
                try
                {
                    callback.SynchronousCallback(value);
                }
                catch (Exception exception)
                {
                    AutoCSer.LogHelper.ExceptionIgnoreException(exception);
                }
            }
        }
        /// <summary>
        /// 调用回调
        /// </summary>
        /// <param name="methodParameter"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void CallbackResponseParameter(CallInputOutputMethodParameter methodParameter, ResponseParameter value)
        {
            methodParameter.SuccessCallback(value);
        }
        /// <summary>
        /// 创建持久化检查方法调用参数
        /// </summary>
        /// <param name="beforePersistenceMethod"></param>
        /// <returns></returns>
        internal abstract CallInputOutputMethodParameter CreateBeforePersistenceMethodParameter(CallInputOutputMethod beforePersistenceMethod);
        /// <summary>
        /// 获取持久化检查方法返回值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodParameter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ValueResult<ResponseParameter> GetBeforePersistenceResponseParameter<T>(CallInputOutputMethodParameter methodParameter, ValueResult<T> value)
        {
            if (value.IsValue) return ResponseParameter.Create(value.Value, methodParameter.Method.Flags);
            return default(ValueResult<ResponseParameter>);
        }
        ///// <summary>
        ///// 从其它调用方法参数中获取调用回调
        ///// </summary>
        ///// <param name="callbackParameter"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal void GetCallback(CallInputOutputMethodParameter callbackParameter)
        //{
        //    callback = callbackParameter.callback;
        //    callbackParameter.callback = null;
        //}
        /// <summary>
        /// 创建方法调用回调包装对象
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal MethodCallback<T> CreateMethodCallback<T>()
        {
            if (callback != null)
            {
                MethodCallback<T> methodCallback = new MethodCallback<T>(callback, Method.Flags);
                callback = null;
                return methodCallback;
            }
            return MethodCallback<T>.NullCallback;
        }
    }
    /// <summary>
    /// 调用方法与参数信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CallInputOutputMethodParameter<T> : CallInputOutputMethodParameter
        where T : struct
    {
        /// <summary>
        /// Input parameters
        /// </summary>
        internal T Parameter;
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        internal CallInputOutputMethodParameter(ServerNode node, CallInputOutputMethod method) : base(node, method) { }
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methodIndex"></param>
        /// <param name="parameter"></param>
        internal CallInputOutputMethodParameter(ServerNode node, int methodIndex, ref T parameter) : base(node, (CallInputOutputMethod)node.NodeCreator.Methods[methodIndex].notNull()) 
        {
            this.Parameter = parameter;
            callback = EmptyCommandServerCallback<ResponseParameter>.Default;
        }
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methodIndex"></param>
        /// <param name="parameter"></param>
        /// <param name="callback"></param>
#if NetStandard21
        internal CallInputOutputMethodParameter(ServerNode node, int methodIndex, ref T parameter, CommandServerCallback<ResponseParameter>? callback)
#else
        internal CallInputOutputMethodParameter(ServerNode node, int methodIndex, ref T parameter, CommandServerCallback<ResponseParameter> callback) 
#endif
            : base(node, (CallInputOutputMethod)node.NodeCreator.Methods[methodIndex].notNull())
        {
            this.Parameter = parameter;
            this.callback = callback ?? EmptyCommandServerCallback<ResponseParameter>.Default;
        }
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        /// <param name="parameter"></param>
        internal CallInputOutputMethodParameter(ServerNode node, CallInputOutputMethod method, T parameter) : base(node, method) 
        {
            this.Parameter = parameter;
        }
//        /// <summary>
//        /// 复制调用方法与参数信息
//        /// </summary>
//        /// <param name="index"></param>
//        /// <param name="methodIndex"></param>
//        /// <returns></returns>
//#if NetStandard21
//        internal override InputMethodParameter? Clone(NodeIndex index, int methodIndex)
//#else
//        internal override InputMethodParameter Clone(NodeIndex index, int methodIndex)
//#endif
//        {
//            if (Method.Index == methodIndex && index.Equals(Node.Index))
//            {
//                CallInputOutputMethodParameter<T> methodParameter = (CallInputOutputMethodParameter<T>)base.MemberwiseClone();
//                methodParameter.clearClone();
//                methodParameter.Parameter = default(T);
//                return methodParameter;
//            }
//            return null;
//        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal override void Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            Deserialize(deserializer, Method, ref Parameter);
        }
        /// <summary>
        /// 输入参数反序列化（初始化加载持久化数据）
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal override bool Deserialize(AutoCSer.BinaryDeserializer deserializer, ref SubArray<byte> buffer)
        {
            return Deserialize(deserializer, ref buffer, Method, ref Parameter);
        }
        /// <summary>
        /// 持久化序列化
        /// </summary>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
#if NetStandard21
        internal override MethodParameter? PersistenceSerialize(AutoCSer.BinarySerializer serializer)
#else
        internal override MethodParameter PersistenceSerialize(AutoCSer.BinarySerializer serializer)
#endif
        {
            return PersistenceSerialize(serializer, Method, ref Parameter);
        }
        /// <summary>
        /// 创建持久化检查方法调用参数
        /// </summary>
        /// <param name="beforePersistenceMethod"></param>
        /// <returns></returns>
        internal override CallInputOutputMethodParameter CreateBeforePersistenceMethodParameter(CallInputOutputMethod beforePersistenceMethod)
        {
            return new BeforePersistenceMethodParameter<T>(Node, beforePersistenceMethod, Parameter);
        }
        /// <summary>
        /// 获取输入参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T GetParameter(CallInputOutputMethodParameter<T> parameter)
        {
            return parameter.Parameter;
        }
    }
}
