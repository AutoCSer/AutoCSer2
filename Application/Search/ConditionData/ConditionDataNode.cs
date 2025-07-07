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
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
    public abstract class ConditionDataNode<NT, KT, VT> : MethodParameterCreatorNode<NT, VT>, ISnapshot<VT>
        where NT : IConditionDataNode<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
        /// <summary>
        /// Get the snapshot data collection container size for pre-applying snapshot data containers
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        protected abstract int snapshotCapacity { get; }
        /// <summary>
        /// Get the snapshot data collection
        /// 获取快照数据集合
        /// </summary>
        protected abstract IEnumerable<VT> snapshotValues { get; }
        /// <summary>
        /// 初始化加载数据数组大小，默认为 1024
        /// </summary>
        protected virtual int loadArraySize { get { return 1 << 10; } }
        /// <summary>
        /// 是否已经加载初始化数据
        /// </summary>
        protected bool isLoaded;
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
        public ISnapshotEnumerable<bool> SnapshotEnumerable { get { return new SnapshotGetValueEmpty<bool>(getIsLoaded); } }
        /// <summary>
        /// 非索引条件查询数据节点
        /// </summary>
        protected ConditionDataNode() { }
        /// <summary>
        /// Initialization loading is completed and processed
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>The new node that has been loaded and replaced
        /// 加载完毕替换的新节点</returns>
