﻿using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
    /// <summary>
    /// Node interface with reusable hash indexes with removal tags
    /// 带移除标记的可重用哈希索引节点接口
    /// </summary>
    /// <typeparam name="KT">Index keyword type
    /// 索引关键字类型</typeparam>
    /// <typeparam name="VT">Data keyword type
    /// 数据关键字类型</typeparam>
    [ServerNode(IsLocalClient = true, IsMethodParameterCreator = true)]
    public partial interface IRemoveMarkHashIndexNode<KT, VT>
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
        void SnapshotSet(BinarySerializeKeyValue<KT, BlockIndexData<VT>> value);
        /// <summary>
        /// Add matching data keyword (Initialize and load the persistent data)
        /// 添加匹配数据关键字（初始化加载持久化数据）
        /// </summary>
        /// <param name="key">Index keyword
        /// 索引关键字</param>
        /// <param name="value">Matching data keyword
        /// 匹配数据关键字</param>
        /// <returns>Returning false indicates that the keyword data is null
        /// 返回 false 表示关键字数据为 null</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool AppendLoadPersistence(KT key, VT value);
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
        /// Add matching data keyword (Initialize and load the persistent data)
        /// 添加匹配数据关键字（初始化加载持久化数据）
        /// </summary>
        /// <param name="keys">Index keyword collection
        /// 索引关键字集合</param>
        /// <param name="value">Matching data keyword
        /// 匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void AppendArrayLoadPersistence(KT[] keys, VT value);
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
        /// Add matching data keyword (Initialize and load the persistent data)
        /// 添加匹配数据关键字（初始化加载持久化数据）
        /// </summary>
        /// <param name="keys">Index keyword collection
        /// 索引关键字集合</param>
        /// <param name="value">Matching data keyword
        /// 匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void AppendLeftArrayLoadPersistence(LeftArray<KT> keys, VT value);
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
        /// Delete the matching data keyword (Initialize and load the persistent data)
        /// 删除匹配数据关键字（初始化加载持久化数据）
        /// </summary>
        /// <param name="key">Index keyword
        /// 索引关键字</param>
        /// <param name="value">Matching data keyword
        /// 匹配数据关键字</param>
        /// <returns>Returning false indicates that the keyword data is null or the index keyword is not found
        /// 返回 false 表示关键字数据为 null 或者没有找到索引关键字</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool RemoveLoadPersistence(KT key, VT value);
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
        /// Delete the matching data keyword (Initialize and load the persistent data)
        /// 删除匹配数据关键字（初始化加载持久化数据）
        /// </summary>
        /// <param name="keys">Index keyword collection
        /// 索引关键字集合</param>
        /// <param name="value">Matching data keyword
        /// 匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void RemoveArrayLoadPersistence(KT[] keys, VT value);
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
        /// Delete the matching data keyword (Initialize and load the persistent data)
        /// 删除匹配数据关键字（初始化加载持久化数据）
        /// </summary>
        /// <param name="keys">Index keyword collection
        /// 索引关键字集合</param>
        /// <param name="value">Matching data keyword
        /// 匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void RemoveLeftArrayLoadPersistence(LeftArray<KT> keys, VT value);
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
        /// <summary>
        /// The operation of writing the disk block index information has been completed (Initialize and load the persistent data)
        /// 磁盘块索引信息写入完成操作（初始化加载持久化数据）
        /// </summary>
        /// <param name="key">Index keyword
        /// 索引关键字</param>
        /// <param name="blockIndex">Disk block index information
        /// 磁盘块索引信息</param>
        /// <param name="valueCount">The number of newly added data
        /// 新增数据数量</param>
        void WriteCompletedLoadPersistence(KT key, BlockIndex blockIndex, int valueCount);
        /// <summary>
        /// The operation of writing the disk block index information has been completed
        /// 磁盘块索引信息写入完成操作
        /// </summary>
        /// <param name="key">Index keyword
        /// 索引关键字</param>
        /// <param name="blockIndex">Disk block index information
        /// 磁盘块索引信息</param>
        /// <param name="valueCount">The number of newly added data
        /// 新增数据数量</param>
        void WriteCompleted(KT key, BlockIndex blockIndex, int valueCount);
        /// <summary>
        /// Get the index information of the index data disk block
        /// 获取索引数据磁盘块索引信息
        /// </summary>
        /// <param name="key">Index keyword
        /// 索引关键字</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        BlockIndexData<VT> GetBlockIndexData(KT key);
        /// <summary>
        /// Get the index information of the index data disk block
        /// 获取索引数据磁盘块索引信息
        /// </summary>
        /// <param name="keys">Index keyword collection
        /// 索引关键字集合</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        BlockIndexData<VT>[] GetBlockIndexDataArray(KT[] keys);
        /// <summary>
        /// Gets the collection of updated keyword
        /// 获取更新关键字集合
        /// </summary>
        /// <param name="callback">The callback delegate for gets the collection of updated keywords
        /// 获取更新关键字集合回调委托</param>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true, IsCallbackClient = true)]
        void GetChangeKeys(MethodKeepCallback<KT> callback);
    }
}
