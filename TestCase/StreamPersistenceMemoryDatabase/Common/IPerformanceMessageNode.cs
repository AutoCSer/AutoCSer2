using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 性能测试消息处理节点接口
    /// </summary>
    [ServerNode]
    public partial interface IPerformanceMessageNode : IMessageNode<PerformanceMessage>
    {
    }
}
