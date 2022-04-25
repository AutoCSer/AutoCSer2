using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ServiceRegistry
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CommandServerConfig commandServerConfig = new CommandServerConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPort.ServiceRegistry) };
            using (CommandListener commandListener = new CommandListener(commandServerConfig
                , CommandServerInterfaceControllerCreator.GetCreator(server => (ITimestampVerifyService)new AutoCSer.CommandService.TimestampVerifyService(AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                , CommandServerInterfaceControllerCreator.GetCreator(server => (IServiceRegistryService)new AutoCSer.CommandService.ServiceRegistryService(server))
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
