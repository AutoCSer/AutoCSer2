using AutoCSer.CommandService;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ProcessGuardClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CommandClientConfig commandClientConfig = new ProcessGuardCommandClientConfig { Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPort.ProcessGuard) };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                await commandClient.GetSocketAsync();
                ProcessGuardClientSocketEvent client = (ProcessGuardClientSocketEvent)commandClient.SocketEvent;
                Console.WriteLine("Press quit to exit.");
                CatchTask.AddIgnoreException(check(client));
                while (Console.ReadLine() != "quit") ;
                await client.RemoveGuardAsync();
            }
        }
        /// <summary>
        /// 请求守护成功后 10 秒自动退出进程
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static async Task check(ProcessGuardClientSocketEvent client)
        {
            do
            {
                CommandClientReturnValue<bool> Result = client.GuardReturnValue;
                if (Result.IsSuccess)
                {
                    if (Result.Value)
                    {
                        Console.WriteLine("Guard success ");
                        for (int count = 10; count != 0;)
                        {
                            Console.Write(--count);
                            await Task.Delay(1000);
                        }
                        Environment.Exit(0);
                        return;
                    }
                    else Console.Write('X');
                }
                else Console.Write('.');
                await Task.Delay(100);
            }
            while (true);
        }
    }
}
