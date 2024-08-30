using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 调用方法与参数信息
    /// </summary>
    internal abstract class InputKeepCallbackMethodParameter : InputMethodParameter
    {
        /// <summary>
        /// 服务端节点方法
        /// </summary>
        internal InputKeepCallbackMethod Method;
        /// <summary>
        /// 调用回调
        /// </summary>
        internal CommandServerKeepCallback<KeepCallbackResponseParameter> callback;
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        public InputKeepCallbackMethodParameter(ServerNode node, InputKeepCallbackMethod method) : base(node)
        {
            this.Method = method;
        }
        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        internal CallStateEnum InputKeepCallback(ref CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
        {
            CallStateEnum state = Node.CallState;
            if (state == CallStateEnum.Success)
            {
                if (Method.IsClientCall)
                {
                    this.callback = callback;
                    CommandServerSocketSessionObjectService service = Node.NodeCreator.Service;
                    if (Method.IsPersistence)
                    {
                        if (Node.IsPersistence && !service.IsMaster) return CallStateEnum.OnlyMaster;
                        if (Method.BeforePersistenceMethodIndex >= 0)
                        {
                            CallInputOutputMethod beforePersistenceMethod = (CallInputOutputMethod)Node.NodeCreator.Methods[Method.BeforePersistenceMethodIndex];
                            BeforePersistenceMethodParameter = CreateBeforePersistenceMethodParameter(beforePersistenceMethod);
                            service.CurrentMethodParameter = BeforePersistenceMethodParameter;
                            ValueResult<ResponseParameter> value = beforePersistenceMethod.CallOutputBeforePersistence(BeforePersistenceMethodParameter);
                            if (value.IsValue)
                            {
                                callback = null;
                                this.callback?.CallbackCancelKeep(value.Value.CreateKeepCallback());
                                return CallStateEnum.Success;
                            }
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
                        Method.InputKeepCallback(this);
                    }
                    finally
                    {
                        this.callback?.CallbackCancelKeep(new KeepCallbackResponseParameter(CallStateEnum.Unknown));
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
                        Method.InputKeepCallback(this);
                    }
                    finally
                    {
                        CommandServerKeepCallback<KeepCallbackResponseParameter> callback = this.callback;
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
                                if (Method.IsIgnorePersistenceCallbackException && !Node.IsPersistenceCallbackChanged)
                                {
                                    service.WritePersistenceCallbackExceptionPosition(persistenceCallbackExceptionPosition);
                                    rebuilder = null;
                                    isPersistenceCallbackException = false;
                                    callback.CallbackCancelKeep(new KeepCallbackResponseParameter(CallStateEnum.IgnorePersistenceCallbackException));
                                }
                                else callback.CallbackCancelKeep(new KeepCallbackResponseParameter(CallStateEnum.PersistenceCallbackException));
                            }
                            finally
                            {
                                if (isPersistenceCallbackException) Node.SetPersistenceCallbackException();
                                if (rebuilder != null && Node.IsRebuild && !Node.Rebuilding) rebuilder.PushQueue(this);
                            }
                        }
                    }
                }
                else Method.InputKeepCallback(this);
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
            CommandServerKeepCallback<KeepCallbackResponseParameter> callback = this.callback;
            if (callback != null)
            {
                this.callback = null;
                callback.CallbackCancelKeep(new KeepCallbackResponseParameter(state));
            }
            return LinkNext;
        }
        /// <summary>
        /// 创建持久化检查方法调用参数
        /// </summary>
        /// <param name="beforePersistenceMethod"></param>
        /// <returns></returns>
        internal abstract CallInputOutputMethodParameter CreateBeforePersistenceMethodParameter(CallInputOutputMethod beforePersistenceMethod);
        /// <summary>
        /// 枚举回调
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        private void enumerableCallback<T>(System.Collections.Generic.IEnumerable<T> values)
        {
            CommandServerKeepCallback<KeepCallbackResponseParameter> callback = this.callback;
            if (callback != null)
            {
                this.callback = null;
                if (values != null)
                {
                    if (!callback.Callback(KeepCallbackResponseParameter.CreateValues(values, Method.IsSimpleSerializeParamter))) return;
                }
                callback.CancelKeep();
            }
            else if (values != null)
            {
                foreach (T value in values) ;
            }
        }
        /// <summary>
        /// 枚举回调
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodParameter"></param>
        /// <param name="values"></param>
        internal static void EnumerableCallback<T>(InputKeepCallbackMethodParameter methodParameter, System.Collections.Generic.IEnumerable<T> values)
        {
            methodParameter.enumerableCallback(values);
        }
    }
    /// <summary>
    /// 调用方法与参数信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class InputKeepCallbackMethodParameter<T> : InputKeepCallbackMethodParameter
        where T : struct
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        private T parameter;
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        public InputKeepCallbackMethodParameter(ServerNode node, InputKeepCallbackMethod method) : base(node, method) { }
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methodIndex"></param>
        /// <param name="parameter"></param>
        public InputKeepCallbackMethodParameter(ServerNode node, int methodIndex, ref T parameter) : base(node, (InputKeepCallbackMethod)node.NodeCreator.Methods[methodIndex])
        {
            this.parameter = parameter;
            callback = KeepCallbackResponseParameter.EmptyKeepCallback;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        internal override void Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            if (Method.IsSimpleDeserializeParamter) deserializer.SimpleDeserialize(ref parameter);
            else deserializer.InternalIndependentDeserializeNotReference(ref parameter);
        }
        /// <summary>
        /// 输入参数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        internal override bool Deserialize(AutoCSer.BinaryDeserializer deserializer, ref SubArray<byte> buffer)
        {
            if (Method.IsSimpleDeserializeParamter) return deserializer.SimpleDeserialize(ref buffer, ref parameter);
            return deserializer.InternalIndependentDeserializeNotReference(ref buffer, ref parameter);
        }
        /// <summary>
        /// 持久化序列化
        /// </summary>
        /// <param name="serializer">序列化</param>
        /// <returns></returns>
        internal override MethodParameter PersistenceSerialize(AutoCSer.BinarySerializer serializer)
        {
            return PersistenceSerialize(serializer, Method, ref parameter);
        }
        /// <summary>
        /// 创建持久化检查方法调用参数
        /// </summary>
        /// <param name="beforePersistenceMethod"></param>
        /// <returns></returns>
        internal override CallInputOutputMethodParameter CreateBeforePersistenceMethodParameter(CallInputOutputMethod beforePersistenceMethod)
        {
            return new BeforePersistenceMethodParameter<T>(Node, beforePersistenceMethod, parameter);
        }
        /// <summary>
        /// 获取输入参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static T GetParameter(InputKeepCallbackMethodParameter<T> parameter)
        {
            return parameter.parameter;
        }
    }
}
