using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试仅存档节点接口
    /// </summary>
    [ServerNode]
    public partial interface ITestClassOnlyPersistenceNode : IOnlyPersistenceNode<TestClass>
    {
    }
}
