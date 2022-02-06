using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DistributedLockClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CommandClientConfig commandClientConfig = new CommandClientConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPort.DistributedLock) };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                if (await commandClient.GetSocketAsync() == null)
                {
                    Console.WriteLine("ERROR");
                    Console.ReadKey();
                    return;
                }

                IDistributedLockClientSocketEvent<int> client = (CommandClientSocketEvent)commandClient.SocketEvent;
                for (int clientID = 0; clientID != 10; ++clientID)
                {
                    CatchTask.AddIgnoreException(new LockClient(client, clientID).Start());
                }

                Console.WriteLine("Press quit to exit.");
                while (Console.ReadLine() != "quit") ;
            }
        }
    }
}
