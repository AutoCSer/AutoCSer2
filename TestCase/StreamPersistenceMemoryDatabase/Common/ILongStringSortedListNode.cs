using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试排序列表节点
    /// </summary>
    [ServerNode]
    public partial interface ILongStringSortedListNode : ISortedListNode<long, string>
    {
    }
}
