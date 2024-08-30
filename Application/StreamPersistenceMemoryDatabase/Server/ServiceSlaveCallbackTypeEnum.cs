using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 从节点客户端信息回调类型
    /// </summary>
    internal enum ServiceSlaveCallbackTypeEnum : byte
    {
        /// <summary>
        /// 异常移除节点
        /// </summary>
        Remove,
        /// <summary>
        /// 检查持久化回调异常位置文件已写入位置
        /// </summary>
        CheckPersistenceCallbackExceptionPosition
    }
}
