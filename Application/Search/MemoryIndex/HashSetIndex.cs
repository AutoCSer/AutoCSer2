using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.Search.RemoveMarkHashIndexCache;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.MemoryIndex
{
    /// <summary>
    /// 哈希索引
    /// </summary>
    /// <typeparam name="T">数据关键字类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal sealed class HashSetIndex<T> : IIndex<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 索引数据（8B 哈希+ VT）
        /// </summary>
        private AutoCSer.RemoveMarkHashSet<T> littleValues;
        /// <summary>
        /// 索引数据
        /// </summary>
        private HashSet<T> manyValues;
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count
        {
            get
            {
                return !object.ReferenceEquals(littleValues, BlockIndexDataCacheNode<T>.EmptyRemoveMarkHashSet) ? littleValues.Count : manyValues.Count;
            }
        }
        /// <summary>
        /// 哈希索引
        /// </summary>
        /// <param name="value"></param>
        internal HashSetIndex(T value)
        {
            littleValues = new RemoveMarkHashSet<T>();
            littleValues.Add(value);
            manyValues = BlockIndexDataCacheNode<T>.EmptyHashSet;
        }
        /// <summary>
        /// 哈希索引
        /// </summary>
        /// <param name="values"></param>
        internal HashSetIndex(T[] values)
        {
            if (values.Length <= RemoveMarkHashSetCapacity.MaxCapacity)
            {
                littleValues = new RemoveMarkHashSet<T>(values);
                manyValues = BlockIndexDataCacheNode<T>.EmptyHashSet;
            }
            else
            {
                manyValues = new HashSet<T>(values);
                littleValues = BlockIndexDataCacheNode<T>.EmptyRemoveMarkHashSet;
            }
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <returns></returns>
        internal T[] ToArray()
        {
            if (!object.ReferenceEquals(littleValues, BlockIndexDataCacheNode<T>.EmptyRemoveMarkHashSet))
            {
                return littleValues.Count != 0 ? littleValues.GetArray() : EmptyArray<T>.Array;
            }
            return manyValues.Count != 0 ? manyValues.getArray() : EmptyArray<T>.Array;
        }
        /// <summary>
        /// Determine whether there is data
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Contains(T value)
        {
            return !object.ReferenceEquals(littleValues, BlockIndexDataCacheNode<T>.EmptyRemoveMarkHashSet) ? littleValues.Contains(value) : manyValues.Contains(value);
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="value"></param>
        internal void Append(T value)
        {
            if (!object.ReferenceEquals(littleValues, BlockIndexDataCacheNode<T>.EmptyRemoveMarkHashSet))
            {
                if (littleValues.Count != RemoveMarkHashSetCapacity.MaxCapacity) littleValues.Add(value);
                else if (!littleValues.Contains(value))
                {
#if NetStandard21
                    HashSet<T> hashSet = new HashSet<T>(RemoveMarkHashSetCapacity.MaxCapacity + 1);
#else
                    HashSet<T> hashSet = new HashSet<T>();
#endif
                    foreach (T nextValue in littleValues.OnlyValues) hashSet.Add(nextValue);
                    hashSet.Add(value);
                    manyValues = hashSet;
                    littleValues = BlockIndexDataCacheNode<T>.EmptyRemoveMarkHashSet;
                }
            }
            else manyValues.Add(value);
        }
        /// <summary>
        /// Delete data
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Remove(T value)
        {
            if (!object.ReferenceEquals(littleValues, BlockIndexDataCacheNode<T>.EmptyRemoveMarkHashSet)) littleValues.Remove(value);
            else manyValues.Remove(value);
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        ArrayBuffer<T> IIndex<T>.Get(QueryCondition<T> condition)
        {
            if (!object.ReferenceEquals(littleValues, BlockIndexDataCacheNode<T>.EmptyRemoveMarkHashSet))
            {
                int count = littleValues.Count;
                if (count != 0)
                {
                    ArrayBuffer<T> buffer = condition.GetBuffer(count);
                    foreach (RemoveMarkHashNode<T> node in littleValues.Nodes)
                    {
                        buffer.UnsafeAdd(node.Value);
                        if (--count == 0) break;
                    }
                    return buffer;
                }
            }
            else if (manyValues.Count != 0)
            {
                ArrayBuffer<T> buffer = condition.GetBuffer(manyValues.Count);
                foreach (T value in manyValues) buffer.UnsafeAdd(value);
                return buffer;
            }
            return condition.GetNullBuffer();
        }
        /// <summary>
        /// 计算查询数据关键字（非索引条件过滤）
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        ArrayBuffer<T> IIndex<T>.GetFilter(QueryCondition<T> condition)
        {
            if (!object.ReferenceEquals(littleValues, BlockIndexDataCacheNode<T>.EmptyRemoveMarkHashSet))
            {
                if (littleValues.Count != 0)
                {
                    return condition.Filter(littleValues.OnlyValues, condition.GetBuffer(littleValues.Count));
                }
            }
            else if (manyValues.Count != 0)
            {
                return condition.Filter(manyValues, condition.GetBuffer(manyValues.Count));
            }
            return condition.GetNullBuffer();
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="hashSet"></param>
        void IIndex<T>.Get(BufferHashSet<T> hashSet)
        {
            if (!object.ReferenceEquals(littleValues, BlockIndexDataCacheNode<T>.EmptyRemoveMarkHashSet)) littleValues.Get(hashSet);
            else
            {
                foreach (T value in manyValues) hashSet.Add(value);
            }
        }
        /// <summary>
        /// 判断是否全部包含于另一个集合
        /// </summary>
        /// <param name="index">另一个集合</param>
        /// <returns></returns>
        bool IIndex<T>.AllIn(IIndex<T> index)
        {
            if (!object.ReferenceEquals(littleValues, BlockIndexDataCacheNode<T>.EmptyRemoveMarkHashSet))
            {
                int count = littleValues.Count;
                if (count != 0)
                {
                    foreach (RemoveMarkHashNode<T> node in littleValues.Nodes)
                    {
                        if (!index.Contains(node.Value)) return false;
                        if (--count == 0) return true;
                    }
                }
            }
            else
            {
                foreach (T value in manyValues)
                {
                    if (!index.Contains(value)) return false;
                }
            }
            return true;
        }
    }
}
