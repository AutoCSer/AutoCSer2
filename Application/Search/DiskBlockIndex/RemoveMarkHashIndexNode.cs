using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
    /// <summary>
    /// 带移除标记的可重用哈希索引节点
    /// </summary>
    /// <typeparam name="KT">索引关键字类型</typeparam>
    /// <typeparam name="VT">数据关键字类型</typeparam>
    public abstract class RemoveMarkHashIndexNode<KT, VT> : MethodParameterCreatorNode<IRemoveMarkHashIndexNode<KT, VT>, BinarySerializeKeyValue<KT, BlockIndexData<VT>>>, IRemoveMarkHashIndexNode<KT, VT>, ISnapshot<BinarySerializeKeyValue<KT, BlockIndexData<VT>>>
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
        private readonly Dictionary<KT, RemoveMarkHashIndex<KT, VT>> indexs;
        /// <summary>
        /// 关键字变更回调
        /// </summary>
        private LeftArray<MethodKeepCallback<KT>> callbacks;
        /// <summary>
        /// 带移除标记的可重用哈希索引节点
        /// </summary>
        /// <param name="capacity">初始化容器尺寸</param>
        protected RemoveMarkHashIndexNode(int capacity = 0)
        {
            callbacks.SetEmpty();
            indexs = DictionaryCreator<KT>.Create<RemoveMarkHashIndex<KT, VT>>(capacity);
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
#if NetStandard21
        public override IRemoveMarkHashIndexNode<KT, VT>? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override IRemoveMarkHashIndexNode<KT, VT> StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            foreach (RemoveMarkHashIndex<KT, VT> index in indexs.Values) index.Loaded();
            return null;
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int ISnapshot<BinarySerializeKeyValue<KT, BlockIndexData<VT>>>.GetSnapshotCapacity(ref object customObject)
        {
            return indexs.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        SnapshotResult<BinarySerializeKeyValue<KT, BlockIndexData<VT>>> ISnapshot<BinarySerializeKeyValue<KT, BlockIndexData<VT>>>.GetSnapshotResult(BinarySerializeKeyValue<KT, BlockIndexData<VT>>[] snapshotArray, object customObject)
        {
            SnapshotResult<BinarySerializeKeyValue<KT, BlockIndexData<VT>>> result = new SnapshotResult<BinarySerializeKeyValue<KT, BlockIndexData<VT>>>(indexs.Count, snapshotArray.Length);
            foreach (KeyValuePair<KT, RemoveMarkHashIndex<KT, VT>> index in indexs) result.Add(snapshotArray, new BinarySerializeKeyValue<KT, BlockIndexData<VT>>(index.Key, new BlockIndexData<VT>(index.Value)));
            return result;
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSet(BinarySerializeKeyValue<KT, BlockIndexData<VT>> value)
        {
            indexs.Add(value.Key, new RemoveMarkHashIndex<KT, VT>(this, value.Key, ref value.Value));
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
        /// <returns>返回 false 表示关键字数据为 null</returns>
        public bool AppendLoadPersistence(KT key, VT value)
        {
            if (key != null && value != null)
            {
                var index = default(RemoveMarkHashIndex<KT, VT>);
                if (indexs.TryGetValue(key, out index)) index.AppendLoadPersistence(value);
                else indexs.Add(key, new RemoveMarkHashIndex<KT, VT>(this, key, value));
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
            if (key != null && value != null)
            {
                var index = default(RemoveMarkHashIndex<KT, VT>);
                if (indexs.TryGetValue(key, out index)) index.Append(value);
                else
                {
                    indexs.Add(key, new RemoveMarkHashIndex<KT, VT>(this, key, value));
                    Callback(key);
                }
                return true;
            }
            return false;
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
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <param name="value">匹配数据关键字</param>
        public void AppendLeftArrayLoadPersistence(LeftArray<KT> keys, VT value)
        {
            int count = keys.Length;
            if (count != 0)
            {
                foreach (KT key in keys.Array)
                {
                    AppendLoadPersistence(key, value);
                    if (--count == 0) break;
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
            int count = keys.Length;
            if (count != 0)
            {
                foreach (KT key in keys.Array)
                {
                    Append(key, value);
                    if (--count == 0) break;
                }
            }
        }
        /// <summary>
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        /// <returns>返回 false 表示关键字数据为 null 或者没有找到索引关键字</returns>
        public bool RemoveLoadPersistence(KT key, VT value)
        {
            if (key != null && value != null)
            {
                var index = default(RemoveMarkHashIndex<KT, VT>);
                if (indexs.TryGetValue(key, out index)) index.RemoveLoadPersistence(value);
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
            if (key != null && value != null)
            {
                var index = default(RemoveMarkHashIndex<KT, VT>);
                if (indexs.TryGetValue(key, out index))
                {
                    index.Remove(value);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        public void RemoveArrayLoadPersistence(KT[] keys, VT value)
        {
            if (keys != null)
            {
                foreach (KT key in keys) RemoveLoadPersistence(key, value);
            }
        }
        /// <summary>
        /// 删除匹配数据关键字
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
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        public void RemoveLeftArrayLoadPersistence(LeftArray<KT> keys, VT value)
        {
            int count = keys.Length;
            if (count != 0)
            {
                foreach (KT key in keys.Array)
                {
                    RemoveLoadPersistence(key, value);
                    if (--count == 0) break;
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
            int count = keys.Length;
            if (count != 0)
            {
                foreach (KT key in keys.Array)
                {
                    Remove(key, value);
                    if (--count == 0) break;
                }
            }
        }
        /// <summary>
        /// 磁盘块索引信息写入完成操作
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <param name="valueCount">新增数据数量</param>
        public void WriteCompletedLoadPersistence(KT key, BlockIndex blockIndex, int valueCount)
        {
            var index = default(RemoveMarkHashIndex<KT, VT>);
            if (indexs.TryGetValue(key, out index)) index.WriteCompletedLoadPersistence(blockIndex, valueCount);
        }
        /// <summary>
        /// 磁盘块索引信息写入完成操作
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <param name="valueCount">新增数据数量</param>
        public void WriteCompleted(KT key, BlockIndex blockIndex, int valueCount)
        {
            var index = default(RemoveMarkHashIndex<KT, VT>);
            if (indexs.TryGetValue(key, out index)) index.WriteCompleted(blockIndex, valueCount);
        }
        /// <summary>
        /// 获取索引数据磁盘块索引信息
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <returns></returns>
        public BlockIndexData<VT> GetBlockIndexData(KT key)
        {
            if (key != null)
            {
                var index = default(RemoveMarkHashIndex<KT, VT>);
                if (indexs.TryGetValue(key, out index)) return index.GetBlockIndexData();
            }
            return default(BlockIndexData<VT>);
        }
        /// <summary>
        /// 获取索引数据磁盘块索引信息
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <returns></returns>
        public BlockIndexData<VT>[] GetBlockIndexDataArray(KT[] keys)
        {
            if (keys != null && keys.Length != 0)
            {
                BlockIndexData<VT>[] array = new BlockIndexData<VT>[keys.Length];
                var index = default(RemoveMarkHashIndex<KT, VT>);
                int arrayIndex = 0;
                foreach (KT key in keys)
                {
                    if (key != null)
                    {
                        if (indexs.TryGetValue(key, out index)) array[arrayIndex] = index.GetBlockIndexData();
                    }
                    ++arrayIndex;
                }
                return array;
            }
            return EmptyArray<BlockIndexData<VT>>.Array;
        }
        /// <summary>
        /// 获取更新关键字集合
        /// </summary>
        /// <param name="callback">获取更新关键字集合回调</param>
        public void GetChangeKeys(MethodKeepCallback<KT> callback)
        {
            bool isCallback = false;
            try
            {
                callbacks.Add(callback);
                isCallback = true;
            }
            finally
            {
                if (!isCallback) callback.CancelKeep();
            }
        }
        /// <summary>
        /// 获取更新关键字集合回调
        /// </summary>
        /// <param name="key"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Callback(KT key)
        {
            MethodKeepCallback<KT>.Callback(ref callbacks, key);
        }
    }
}
