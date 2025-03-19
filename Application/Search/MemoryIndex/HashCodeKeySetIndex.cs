using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.Search.RemoveMarkHashIndexCache;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.MemoryIndex
{
    /// <summary>
    /// 哈希索引
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal sealed class HashCodeKeySetIndex : IIndex<uint>, IIndex<int>
    {
        /// <summary>
        /// 索引数据（8B 哈希+ VT）
        /// </summary>
        private RemoveMarkHashSet littleValues;
        /// <summary>
        /// 索引数据
        /// </summary>
        private ReusableHashCodeKeyHashSet manyValues;
        /// <summary>
        /// 数据数量
        /// </summary>
        public int Count
        {
            get
            {
                return !object.ReferenceEquals(littleValues, RemoveMarkHashSet.Empty) ? littleValues.Count : manyValues.Count;
            }
        }
        /// <summary>
        /// 哈希索引
        /// </summary>
        /// <param name="value"></param>
        internal HashCodeKeySetIndex(uint value)
        {
            littleValues = new RemoveMarkHashSet();
            littleValues.Add(value);
            manyValues = ReusableHashCodeKeyHashSet.Empty;
        }
        /// <summary>
        /// 哈希索引
        /// </summary>
        /// <param name="values"></param>
        internal HashCodeKeySetIndex(uint[] values)
        {
            if (values.Length <= RemoveMarkHashSetCapacity.MaxCapacity)
            {
                littleValues = new RemoveMarkHashSet(values);
                manyValues = ReusableHashCodeKeyHashSet.Empty;
            }
            else
            {
                manyValues = new ReusableHashCodeKeyHashSet(values);
                littleValues = RemoveMarkHashSet.Empty;
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        internal void Free()
        {
            if (!object.ReferenceEquals(littleValues, RemoveMarkHashSet.Empty))
            {
                littleValues.Dispose();
                manyValues = ReusableHashCodeKeyHashSet.Empty;
                littleValues = RemoveMarkHashSet.Empty;
            }
        }
        /// <summary>
        /// 获取数组
        /// </summary>
        /// <returns></returns>
        internal uint[] ToArray()
        {
            if (!object.ReferenceEquals(littleValues, RemoveMarkHashSet.Empty))
            {
                return littleValues.Count != 0 ? littleValues.GetArray() : EmptyArray<uint>.Array;
            }
            return manyValues.Count != 0 ? manyValues.GetArray() : EmptyArray<uint>.Array;
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Contains(uint value)
        {
            return !object.ReferenceEquals(littleValues, RemoveMarkHashSet.Empty) ? littleValues.Contains(value) : manyValues.Contains(value);
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Contains(int value)
        {
            return Contains((uint)value);
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        internal void Append(uint value)
        {
            if (!object.ReferenceEquals(littleValues, RemoveMarkHashSet.Empty))
            {
                if (littleValues.Count != RemoveMarkHashSetCapacity.MaxCapacity) littleValues.Add(value);
                else if (!littleValues.Contains(value))
                {
                    ReusableHashCodeKeyHashSet hashSet = new ReusableHashCodeKeyHashSet(littleValues);
                    hashSet.Add(value);
                    littleValues.Dispose();
                    manyValues = hashSet;
                    littleValues = RemoveMarkHashSet.Empty;
                }
            }
            else manyValues.Add(value);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Remove(uint value)
        {
            if (!object.ReferenceEquals(littleValues, RemoveMarkHashSet.Empty)) littleValues.Remove(value);
            else manyValues.Remove(value);
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        unsafe ArrayBuffer<uint> IIndex<uint>.Get(QueryCondition<uint> condition)
        {
            if (!object.ReferenceEquals(littleValues, RemoveMarkHashSet.Empty))
            {
                int count = littleValues.Count;
                if (count != 0)
                {
                    ArrayBuffer<uint> buffer = condition.GetBuffer(count);
                    fixed (uint* bufferFixed = buffer.Array) littleValues.GetBuffer(bufferFixed);
                    buffer.SetCount(count);
                    return buffer;
                }
            }
            else
            {
                int count = manyValues.Count;
                if (count != 0)
                {
                    ArrayBuffer<uint> buffer = condition.GetBuffer(count);
                    foreach (ReusableHashNode node in manyValues.Nodes)
                    {
                        buffer.UnsafeAdd(node.HashCode);
                        if (--count == 0) break;
                    }
                    return buffer;
                }
            }
            return condition.GetNullBuffer();
        }
        /// <summary>
        /// 计算查询数据关键字（非索引条件过滤）
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        ArrayBuffer<uint> IIndex<uint>.GetFilter(QueryCondition<uint> condition)
        {
            if (!object.ReferenceEquals(littleValues, RemoveMarkHashSet.Empty))
            {
                if (littleValues.Count != 0)
                {
                    return condition.Filter(littleValues.OnlyValues, condition.GetBuffer(littleValues.Count));
                }
            }
            else if (manyValues.Count != 0)
            {
                return condition.Filter(manyValues.Values, condition.GetBuffer(manyValues.Count));
            }
            return condition.GetNullBuffer();
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="hashSet"></param>
        void IIndex<uint>.Get(BufferHashSet<uint> hashSet)
        {
            if (!object.ReferenceEquals(littleValues, RemoveMarkHashSet.Empty)) littleValues.Get(hashSet);
            else
            {
                int count = manyValues.Count;
                if (count != 0)
                {
                    foreach (ReusableHashNode node in manyValues.Nodes)
                    {
                        hashSet.Add(node.HashCode);
                        if (--count == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 判断是否全部包含于另一个集合
        /// </summary>
        /// <param name="index">另一个集合</param>
        /// <returns></returns>
        bool IIndex<uint>.AllIn(IIndex<uint> index)
        {
            if (!object.ReferenceEquals(littleValues, RemoveMarkHashSet.Empty))
            {
                foreach (uint value in littleValues.OnlyValues)
                {
                    if (!index.Contains(value)) return false;
                }
            }
            else
            {
                int count = manyValues.Count;
                if (count != 0)
                {
                    foreach (ReusableHashNode node in manyValues.Nodes)
                    {
                        if (!index.Contains(node.HashCode)) return false;
                        if (--count == 0) break;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 计算查询数据关键字
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        unsafe ArrayBuffer<int> IIndex<int>.Get(QueryCondition<int> condition)
        {
            if (!object.ReferenceEquals(littleValues, RemoveMarkHashSet.Empty))
            {
                int count = littleValues.Count;
                if (count != 0)
                {
                    ArrayBuffer<int> buffer = condition.GetBuffer(count);
                    fixed (int* bufferFixed = buffer.Array) littleValues.GetBuffer((uint*)bufferFixed);
                    buffer.SetCount(count);
                    return buffer;
                }
            }
            else
            {
                int count = manyValues.Count;
                if (count != 0)
                {
                    ArrayBuffer<int> buffer = condition.GetBuffer(count);
                    foreach (ReusableHashNode node in manyValues.Nodes)
                    {
                        buffer.UnsafeAdd((int)node.HashCode);
                        if (--count == 0) break;
                    }
                    return buffer;
                }
            }
            return condition.GetNullBuffer();
        }
        /// <summary>
        /// 计算查询数据关键字（非索引条件过滤）
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        ArrayBuffer<int> IIndex<int>.GetFilter(QueryCondition<int> condition)
        {
            if (!object.ReferenceEquals(littleValues, RemoveMarkHashSet.Empty))
            {
                if (littleValues.Count != 0)
                {
                    return condition.Filter(littleValues.OnlyIntValues, condition.GetBuffer(littleValues.Count));
                }
            }
            else if (manyValues.Count != 0)
            {
                return condition.Filter(manyValues.IntValues, condition.GetBuffer(manyValues.Count));
            }
            return condition.GetNullBuffer();
        }
        /// <summary>
        /// 并集 OR
        /// </summary>
        /// <param name="hashSet"></param>
        void IIndex<int>.Get(BufferHashSet<int> hashSet)
        {
            if (!object.ReferenceEquals(littleValues, RemoveMarkHashSet.Empty)) littleValues.Get(hashSet);
            else
            {
                int count = manyValues.Count;
                if (count != 0)
                {
                    foreach (ReusableHashNode node in manyValues.Nodes)
                    {
                        hashSet.Add((int)node.HashCode);
                        if (--count == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 判断是否全部包含于另一个集合
        /// </summary>
        /// <param name="index">另一个集合</param>
        /// <returns></returns>
        bool IIndex<int>.AllIn(IIndex<int> index)
        {
            if (!object.ReferenceEquals(littleValues, RemoveMarkHashSet.Empty))
            {
                foreach (uint value in littleValues.OnlyValues)
                {
                    if (!index.Contains((int)value)) return false;
                }
            }
            else
            {
                int count = manyValues.Count;
                if (count != 0)
                {
                    foreach (ReusableHashNode node in manyValues.Nodes)
                    {
                        if (!index.Contains((int)node.HashCode)) return false;
                        if (--count == 0) break;
                    }
                }
            }
            return true;
        }
    }
}
