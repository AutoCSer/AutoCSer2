using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试哈希表节点接口
    /// </summary>
    [ServerNode]
    public partial interface IStringHashSetNode : IHashSetNode<string>
    {
    }
}
