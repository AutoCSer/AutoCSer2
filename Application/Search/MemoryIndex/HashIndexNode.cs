using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.Search.MemoryIndex
{
    /// <summary>
    /// 哈希索引节点
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    /// <typeparam name="VT">数据关键字类型</typeparam>
    public sealed class HashIndexNode<KT, VT> : IHashIndexNode<KT, VT>, ISnapshot<BinarySerializeKeyValue<KT, VT[]>>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IEquatable<VT>
#else
        where KT : IEquatable<KT>
        where VT : IEquatable<VT>
#endif
    {
        /// <summary>
        /// 关键字索引集合
        /// </summary>
        private readonly FragmentDictionary256<KT, HashSetIndex<VT>> indexs;
        /// <summary>
        /// 哈希索引节点
        /// </summary>
        public HashIndexNode()
        {
            indexs = new FragmentDictionary256<KT, HashSetIndex<VT>>();
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int ISnapshot<BinarySerializeKeyValue<KT, VT[]>>.GetSnapshotCapacity(ref object customObject)
        {
            return indexs.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        SnapshotResult<BinarySerializeKeyValue<KT, VT[]>> ISnapshot<BinarySerializeKeyValue<KT, VT[]>>.GetSnapshotResult(BinarySerializeKeyValue<KT, VT[]>[] snapshotArray, object customObject)
        {
            SnapshotResult<BinarySerializeKeyValue<KT, VT[]>> result = new SnapshotResult<BinarySerializeKeyValue<KT, VT[]>>(indexs.Count, snapshotArray.Length);
            foreach (KeyValuePair<KT, HashSetIndex<VT>> index in indexs.KeyValues) result.Add(snapshotArray, new BinarySerializeKeyValue<KT, VT[]>(index.Key, index.Value.ToArray()));
            return result;
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        void ISnapshot<BinarySerializeKeyValue<KT, VT[]>>.SetSnapshotResult(ref LeftArray<BinarySerializeKeyValue<KT, VT[]>> array, ref LeftArray<BinarySerializeKeyValue<KT, VT[]>> newArray) { }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSet(BinarySerializeKeyValue<KT, VT[]> value)
        {
            indexs.Add(value.Key, new HashSetIndex<VT>(value.Value));
        }
        /// <summary>
        /// 添加匹配数据关键字 持久化前检查
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        /// <returns></returns>
        public ValueResult<bool> AppendBeforePersistence(KT key, VT value)
        {
            if (key != null && value != null)
            {
                var index = default(HashSetIndex<VT>);
                if (indexs.TryGetValue(key, out index))
                {
                    if (index.Contains(value)) return true;
                }
                return default(ValueResult<bool>);
            }
            return false;
        }
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        /// <returns>返回 false 表示关键字数据为 null</returns>
        public bool Append(KT key, VT value)
        {
            var index = default(HashSetIndex<VT>);
            if (indexs.TryGetValue(key, out index)) index.Append(value);
            else indexs.Add(key, new HashSetIndex<VT>(value));
            return true;
        }
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <param name="value">匹配数据关键字</param>
        public void AppendArray(KT[] keys, VT value)
        {
            if (keys != null && value != null)
            {
                foreach (KT key in keys)
                {
                    if (key != null) Append(key, value);
                }
            }
        }
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <param name="value">匹配数据关键字</param>
        public void AppendLeftArray(LeftArray<KT> keys, VT value)
        {
            if (value != null)
            {
                int count = keys.Length;
                if (count != 0)
                {
                    foreach (KT key in keys.Array)
                    {
                        if (key != null) Append(key, value);
                        if (--count == 0) break;
                    }
                }
            }
        }
        /// <summary>
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        /// <returns></returns>
        public ValueResult<bool> RemoveBeforePersistence(KT key, VT value)
        {
            if (key != null && value != null)
            {
                var index = default(HashSetIndex<VT>);
                if (indexs.TryGetValue(key, out index) && index.Contains(value)) return default(ValueResult<bool>);
            }
            return false;
        }
        /// <summary>
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        /// <returns>返回 false 表示关键字数据为 null 或者没有找到索引关键字</returns>
        public bool Remove(KT key, VT value)
        {
            var index = default(HashSetIndex<VT>);
            if (indexs.TryGetValue(key, out index))
            {
                index.Remove(value);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        public void RemoveArray(KT[] keys, VT value)
        {
            if (keys != null && value != null)
            {
                foreach (KT key in keys)
                {
                    if (key != null) Remove(key, value);
                }
            }
        }
        /// <summary>
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        public void RemoveLeftArray(LeftArray<KT> keys, VT value)
        {
            if (value != null)
            {
                int count = keys.Length;
                if (count != 0)
                {
                    foreach (KT key in keys.Array)
                    {
                        if (key != null) Remove(key, value);
                        if (--count == 0) break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取索引条件
        /// </summary>
        /// <param name="keys">索引关键字</param>
        /// <param name="type">索引合并操作类型</param>
        /// <returns>失败返回 null</returns>
#if NetStandard21
        public IIndexCondition<VT>? GetIndexCondition(KT[] keys, IndexMergeTypeEnum type = IndexMergeTypeEnum.Intersection)
#else
        public IIndexCondition<VT> GetIndexCondition(KT[] keys, IndexMergeTypeEnum type = IndexMergeTypeEnum.Intersection)
#endif
        {
            var index = default(HashSetIndex<VT>);
            if (keys.Length == 1) return this.indexs.TryGetValue(keys[0], out index) && index.Count != 0 ? new IndexCondition<VT>(index) : null;
            LeftArray<IIndex<VT>> indexs = new LeftArray<IIndex<VT>>(keys.Length);
            foreach (KT key in keys)
            {
                if (this.indexs.TryGetValue(key, out index))
                {
                    if (index.Count == 0)
                    {
                        if (type == IndexMergeTypeEnum.Intersection) return null;
                    }
                    else indexs.Add(index);
                }
            }
            switch (indexs.Length)
            {
                case 0: return null;
                case 1: return new IndexCondition<VT>(indexs.Array[0]);
                default: return new IndexArrayCondition<VT>(indexs, type);
            }
        }
    }
}
