using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试队列节点接口
    /// </summary>
    [ServerNode]
    public partial interface IStringQueueNode : IQueueNode<string>
    {
    }
}
