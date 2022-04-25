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
                        AutoCSer.Net.CommandClientReturnValue<int> version = await client.ServiceRegistryClient.GetVersion();
                        if (version.IsSuccess) Console.Write((char)(version.Value + '0'));
                        else Console.Write($"*{version.ReturnType}*");
                    }
                    else Console.Write('.');
                    await Task.Delay(100);
                }
                while (true);
            }
        }
    }
}
