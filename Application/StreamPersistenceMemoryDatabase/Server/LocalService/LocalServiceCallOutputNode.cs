using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LocalServiceCallOutputNode<T> : LocalServiceQueueNode<LocalResult<T>>
    {
        /// <summary>
        /// Local service client node
        /// 本地服务客户端节点
        /// </summary>
        private readonly LocalClientNode clientNode;
        /// <summary>
        /// 调用方法编号
        /// </summary>
        private readonly int methodIndex;
        /// <summary>
        /// Node index information
        /// 节点索引信息
        /// </summary>
        private readonly NodeIndex nodeIndex;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        private readonly LocalServiceCallOutputNodeCallback<T> callback;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">Local service client node
        /// 本地服务客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="isWriteQueue"></param>
        private LocalServiceCallOutputNode(LocalClientNode clientNode, int methodIndex, bool isWriteQueue) : base(clientNode.Client.Service)
        {
            this.clientNode = clientNode;
            this.methodIndex = methodIndex;
            callback = new LocalServiceCallOutputNodeCallback<T>(this);
            nodeIndex = clientNode.Index;
            if (isWriteQueue) service.CommandServerCallQueue.AppendWriteOnly(this);
            else service.CommandServerCallQueue.AppendReadOnly(this);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        public override void RunTask()
        {
            service.CallOutput(nodeIndex, methodIndex, callback);
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
        /// <summary>
        /// 创建本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">Local service client node
        /// 本地服务客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="isWriteQueue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LocalServiceQueueNode<LocalResult<T>> Create(LocalClientNode clientNode, int methodIndex, bool isWriteQueue)
        {
            return new LocalServiceCallOutputNode<T>(clientNode, methodIndex, isWriteQueue);
        }
    }
}
