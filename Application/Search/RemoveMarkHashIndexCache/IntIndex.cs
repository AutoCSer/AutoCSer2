using AutoCSer.CommandService.Search.DiskBlockIndex;
using AutoCSer.CommandService.Search.IndexQuery;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.Search.RemoveMarkHashIndexCache
{
    /// <summary>
    /// 索引数据磁盘块索引缓存节点 KT:int
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    public sealed class IntIndex<KT> : UIntIndex<KT, int>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
        /// <summary>
        /// 少量数据索引
        /// </summary>
        internal override IIndex<int> LittleIndex { get { return new RemoveMarkIntHashSetIndex(LittleValues); } }
        /// <summary>
        /// 大量数据索引
        /// </summary>
        internal override IIndex<int> ManyIndex { get { return new ReusableIntHashSetIndex(manyValues); } }
        /// <summary>
        /// 索引数据磁盘块索引缓存节点
        /// </summary>
        /// <param name="cache">索引数据磁盘块索引缓存</param>
        /// <param name="key">关键字</param>
        internal IntIndex(BlockIndexDataCache<KT, int> cache, KT key) : base(cache, key) { }
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
        /// 加载数据
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="capacity"></param>
        internal override void Load(LeftArray<PersistenceNode<int>> nodes, int capacity)
        {
            load(nodes, new ReusableHashCodeKeyHashSet(capacity));
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="hashSet"></param>
        private void load(LeftArray<PersistenceNode<int>> nodes, ReusableHashCodeKeyHashSet hashSet)
        {
            PersistenceNode<int>[] nodeArray = nodes.Array;
            int nodeIndex = nodes.Length;
            do
            {
                PersistenceNode<int> node = nodeArray[--nodeIndex];
                int valueCount = node.ValueCount;
                foreach (int value in node.Values)
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
                data = new BlockIndexData<int>(data.BlockIndex, hashSet.GetIntArray());
                Type = IndexDataTypeEnum.BlockIndexData;
            }
            if (count != 0) cache.AddValueCount(this, count);
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="manyIndex"></param>
        internal override void Load(LeftArray<PersistenceNode<int>> nodes, IIndex<int> manyIndex)
        {
            load(nodes, ((ReusableIntHashSetIndex)manyIndex).HashSet);
        }
        /// <summary>
        /// 加载数据失败恢复数据
        /// </summary>
        /// <param name="manyIndex"></param>
        internal override void SetManyValues(IIndex<int> manyIndex)
        {
            manyValues = ((ReusableIntHashSetIndex)manyIndex).HashSet;
        }
    }
}
