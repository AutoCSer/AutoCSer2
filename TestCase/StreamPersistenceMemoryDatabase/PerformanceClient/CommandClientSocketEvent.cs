using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance
{
    /// <summary>
    /// Command client socket events
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<CommandClientSocketEvent>, IStreamPersistenceMemoryDatabaseClientSocketEvent
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
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
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
        public CommandClientSocketEvent(CommandClient client) : base(client) { }

        /// <summary>
        /// 日志流持久化内存数据库客户端
        /// </summary>
        internal static readonly AutoCSer.Net.CommandClient CommandClient = new AutoCSer.Net.CommandClient(AutoCSer.TestCase.Common.JsonFileConfig.Default.IsCompressConfig
            ? new AutoCSer.Net.CommandClientCompressConfig
            {
                Host = AutoCSer.TestCase.Common.JsonFileConfig.Default.GetClientHostEndPoint(Common.CommandServerPortEnum.StreamPersistenceMemoryDatabase),
                GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client),
            }
            : new AutoCSer.Net.CommandClientConfig
            {
                Host = AutoCSer.TestCase.Common.JsonFileConfig.Default.GetClientHostEndPoint(Common.CommandServerPortEnum.StreamPersistenceMemoryDatabase),
                GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client),
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
    }
}
