using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 日志流持久化内存数据库客户端套接字事件
    /// </summary>
    public interface IStreamPersistenceMemoryDatabaseClientSocketEvent
    {
        /// <summary>
        /// 日志流持久化内存数据库客户端接口
        /// </summary>
        IStreamPersistenceMemoryDatabaseClient StreamPersistenceMemoryDatabaseClient { get; }
    }
}
