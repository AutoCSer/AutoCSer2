using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ProcessGuard
{
    class Program
    {
        /// <summary>
        /// 服务端需要以管理员身份运行
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            CommandServerConfig commandServerConfig = new CommandServerConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ProcessGuard) };
            await using (CommandListener commandListener = new CommandListener(commandServerConfig
                , CommandServerInterfaceControllerCreator.GetCreator(server => new AutoCSer.CommandService.ProcessGuardService())
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
