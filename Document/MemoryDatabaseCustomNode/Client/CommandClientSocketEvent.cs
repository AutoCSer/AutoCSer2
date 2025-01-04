using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Document.MemoryDatabaseCustomNode.Client
{    /// <summary>
     /// 命令客户端套接字事件
     /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<CommandClientSocketEvent>, AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        [AllowNull]
        public AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; private set; }
        /// <summary>
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
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public CommandClientSocketEvent(AutoCSer.Net.ICommandClient client) : base(client) { }

        /// <summary>
        /// 日志流持久化内存数据库客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<ICustomServiceNodeClientNode, CommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<ICustomServiceNodeClientNode, CommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
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
            Console.WriteLine($"{nameof(CounterNode)} {await CounterNode.Test()}");
            Console.WriteLine($"{nameof(DictionaryCounterNode)} {await DictionaryCounterNode.Test()}");
            Console.WriteLine($"{nameof(DictionarySnapshotCloneCounterNode)} {await DictionarySnapshotCloneCounterNode.Test()}");
            Console.WriteLine($"{nameof(BeforePersistenceNode)} {await BeforePersistenceNode.Test()}");
        }
    }

}
