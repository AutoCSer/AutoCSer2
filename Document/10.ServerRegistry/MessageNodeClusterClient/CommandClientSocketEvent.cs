using System;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.Document.ServerRegistry.MessageNodeClusterClient
{
    /// <summary>
    /// The RPC client instance of the message cluster
    /// 消息集群 RPC 客户端实例
    /// </summary>
    internal sealed class CommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<CommandClientSocketEvent>, AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// In-memory database client interface instance
        /// 内存数据库客户端接口实例
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
            }
        }
        /// <summary>
        /// The RPC client instance of the message cluster
        /// 消息集群 RPC 客户端实例
        /// </summary>
        /// <param name="client">Command client</param>
        public CommandClientSocketEvent(AutoCSer.Net.CommandClient client) : base(client) { }
    }
}
