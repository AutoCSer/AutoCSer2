using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 进程守护客户端套接字事件
    /// </summary>
    public interface IProcessGuardClientSocketEvent
    {
        /// <summary>
        /// 分布式锁客户端接口
        /// </summary>
        IProcessGuardClient ProcessGuardClient { get; }
    }
}
