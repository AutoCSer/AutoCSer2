using AutoCSer.CommandService;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ServiceRegistryService
{
    /// <summary>
    /// 服务版本测试
    /// </summary>
    internal sealed class ServiceVersion
    {
        /// <summary>
        /// 端口注册客户端
        /// </summary>
        private readonly CommandClientSocketEvent portRegistryClient;
        /// <summary>
        /// 服务版本
        /// </summary>
        private readonly uint version;
        /// <summary>
        /// 服务版本测试
        /// </summary>
        /// <param name="version"></param>
        internal ServiceVersion(CommandClientSocketEvent portRegistryClient, uint version)
        {
            this.portRegistryClient = portRegistryClient;
            this.version = version;
        }
        /// <summary>
        /// 启动测试服务
        /// </summary>
        /// <returns></returns>
        internal async Task Start()
        {
            CommandServerConfig commandServerConfig = new CommandServerConfig { Host = new HostEndPoint(0), ServiceName = "AutoCSer.TestCase.ServiceRegistry", PortRegistryClient = portRegistryClient };
            await using (CommandListener commandListener = new CommandListenerBuilder(0)
                .Append(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<IService>(new Service(version))
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    Console.WriteLine($"{commandListener.EndPoint} version {version}");
                    await Task.Delay(5 * 1000);
                    new ServiceVersion(portRegistryClient, version + 1).Start().NotWait();
                    await Task.Delay(1 * 1000);
                }
            }
        }
    }
}
