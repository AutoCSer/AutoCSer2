using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class LocalServiceCallInputOutputNode<T> : LocalServiceQueueNode<LocalResult<T>>
    {
        /// <summary>
        /// Local service client node
        /// 本地服务客户端节点
        /// </summary>
        private readonly LocalClientNode clientNode;
        /// <summary>
        /// 请求节点索引信息
        /// </summary>
        private readonly NodeIndex nodeIndex;
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private readonly CallInputOutputMethodParameter parameter;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private readonly LocalServiceCallInputOutputNodeCallback<T> callback;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="parameter">调用方法与参数信息</param>
        internal LocalServiceCallInputOutputNode(LocalClientNode clientNode, CallInputOutputMethodParameter parameter) : base(clientNode.Client.Service)
        {
            this.clientNode = clientNode;
            nodeIndex = clientNode.Index;
            this.parameter = parameter;
            callback = new LocalServiceCallInputOutputNodeCallback<T>(this);
            if ((parameter.Method.Flags & MethodFlagsEnum.IsWriteQueue) == 0) service.CommandServerCallQueue.AppendReadOnly(this);
            else service.CommandServerCallQueue.AppendWriteOnly(this);
        }
        /// <summary>
        /// 调用状态错误
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="result"></param>
        internal LocalServiceCallInputOutputNode(LocalClientNode clientNode, CallStateEnum result) : base(clientNode.Client.Service)
        {
            this.clientNode = clientNode;
            nodeIndex = clientNode.Index;
            this.result = new LocalResult<T>(result);
            IsCompleted = true;
            continuation = Common.EmptyAction;
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        public override void RunTask()
        {
            service.CallInputOutput(parameter, callback);
        }
        /// <summary>
        /// 队列节点回调设置结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal bool Callback(ResponseParameter result)
        {
            if (result.State == CallStateEnum.Success) this.result = ((ResponseParameter<T>)result).Value.ReturnValue;
            else this.result = new LocalResult<T>(result.State);
            completed(clientNode.IsSynchronousCallback);
            if (result.State != CallStateEnum.Success) clientNode.CheckState(nodeIndex, result.State);
            return true;
        }
    }
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    public static class LocalServiceCallInputOutputNode
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
        public static LocalServiceQueueNode<LocalResult<T>> Create<T, PT>(LocalClientNode clientNode, int methodIndex, PT parameter) where PT : struct
        {
            CallStateEnum state;
            NodeIndex nodeIndex = clientNode.Index;
            var callInputOutputMethodParameter = clientNode.Client.Service.CreateInputMethodParameter(nodeIndex, methodIndex, out state).castType<CallInputOutputMethodParameter<PT>>();
            if (state == CallStateEnum.Success)
            {
                var methodParameter = callInputOutputMethodParameter.notNull();
                methodParameter.Parameter = parameter;
                return new LocalServiceCallInputOutputNode<T>(clientNode, methodParameter);
            }
            clientNode.CheckState(nodeIndex, state);
            return new LocalServiceCallInputOutputNode<T>(clientNode, state);
        }
    }
}
