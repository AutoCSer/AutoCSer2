using AutoCSer.CommandService;
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
            try
            {
                CommandServerConfig commandServerConfig = new CommandServerConfig
                {
                    Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.StreamPersistenceMemoryDatabase),
                };
                ServiceConfig cacheServiceConfig = new ServiceConfig
                {
                    PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase)),
                    PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase) + nameof(ServiceConfig.PersistenceSwitchPath)),
                    CanCreateSlave = true
                };
                await using (CommandListener commandListener = new CommandListenerBuilder(0)
                    .Append<AutoCSer.CommandService.ITimestampVerifyService>(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                    .Append<IStreamPersistenceMemoryDatabaseService>(cacheServiceConfig.Create<ICustomServiceNode>(p => new CustomServiceNode(p)))
                    .CreateCommandListener(commandServerConfig))
                {
                    if (await commandListener.Start())
                    {
                        Console.WriteLine("Press quit to exit.");
                        while (Console.ReadLine() != "quit") ;
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
