using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试数组节点
    /// </summary>
    internal sealed class StringLeftArrayNode : LeftArrayNode<string>, IStringLeftArrayNode
    {
        /// <summary>
        /// 数组节点
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        public StringLeftArrayNode(int capacity = 0) : base(capacity) { }
    }
}
