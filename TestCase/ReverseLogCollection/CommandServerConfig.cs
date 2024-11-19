using System;
using System.Threading.Tasks;
using AutoCSer.Net;
using AutoCSer.CommandService;
using System.Net;

namespace AutoCSer.TestCase.ReverseLogCollection
{
    /// <summary>
    /// 命令服务端配置
    /// </summary>
    internal sealed class CommandServerConfig : ServiceRegistryCommandServerConfig
    {
        /// <summary>
        /// 获取服务注册组件（初始化时一次性调用）
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public override async Task<AutoCSer.Net.CommandServiceRegistrar> GetRegistrar(CommandListener server)
        {
            ServiceRegistryCommandClientConfig commandClientConfig = new AutoCSer.TestCase.ReverseLogCollectionCommon.ServiceRegistryCommandClientConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ServiceRegistry) };
            ServiceRegistryClient client = await ServiceRegistryClient.Get(commandClientConfig, this);
            return await ServiceRegistryCommandServiceRegistrar.Create(server, client, this, null) ?? await base.GetRegistrar(server);
        }
        /// <summary>
        /// 获取服务注册日志
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public override ServiceRegisterLog GetServiceRegisterLog(IPEndPoint endPoint)
        {
            return new ServiceRegisterLog(this, endPoint, ServiceRegisterOperationTypeEnum.ClusterNode);
        }
    }
}
