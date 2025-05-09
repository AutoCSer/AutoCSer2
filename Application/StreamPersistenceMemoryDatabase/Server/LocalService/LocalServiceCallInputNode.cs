using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    public sealed class LocalServiceCallInputNode : LocalServiceQueueNode<LocalResult>
    {
        /// <summary>
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
        private readonly CallInputMethodParameter parameter;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private readonly LocalServiceCallInputNodeCallback callback;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="parameter">调用方法与参数信息</param>
        private LocalServiceCallInputNode(LocalClientNode clientNode, CallInputMethodParameter parameter) : base(clientNode.Client.Service)
        {
            this.clientNode = clientNode;
            nodeIndex = clientNode.Index;
            this.parameter = parameter;
            callback = new LocalServiceCallInputNodeCallback(this);
            if ((parameter.Method.Flags & MethodFlagsEnum.IsWriteQueue) == 0) service.CommandServerCallQueue.AppendReadOnly(this);
            else service.CommandServerCallQueue.AppendWriteOnly(this);
        }
        /// <summary>
        /// 调用状态错误
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="result"></param>
        private LocalServiceCallInputNode(LocalClientNode clientNode, CallStateEnum result) : base(clientNode.Client.Service)
        {
            this.clientNode = clientNode;
            nodeIndex = clientNode.Index;
            this.result = new LocalResult(result);
            IsCompleted = true;
            continuation = Common.EmptyAction;
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        public override void RunTask()
        {
            service.CallInput(parameter, callback);
        }
        /// <summary>
        /// 队列节点回调设置结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal bool Callback(CallStateEnum result)
        {
            this.result = new LocalResult(result);
            completed(clientNode.IsSynchronousCallback);
            if (result != CallStateEnum.Success) clientNode.CheckState(nodeIndex, result);
            return true;
        }

        /// <summary>
        /// 创建本地服务调用节点方法队列节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="parameter">调用参数</param>
        /// <returns></returns>
        public static LocalServiceQueueNode<LocalResult> Create<T>(LocalClientNode clientNode, int methodIndex, T parameter) where T : struct
        {
            CallStateEnum state;
            NodeIndex nodeIndex = clientNode.Index;
            var callInputMethodParameter = clientNode.Client.Service.CreateInputMethodParameter(nodeIndex, methodIndex, out state).castType<CallInputMethodParameter<T>>();
            if (state == CallStateEnum.Success)
            {
                var methodParameter = callInputMethodParameter.notNull();
                methodParameter.Parameter = parameter;
                return new LocalServiceCallInputNode(clientNode, methodParameter);
            }
            clientNode.CheckState(nodeIndex, state);
            return new LocalServiceCallInputNode(clientNode, state);
        }
    }
}
