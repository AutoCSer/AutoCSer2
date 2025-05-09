using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试排序列表节点
    /// </summary>
    internal sealed class LongStringSortedListNode : SortedListNode<long, string>, ILongStringSortedListNode
    {
        /// <summary>
        /// 排序列表节点
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        public LongStringSortedListNode(int capacity) : base(capacity) { }
    }
}
