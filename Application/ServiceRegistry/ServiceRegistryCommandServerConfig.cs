using System;
using System.Net;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 命令服务端配置
    /// </summary>
    public class ServiceRegistryCommandServerConfig : AutoCSer.Net.CommandServerConfig
    {
        /// <summary>
        /// 获取服务注册日志
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public virtual ServiceRegisterLog GetServiceRegisterLog(IPEndPoint endPoint)
        {
            return new ServiceRegisterLog(this, endPoint);
        }
    }
}
