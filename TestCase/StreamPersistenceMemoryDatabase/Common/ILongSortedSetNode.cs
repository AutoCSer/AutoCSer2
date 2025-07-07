using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试排序集合节点接口 节点接口
    /// </summary>
    [ServerNode]
    public partial interface ILongSortedSetNode : ISortedSetNode<long>
    {
    }
}
