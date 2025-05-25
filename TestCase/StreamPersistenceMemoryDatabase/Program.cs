using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            await AutoCSer.Common.Config.AppendRemoteTypeAsync(typeof(TestClass));
            await AutoCSer.Common.Config.AppendRemoteTypeAsync(typeof(TestClassMessage));
            await AutoCSer.Common.Config.AppendRemoteTypeAsync(typeof(PerformanceMessage));

            try
            {
                CommandServerConfig commandServerConfig = AutoCSer.TestCase.Common.JsonFileConfig.Default.IsCompressConfig
                    ? new CommandServerCompressConfig
                    {
                        Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.StreamPersistenceMemoryDatabase, string.Empty),
                    }
                    : new CommandServerConfig
                    {
                        Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.StreamPersistenceMemoryDatabase, string.Empty),
                    };
                ServiceConfig databaseServiceConfig = new ServiceConfig
                {
                    PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase)),
                    PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase) + nameof(ServiceConfig.PersistenceSwitchPath)),
                    CanCreateSlave = true
                };
                await using (CommandListener commandListener = new CommandListenerBuilder(0)
                    .Append<AutoCSer.CommandService.ITimestampVerifyService>(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                    .Append<IStreamPersistenceMemoryDatabaseService>(databaseServiceConfig.Create<ICustomServiceNode>(p => new CustomServiceNode(p)))
                    //.Append<IReadWriteQueueService>(databaseServiceConfig.Create<ICustomServiceNode>(p => new CustomServiceNode(p)))
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
                ConsoleWriteQueue.Breakpoint(exception.ToString());
                Console.ReadLine();
            }
        }
    }
}
