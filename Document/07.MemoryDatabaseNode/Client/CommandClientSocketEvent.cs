using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// RPC client instance
    /// RPC 客户端实例
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<CommandClientSocketEvent>, AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// In-memory database client interface instance
        /// 内存数据库客户端接口实例
        /// </summary>
        [AllowNull]
        public AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<AutoCSer.Net.CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService), typeof(AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient));
                //yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IReadWriteQueueService), typeof(AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient));
            }
        }
        /// <summary>
        /// RPC client instance
        /// RPC 客户端实例
        /// </summary>
        /// <param name="client">Command client
        /// 命令客户端</param>
        public CommandClientSocketEvent(AutoCSer.Net.CommandClient client) : base(client) { }

        /// <summary>
        /// In-memory database client singleton
        /// 内存数据库客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<CommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<CommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
        });
        /// <summary>
        /// The client test
        /// 客户端测试
        /// </summary>
        /// <returns></returns>
        internal static async Task Test()
        {
            Console.WriteLine($"{nameof(DictionaryNode)} {await DictionaryNode.Test()}");
            Console.WriteLine($"{nameof(ByteArrayDictionaryNode)} {await ByteArrayDictionaryNode.Test()}");
            Console.WriteLine($"{nameof(HashBytesDictionaryNode)} {await HashBytesDictionaryNode.Test()}");
            Console.WriteLine($"{nameof(FragmentDictionaryNode)} {await FragmentDictionaryNode.Test()}");
            Console.WriteLine($"{nameof(ByteArrayFragmentDictionaryNode)} {await ByteArrayFragmentDictionaryNode.Test()}");
            Console.WriteLine($"{nameof(HashBytesFragmentDictionaryNode)} {await HashBytesFragmentDictionaryNode.Test()}");

            Console.WriteLine($"{nameof(Message.BinaryMessageConsumer)} {await Message.BinaryMessageConsumer.Test()}");
            Console.WriteLine($"{nameof(Message.ServerByteArrayMessageJsonConsumer)} {await Message.ServerByteArrayMessageJsonConsumer.Test()}");
            Console.WriteLine($"{nameof(Message.ServerByteArrayMessageConsumer)} {await Message.ServerByteArrayMessageConsumer.Test()}");
            Console.WriteLine($"{nameof(Message.ServerByteArrayMessageStringConsumer)} {await Message.ServerByteArrayMessageStringConsumer.Test()}");
            Console.WriteLine($"{nameof(Message.ServerByteArrayBinaryMessageConsumer)} {await Message.ServerByteArrayBinaryMessageConsumer.Test()}");

            Console.WriteLine($"{nameof(DistributedLockNode)} {await DistributedLockNode.Test()}");
            Console.WriteLine($"{nameof(ManyHashBitMapClientFilterNode)} {await ManyHashBitMapClientFilterNode.Test()}");
            Console.WriteLine($"{nameof(ManyHashBitMapFilterNode)} {await ManyHashBitMapFilterNode.Test()}");
            Console.WriteLine($"{nameof(SortedDictionaryNode)} {await SortedDictionaryNode.Test()}");
            Console.WriteLine($"{nameof(SortedSetNode)} {await SortedSetNode.Test()}");
            Console.WriteLine($"{nameof(SearchTreeDictionaryNode)} {await SearchTreeDictionaryNode.Test()}");
            Console.WriteLine($"{nameof(SearchTreeSetNode)} {await SearchTreeSetNode.Test()}");
            Console.WriteLine($"{nameof(HashSetNode)} {await HashSetNode.Test()}");
            Console.WriteLine($"{nameof(FragmentHashSetNode)} {await FragmentHashSetNode.Test()}");
            Console.WriteLine($"{nameof(IdentityGeneratorNode)} {await IdentityGeneratorNode.Test()}");
            Console.WriteLine($"{nameof(QueueNode)} {await QueueNode.Test()}");
            Console.WriteLine($"{nameof(ByteArrayQueueNode)} {await ByteArrayQueueNode.Test()}");
            Console.WriteLine($"{nameof(StackNode)} {await StackNode.Test()}");
            Console.WriteLine($"{nameof(ByteArrayStackNode)} {await ByteArrayStackNode.Test()}");
            Console.WriteLine($"{nameof(ArrayNode)} {await ArrayNode.Test()}");
            Console.WriteLine($"{nameof(LeftArrayNode)} {await LeftArrayNode.Test()}");
            Console.WriteLine($"{nameof(SortedListNode)} {await SortedListNode.Test()}");
            Console.WriteLine($"{nameof(BitmapNode)} {await BitmapNode.Test()}");
            Console.WriteLine($"{nameof(OnlyPersistenceNode)} {await OnlyPersistenceNode.Test()}");
        }
    }
}
