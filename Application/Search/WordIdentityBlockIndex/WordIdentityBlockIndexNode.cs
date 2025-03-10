using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.Search.StaticTrieGraph;
using AutoCSer.CommandService.Search.WordIdentityBlockIndex;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 分词结果磁盘块索引信息节点
    /// </summary>
    /// <typeparam name="T">分词数据关键字类型</typeparam>
    public abstract class WordIdentityBlockIndexNode<T> : MethodParameterCreatorNode<IWordIdentityBlockIndexNode<T>, BinarySerializeKeyValue<T, BlockIndex>>, IWordIdentityBlockIndexNode<T>, ISnapshot<BinarySerializeKeyValue<T, BlockIndex>>, ISnapshot<bool>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 分词数据磁盘块索引信息集合
        /// </summary>
        internal readonly FragmentDictionary256<T, WordIdentityBlockIndex<T>> Datas;
        /// <summary>
        /// 初始化加载数据获取分词结果磁盘块索引信息节点单例
        /// </summary>
        protected abstract StreamPersistenceMemoryDatabaseClientNodeCache<IWordIdentityBlockIndexNodeClientNode<T>> loadClientNode { get; }
        /// <summary>
        /// 获取字符串 Trie 图节点单例
        /// </summary>
        protected abstract StreamPersistenceMemoryDatabaseClientNodeCache<IStaticTrieGraphNodeClientNode> trieGraphNode { get; }
        /// <summary>
        /// 初始化加载数据数组大小，默认为 1024
        /// </summary>
        protected int loadArraySize { get { return 1 << 10; } }
        /// <summary>
        /// 是否已经加载初始化数据
        /// </summary>
        private bool isLoaded;
        /// <summary>
        /// 分词结果磁盘块索引信息节点
        /// </summary>
        protected WordIdentityBlockIndexNode() 
        {
            Datas = new FragmentDictionary256<T, WordIdentityBlockIndex<T>>();
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
#if NetStandard21
        public override IWordIdentityBlockIndexNode<T>? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override IWordIdentityBlockIndexNode<T> StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            foreach (KeyValuePair<T, WordIdentityBlockIndex<T>> data in Datas.KeyValues) data.Value.Loaded(this, data.Key);
            if (!isLoaded) load().NotWait();
            return null;
        }
        /// <summary>
        /// 获取初始化加载所有数据命令
        /// </summary>
        /// <returns></returns>
        protected abstract EnumeratorCommand<T> getLoadCommand();
        /// <summary>
        /// 初始化加载所有数据
        /// </summary>
        /// <returns></returns>
        protected virtual async Task load()
        {
            await AutoCSer.Threading.SwitchAwaiter.Default;
            var command = default(EnumeratorCommand<T>);
            do
            {
                try
                {
                    ResponseResult<IWordIdentityBlockIndexNodeClientNode<T>> nodeResult = await loadClientNode.GetSynchronousNode();
                    if (nodeResult.IsSuccess)
                    {
                        command = await getLoadCommand();
                        if (command != null)
                        {
                            IWordIdentityBlockIndexNodeClientNode<T> node = nodeResult.Value.notNull();
                            T[] keys = new T[Math.Max(loadArraySize, 1)];
                            LoadCallback<T> callback = new LoadCallback<T>(this, keys);
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
        private async Task<int> create(IWordIdentityBlockIndexNodeClientNode<T> node, LoadCallback<T> callback)
        {
            StreamPersistenceMemoryDatabaseCallQueue.AddOnly(callback);
            await callback.Wait();
            T[] keys = callback.Keys;
            ResponseParameterAwaiter<WordIdentityBlockIndexUpdateStateEnum>[] responses = callback.CreateResponses;
            int index = callback.NewCount;
            while (index > 0)
            {
                --index;
                responses[index] = node.Create(keys[index]);
            }
            for (index = callback.NewCount; index > 0;)
            {
                ResponseResult<WordIdentityBlockIndexUpdateStateEnum> state = await responses[--index];
                if (!state.IsSuccess || state.Value != WordIdentityBlockIndexUpdateStateEnum.Success) return -1;
            }
            return index;
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int ISnapshot<BinarySerializeKeyValue<T, BlockIndex>>.GetSnapshotCapacity(ref object customObject)
        {
            return Datas.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        SnapshotResult<BinarySerializeKeyValue<T, BlockIndex>> ISnapshot<BinarySerializeKeyValue<T, BlockIndex>>.GetSnapshotResult(BinarySerializeKeyValue<T, BlockIndex>[] snapshotArray, object customObject)
        {
            SnapshotResult<BinarySerializeKeyValue<T, BlockIndex>> result = new SnapshotResult<BinarySerializeKeyValue<T, BlockIndex>>(Datas.Count, snapshotArray.Length);
            foreach (KeyValuePair<T, WordIdentityBlockIndex<T>> data in Datas.KeyValues) result.Add(snapshotArray, new BinarySerializeKeyValue<T, BlockIndex>(data.Key, data.Value.BlockIndex));
            return result;
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSet(BinarySerializeKeyValue<T, BlockIndex> value)
        {
            Datas.Add(value.Key, new WordIdentityBlockIndex<T>(value.Value));
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int ISnapshot<bool>.GetSnapshotCapacity(ref object customObject)
        {
            return isLoaded ? 1 : 0;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        SnapshotResult<bool> ISnapshot<bool>.GetSnapshotResult(bool[] snapshotArray, object customObject)
        {
            if (isLoaded) return new SnapshotResult<bool>(snapshotArray, true);
            return new SnapshotResult<bool>(0);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        void ISnapshot<bool>.SetSnapshotResult(ref LeftArray<bool> array, ref LeftArray<bool> newArray) { }
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
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        public void CreateLoadPersistence(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            if (key != null)
            {
                var data = default(WordIdentityBlockIndex<T>);
                if (Datas.TryGetValue(key, out data)) data.IsLoadedDeleted = false;
                else Datas.Add(key, new WordIdentityBlockIndex<T>());
            }
        }
        /// <summary>
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        public void Create(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            WordIdentityBlockIndexUpdateStateEnum state = WordIdentityBlockIndexUpdateStateEnum.Unknown;
            try
            {
                if (key != null)
                {
                    if (!Datas.ContainsKey(key))
                    {
                        WordIdentityBlockIndex<T> data = new WordIdentityBlockIndex<T>();
                        Datas.Add(key, data);
                        data.Create(this, key, callback).NotWait();
                        state = WordIdentityBlockIndexUpdateStateEnum.Callbacked;
                    }
                    else state = WordIdentityBlockIndexUpdateStateEnum.Success;
                }
                else state = WordIdentityBlockIndexUpdateStateEnum.NullKey;
            }
            finally
            {
                if (state != WordIdentityBlockIndexUpdateStateEnum.Callbacked) callback.Callback(state);
            }
        }
        /// <summary>
        /// 更新分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        public void UpdateLoadPersistence(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            CreateLoadPersistence(key, callback);
        }
        /// <summary>
        /// 更新分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        public void Update(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            WordIdentityBlockIndexUpdateStateEnum state = WordIdentityBlockIndexUpdateStateEnum.Unknown;
            try
            {
                if (key != null)
                {
                    var data = default(WordIdentityBlockIndex<T>);
                    if (Datas.TryGetValue(key, out data)) data.Update(this, key, callback).NotWait();
                    else
                    {
                        Datas.Add(key, data = new WordIdentityBlockIndex<T>());
                        data.Create(this, key, callback).NotWait();
                    }
                    state = WordIdentityBlockIndexUpdateStateEnum.Callbacked;
                }
                else state = WordIdentityBlockIndexUpdateStateEnum.NullKey;
            }
            finally
            {
                if (state != WordIdentityBlockIndexUpdateStateEnum.Callbacked) callback.Callback(state);
            }
        }
        /// <summary>
        /// 删除分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        public void DeleteLoadPersistence(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            if (key != null)
            {
                var data = default(WordIdentityBlockIndex<T>);
                if (Datas.TryGetValue(key, out data)) data.IsLoadedDeleted = true;
            }
        }
        /// <summary>
        /// 删除分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        public void Delete(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            WordIdentityBlockIndexUpdateStateEnum state = WordIdentityBlockIndexUpdateStateEnum.Unknown;
            try
            {
                if (key != null)
                {
                    var data = default(WordIdentityBlockIndex<T>);
                    if (Datas.TryGetValue(key, out data))
                    {
                        data.Delete(this, key, callback).NotWait();
                        state = WordIdentityBlockIndexUpdateStateEnum.Callbacked;
                    }
                    else state = WordIdentityBlockIndexUpdateStateEnum.Success;
                }
                else state = WordIdentityBlockIndexUpdateStateEnum.NullKey;
            }
            finally
            {
                if (state != WordIdentityBlockIndexUpdateStateEnum.Callbacked) callback.Callback(state);
            }
        }
        /// <summary>
        /// 分词结果磁盘块索引信息完成更新操作
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <param name="callback"></param>
        public void CompletedLoadPersistence(T key, BlockIndex blockIndex, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            var data = default(WordIdentityBlockIndex<T>);
            if (Datas.TryGetValue(key, out data)) data.BlockIndex = blockIndex;
        }
        /// <summary>
        /// 分词结果磁盘块索引信息完成更新操作
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <param name="callback"></param>
        public void Completed(T key, BlockIndex blockIndex, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            WordIdentityBlockIndexUpdateStateEnum state = WordIdentityBlockIndexUpdateStateEnum.Unknown;
            try
            {
                var data = default(WordIdentityBlockIndex<T>);
                if (Datas.TryGetValue(key, out data)) data.Completed(blockIndex).NotWait();
                state = WordIdentityBlockIndexUpdateStateEnum.Success;
            }
            finally { callback.Callback(state); }
        }
        /// <summary>
        /// 删除分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        public void DeletedLoadPersistence(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            Datas.Remove(key);
        }
        /// <summary>
        /// 删除分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        public void Deleted(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            WordIdentityBlockIndexUpdateStateEnum state = WordIdentityBlockIndexUpdateStateEnum.Unknown;
            try
            {
                Datas.Remove(key);
                state = WordIdentityBlockIndexUpdateStateEnum.Success;
            }
            finally { callback.Callback(state); }
        }
        /// <summary>
        /// 执行任务异常处理
        /// </summary>
        /// <param name="data"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public virtual Task OnException(WordIdentityBlockIndex<T> data, Exception exception) { return AutoCSer.Common.CompletedTask; }
        /// <summary>
        /// 根据关键字获取需要分词的文本数据
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <returns>null 表示没有找到关键字数据</returns>
#if NetStandard21
        public abstract AutoCSer.Net.ReturnCommand<string?> GetText(T key);
#else
        public abstract AutoCSer.Net.ReturnCommand<string> GetText(T key);
#endif
        /// <summary>
        /// 获取分词词语编号标识集合
        /// </summary>
        /// <param name="text">分词文本</param>
        /// <returns>词语编号标识集合</returns>
        public virtual async Task<ResponseResult<int[]>> GetWordIdentitys(string text)
        {
            ResponseResult<IStaticTrieGraphNodeClientNode> node = await trieGraphNode.GetSynchronousNode();
            if (!node.IsSuccess) return node.Cast<int[]>();
            return await node.Value.notNull().GetAddTextIdentity(text);
        }
        /// <summary>
        /// 根据分词数据关键字获取磁盘块索引信息客户端
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <returns></returns>
        public abstract IDiskBlockClient GetDiskBlockClient(T key);
        /// <summary>
        /// 获取磁盘块索引信息客户端
        /// </summary>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <returns></returns>
        public abstract IDiskBlockClient GetDiskBlockClient(BlockIndex blockIndex);
        /// <summary>
        /// 添加分词匹配数据关键字
        /// </summary>
        /// <param name="wordIdentitys">词语编号标识集合</param>
        /// <param name="key">匹配数据关键字</param>
        /// <returns></returns>
        public abstract Task<ResponseResult> AppendIndex(int[] wordIdentitys, T key);
        /// <summary>
        /// 更新分词匹配数据关键字
        /// </summary>
        /// <param name="wordIdentitys">词语编号标识集合</param>
        /// <param name="historyWordIdentitys">更新之前的词语编号标识集合</param>
        /// <param name="key">匹配数据关键字</param>
        /// <returns></returns>
        public abstract Task<ResponseResult> AppendIndex(int[] wordIdentitys, int[] historyWordIdentitys, T key);
        /// <summary>
        /// 删除分词匹配数据关键字
        /// </summary>
        /// <param name="wordIdentitys">词语编号标识集合</param>
        /// <param name="key">匹配数据关键字</param>
        /// <returns></returns>
        public abstract Task<ResponseResult> RemoveIndex(int[] wordIdentitys, T key);
    }
}
