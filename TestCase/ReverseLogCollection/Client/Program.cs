using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ReverseLogCollectionClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (ServerRegistryClusterClient Client = new ServerRegistryClusterClient())
            {
                Console.WriteLine("Press quit to exit.");
                while (await AutoCSer.Breakpoint.ReadLineDelay() != "quit") ;
            }
        }
    }
}
