using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 发布任务状态变更操作类型
    /// </summary>
    public enum DeployTaskOperationTypeEnum : ushort
    {
        /// <summary>
        /// 未知操作
        /// </summary>
        Unknown,
        /// <summary>
        /// 执行程序
        /// </summary>
        StartProcess,
        /// <summary>
        /// 复制文件
        /// </summary>
        CopyFile,

        /// <summary>
        /// 保留，仅用于计数
        /// </summary>
        COUNT
    }
}
