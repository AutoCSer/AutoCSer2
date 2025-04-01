using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 基分片哈希表节点
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    public sealed class FragmentHashSetNode<T> : IFragmentHashSetNode<T>, IEnumerableSnapshot<T>
        where T : IEquatable<T>
    {
        /// <summary>
        /// 256 基分片 哈希表
        /// </summary>
        private readonly FragmentSnapshotHashSet256<T> hashSet = new FragmentSnapshotHashSet256<T>();
        /// <summary>
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<T> IEnumerableSnapshot<T>.SnapshotEnumerable { get { return hashSet.GetSnapshot(); } }
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
        /// 可重用哈希表重置数据位置（存在引用类型数据会造成内存泄露）
        /// </summary>
        public void ReusableClear()
        {
            hashSet.ClearCount();
        }
        /// <summary>
        /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        public void ClearArray()
        {
            hashSet.ClearArray();
        }
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        public bool Add(T value)
        {
            return value != null && hashSet.Add(value);
        }
        /// <summary>
        /// 如果关键字不存在则添加数据
        /// </summary>
        /// <param name="values"></param>
        /// <returns>添加数据数量</returns>
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
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return value != null && hashSet.Contains(value);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在关键字</returns>
        public bool Remove(T value)
        {
            return value != null && hashSet.Remove(value);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="values"></param>
        /// <returns>删除数据数量</returns>
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
