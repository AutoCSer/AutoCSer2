using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Dictionary node interface
    /// 字典节点接口
    /// </summary>
    [ServerNode]
    public partial interface IHashBytesDictionaryNode
    {
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
#if NetStandard21
        void SnapshotAdd(BinarySerializeKeyValue<byte[], byte[]?> value);
#else
        void SnapshotAdd(BinarySerializeKeyValue<byte[], byte[]> value);
#endif
        /// <summary>
        /// Clear all data and rebuild the container (to solve the problem of low performance of the clear call when the data volume is large)
        /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        /// <param name="capacity">Initialize the size of the new container
        /// 新容器初始化大小</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Renew(int capacity = 0);
        /// <summary>
        /// Get the quantity of data
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// Try to add data
        /// 尝试添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword already exists
        /// 返回 false 表示关键字已经存在</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool TryAdd(ServerByteArray key, ServerByteArray value);
        /// <summary>
        /// Force the data to be set and overwrite if the keyword already exists
        /// 强制设置数据，如果关键字已存在则覆盖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Return false on failure</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Set(ServerByteArray key, ServerByteArray value);
        /// <summary>
        /// Get data based on keywords
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
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        ResponseParameter TryGetResponseParameter(ServerByteArray key);
        /// <summary>
        /// Clear all data
        /// 清除所有数据
        /// </summary>
        void Clear();
        /// <summary>
        /// Determine whether the keyword exists
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool ContainsKey(ServerByteArray key);
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Remove(ServerByteArray key);
        /// <summary>
        /// Delete the keywords and return the deleted data
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Deleted data and no returned data indicate that the keyword does not exist
        /// 被删除数据，无返回数据表示关键字不存在</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
#if NetStandard21
        ValueResult<byte[]?> GetRemove(ServerByteArray key);
#else
        ValueResult<byte[]> GetRemove(ServerByteArray key);
#endif
        /// <summary>
        /// Delete the keywords and return the deleted data
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Deleted data and no returned data indicate that the keyword does not exist
        /// 被删除数据，无返回数据表示关键字不存在</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ResponseParameter GetRemoveResponseParameter(ServerByteArray key);
    }
}
