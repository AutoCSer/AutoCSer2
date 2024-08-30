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
        private CommandServerCallback<CallStateEnum> callback;
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
            method = (CallMethod)node.NodeCreator.Methods[methodIndex];
            callback = EmptyCommandServerCallback<CallStateEnum>.Default;
        }
        /// <summary>
        /// 持久化回调
        /// </summary>
        /// <returns></returns>
        internal override MethodParameter PersistenceCallback()
        {
            CommandServerCallback<CallStateEnum> callback = this.callback;
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
                            bool isPersistenceCallbackException = true;
                            try
                            {
                                if (method.IsIgnorePersistenceCallbackException && !Node.IsPersistenceCallbackChanged)
                                {
                                    service.WritePersistenceCallbackExceptionPosition(persistenceCallbackExceptionPosition);
                                    rebuilder = null;
                                    isPersistenceCallbackException = false;
                                    callback.Callback(CallStateEnum.IgnorePersistenceCallbackException);
                                }
                                else callback.Callback(CallStateEnum.PersistenceCallbackException);
                            }
                            finally
                            {
                                if (isPersistenceCallbackException) Node.SetPersistenceCallbackException();
                                if (rebuilder != null && Node.IsRebuild && !Node.Rebuilding) rebuilder.PushQueue(this);
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
        internal override MethodParameter PersistenceCallback(CallStateEnum state)
        {
            CommandServerCallback<CallStateEnum> callback = this.callback;
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
        internal override MethodParameter PersistenceSerialize(AutoCSer.BinarySerializer serializer)
        {
            return persistenceSerialize(serializer, method.Index);
        }
    }
}
