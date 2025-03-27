using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
    /// <summary>
    /// 带移除标记的可重用哈希索引节点接口
    /// </summary>
    /// <typeparam name="T">索引关键字类型</typeparam>
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true, IsMethodParameterCreator = true)]
    public partial interface IRemoveMarkHashKeyIndexNode<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(BinarySerializeKeyValue<T, BlockIndexData<uint>> value);
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        /// <returns>返回 false 表示关键字数据为 null</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool AppendLoadPersistence(T key, uint value);
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        /// <returns>返回 false 表示关键字数据为 null</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Append(T key, uint value);
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <param name="value">匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void AppendArrayLoadPersistence(T[] keys, uint value);
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <param name="value">匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void AppendArray(T[] keys, uint value);
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <param name="value">匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void AppendLeftArrayLoadPersistence(LeftArray<T> keys, uint value);
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <param name="value">匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void AppendLeftArray(LeftArray<T> keys, uint value);
        /// <summary>
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        /// <returns>返回 false 表示关键字数据为 null 或者没有找到索引关键字</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool RemoveLoadPersistence(T key, uint value);
        /// <summary>
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        /// <returns>返回 false 表示关键字数据为 null 或者没有找到索引关键字</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Remove(T key, uint value);
        /// <summary>
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void RemoveArrayLoadPersistence(T[] keys, uint value);
        /// <summary>
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void RemoveArray(T[] keys, uint value);
        /// <summary>
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void RemoveLeftArrayLoadPersistence(LeftArray<T> keys, uint value);
        /// <summary>
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void RemoveLeftArray(LeftArray<T> keys, uint value);
        /// <summary>
        /// 磁盘块索引信息写入完成操作
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <param name="valueCount">新增数据数量</param>
        void WriteCompletedLoadPersistence(T key, BlockIndex blockIndex, int valueCount);
        /// <summary>
        /// 磁盘块索引信息写入完成操作
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <param name="valueCount">新增数据数量</param>
        void WriteCompleted(T key, BlockIndex blockIndex, int valueCount);
        /// <summary>
        /// 获取索引数据磁盘块索引信息
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        BlockIndexData<uint> GetBlockIndexData(T key);
        /// <summary>
        /// 获取索引数据磁盘块索引信息
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        BlockIndexData<uint>[] GetBlockIndexDataArray(T[] keys);
        /// <summary>
        /// 获取索引数据磁盘块索引信息
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        BlockIndexData<int> GetIntBlockIndexData(T key);
        /// <summary>
        /// 获取索引数据磁盘块索引信息
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        BlockIndexData<int>[] GetIntBlockIndexDataArray(T[] keys);
        /// <summary>
        /// 获取更新关键字集合
        /// </summary>
        /// <param name="callback">获取更新关键字集合回调</param>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true, IsCallbackClient = true)]
        void GetChangeKeys(MethodKeepCallback<T> callback);
    }
}
