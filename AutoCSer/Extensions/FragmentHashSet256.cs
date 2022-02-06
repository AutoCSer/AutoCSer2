using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 256 基分片 哈希表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FragmentHashSet256<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 哈希表
        /// </summary>
        private readonly HashSet<T>[] hashSets = new HashSet<T>[256];
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
                foreach (HashSet<T> hashSet in hashSets)
                {
                    if (hashSet != null)
                    {
                        foreach (T value in hashSet) yield return value;
                    }
                }
            }
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        public void Clear()
        {
            foreach (HashSet<T> hashSet in hashSets)
            {
                if (hashSet != null) hashSet.Clear();
            }
            Count = 0;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
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
        internal HashSet<T> GetOrCreateHashSet(T value)
        {
            int index = value.GetHashCode() & 0xff;
            HashSet<T> hashSet = hashSets[index];
            if (hashSet == null) hashSets[index] = hashSet = HashSetCreator<T>.Create();
            return hashSet;
        }
        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        public bool Contains(T value)
        {
            HashSet<T> hashSet = hashSets[value.GetHashCode() & 0xff];
            return hashSet != null && hashSet.Contains(value);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否存在数据</returns>
        public bool Remove(T value)
        {
            HashSet<T> hashSet = hashSets[value.GetHashCode() & 0xff];
            if (hashSet != null && hashSet.Remove(value))
            {
                --Count;
                return true;
            }
            return false;
        }
    }
}
