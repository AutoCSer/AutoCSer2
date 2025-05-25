using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.CommandServerPerformance
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            CommandServerConfig commandServerConfig = AutoCSer.TestCase.Common.JsonFileConfig.Default.IsCompressConfig
                ? new CommandServerCompressConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Performance, string.Empty), TaskQueueMaxConcurrent = 16 }
                : new CommandServerConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Performance, string.Empty), TaskQueueMaxConcurrent = 16 };
            await using (CommandListener commandListener = new CommandListener(commandServerConfig
                , CommandServerInterfaceControllerCreator.GetCreator<IService>(new Service())
                ))
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
