using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 关键字索引节点接口
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    /// <typeparam name="VT">数据关键字类型</typeparam>
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true, IsMethodParameterCreator = true)]
    public partial interface IIndexNode<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
    {
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(BinarySerializeKeyValue<KT, IndexData<VT>> value);
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void AppendLoadPersistence(KT key, VT value);
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Append(KT key, VT value);
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <param name="value">匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void AppendArrayLoadPersistence(KT[] keys, VT value);
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <param name="value">匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void AppendArray(KT[] keys, VT value);
        /// <summary>
        /// 移除匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void RemoveLoadPersistence(KT key, VT value);
        /// <summary>
        /// 移除匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Remove(KT key, VT value);
        /// <summary>
        /// 移除匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <param name="value">匹配数据关键字</param>
        void RemoveArrayLoadPersistence(KT[] keys, VT value);
        /// <summary>
        /// 移除匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        void RemoveArray(KT[] keys, VT value);
        /// <summary>
        /// 磁盘块索引信息写入完成
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="blockIndex"></param>
        /// <param name="valueCount"></param>
        /// <param name="version"></param>
        void CompletedLoadPersistence(KT key, BlockIndex blockIndex, int valueCount, int version);
        /// <summary>
        /// 磁盘块索引信息写入完成
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="blockIndex"></param>
        /// <param name="valueCount"></param>
        /// <param name="version"></param>
        [ServerMethod(IsClientCall = false)]
        void Completed(KT key, BlockIndex blockIndex, int valueCount, int version);
        /// <summary>
        /// 获取索引数据
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        IndexData<VT> GetData(KT key);
    }
}
