using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 基分片哈希表节点
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    public sealed class FragmentHashSetNode<T> : IFragmentHashSetNode<T>, ISnapshot<T>
        where T : IEquatable<T>
    {
        /// <summary>
        /// 256 基分片 哈希表
        /// </summary>
        private readonly FragmentHashSet256<T> hashSet = new FragmentHashSet256<T>();
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
        public LeftArray<T> GetSnapshotArray()
        {
            return hashSet.GetArray();
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count() { return hashSet.Count; }
        /// <summary>
        /// 清除数据（保留分片数组）
        /// </summary>
        public void Clear()
        {
            hashSet.Clear();
        }
        /// <summary>
        /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        public void ClearArray()
        {
            hashSet.ClearArray();
        }
        /// <summary>
        /// 如果关键字不存在则添加数据 持久化参数检查
        /// </summary>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> AddBeforePersistence(T value)
        {
            if (value == null || hashSet.Contains(value)) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        public bool Add(T value)
        {
            return hashSet.Add(value);
        }
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return value != null && hashSet.Contains(value);
        }
        /// <summary>
        /// 删除关键字 持久化参数检查
        /// </summary>
        /// <param name="value"></param>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<bool> RemoveBeforePersistence(T value)
        {
            if (value == null || !hashSet.Contains(value)) return false;
            return default(ValueResult<bool>);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在关键字</returns>
        public bool Remove(T value)
        {
            return hashSet.Remove(value);
        }
    }
}
