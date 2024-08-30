using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ServiceRegistry.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ServiceRegistryCommandClientConfig commandClientConfig = new ServiceRegistryCommandClientConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ServiceRegistry),
                ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
            };
            commandClientConfig.GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client, commandClientConfig, AutoCSer.TestCase.Common.Config.TimestampVerifyString);
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                CommandClientSocket socket = await commandClient.GetSocketAsync();
                if (socket != null)
                {
                    PortRegistryClient portRegistryClient = ((CommandClientSocketEvent)commandClient.SocketEvent).PortRegistryClient;

                    Console.WriteLine("Press quit to exit.");
                    await new ServiceVersion(portRegistryClient, 0).Start();
                    while (Console.ReadLine() != "quit") ;
                }
                else
                {
                    ConsoleWriteQueue.WriteLine("端口注册服务连接失败", ConsoleColor.Red);
                    Console.ReadLine();
                }
            }
        }
    }
}
