using AutoCSer.Extensions;
using AutoCSer.TestCase.Common;
using System;

namespace AutoCSer.Document.ServerRegistry.MessageNodeCluster
{
    /// <summary>
    /// 消息集群服务配置
    /// </summary>
    internal sealed class CommandServerConfig : AutoCSer.Net.CommandServerConfig
    {
        /// <summary>
        /// 服务端注册组件
        /// </summary>
        private AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistry.CommandServiceRegistrar<CommandClientSocketEvent>? registrar;
        /// <summary>
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
        /// 消息集群服务集合
        /// </summary>
        private static LeftArray<AutoCSer.Net.CommandListener> messageServers = new LeftArray<Net.CommandListener>(16);
        /// <summary>
        /// 消息集群测试服务端，每隔 1 秒增加一个新服务节点
        /// </summary>
        /// <returns></returns>
        internal static async Task Test()
        {
            do
            {
                for (ushort port = (ushort)CommandServerPortEnum.ServiceRegistryPort; port != (ushort)CommandServerPortEnum.ServiceRegistryPort + 16; ++port)
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
        /// 创建服务端节点
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        internal static async Task<AutoCSer.Net.CommandListener?> Create(ushort port)
        {
            AutoCSer.Document.ServerRegistry.MessageNodeCluster.ServiceConfig databaseServiceConfig = new AutoCSer.Document.ServerRegistry.MessageNodeCluster.ServiceConfig
            {
                PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, Path.Combine(nameof(AutoCSer.Document.ServerRegistry.MessageNodeCluster)), port.toString()),
                PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.ServerRegistry.MessageNodeCluster), nameof(AutoCSer.Document.ServerRegistry.ServiceConfig.PersistenceSwitchPath) + port.toString())
            };
            AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService databaseService = databaseServiceConfig.Create();

            CommandServerConfig commandServerConfig = new CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint(port),
                ServerName = nameof(AutoCSer.Document.ServerRegistry.MessageNodeCluster),
            };
            AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService>(databaseService)
                .CreateCommandListener(commandServerConfig);
            if (await commandListener.Start())
            {
                Console.WriteLine($"Message port {port.toString()} started.");
                return commandListener;
            }
            return null;
        }
    }
}
