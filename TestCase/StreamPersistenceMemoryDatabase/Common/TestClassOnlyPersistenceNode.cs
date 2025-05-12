using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试仅存档节点
    /// </summary>
    internal sealed class TestClassOnlyPersistenceNode : OnlyPersistenceNode<TestClass>, ITestClassOnlyPersistenceNode
    {
    }
}
