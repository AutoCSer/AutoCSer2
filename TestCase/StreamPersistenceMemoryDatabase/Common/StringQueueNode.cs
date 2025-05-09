using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试队列节点接口
    /// </summary>
    internal sealed class StringQueueNode : QueueNode<string>, IStringQueueNode
    {
        /// <summary>
        /// 队列节点接口
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        public StringQueueNode(int capacity = 0) : base(capacity) { }
    }
}
