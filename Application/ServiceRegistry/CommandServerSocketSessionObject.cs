using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.ServiceRegistry
{
    /// <summary>
    /// 服务注册会话对象操作
    /// </summary>
    internal sealed class CommandServerSocketSessionObject : ICommandServerSocketSessionObject<ServiceRegistryService, ServiceRegisterSession>
    {
        /// <summary>
        /// 尝试从命令服务套接字自定义会话对象获取指定会话对象
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>失败返回 null</returns>
        ServiceRegisterSession ICommandServerSocketSessionObject<ServiceRegistryService, ServiceRegisterSession>.TryGetSessionObject(CommandServerSocket socket)
        {
            return (ServiceRegisterSession)socket.SessionObject;
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
            socket.SessionObject = session;
            return session;
        }
        /// <summary>
        /// 默认服务注册会话对象操作
        /// </summary>
        internal static readonly CommandServerSocketSessionObject Default = new CommandServerSocketSessionObject();
    }
}
