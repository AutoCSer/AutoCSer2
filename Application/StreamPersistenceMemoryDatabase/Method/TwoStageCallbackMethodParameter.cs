using AutoCSer.Extensions;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 调用方法与参数信息
    /// </summary>
    public class TwoStageCallbackMethodParameter : MethodParameter
    {
        /// <summary>
        /// Server node method information
        /// 服务端节点方法信息
        /// </summary>
        private readonly TwoStageCallbackMethod method;
        /// <summary>
        /// 调用回调
        /// </summary>
#if NetStandard21
        private CommandServerCallback<ResponseParameter>? callback;
#else
        private CommandServerCallback<ResponseParameter> callback;
#endif
        /// <summary>
        /// 调用回调
        /// </summary>
#if NetStandard21
        private CommandServerKeepCallback<KeepCallbackResponseParameter>? keepCallback;
#else
        private CommandServerKeepCallback<KeepCallbackResponseParameter> keepCallback;
#endif
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method"></param>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
        internal TwoStageCallbackMethodParameter(ServerNode node, TwoStageCallbackMethod method, CommandServerCallback<ResponseParameter> callback, CommandServerKeepCallback<KeepCallbackResponseParameter> keepCallback) : base(node)
        {
            this.method = method;
            this.callback = callback;
            this.keepCallback = keepCallback;
        }
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        /// <param name="node"></param>
        /// <param name="methodIndex"></param>
        /// <param name="callback"></param>
        /// <param name="keepCallback"></param>
#if NetStandard21
        internal TwoStageCallbackMethodParameter(ServerNode node, int methodIndex, CommandServerCallback<ResponseParameter>? callback, CommandServerKeepCallback<KeepCallbackResponseParameter>? keepCallback) : base(node)
#else
        internal TwoStageCallbackMethodParameter(ServerNode node, int methodIndex, CommandServerCallback<ResponseParameter> callback, CommandServerKeepCallback<KeepCallbackResponseParameter> keepCallback) : base(node)
#endif
        {
            this.method = (TwoStageCallbackMethod)node.NodeCreator.Methods[methodIndex].notNull();
            this.callback = callback ?? EmptyCommandServerCallback<ResponseParameter>.Default;
            this.keepCallback = keepCallback ?? KeepCallbackResponseParameter.EmptyKeepCallback;
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
            var keepCallback = this.keepCallback;
            if (keepCallback != null)
            {
                this.keepCallback = null;
                if (Node.IsPersistence)
                {
                    try
                    {
                        Node.IsPersistenceCallbackChanged = false;
                        method.TwoStageCallback(Node, callback, ref keepCallback);
                    }
                    finally
                    {
                        StreamPersistenceMemoryDatabaseServiceBase service = Node.NodeCreator.Service;
                        var rebuilder = service.Rebuilder;
                        if (keepCallback == null)
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
                                    keepCallback.VirtualCallbackCancelKeep(new KeepCallbackResponseParameter(CallStateEnum.IgnorePersistenceCallbackException));
                                }
                                else keepCallback.VirtualCallbackCancelKeep(new KeepCallbackResponseParameter(CallStateEnum.PersistenceCallbackException));
                            }
                            finally
                            {
                                if (isPersistenceCallbackException) Node.SetPersistenceCallbackException();
                                if (rebuilder != null && !Node.Rebuilding) rebuilder.PushQueue(this);
                            }
                        }
                    }
                }
                else method.TwoStageCallback(Node, callback, ref keepCallback);
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
            var keepCallback = this.keepCallback;
            if (keepCallback != null)
            {
                this.keepCallback = null;
                keepCallback.VirtualCallbackCancelKeep(new KeepCallbackResponseParameter(state));
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
