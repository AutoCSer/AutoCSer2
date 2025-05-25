using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.StreamPersistenceMemoryDatabasePerformance
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<CommandClientSocketEvent>, IStreamPersistenceMemoryDatabaseClientSocketEvent
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
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
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
        public CommandClientSocketEvent(ICommandClient client) : base(client) { }

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
        /// 日志流持久化内存数据库客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode, CommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode, CommandClientSocketEvent>(CommandClient);
        /// <summary>
        /// 日志流持久化内存数据库客户端单例（支持并发读取操作）
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode, CommandClientSocketEvent> StreamPersistenceMemoryDatabaseReadWriteQueueClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode, CommandClientSocketEvent>(CommandClient, (client) => new ReadWriteQueueClientSocketEvent(client));
    }
}
