using System;
using System.Threading.Tasks;
using AutoCSer.Net;
using AutoCSer.CommandService;
using System.Net;
using System.Reflection;

namespace AutoCSer.TestCase.ServiceRegistry.Service
{
    /// <summary>
    /// 命令服务端配置
    /// </summary>
    internal sealed class CommandServerConfig : ServiceRegistryCommandServerConfig
    {
        /// <summary>
        /// 端口注册客户端
        /// </summary>
        public PortRegistryClient PortRegistryClient;
        /// <summary>
        /// 获取服务注册组件（初始化时一次性调用）
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public override async Task<AutoCSer.Net.CommandServiceRegistrar> GetRegistrar(CommandListener server)
        {
            ServiceRegistryCommandClientConfig commandClientConfig = new ServiceRegistryCommandClientConfig 
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ServiceRegistry),
                ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
            };
            commandClientConfig.GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client, commandClientConfig, AutoCSer.TestCase.Common.Config.TimestampVerifyString);
            ServiceRegistryClient serviceRegistryClient = await ServiceRegistryClient.Get(commandClientConfig, this);
            return await ServiceRegistryCommandServiceRegistrar.Create(server, serviceRegistryClient, this, PortRegistryClient) ?? await base.GetRegistrar(server);
        }
        /// <summary>
        /// 获取服务注册日志
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public override ServiceRegisterLog GetServiceRegisterLog(IPEndPoint endPoint)
        {
            return new ServiceRegisterLog(this, endPoint);
        }
    }
}
