using AutoCSer.CommandService;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.ReverseLogCollectionClient
{
    /// <summary>
    /// 命令客户端套接字事件
    /// </summary>
    internal sealed class ServerRegistryLogCommandClientSocketEvent : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerRegistryLogCommandClientSocketEvent<ServerRegistryLogCommandClientSocketEvent>, AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证客户端示例接口
        /// </summary>
        public ITimestampVerifyClient TimestampVerifyClient { get; private set; }
        /// <summary>
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        public AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<AutoCSer.Net.CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new CommandClientControllerCreatorParameter(typeof(IStreamPersistenceMemoryDatabaseService), typeof(IStreamPersistenceMemoryDatabaseClient));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public ServerRegistryLogCommandClientSocketEvent(AutoCSer.Net.ICommandClient client) : base(client) { }
        /// <summary>
        /// 客户端创建套接字连接以后调用认证 API
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override Task<CommandClientReturnValue<CommandServerVerifyStateEnum>> CallVerifyMethod(CommandClientController controller)
        {
            return getCompletedTask(TimestampVerifyChecker.Verify(controller, AutoCSer.TestCase.Common.Config.TimestampVerifyString));
        }

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
