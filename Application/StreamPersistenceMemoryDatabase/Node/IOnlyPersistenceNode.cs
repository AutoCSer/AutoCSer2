using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 仅存档节点接口（用于大量数据快速存档，不修改内存数据，也不定义快照操作）
    /// </summary>
    /// <typeparam name="T">存档数据类型</typeparam>
#if !AOT
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true)]
#endif
    public partial interface IOnlyPersistenceNode<T>
    {
        /// <summary>
        /// 加载保存数据，用于扫描存档模式
        /// </summary>
        /// <param name="value">存档数据</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void SaveLoadPersistence(T value);
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="value">存档数据</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Save(T value);
        /// <summary>
        /// 加载保存数据，用于扫描存档模式
        /// </summary>
        /// <param name="value">存档数据</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true, IsSendOnly = true)]
        void SaveSendOnlyLoadPersistence(T value);
        /// <summary>
        /// 保存数据（服务端不响应）
        /// </summary>
        /// <param name="value">存档数据</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true, IsSendOnly = true)]
        void SaveSendOnly(T value);
    }
}
