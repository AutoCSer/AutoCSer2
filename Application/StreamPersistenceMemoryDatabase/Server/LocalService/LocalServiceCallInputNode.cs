using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    internal sealed class LocalServiceCallInputNode : LocalServiceQueueNode<ResponseResult>
    {
        /// <summary>
        /// 本地服务客户端节点
        /// </summary>
        private readonly LocalClientNode clientNode;
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        private readonly CallInputMethodParameter parameter;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        private readonly LocalServiceCallInputNodeCallback callback;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="parameter">调用方法与参数信息</param>
        private LocalServiceCallInputNode(LocalClientNode clientNode, CallInputMethodParameter parameter) : base(clientNode.Client.Service)
        {
            this.clientNode = clientNode;
            this.parameter = parameter;
            callback = new LocalServiceCallInputNodeCallback(this);
            service.CommandServerCallQueue.AddOnly(this);
        }
        /// <summary>
        /// 调用状态错误
        /// </summary>
        /// <param name="result"></param>
        private LocalServiceCallInputNode(CallStateEnum result) : base(null)
        {
            this.result = new ResponseResult(result);
            IsCompleted = true;
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
            this.result = new ResponseResult(result);
            completed();
            if (result != CallStateEnum.Success) clientNode.CheckState(parameter.Node.Index, result);
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
        internal static LocalServiceQueueNode<ResponseResult> Create<T>(LocalClientNode clientNode, int methodIndex, T parameter) where T : struct
        {
            CallStateEnum state;
            NodeIndex nodeIndex = clientNode.Index;
            CallInputMethodParameter<T> methodParameter = (CallInputMethodParameter<T>)clientNode.Client.Service.CreateInputMethodParameter(nodeIndex, methodIndex, out state);
            if (state == CallStateEnum.Success)
            {
                methodParameter.Parameter = parameter;
                return new LocalServiceCallInputNode(clientNode, methodParameter);
            }
            clientNode.CheckState(nodeIndex, state);
            return new LocalServiceCallInputNode(state);
        }
    }
}
