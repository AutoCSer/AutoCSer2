﻿using AutoCSer.Extensions;

namespace AutoCSer.Document.MemoryDatabaseCustomNode
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            AutoCSer.Document.MemoryDatabaseCustomNode.ServiceConfig cacheServiceConfig = new AutoCSer.Document.MemoryDatabaseCustomNode.ServiceConfig
            {
                PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseCustomNode)),
                PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.Document.MemoryDatabaseCustomNode) + nameof(AutoCSer.Document.MemoryDatabaseCustomNode.ServiceConfig.PersistenceSwitchPath))
            };
            AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService databaseService = cacheServiceConfig.Create<ICustomServiceNode>(p => new CustomServiceNode(p));

            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService>(databaseService)
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    await Client.CommandClientSocketEvent.Test();

                    Console.WriteLine("Press quit to exit.");
                    while (Console.ReadLine() != "quit") ;
                }
            }
        }
    }
}