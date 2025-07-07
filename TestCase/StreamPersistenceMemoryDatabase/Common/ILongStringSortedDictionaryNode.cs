using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试排序字典节点
    /// </summary>
    [ServerNode]
    public partial interface ILongStringSortedDictionaryNode : ISortedDictionaryNode<long, string>
    {
    }
}
