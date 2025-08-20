using AutoCSer.CommandService;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ServerRegistryServiceClient
{
    /// <summary>
    /// 命令客户端配置
    /// </summary>
    internal sealed class CommandClientConfig : AutoCSer.Net.CommandClientConfig
    {
        /// <summary>
        /// Automatically start the connection
        /// 自动启动连接
        /// </summary>
        /// <param name="client"></param>
        public override void AutoCreateSocket(CommandClient client)
        {
            if (IsAutoSocket) AutoCreateSocketAsync(client).AutoCSerNotWait();
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
        public override Task<CommandClientServiceRegistrar> GetRegistrar(CommandClient commandClient)
        {
            return Task.FromResult((CommandClientServiceRegistrar)new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry.CommandClientServiceRegistrar<ServerRegistryLogCommandClientSocketEvent>(commandClient, ServerRegistryLogCommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.ClientCache.SocketEvent, ServerRegistryLogCommandClientSocketEvent.ServerRegistryNodeCache));
        }
    }
}
