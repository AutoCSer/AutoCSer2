using System;

namespace AutoCSer.TestCase.ExceptionStatistics
{
    /// <summary>
    /// Command client socket events
    /// 命令客户端套接字事件
    /// </summary>
    public static class ProcessGuardCommandClientSocketEvent
    {
        /// <summary>
        /// Log stream persistence in-memory database client singleton
        /// 日志流持久化内存数据库客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<CommandService.StreamPersistenceMemoryDatabase.ProcessGuardCommandClientSocketEvent> StreamPersistenceMemoryDatabaseClientCache = new AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientCache<CommandService.StreamPersistenceMemoryDatabase.ProcessGuardCommandClientSocketEvent>(new AutoCSer.Net.CommandClientConfig
        {
            Host = new AutoCSer.Net.HostEndPoint((ushort)AutoCSer.TestCase.Common.CommandServerPortEnum.ProcessGuard),
            GetSocketEventDelegate = (client) => new CommandService.StreamPersistenceMemoryDatabase.ProcessGuardCommandClientSocketEvent(client)
        });
        /// <summary>
        /// 进程守护节点客户端单例
        /// </summary>
        public static readonly AutoCSer.CommandService.StreamPersistenceMemoryDatabaseClientNodeCache<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.IProcessGuardNodeClientNode> ProcessGuardNodeCache = StreamPersistenceMemoryDatabaseClientCache.CreateNode(client => client.GetOrCreateProcessGuardNode());
    }
}
