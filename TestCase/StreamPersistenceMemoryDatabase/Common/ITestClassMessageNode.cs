using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试消息处理节点接口
    /// </summary>
    [ServerNode]
    public partial interface ITestClassMessageNode : IMessageNode<TestClassMessage>
    {
    }
}
