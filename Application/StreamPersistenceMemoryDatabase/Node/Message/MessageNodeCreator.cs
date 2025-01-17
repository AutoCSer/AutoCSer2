using AutoCSer.Extensions;
using System;
using System.Reflection;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 创建消息处理节点
    /// </summary>
    internal sealed class MessageNodeCreator
    {
        /// <summary>
        /// 服务基础操作
        /// </summary>
        private readonly ServiceNode node;
        /// <summary>
        /// 节点索引信息
        /// </summary>
        private NodeIndex index;
        /// <summary>
        /// 节点全局关键字
        /// </summary>
        private readonly string key;
        /// <summary>
        /// 节点信息
        /// </summary>
        private readonly NodeInfo nodeInfo;
        /// <summary>
        /// 正在处理消息数组大小
        /// </summary>
        private readonly int arraySize;
        /// <summary>
        /// 消息处理超时秒数
        /// </summary>
        private readonly int timeoutSeconds;
        /// <summary>
        /// 消息超时检查间隔秒数
        /// </summary>
        private readonly int checkTimeoutSeconds;
        /// <summary>
        /// 创建消息处理节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index"></param>
        /// <param name="key"></param>
        /// <param name="nodeInfo"></param>
        /// <param name="arraySize"></param>
        /// <param name="timeoutSeconds"></param>
        /// <param name="checkTimeoutSeconds"></param>
        internal MessageNodeCreator(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo, int arraySize, int timeoutSeconds, int checkTimeoutSeconds)
        {
            this.node = node;
            this.index = index;
            this.key = key;
            this.nodeInfo = nodeInfo;
            this.arraySize = arraySize;
            this.timeoutSeconds = timeoutSeconds;
            this.checkTimeoutSeconds = checkTimeoutSeconds;
        }
        /// <summary>
        /// 创建消息处理节点 MessageNode{T}
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        internal NodeIndex Create(Type messageType)
        {
            createMethodInfo.MakeGenericMethod(messageType).Invoke(this, null);
            return index;
        }
        /// <summary>
        /// 创建消息处理节点 MessageNode{T}
        /// </summary>
        private void create<T>() where T : Message<T>
        {
            index = node.CreateSnapshotNode(index, key, nodeInfo, () => MessageNode<T>.Create(node.Service, arraySize, timeoutSeconds, checkTimeoutSeconds));
        }

        /// <summary>
        /// 创建消息处理节点方法信息
        /// </summary>
        private static readonly MethodInfo createMethodInfo = typeof(MessageNodeCreator).GetMethod(nameof(create), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).notNull();
    }

}
