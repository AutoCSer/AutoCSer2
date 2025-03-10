using AutoCSer.CommandService;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchDataSource.UserMessageNode
{
    /// <summary>
    /// 用户搜索数据更新消息节点客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : CommandClientSocketEventTask<CommandClientSocketEvent>, IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// 基于递增登录时间戳验证的服务认证客户端示例接口
        /// </summary>
        public ITimestampVerifyClient TimestampVerifyClient { get; private set; }
        /// <summary>
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        public IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; private set; }
        /// <summary>
        /// 客户端控制器创建器参数集合
        /// </summary>
        public override IEnumerable<CommandClientControllerCreatorParameter> ControllerCreatorParameters
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
        public CommandClientSocketEvent(ICommandClient client) : base(client) { }
        /// <summary>
        /// 客户端创建套接字连接以后调用认证 API
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override Task<CommandClientReturnValue<CommandServerVerifyStateEnum>> CallVerifyMethod(CommandClientController controller)
        {
            return getCompletedTask(TimestampVerifyChecker.Verify(controller, Common.Config.TimestampVerifyString));
        }

        /// <summary>
        /// 日志流持久化内存数据库客户端单例
        /// </summary>
        public static readonly StreamPersistenceMemoryDatabaseClientCache<ITimeoutMessageServiceNodeClientNode, CommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new StreamPersistenceMemoryDatabaseClientCache<ITimeoutMessageServiceNodeClientNode, CommandClientSocketEvent>(new CommandClientConfig
        {
            Host = new HostEndPoint((ushort)Common.CommandServerPortEnum.SearchDataSource),
            GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
        });
        /// <summary>
        /// 用户搜索数据更新消息节点单例
        /// </summary>
        public static readonly StreamPersistenceMemoryDatabaseClientNodeCache<ITimeoutMessageNodeClientNode<OperationData<int>>> UserMessageNodeCache = StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateNode<ITimeoutMessageNodeClientNode<OperationData<int>>>(nameof(User), (index, key, nodeInfo) => client.ClientNode.CreateSearchUserMessageNode(index, key, nodeInfo, 60)));
    }
}
