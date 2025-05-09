using AutoCSer.Extensions;
using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    public sealed class LocalServiceCallbackInputNode : ReadWriteQueueNode
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
        private readonly CallInputMethodParameter parameter;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        private readonly LocalServiceCallbackInputNodeCallback serverCallback;
        /// <summary>
        /// 客户端回调委托
        /// </summary>
        private readonly Action<LocalResult> callback;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="parameter">调用方法与参数信息</param>
        /// <param name="callback">客户端回调委托</param>
        private LocalServiceCallbackInputNode(LocalClientNode clientNode, CallInputMethodParameter parameter, Action<LocalResult> callback)
        {
            this.clientNode = clientNode;
            nodeIndex = clientNode.Index;
            this.parameter = parameter;
            this.callback = callback;
            serverCallback = new LocalServiceCallbackInputNodeCallback(this);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        public override void RunTask()
        {
            clientNode.Client.Service.CallInput(parameter, serverCallback);
        }
        /// <summary>
        /// 队列节点回调设置结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal bool Callback(CallStateEnum result)
        {
            if (result == CallStateEnum.Success)
            {
                if (clientNode.IsSynchronousCallback) callback(new LocalResult(result));
                else Task.Run(() => callback(new LocalResult(result)));
            }
            else
            {
                try
                {
                    if (clientNode.IsSynchronousCallback) callback(new LocalResult(result));
                    else Task.Run(() => callback(new LocalResult(result)));
                }
                finally { clientNode.CheckState(nodeIndex, result); }
            }
            return true;
        }

        /// <summary>
        /// 创建本地服务调用节点方法队列节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="parameter">调用参数</param>
        /// <param name="callback">客户端回调委托</param>
        /// <returns></returns>
        public static void Create<T>(LocalClientNode clientNode, int methodIndex, T parameter, Action<LocalResult> callback) where T : struct
        {
            CallStateEnum state = CallStateEnum.Unknown;
            try
            {
                NodeIndex nodeIndex = clientNode.Index;
                var callInputMethodParameter = clientNode.Client.Service.CreateInputMethodParameter(nodeIndex, methodIndex, out state).castType<CallInputMethodParameter<T>>();
                if (state == CallStateEnum.Success)
                {
                    var methodParameter = callInputMethodParameter.notNull();
                    methodParameter.Parameter = parameter;
                    LocalServiceCallbackInputNode node = new LocalServiceCallbackInputNode(clientNode, methodParameter, callback);
                    if ((methodParameter.Method.Flags & MethodFlagsEnum.IsWriteQueue) == 0) clientNode.Client.Service.CommandServerCallQueue.AppendReadOnly(node);
                    else clientNode.Client.Service.CommandServerCallQueue.AppendWriteOnly(node);
                    state = CallStateEnum.Callbacked;
                    return;
                }
                clientNode.CheckState(nodeIndex, state);
            }
            finally
            {
                if (state != CallStateEnum.Callbacked) callback(new LocalResult(state));
            }
        }
    }
}
