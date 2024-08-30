using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabaseBackuper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            SlaveServiceConfig slaveServiceConfig = new SlaveServiceConfig
            {
                PersistenceFileName = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(AutoCSer.TestCase.StreamPersistenceMemoryDatabaseBackuper), StreamPersistenceMemoryDatabaseServiceConfig.DefaultPersistenceFileName),
            };
            CommandClientConfig commandClientConfig = new CommandClientConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.StreamPersistenceMemoryDatabase),
                ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client, slaveServiceConfig)
            };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                if (await commandClient.GetSocketAsync() == null)
                {
                    ConsoleWriteQueue.Breakpoint("ERROR");
                    Console.ReadKey();
                    return;
                }
                using (CommandClientSocketEvent backuperClient = (CommandClientSocketEvent)commandClient.SocketEvent)
                {
                    Console.WriteLine("Press quit to exit.");
                    while (Console.ReadLine() != "quit") ;
                }
            }
        }
    }
}