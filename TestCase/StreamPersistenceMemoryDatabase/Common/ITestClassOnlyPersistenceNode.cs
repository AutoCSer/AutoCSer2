using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试仅存档节点接口
    /// </summary>
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true)]
    public partial interface ITestClassOnlyPersistenceNode : IOnlyPersistenceNode<TestClass>
    {
    }
}
