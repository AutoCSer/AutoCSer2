using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 进程守护客户端接口
    /// </summary>
    public partial interface IProcessGuardServiceClientController
    {
        /// <summary>
        /// 添加待守护进程
        /// </summary>
        /// <param name="processInfo">进程信息</param>
        /// <param name="callback">是否添加成功 回调委托</param>
        /// <returns>await</returns>
        CallbackCommand Guard(ProcessGuardInfo processInfo, Action<CommandClientReturnValue<bool>> callback);

        /// <summary>
        /// 删除被守护进程
        /// </summary>
        /// <param name="processId">进程标识</param>
        /// <param name="processName">进程名称</param>
        /// <param name="callback">回调委托</param>
        /// <returns>await</returns>
        CallbackCommand Remove(int processId, string processName, CommandClientCallback callback);
    }
}
