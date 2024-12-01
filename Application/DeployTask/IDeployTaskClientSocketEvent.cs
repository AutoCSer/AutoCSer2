using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 发布任务客户端套接字事件
    /// </summary>
    public interface IDeployTaskClientSocketEvent
    {
        /// <summary>
        /// 发布任务客户端接口
        /// </summary>
        IDeployTaskServiceClientController DeployTaskClient { get; }
    }
}
