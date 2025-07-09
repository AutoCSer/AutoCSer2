using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Persistent type
    /// 持久化类型
    /// </summary>
    public enum PersistenceTypeEnum : byte
    {
        /// <summary>
        /// In the regular mode, historical data is loaded during the cold start of the database to restore the state of the in-memory data
        /// 常规模式，数据库冷启动时加载历史数据恢复内存数据状态
        /// </summary>
        MemoryDatabase,
        /// <summary>
        /// Archive-only mode (Although the persistence API can achieve the archive-only effect without operating on memory data in the regular mode, some operations need to be triggered manually)
        /// 仅存档模式（虽然常规模式下持久化 API 不操作内存数据也可以达到仅存档效果，但是某些操作需要手动触发）
        /// </summary>
        OnlyPersistence,
        /// <summary>
        /// The scanning archive mode loads all historical data in chronological order, which is used for operations such as the migration of offline data in the rapid archive mode (scenarios where the main data is not set for snapshot persistence)
        /// 扫描存档模式，按照时间顺序加载所有历史数据，用于快速存档模式（主体数据不设置快照持久化的场景）的离线数据的迁移等操作
        /// </summary>
        ScanPersistence,
    }
}
