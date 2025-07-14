using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class LocalServiceInputKeepCallbackEnumeratorNode<T> : LocalServiceQueueNode<LocalKeepCallback<T>>
    {
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private readonly InputKeepCallbackMethodParameter parameter;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        private readonly LocalServiceKeepCallbackEnumeratorNodeCallback<T> callback;
        /// <summary>
        /// Local service client node
        /// 本地服务客户端节点
        /// </summary>
        private readonly LocalClientNode clientNode;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="parameter">调用方法与参数信息</param>
        internal LocalServiceInputKeepCallbackEnumeratorNode(LocalClientNode clientNode, InputKeepCallbackMethodParameter parameter) : base(clientNode.Client.Service)
        {
            this.clientNode = clientNode;
            this.parameter = parameter;
            callback = new LocalServiceKeepCallbackEnumeratorNodeCallback<T>(clientNode.IsSynchronousCallback);
            result = callback.Response;
            if ((parameter.Method.Flags & MethodFlagsEnum.IsWriteQueue) == 0) service.CommandServerCallQueue.AppendReadOnly(this);
            else service.CommandServerCallQueue.AppendWriteOnly(this);
        }
        /// <summary>
        /// 调用状态错误
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="result"></param>
        internal LocalServiceInputKeepCallbackEnumeratorNode(LocalClientNode clientNode, CallStateEnum result) : base(clientNode.Client.Service)
        {
            IsCompleted = true;
            continuation = Common.EmptyAction;
            this.clientNode = clientNode;
            callback = new LocalServiceKeepCallbackEnumeratorNodeCallback<T>(clientNode.IsSynchronousCallback);
            callback.VirtualCallbackCancelKeep(new KeepCallbackResponseParameter(result));
            this.result = callback.Response;
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
        /// <summary>
        /// 调用方主动取消回调
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Cancel()
        {
            callback.SetCancelKeep();
        }
    }
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    public static class LocalServiceInputKeepCallbackEnumeratorNode
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
        /// <returns></returns>
        public static LocalServiceQueueNode<LocalKeepCallback<T>> Create<T, PT>(LocalClientNode clientNode, int methodIndex, PT parameter) where PT : struct
        {
            CallStateEnum state;
            NodeIndex nodeIndex = clientNode.Index;
            var inputKeepCallbackMethodParameter = clientNode.Client.Service.CreateInputMethodParameter(nodeIndex, methodIndex, out state).castType<InputKeepCallbackMethodParameter<PT>>();
            if (state == CallStateEnum.Success)
            {
                var methodParameter = inputKeepCallbackMethodParameter.notNull();
                methodParameter.Parameter = parameter;
                return new LocalServiceInputKeepCallbackEnumeratorNode<T>(clientNode, methodParameter);
            }
            clientNode.CheckState(nodeIndex, state);
            return new LocalServiceInputKeepCallbackEnumeratorNode<T>(clientNode, state);
        }
    }
}
