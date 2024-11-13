using AutoCSer.Extensions;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.TimestampVerify
{
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证会话对象操作对象
    /// </summary>
    internal class CommandListenerSession : ICommandListenerSession<ITimestampVerifySession>
    {
        /// <summary>
        /// 最后访问的会话对象
        /// </summary>
        private TimestampVerifySession lastSession = TimestampVerifySession.Null;
        /// <summary>
        /// 尝试从命令服务套接字自定义会话对象获取指定会话对象
        /// </summary>
        /// <param name="socket">命令服务套接字</param>
        /// <returns>失败返回 null</returns>
#if NetStandard21
        ITimestampVerifySession? ICommandListenerGetSession<ITimestampVerifySession>.TryGetSessionObject(CommandServerSocket socket)
#else
        ITimestampVerifySession ICommandListenerGetSession<ITimestampVerifySession>.TryGetSessionObject(CommandServerSocket socket)
#endif
        {
            var session = this.lastSession;
            if (object.ReferenceEquals(session.CommandServerSocket, socket)) return session;
            session = socket.SessionObject.castType<TimestampVerifySession>();
            if (session != null) lastSession = session;
            return session;
        }
        /// <summary>
        /// 创建会话对象
        /// </summary>
        /// <param name="socket">命令服务套接字</param>
        /// <returns></returns>
        ITimestampVerifySession ICommandListenerSession<ITimestampVerifySession>.CreateSessionObject(CommandServerSocket socket)
        {
            TimestampVerifySession session = new TimestampVerifySession(socket);
            socket.SessionObject = lastSession = session;
            return session;
        }

        /// <summary>
        /// 默认基于递增登录时间戳验证的服务认证会话对象操作对象
        /// </summary>
        internal static readonly CommandListenerSession Default = new CommandListenerSession();
    }
}
namespace AutoCSer.CommandService
{
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证会话对象
    /// </summary>
    public interface ITimestampVerifySession
    {
        /// <summary>
        /// 服务端分配的时间戳
        /// </summary>
        long ServerTimestamp { get; set; }
    }
    /// <summary>
    /// 基于递增登录时间戳验证的服务认证会话对象
    /// </summary>
    public sealed class TimestampVerifySession : CommandServerSocketSessionObject, ITimestampVerifySession
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
