using AutoCSer.TestCase.ReverseLogCollection;
using AutoCSer.TestCase.ReverseLogCollectionCommon;
using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ReverseLogCollectionClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ServiceRegistryCommandClientConfig commandClientConfig = new ServiceRegistryCommandClientConfig { Host = new Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPort.ServiceRegistry), ServiceName = LogInfo.ServiceName };
            using (ReverseLogCollectionClientServiceRegistrar serviceRegistrar = await ReverseLogCollectionClientServiceRegistrar.Create(commandClientConfig))
            {
                Console.WriteLine("Press quit to exit.");
                while (Console.ReadLine() != "quit") ;
            }
        }
    }
}
