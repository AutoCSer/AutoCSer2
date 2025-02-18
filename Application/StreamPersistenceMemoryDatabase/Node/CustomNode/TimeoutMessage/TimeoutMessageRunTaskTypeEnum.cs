using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode
{
    /// <summary>
    /// 运行任务类型
    /// </summary>
    public enum TimeoutMessageRunTaskTypeEnum : byte
    {
        /// <summary>
        /// 客户端主动启动任务
        /// </summary>
        ClientCall,
        /// <summary>
        /// 超时启动任务
        /// </summary>
        Timeout,
        /// <summary>
        /// 失败任务重试
        /// </summary>
        RetryFailed,
        /// <summary>
        /// 数据库重启重试未完成任务
        /// </summary>
        Loaded,
    }
}
