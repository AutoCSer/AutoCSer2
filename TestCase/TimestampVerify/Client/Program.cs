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
            CommandClientConfig commandClientConfig = new CommandClientConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.TimestampVerify),
                GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
            };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                CommandClientSocketEvent client = (CommandClientSocketEvent)await commandClient.GetSocketEvent();
                if (client == null)
                {
                    ConsoleWriteQueue.WriteLine("ERROR", ConsoleColor.Red);
                    Console.ReadKey();
                    return;
                }

                CommandClientReturnValue<int> returnValue = client.TimestampVerifyClient.Add(1, 2);
                Console.WriteLine(returnValue.ReturnType.ToString());
                Console.WriteLine(returnValue.Value.ToString());
                Console.ReadKey();
            }
        }
    }
}
