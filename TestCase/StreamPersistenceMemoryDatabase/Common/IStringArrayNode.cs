using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试数组节点接口
    /// </summary>
    [ServerNode]
    public partial interface IStringArrayNode : IArrayNode<string>
    {
    }
}
