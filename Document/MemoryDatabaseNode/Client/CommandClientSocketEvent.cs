using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Document.MemoryDatabaseNode.Client
{
    /// <summary>
    /// Command client socket event
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<CommandClientSocketEvent>, AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// Log stream persistence memory database client interface
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        [AllowNull]
        public AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; private set; }
        /// <summary>
        /// Client controller creator parameter set
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<AutoCSer.Net.CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService), typeof(AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient));
            }
        }
        /// <summary>
        /// Command client socket event
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client
        /// 命令客户端</param>
        public CommandClientSocketEvent(AutoCSer.Net.ICommandClient client) : base(client) { }

        /// <summary>
        /// Log stream persistence memory database client single example
        /// 日志流持久化内存数据库客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode, CommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode, CommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.Document),
            GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
        });
        /// <summary>
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

            Console.WriteLine($"{nameof(MessageNode.BinaryMessageConsumer)} {await MessageNode.BinaryMessageConsumer.Test()}");
            Console.WriteLine($"{nameof(MessageNode.ServerByteArrayMessageJsonConsumer)} {await MessageNode.ServerByteArrayMessageJsonConsumer.Test()}");
            Console.WriteLine($"{nameof(MessageNode.ServerByteArrayMessageConsumer)} {await MessageNode.ServerByteArrayMessageConsumer.Test()}");
            Console.WriteLine($"{nameof(MessageNode.ServerByteArrayMessageStringConsumer)} {await MessageNode.ServerByteArrayMessageStringConsumer.Test()}");
            Console.WriteLine($"{nameof(MessageNode.ServerByteArrayBinaryMessageConsumer)} {await MessageNode.ServerByteArrayBinaryMessageConsumer.Test()}");

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
        }
    }
}
