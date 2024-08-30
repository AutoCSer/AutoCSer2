using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 发布任务
    /// </summary>
    public interface IDeployTask
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        DeployTaskOperationTypeEnum OperationType { get; }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns>错误日志</returns>
        Task<DeployTaskLog> Run();
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <returns></returns>
        Task Cancel();
    }
}
