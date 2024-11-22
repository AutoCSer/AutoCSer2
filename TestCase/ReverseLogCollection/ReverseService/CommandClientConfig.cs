using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.LogCollectionReverseService
{
    /// <summary>
    /// 命令客户端配置
    /// </summary>
    internal sealed class CommandClientConfig : AutoCSer.CommandService.ReverseLogCollection.CommandReverseListenerConfig, IServiceRegistryCommandServerConfig
    {
        /// <summary>
        /// 获取命令客户端套接字事件（初始化时一次性调用）
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public override AutoCSer.Net.CommandClientSocketEvent GetSocketEvent(AutoCSer.Net.CommandClient client)
        {
            return new CommandClientSocketEvent(client);
        }
        /// <summary>
        /// 获取服务注册组件（初始化时一次性调用）
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public override async Task<AutoCSer.Net.CommandServiceRegistrar> GetRegistrar(AutoCSer.Net.CommandReverseListener server)
        {
            ServiceRegistryCommandClientConfig commandClientConfig = new AutoCSer.TestCase.ReverseLogCollectionCommon.ServiceRegistryCommandClientConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ServiceRegistry) };
            ServiceRegistryClient client = await ServiceRegistryClient.Get(commandClientConfig, this);
            return await ServiceRegistryCommandServiceRegistrar.Create(server, client, this, this, null) ?? await base.GetRegistrar(server);
        }
        /// <summary>
        /// 获取服务注册日志
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        ServiceRegisterLog IServiceRegistryCommandServerConfig.GetServiceRegisterLog(IPEndPoint endPoint)
        {
            return new ServiceRegisterLog(this, endPoint, ServiceRegisterOperationTypeEnum.ClusterNode);
        }
    }
}
