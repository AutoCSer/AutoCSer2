using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LocalServiceKeepCallbackNode<T> : LocalServiceQueueNode<IDisposable>
    {
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
        /// 调用方法编号
        /// </summary>
        private readonly int methodIndex;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">Local service client node
        /// 本地服务客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The client callback delegate
        /// 客户端回调委托</param>
        /// <param name="isWriteQueue"></param>
        private LocalServiceKeepCallbackNode(LocalClientNode clientNode, int methodIndex, Action<LocalResult<T>> callback, bool isWriteQueue) : base(clientNode.Client.Service)
        {
            this.clientNode = clientNode;
            this.methodIndex = methodIndex;
            this.callback = new LocalServiceKeepCallbackNodeCallback<T>(callback, clientNode.IsSynchronousCallback);
            result = this.callback;
            if (isWriteQueue) service.CommandServerCallQueue.AppendWriteOnly(this);
            else service.CommandServerCallQueue.AppendReadOnly(this);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        public override void RunTask()
        {
            service.KeepCallback(clientNode.Index, methodIndex, callback);
            completed();
        }
        /// <summary>
        /// 创建本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">Local service client node
        /// 本地服务客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The client callback delegate
        /// 客户端回调委托</param>
        /// <param name="isWriteQueue"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LocalServiceQueueNode<IDisposable> Create(LocalClientNode clientNode, int methodIndex, Action<LocalResult<T>> callback, bool isWriteQueue)
        {
            return new LocalServiceKeepCallbackNode<T>(clientNode, methodIndex, callback, isWriteQueue);
        }
    }
}
