using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 吞吐性能测试消息
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    public sealed partial class PerformanceMessage : Message<PerformanceMessage>
    {
        /// <summary>
        /// 消息数据
        /// </summary>
        public int Message;
    }
}
