using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class LocalServiceInputKeepCallbackNode<T> : LocalServiceQueueNode<IDisposable>
    {
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private readonly InputKeepCallbackMethodParameter parameter;
        /// <summary>
        /// Local service client node
        /// 本地服务客户端节点
        /// </summary>
        private readonly LocalClientNode clientNode;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        private readonly LocalServiceKeepCallbackNodeCallback<T> callback;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="parameter">调用方法与参数信息</param>
        /// <param name="callback">回调委托</param>
        internal LocalServiceInputKeepCallbackNode(LocalClientNode clientNode, InputKeepCallbackMethodParameter parameter, Action<LocalResult<T>> callback) : base(clientNode.Client.Service)
        {
            this.clientNode = clientNode;
            this.parameter = parameter;
            this.callback = new LocalServiceKeepCallbackNodeCallback<T>(callback, clientNode.IsSynchronousCallback);
            result = this.callback;
            if ((parameter.Method.Flags & MethodFlagsEnum.IsWriteQueue) == 0) service.CommandServerCallQueue.AppendReadOnly(this);
            else service.CommandServerCallQueue.AppendWriteOnly(this);
        }
        /// <summary>
        /// 调用状态错误
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="result"></param>
        /// <param name="callback">回调委托</param>
        internal LocalServiceInputKeepCallbackNode(LocalClientNode clientNode, CallStateEnum result, Action<LocalResult<T>> callback) : base(clientNode.Client.Service)
        {
            IsCompleted = true;
            continuation = Common.EmptyAction;
            this.clientNode = clientNode;
            this.callback = new LocalServiceKeepCallbackNodeCallback<T>(callback, clientNode.IsSynchronousCallback);
            this.callback.VirtualCallbackCancelKeep(new KeepCallbackResponseParameter(result));
            this.result = this.callback;
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        public override void RunTask()
        {
            service.InputKeepCallback(parameter, callback);
            completed();
        }
    }
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    public static class LocalServiceInputKeepCallbackNode
    {
        /// <summary>
        /// 创建本地服务调用节点方法队列节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="PT"></typeparam>
        /// <param name="clientNode">Local service client node
        /// 本地服务客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="parameter">Call parameters
        /// 调用参数</param>
        /// <param name="callback">The client callback delegate
        /// 客户端回调委托</param>
        /// <returns></returns>
        public static LocalServiceQueueNode<IDisposable> Create<T, PT>(LocalClientNode clientNode, int methodIndex, PT parameter, Action<LocalResult<T>> callback) where PT : struct
        {
            CallStateEnum state;
            NodeIndex nodeIndex = clientNode.Index;
            var inputKeepCallbackMethodParameter = clientNode.Client.Service.CreateInputMethodParameter(nodeIndex, methodIndex, out state).castType<InputKeepCallbackMethodParameter<PT>>();
            if (state == CallStateEnum.Success)
            {
                var methodParameter = inputKeepCallbackMethodParameter.notNull();
                methodParameter.Parameter = parameter;
                return new LocalServiceInputKeepCallbackNode<T>(clientNode, methodParameter, callback);
            }
            clientNode.CheckState(nodeIndex, state);
            return new LocalServiceInputKeepCallbackNode<T>(clientNode, state, callback);
        }
    }
}
