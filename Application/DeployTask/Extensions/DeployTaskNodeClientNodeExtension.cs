using AutoCSer.CommandService;
using AutoCSer.CommandService.DeployTask;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 发布任务节点接口客户端扩展操作
    /// </summary>
    public static class DeployTaskNodeClientNodeExtension
    {
        /// <summary>
        /// Add a sub-task
        /// 添加子任务
        /// </summary>
        /// <param name="node"></param>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="uploadFileClient">文件上传客户端</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ResponseParameterAwaiter<AutoCSer.CommandService.DeployTask.OperationStateEnum> AppendStepTask(this IDeployTaskNodeClientNode node, long identity, UploadFileClient uploadFileClient)
        {
            return node.AppendStepTask(identity, uploadFileClient.UploadCompletedTask.StepTaskData);
        }
        /// <summary>
        /// Add a sub-task
        /// 添加子任务
        /// </summary>
        /// <param name="node"></param>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="task">上传文件最后移动文件操作任务</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ResponseParameterAwaiter<AutoCSer.CommandService.DeployTask.OperationStateEnum> AppendStepTask(this IDeployTaskNodeClientNode node, long identity, UploadCompletedTask task)
        {
            return node.AppendStepTask(identity, task.StepTaskData);
        }
        /// <summary>
        /// Add a sub-task
        /// 添加子任务
        /// </summary>
        /// <param name="node"></param>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="task">The task of executing a program
        /// 执行程序任务</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ResponseParameterAwaiter<AutoCSer.CommandService.DeployTask.OperationStateEnum> AppendStepTask(this IDeployTaskNodeClientNode node, long identity, StartProcessTask task)
        {
            return node.AppendStepTask(identity, task.StepTaskData);
        }
        /// <summary>
        /// Add a sub-task
        /// 添加子任务
        /// </summary>
        /// <param name="node"></param>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="serverProcessFileName">运行文件</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ResponseParameterAwaiter<AutoCSer.CommandService.DeployTask.OperationStateEnum> AppendStepTask(this IDeployTaskNodeClientNode node, long identity, string serverProcessFileName)
        {
            return node.AppendStepTask(identity, new StartProcessTask(serverProcessFileName).StepTaskData);
        }
        /// <summary>
        /// Remove completed or un-started task
        /// 移除已结束或者未开始任务
        /// </summary>
        /// <param name="node"></param>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ResponseParameterAwaiter<AutoCSer.CommandService.DeployTask.OperationStateEnum> Remove(this IDeployTaskNodeClientNode node, long identity)
        {
            return node.Remove(identity, AutoCSer.Threading.SecondTimer.UtcNow);
        }
        /// <summary>
        /// 立即启动任务
        /// </summary>
        /// <param name="node"></param>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ResponseParameterAwaiter<AutoCSer.CommandService.DeployTask.OperationStateEnum> Start(this IDeployTaskNodeClientNode node, long identity)
        {
            return node.Start(identity, DateTime.MinValue);
        }
    }
}
