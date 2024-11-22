using System;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ReverseLogCollectionClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AutoCSer.TestCase.ReverseLogCollectionCommon.ServiceRegistryCommandClientConfig commandClientConfig = new AutoCSer.TestCase.ReverseLogCollectionCommon.ServiceRegistryCommandClientConfig { Host = new Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ServiceRegistry), ServiceName = AutoCSer.TestCase.ReverseLogCollectionCommon.LogInfo.ServiceName };
            using (ReverseLogCollectionClientServiceRegistrar serviceRegistrar = await ReverseLogCollectionClientServiceRegistrar.Create(commandClientConfig))
            {
                Console.WriteLine("Press quit to exit.");
                while (Console.ReadLine() != "quit") ;
            }
        }
    }
}
