using AutoCSer.CommandService;
using AutoCSer.CommandService.Search;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchDiskBlockIndex
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            ServiceConfig databaseServiceConfig = new ServiceConfig
            {
                PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.SearchDiskBlockIndex)),
                PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.SearchDiskBlockIndex) + nameof(ServiceConfig.PersistenceSwitchPath))
            };
            AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService databaseService = databaseServiceConfig.Create<IDiskBlockIndexServiceNode> (p => new ServiceNode(p));

            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.SearchDiskBlockIndex),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<ITimestampVerifyService>(server => new TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append<IStreamPersistenceMemoryDatabaseService>(databaseService)
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    Console.WriteLine("Press quit to exit.");
                    while (await AutoCSer.Breakpoint.ReadLineDelay() != "quit") ;
                }
            }
        }
    }
}
