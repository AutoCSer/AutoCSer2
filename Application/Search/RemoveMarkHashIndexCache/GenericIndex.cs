using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.CommandService.Search.IndexQuery;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoCSer.CommandService.Search.RemoveMarkHashIndexCache
{
    /// <summary>
    /// 索引数据磁盘块索引缓存节点
    /// </summary>
    /// <typeparam name="KT">Index keyword type
    /// 索引关键字类型</typeparam>
    /// <typeparam name="VT">索引数据类型</typeparam>
    public sealed class GenericIndex<KT, VT> : BlockIndexDataCacheNode<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
    {
        /// <summary>
        /// 索引数据（8B 哈希+ VT）
        /// </summary>
        private RemoveMarkHashSet<VT> littleValues;
        /// <summary>
        /// 索引数据
        /// </summary>
        private HashSet<VT> manyValues;
        /// <summary>
        /// 少量数据索引
        /// </summary>
        internal override IIndex<VT> LittleIndex { get { return new RemoveMarkHashSetIndex<VT>(littleValues); } }
        /// <summary>
        /// 大量数据索引
        /// </summary>
        internal override IIndex<VT> ManyIndex { get { return new HashSetIndex<VT>(manyValues); } }
        /// <summary>
        /// 少量数据集合数据量
        /// </summary>
        protected override int littleCount { get { return littleValues.Capacity; }  }
        /// <summary>
        /// 大量数据集合数据量
        /// </summary>
        protected override int manyCount { get { return manyValues.Count; } }
        /// <summary>
        /// 索引数据磁盘块索引缓存节点
        /// </summary>
        /// <param name="cache">索引数据磁盘块索引缓存</param>
        /// <param name="key">keyword</param>
        internal GenericIndex(BlockIndexDataCache<KT, VT> cache, KT key) : base(cache, key)
        {
            littleValues = EmptyRemoveMarkHashSet;
            manyValues = EmptyHashSet;
        }
        /// <summary>
        /// 设置为少量数据索引
        /// </summary>
        protected override void setLittleValues()
        {
            littleValues = new RemoveMarkHashSet<VT>(data.Values);
        }
        /// <summary>
        /// 设置为大量数据索引
        /// </summary>
        protected override void setManyValues()
        {
            manyValues = new HashSet<VT>(data.Values);
        }
        /// <summary>
        /// 获取少量数据集合数据量
        /// </summary>
        /// <returns></returns>
        protected override int getRemoveLittleCount()
        {
            int count = littleValues.Capacity;
            littleValues = EmptyRemoveMarkHashSet;
            return count;
        }
        /// <summary>
        /// 获取大量数据集合数据量
        /// </summary>
        /// <returns></returns>
        protected override int getRemoveManyCount()
        {
            int count = manyValues.Count;
            manyValues = EmptyHashSet;
            return count;
        }
        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="capacity"></param>
        internal override void Load(LeftArray<PersistenceNode<VT>> nodes, int capacity)
        {
#if NetStandard21
            load(nodes, capacity, new HashSet<VT>(capacity));
#else
            load(nodes, capacity, new HashSet<VT>());
#endif
        }
        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="capacity"></param>
        /// <param name="hashSet"></param>
        private void load(LeftArray<PersistenceNode<VT>> nodes, int capacity, HashSet<VT> hashSet)
        {
            PersistenceNode<VT>[] nodeArray = nodes.Array;
            int nodeIndex = nodes.Length;
            do
            {
                nodeArray[--nodeIndex].Load(hashSet);
            }
            while (nodeIndex != 0);
            int count = hashSet.Count;
            if (count >= LittleMinCount)
            {
                if (count <= RemoveMarkHashSetCapacity.MaxCapacity)
                {
                    littleValues = new RemoveMarkHashSet<VT>(hashSet);
                    Type = IndexDataTypeEnum.Little;
                }
                else
                {
#if NetStandard21
                    manyValues = (long)hashSet.Count << 2 >= (long)capacity * 3 ? hashSet : new HashSet<VT>(hashSet);
#else
                    manyValues = hashSet;
#endif
                    Type = IndexDataTypeEnum.Many;
                }
                data.SetEmptyValues();
            }
            else
            {
                data = new BlockIndexData<VT>(data.BlockIndex, count != 0 ? hashSet.ToArray() : EmptyArray<VT>.Array);
                Type = IndexDataTypeEnum.BlockIndexData;
            }
            if (count != 0) cache.AddValueCount(this, count);
        }
        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="manyIndex"></param>
        internal override void Load(LeftArray<PersistenceNode<VT>> nodes, IIndex<VT> manyIndex)
        {
            HashSet<VT> hashSet = ((HashSetIndex<VT>)manyIndex).HashSet;
            load(nodes, hashSet.Count, hashSet);
        }
        /// <summary>
        /// 加载数据失败恢复数据
        /// </summary>
        /// <param name="manyIndex"></param>
        internal override void SetManyValues(IIndex<VT> manyIndex)
        {
            manyValues = ((HashSetIndex<VT>)manyIndex).HashSet;
        }
    }
}
