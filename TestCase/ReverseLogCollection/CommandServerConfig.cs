using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ReverseLogCollection
{
    /// <summary>
    /// Message cluster service configuration
    /// 消息集群服务配置
    /// </summary>
    internal sealed class CommandServerConfig : AutoCSer.Net.CommandServerConfig
    {
        /// <summary>
        /// Server registration component
        /// 服务注册组件
        /// </summary>
        private AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry.CommandServiceRegistrar<ServerRegistryCommandClientSocketEvent> registrar;
        /// <summary>
        /// Get the service registration component
        /// 获取服务注册组件
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public override Task<AutoCSer.Net.CommandServiceRegistrar> GetRegistrar(AutoCSer.Net.CommandListener server)
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
