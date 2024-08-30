using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DistributedLock
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CommandServerConfig commandServerConfig = new CommandServerConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.DistributedLock) };
            await using (CommandListener commandListener = new CommandListenerBuilder(0)
                .Append(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                .Append(server => new AutoCSer.CommandService.DistributedLockService<int>())
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    Console.WriteLine("Press quit to exit.");
                    while (Console.ReadLine() != "quit") ;
                }
            }
        }
    }
}
