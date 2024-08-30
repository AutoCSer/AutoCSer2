using AutoCSer.CommandService;
using System;

namespace AutoCSer.DeployServer
{
    /// <summary>
    /// 发布任务状态变更操作类型
    /// </summary>
    internal enum CustomDeployTaskOperationType : ushort
    {
        /// <summary>
        /// 示例发布以后的后续处理
        /// </summary>
        CopyExample = DeployTaskOperationType.COUNT
    }
}
