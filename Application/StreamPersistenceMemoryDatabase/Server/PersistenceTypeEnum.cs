using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 持久化类型
    /// </summary>
    public enum PersistenceTypeEnum : byte
    {
        /// <summary>
        /// 常规模式，数据库冷启动时加载历史数据恢复内存数据状态
        /// </summary>
        MemoryDatabase,
        /// <summary>
        /// 扫描存档模式，按照时间顺序加载所有历史数据，用于快速存档模式（主体数据不设置快照持久化的场景）的离线数据的迁移等操作
        /// </summary>
        ScanPersistence,
    }
}
