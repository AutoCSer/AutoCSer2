using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 性能测试二叉搜索树节点接口
    /// </summary>
    [ServerNode]
    public partial interface IPerformanceSearchTreeDictionaryNode : ISearchTreeDictionaryNode<int, int, PerformanceKeyValue>
    {
    }
}
