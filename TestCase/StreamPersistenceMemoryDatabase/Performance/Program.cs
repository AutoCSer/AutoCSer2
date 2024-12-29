using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance.Data;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                CommandServerConfig commandServerConfig = new CommandServerConfig
                {
                    Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.StreamPersistenceMemoryDatabase),
                };
                ServiceConfig cacheServiceConfig = new ServiceConfig
                {
                    PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance)),
                    PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance) + nameof(ServiceConfig.PersistenceSwitchPath)),
                    CanCreateSlave = true
                };
                await using (CommandListener commandListener = new CommandListenerBuilder(0)
                    .Append<IStreamPersistenceMemoryDatabaseService>(cacheServiceConfig.Create())
                    .CreateCommandListener(commandServerConfig))
                {
                    if (await commandListener.Start())
                    {
                        await client();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Console.ReadLine();
            }
        }
        private static async Task client()
        {
            do
            {
                Data.Address data = AutoCSer.RandomObject.Creator<Data.Address>.CreateNotNull();
                AutoCSer.TestCase.Common.ClientPerformance.Left = AutoCSer.Random.Default.Next();

                await StringByteArrayFragmentDictionaryNode.Test(data);
                //StringByteArrayFragmentDictionaryNode+Server.SetBinarySerialize 8192 Concurrent Completed 4194304/10131ms = 414/ms
                //StringByteArrayFragmentDictionaryNode+Server.GetBinarySerialize 8192 Concurrent Completed 4194304/10423ms = 402/ms
                //StringByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 4194304/5794ms = 723/ms
                //StringByteArrayFragmentDictionaryNode+Server.SetJsonSerialize 8192 Concurrent Completed 4194304/10833ms = 387/ms
                //StringByteArrayFragmentDictionaryNode+Server.GetJsonSerialize 8192 Concurrent Completed 4194304/13401ms = 312/ms
                //StringByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 4194304/6001ms = 698/ms

                await IntByteArrayFragmentDictionaryNode.Test(data);
                //IntByteArrayFragmentDictionaryNode+Server.SetBinarySerialize 8192 Concurrent Completed 4194304/8043ms = 521/ms
                //IntByteArrayFragmentDictionaryNode+Server.GetBinarySerialize 8192 Concurrent Completed 4194304/8646ms = 485/ms
                //IntByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 4194304/5255ms = 798/ms
                //IntByteArrayFragmentDictionaryNode+Server.SetJsonSerialize 8192 Concurrent Completed 4194304/9098ms = 461/ms
                //IntByteArrayFragmentDictionaryNode+Server.GetJsonSerialize 8192 Concurrent Completed 4194304/12383ms = 338/ms
                //IntByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 4194304/4588ms = 914/ms

                Console.WriteLine("Press quit to exit.");
                if (Console.ReadLine() == "quit") return;
            }
            while (true);
        }
    }
}
