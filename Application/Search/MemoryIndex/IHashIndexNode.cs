using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.Search.MemoryIndex
{
    /// <summary>
    /// Hash index node interface
    /// 哈希索引节点接口
    /// </summary>
    /// <typeparam name="KT">Index keyword type
    /// 索引关键字类型</typeparam>
    /// <typeparam name="VT">Data keyword type
    /// 数据关键字类型</typeparam>
    [ServerNode(IsClient = false, IsLocalClient = true)]
    public partial interface IHashIndexNode<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
    {
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(BinarySerializeKeyValue<KT, VT[]> value);
        /// <summary>
        /// Add matching data keyword (Check the input parameters before the persistence operation)
        /// 添加匹配数据关键字（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="key">Index keyword
        /// 索引关键字</param>
        /// <param name="value">Matching data keyword
        /// 匹配数据关键字</param>
        /// <returns></returns>
        ValueResult<bool> AppendBeforePersistence(KT key, VT value);
        /// <summary>
        /// Add matching data keyword
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="key">Index keyword
        /// 索引关键字</param>
        /// <param name="value">Matching data keyword
        /// 匹配数据关键字</param>
        /// <returns>Returning false indicates that the keyword data is null
        /// 返回 false 表示关键字数据为 null</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Append(KT key, VT value);
        /// <summary>
        /// Add matching data keyword
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="keys">Index keyword collection
        /// 索引关键字集合</param>
        /// <param name="value">Matching data keyword
        /// 匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void AppendArray(KT[] keys, VT value);
        /// <summary>
        /// Add matching data keyword
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="keys">Index keyword collection
        /// 索引关键字集合</param>
        /// <param name="value">Matching data keyword
        /// 匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void AppendLeftArray(LeftArray<KT> keys, VT value);
        /// <summary>
        /// Delete the matching data keyword (Check the input parameters before the persistence operation)
        /// 删除匹配数据关键字（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="key">Index keyword
        /// 索引关键字</param>
        /// <param name="value">Matching data keyword
        /// 匹配数据关键字</param>
        /// <returns></returns>
        ValueResult<bool> RemoveBeforePersistence(KT key, VT value);
        /// <summary>
        /// Delete the matching data keyword
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="key">Index keyword
        /// 索引关键字</param>
        /// <param name="value">Matching data keyword
        /// 匹配数据关键字</param>
        /// <returns>Returning false indicates that the keyword data is null or the index keyword is not found
        /// 返回 false 表示关键字数据为 null 或者没有找到索引关键字</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Remove(KT key, VT value);
        /// <summary>
        /// Delete the matching data keyword
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="keys">Index keyword collection
        /// 索引关键字集合</param>
        /// <param name="value">Matching data keyword
        /// 匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void RemoveArray(KT[] keys, VT value);
        /// <summary>
        /// Delete the matching data keyword
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="keys">Index keyword collection
        /// 索引关键字集合</param>
        /// <param name="value">Matching data keyword
        /// 匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void RemoveLeftArray(LeftArray<KT> keys, VT value);
    }
}
