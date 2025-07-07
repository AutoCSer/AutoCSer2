using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试栈节点接口
    /// </summary>
    [ServerNode]
    public partial interface IStringStackNode : IStackNode<string>
    {
    }
}
