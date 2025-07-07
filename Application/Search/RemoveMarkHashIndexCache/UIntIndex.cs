using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.CommandService.Search.IndexQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.RemoveMarkHashIndexCache
{
    /// <summary>
    /// 索引数据磁盘块索引缓存节点
    /// </summary>
    /// <typeparam name="KT">Index keyword type
    /// 索引关键字类型</typeparam>
    /// <typeparam name="VT">索引数据类型</typeparam>
    public abstract class UIntIndex<KT, VT> : BlockIndexDataCacheNode<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
    {
        /// <summary>
        /// 索引数据（8B 数据）
        /// </summary>
        internal RemoveMarkHashSet LittleValues;
        /// <summary>
        /// 索引数据（16B 数据）
        /// </summary>
        protected ReusableHashCodeKeyHashSet manyValues;
        /// <summary>
        /// 少量数据集合数据量
        /// </summary>
        protected override int littleCount { get { return LittleValues.Capacity; } }
        /// <summary>
        /// 大量数据集合数据量
        /// </summary>
        protected override int manyCount { get { return manyValues.Capacity; } }
        /// <summary>
        /// 释放节点集合
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free()
        {
            if (Type == IndexDataTypeEnum.Little)
            {
                LittleValues.Dispose();
                Type = IndexDataTypeEnum.NotLoaded;
            }
        }
        /// <summary>
        /// 索引数据磁盘块索引缓存节点
        /// </summary>
        /// <param name="cache">索引数据磁盘块索引缓存</param>
        /// <param name="key">keyword</param>
        protected UIntIndex(BlockIndexDataCache<KT, VT> cache, KT key) : base(cache, key)
        {
            LittleValues = RemoveMarkHashSet.Empty;
            manyValues = ReusableHashCodeKeyHashSet.Empty;
        }
        /// <summary>
        /// 获取少量数据集合数据量
        /// </summary>
        /// <returns></returns>
        protected override int getRemoveLittleCount()
        {
            int count = LittleValues.Capacity;
            LittleValues = RemoveMarkHashSet.Empty;
            return count;
        }
        /// <summary>
        /// 获取大量数据集合数据量
        /// </summary>
        /// <returns></returns>
        protected override int getRemoveManyCount()
        {
            int count = manyValues.Capacity;
            manyValues = ReusableHashCodeKeyHashSet.Empty;
            return count;
        }
        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="hashSet"></param>
        protected void load(ReusableHashCodeKeyHashSet hashSet)
        {
            if (hashSet.Count <= RemoveMarkHashSetCapacity.MaxCapacity)
            {
                LittleValues = new RemoveMarkHashSet(hashSet);
                Type = IndexDataTypeEnum.Little;
            }
            else
            {
                manyValues = ReusableDictionary.GetCapacity(hashSet.Count) != hashSet.Capacity ? new ReusableHashCodeKeyHashSet(hashSet) : hashSet;
                Type = IndexDataTypeEnum.Many;
            }
            data.SetEmptyValues();
        }
    }
    /// <summary>
    /// 索引数据磁盘块索引缓存节点 KT:uint
    /// </summary>
    /// <typeparam name="KT">Index keyword type
    /// 索引关键字类型</typeparam>
    public sealed class UIntIndex<KT> : UIntIndex<KT, uint>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
        /// <summary>
        /// 索引数据磁盘块索引缓存节点
        /// </summary>
        /// <param name="cache">索引数据磁盘块索引缓存</param>
        /// <param name="key">keyword</param>
        internal UIntIndex(BlockIndexDataCache<KT, uint> cache, KT key) : base(cache, key) { }
        /// <summary>
        /// 少量数据索引
        /// </summary>
        internal override IIndex<uint> LittleIndex { get { return new RemoveMarkHashSetIndex(LittleValues); } }
        /// <summary>
        /// 大量数据索引
        /// </summary>
        internal override IIndex<uint> ManyIndex { get { return new ReusableHashCodeKeyHashSetIndex(manyValues); } }
        /// <summary>
        /// 设置为少量数据索引
        /// </summary>
        protected override void setLittleValues()
        {
            LittleValues = new RemoveMarkHashSet(data.Values);
        }
        /// <summary>
        /// 设置为大量数据索引
        /// </summary>
        protected override void setManyValues()
        {
            manyValues = new ReusableHashCodeKeyHashSet(data.Values);
        }
        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="capacity"></param>
        internal override void Load(LeftArray<PersistenceNode<uint>> nodes, int capacity)
        {
            load(nodes, new ReusableHashCodeKeyHashSet(capacity));
        }
        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="hashSet"></param>
        private void load(LeftArray<PersistenceNode<uint>> nodes, ReusableHashCodeKeyHashSet hashSet)
        {
            PersistenceNode<uint>[] nodeArray = nodes.Array;
            int nodeIndex = nodes.Length;
            do
            {
                PersistenceNode<uint> node = nodeArray[--nodeIndex];
                int valueCount = node.ValueCount;
                foreach (uint value in node.Values)
                {
                    if (valueCount != 0)
                    {
                        hashSet.Add(value);
                        --valueCount;
                    }
                    else hashSet.Remove(value);
                }
            }
            while (nodeIndex != 0);
            int count = hashSet.Count;
            if (count >= LittleMinCount) load(hashSet);
            else
            {
                data = new BlockIndexData<uint>(data.BlockIndex, hashSet.GetArray());
                Type = IndexDataTypeEnum.BlockIndexData;
            }
            if (count != 0) cache.AddValueCount(this, count);
        }
        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="manyIndex"></param>
        internal override void Load(LeftArray<PersistenceNode<uint>> nodes, IIndex<uint> manyIndex)
        {
            load(nodes, ((ReusableHashCodeKeyHashSetIndex)manyIndex).HashSet);
        }
        /// <summary>
        /// 加载数据失败恢复数据
        /// </summary>
        /// <param name="manyIndex"></param>
        internal override void SetManyValues(IIndex<uint> manyIndex)
        {
            manyValues = ((ReusableHashCodeKeyHashSetIndex)manyIndex).HashSet;
        }
    }
}
