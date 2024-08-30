using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 发布任务状态变更操作状态
    /// </summary>
    public enum DeployTaskOperationStateEnum : byte
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        Unknown,
        /// <summary>
        /// 任务执行完成
        /// </summary>
        Completed,
        /// <summary>
        /// 取消任务
        /// </summary>
        Cancel,
        /// <summary>
        /// 启动执行任务
        /// </summary>
        StartRun,
        /// <summary>
        /// 任务执行异常
        /// </summary>
        Exception,
        /// <summary>
        /// 没有找到文件
        /// </summary>
        NotFoundFile,
        /// <summary>
        /// 没有找到任务
        /// </summary>
        NotFoundTask,
        /// <summary>
        /// 任务集合为空
        /// </summary>
        EmptyTask,
        /// <summary>
        /// 启动任务调用冲突
        /// </summary>
        StartedError,
        /// <summary>
        /// 启动定时时间超出范围错误
        /// </summary>
        StartTimeError,
    }
}
