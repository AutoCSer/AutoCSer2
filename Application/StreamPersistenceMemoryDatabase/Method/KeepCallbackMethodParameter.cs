using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 调用方法与参数信息
    /// </summary>
    public class KeepCallbackMethodParameter : MethodParameter
    {
        /// <summary>
        /// 服务端节点方法
        /// </summary>
        private readonly KeepCallbackMethod method;
        /// <summary>
        /// 调用回调
        /// </summary>
        private CommandServerKeepCallback<KeepCallbackResponseParameter> callback;
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        /// <param name="callback"></param>
        internal KeepCallbackMethodParameter(ServerNode node, KeepCallbackMethod method, CommandServerKeepCallback<KeepCallbackResponseParameter> callback) : base(node)
        {
            this.method = method;
            this.callback = callback;
        }
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methodIndex"></param>
        internal KeepCallbackMethodParameter(ServerNode node, int methodIndex) : base(node)
        {
            this.method = (KeepCallbackMethod)node.NodeCreator.Methods[methodIndex];
            callback = KeepCallbackResponseParameter.EmptyKeepCallback;
        }
        /// <summary>
        /// 持久化回调
        /// </summary>
        /// <returns></returns>
        internal override MethodParameter PersistenceCallback()
        {
            CommandServerKeepCallback<KeepCallbackResponseParameter> callback = this.callback;
            if (callback != null)
            {
                this.callback = null;
                if (Node.IsPersistence)
                {
                    try
                    {
                        Node.IsPersistenceCallbackChanged = false;
                        method.KeepCallback(Node, ref callback);
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
                else method.KeepCallback(Node, ref callback);
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
