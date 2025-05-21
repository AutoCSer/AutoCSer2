using System;
using System.IO;
using System.Threading.Tasks;
using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            try
            {
                CommandServerConfig commandServerConfig = AutoCSer.TestCase.Common.Config.IsCompressConfig
                    ? new CommandServerCompressConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.StreamPersistenceMemoryDatabase, string.Empty) }
                    : new CommandServerConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.StreamPersistenceMemoryDatabase, string.Empty) };
                ServiceConfig databaseServiceConfig = new ServiceConfig
                {
                    PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance)),
                    PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance) + nameof(ServiceConfig.PersistenceSwitchPath))
                };
                ServiceConfig readWriteQueueDatabaseServiceConfig = new ServiceConfig
                {
                    PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance)) + nameof(IReadWriteQueueService),
                    PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance) + nameof(IReadWriteQueueService) + nameof(ServiceConfig.PersistenceSwitchPath))
                };
                await using (CommandListener commandListener = new CommandListenerBuilder(0)
                    .Append<IStreamPersistenceMemoryDatabaseService>(databaseServiceConfig.Create())
                    .Append<IReadWriteQueueService>(readWriteQueueDatabaseServiceConfig.Create())
                    .CreateCommandListener(commandServerConfig))
                {
                    if (await commandListener.Start())
                    {
                        Console.WriteLine("Press quit to exit.");
                        while (await AutoCSer.Breakpoint.ReadLineDelay() != "quit") ;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Console.ReadLine();
            }
        }
    }
}
