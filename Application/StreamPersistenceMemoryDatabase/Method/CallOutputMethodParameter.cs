using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 调用方法与参数信息
    /// </summary>
    public class CallOutputMethodParameter : MethodParameter
    {
        /// <summary>
        /// Server node method information
        /// 服务端节点方法信息
        /// </summary>
        private readonly CallOutputMethod method;
        /// <summary>
        /// 调用回调
        /// </summary>
#if NetStandard21
        private CommandServerCallback<ResponseParameter>? callback;
#else
        private CommandServerCallback<ResponseParameter> callback;
#endif
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        /// <param name="callback"></param>
        internal CallOutputMethodParameter(ServerNode node, CallOutputMethod method, CommandServerCallback<ResponseParameter> callback) : base(node)
        {
            this.method = method;
            this.callback = callback;
        }
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
#if NetStandard21
        internal CallOutputMethodParameter(ServerNode node, int methodIndex, CommandServerCallback<ResponseParameter>? callback) : base(node)
#else
        internal CallOutputMethodParameter(ServerNode node, int methodIndex, CommandServerCallback<ResponseParameter> callback) : base(node)
#endif
        {
            this.method = (CallOutputMethod)node.NodeCreator.Methods[methodIndex].notNull();
            this.callback = callback ?? EmptyCommandServerCallback<ResponseParameter>.Default;
        }
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methodIndex"></param>
        internal CallOutputMethodParameter(ServerNode node, int methodIndex) : base(node)
        {
            this.method = (CallOutputMethod)node.NodeCreator.Methods[methodIndex].notNull();
            callback = EmptyCommandServerCallback<ResponseParameter>.Default;
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
            var callback = this.callback;
            if (callback != null)
            {
                this.callback = null;
                if (Node.IsPersistence)
                {
                    try
                    {
                        Node.IsPersistenceCallbackChanged = false;
                        method.CallOutput(Node, ref callback);
                    }
                    finally
                    {
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
                            bool isPersistenceCallbackException = true;
                            try
                            {
                                if (method.IsIgnorePersistenceCallbackException && !Node.IsPersistenceCallbackChanged)
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
                else method.CallOutput(Node, ref callback);
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
            return persistenceSerialize(serializer, method.Index);
        }
    }
}
