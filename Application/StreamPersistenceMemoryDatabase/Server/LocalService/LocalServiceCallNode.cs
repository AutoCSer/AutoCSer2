using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    public sealed class LocalServiceCallNode : LocalServiceQueueNode<LocalResult>
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
        private readonly LocalServiceCallNodeCallback callback;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">Local service client node
        /// 本地服务客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="isWriteQueue"></param>
        private LocalServiceCallNode(LocalClientNode clientNode, int methodIndex, bool isWriteQueue) : base(clientNode.Client.Service)
        {
            this.clientNode = clientNode;
            this.methodIndex = methodIndex;
            callback = new LocalServiceCallNodeCallback(this);
            nodeIndex = clientNode.Index;
            if(isWriteQueue) service.CommandServerCallQueue.AppendWriteOnly(this);
            else service.CommandServerCallQueue.AppendReadOnly(this);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        public override void RunTask()
        {
            service.Call(nodeIndex, methodIndex, callback);
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
        /// <param name="clientNode">Local service client node
        /// 本地服务客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="isWriteQueue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LocalServiceQueueNode<LocalResult> Create(LocalClientNode clientNode, int methodIndex, bool isWriteQueue)
        {
            return new LocalServiceCallNode(clientNode, methodIndex, isWriteQueue);
        }
    }
}
