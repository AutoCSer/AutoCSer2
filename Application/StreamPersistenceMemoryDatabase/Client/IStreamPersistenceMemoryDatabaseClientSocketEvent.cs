using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// Log stream persists in-memory database client socket events
    /// 日志流持久化内存数据库客户端套接字事件
    /// </summary>
    public interface IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// Log stream persistence in-memory database client interface
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; }
    }
}
