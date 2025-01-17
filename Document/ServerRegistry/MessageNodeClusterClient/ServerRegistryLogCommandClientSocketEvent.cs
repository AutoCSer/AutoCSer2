using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Document.ServerRegistry.MessageNodeClusterClient
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class ServerRegistryLogCommandClientSocketEvent : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryLogCommandClientSocketEvent<ServerRegistryLogCommandClientSocketEvent>, AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClientSocketEvent
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
        public ServerRegistryLogCommandClientSocketEvent(AutoCSer.Net.ICommandClient client) : base(client) { }

        /// <summary>
        /// 日志流持久化内存数据库客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode, ServerRegistryLogCommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode, ServerRegistryLogCommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ServiceRegistry),
            GetSocketEventDelegate = (client) => new ServerRegistryLogCommandClientSocketEvent(client)
        });
        /// <summary>
        /// 服务注册节点客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServerRegistryNodeClientNode> ServerRegistryNodeCache = StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateServerRegistryNode());
    }
}
