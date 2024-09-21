using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 本地服务调用节点方法队列节点回调对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class LocalServiceCallOutputNodeCallback<T> : CommandServerCallback<ResponseParameter>
    {
        /// <summary>
        /// 本地服务调用节点方法队列节点
        /// </summary>
        private readonly LocalServiceCallOutputNode<T> node;
        /// <summary>
        /// 本地服务调用节点方法队列节点回调对象
        /// </summary>
        /// <param name="node">本地服务调用节点方法队列节点</param>
        internal LocalServiceCallOutputNodeCallback(LocalServiceCallOutputNode<T> node)
        {
            this.node = node;
        }
        /// <summary>
        /// 回调设置结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool Callback(ResponseParameter result)
        {
            return node.Callback(result);
        }
    }
}
