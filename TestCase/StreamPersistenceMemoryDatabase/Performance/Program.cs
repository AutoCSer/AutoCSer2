using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;

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
                    PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance)),
                    PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance) + nameof(ServiceConfig.PersistenceSwitchPath)),
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
            CommandClientConfig commandClientConfig = new CommandClientConfig
            {
                Host = new HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.StreamPersistenceMemoryDatabase),
                GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client),
                CheckSeconds = 0
            };
            using (CommandClient commandClient = new CommandClient(commandClientConfig))
            {
                CommandClientSocketEvent client = (CommandClientSocketEvent)await commandClient.GetSocketEvent();
                if (client != null)
                {
                    Data.Address data = AutoCSer.RandomObject.Creator<Data.Address>.CreateNotNull();
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<IServiceNodeClientNode> clientNode = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<IServiceNodeClientNode>(client);
                    do
                    {
                        AutoCSer.TestCase.Common.ClientPerformance.Left = AutoCSer.Random.Default.Next();

                        await StringByteArrayFragmentDictionaryNode.Test(clientNode, data);
                        //StringByteArrayFragmentDictionaryNode+Server.SetBinarySerialize 8192 Concurrent Completed 11370ms 368/ms
                        //StringByteArrayFragmentDictionaryNode+Server.GetBinarySerialize 8192 Concurrent Completed 9409ms 445/ms
                        //StringByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 6956ms 602/ms
                        //StringByteArrayFragmentDictionaryNode+Server.SetJsonSerialize 8192 Concurrent Completed 11429ms 366/ms
                        //StringByteArrayFragmentDictionaryNode+Server.GetJsonSerialize 8192 Concurrent Completed 13955ms 300/ms
                        //StringByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 8334ms 503/ms

                        await IntByteArrayFragmentDictionaryNode.Test(clientNode, data);
                        //IntByteArrayFragmentDictionaryNode+Server.SetBinarySerialize 8192 Concurrent Completed 8072ms 519/ms
                        //IntByteArrayFragmentDictionaryNode+Server.GetBinarySerialize 8192 Concurrent Completed 9220ms 454/ms
                        //IntByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 4931ms 850/ms
                        //IntByteArrayFragmentDictionaryNode+Server.SetJsonSerialize 8192 Concurrent Completed 9318ms 450/ms
                        //IntByteArrayFragmentDictionaryNode+Server.GetJsonSerialize 8192 Concurrent Completed 13438ms 312/ms
                        //IntByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 4784ms 876/ms

                        Console.WriteLine("Press quit to exit.");
                        if (Console.ReadLine() == "quit") return;
                    }
                    while (true);
                }
            }
        }
    }
}
