using AutoCSer.Extensions;
using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 接口实时调用监视服务会话对象操作对象
    /// </summary>
    public class CommandListenerSession : ICommandListenerSession<ITimestampVerifySession>, ICommandListenerSession<IInterfaceMonitorSession>
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
        private TimestampVerifySession? tryGetSessionObject(CommandServerSocket socket)
#else
        private TimestampVerifySession tryGetSessionObject(CommandServerSocket socket)
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
        private TimestampVerifySession createSessionObject(CommandServerSocket socket)
        {
            TimestampVerifySession session = new TimestampVerifySession(socket);
            socket.SessionObject = lastSession = session;
            return session;
        }
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
            return tryGetSessionObject(socket);
        }
        /// <summary>
        /// 创建会话对象
        /// </summary>
        /// <param name="socket">命令服务套接字</param>
        /// <returns></returns>
        ITimestampVerifySession ICommandListenerSession<ITimestampVerifySession>.CreateSessionObject(CommandServerSocket socket)
        {
            return createSessionObject(socket);
        }
        /// <summary>
        /// 尝试从命令服务套接字自定义会话对象获取指定会话对象
        /// </summary>
        /// <param name="socket">命令服务套接字</param>
        /// <returns>失败返回 null</returns>
#if NetStandard21
        IInterfaceMonitorSession? ICommandListenerGetSession<IInterfaceMonitorSession>.TryGetSessionObject(CommandServerSocket socket)
#else
        IInterfaceMonitorSession ICommandListenerGetSession<IInterfaceMonitorSession>.TryGetSessionObject(CommandServerSocket socket)
#endif
        {
            return tryGetSessionObject(socket);
        }
        /// <summary>
        /// 创建会话对象
        /// </summary>
        /// <param name="socket">命令服务套接字</param>
        /// <returns></returns>
        IInterfaceMonitorSession ICommandListenerSession<IInterfaceMonitorSession>.CreateSessionObject(CommandServerSocket socket)
        {
            return createSessionObject(socket);
        }

        /// <summary>
        /// 默认接口实时调用监视服务会话对象操作对象
        /// </summary>
        internal static readonly CommandListenerSession Default = new CommandListenerSession();
    }
}
