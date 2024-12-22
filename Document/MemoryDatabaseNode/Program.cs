using AutoCSer.Extensions;

namespace AutoCSer.Document.MemoryDatabaseNode
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            AutoCSer.Document.MemoryDatabaseNode.Server.ServiceConfig cacheServiceConfig = new AutoCSer.Document.MemoryDatabaseNode.Server.ServiceConfig
            {
                PersistencePath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(AutoCSer.Document.MemoryDatabaseNode)),
                PersistenceSwitchPath = Path.Combine(AutoCSer.TestCase.Common.Config.AutoCSerTemporaryPath, nameof(AutoCSer.Document.MemoryDatabaseNode) + nameof(AutoCSer.Document.MemoryDatabaseNode.Server.ServiceConfig.PersistenceSwitchPath))
            };
            AutoCSer.CommandService.StreamPersistenceMemoryDatabaseService databaseService = cacheServiceConfig.Create();

            AutoCSer.Net.CommandServerConfig commandServerConfig = new AutoCSer.Net.CommandServerConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            };
            await using (AutoCSer.Net.CommandListener commandListener = new AutoCSer.Net.CommandListenerBuilder(0)
                .Append<AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService>(databaseService)
                .CreateCommandListener(commandServerConfig))
            {
                if (await commandListener.Start())
                {
                    await client();

                    Console.WriteLine("Press quit to exit.");
                    while (Console.ReadLine() != "quit") ;
                }
            }
        }
        /// <summary>
        /// 客户端测试
        /// </summary>
        /// <returns></returns>
        private static async Task client()
        {
            AutoCSer.Net.CommandClientConfig commandClientConfig = new AutoCSer.Net.CommandClientConfig
            {
                Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
                GetSocketEventDelegate = (client) => new AutoCSer.Document.MemoryDatabaseNode.Client.CommandClientSocketEvent(client)
            };
            using (AutoCSer.Net.CommandClient commandClient = new AutoCSer.Net.CommandClient(commandClientConfig))
            {
                var socketEvent = (AutoCSer.Document.MemoryDatabaseNode.Client.CommandClientSocketEvent?)await commandClient.GetSocketEvent();
                if (socketEvent != null)
                {
                    AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode> clientNode = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClient<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode>(socketEvent);
                    Console.WriteLine($"{nameof(Client.DictionaryNode)} {await Client.DictionaryNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.ByteArrayDictionaryNode)} {await Client.ByteArrayDictionaryNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.HashBytesDictionaryNode)} {await Client.HashBytesDictionaryNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.FragmentDictionaryNode)} {await Client.FragmentDictionaryNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.ByteArrayFragmentDictionaryNode)} {await Client.ByteArrayFragmentDictionaryNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.HashBytesFragmentDictionaryNode)} {await Client.HashBytesFragmentDictionaryNode.Test(clientNode)}");

                    Console.WriteLine($"{nameof(Client.MessageNode.BinaryMessageConsumer)} {await Client.MessageNode.BinaryMessageConsumer.Test(commandClient, clientNode)}");
                    Console.WriteLine($"{nameof(Client.MessageNode.ServerByteArrayMessageJsonConsumer)} {await Client.MessageNode.ServerByteArrayMessageJsonConsumer.Test(commandClient, clientNode)}");
                    Console.WriteLine($"{nameof(Client.MessageNode.ServerByteArrayMessageConsumer)} {await Client.MessageNode.ServerByteArrayMessageConsumer.Test(commandClient, clientNode)}");
                    Console.WriteLine($"{nameof(Client.MessageNode.ServerByteArrayMessageStringConsumer)} {await Client.MessageNode.ServerByteArrayMessageStringConsumer.Test(commandClient, clientNode)}");
                    Console.WriteLine($"{nameof(Client.MessageNode.ServerByteArrayBinaryMessageConsumer)} {await Client.MessageNode.ServerByteArrayBinaryMessageConsumer.Test(commandClient, clientNode)}");

                    Console.WriteLine($"{nameof(Client.DistributedLockNode)} {await Client.DistributedLockNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.SortedDictionaryNode)} {await Client.SortedDictionaryNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.SortedSetNode)} {await Client.SortedSetNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.SearchTreeDictionaryNode)} {await Client.SearchTreeDictionaryNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.SearchTreeSetNode)} {await Client.SearchTreeSetNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.HashSetNode)} {await Client.HashSetNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.FragmentHashSetNode)} {await Client.FragmentHashSetNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.IdentityGeneratorNode)} {await Client.IdentityGeneratorNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.QueueNode)} {await Client.QueueNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.ByteArrayQueueNode)} {await Client.ByteArrayQueueNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.StackNode)} {await Client.StackNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.ByteArrayStackNode)} {await Client.ByteArrayStackNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.ArrayNode)} {await Client.ArrayNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.LeftArrayNode)} {await Client.LeftArrayNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.SortedListNode)} {await Client.SortedListNode.Test(clientNode)}");
                    Console.WriteLine($"{nameof(Client.BitmapNode)} {await Client.BitmapNode.Test(clientNode)}");
                }
            }
        }
    }
}
