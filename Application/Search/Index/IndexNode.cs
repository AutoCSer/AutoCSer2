using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 关键字索引节点
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    /// <typeparam name="VT">数据关键字类型</typeparam>
    public abstract class IndexNode<KT, VT> : MethodParameterCreatorNode<IIndexNode<KT, VT>, BinarySerializeKeyValue<KT, IndexData<VT>>>, IIndexNode<KT, VT>, ISnapshot<BinarySerializeKeyValue<KT, IndexData<VT>>>
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
        private readonly FragmentDictionary256<KT, HashSetIndex<KT, VT>> indexs;
        /// <summary>
        /// 触发重置写入磁盘块索引信息的更新数据量
        /// </summary>
        private readonly int writeCount;
        /// <summary>
        /// 关键字索引节点
        /// </summary>
        /// <param name="writeCount">触发重置写入磁盘块索引信息的更新数据量，最小值为 16</param>
        protected IndexNode(int writeCount = 1 << 10)
        {
            this.writeCount = Math.Max(writeCount, 16);
            indexs = new FragmentDictionary256<KT, HashSetIndex<KT, VT>>();
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int ISnapshot<BinarySerializeKeyValue<KT, IndexData<VT>>>.GetSnapshotCapacity(ref object customObject)
        {
            return indexs.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        SnapshotResult<BinarySerializeKeyValue<KT, IndexData<VT>>> ISnapshot<BinarySerializeKeyValue<KT, IndexData<VT>>>.GetSnapshotResult(BinarySerializeKeyValue<KT, IndexData<VT>>[] snapshotArray, object customObject)
        {
            SnapshotResult<BinarySerializeKeyValue<KT, IndexData<VT>>> result = new SnapshotResult<BinarySerializeKeyValue<KT, IndexData<VT>>>(indexs.Count, snapshotArray.Length);
            foreach (KeyValuePair<KT, HashSetIndex<KT, VT>> index in indexs.KeyValues) result.Add(snapshotArray, new BinarySerializeKeyValue<KT, IndexData<VT>>(index.Key, new IndexData<VT>(index.Value)));
            return result;
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSet(BinarySerializeKeyValue<KT, IndexData<VT>> value)
        {
            indexs.Add(value.Key, new HashSetIndex<KT, VT>(ref value.Value));
        }
        /// <summary>
        /// 根据索引关键字获取磁盘块索引信息客户端
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <returns></returns>
        public abstract IDiskBlockClient GetDiskBlockClient(KT key);
        /// <summary>
        /// 获取磁盘块索引信息客户端
        /// </summary>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <returns></returns>
        public abstract IDiskBlockClient GetDiskBlockClient(BlockIndex blockIndex);
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        public void AppendLoadPersistence(KT key, VT value)
        {
            if (key != null && value != null)
            {
                var index = default(HashSetIndex<KT, VT>);
                if (indexs.TryGetValue(key, out index)) index.Append(value);
                else indexs.Add(key, new HashSetIndex<KT, VT>(value));
            }
        }
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        public void Append(KT key, VT value)
        {
            if (key != null && value != null)
            {
                var index = default(HashSetIndex<KT, VT>);
                if (indexs.TryGetValue(key, out index))
                {
                    if (index.Append(value) >= writeCount) index.TryWrite(this, key);
                }
                else indexs.Add(key, new HashSetIndex<KT, VT>(value));
            }
        }
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <param name="value">匹配数据关键字</param>
        public void AppendArrayLoadPersistence(KT[] keys, VT value)
        {
            if (keys != null)
            {
                foreach (KT key in keys) AppendLoadPersistence(key, value);
            }
        }
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <param name="value">匹配数据关键字</param>
        public void AppendArray(KT[] keys, VT value)
        {
            if (keys != null)
            {
                foreach (KT key in keys) Append(key, value);
            }
        }
        /// <summary>
        /// 移除匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        public void RemoveLoadPersistence(KT key, VT value)
        {
            if (key != null && value != null)
            {
                var index = default(HashSetIndex<KT, VT>);
                if (indexs.TryGetValue(key, out index)) index.Remove(value);
            }
        }
        /// <summary>
        /// 移除匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        public void Remove(KT key, VT value)
        {
            if (key != null && value != null)
            {
                var index = default(HashSetIndex<KT, VT>);
                if (indexs.TryGetValue(key, out index))
                {
                    if (index.Remove(value) >= writeCount) index.TryWrite(this, key);
                }
            }
        }
        /// <summary>
        /// 移除匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <param name="value">匹配数据关键字</param>
        public void RemoveArrayLoadPersistence(KT[] keys, VT value)
        {
            if (keys != null)
            {
                foreach (KT key in keys) RemoveLoadPersistence(key, value);
            }
        }
        /// <summary>
        /// 移除匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        public void RemoveArray(KT[] keys, VT value)
        {
            if (keys != null)
            {
                foreach (KT key in keys) Remove(key, value);
            }
        }
        /// <summary>
        /// 磁盘块索引信息写入完成
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="blockIndex"></param>
        /// <param name="valueCount"></param>
        /// <param name="version"></param>
        public void CompletedLoadPersistence(KT key, BlockIndex blockIndex, int valueCount, int version)
        {
            indexs[key].Completed(blockIndex, valueCount, version);
        }
        /// <summary>
        /// 磁盘块索引信息写入完成
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="blockIndex"></param>
        /// <param name="valueCount"></param>
        /// <param name="version"></param>
        public void Completed(KT key, BlockIndex blockIndex, int valueCount, int version)
        {
            HashSetIndex<KT, VT> index = indexs[key];
            if (index.Completed(blockIndex, valueCount, version) >= writeCount) index.Write(this, key);
        }
        /// <summary>
        /// 获取索引数据
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <returns></returns>
        public IndexData<VT> GetData(KT key)
        {
            if (key != null)
            {
                var index = default(HashSetIndex<KT, VT>);
                if (indexs.TryGetValue(key, out index)) return new IndexData<VT>(index);
            }
            return default(IndexData<VT>);
        }
    }
}
