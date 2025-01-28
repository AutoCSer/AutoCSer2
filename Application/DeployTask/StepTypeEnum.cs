using System;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 子任务类型
    /// </summary>
    public enum StepTypeEnum : ushort
    {
        /// <summary>
        /// 未知类型，发布任务启动阶段
        /// </summary>
        Unknown,
        /// <summary>
        /// 执行程序
        /// </summary>
        StartProcess,
        /// <summary>
        /// 上传文件最后移动文件操作
        /// </summary>
        UploadCompleted,
    }
}
