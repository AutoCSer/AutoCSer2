using AutoCSer.Extensions;
using AutoCSer.TestCase.Common;
using System;

namespace AutoCSer.TestCase.ServerRegistryService
{
    class Program
    {
        static void Main(string[] args)
        {
            new Service((ushort)CommandServerPortEnum.ServiceRegistryPort).Start().NotWait();

            Console.WriteLine("Press quit to exit.");
            while (Console.ReadLine() != "quit") ;
        }
    }
}
