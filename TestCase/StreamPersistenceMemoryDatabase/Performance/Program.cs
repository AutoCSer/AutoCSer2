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
            await AutoCSer.Threading.SwitchAwaiter.Default;

            try
            {
                CommandServerConfig commandServerConfig = new CommandServerConfig
                {
                    Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.StreamPersistenceMemoryDatabase),
                };
                ServiceConfig databaseServiceConfig = new ServiceConfig
                {
                    PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance)),
                    PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryFilePath, nameof(AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance) + nameof(ServiceConfig.PersistenceSwitchPath)),
                    CanCreateSlave = true
                };
                await using (CommandListener commandListener = new CommandListenerBuilder(0)
                    .Append<IStreamPersistenceMemoryDatabaseService>(databaseServiceConfig.Create())
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
                //StringByteArrayFragmentDictionaryNode+Server.SetBinarySerialize 8192 Concurrent Completed 4194304/10151ms = 413/ms
                //StringByteArrayFragmentDictionaryNode+Server.GetBinarySerialize 8192 Concurrent Completed 4194304/7611ms = 551/ms
                //StringByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 4194304/5870ms = 714/ms
                //StringByteArrayFragmentDictionaryNode+Server.SetJsonSerialize 8192 Concurrent Completed 4194304/11368ms = 368/ms
                //StringByteArrayFragmentDictionaryNode+Server.GetJsonSerialize 8192 Concurrent Completed 4194304/9808ms = 427/ms
                //StringByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 4194304/6216ms = 674/ms

                await IntByteArrayFragmentDictionaryNode.Test(data);
                //IntByteArrayFragmentDictionaryNode+Server.SetBinarySerialize 8192 Concurrent Completed 4194304/8037ms = 521/ms
                //IntByteArrayFragmentDictionaryNode+Server.GetBinarySerialize 8192 Concurrent Completed 4194304/5650ms = 742/ms
                //IntByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 4194304/4748ms = 883/ms
                //IntByteArrayFragmentDictionaryNode+Server.SetJsonSerialize 8192 Concurrent Completed 4194304/9611ms = 436/ms
                //IntByteArrayFragmentDictionaryNode+Server.GetJsonSerialize 8192 Concurrent Completed 4194304/8960ms = 468/ms
                //IntByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 4194304/4750ms = 883/ms

                await IntDictionaryNode.Test();

                Console.WriteLine("Press quit to exit.");
                if (Console.ReadLine() == "quit") return;
            }
            while (true);
        }
    }
}
