using AutoCSer.CommandService;
using AutoCSer.CommandService.DiskBlock;
using AutoCSer.Net;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DiskBlock
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                FileBlockServiceConfig fileBlockServiceConfig = new FileBlockServiceConfig 
                {
                    Identity = 1,
                    Path = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.CommandService.DiskBlock))
                };
                DiskBlockService diskBlockService = await fileBlockServiceConfig.CreateFileBlockService();
                CommandServerConfig commandServerConfig = new CommandServerConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.DiskBlock) };
                await using (CommandListener commandListener = new CommandListenerBuilder(0)
                    .Append<AutoCSer.CommandService.ITimestampVerifyService>(server => new AutoCSer.CommandService.TimestampVerifyService(server, AutoCSer.TestCase.Common.Config.TimestampVerifyString))
                    .Append<IDiskBlockService>(diskBlockService)
                    .CreateCommandListener(commandServerConfig))
                {
                    if (await commandListener.Start())
                    {
                        Console.WriteLine("Press quit to exit.");
                        while (Console.ReadLine() != "quit") ;
                    }
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
                Console.ReadLine();
            }
        }
    }
}
