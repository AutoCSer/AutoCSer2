using AutoCSer.CommandService;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Net.Sockets;

namespace AutoCSer.TestCase.ServiceRegistry
{
    /// <summary>
    /// 服务注册服务会话对象操作
    /// </summary>
    internal sealed class CommandListenerSession : ICommandListenerSession<IServiceRegisterSocketSession, ServiceRegistryService>, ICommandListenerSession<ITimestampVerifySession>
    {
        /// <summary>
        /// 最后访问的会话对象
        /// </summary>
#if NetStandard21
        private ServiceRegisterSocketSession? lastSession;
#else
        private ServiceRegisterSocketSession lastSession;
#endif
        /// <summary>
        /// 尝试从命令服务套接字自定义会话对象获取指定会话对象
        /// </summary>
        /// <param name="socket">命令服务套接字</param>
        /// <returns>失败返回 null</returns>
#if NetStandard21
        IServiceRegisterSocketSession? ICommandListenerGetSession<IServiceRegisterSocketSession>.TryGetSessionObject(CommandServerSocket socket)
#else
        IServiceRegisterSocketSession ICommandListenerGetSession<IServiceRegisterSocketSession>.TryGetSessionObject(CommandServerSocket socket)
#endif
        {
            var session = this.lastSession;
            if (session != null && object.ReferenceEquals(session.Socket, socket)) return session.ServiceRegisterSession;
            session = socket.SessionObject.castType<ServiceRegisterSocketSession>();
            if (session != null)
            {
                lastSession = session;
                return session.ServiceRegisterSession;
            }
            return null;
        }
        /// <summary>
        /// 创建会话对象
        /// </summary>
        /// <param name="service">默认服务注册</param>
        /// <param name="socket">命令服务套接字</param>
        /// <returns></returns>
        IServiceRegisterSocketSession ICommandListenerSession<IServiceRegisterSocketSession, ServiceRegistryService>.CreateSessionObject(ServiceRegistryService service, CommandServerSocket socket)
        {
            var session = socket.SessionObject.castType<ServiceRegisterSocketSession>();
            if (session != null) return session.CreateServiceRegisterSession(service);
            session = new ServiceRegisterSocketSession(socket);
            socket.SessionObject = lastSession = session;
            return session.CreateServiceRegisterSession(service);
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
            var session = this.lastSession;
            if (session != null && object.ReferenceEquals(session.Socket, socket)) return session.TimestampVerifySession;
            session = socket.SessionObject.castType<ServiceRegisterSocketSession>();
            if (session != null)
            {
                lastSession = session;
                return session.TimestampVerifySession;
            }
            return null;
        }
        /// <summary>
        /// 创建会话对象
        /// </summary>
        /// <param name="socket">命令服务套接字</param>
        /// <returns></returns>
        ITimestampVerifySession ICommandListenerSession<ITimestampVerifySession>.CreateSessionObject(CommandServerSocket socket)
        {
            var session = socket.SessionObject.castType<ServiceRegisterSocketSession>();
            if (session != null) return session.CreateITimestampVerifySession();
            session = new ServiceRegisterSocketSession(socket);
            socket.SessionObject = lastSession = session;
            return session.CreateITimestampVerifySession();
        }
    }
}
