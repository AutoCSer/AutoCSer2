using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 发布任务客户端接口
    /// </summary>
    public partial interface IDeployTaskServiceClientController
    {
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="callback">返回 false 表示没有找到任务或者任务已经启动不允许取消</param>
        /// <returns></returns>
#if NetStandard21
        CallbackCommand Cancel(long taskIdentity, Action<CommandClientReturnValue<long>>? callback);
#else
        CallbackCommand Cancel(long taskIdentity, Action<CommandClientReturnValue<long>> callback);
#endif

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="taskIdentity">任务ID</param>
        /// <param name="startTime">运行任务时间</param>
        /// <param name="callback">任务状态变更回调委托</param>
        /// <returns></returns>
        KeepCallbackCommand Start(long taskIdentity, DateTime startTime, Action<CommandClientReturnValue<DeployTaskLog>, KeepCallbackCommand> callback);
    }
}
