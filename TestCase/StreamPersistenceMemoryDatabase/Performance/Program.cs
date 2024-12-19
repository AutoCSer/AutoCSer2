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
                    PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase)),
                    PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase) + nameof(ServiceConfig.PersistenceSwitchPath)),
                    CanCreateSlave = true
                };
                await using (CommandListener commandListener = new CommandListenerBuilder(0)
                    .Append<IStreamPersistenceMemoryDatabaseService>(cacheServiceConfig.Create<IServiceNode>(p => new ServiceNode(p)))
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

                        await IntByteArrayFragmentDictionaryNode.Test(clientNode, data);
                        //IntByteArrayFragmentDictionaryNode+Server.SetBinarySerialize 8192 Concurrent Completed 10714ms 391/ms
                        //IntByteArrayFragmentDictionaryNode+Server.GetBinarySerialize 8192 Concurrent Completed 14934ms 280/ms
                        //IntByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 5887ms 712/ms
                        //IntByteArrayFragmentDictionaryNode+Server.SetJsonSerialize 8192 Concurrent Completed 11534ms 363/ms
                        //IntByteArrayFragmentDictionaryNode+Server.GetJsonSerialize 8192 Concurrent Completed 21640ms 193/ms
                        //IntByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 5724ms 732/ms

                        await StringByteArrayFragmentDictionaryNode.Test(clientNode, data);
                        //StringByteArrayFragmentDictionaryNode+Server.SetBinarySerialize 8192 Concurrent Completed 15118ms 277/ms
                        //StringByteArrayFragmentDictionaryNode+Server.GetBinarySerialize 8192 Concurrent Completed 15326ms 273/ms
                        //StringByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 7192ms 583/ms
                        //StringByteArrayFragmentDictionaryNode+Server.SetJsonSerialize 8192 Concurrent Completed 13405ms 312/ms
                        //StringByteArrayFragmentDictionaryNode+Server.GetJsonSerialize 8192 Concurrent Completed 22298ms 188/ms
                        //StringByteArrayFragmentDictionaryNode+Server.Remove 8192 Concurrent Completed 7195ms 582/ms

                        Console.WriteLine("Press quit to exit.");
                        if (Console.ReadLine() == "quit") return;
                    }
                    while (true);
                }
            }
        }
    }
}
