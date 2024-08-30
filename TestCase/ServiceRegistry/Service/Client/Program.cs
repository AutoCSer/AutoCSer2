using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.TestCase.ServiceRegistry.Service;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ServiceRegistry.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CommandClientConfig commandClientConfig = new CommandClientConfig { Host = new HostEndPoint(0), ServiceName = "AutoCSer.TestCase.ServiceRegistry" };
            using (CommandClient commandClient = new CommandClient(commandClientConfig
                , CommandClientInterfaceControllerCreator.GetCreator<ITimestampVerifyClient, ITimestampVerifyService>()
                , CommandClientInterfaceControllerCreator.GetCreator<IClient, IService>()
                ))
            {
                await commandClient.GetSocketAsync();
                CommandClientSocketEvent client = (CommandClientSocketEvent)commandClient.SocketEvent;
                do
                {
                    if (client.ServiceRegistryClient != null)
                    {
                        AutoCSer.Net.CommandClientReturnValue<uint> version = await client.ServiceRegistryClient.GetVersion();
                        if (version.IsSuccess)
                        {
                            Console.Write(version.Value);
                            Console.Write('.');
                        }
                        else ConsoleWriteQueue.WriteLine($"*{version.ReturnType}*", ConsoleColor.Red);
                    }
                    else Console.Write('*');
                    await Task.Delay(100);
                }
                while (true);
            }
        }
    }
}
