using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class LocalServiceInputKeepCallbackNode<T> : LocalServiceQueueNode<KeepCallbackResponse<T>>
    {
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        private readonly InputKeepCallbackMethodParameter parameter;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        private readonly LocalServiceKeepCallbackNodeCallback<T> callback;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="parameter">调用方法与参数信息</param>
        internal LocalServiceInputKeepCallbackNode(LocalClientNode clientNode, InputKeepCallbackMethodParameter parameter) : base(clientNode.Client.Service)
        {
            this.parameter = parameter;
            callback = new LocalServiceKeepCallbackNodeCallback<T>();
            result = callback.Response;
            service.CommandServerCallQueue.AddOnly(this);
        }
        /// <summary>
        /// 调用状态错误
        /// </summary>
        /// <param name="result"></param>
        internal LocalServiceInputKeepCallbackNode(CallStateEnum result) : base(null)
        {
            callback = new LocalServiceKeepCallbackNodeCallback<T>();
            callback.VirtualCallbackCancelKeep(new KeepCallbackResponseParameter(result));
            this.result = callback.Response;
            IsCompleted = true;
        }
        /// <summary>
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
    internal static class LocalServiceInputKeepCallbackNode
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
        internal static LocalServiceQueueNode<KeepCallbackResponse<T>> Create<T, PT>(LocalClientNode clientNode, int methodIndex, PT parameter) where PT : struct
        {
            CallStateEnum state;
            NodeIndex nodeIndex = clientNode.Index;
            InputKeepCallbackMethodParameter<PT> methodParameter = (InputKeepCallbackMethodParameter<PT>)clientNode.Client.Service.CreateInputMethodParameter(nodeIndex, methodIndex, out state);
            if (state == CallStateEnum.Success)
            {
                methodParameter.Parameter = parameter;
                return new LocalServiceInputKeepCallbackNode<T>(clientNode, methodParameter);
            }
            clientNode.CheckState(nodeIndex, state);
            return new LocalServiceInputKeepCallbackNode<T>(state);
        }
    }
}
