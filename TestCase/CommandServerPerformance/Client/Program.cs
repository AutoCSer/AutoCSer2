using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.CommandClientPerformance
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            do
            {
                await AwaiterClient.Test();
                Console.WriteLine();
                await CallbackClient.Test();
                Console.WriteLine();
                await SynchronousCllient.Test();

                Console.WriteLine("Press quit to exit.");
                if (Console.ReadLine() == "quit") return;
            }
            while (true);
        }
    }
}
