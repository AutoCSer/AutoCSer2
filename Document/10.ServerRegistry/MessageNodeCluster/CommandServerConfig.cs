using AutoCSer.Extensions;
using System;

namespace AutoCSer.Document.ServerRegistry.MessageNodeCluster
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
        private AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry.CommandServiceRegistrar<CommandClientSocketEvent>? registrar;
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
                registrar = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry.CommandServiceRegistrar<CommandClientSocketEvent>(server, CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.ClientCache, CommandClientSocketEvent.ServerRegistryNodeCache, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryOperationTypeEnum.ClusterNode);
                return CommandClientSocketEvent.StreamPersistenceMemoryDatabaseClientCache.ClientCache.SocketEvent.Append(registrar);
            }
            return Task.FromResult((AutoCSer.Net.CommandServiceRegistrar)registrar);
        }

        /// <summary>
        /// The message cluster command server listens for the collection
        /// 消息集群命令服务端监听集合
        /// </summary>
        private static LeftArray<AutoCSer.Net.CommandListener> messageServers = new LeftArray<AutoCSer.Net.CommandListener>(16);
        /// <summary>
        /// Message cluster testing: Add a new server node every one second
        /// 消息集群测试，每隔 1 秒增加一个新服务端节点
        /// </summary>
        /// <returns></returns>
        internal static async Task Test()
        {
            do
            {
                for (ushort port = (ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ServiceRegistryPort; port != (ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ServiceRegistryPort + 16; ++port)
                {
                    var nextServer = await Create(port);
                    if (nextServer != null) messageServers.Add(nextServer);
                    await Task.Delay(1000);
                }
                foreach (AutoCSer.Net.CommandListener nextServer in messageServers) await nextServer.DisposeServiceRegistrar();
                messageServers.Clear();
            }
            while (true);
        }
        /// <summary>
        /// Create a server node
        /// 创建服务端节点
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        internal static async Task<AutoCSer.Net.CommandListener?> Create(ushort port)
        {
            AutoCSer.Document.ServerRegistry.MessageNodeCluster.ServiceConfig databaseServiceConfig = new AutoCSer.Document.ServerRegistry.MessageNodeCluster.ServiceConfig
            {
                PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, Path.Combine(nameof(AutoCSer.Document.ServerRegistry.MessageNodeCluster)), port.AutoCSerExtensions().ToString()),
                PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.ServerRegistry.MessageNodeCluster), nameof(AutoCSer.Document.ServerRegistry.ServiceConfig.PersistenceSwitchPath) + port.AutoCSerExtensions().ToString())
            };
            AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService databaseService = databaseServiceConfig.Create();

            CommandServerConfig commandServerConfig = new CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint(port),
                ServerName = nameof(AutoCSer.Document.ServerRegistry.MessageNodeCluster),
            };
            AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                //The production environment needs to add a service authentication API to prevent illegal client access
                //生产环境需要增加服务认证 API 以防止非法客户端访问
                .Append<AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService>(databaseService)
                .CreateCommandListener(commandServerConfig);
            if (await commandListener.Start())
            {
                Console.WriteLine($"Message port {port.AutoCSerExtensions().ToString()} started.");
                return commandListener;
            }
            return null;
        }
    }
}
