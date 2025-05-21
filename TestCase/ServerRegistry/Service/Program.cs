using AutoCSer.Extensions;
using AutoCSer.TestCase.Common;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ServerRegistryService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            new Service((ushort)CommandServerPortEnum.ServiceRegistryPort).Start().NotWait();

            Console.WriteLine("Press quit to exit.");
            while (await AutoCSer.Breakpoint.ReadLineDelay() != "quit") ;
        }
    }
}
