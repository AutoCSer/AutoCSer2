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
        //public readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode, ProcessGuardCommandClientSocketEvent> ClientCache;
        ///// <summary>
        ///// 进程守护节点客户端
        ///// </summary>
        //public readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IProcessGuardNodeClientNode> ProcessGuardNodeCache;
        /// <summary>
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
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
        public ProcessGuardCommandClientSocketEvent(AutoCSer.Net.ICommandClient client) : base(client)
        {
            //ClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IServiceNodeClientNode, ProcessGuardCommandClientSocketEvent>(client);
            //ProcessGuardNodeCache = ClientCache.CreateNode(client => client.GetOrCreateProcessGuardNode());
        }
    }
}
