using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class LocalServiceCallbackInputOutputNode<T> : ReadWriteQueueNode
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
        private readonly CallInputOutputMethodParameter parameter;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        private readonly LocalServiceCallbackInputOutputNodeCallback<T> serverCallback;
        /// <summary>
        /// The client callback delegate
        /// 客户端回调委托
        /// </summary>
        private Action<LocalResult<T>> callback;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="parameter">调用方法与参数信息</param>
        /// <param name="callback">The client callback delegate
        /// 客户端回调委托</param>
        internal LocalServiceCallbackInputOutputNode(LocalClientNode clientNode, CallInputOutputMethodParameter parameter, Action<LocalResult<T>> callback)
        {
            this.clientNode = clientNode;
            nodeIndex = clientNode.Index;
            this.parameter = parameter;
            this.callback = callback;
            serverCallback = new LocalServiceCallbackInputOutputNodeCallback<T>(this);
        }
        /// <summary>
        /// Call the node method
        /// 调用节点方法
        /// </summary>
        public override void RunTask()
        {
            clientNode.Client.Service.CallInputOutput(parameter, serverCallback);
        }
        /// <summary>
        /// 队列节点回调设置结果
        /// </summary>
        /// <param name="responseParameter"></param>
        /// <returns></returns>
        internal bool Callback(ResponseParameter responseParameter)
        {
            Action<LocalResult<T>> callback = Interlocked.Exchange(ref this.callback, LocalResult<T>.EmptyAction);
            if (!object.ReferenceEquals(callback, LocalResult.EmptyAction))
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
            return false;
        }
    }
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    public static class LocalServiceCallbackInputOutputNode
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
        /// <param name="callback">The client callback delegate
        /// 客户端回调委托</param>
        public static void Create<T, PT>(LocalClientNode clientNode, int methodIndex, PT parameter, Action<LocalResult<T>> callback) where PT : struct
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                NodeIndex nodeIndex = clientNode.Index;
                var callInputOutputMethodParameter = clientNode.Client.Service.CreateInputMethodParameter(nodeIndex, methodIndex, out state).castType<CallInputOutputMethodParameter<PT>>();
                if (state == CallStateEnum.Success)
                {
                    var methodParameter = callInputOutputMethodParameter.notNull();
                    methodParameter.Parameter = parameter;
                    LocalServiceCallbackInputOutputNode<T> node = new LocalServiceCallbackInputOutputNode<T>(clientNode, methodParameter, callback);
                    if ((methodParameter.Method.Flags & MethodFlagsEnum.IsWriteQueue) == 0) clientNode.Client.Service.CommandServerCallQueue.AppendReadOnly(node);
                    else clientNode.Client.Service.CommandServerCallQueue.AppendWriteOnly(node);
                    state = CallStateEnum.Callbacked;
                    return;
                }
                clientNode.CheckState(nodeIndex, state);
            }
            finally
            {
                if (state != CallStateEnum.Callbacked) callback(state);
            }
        }
    }
}
