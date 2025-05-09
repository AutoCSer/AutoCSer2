using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试排序集合节点
    /// </summary>
    internal sealed class LongSortedSetNode : SortedSetNode<long>, ILongSortedSetNode
    {
    }
}
