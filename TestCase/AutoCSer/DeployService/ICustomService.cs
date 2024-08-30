using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.DeployService
{
    /// <summary>
    /// 发布任务服务端接口 自定义扩展
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface(TaskQueueMaxConcurrent = 1, TaskQueueWaitCount = 256)]
    public interface ICustomService : IDeployTaskService
    {
        ///// <summary>
        ///// 添加示例发布以后的后续处理任务
        ///// </summary>
        ///// <param name="socket"></param>
        ///// <param name="queue"></param>
        ///// <param name="taskIdentity">任务ID</param>
        ///// <returns></returns>
        //Task<DeployTaskAppendResult> AppendCopyExample(CommandServerSocket socket, CommandServerCallTaskLowPriorityQueue queue, long taskIdentity);
    }
}
