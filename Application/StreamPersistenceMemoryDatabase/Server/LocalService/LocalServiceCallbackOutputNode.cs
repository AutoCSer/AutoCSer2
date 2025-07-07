using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class LocalServiceCallbackOutputNode<T> : ReadWriteQueueNode
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
        private readonly LocalServiceCallbackOutputNodeCallback<T> serverCallback;
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        private readonly Action<LocalResult<T>> callback;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">Local service client node
        /// 本地服务客户端节点</param>
        /// <param name="methodIndex">Call method number
        /// 调用方法编号</param>
        /// <param name="callback">The client callback delegate
        /// 客户端回调委托</param>
        private LocalServiceCallbackOutputNode(LocalClientNode clientNode, int methodIndex, Action<LocalResult<T>> callback)
        {
            this.clientNode = clientNode;
            nodeIndex = clientNode.Index;
            this.methodIndex = methodIndex;
            this.callback = callback;
            serverCallback = new LocalServiceCallbackOutputNodeCallback<T>(this);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        public override void RunTask()
        {
            clientNode.Client.Service.CallOutput(nodeIndex, methodIndex, serverCallback);
        }
        /// <summary>
        /// 队列节点回调设置结果
        /// </summary>
        /// <param name="responseParameter"></param>
        /// <returns></returns>
        internal bool Callback(ResponseParameter responseParameter)
        {
            if (responseParameter.State == CallStateEnum.Success)
            {
                var result = ((ResponseParameter<T>)responseParameter).Value.ReturnValue;
                if (clientNode.IsSynchronousCallback) callback(result);
                else Task.Run(() => callback(result));
            }
            else
            {
                try
                {
                    if (clientNode.IsSynchronousCallback) callback(responseParameter.State);
                    else Task.Run(() => callback(responseParameter.State));
                }
                finally { clientNode.CheckState(nodeIndex, responseParameter.State); }
            }
            return true;
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
        public static void Create(LocalClientNode clientNode, int methodIndex, Action<LocalResult<T>> callback, bool isWriteQueue)
        {
            bool isCallback = true;
            try
            {
                LocalServiceCallbackOutputNode<T> node = new LocalServiceCallbackOutputNode<T>(clientNode, methodIndex, callback);
                if(isWriteQueue) clientNode.Client.Service.CommandServerCallQueue.AppendWriteOnly(node);
                else clientNode.Client.Service.CommandServerCallQueue.AppendReadOnly(node);
                isCallback = false;
            }
            finally
            {
                if (isCallback) callback(CallStateEnum.Unknown);
            }
        }
    }
}
