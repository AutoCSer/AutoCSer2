using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Binary search tree node interface
    /// 二叉搜索树节点接口
    /// </summary>
    /// <typeparam name="KT">Sort keyword type
    /// 排序关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
    /// <typeparam name="ST">Snapshot data type
    /// 快照数据类型</typeparam>
    public interface ISearchTreeDictionaryNode<KT, VT, ST>
        where KT : IComparable<KT>
    {
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotAdd(ST value);
        /// <summary>
        /// Get the number of node data
        /// 获取节点数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// Get the tree height has a time complexity of O(n)
        /// 获取树高度，时间复杂度 O(n)
        /// </summary>
        [ServerMethod(IsPersistence = false)]
        int GetHeight();
        /// <summary>
        /// Clear the data
        /// 清除数据
        /// </summary>
        void Clear();
        /// <summary>
        /// Set the data
        /// 设置数据
        /// </summary>
        /// <param name="key">keyword</param>
        /// <param name="value">data</param>
        /// <returns>Have new keywords been added
        /// 是否添加了新关键字</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Set(KT key, VT value);
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="key">keyword</param>
        /// <param name="value">data</param>
        /// <returns>Whether new data has been added
        /// 是否添加了新数据</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool TryAdd(KT key, VT value);
        /// <summary>
        /// Delete node based on keyword
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Remove(KT key);
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="keys"></param>
        /// <returns>The number of deleted keywords
        /// 删除关键字数量</returns>
        int RemoveKeys(KT[] keys);
        /// <summary>
        /// Delete node based on keyword
        /// 根据关键字删除节点
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Deleted data and no returned data indicate that the keyword does not exist
        /// 被删除数据，无返回数据表示关键字不存在</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ValueResult<VT> GetRemove(KT key);
        /// <summary>
        /// Determines if the keyword exists
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Whether the keyword exists
        /// 是否存在关键字</returns>
        [ServerMethod(IsPersistence = false)]
        bool ContainsKey(KT key);
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Target data
        /// 目标数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<VT> TryGetValue(KT key);
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        VT[] GetValueArray(KT[] keys);
        /// <summary>
        /// Get the matching node location based on the keyword
        /// 根据关键字获取匹配节点位置
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Returning -1 indicates a failed match
        /// 返回 -1 表示失败匹配</returns>
        [ServerMethod(IsPersistence = false)]
        int IndexOf(KT key);
        /// <summary>
        /// Get the number of nodes smaller than the specified keyword
        /// 获取比指定关键字小的节点数量
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Returning -1 indicates that the data to be matched is null
        /// 返回 -1 表示待匹配数据为 null</returns>
        [ServerMethod(IsPersistence = false)]
        int CountLess(KT key);
        /// <summary>
        /// Get the number of nodes larger than the specified keyword
        /// 获取比指定关键字大的节点数量
        /// </summary>
        /// <param name="key">keyword</param>
        /// <returns>Returning -1 indicates that the data to be matched is null
        /// 返回 -1 表示待匹配数据为 null</returns>
        [ServerMethod(IsPersistence = false)]
        int CountThan(KT key);
        /// <summary>
        /// Get data based on the node position
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">Node position
        /// 节点位置</param>
        /// <returns>data</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<ST> TryGetKeyValueByIndex(int index);
        /// <summary>
        /// Get data based on the node position
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">Node position
        /// 节点位置</param>
        /// <returns>data</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<VT> TryGetValueByIndex(int index);
        /// <summary>
        /// Get the first pair of data
        /// 获取第一对数据
        /// </summary>
        /// <returns>The first pair of data
        /// 第一对数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<ST> TryGetFirstKeyValue();
        /// <summary>
        /// Get the last pair of data
        /// 获取最后一对数据
        /// </summary>
        /// <returns>The last pair of data
        /// 最后一对数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<ST> TryGetLastKeyValue();
        /// <summary>
        /// Get the first keyword
        /// 获取第一个关键字
        /// </summary>
        /// <returns>The first keyword
        /// 第一个关键字</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<KT> TryGetFirstKey();
        /// <summary>
        /// Get the last keyword
        /// 获取最后一个关键字
        /// </summary>
        /// <returns>The last keyword
        /// 最后一个关键字</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<KT> TryGetLastKey();
        /// <summary>
        /// Get the first data
        /// 获取第一个数据
        /// </summary>
        /// <returns>The first data
        /// 第一个数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<VT> TryGetFirstValue();
        /// <summary>
        /// Get the last data
        /// 获取最后一个数据
        /// </summary>
        /// <returns>The last data
        /// 最后一个数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<VT> TryGetLastValue();
        /// <summary>
        /// Get a collection of data based on the range
        /// 根据范围获取数据集合
        /// </summary>
        /// <param name="skipCount">The number of skipped records
        /// 跳过记录数</param>
        /// <param name="getCount">The number of records to be obtained
        /// 获取记录数</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        IEnumerable<ValueResult<VT>> GetValues(int skipCount, byte getCount);
    }
    /// <summary>
    /// Binary search tree node interface
    /// 二叉搜索树节点接口
    /// </summary>
    /// <typeparam name="KT">Sort keyword type
    /// 排序关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
    [ServerNode(IsLocalClient = true, IsReturnValueNode = ServerNodeAttribute.DefaultIsReturnValueNode)]
    public partial interface ISearchTreeDictionaryNode<KT, VT> : ISearchTreeDictionaryNode<KT, VT, KeyValue<KT, VT>>
        where KT : IComparable<KT>
    {
    }
}
