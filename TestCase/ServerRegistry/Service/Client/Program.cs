using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.TestCase.Common;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ServerRegistryServiceClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            AutoCSer.Net.CommandClientSocketEventCache<CommandClientSocketEvent> commandClient = new AutoCSer.Net.CommandClientSocketEventCache<CommandClientSocketEvent>(new CommandClientConfig
            {
                Host = new HostEndPoint(0),
                ServerName = nameof(AutoCSer.TestCase.ServerRegistryService)
            });
            using (commandClient.Client)
            {
                CommandClientSocketEvent client = commandClient.SocketEvent;
                await commandClient.Client.GetSocketAsync();
                do
                {
                    if (client.ServerRegistryServiceClient != null)
                    {
                        AutoCSer.Net.CommandClientReturnValue<ushort> port = await client.ServerRegistryServiceClient.GetPort();
                        if (port.IsSuccess)
                        {
                            Console.Write(port.Value - CommandServerPortEnum.ServiceRegistryPort);
                            Console.Write('.');
                        }
                        else ConsoleWriteQueue.WriteLine($"*{port.ReturnType}*", ConsoleColor.Red);
                    }
                    else Console.Write('*');
                    await Task.Delay(1);
                }
                while (true);
            }
        }
    }
}
