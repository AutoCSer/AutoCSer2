using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.DiskBlockIndex
{
    /// <summary>
    /// 带移除标记的可重用哈希索引节点
    /// </summary>
    /// <typeparam name="T">索引关键字类型</typeparam>
    public abstract class RemoveMarkHashKeyIndexNode<T> : MethodParameterCreatorNode<IRemoveMarkHashKeyIndexNode<T>, BinarySerializeKeyValue<T, BlockIndexData<uint>>>, IRemoveMarkHashKeyIndexNode<T>, ISnapshot<BinarySerializeKeyValue<T, BlockIndexData<uint>>>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 关键字索引集合
        /// </summary>
        private readonly Dictionary<T, RemoveMarkHashKeyIndex<T>> indexs;
        /// <summary>
        /// 关键字变更回调
        /// </summary>
        private LeftArray<MethodKeepCallback<T>> callbacks;
        /// <summary>
        /// 带移除标记的可重用哈希索引节点
        /// </summary>
        /// <param name="capacity">初始化容器尺寸</param>
        protected RemoveMarkHashKeyIndexNode(int capacity = 0)
        {
            callbacks.SetEmpty();
            indexs = DictionaryCreator<T>.Create<RemoveMarkHashKeyIndex<T>>(capacity);
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
#if NetStandard21
        public override IRemoveMarkHashKeyIndexNode<T>? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override IRemoveMarkHashKeyIndexNode<T> StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            foreach (RemoveMarkHashKeyIndex<T> index in indexs.Values) index.Loaded();
            return null;
        }
        /// <summary>
        /// 释放索引节点
        /// </summary>
        private void free()
        {
            foreach (RemoveMarkHashKeyIndex<T> index in indexs.Values) index.Values.Free();
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
        int ISnapshot<BinarySerializeKeyValue<T, BlockIndexData<uint>>>.GetSnapshotCapacity(ref object customObject)
        {
            return indexs.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        SnapshotResult<BinarySerializeKeyValue<T, BlockIndexData<uint>>> ISnapshot<BinarySerializeKeyValue<T, BlockIndexData<uint>>>.GetSnapshotResult(BinarySerializeKeyValue<T, BlockIndexData<uint>>[] snapshotArray, object customObject)
        {
            SnapshotResult<BinarySerializeKeyValue<T, BlockIndexData<uint>>> result = new SnapshotResult<BinarySerializeKeyValue<T, BlockIndexData<uint>>>(indexs.Count, snapshotArray.Length);
            foreach (KeyValuePair<T, RemoveMarkHashKeyIndex<T>> index in indexs) result.Add(snapshotArray, new BinarySerializeKeyValue<T, BlockIndexData<uint>>(index.Key, index.Value.GetBlockIndexData(false)));
            return result;
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSet(BinarySerializeKeyValue<T, BlockIndexData<uint>> value)
        {
            indexs.Add(value.Key, new RemoveMarkHashKeyIndex<T>(this, value.Key, ref value.Value));
        }
        /// <summary>
        /// 根据索引关键字获取磁盘块索引信息客户端
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <returns></returns>
        public abstract IDiskBlockClient GetDiskBlockClient(T key);
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
        public bool AppendLoadPersistence(T key, uint value)
        {
            if (key != null)
            {
                var index = default(RemoveMarkHashKeyIndex<T>);
                if (indexs.TryGetValue(key, out index)) index.AppendLoadPersistence(value);
                else indexs.Add(key, new RemoveMarkHashKeyIndex<T>(this, key, value));
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
            if (key != null)
            {
                var index = default(RemoveMarkHashKeyIndex<T>);
                if (indexs.TryGetValue(key, out index)) index.Append(value);
                else
                {
                    indexs.Add(key, new RemoveMarkHashKeyIndex<T>(this, key, value));
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
        public void AppendArrayLoadPersistence(T[] keys, uint value)
        {
            if (keys != null)
            {
                foreach (T key in keys) AppendLoadPersistence(key, value);
            }
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
                foreach (T key in keys) Append(key, value);
            }
        }
        /// <summary>
        /// 添加匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <param name="value">匹配数据关键字</param>
        public void AppendLeftArrayLoadPersistence(LeftArray<T> keys, uint value)
        {
            int count = keys.Length;
            if (count != 0)
            {
                foreach (T key in keys.Array)
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
        public void AppendLeftArray(LeftArray<T> keys, uint value)
        {
            int count = keys.Length;
            if (count != 0)
            {
                foreach (T key in keys.Array)
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
        public bool RemoveLoadPersistence(T key, uint value)
        {
            if (key != null)
            {
                var index = default(RemoveMarkHashKeyIndex<T>);
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
        public bool Remove(T key, uint value)
        {
            if (key != null)
            {
                var index = default(RemoveMarkHashKeyIndex<T>);
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
        public void RemoveArrayLoadPersistence(T[] keys, uint value)
        {
            if (keys != null)
            {
                foreach (T key in keys) RemoveLoadPersistence(key, value);
            }
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
                foreach (T key in keys) Remove(key, value);
            }
        }
        /// <summary>
        /// 删除匹配数据关键字
        /// </summary>
        /// <param name="keys">索引关键字</param>
        /// <param name="value">匹配数据关键字</param>
        public void RemoveLeftArrayLoadPersistence(LeftArray<T> keys, uint value)
        {
            int count = keys.Length;
            if (count != 0)
            {
                foreach (T key in keys.Array)
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
        public void RemoveLeftArray(LeftArray<T> keys, uint value)
        {
            int count = keys.Length;
            if (count != 0)
            {
                foreach (T key in keys.Array)
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
        public void WriteCompletedLoadPersistence(T key, BlockIndex blockIndex, int valueCount)
        {
            var index = default(RemoveMarkHashKeyIndex<T>);
            if (indexs.TryGetValue(key, out index)) index.WriteCompletedLoadPersistence(blockIndex, valueCount);
        }
        /// <summary>
        /// 磁盘块索引信息写入完成操作
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <param name="valueCount">新增数据数量</param>
        public void WriteCompleted(T key, BlockIndex blockIndex, int valueCount)
        {
            var index = default(RemoveMarkHashKeyIndex<T>);
            if (indexs.TryGetValue(key, out index)) index.WriteCompleted(blockIndex, valueCount);
        }
        /// <summary>
        /// 获取索引数据磁盘块索引信息
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <returns></returns>
        public BlockIndexData<uint> GetBlockIndexData(T key)
        {
            if (key != null)
            {
                var index = default(RemoveMarkHashKeyIndex<T>);
                if (indexs.TryGetValue(key, out index)) return index.GetBlockIndexData(true);
            }
            return default(BlockIndexData<uint>);
        }
        /// <summary>
        /// 获取索引数据磁盘块索引信息
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <returns></returns>
        public BlockIndexData<uint>[] GetBlockIndexDataArray(T[] keys)
        {
            if (keys != null && keys.Length != 0)
            {
                BlockIndexData<uint>[] array = new BlockIndexData<uint>[keys.Length];
                var index = default(RemoveMarkHashKeyIndex<T>);
                int arrayIndex = 0;
                foreach (T key in keys)
                {
                    if (key != null)
                    {
                        if (indexs.TryGetValue(key, out index)) array[arrayIndex] = index.GetBlockIndexData(true);
                    }
                    ++arrayIndex;
                }
                return array;
            }
            return EmptyArray<BlockIndexData<uint>>.Array;
        }
        /// <summary>
        /// 获取索引数据磁盘块索引信息
        /// </summary>
        /// <param name="key">索引关键字</param>
        /// <returns></returns>
        public BlockIndexData<int> GetIntBlockIndexData(T key)
        {
            if (key != null)
            {
                var index = default(RemoveMarkHashKeyIndex<T>);
                if (indexs.TryGetValue(key, out index)) return index.GetIntBlockIndexData(true);
            }
            return default(BlockIndexData<int>);
        }
        /// <summary>
        /// 获取索引数据磁盘块索引信息
        /// </summary>
        /// <param name="keys">索引关键字集合</param>
        /// <returns></returns>
        public BlockIndexData<int>[] GetIntBlockIndexDataArray(T[] keys)
        {
            if (keys != null && keys.Length != 0)
            {
                BlockIndexData<int>[] array = new BlockIndexData<int>[keys.Length];
                var index = default(RemoveMarkHashKeyIndex<T>);
                int arrayIndex = 0;
                foreach (T key in keys)
                {
                    if (key != null)
                    {
                        if (indexs.TryGetValue(key, out index)) array[arrayIndex] = index.GetIntBlockIndexData(true);
                    }
                    ++arrayIndex;
                }
                return array;
            }
            return EmptyArray<BlockIndexData<int>>.Array;
        }
        /// <summary>
        /// 获取更新关键字集合
        /// </summary>
        /// <param name="callback">获取更新关键字集合回调</param>
        public void GetChangeKeys(MethodKeepCallback<T> callback)
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
        internal void Callback(T key)
        {
            MethodKeepCallback<T>.Callback(ref callbacks, key);
        }
    }
}
