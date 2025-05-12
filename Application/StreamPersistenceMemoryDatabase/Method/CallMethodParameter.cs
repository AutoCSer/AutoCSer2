using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 调用方法与参数信息
    /// </summary>
    public class CallMethodParameter : MethodParameter
    {
        /// <summary>
        /// 服务端节点方法
        /// </summary>
        private readonly CallMethod method;
        /// <summary>
        /// 调用回调
        /// </summary>
#if NetStandard21
        private CommandServerCallback<CallStateEnum>? callback;
#else
        private CommandServerCallback<CallStateEnum> callback;
#endif
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        /// <param name="callback"></param>
        internal CallMethodParameter(ServerNode node, CallMethod method, CommandServerCallback<CallStateEnum> callback) : base(node)
        {
            this.method = method;
            this.callback = callback;
        }
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methodIndex"></param>
        internal CallMethodParameter(ServerNode node, int methodIndex) : base(node)
        {
            method = (CallMethod)node.NodeCreator.Methods[methodIndex].notNull();
            callback = EmptyCommandServerCallback<CallStateEnum>.Default;
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
                        method.Call(Node, ref callback);
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
                                    callback.SynchronousCallback(CallStateEnum.IgnorePersistenceCallbackException);
                                }
                                else callback.SynchronousCallback(CallStateEnum.PersistenceCallbackException);
                            }
                            finally
                            {
                                if (isPersistenceCallbackException) Node.SetPersistenceCallbackException();
                                if (rebuilder != null && !Node.Rebuilding) rebuilder.PushQueue(this);
                            }
                        }
                    }
                }
                else method.Call(Node, ref callback);
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
                callback.Callback(state);
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
