using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 基分片哈希表节点
    /// </summary>
    /// <typeparam name="T">Keyword type
    /// 关键字类型</typeparam>
#if AOT
    public abstract class FragmentHashSetNode<T> : IEnumerableSnapshot<T>
#else
    public sealed class FragmentHashSetNode<T> : IFragmentHashSetNode<T>, IEnumerableSnapshot<T>
#endif
        where T : IEquatable<T>
    {
        /// <summary>
        /// 256 基分片 哈希表
        /// </summary>
        private readonly FragmentSnapshotHashSet256<T> hashSet = new FragmentSnapshotHashSet256<T>();
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<T> IEnumerableSnapshot<T>.SnapshotEnumerable { get { return hashSet.GetSnapshot(); } }
        /// <summary>
        /// Get the quantity of data
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count() { return hashSet.Count; }
        /// <summary>
        /// Clear the data (retain the fragmented array)
        /// 清除数据（保留分片数组）
        /// </summary>
        public void Clear()
        {
            hashSet.Clear();
        }
        /// <summary>
        /// Reusable hash tables reset data locations (The presence of reference type data can cause memory leaks)
        /// 可重用哈希表重置数据位置（存在引用类型数据会造成内存泄露）
        /// </summary>
        public void ReusableClear()
        {
            hashSet.ClearCount();
        }
        /// <summary>
        /// Clear fragmented array (used to solve the problem of low performance of clear call when the amount of data is large)
        /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        public void ClearArray()
        {
            hashSet.ClearArray();
        }
        /// <summary>
        /// If the keyword does not exist, add the data
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword already exists
        /// 返回 false 表示关键字已经存在</returns>
        public bool Add(T value)
        {
            return value != null && hashSet.Add(value);
        }
        /// <summary>
        /// If the keyword does not exist, add the data
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="values"></param>
        /// <returns>The quantity of the added data
        /// 添加数据数量</returns>
        public int AddValues(T[] values)
        {
            if (values != null)
            {
                int count = 0;
                foreach(T value in values)
                {
                    if (value != null && hashSet.Add(value)) ++count;
                }
                return count;
            }
            return 0;
        }
        /// <summary>
        /// Determine whether the keyword exists
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return value != null && hashSet.Contains(value);
        }
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that the keyword does not exist
        /// 返回 false 表示关键字不存在</returns>
        public bool Remove(T value)
        {
            return value != null && hashSet.Remove(value);
        }
        /// <summary>
        /// Remove keyword
        /// 删除关键字
        /// </summary>
        /// <param name="values"></param>
        /// <returns>The quantity of deleted data
        /// 删除数据数量</returns>
        public int RemoveValues(T[] values)
        {
            if (values != null)
            {
                int count = 0;
                foreach (T value in values)
                {
                    if (value != null && hashSet.Remove(value)) ++count;
                }
                return count;
            }
            return 0;
        }
    }
}
