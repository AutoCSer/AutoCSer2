using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 256 基分片 快照哈希表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class FragmentSnapshotHashSet256<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 哈希表
        /// </summary>
#if NetStandard21
        internal readonly SnapshotHashSet<T>?[] HashSets = new SnapshotHashSet<T>?[256];
#else
        internal readonly SnapshotHashSet<T>[] HashSets = new SnapshotHashSet<T>[256];
#endif
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count { get; internal set; }
        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<T> Values
        {
            get
            {
                foreach (var hashSet in HashSets)
                {
                    if (hashSet != null)
                    {
                        foreach (T value in hashSet.Values) yield return value;
                    }
                }
            }
        }
        /// <summary>
        /// 快照集合
        /// </summary>
        public ISnapshotEnumerable<T> GetSnapshot() { return new FragmentSnapshotHashSetEnumerable256<T>(this); }
        /// <summary>
        /// 清除数据
        /// </summary>
        public void Clear()
        {
            foreach (var hashSet in HashSets) hashSet?.ClearArray();
            Count = 0;
        }
        /// <summary>
        /// 清除计数位置信息
        /// </summary>
        internal void ClearCount()
        {
            foreach (var hashSet in HashSets) hashSet?.ClearCount();
            Count = 0;
        }
        /// <summary>
        /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void ClearArray()
        {
            Array.Clear(HashSets, 0, 256);
            Count = 0;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Add(T value)
        {
            if (GetOrCreateHashSet(value).Add(value))
            {
                ++Count;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据数据获取哈希表，不存在时创建哈希表
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal SnapshotHashSet<T> GetOrCreateHashSet(T value)
        {
            int index = value.GetHashCode() & 0xff;
            var hashSet = HashSets[index];
            if (hashSet == null) HashSets[index] = hashSet = new SnapshotHashSet<T>();
            return hashSet;
        }
        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Contains(T value)
        {
            var hashSet = HashSets[value.GetHashCode() & 0xff];
            return hashSet != null && hashSet.Contains(value);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据</returns>
        public bool Remove(T value)
        {
            var hashSet = HashSets[value.GetHashCode() & 0xff];
            if (hashSet != null && hashSet.Remove(value))
            {
                --Count;
                return true;
            }
            return false;
        }
        ///// <summary>
        ///// 获取数据集合
        ///// </summary>
        ///// <returns>数据集合</returns>
        //public LeftArray<T> GetArray()
        //{
        //    if (Count == 0) return new LeftArray<T>(0);
        //    LeftArray<T> array = new LeftArray<T>(Count);
        //    foreach (var hashSet in hashSets)
        //    {
        //        if (hashSet != null)
        //        {
        //            foreach (T value in hashSet) array.Array[array.Length++] = value;
        //        }
        //    }
        //    return array;
        //}
    }
}
