using AutoCSer.CommandService.Search.IndexQuery;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.Search.MemoryIndex
{
    /// <summary>
    /// 哈希索引节点
    /// </summary>
    /// <typeparam name="T">索引关键字类型</typeparam>
    public sealed class HashCodeKeyIndexNode<T> : ContextNode<IHashCodeKeyIndexNode<T>, BinarySerializeKeyValue<T, uint[]>>, IHashCodeKeyIndexNode<T>, ISnapshot<BinarySerializeKeyValue<T, uint[]>>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 关键字索引集合
        /// </summary>
        private readonly FragmentDictionary256<T, HashCodeKeySetIndex> indexs;
        /// <summary>
        /// 哈希索引节点
        /// </summary>
        public HashCodeKeyIndexNode()
        {
            indexs = new FragmentDictionary256<T, HashCodeKeySetIndex>();
        }
        /// <summary>
        /// 释放索引节点
        /// </summary>
        private void free()
        {
            foreach (HashCodeKeySetIndex index in indexs.Values) index.Free();
            indexs.Clear();
        }
        /// <summary>
        /// 节点移除后处理
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceNodeOnRemoved()
        {
            free();
        }
        /// <summary>
        /// 数据库服务关闭操作
        /// </summary>
        public override void StreamPersistenceMemoryDatabaseServiceDisposable()
        {
            free();
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int ISnapshot<BinarySerializeKeyValue<T, uint[]>>.GetSnapshotCapacity(ref object customObject)
        {
            return indexs.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        SnapshotResult<BinarySerializeKeyValue<T, uint[]>> ISnapshot<BinarySerializeKeyValue<T, uint[]>>.GetSnapshotResult(BinarySerializeKeyValue<T, uint[]>[] snapshotArray, object customObject)
        {
            SnapshotResult<BinarySerializeKeyValue<T, uint[]>> result = new SnapshotResult<BinarySerializeKeyValue<T, uint[]>>(indexs.Count, snapshotArray.Length);
            foreach (KeyValuePair<T, HashCodeKeySetIndex> index in indexs.KeyValues) result.Add(snapshotArray, new BinarySerializeKeyValue<T, uint[]>(index.Key, index.Value.ToArray()));
            return result;
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSet(BinarySerializeKeyValue<T, uint[]> value)
        {
            bool isAdd = false;
            HashCodeKeySetIndex index = new HashCodeKeySetIndex(value.Value);
            try
            {
                indexs.Add(value.Key, index);
                isAdd = true;
            }
            finally
            {
                if (!isAdd) index.Free();
            }
        }
        /// <summary>
        /// 添加匹配数据关键字 持久化前检查
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        /// <returns></returns>
        public ValueResult<bool> AppendBeforePersistence(T key, uint value)
        {
            if (key != null)
            {
                var index = default(HashCodeKeySetIndex);
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
        public bool Append(T key, uint value)
        {
            var index = default(HashCodeKeySetIndex);
            if (indexs.TryGetValue(key, out index)) index.Append(value);
            else
            {
                bool isAdd = false;
                index = new HashCodeKeySetIndex(value);
                try
                {
                    indexs.Add(key, index);
                    isAdd = true;
                }
                finally
                {
                    if (!isAdd) index.Free();
                }
            }
            return true;
        }
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <param name="value">匹配数据关键字</param>
        public void AppendArray(T[] keys, uint value)
        {
            if (keys != null)
            {
                foreach (T key in keys)
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
        public void AppendLeftArray(LeftArray<T> keys, uint value)
        {
            int count = keys.Length;
            if (count != 0)
            {
                foreach (T key in keys.Array)
                {
                    if (key != null) Append(key, value);
                    if (--count == 0) break;
                }
            }
        }
        /// <summary>
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        /// <returns></returns>
        public ValueResult<bool> RemoveBeforePersistence(T key, uint value)
        {
            if (key != null)
            {
                var index = default(HashCodeKeySetIndex);
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
        public bool Remove(T key, uint value)
        {
            var index = default(HashCodeKeySetIndex);
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
        public void RemoveArray(T[] keys, uint value)
        {
            if (keys != null)
            {
                foreach (T key in keys)
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
        public void RemoveLeftArray(LeftArray<T> keys, uint value)
        {
            int count = keys.Length;
            if (count != 0)
            {
                foreach (T key in keys.Array)
                {
                    if (key != null) Remove(key, value);
                    if (--count == 0) break;
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
        public IIndexCondition<uint>? GetIndexCondition(T[] keys, IndexMergeTypeEnum type = IndexMergeTypeEnum.Intersection)
#else
        public IIndexCondition<uint> GetIndexCondition(T[] keys, IndexMergeTypeEnum type = IndexMergeTypeEnum.Intersection)
#endif
        {
            var index = default(HashCodeKeySetIndex);
            if (keys.Length == 1) return this.indexs.TryGetValue(keys[0], out index) && index.Count != 0 ? new IndexCondition<uint>(index) : null;
            LeftArray<IIndex<uint>> indexs = new LeftArray<IIndex<uint>>(keys.Length);
            foreach(T key in keys)
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
                case 1: return new IndexCondition<uint>(indexs.Array[0]);
                default: return new IndexArrayCondition<uint>(indexs, type);
            }
        }
        /// <summary>
        /// 获取索引条件
        /// </summary>
        /// <param name="keys">索引关键字</param>
        /// <param name="type">索引合并操作类型</param>
        /// <returns>失败返回 null</returns>
#if NetStandard21
        public IIndexCondition<int>? GetIntIndexCondition(T[] keys, IndexMergeTypeEnum type = IndexMergeTypeEnum.Intersection)
#else
        public IIndexCondition<int> GetIntIndexCondition(T[] keys, IndexMergeTypeEnum type = IndexMergeTypeEnum.Intersection)
#endif
        {
            var index = default(HashCodeKeySetIndex);
            if (keys.Length == 1) return this.indexs.TryGetValue(keys[0], out index) && index.Count != 0 ? new IndexCondition<int>(index) : null;
            LeftArray<IIndex<int>> indexs = new LeftArray<IIndex<int>>(keys.Length);
            foreach (T key in keys)
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
                case 1: return new IndexCondition<int>(indexs.Array[0]);
                default: return new IndexArrayCondition<int>(indexs, type);
            }
        }
    }
}
