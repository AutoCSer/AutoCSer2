using AutoCSer.CommandService;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ServiceRegistry.Client
{
    /// <summary>
    /// 命令客户端配置
    /// </summary>
    internal sealed class CommandClientConfig : AutoCSer.Net.CommandClientConfig
    {
        /// <summary>
        /// 注册服务命令客户端配置
        /// </summary>
        public CommandClientConfig()
        {
            ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        }
        /// <summary>
        /// 自动启动连接
        /// </summary>
        /// <param name="client"></param>
        public override void AutoCreateSocket(CommandClient client)
        {
            if (!IsAutoSocket) return;
            if (ServiceName != null) AutoCreateSocketAsync(client).NotWait();
            else base.AutoCreateSocket(client);
        }
        /// <summary>
        /// 获取命令客户端套接字事件（初始化时一次性调用）
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public override AutoCSer.Net.CommandClientSocketEvent GetSocketEvent(CommandClient client)
        {
            return new CommandClientSocketEvent(client);
        }
        /// <summary>
        /// 获取服务注册客户端监听组件（初始化时一次性调用）
        /// </summary>
        /// <param name="commandClient"></param>
        /// <returns></returns>
        public override async Task<CommandClientServiceRegistrar> GetRegistrar(CommandClient commandClient)
        {
            ServiceRegistryCommandClientConfig commandClientConfig = new ServiceRegistryCommandClientConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ServiceRegistry),
                ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
            };
            commandClientConfig.GetSocketEventDelegate = (client) => new ServiceRegistryCommandClientSocketEvent(client, commandClientConfig, AutoCSer.TestCase.Common.Config.TimestampVerifyString);
            ServiceRegistryClient serviceRegistryClient = await ServiceRegistryClient.Get(commandClientConfig, this);
            return await ServiceRegistryCommandClientServiceRegistrar.Create(commandClient, serviceRegistryClient, this) ?? await base.GetRegistrar(commandClient);
        }
    }
}
