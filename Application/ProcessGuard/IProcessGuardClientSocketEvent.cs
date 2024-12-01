using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 进程守护客户端套接字事件
    /// </summary>
    public interface IProcessGuardClientSocketEvent
    {
        /// <summary>
        /// 进程守护客户端
        /// </summary>
        ProcessGuardClient ProcessGuardClient { get; }
        /// <summary>
        /// 进程守护客户端接口
        /// </summary>
        IProcessGuardServiceClientController IProcessGuardClient { get; }
    }
}
