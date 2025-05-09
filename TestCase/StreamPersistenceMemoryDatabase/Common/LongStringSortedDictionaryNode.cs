using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试排序字典节点
    /// </summary>
    internal sealed class LongStringSortedDictionaryNode : SortedDictionaryNode<long, string>, ILongStringSortedDictionaryNode
    {
    }
}
