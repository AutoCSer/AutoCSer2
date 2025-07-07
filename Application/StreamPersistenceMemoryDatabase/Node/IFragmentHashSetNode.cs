namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 base fragment hash table node interface
    /// 256 基分片 哈希表 节点接口
    /// </summary>
    /// <typeparam name="T">Keyword type
    /// 关键字类型</typeparam>
#if !AOT
    [ServerNode(IsLocalClient = true)]
#endif
    public partial interface IFragmentHashSetNode<T>
    {
        /// <summary>
        /// Get the quantity of data
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// Clear the data (retain the fragmented array)
        /// 清除数据（保留分片数组）
        /// </summary>
        void Clear();
        /// <summary>
        /// Reusable hash tables reset data locations (The presence of reference type data can cause memory leaks)
        /// 可重用哈希表重置数据位置（存在引用类型数据会造成内存泄露）
        /// </summary>
        void ReusableClear();
        /// <summary>
        /// Clear fragmented array (used to solve the problem of low performance of clear call when the amount of data is large)
        /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        void ClearArray();
        /// <summary>
        /// If the keyword does not exist, add the data
        /// 如果关键字不存在则添加数据
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