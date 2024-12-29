using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 自定义发布任务
    /// </summary>
    internal sealed class CustomDeployTask : IDeployTask
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        private readonly ushort operationType;
        /// <summary>
        /// 获取执行任务
        /// </summary>
        private readonly Func<Task<DeployTaskLog>> getTask;
        /// <summary>
        /// 获取取消任务
        /// </summary>
#if NetStandard21
        private readonly Func<Task>? getCancelTask;
#else
        private readonly Func<Task> getCancelTask;
#endif
        /// <summary>
        /// 操作类型
        /// </summary>
        DeployTaskOperationTypeEnum IDeployTask.OperationType { get { return (DeployTaskOperationTypeEnum)operationType; } }
        /// <summary>
        /// 自定义发布任务
        /// </summary>
        /// <param name="operationType">操作类型</param>
        /// <param name="getTask">获取执行任务</param>
        /// <param name="getCancelTask">获取取消任务</param>
#if NetStandard21
        internal CustomDeployTask(ushort operationType, Func<Task<DeployTaskLog>> getTask, Func<Task>? getCancelTask = null)
#else
        internal CustomDeployTask(ushort operationType, Func<Task<DeployTaskLog>> getTask, Func<Task> getCancelTask = null)
#endif
        {
            this.operationType = operationType;
            this.getTask = getTask;
            this.getCancelTask = getCancelTask;
        }
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns>错误日志</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        Task<DeployTaskLog> IDeployTask.Run()
        {
            return getTask();
        }
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        Task IDeployTask.Cancel()
        {
            return getCancelTask != null ? getCancelTask() : AutoCSer.Common.CompletedTask;
        }
    }
}
