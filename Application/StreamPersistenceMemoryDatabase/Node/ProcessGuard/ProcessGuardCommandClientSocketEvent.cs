using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 进程守护节点命令客户端套接字事件
    /// </summary>
    public class ProcessGuardCommandClientSocketEvent : AutoCSer.Net.CommandClientSocketEventTask<ProcessGuardCommandClientSocketEvent>, AutoCSer.CommandService.IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        ///// <summary>
        ///// 日志流持久化内存数据库客户端
        ///// </summary>
        //public readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<ProcessGuardCommandClientSocketEvent> ClientCache;
        ///// <summary>
        ///// 进程守护节点客户端
        ///// </summary>
        //public readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IProcessGuardNodeClientNode> ProcessGuardNodeCache;
        /// <summary>
        /// Log stream persistence in-memory database client interface
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
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
        /// Command client socket events
        /// 命令客户端套接字事件
        /// </summary>
        /// <param name="client">Command client</param>
        public ProcessGuardCommandClientSocketEvent(AutoCSer.Net.CommandClient client) : base(client)
        {
            //ClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<ProcessGuardCommandClientSocketEvent>(client);
            //ProcessGuardNodeCache = ClientCache.CreateNode(client => client.GetOrCreateProcessGuardNode());
        }
    }
}
