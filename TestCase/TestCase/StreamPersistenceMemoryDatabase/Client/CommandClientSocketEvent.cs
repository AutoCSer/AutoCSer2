using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabase.Client
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<CommandClientSocketEvent>, AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        public AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; private set; }
        /// <summary>
        /// 日志流持久化内存数据库客户端接口（支持并发读取操作）
        /// </summary>
        public AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseReadWriteQueueClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
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
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
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
        /// 日志流持久化内存数据库客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode, CommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode, CommandClientSocketEvent>(CommandClient);
        /// <summary>
        /// 日志流持久化内存数据库客户端单例（支持并发读取操作）
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode, CommandClientSocketEvent> StreamPersistenceMemoryDatabaseReadWriteQueueClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode, CommandClientSocketEvent>(CommandClient, (client) => new ReadWriteQueueClientSocketEvent(client));
        /// <summary>
        /// 客户端测试
        /// </summary>
        /// <returns></returns>
        internal static async Task<bool> TestCase()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;

            if (!await DictionaryNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ByteArrayDictionaryNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await HashBytesDictionaryNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await FragmentDictionaryNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ByteArrayFragmentDictionaryNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await HashBytesFragmentDictionaryNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await MessageNode.BinaryMessageConsumer.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await MessageNode.ServerByteArrayMessageJsonConsumer.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await MessageNode.ServerByteArrayMessageConsumer.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await MessageNode.ServerByteArrayMessageStringConsumer.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await MessageNode.ServerByteArrayBinaryMessageConsumer.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }

            if (!await DistributedLockNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ManyHashBitMapClientFilterNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ManyHashBitMapFilterNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await SortedDictionaryNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await SortedSetNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await SearchTreeDictionaryNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await SearchTreeSetNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await HashSetNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await FragmentHashSetNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await IdentityGeneratorNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await QueueNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ByteArrayQueueNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await StackNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ByteArrayStackNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await ArrayNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await LeftArrayNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await SortedListNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            if (!await BitmapNode.Test())
            {
                return AutoCSer.Breakpoint.ReturnFalse();
            }
            return true;
        }
    }
}
