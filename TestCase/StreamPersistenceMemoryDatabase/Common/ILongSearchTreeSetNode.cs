using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试二叉搜索树集合节点接口 节点接口
    /// </summary>
    [ServerNode]
    public partial interface ILongSearchTreeSetNode : ISearchTreeSetNode<long>
    {
    }
}
