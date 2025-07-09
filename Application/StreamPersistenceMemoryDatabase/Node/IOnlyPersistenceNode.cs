using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Archive-only data node interface (It is used for the rapid archiving of large amounts of concurrent data without modifying memory data or defining snapshot operations)
    /// 仅存档数据节点接口（用于大量并发数据快速存档，不修改内存数据，也不定义快照操作）
    /// </summary>
    /// <typeparam name="T">Archive data type
    /// 存档数据类型</typeparam>
#if !AOT
    [ServerNode(IsLocalClient = true)]
#endif
    public partial interface IOnlyPersistenceNode<T>
    {
        /// <summary>
        /// Load the archived data for scanning the archived mode (initializing the loading of persistent data)
        /// 加载存档数据，用于扫描存档模式（初始化加载持久化数据）
        /// </summary>
        /// <param name="value">Data to be archive
        /// 待存档数据</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void SaveLoadPersistence(T value);
        /// <summary>
        /// Data archiving
        /// 数据存档
        /// </summary>
        /// <param name="value">Data to be archive
        /// 待存档数据</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Save(T value);
        /// <summary>
        /// Load the archived data for scanning the archived mode (initializing the loading of persistent data)
        /// 加载保存数据，用于扫描存档模式（初始化加载持久化数据）
        /// </summary>
        /// <param name="value">Data to be archive
        /// 待存档数据</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true, IsSendOnly = true)]
        void SaveSendOnlyLoadPersistence(T value);
        /// <summary>
        /// Data archiving (The server does not respond)
        /// 数据存档（服务端不响应）
        /// </summary>
        /// <param name="value">Data to be archive
        /// 待存档数据</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true, IsSendOnly = true)]
        void SaveSendOnly(T value);
    }
}
