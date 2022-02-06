using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ServiceRegistry.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Press quit to exit.");
            await new ServiceVersion(0).Start();
            while (Console.ReadLine() != "quit") ;
        }
    }
}
