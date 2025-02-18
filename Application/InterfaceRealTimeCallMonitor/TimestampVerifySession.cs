using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证会话对象
    /// </summary>
    public sealed class TimestampVerifySession : AutoCSer.CommandService.TimestampVerifySession, IInterfaceMonitorSession
    {
        /// <summary>
        /// 实时调用监视信息
        /// </summary>
#if NetStandard21
        public InterfaceMonitor? InterfaceMonitor { get; set; }
#else
        public InterfaceMonitor InterfaceMonitor { get; set; }
#endif
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证会话对象
        /// </summary>
        /// <param name="socket"></param>
        public TimestampVerifySession(CommandServerSocket socket) : base(socket) { }

        /// <summary>
        /// 默认空命令服务套接字会话对象
        /// </summary>
        internal new static readonly TimestampVerifySession Null = new TimestampVerifySession(CommandServerSocket.CommandServerSocketContext);
    }
}
