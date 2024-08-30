using AutoCSer.CommandService;
using AutoCSer.Net;
using System;

namespace AutoCSer.TestCase.ServiceRegistry
{
    /// <summary>
    /// 套接字自定义会话对象操作
    /// </summary>
    internal sealed class CommandServerSocketSessionObject : ICommandServerSocketSessionObject<ServiceRegistryService, ServiceRegisterSession>, ICommandServerSocketSessionObject<TimestampVerifySession>
    {
        /// <summary>
        /// 获取或者创建套接字自定义会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private CommandServerSocketSessionObject getOrCreate(CommandServerSocket socket)
        {
            CommandServerSocketSessionObject sessionObject = (CommandServerSocketSessionObject)socket.SessionObject;
            if (sessionObject != null) return sessionObject;
            socket.SessionObject = sessionObject = new CommandServerSocketSessionObject();
            return sessionObject;
        }
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证会话对象
        /// </summary>
        private TimestampVerifySession timestampVerifySession;
        /// <summary>
        /// 尝试从命令服务套接字自定义会话对象获取指定会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>失败返回 null</returns>
        TimestampVerifySession ICommandServerSocketSessionObject<TimestampVerifySession>.TryGetSessionObject(CommandServerSocket socket)
        {
            return timestampVerifySession;
        }
        /// <summary>
        /// 创建会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        TimestampVerifySession ICommandServerSocketSessionObject<TimestampVerifySession>.CreateSessionObject(CommandServerSocket socket)
        {
            TimestampVerifySession session = new TimestampVerifySession();
            getOrCreate(socket).timestampVerifySession = session;
            return session;
        }
        /// <summary>
        /// 服务注册会话对象
        /// </summary>
        private ServiceRegisterSession serviceRegisterSession;
        /// <summary>
        /// 尝试从命令服务套接字自定义会话对象获取指定会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>失败返回 null</returns>
        ServiceRegisterSession ICommandServerSocketSessionObject<ServiceRegistryService, ServiceRegisterSession>.TryGetSessionObject(CommandServerSocket socket)
        {
            return serviceRegisterSession;
        }
        /// <summary>
        /// 创建会话对象
        /// </summary>
        /// <param name="service"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
        ServiceRegisterSession ICommandServerSocketSessionObject<ServiceRegistryService, ServiceRegisterSession>.CreateSessionObject(ServiceRegistryService service, CommandServerSocket socket)
        {
            ServiceRegisterSession session = new ServiceRegisterSession(service, socket);
            getOrCreate(socket).serviceRegisterSession = session;
            return session;
        }
    }
}
