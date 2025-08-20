using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Hash table node interface
    /// 哈希表节点接口
    /// </summary>
    /// <typeparam name="T">Keyword type
    /// 关键字类型</typeparam>
    [ServerNode(IsLocalClient = true)]
    public partial interface IHashSetNode<T> where T : IEquatable<T>
    {
        /// <summary>
        /// Clear all data and rebuild the container (to solve the problem of low performance of the clear call when the data volume is large)
        /// 清除所有数据并重建容器（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Renew(int capacity);
        /// <summary>
        /// Get the quantity of data
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword already exists
        /// 返回 false 表示关键字已经存在</returns>
        [ServerMethod(SnapshotMethodSort = 1, IsIgnorePersistenceCallbackException = true)]
        bool Add(T value);
        /// <summary>
        /// If the keyword does not exist, add the data
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="values"></param>
        /// <returns>The quantity of the added data
        /// 添加数据数量</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        int AddValues(T[] values);
        /// <summary>
        /// Clear all data
        /// 清除所有数据
        /// </summary>
        void Clear();
        /// <summary>
        /// Reusable dictionaries reset data locations (The presence of reference type data can cause memory leaks)
        /// 可重用字典重置数据位置（存在引用类型数据会造成内存泄露）
        /// </summary>
        void ReusableClear();
        /// <summary>
        /// Determine whether the keyword exists
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool Contains(T value);
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool Remove(T value);
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="values"></param>
        /// <returns>The quantity of deleted data
        /// 删除数据数量</returns>
        int RemoveValues(T[] values);
    }
}
