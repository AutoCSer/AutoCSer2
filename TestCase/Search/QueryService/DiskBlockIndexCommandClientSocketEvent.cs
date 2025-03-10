using AutoCSer.CommandService;
using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.Net;
using AutoCSer.TestCase.SearchCommon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchQueryService
{
    /// <summary>
    /// 索引节点客户端套接字事件
    /// </summary>
    internal sealed class DiskBlockIndexCommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<DiskBlockIndexCommandClientSocketEvent>, IStreamPersistenceMemoryDatabaseClientSocketEvent
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
        public override IEnumerable<AutoCSer.Net.CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(IStreamPersistenceMemoryDatabaseService), typeof(IStreamPersistenceMemoryDatabaseClient));
            }
        }
        /// <summary>
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">命令客户端</param>
        public DiskBlockIndexCommandClientSocketEvent(AutoCSer.Net.ICommandClient client) : base(client) { }
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
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<IDiskBlockIndexServiceNodeClientNode, DiskBlockIndexCommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<IDiskBlockIndexServiceNodeClientNode, DiskBlockIndexCommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.SearchDiskBlockIndex),
            GetSocketEventDelegate = (client) => new DiskBlockIndexCommandClientSocketEvent(client)
        });
        /// <summary>
        /// 用户名称索引节点单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IRemoveMarkHashKeyIndexNodeClientNode<int>> UserNameDiskBlockIndexNodeCache = StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateNode<IRemoveMarkHashKeyIndexNodeClientNode<int>>(nameof(NodeTypeEnum.UserNameNode), (index, key, nodeInfo) => client.ClientNode.CreateRemoveMarkHashKeyIndexNode(index, key, nodeInfo, typeof(int), 0)));
        /// <summary>
        /// 用户备注索引节点单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IRemoveMarkHashKeyIndexNodeClientNode<int>> UserRemarkDiskBlockIndexNodeCache = StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateNode<IRemoveMarkHashKeyIndexNodeClientNode<int>>(nameof(NodeTypeEnum.UserRemarkNode), (index, key, nodeInfo) => client.ClientNode.CreateRemoveMarkHashKeyIndexNode(index, key, nodeInfo, typeof(int), 0)));
    }
}
