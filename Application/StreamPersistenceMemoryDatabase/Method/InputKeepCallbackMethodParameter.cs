using AutoCSer.Extensions;
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
    public abstract class InputKeepCallbackMethodParameter : InputMethodParameter
    {
        /// <summary>
        /// Server node method information
        /// 服务端节点方法信息
        /// </summary>
        internal InputKeepCallbackMethod Method;
        /// <summary>
        /// 调用回调
        /// </summary>
#if NetStandard21
        internal CommandServerKeepCallback<KeepCallbackResponseParameter>? callback;
#else
        internal CommandServerKeepCallback<KeepCallbackResponseParameter> callback;
#endif
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
#if NetStandard21
        internal CallStateEnum InputKeepCallback([MaybeNull] ref CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
#else
        internal CallStateEnum InputKeepCallback(ref CommandServerKeepCallback<KeepCallbackResponseParameter> callback)
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
                            this.callback?.VirtualCallbackCancelKeep(value.Value.CreateKeepCallback());
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
                    Method.InputKeepCallback(this);
                }
                finally
                {
                    this.callback?.VirtualCallbackCancelKeep(new KeepCallbackResponseParameter(CallStateEnum.Unknown));
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
                        Method.InputKeepCallback(this);
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
                                    callback.VirtualCallbackCancelKeep(new KeepCallbackResponseParameter(CallStateEnum.IgnorePersistenceCallbackException));
                                }
                                else callback.VirtualCallbackCancelKeep(new KeepCallbackResponseParameter(CallStateEnum.PersistenceCallbackException));
                            }
                            finally
                            {
                                if (isPersistenceCallbackException) Node.SetPersistenceCallbackException();
                                if (rebuilder != null && !Node.Rebuilding) rebuilder.PushQueue(this);
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
                callback.VirtualCallbackCancelKeep(new KeepCallbackResponseParameter(state));
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
            var callback = this.callback;
            if (callback != null)
            {
                this.callback = null;
                if (values != null)
                {
                    if (!callback.Callback(KeepCallbackResponseParameter.CreateValues(values, Method.Flags))) return;
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
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumerableCallback<T>(InputKeepCallbackMethodParameter methodParameter, System.Collections.Generic.IEnumerable<T> values)
        {
            methodParameter.enumerableCallback(values);
        }

        /// <summary>
        /// 创建方法调用回调包装对象
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal MethodKeepCallback<T> CreateMethodKeepCallback<T>()
        {
            if (callback != null)
            {
                MethodKeepCallback<T> methodKeepCallback = new MethodKeepCallback<T>(callback, Method.Flags);
                callback = null;
                return methodKeepCallback;
            }
            return MethodKeepCallback<T>.NullCallback;
        }
    }
    /// <summary>
    /// 调用方法与参数信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class InputKeepCallbackMethodParameter<T> : InputKeepCallbackMethodParameter
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
        public InputKeepCallbackMethodParameter(ServerNode node, InputKeepCallbackMethod method) : base(node, method) { }
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methodIndex"></param>
        /// <param name="parameter"></param>
        public InputKeepCallbackMethodParameter(ServerNode node, int methodIndex, ref T parameter) : base(node, (InputKeepCallbackMethod)node.NodeCreator.Methods[methodIndex].notNull())
        {
            this.Parameter = parameter;
            callback = KeepCallbackResponseParameter.EmptyKeepCallback;
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
//                InputKeepCallbackMethodParameter<T> methodParameter = (InputKeepCallbackMethodParameter<T>)base.MemberwiseClone();
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
        public static T GetParameter(InputKeepCallbackMethodParameter<T> parameter)
        {
            return parameter.Parameter;
        }
    }
}
