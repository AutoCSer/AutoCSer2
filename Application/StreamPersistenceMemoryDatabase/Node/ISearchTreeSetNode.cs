using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Binary search tree collection node interface
    /// 二叉搜索树集合节点接口
    /// </summary>
    /// <typeparam name="T">Keyword type
    /// 关键字类型</typeparam>
#if !AOT
    [ServerNode(IsLocalClient = true)]
#endif
    public partial interface ISearchTreeSetNode<T> where T : IComparable<T>
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
        /// <param name="value">keyword</param>
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
        /// <param name="value">keyword</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool Contains(T value);
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="value">keyword</param>
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
        /// Get the first data
        /// 获取第一个数据
        /// </summary>
        /// <returns>No return value is returned when there is no data
        /// 没有数据时返回无返回值</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> GetFrist();
        /// <summary>
        /// Get the last data
        /// 获取最后一个数据
        /// </summary>
        /// <returns>No return value is returned when there is no data
        /// 没有数据时返回无返回值</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> GetLast();
        /// <summary>
        /// Get the matching node location based on the keyword
        /// 根据关键字获取匹配节点位置
        /// </summary>
        /// <param name="value">keyword</param>
        /// <returns>Returning -1 indicates a failed match
        /// 返回 -1 表示失败匹配</returns>
        [ServerMethod(IsPersistence = false)]
        int IndexOf(T value);
        /// <summary>
        /// Get the number of nodes smaller than the specified keyword
        /// 获取比指定关键字小的节点数量
        /// </summary>
        /// <param name="value">keyword</param>
        /// <returns>Returning -1 indicates that the data to be matched is null
        /// 返回 -1 表示待匹配数据为 null</returns>
        [ServerMethod(IsPersistence = false)]
        int CountLess(T value);
        /// <summary>
        /// Get the number of nodes larger than the specified keyword
        /// 获取比指定关键字大的节点数量
        /// </summary>
        /// <param name="value">keyword</param>
        /// <returns>Returning -1 indicates that the data to be matched is null
        /// 返回 -1 表示待匹配数据为 null</returns>
        [ServerMethod(IsPersistence = false)]
        int CountThan(T value);
        /// <summary>
        /// Get data based on the node position
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">Node position
        /// 节点位置</param>
        /// <returns>data</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> GetByIndex(int index);
    }
}
