using System;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 发布任务状态变更操作状态
    /// </summary>
    public enum OperationStateEnum : byte
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        Unknown,
        /// <summary>
        /// 任务执行成功
        /// </summary>
        Success,
        /// <summary>
        /// 任务已经关闭
        /// </summary>
        Closed,
        /// <summary>
        /// 没有找到任务
        /// </summary>
        NotFoundTask,
        /// <summary>
        /// 任务执行异常
        /// </summary>
        Exception,
        /// <summary>
        /// 移除任务
        /// </summary>
        Remove,
        /// <summary>
        /// 任务已经处于启动状态，不允许操作
        /// </summary>
        StartedError,
        /// <summary>
        /// 子任务集合为空
        /// </summary>
        EmptyTask,
        /// <summary>
        /// 启动定时时间超出范围错误
        /// </summary>
        StartTimeError,
        /// <summary>
        /// 没有找到文件
        /// </summary>
        NotFoundFile,
        /// <summary>
        /// 数据解压缩失败
        /// </summary>
        DecompressFailed,
        /// <summary>
        /// 数据反序列化失败
        /// </summary>
        DeserializeFailed,
    }
}
