using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Throughput performance test message
    /// 吞吐性能测试消息
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public sealed partial class PerformanceMessage : Message<PerformanceMessage>
    {
        /// <summary>
        /// Message data
        /// 消息数据
        /// </summary>
        public int Message;
    }
}
