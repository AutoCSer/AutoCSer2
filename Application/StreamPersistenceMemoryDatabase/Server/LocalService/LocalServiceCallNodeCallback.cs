using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点回调对象
    /// </summary>
    internal sealed class LocalServiceCallNodeCallback : CommandServerCallback<CallStateEnum>
    {
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        private readonly LocalServiceCallNode node;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        /// <param name="node">本地服务调用节点方法队列节点</param>
        internal LocalServiceCallNodeCallback(LocalServiceCallNode node)
        {
            this.node = node;
        }
        /// <summary>
        /// 回调设置结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool Callback(CallStateEnum result)
        {
            return node.Callback(result);
        }
    }
}
