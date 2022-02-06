using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.TimestampVerifyClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CommandClientConfig commandClientConfig = new CommandClientConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPort.TimestampVerify) };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                if (await commandClient.GetSocketAsync() == null)
                {
                    Console.WriteLine("ERROR");
                    Console.ReadKey();
                    return;
                }

                CommandClientSocketEvent client = (CommandClientSocketEvent)commandClient.SocketEvent;
                CommandClientReturnValue<int> returnValue = client.TimestampVerifyClient.Add(1, 2);
                Console.WriteLine(returnValue.ReturnType.ToString());
                Console.WriteLine(returnValue.Value.ToString());
                Console.ReadKey();
            }
        }
    }
}
