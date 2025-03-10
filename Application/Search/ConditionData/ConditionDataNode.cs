using AutoCSer.CommandService.Search.ConditionData;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 非索引条件查询数据节点
    /// </summary>
    /// <typeparam name="NT">节点接口类型</typeparam>
    /// <typeparam name="KT">关键字类型</typeparam>
    /// <typeparam name="VT">数据类型</typeparam>
    /// <typeparam name="CT">客户端节点类型</typeparam>
    public abstract class ConditionDataNode<NT, KT, VT, CT> : MethodParameterCreatorNode<NT, VT>, ISnapshot<VT>
        where NT : IConditionDataNode<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
        where VT : notnull, IConditionData
#else
        where KT : IEquatable<KT>
        where VT : IConditionData
#endif
        where CT : class
    {
        /// <summary>
        /// 初始化加载数据获取非索引条件查询数据节点单例
        /// </summary>
        protected abstract StreamPersistenceMemoryDatabaseClientNodeCache<CT> loadClientNode { get; }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        protected abstract int snapshotCapacity { get; }
        /// <summary>
        /// 获取快照数据集合
        /// </summary>
        protected abstract IEnumerable<VT> snapshotValues { get; }
        /// <summary>
        /// 初始化加载数据数组大小，默认为 1024
        /// </summary>
        protected int loadArraySize { get { return 1 << 10; } }
        /// <summary>
        /// 是否已经加载初始化数据
        /// </summary>
        protected bool isLoaded;
        /// <summary>
        /// 非索引条件查询数据节点
        /// </summary>
        protected ConditionDataNode() { }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
#if NetStandard21
        public override NT? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override NT StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            if (!isLoaded) load().NotWait();
            return default(NT);
        }
        /// <summary>
        /// 获取初始化加载所有关键字数据命令
        /// </summary>
        /// <returns></returns>
        protected abstract EnumeratorCommand<KT> getLoadCommand();
        /// <summary>
        /// 初始化加载所有数据
        /// </summary>
        /// <returns></returns>
        protected virtual async Task load()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            var command = default(EnumeratorCommand<KT>);
            do
            {
                try
                {
                    ResponseResult<CT> nodeResult = await loadClientNode.GetSynchronousNode();
                    if (nodeResult.IsSuccess)
                    {
                        command = await getLoadCommand();
                        if (command != null)
                        {
                            CT node = nodeResult.Value.notNull();
                            KT[] keys = new KT[Math.Max(loadArraySize, 1)];
                            LoadCallback<NT, KT, VT, CT> callback = new LoadCallback<NT, KT, VT, CT>(this, keys);
                            int index = 0;
                            while (await command.MoveNext())
                            {
                                keys[index++] = command.Current;
                                if (index == keys.Length)
                                {
                                    if (StreamPersistenceMemoryDatabaseNodeIsRemoved) return;
                                    index = await create(node, callback);
                                }
                            }
                            if (index >= 0)
                            {
                                if (index > 0)
                                {
                                    if (StreamPersistenceMemoryDatabaseNodeIsRemoved) return;
                                    if (index != keys.Length) Array.Resize(ref callback.Keys, index);
                                    index = await create(node, callback);
                                }
                                if (index == 0 && command.ReturnType == CommandClientReturnTypeEnum.Success)
                                {
                                    StreamPersistenceMemoryDatabaseMethodParameterCreator.Loaded();
                                    command = null;
                                    return;
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    await AutoCSer.LogHelper.Exception(exception);
                }
                finally
                {
                    if (command != null)
                    {
                        await ((IAsyncDisposable)command).DisposeAsync();
                        command = null;
                    }
                }
                await Task.Delay(1000);
            }
            while (!StreamPersistenceMemoryDatabaseNodeIsRemoved);
        }
        /// <summary>
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="node">客户端节点信息</param>
        /// <param name="callback">初始化加载数据回调</param>
        /// <returns>0 表示成功，-1 表示失败</returns>
        private async Task<int> create(CT node, LoadCallback<NT, KT, VT, CT> callback)
        {
            StreamPersistenceMemoryDatabaseCallQueue.AddOnly(callback);
            await callback.Wait();
            KT[] keys = callback.Keys;
            ResponseParameterAwaiter<ConditionDataUpdateStateEnum>[] responses = callback.CreateResponses;
            int index = callback.NewCount;
            while (index > 0)
            {
                --index;
                responses[index] = loadCreate(node, keys[index]);
            }
            for (index = callback.NewCount; index > 0;)
            {
                ResponseResult<ConditionDataUpdateStateEnum> state = await responses[--index];
                if (!state.IsSuccess || state.Value != ConditionDataUpdateStateEnum.Success) return -1;
            }
            return index;
        }
        /// <summary>
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract ResponseParameterAwaiter<ConditionDataUpdateStateEnum> loadCreate(CT client, KT key);
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int ISnapshot<VT>.GetSnapshotCapacity(ref object customObject)
        {
            return snapshotCapacity;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        SnapshotResult<VT> ISnapshot<VT>.GetSnapshotResult(VT[] snapshotArray, object customObject)
        {
            SnapshotResult<VT> result = new SnapshotResult<VT>(snapshotCapacity, snapshotArray.Length);
            foreach (VT value in snapshotValues) result.Add(snapshotArray, value);
            return result;
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public abstract void SnapshotSet(VT value);
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        public SnapshotResult<bool> GetSnapshotResult(bool[] snapshotArray, object customObject)
        {
            if (isLoaded) return new SnapshotResult<bool>(snapshotArray, true);
            return new SnapshotResult<bool>(0);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        public void SetSnapshotResult(ref LeftArray<bool> array, ref LeftArray<bool> newArray) { }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSetLoaded(bool value)
        {
            isLoaded = value;
        }
        /// <summary>
        /// 初始化数据加载完成
        /// </summary>
        public void Loaded()
        {
            isLoaded = true;
        }
        /// <summary>
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract bool ContainsKey(KT key);
        /// <summary>
        /// 创建非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="callback"></param>
        public void Create(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            if (key != null) create(key, callback);
            else callback.Callback(ConditionDataUpdateStateEnum.NullKey);
        }
        /// <summary>
        /// 创建非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="callback"></param>
        protected abstract void create(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback);
        /// <summary>
        /// 更新非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="callback"></param>
        public void Update(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            if (key != null) update(key, callback);
            else callback.Callback(ConditionDataUpdateStateEnum.NullKey);
        }
        /// <summary>
        /// 更新非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="callback"></param>
        protected abstract void update(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback);
        /// <summary>
        /// 删除非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="callback"></param>
        public void DeleteLoadPersistence(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            if (key != null) deleteLoadPersistence(key);
        }
        /// <summary>
        /// 删除非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        protected abstract void deleteLoadPersistence(KT key);
        /// <summary>
        /// 删除非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="callback"></param>
        public void Delete(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            ConditionDataUpdateStateEnum state = ConditionDataUpdateStateEnum.Unknown;
            try
            {
                if (key != null) state = delete(key);
                else state = ConditionDataUpdateStateEnum.NullKey;
            }
            finally { callback.Callback(state); }
        }
        /// <summary>
        /// 删除非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <returns></returns>
        protected abstract ConditionDataUpdateStateEnum delete(KT key);
        /// <summary>
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="value">非索引条件查询数据</param>
        /// <returns></returns>
        public ValueResult<ConditionDataUpdateStateEnum> CompletedBeforePersistence(KT key, VT value)
        {
            if (key != null) return completedBeforePersistence(key, value);
            return ConditionDataUpdateStateEnum.NullKey;
        }
        /// <summary>
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="value">非索引条件查询数据</param>
        /// <returns></returns>
        protected abstract ValueResult<ConditionDataUpdateStateEnum> completedBeforePersistence(KT key, VT value);
        /// <summary>
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="value">非索引条件查询数据</param>
        /// <param name="callback"></param>
        public void CompletedLoadPersistence(KT key, VT value, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            if (key != null) completedLoadPersistence(key, value);
        }
        /// <summary>
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="value">非索引条件查询数据</param>
        protected abstract void completedLoadPersistence(KT key, VT value);
        /// <summary>
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="value">非索引条件查询数据</param>
        /// <param name="callback"></param>
        public void Completed(KT key, VT value, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            ConditionDataUpdateStateEnum state = ConditionDataUpdateStateEnum.Unknown;
            try
            {
                if (key != null) state = completed(key, value);
                else state = ConditionDataUpdateStateEnum.NullKey;
            }
            finally { callback.Callback(state); }
        }
        /// <summary>
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="value">非索引条件查询数据</param>
        /// <returns></returns>
        protected abstract ConditionDataUpdateStateEnum completed(KT key, VT value);
    }
}
