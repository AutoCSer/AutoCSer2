using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Document.MemoryDatabaseCustomNode.Client
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
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        public CommandClientSocketEvent(AutoCSer.Net.CommandClient client) : base(client) { }

        /// <summary>
        /// Log stream persistence in-memory database client singleton
        /// 日志流持久化内存数据库客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<ICustomServiceNodeClientNode, CommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<ICustomServiceNodeClientNode, CommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
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
            Console.WriteLine($"{nameof(CounterNode)} {await CounterNode.Test()}");
            Console.WriteLine($"{nameof(DictionaryCounterNode)} {await DictionaryCounterNode.Test()}");
            Console.WriteLine($"{nameof(DictionarySnapshotCloneCounterNode)} {await DictionarySnapshotCloneCounterNode.Test()}");
            Console.WriteLine($"{nameof(BeforePersistenceNode)} {await BeforePersistenceNode.Test()}");
            Console.WriteLine($"{nameof(LoadPersistenceNode)} {await LoadPersistenceNode.Test()}");
        }
    }
}
