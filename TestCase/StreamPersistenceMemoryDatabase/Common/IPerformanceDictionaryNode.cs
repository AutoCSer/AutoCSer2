using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 性能测试字典节点接口
    /// </summary>
    [ServerNode]
    public partial interface IPerformanceDictionaryNode : IDictionaryNode<int, int, PerformanceKeyValue>
    {
    }
}
