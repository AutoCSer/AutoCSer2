﻿using AutoCSer.Extensions;
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
                //The production environment needs to add a service authentication API to prevent illegal client access
                //生产环境需要增加服务认证 API 以防止非法客户端访问
                .Append<AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService>(databaseService)
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    await AutoCSer.Document.ServerRegistry.MessageNodeCluster.CommandClientSocketEvent.ServerRegistryNodeCache.GetNode();
                    await AutoCSer.Document.ServerRegistry.MessageNodeClusterClient.ServerRegistryLogCommandClientSocketEvent.ServerRegistryNodeCache.GetNode();
                    await AutoCSer.Document.ServerRegistry.MessageNodeClusterClient.ServerRegistryClusterClient.Client.GetNode();

                    await using (var messageServer = await AutoCSer.Document.ServerRegistry.MessageNodeCluster.CommandServerConfig.Create((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ServiceRegistryPort - 1))
                    {
                        AutoCSer.Document.ServerRegistry.MessageNodeCluster.CommandServerConfig.Test().Catch();
                        AutoCSer.Document.ServerRegistry.MessageNodeClusterClient.ServerRegistryClusterClient.Test().Catch();

                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
