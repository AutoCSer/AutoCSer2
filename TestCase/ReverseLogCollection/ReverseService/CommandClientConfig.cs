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
    internal sealed class CommandClientConfig : AutoCSer.CommandService.ReverseLogCollection.CommandReverseListenerConfig
    {
        /// <summary>
        /// Server registration component
        /// 服务注册组件
        /// </summary>
        private AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry.CommandServiceRegistrar<ServerRegistryCommandClientSocketEvent> registrar;
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
        public override Task<AutoCSer.Net.CommandServiceRegistrar> GetRegistrar(AutoCSer.Net.CommandReverseListener server)
        {
            if (registrar == null)
            {
                registrar = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry.CommandServiceRegistrar<ServerRegistryCommandClientSocketEvent>(server, ServerRegistryCommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.ClientCache, ServerRegistryCommandClientSocketEvent.ServerRegistryNodeCache, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryOperationTypeEnum.ClusterNode);
                return ServerRegistryCommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.ClientCache.SocketEvent.Append(registrar);
            }
            return Task.FromResult((AutoCSer.Net.CommandServiceRegistrar)registrar);
        }
    }
}
