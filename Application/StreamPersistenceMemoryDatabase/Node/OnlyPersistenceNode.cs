using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 仅存档节点（用于大量数据快速存档，不修改内存数据，也不定义快照操作）
    /// </summary>
    /// <typeparam name="T">Archive data type
    /// 存档数据类型</typeparam>
#if AOT
    public abstract class OnlyPersistenceNode<T> : IOnlyPersistenceNode<T>
#else
    public sealed class OnlyPersistenceNode<T> : IOnlyPersistenceNode<T>
#endif
    {
        /// <summary>
        /// Load the archived data for scanning the archived mode (initializing the loading of persistent data)
        /// 加载保存数据，用于扫描存档模式（初始化加载持久化数据）
        /// </summary>
        /// <param name="value">Data to be archive
        /// 待存档数据</param>
        public void SaveLoadPersistence(T value) { }
        /// <summary>
        /// Data archiving
        /// 数据存档
        /// </summary>
        /// <param name="value">Data to be archive
        /// 待存档数据</param>
        public void Save(T value) { }
        /// <summary>
        /// Load the archived data for scanning the archived mode (initializing the loading of persistent data)
        /// 加载保存数据，用于扫描存档模式（初始化加载持久化数据）
        /// </summary>
        /// <param name="value">Data to be archive
        /// 待存档数据</param>
        public void SaveSendOnlyLoadPersistence(T value) { }
        /// <summary>
        /// Data archiving (The server does not respond)
        /// 数据存档（服务端不响应）
        /// </summary>
        /// <param name="value">Data to be archive
        /// 待存档数据</param>
        public void SaveSendOnly(T value) { }
    }
}
