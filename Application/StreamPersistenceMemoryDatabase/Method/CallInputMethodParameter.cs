using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 调用方法与参数信息
    /// </summary>
    internal abstract class CallInputMethodParameter : InputMethodParameter
    {
        /// <summary>
        /// 服务端节点方法
        /// </summary>
        protected readonly CallInputMethod method;
        /// <summary>
        /// 调用回调
        /// </summary>
        protected CommandServerCallback<CallStateEnum> callback;
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        public CallInputMethodParameter(ServerNode node, CallInputMethod method) : base(node)
        {
            this.method = method;
        }
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal CallStateEnum CallInput(ref CommandServerCallback<CallStateEnum> callback)
        {
            CallStateEnum state = Node.CallState;
            if (state == CallStateEnum.Success)
            {
                if (method.IsClientCall)
                {
                    this.callback = callback;
                    CommandServerSocketSessionObjectService service = Node.NodeCreator.Service;
                    if (method.IsPersistence)
                    {
                        if (Node.IsPersistence && !service.IsMaster) return CallStateEnum.OnlyMaster;
                        if (method.BeforePersistenceMethodIndex >= 0)
                        {
                            CallInputOutputMethod beforePersistenceMethod = (CallInputOutputMethod)Node.NodeCreator.Methods[method.BeforePersistenceMethodIndex];
                            BeforePersistenceMethodParameter = CreateBeforePersistenceMethodParameter(beforePersistenceMethod);
                            service.CurrentMethodParameter = BeforePersistenceMethodParameter;
                            if (!beforePersistenceMethod.CallBeforePersistence(BeforePersistenceMethodParameter)) return CallStateEnum.Success;
                        }
                        if (Node.IsPersistence)
                        {
                            service.PushPersistenceMethodParameter(this, ref callback);
                            return CallStateEnum.Success;
                        }
                    }
                    callback = null;
                    try
                    {
                        service.SetCurrentMethodParameter(this);
                        method.CallInput(this);
                    }
                    finally
                    {
                        this.callback?.SynchronousCallback(CallStateEnum.Unknown);
                    }
                    return CallStateEnum.Success;
                }
                return CallStateEnum.NotAllowClientCall;
            }
            return state;
        }
        /// <summary>
        /// 持久化回调
        /// </summary>
        /// <returns></returns>
        internal override MethodParameter PersistenceCallback()
        {
            if (this.callback != null)
            {
                if (Node.IsPersistence)
                {
                    try
                    {
                        Node.IsPersistenceCallbackChanged = false;
                        method.CallInput(this);
                    }
                    finally
                    {
                        CommandServerCallback<CallStateEnum> callback = this.callback;
                        CommandServerSocketSessionObjectService service = Node.NodeCreator.Service;
                        PersistenceRebuilder rebuilder = service.Rebuilder;
                        if (callback == null)
                        {
                            if (rebuilder != null && Node.IsRebuild && !Node.Rebuilding)
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
                                if (method.IsIgnorePersistenceCallbackException && !Node.IsPersistenceCallbackChanged)
                                {
                                    service.WritePersistenceCallbackExceptionPosition(persistenceCallbackExceptionPosition);
                                    rebuilder = null;
                                    isPersistenceCallbackException = false;
                                    callback.SynchronousCallback(CallStateEnum.IgnorePersistenceCallbackException);
                                }
                                else callback.SynchronousCallback(CallStateEnum.PersistenceCallbackException);
                            }
                            finally
                            {
                                if (isPersistenceCallbackException) Node.SetPersistenceCallbackException();
                                if (rebuilder != null && Node.IsRebuild && !Node.Rebuilding) rebuilder.PushQueue(this);
                            }
                        }
                    }
                }
                else method.CallInput(this);
            }
            return LinkNext;
        }
        /// <summary>
        /// 持久化异常回调
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        internal override MethodParameter PersistenceCallback(CallStateEnum state)
        {
            CommandServerCallback<CallStateEnum> callback = this.callback;
            if (callback != null)
            {
                this.callback = null;
                callback.SynchronousCallback(state);
            }
            return LinkNext;
        }
        /// <summary>
        /// 调用回调
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SuccessCallback()
        {
            CommandServerCallback<CallStateEnum> callback = this.callback;
            if (callback != null)
            {
                this.callback = null;
                callback.SynchronousCallback(CallStateEnum.Success);
            }
        }
        /// <summary>
        /// 调用回调
        /// </summary>
        /// <param name="methodParameter"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Callback(CallInputMethodParameter methodParameter)
        {
            methodParameter.SuccessCallback();
        }
        /// <summary>
        /// 创建持久化检查方法调用参数
        /// </summary>
        /// <param name="beforePersistenceMethod"></param>
        /// <returns></returns>
        internal abstract CallInputOutputMethodParameter CreateBeforePersistenceMethodParameter(CallInputOutputMethod beforePersistenceMethod);
    }
    /// <summary>
    /// 调用方法与参数信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class CallInputMethodParameter<T> : CallInputMethodParameter
        where T : struct
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        internal T Parameter;
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        public CallInputMethodParameter(ServerNode node, CallInputMethod method) : base(node, method) { }
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methodIndex"></param>
        /// <param name="parameter"></param>
        public CallInputMethodParameter(ServerNode node, int methodIndex, ref T parameter) : base(node, (CallInputMethod)node.NodeCreator.Methods[methodIndex])
        {
            this.Parameter = parameter;
            callback = EmptyCommandServerCallback<CallStateEnum>.Default;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal override void Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            if (method.IsSimpleDeserializeParamter) deserializer.SimpleDeserialize(ref Parameter);
            else deserializer.InternalIndependentDeserializeNotReference(ref Parameter);
        }
        /// <summary>
        /// 输入参数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal override bool Deserialize(AutoCSer.BinaryDeserializer deserializer, ref SubArray<byte> buffer)
        {
            if (method.IsSimpleDeserializeParamter) return deserializer.SimpleDeserialize(ref buffer, ref Parameter);
            return deserializer.InternalIndependentDeserializeNotReference(ref buffer, ref Parameter);
        }
        /// <summary>
        /// 持久化序列化
        /// </summary>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        internal override MethodParameter PersistenceSerialize(AutoCSer.BinarySerializer serializer)
        {
            return PersistenceSerialize(serializer, method, ref Parameter);
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
        internal static T GetParameter(CallInputMethodParameter<T> parameter)
        {
            return parameter.Parameter;
        }
    }
}
