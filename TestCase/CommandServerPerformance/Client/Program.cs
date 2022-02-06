using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.CommandClientPerformance
{
    class Program
    {
        static async Task Main(string[] args)
        {
            do
            {
                //await CallbackClient.Test();
                await AwaiterClient.Test();
                await SynchronousCllient.Test();

                Console.WriteLine("Press quit to exit.");
                if (Console.ReadLine() == "quit") return;
            }
            while (true);
        }
    }
}
