using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 仅存档节点（用于大量数据快速存档，不修改内存数据，也不定义快照操作）
    /// </summary>
    /// <typeparam name="T">存档数据类型</typeparam>
#if AOT
    public abstract class OnlyPersistenceNode<T> : IOnlyPersistenceNode<T>
#else
    public sealed class OnlyPersistenceNode<T> : IOnlyPersistenceNode<T>
#endif
    {
        /// <summary>
        /// 加载保存数据，用于扫描存档模式
        /// </summary>
        /// <param name="value">存档数据</param>
        public void SaveLoadPersistence(T value) { }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="value">存档数据</param>
        public void Save(T value) { }
        /// <summary>
        /// 加载保存数据，用于扫描存档模式
        /// </summary>
        /// <param name="value">存档数据</param>
        public void SaveSendOnlyLoadPersistence(T value) { }
        /// <summary>
        /// 保存数据（服务端不响应）
        /// </summary>
        /// <param name="value">存档数据</param>
        public void SaveSendOnly(T value) { }
    }
}
