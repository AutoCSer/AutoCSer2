using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.DeployServer
{
    /// <summary>
    /// 发布任务服务端 自定义扩展
    /// </summary>
    internal sealed class CustomDeployService  : DeployTaskService, AutoCSer.DeployService.ICustomService
    {
        ///// <summary>
        ///// 添加示例发布以后的后续处理任务
        ///// </summary>
        ///// <param name="socket"></param>
        ///// <param name="queue"></param>
        ///// <param name="taskIdentity">任务ID</param>
        ///// <returns></returns>
        //async Task<DeployTaskAppendResult> AutoCSer.DeployService.ICustomService.AppendCopyExample(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity)
        //{
        //    if (Tasks.TryGetValue(taskIdentity, out DeployTaskBuilder builder)) return await builder.AppendCustom((ushort)CustomDeployTaskOperationType.CopyExample, () => copyExample());
        //    return new DeployTaskAppendResult { AppendState = DeployTaskAppendState.NotFound };
        //}
        ///// <summary>
        ///// 示例发布以后的后续处理
        ///// </summary>
        ///// <returns></returns>
        //async Task<DeployTaskLog> copyExample()
        //{
        //    string path = AutoCSer.DeployConfig.Common.ServerPath + @"www.AutoCSer.com\Download\";
        //    await AutoCSer.Common.Config.FileCopyTo(path + "AutoCSer.zip", path + "AutoCSer.Example.zip", true);
        //    return new DeployTaskLog { OperationState = DeployTaskOperationState.Completed };
        //}
    }
}
