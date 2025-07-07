using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Sort collection node interface
    /// 排序集合节点接口
    /// </summary>
    /// <typeparam name="T">Keyword type
    /// 关键字类型</typeparam>
#if !AOT
    [ServerNode(IsLocalClient = true)]
#endif
    public partial interface ISortedSetNode<T> where T : IComparable<T>
    {
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
        /// <summary>
        /// Get the minimum value
        /// 获取最小值
        /// </summary>
        /// <returns>No return value is returned when there is no data
        /// 没有数据时返回无返回值</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> GetMin();
        /// <summary>
        /// Get the maximum value
        /// 获取最大值
        /// </summary>
        /// <returns>No return value is returned when there is no data
        /// 没有数据时返回无返回值</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> GetMax();
    }
}
