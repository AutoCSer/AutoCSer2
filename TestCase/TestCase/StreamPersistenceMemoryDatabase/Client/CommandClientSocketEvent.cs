using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Client
{
    /// <summary>
    /// Command client socket events
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<CommandClientSocketEvent>, AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// Log stream persistence in-memory database client interface
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        public AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; private set; }
        /// <summary>
        /// Log stream persistence in-memory database client interface (Support concurrent read operations)
        /// 日志流持久化内存数据库客户端接口（支持并发读取操作）
        /// </summary>
        public AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseReadWriteQueueClient { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<AutoCSer.Net.CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseService), typeof(AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient), null, nameof(StreamPersistenceMemoryDatabaseClient));
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IReadWriteQueueService), typeof(AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient), null, nameof(StreamPersistenceMemoryDatabaseReadWriteQueueClient));
            }
        }
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        public CommandClientSocketEvent(AutoCSer.Net.ICommandClient client) : base(client) { }

        /// <summary>
        /// 日志流持久化内存数据库客户端
        /// </summary>
        internal static readonly AutoCSer.Net.CommandClient CommandClient = new AutoCSer.Net.CommandClient(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.StreamPersistenceMemoryDatabase),
            GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
        });
        /// <summary>
        /// Log stream persistence in-memory database client singleton
        /// 日志流持久化内存数据库客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<CommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<CommandClientSocketEvent>(CommandClient);
        /// <summary>
        /// Log stream persistence in-memory database client singleton (supporting concurrent read operations)
        /// 日志流持久化内存数据库客户端单例（支持并发读取操作）
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientCache<CommandClientSocketEvent> StreamPersistenceMemoryDatabaseReadWriteQueueClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientCache<CommandClientSocketEvent>(CommandClient, (client) => new ReadWriteQueueClientSocketEvent(client));
        /// <summary>
        /// The client test
        /// 客户端测试
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> TestCase()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;


            Task<bool> distributedLockNodeTask = DistributedLockNode.Test();
            Task<bool> binaryMessageConsumerTask = MessageNode.BinaryMessageConsumer.Test();
            Task<bool> serverByteArrayMessageJsonConsumerTask = MessageNode.ServerByteArrayMessageJsonConsumer.Test();
            Task<bool> serverByteArrayMessageConsumerTask = MessageNode.ServerByteArrayMessageConsumer.Test();
            Task<bool> serverByteArrayMessageStringConsumerTask = MessageNode.ServerByteArrayMessageStringConsumer.Test();
            Task<bool> serverByteArrayBinaryMessageConsumerTask = MessageNode.ServerByteArrayBinaryMessageConsumer.Test();

            Task<bool> dictionaryNodeTask = DictionaryNode.Test();
            Task<bool> byteArrayDictionaryNodeTask = ByteArrayDictionaryNode.Test();
            Task<bool> hashBytesDictionaryNodeTask = HashBytesDictionaryNode.Test();
            Task<bool> fragmentDictionaryNodeTask = FragmentDictionaryNode.Test();
            Task<bool> byteArrayFragmentDictionaryNodeTask = ByteArrayFragmentDictionaryNode.Test();
            Task<bool> hashBytesFragmentDictionaryNodeTask = HashBytesFragmentDictionaryNode.Test();

            Task<bool> manyHashBitMapClientFilterNodeTask = ManyHashBitMapClientFilterNode.Test();
            Task<bool> manyHashBitMapFilterNodeTask = ManyHashBitMapFilterNode.Test();
            Task<bool> sortedDictionaryNodeTask = SortedDictionaryNode.Test();
            Task<bool> sortedSetNodeTask = SortedSetNode.Test();
            Task<bool> searchTreeDictionaryNodeTask = SearchTreeDictionaryNode.Test();
            Task<bool> searchTreeSetNodeTask = SearchTreeSetNode.Test();
            Task<bool> hashSetNodeTask = HashSetNode.Test();
            Task<bool> fragmentHashSetNodeTask = FragmentHashSetNode.Test();
            Task<bool> identityGeneratorNodeTask = IdentityGeneratorNode.Test();
            Task<bool> queueNodeTask = QueueNode.Test();
            Task<bool> byteArrayQueueNodeTask = ByteArrayQueueNode.Test();
            Task<bool> stackNodeTask = StackNode.Test();
            Task<bool> byteArrayStackNodeTask = ByteArrayStackNode.Test();
            Task<bool> arrayNodeTask = ArrayNode.Test();
            Task<bool> leftArrayNodeTask = LeftArrayNode.Test();
            Task<bool> sortedListNodeTask = SortedListNode.Test();
            Task<bool> bitmapNodeTask = BitmapNode.Test();


            if (!await dictionaryNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await byteArrayDictionaryNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await hashBytesDictionaryNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await fragmentDictionaryNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await byteArrayFragmentDictionaryNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await hashBytesFragmentDictionaryNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await manyHashBitMapClientFilterNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await manyHashBitMapFilterNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await sortedDictionaryNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await sortedSetNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await searchTreeDictionaryNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await searchTreeSetNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await hashSetNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await fragmentHashSetNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await identityGeneratorNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await queueNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await byteArrayQueueNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await stackNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await byteArrayStackNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await arrayNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await leftArrayNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await sortedListNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await bitmapNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await binaryMessageConsumerTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await serverByteArrayMessageJsonConsumerTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await serverByteArrayMessageConsumerTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await serverByteArrayMessageStringConsumerTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await serverByteArrayBinaryMessageConsumerTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await distributedLockNodeTask)
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
