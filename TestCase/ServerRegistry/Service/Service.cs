using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.Common;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ServerRegistryService
{
    /// <summary>
    /// 命令服务注册测试接口
    /// </summary>
    internal sealed class Service : IService
    {
        /// <summary>
        /// 获取当前服务端口号
        /// </summary>
        private readonly ushort port;
        /// <summary>
        /// 命令服务注册测试接口
        /// </summary>
        /// <param name="port">获取当前服务端口号</param>
        internal Service(ushort port)
        {
            this.port = port;
        }
        /// <summary>
        /// 获取当前服务端口号
        /// </summary>
        /// <returns>服务端口号</returns>
        ushort IService.GetPort() { return port; }

        /// <summary>
        /// 启动测试服务
        /// </summary>
        /// <returns></returns>
        internal async Task Start()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            CommandServerConfig commandServerConfig = new CommandServerConfig { Host = new HostEndPoint(port), ServerName = nameof(AutoCSer.TestCase.ServerRegistryService) };
            await using (CommandListener commandListener = new CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.ITimestampVerifyService>(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<IService>(this)
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    Console.WriteLine($"{commandListener.EndPoint}");
                    await Task.Delay(5 * 1000);
                    ushort port = this.port;
                    if (++port == (ushort)CommandServerPortEnum.ServiceRegistryPort + 10) port = (ushort)CommandServerPortEnum.ServiceRegistryPort;
                    new Service(port).Start().AutoCSerNotWait();
                    await Task.Delay(1 * 1000);
                }
            }
        }
    }
}
