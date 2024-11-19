using System;
using System.Net;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 命令服务端配置
    /// </summary>
    public interface IServiceRegistryCommandServerConfig
    {
        /// <summary>
        /// 获取服务注册日志
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        ServiceRegisterLog GetServiceRegisterLog(IPEndPoint endPoint);
    }
}