#if NetStandard21
        public override NT? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override NT StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            if (!isLoaded) load().Catch();
            return default(NT);
        }
        /// <summary>
        /// 初始化加载所有数据
        /// </summary>
        /// <returns></returns>
        protected abstract Task load();
        /// <summary>
        /// 获取初始化加载所有数据命令
        /// </summary>
        /// <returns></returns>
        protected abstract EnumeratorCommand<VT> getLoadCommand();
        /// <summary>
        /// Get the snapshot data collection container size for pre-applying snapshot data containers
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>The size of the snapshot data collection container
        /// 快照数据集合容器大小</returns>
        int ISnapshot<VT>.GetSnapshotCapacity(ref object customObject)
        {
            return snapshotCapacity;
        }
        /// <summary>
        /// Get the snapshot data collection. If the data object may be modified, the cloned data object should be returned to prevent the data from being modified during the snapshot establishment
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>Snapshot data
        /// 快照数据</returns>
        SnapshotResult<VT> ISnapshot<VT>.GetSnapshotResult(VT[] snapshotArray, object customObject)
        {
            SnapshotResult<VT> result = new SnapshotResult<VT>(snapshotCapacity, snapshotArray.Length);
            foreach (VT value in snapshotValues) result.Add(snapshotArray, value);
            return result;
        }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        public abstract void SnapshotSet(VT value);
        /// <summary>
        /// 是否已经加载初始化数据
        /// </summary>
        /// <returns></returns>
        private KeyValue<bool, bool> getIsLoaded()
        {
            if (isLoaded) return new KeyValue<bool, bool>(true, true);
            return default(KeyValue<bool, bool>);
        }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        public void SnapshotSetLoaded(bool value)
        {
            isLoaded = value;
        }
        /// <summary>
        /// The initialization data loading has been completed
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
        /// 判断是否存在关键字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool Contains(VT value);
        /// <summary>
        /// Create non-indexed conditional query data (Check the input parameters before the persistence operation)
        /// 创建非索引条件查询数据（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        /// <returns>Returning true indicates that a persistence operation is required
        /// 返回 true 表示需要持久化操作</returns>
        public abstract bool LoadCreateBeforePersistence(VT value);
        /// <summary>
        /// Create non-indexed conditional query data (Initialize and load the persistent data)
        /// 创建非索引条件查询数据（初始化加载持久化数据）
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        public abstract void LoadCreateLoadPersistence(VT value);
        /// <summary>
        /// Create non-indexed conditional query data
        /// 创建非索引条件查询数据
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        public abstract void LoadCreate(VT value);
        /// <summary>
        /// Create non-indexed conditional query data
        /// 创建非索引条件查询数据
        /// </summary>
        /// <param name="key">Data keyword</param>
        /// <param name="callback"></param>
        public void Create(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            if (key != null) create(key, callback);
            else callback.Callback(ConditionDataUpdateStateEnum.NullKey);
        }
        /// <summary>
        /// Create non-indexed conditional query data
        /// 创建非索引条件查询数据
        /// </summary>
        /// <param name="key">Data keyword</param>
        /// <param name="callback"></param>
        protected abstract void create(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback);
        /// <summary>
        /// Update non-indexed condition query data
        /// 更新非索引条件查询数据
        /// </summary>
        /// <param name="key">Data keyword</param>
        /// <param name="callback"></param>
        public void Update(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            if (key != null) update(key, callback);
            else callback.Callback(ConditionDataUpdateStateEnum.NullKey);
        }
        /// <summary>
        /// Update non-indexed condition query data
        /// 更新非索引条件查询数据
        /// </summary>
        /// <param name="key">Data keyword</param>
        /// <param name="callback"></param>
        protected abstract void update(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback);
        /// <summary>
        /// Delete non-indexed condition query data (Initialize and load the persistent data)
        /// 删除非索引条件查询数据（初始化加载持久化数据）
        /// </summary>
        /// <param name="key">Data keyword</param>
        /// <param name="callback"></param>
        public void DeleteLoadPersistence(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            if (key != null) deleteLoadPersistence(key);
        }
        /// <summary>
        /// Delete non-indexed condition query data (Initialize and load the persistent data)
        /// 删除非索引条件查询数据（初始化加载持久化数据）
        /// </summary>
        /// <param name="key">Data keyword</param>
        protected abstract void deleteLoadPersistence(KT key);
        /// <summary>
        /// Delete non-indexed condition query data
        /// 删除非索引条件查询数据
        /// </summary>
        /// <param name="key">Data keyword</param>
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
        /// Delete non-indexed condition query data
        /// 删除非索引条件查询数据
        /// </summary>
        /// <param name="key">Data keyword</param>
        /// <returns></returns>
        protected abstract ConditionDataUpdateStateEnum delete(KT key);
        /// <summary>
        /// The non-indexed condition query data has completed the update operation (Check the input parameters before the persistence operation)
        /// 非索引条件查询数据完成更新操作（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        /// <returns></returns>
        public abstract ValueResult<ConditionDataUpdateStateEnum> CompletedBeforePersistence(VT value);
        /// <summary>
        /// The non-indexed condition query data has completed the update operation (Initialize and load the persistent data)
        /// 非索引条件查询数据完成更新操作（初始化加载持久化数据）
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        /// <param name="callback"></param>
        public void CompletedLoadPersistence(VT value, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            completedLoadPersistence(value);
        }
        /// <summary>
        /// The non-indexed condition query data has completed the update operation (Initialize and load the persistent data)
        /// 非索引条件查询数据完成更新操作（初始化加载持久化数据）
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        protected abstract void completedLoadPersistence(VT value);
        /// <summary>
        /// The non-indexed condition query data has completed the update operation
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        /// <param name="callback"></param>
        public void Completed(VT value, MethodCallback<ConditionDataUpdateStateEnum> callback)
        {
            ConditionDataUpdateStateEnum state = ConditionDataUpdateStateEnum.Unknown;
            try
            {
                state = completed(value);
            }
            finally { callback.Callback(state); }
        }
        /// <summary>
        /// The non-indexed condition query data has completed the update operation
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        /// <returns></returns>
        protected abstract ConditionDataUpdateStateEnum completed(VT value);
    }
    /// <summary>
    /// 非索引条件查询数据节点
    /// </summary>
    /// <typeparam name="NT">节点接口类型</typeparam>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
    /// <typeparam name="CT">客户端节点类型</typeparam>
    public abstract class ConditionDataNode<NT, KT, VT, CT> : ConditionDataNode<NT, KT, VT>
        where NT : IConditionDataNode<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
        where CT : class
    {
        /// <summary>
        /// 初始化加载数据获取非索引条件查询数据节点单例
        /// </summary>
        protected readonly StreamPersistenceMemoryDatabaseClientNodeCache<CT> loadClientNode;
        /// <summary>
        /// 非索引条件查询数据节点
        /// </summary>
        /// <param name="loadClientNode"></param>
        protected ConditionDataNode(StreamPersistenceMemoryDatabaseClientNodeCache<CT> loadClientNode)
        {
            this.loadClientNode = loadClientNode;
        }
        /// <summary>
        /// 初始化加载所有数据
        /// </summary>
        /// <returns></returns>
        protected override async Task load()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            var command = default(EnumeratorCommand<VT>);
            do
            {
                try
                {
                    ResponseResult<CT> nodeResult = await loadClientNode.GetSynchronousNode();
                    if (nodeResult.IsSuccess)
                    {
                        command = getLoadCommand();
                        if (command != null) command = await command;
                        if (command != null)
                        {
                            CT node = nodeResult.Value.notNull();
                            VT[] values = new VT[Math.Max(loadArraySize, 1)];
                            LoadCallback<NT, KT, VT, CT> callback = new LoadCallback<NT, KT, VT, CT>(this, values);
                            int index = 0;
                            while (await command.MoveNext())
                            {
                                values[index++] = command.Current;
                                if (index == values.Length)
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
                                    if (index != values.Length) Array.Resize(ref callback.Values, index);
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
        /// Create the disk block index information of the word segmentation result
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="node">客户端节点信息</param>
        /// <param name="callback">初始化加载数据回调</param>
        /// <returns>0 表示成功，-1 表示失败</returns>
        private async Task<int> create(CT node, LoadCallback<NT, KT, VT, CT> callback)
        {
            StreamPersistenceMemoryDatabaseCallQueue.AppendWriteOnly(callback);
            await callback.Wait();
            VT[] values = callback.Values;
            ResponseResultAwaiter[] responses = callback.CreateResponses;
            int index = callback.NewCount;
            while (index > 0)
            {
                --index;
                responses[index] = loadCreate(node, values[index]);
            }
            for (index = callback.NewCount; index > 0;)
            {
                ResponseResult state = await responses[--index];
                if (!state.IsSuccess) return -1;
            }
            return index;
        }
        /// <summary>
        /// Create the disk block index information of the word segmentation result
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected abstract ResponseResultAwaiter loadCreate(CT client, VT value);
    }
}
