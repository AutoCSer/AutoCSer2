using AutoCSer.Net;
using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点
    /// </summary>
    internal sealed class LocalServiceSendOnlyNode : QueueTaskNode
    {
        /// <summary>
        /// 调用方法与参数信息
        /// </summary>
        private readonly SendOnlyMethodParameter parameter;
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="parameter">调用方法与参数信息</param>
        private LocalServiceSendOnlyNode(LocalClientNode clientNode, SendOnlyMethodParameter parameter)
        {
            this.parameter = parameter;
            clientNode.Client.Service.CommandServerCallQueue.AddOnly(this);
        }
        /// <summary>
        /// 调用节点方法
        /// </summary>
        public override void RunTask()
        {
            parameter.SendOnly();
        }

        /// <summary>
        /// 创建本地服务调用节点方法队列节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="clientNode">本地服务客户端节点</param>
        /// <param name="methodIndex">调用方法编号</param>
        /// <param name="parameter">调用参数</param>
        /// <returns></returns>
        internal static MethodParameter Create<T>(LocalClientNode clientNode, int methodIndex, T parameter) where T : struct
        {
            CallStateEnum state;
            NodeIndex nodeIndex = clientNode.Index;
            SendOnlyMethodParameter<T> methodParameter = (SendOnlyMethodParameter<T>)clientNode.Client.Service.CreateInputMethodParameter(nodeIndex, methodIndex, out state);
            if (state == CallStateEnum.Success)
            {
                methodParameter.Parameter = parameter;
                return new LocalServiceSendOnlyNode(clientNode, methodParameter).parameter;
            }
            clientNode.CheckState(nodeIndex, state);
            return null;
        }
    }
}
