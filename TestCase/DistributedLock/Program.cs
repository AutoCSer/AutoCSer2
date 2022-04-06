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
            CommandServerConfig commandServerConfig = new CommandServerConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPort.DistributedLock) };
            using (CommandListener commandListener = new CommandListener(commandServerConfig
                , CommandServerInterfaceControllerCreator.GetCreator(server => (ITimestampVerify)new AutoCSer.CommandService.TimestampVerify(AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                , CommandServerInterfaceControllerCreator.GetCreator(server => (IDistributedLock<int>)new AutoCSer.CommandService.DistributedLock<int>())
                ))
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
