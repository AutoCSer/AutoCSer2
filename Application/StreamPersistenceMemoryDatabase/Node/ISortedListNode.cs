using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Sort list node interface
    /// 排序列表节点接口
    /// </summary>
    /// <typeparam name="KT">Sort keyword type
    /// 排序关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
    [ServerNode(IsLocalClient = true)]
    public partial interface ISortedListNode<KT, VT>
        where KT : IComparable<KT>
    {
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotAdd(KeyValue<KT, VT> value);
        /// <summary>
        /// Get the quantity of data
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// Get the container size
        /// 获取容器大小
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetCapacity();
        /// <summary>
        /// Clear all data
        /// 清除所有数据
        /// </summary>
        void Clear();
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword already exists
        /// 返回 false 表示关键字已经存在</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool TryAdd(KT key, VT value);
        /// <summary>
        /// Determine whether the keyword exists
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool ContainsKey(KT key);
        /// <summary>
        /// To determine whether the data exists, the time complexity is O(n). It is not recommended to call (since the cached data is a serialized copy of the object, the prerequisite for determining whether the objects are equal is to implement IEquatable{VT})
        /// 判断数据是否存在，时间复杂度 O(n) 不建议调用（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool ContainsValue(VT value);
        /// <summary>
        /// Get the ranking position of the key word
        /// 获取关键字排序位置
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A negative number indicates that the keyword was not found
        /// 负数表示没有找到关键字</returns>
        [ServerMethod(IsPersistence = false)]
        int IndexOfKey(KT key);
        /// <summary>
        /// Get the first matching data sort position (since cached data is a serialized copy of the object, the equality test is done by implementing IEquatable{VT})
        /// 获取第一个匹配数据排序位置（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value"></param>
        /// <returns>A negative number indicates that no matching data was found
        /// 负数表示没有找到匹配数据</returns>
        [ServerMethod(IsPersistence = false)]
        int IndexOfValue(VT value);
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Remove(KT key);
        /// <summary>
        /// Delete the keywords and return the deleted data
        /// 删除关键字并返回被删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ValueResult<VT> GetRemove(KT key);
        /// <summary>
        /// Get data based on keywords
        /// 根据关键字获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<VT> TryGetValue(KT key);
        /// <summary>
        /// Delete the data at the specified sort index position
        /// 删除指定排序索引位置数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Returning false indicates that the index is out of range
        /// 返回 false 表示索引超出范围</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool RemoveAt(int index);
    }
}
