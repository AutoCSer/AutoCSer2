using AutoCSer.Extensions;
using AutoCSer.TestCase.Common;
using System;

namespace AutoCSer.Document.ServerRegistry
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            AutoCSer.Document.ServerRegistry.ServiceConfig databaseServiceConfig = new AutoCSer.Document.ServerRegistry.ServiceConfig
            {
                PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.ServerRegistry)),
                PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.ServerRegistry) + nameof(AutoCSer.Document.ServerRegistry.ServiceConfig.PersistenceSwitchPath))
            };
            AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService databaseService = databaseServiceConfig.Create();

            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ServiceRegistry),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService>(databaseService)
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    await AutoCSer.Document.ServerRegistry.MessageNodeCluster.CommandClientSocketEvent.ServerRegistryNodeCache.GetNode();
                    await AutoCSer.Document.ServerRegistry.MessageNodeClusterClient.ServerRegistryLogCommandClientSocketEvent.ServerRegistryNodeCache.GetNode();
                    await AutoCSer.Document.ServerRegistry.MessageNodeClusterClient.ServerRegistryClusterClient.Client.GetNode();

                    await using (var messageServer = await AutoCSer.Document.ServerRegistry.MessageNodeCluster.CommandServerConfig.Create((ushort)CommandServerPortEnum.ServiceRegistryPort - 1))
                    {
                        AutoCSer.Document.ServerRegistry.MessageNodeCluster.CommandServerConfig.Test().NotWait();
                        AutoCSer.Document.ServerRegistry.MessageNodeClusterClient.ServerRegistryClusterClient.Test().NotWait();

                        Console.WriteLine("Press quit to exit.");
                        while (Console.ReadLine() != "quit") ;
                    }
                }
            }
        }
    }
}
