using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 添加任务步骤状态
    /// </summary>
    public enum DeployTaskAppendStateEnum : byte
    {
        /// <summary>
        /// 未知错误
        /// </summary>
        Unknown,
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 没有找到任务
        /// </summary>
        NotFound,
    }
}
