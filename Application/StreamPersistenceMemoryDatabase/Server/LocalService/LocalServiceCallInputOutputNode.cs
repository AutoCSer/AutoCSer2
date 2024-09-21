using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class LocalServiceCallInputOutputNode<T> : LocalServiceQueueNode<ResponseResult<T>>
    {
        /// <summary>
        /// 本地服务客户端节点
        /// </summary>
        private readonly LocalClientNode clientNode;
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        private readonly CallInputOutputMethodParameter parameter;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        private readonly LocalServiceCallInputOutputNodeCallback<T> callback;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="parameter">调用方法与参数信息</param>
        internal LocalServiceCallInputOutputNode(LocalClientNode clientNode, CallInputOutputMethodParameter parameter) : base(clientNode.Client.Service)
        {
            this.clientNode = clientNode;
            this.parameter = parameter;
            callback = new LocalServiceCallInputOutputNodeCallback<T>(this);
            service.CommandServerCallQueue.AddOnly(this);
        }
        /// <summary>
        /// 调用状态错误
        /// </summary>
        /// <param name="result"></param>
        internal LocalServiceCallInputOutputNode(CallStateEnum result) : base(null)
        {
            this.result = new ResponseResult<T>(result);
            IsCompleted = true;
        }
        /// <summary>
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
            else this.result = new ResponseResult<T>(result.State);
            completed();
            if (result.State != CallStateEnum.Success) clientNode.CheckState(parameter.Node.Index, result.State);
            return true;
        }
    }
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    internal static class LocalServiceCallInputOutputNode
    {
        /// <summary>
        /// 创建本地服务调用节点方法队列节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="PT"></typeparam>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="parameter">调用参数</param>
        /// <returns></returns>
        internal static LocalServiceQueueNode<ResponseResult<T>> Create<T, PT>(LocalClientNode clientNode, int methodIndex, PT parameter) where PT : struct
        {
            CallStateEnum state;
            NodeIndex nodeIndex = clientNode.Index;
            CallInputOutputMethodParameter<PT> methodParameter = (CallInputOutputMethodParameter<PT>)clientNode.Client.Service.CreateInputMethodParameter(nodeIndex, methodIndex, out state);
            if (state == CallStateEnum.Success)
            {
                methodParameter.Parameter = parameter;
                return new LocalServiceCallInputOutputNode<T>(clientNode, methodParameter);
            }
            clientNode.CheckState(nodeIndex, state);
            return new LocalServiceCallInputOutputNode<T>(state);
        }
    }
}
