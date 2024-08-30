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
                StreamPersistenceMemoryDatabaseServiceConfig cacheServiceConfig = new StreamPersistenceMemoryDatabaseServiceConfig
                {
                    CommandServerSocketSessionObject = new CommandServerSocketSessionObject(),
                    PersistenceFileName = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase), StreamPersistenceMemoryDatabaseServiceConfig.DefaultPersistenceFileName),
                    CanCreateSlave = true
                };
                StreamPersistenceMemoryDatabaseService cacheService = cacheServiceConfig.Create<ICustomServiceNode>(p => new CustomServiceNode(p));
                CommandServerConfig commandServerConfig = new CommandServerConfig
                {
                    Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.StreamPersistenceMemoryDatabase),
                    SessionObject = cacheService.CommandServerSocketSessionObject
                };
                await using (CommandListener commandListener = new CommandListenerBuilder(0)
                    .Append(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                    .Append<IStreamPersistenceMemoryDatabaseService>(cacheService)
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
