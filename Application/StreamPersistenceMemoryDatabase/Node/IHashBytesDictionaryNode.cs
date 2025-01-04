using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 字典节点接口
    /// </summary>
    [ServerNode(IsAutoMethodIndex = false)]
    public partial interface IHashBytesDictionaryNode
    {
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
#if NetStandard21
        void SnapshotAdd(KeyValue<byte[], byte[]?> value);
#else
        void SnapshotAdd(KeyValue<byte[], byte[]> value);
#endif
        /// <summary>
        /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        /// <param name="capacity">新容器初始化大小</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Renew(int capacity = 0);
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// 尝试添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool TryAdd(ServerByteArray key, ServerByteArray value);
        /// <summary>
        /// 强制设置数据，如果关键字已存在则覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>是否设置成功</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Set(ServerByteArray key, ServerByteArray value);
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
#if NetStandard21
        ValueResult<byte[]?> TryGetValue(ServerByteArray key);
#else
        ValueResult<byte[]> TryGetValue(ServerByteArray key);
#endif
        /// <summary>
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        ResponseParameter TryGetResponseParameter(ServerByteArray key);
        /// <summary>
        /// 清除所有数据
        /// </summary>
        void Clear();
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool ContainsKey(ServerByteArray key);
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否删除成功</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Remove(ServerByteArray key);
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>被删除数据</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
#if NetStandard21
        ValueResult<byte[]?> GetRemove(ServerByteArray key);
#else
        ValueResult<byte[]> GetRemove(ServerByteArray key);
#endif
        /// <summary>
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>被删除数据</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ResponseParameter GetRemoveResponseParameter(ServerByteArray key);
    }
}
