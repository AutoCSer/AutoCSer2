using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试 256 基分片哈希表 节点接口
    /// </summary>
    [ServerNode]
    public partial interface IStringFragmentHashSetNode : IFragmentHashSetNode<string>
    {
    }
}
