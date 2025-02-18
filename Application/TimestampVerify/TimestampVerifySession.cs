using AutoCSer.Extensions;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证会话对象
    /// </summary>
    public class TimestampVerifySession : CommandServerSocketSessionObject, ITimestampVerifySession
    {
        /// <summary>
        /// 服务端分配的时间戳
        /// </summary>
        public long ServerTimestamp { get; set; }
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证会话对象
        /// </summary>
        /// <param name="socket"></param>
        public TimestampVerifySession(CommandServerSocket socket) : base(socket) { }

        /// <summary>
        /// 默认空会话对象
        /// </summary>
        internal new static readonly TimestampVerifySession Null = new TimestampVerifySession(CommandServerSocket.CommandServerSocketContext);
    }
}
