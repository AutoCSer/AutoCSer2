using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 排序集合节点接口
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    [ServerNode(MethodIndexEnumType = typeof(SortedSetNodeMethodEnum), IsAutoMethodIndex = false, IsLocalClient = true)]
    public interface ISortedSetNode<T> where T : IComparable<T>
    {
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        [ServerMethod(IsSnapshotMethod = true, IsIgnorePersistenceCallbackException = true)]
        bool Add(T value);
        /// <summary>
        /// 清除所有数据
        /// </summary>
        void Clear();
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool Contains(T value);
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否删除成功</returns>
        bool Remove(T value);
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <returns>没有数据时返回无返回值</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> GetMin();
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <returns>没有数据时返回无返回值</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> GetMax();
    }
}
