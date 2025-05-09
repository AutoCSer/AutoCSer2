using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 测试消息处理节点
    /// </summary>
    internal sealed class TestClassMessageNode : MessageNode<TestClassMessage, ITestClassMessageNode>, ITestClassMessageNode
    {
        /// <summary>
        /// 消息处理节点
        /// </summary>
        /// <param name="arraySize">正在处理消息数组大小</param>
        /// <param name="timeoutSeconds">消息处理超时秒数</param>
        /// <param name="checkTimeoutSeconds">消息超时检查间隔秒数</param>
        public TestClassMessageNode(int arraySize = 1 << 10, int timeoutSeconds = 30, int checkTimeoutSeconds = 1) : base(arraySize, timeoutSeconds, checkTimeoutSeconds) { }
    }
}
