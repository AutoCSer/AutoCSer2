using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 子任务执行接口
    /// </summary>
    public interface IStepTask
    {
        /// <summary>
        /// Execute the tasks
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        Task<OperationStateEnum> Run();
        /// <summary>
        /// 移除任务
        /// </summary>
        /// <returns></returns>
        Task Remove();
    }
}
