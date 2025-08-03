using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ProcessGuardClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            ResponseResult<IProcessGuardNodeClientNode> client = await CommandClientSocketEvent.ProcessGuardNodeCache.GetNode();
            if (client.IsSuccess)
            {
                guard(client.Value, args).AutoCSerNotWait();
                Console.WriteLine("Press quit to exit.");
                while (await AutoCSer.Breakpoint.ReadLineDelay() != "quit") ;
                await client.Value.RemoveCurrentProcess();
            }
            else
            {
                Console.WriteLine($"{client.ReturnType} + {client.CallState}");
                Console.ReadKey();
            }
        }
        private static async Task guard(IProcessGuardNodeClientNode client, string[] arguments)
        {
            do
            {
                ResponseResult<bool> Result = await client.GuardCurrentProcess(arguments);
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
