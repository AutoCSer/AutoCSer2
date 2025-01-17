using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.LogCollectionReverseClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServerRegistryClusterClient Client = new ServerRegistryClusterClient())
            {
                Console.WriteLine("Press quit to exit.");
                while (Console.ReadLine() != "quit") ;
            }
        }
    }
}
