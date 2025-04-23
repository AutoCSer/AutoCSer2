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
                await AwaiterClientPerformance.Test();
                Console.WriteLine();
                await CallbackClientPerformance.Test();
                Console.WriteLine();
                await SynchronousCllientPerformance.Test();

                Console.WriteLine("Press quit to exit.");
                if (Console.ReadLine() == "quit") break;
            }
            while (true);
#if AOT
            AutoCSer.TestCase.CommandClientPerformance.AotMethod.Call();
#endif
        }
    }
}
