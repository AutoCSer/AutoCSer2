using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试数组节点
    /// </summary>
    internal sealed class StringArrayNode : ArrayNode<string>, IStringArrayNode
    {
        /// <summary>
        /// 测试数组节点
        /// </summary>
        /// <param name="length">数组长度</param>
        public StringArrayNode(int length) : base(length) { }
    }
}
