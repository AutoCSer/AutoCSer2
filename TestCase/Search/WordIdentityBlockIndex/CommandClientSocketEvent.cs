﻿using AutoCSer.CommandService;
using AutoCSer.CommandService.Search;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Net;
using AutoCSer.TestCase.SearchCommon;
using AutoCSer.TestCase.SearchDataSource;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.TestCase.SearchWordIdentityBlockIndex
{
    /// <summary>
    /// 分词结果磁盘块索引信息客户端套接字事件
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<CommandClientSocketEvent>, IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// Sample interface of service authentication client based on incremental login timestamp verification
        /// 基于递增登录时间戳验证的服务认证客户端示例接口
        /// </summary>
        public ITimestampVerifyClient TimestampVerifyClient { get; private set; }
        /// <summary>
        /// Log stream persistence in-memory database client interface
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        public IStreamPersistenceMemoryDatabaseClient SearchUserStreamPersistenceMemoryDatabaseClient { get; private set; }
        /// <summary>
        /// Log stream persistence in-memory database client interface
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        public IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; private set; }
        /// <summary>
        /// The set of parameters for creating the client controller is used to create the client controller object during the initialization of the client socket, and also to automatically bind the controller properties based on the interface type of the client controller after the client socket passes the service authentication API
        /// 客户端控制器创建参数集合，用于命令客户端套接字初始化是创建客户端控制器对象，同时也用于命令客户端套接字事件在通过认证 API 之后根据客户端控制器接口类型自动绑定控制器属性
        /// </summary>
        public override IEnumerable<AutoCSer.Net.CommandClientControllerCreatorParameter> ControllerCreatorParameters
        {
            get
            {
                yield return new CommandClientControllerCreatorParameter(typeof(ITimestampVerifyService), typeof(ITimestampVerifyClient));
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(IReadWriteQueueService), typeof(IStreamPersistenceMemoryDatabaseClient), null, nameof(SearchUserStreamPersistenceMemoryDatabaseClient));
                yield return new AutoCSer.Net.CommandClientControllerCreatorParameter(typeof(IStreamPersistenceMemoryDatabaseService), typeof(IStreamPersistenceMemoryDatabaseClient), null, nameof(StreamPersistenceMemoryDatabaseClient));
            }
        }
        /// <summary>
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        public CommandClientSocketEvent(AutoCSer.Net.CommandClient client) : base(client) { }
        /// <summary>
        /// The client call the authentication API after creating a socket connection
        /// 客户端创建套接字连接以后调用认证 API
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public override Task<CommandClientReturnValue<CommandServerVerifyStateEnum>> CallVerifyMethod(CommandClientController controller)
        {
            return getCompletedTask(TimestampVerifyChecker.Verify(controller, AutoCSer.TestCase.Common.Config.TimestampVerifyString));
        }

        /// <summary>
        /// 日志流持久化内存数据库客户端
        /// </summary>
        internal static readonly AutoCSer.Net.CommandClient CommandClient = new AutoCSer.Net.CommandClient(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.SearchWordIdentityBlockIndex),
            GetSocketEventDelegate = (client) => new CommandClientSocketEvent(client)
        });
        /// <summary>
        /// Log stream persistence in-memory database client singleton
        /// 日志流持久化内存数据库客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<IServiceNodeClientNode, CommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<IServiceNodeClientNode, CommandClientSocketEvent>(CommandClient);
        /// <summary>
        /// 用户名称分词结果磁盘块索引信息节点单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IWordIdentityBlockIndexNodeClientNode<int>> UserNameNodeCache = StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateNode<IWordIdentityBlockIndexNodeClientNode<int>>(nameof(OperationDataTypeEnum.UserNameNode), client.ClientNode.CreateUserNameWordIdentityBlockIndexNode));
        /// <summary>
        /// 用户备注分词结果磁盘块索引信息节点单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<IWordIdentityBlockIndexNodeClientNode<int>> UserRemarkNodeCache = StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateNode<IWordIdentityBlockIndexNodeClientNode<int>>(nameof(OperationDataTypeEnum.UserRemarkNode), client.ClientNode.CreateUserRemarkWordIdentityBlockIndexNode));
        /// <summary>
        /// Log stream persistence in-memory database client singleton
        /// 日志流持久化内存数据库客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientCache<ISearchUserServiceNodeClientNode, CommandClientSocketEvent> SearchUserStreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ClientCache<ISearchUserServiceNodeClientNode, CommandClientSocketEvent>(CommandClient, (client) => new SearchUserCommandClientSocketEvent(client));
        /// <summary>
        /// 用户搜索非索引条件数据节点单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<ISearchUserNodeClientNode> SearchUserNodeCache = SearchUserStreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateNode<ISearchUserNodeClientNode>(nameof(OperationDataTypeEnum.SearchUserNode), client.ClientNode.CreateSearchUserNode));
    }
}
