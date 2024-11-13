using AutoCSer.CommandService;
using AutoCSer.Net;
using System;

namespace AutoCSer.TestCase.ServiceRegistry
{
    /// <summary>
    /// 服务注册会话对象
    /// </summary>
    internal sealed class ServiceRegisterSocketSession
    {
        /// <summary>
        /// 命令服务套接字
        /// </summary>
        internal readonly CommandServerSocket Socket;
        /// <summary>
        /// 服务注册会话对象
        /// </summary>
        internal AutoCSer.CommandService.ServiceRegisterSocketSession ServiceRegisterSession;
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证会话对象
        /// </summary>
        internal TimestampVerifySession TimestampVerifySession;
        /// <summary>
        /// 服务注册会话对象
        /// </summary>
        /// <param name="socket"></param>
        internal ServiceRegisterSocketSession(CommandServerSocket socket)
        {
            Socket = socket;
        }
        /// <summary>
        /// 创建服务注册会话对象
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        internal AutoCSer.CommandService.ServiceRegisterSocketSession CreateServiceRegisterSession(ServiceRegistryService service)
        {
            if (ServiceRegisterSession == null) ServiceRegisterSession = new CommandService.ServiceRegisterSocketSession(service, Socket);
            return ServiceRegisterSession;
        }
        /// <summary>
        /// 创建基于递增登录时间戳验证的服务认证会话对象
        /// </summary>
        /// <returns></returns>
        internal TimestampVerifySession CreateITimestampVerifySession()
        {
            if (TimestampVerifySession == null) TimestampVerifySession = new TimestampVerifySession(Socket);
            return TimestampVerifySession;
        }
    }
}
