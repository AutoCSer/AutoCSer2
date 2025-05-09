using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试栈节点
    /// </summary>
    internal sealed class StringStackNode : StackNode<string>, IStringStackNode
    {
        /// <summary>
        /// 栈节点
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        public StringStackNode(int capacity = 0) : base(capacity) { }
    }
}
