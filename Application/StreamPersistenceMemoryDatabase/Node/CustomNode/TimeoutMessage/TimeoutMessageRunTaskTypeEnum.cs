using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode
{
    /// <summary>
    /// Run task type
    /// 运行任务类型
    /// </summary>
    public enum TimeoutMessageRunTaskTypeEnum : byte
    {
        /// <summary>
        /// The client initiates the task actively
        /// 客户端主动启动任务
        /// </summary>
        ClientCall,
        /// <summary>
        /// Start the task after timeout
        /// 超时启动任务
        /// </summary>
        Timeout,
        /// <summary>
        /// Failed task retry
        /// 失败任务重试
        /// </summary>
        RetryFailed,
        /// <summary>
        /// The database restarts and retries the unfinished task
        /// 数据库重启重试未完成任务
        /// </summary>
        Loaded,
    }
}
