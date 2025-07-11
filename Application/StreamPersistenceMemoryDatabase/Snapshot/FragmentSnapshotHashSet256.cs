﻿using System;
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
        /// The data collection
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
        /// Snapshot collection
        /// 快照集合
        /// </summary>
        public ISnapshotEnumerable<T> GetSnapshot() { return new FragmentSnapshotHashSetEnumerable256<T>(this); }
        /// <summary>
        /// Clear the data
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
        /// Clear fragmented array (used to solve the problem of low performance of clear call when the amount of data is large)
        /// 清除分片数组（用于解决数据量较大的情况下 Clear 调用性能低下的问题）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void ClearArray()
        {
            Array.Clear(HashSets, 0, 256);
            Count = 0;
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Add(T value)
        {
            uint hashCode = (uint)value.GetHashCode();
            int index = (int)(hashCode & 0xff);
            var hashSet = HashSets[index];
            if (hashSet != null)
            {
                if (hashSet.Add(value, hashCode))
                {
                    ++Count;
                    return true;
                }
                return false;
            }
            HashSets[index] = hashSet = new SnapshotHashSet<T>();
            hashSet.Add(value, hashCode);
            ++Count;
            return true;
        }
        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Contains(T value)
        {
            uint hashCode = (uint)value.GetHashCode();
            var hashSet = HashSets[hashCode & 0xff];
            return hashSet != null && hashSet.Contains(value, hashCode);
        }
        /// <summary>
        /// Delete data
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returning false indicates that there is no matching data
        /// 返回 false 表示不存在匹配数据</returns>
        public bool Remove(T value)
        {
            uint hashCode = (uint)value.GetHashCode();
            var hashSet = HashSets[hashCode & 0xff];
            if (hashSet != null && hashSet.Remove(value, hashCode))
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
