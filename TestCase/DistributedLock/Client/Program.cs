using AutoCSer.CommandService;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.DistributedLockClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CommandClientConfig commandClientConfig = new CommandClientConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.DistributedLock),
                ControllerCreatorBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                GetSocketEventDelegate = (client) => new CommandClientSocketEventTaskClient(client)
                //GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)//IO 线程回调 await 后续操作，可以避免线程调度开销，适合后续无阻塞场景
            };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                if (await commandClient.GetSocketAsync() == null)
                {
                    ConsoleWriteQueue.Breakpoint("ERROR");
                    Console.ReadKey();
                    return;
                }

                IDistributedLockClientSocketEvent<int> client = (IDistributedLockClientSocketEvent<int>)commandClient.SocketEvent;
                for (int clientID = 0; clientID != 10; ++clientID)
                {
                    new LockClient(client, clientID).Start().NotWait();
                }

                Console.WriteLine("Press quit to exit.");
                while (Console.ReadLine() != "quit") ;
            }
        }
    }
}
