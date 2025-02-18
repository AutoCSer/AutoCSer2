using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 分词结果磁盘块索引信息节点
    /// </summary>
    /// <typeparam name="T">分词数据关键字类型</typeparam>
    public abstract class WordIdentityBlockIndexNode<T> : MethodParameterCreatorNode<IWordIdentityBlockIndexNode<T>, BinarySerializeKeyValue<T, BlockIndex>>, IWordIdentityBlockIndexNode<T>, ISnapshot<BinarySerializeKeyValue<T, BlockIndex>>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        ///// <summary>
        ///// 字符串 Trie 图客户端节点
        ///// </summary>
        //internal readonly StreamPersistenceMemoryDatabaseClientNodeCache<IStaticTrieGraphNodeClientNode> TrieGraphNode;
        ///// <summary>
        ///// 词语匹配数据关键字索引节点
        ///// </summary>
        //internal readonly StreamPersistenceMemoryDatabaseClientNodeCache<IIndexNodeClientNode<int, T>> IndexNode;
        /// <summary>
        /// 分词数据磁盘块索引信息集合
        /// </summary>
        private readonly FragmentDictionary256<T, WordIdentityBlockIndex<T>> datas;
        /// <summary>
        /// 初始化加载待删除数据关键字集合
        /// </summary>
#if NetStandard21
        private HashSet<T>? deleteLoadPersistenceDatas;
#else
        private HashSet<T> deleteLoadPersistenceDatas;
#endif
        /// <summary>
        /// 操作版本号
        /// </summary>
        private int version;
        /// <summary>
        /// 获取操作版本号
        /// </summary>
        internal int NextVersion
        {
            get
            {
                do
                {
                    int version = System.Threading.Interlocked.Increment(ref this.version);
                    if (version != 0) return version;
                }
                while (true);
            }
        }
        /// <summary>
        /// 分词结果磁盘块索引信息节点
        /// </summary>
        protected WordIdentityBlockIndexNode() 
        {
            datas = new FragmentDictionary256<T, WordIdentityBlockIndex<T>>();
            deleteLoadPersistenceDatas = new HashSet<T>();
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
            foreach (KeyValuePair<T, WordIdentityBlockIndex<T>> data in datas.KeyValues)
            {
                if (data.Value.BlockIndex.IsBinarySerializeNullValue) data.Value.Create(this, data.Key).NotWait();
            }
            if (deleteLoadPersistenceDatas != null)
            {
                foreach (T key in deleteLoadPersistenceDatas)
                {
                    var data = default(WordIdentityBlockIndex<T>);
                    if (datas.TryGetValue(key, out data)) data.Delete(this, key).NotWait();
                }
                deleteLoadPersistenceDatas = null;
            }
            return this;
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int ISnapshot<BinarySerializeKeyValue<T, BlockIndex>>.GetSnapshotCapacity(ref object customObject)
        {
            return datas.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        SnapshotResult<BinarySerializeKeyValue<T, BlockIndex>> ISnapshot<BinarySerializeKeyValue<T, BlockIndex>>.GetSnapshotResult(BinarySerializeKeyValue<T, BlockIndex>[] snapshotArray, object customObject)
        {
            SnapshotResult<BinarySerializeKeyValue<T, BlockIndex>> result = new SnapshotResult<BinarySerializeKeyValue<T, BlockIndex>>(datas.Count, snapshotArray.Length);
            foreach (KeyValuePair<T, WordIdentityBlockIndex<T>> data in datas.KeyValues) result.Add(snapshotArray, new BinarySerializeKeyValue<T, BlockIndex>(data.Key, data.Value.BlockIndex));
            return result;
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSet(BinarySerializeKeyValue<T, BlockIndex> value)
        {
            datas.Add(value.Key, createDataBlockIndex(value.Value));
        }
        /// <summary>
        /// 从快照数据创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <returns></returns>
        protected abstract WordIdentityBlockIndex<T> createDataBlockIndex(BlockIndex blockIndex);
        /// <summary>
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        public void CreateLoadPersistence(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            if (key != null && !datas.ContainsKey(key)) datas.Add(key, createDataBlockIndex());
        }
        /// <summary>
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        public void Create(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            WordIdentityBlockIndexUpdateStateEnum state = WordIdentityBlockIndexUpdateStateEnum.Unknown;
            bool isCallback = false;
            try
            {
                if (key != null)
                {
                    if (!datas.ContainsKey(key))
                    {
                        WordIdentityBlockIndex<T> data = createDataBlockIndex();
                        datas.Add(key, data);
                        isCallback = true;
                        data.Create(this, key, callback).NotWait();
                    }
                    else state = WordIdentityBlockIndexUpdateStateEnum.Success;
                }
                else state = WordIdentityBlockIndexUpdateStateEnum.NullKey;
            }
            finally
            {
                if (!isCallback) callback.Callback(state);
            }
        }
        /// <summary>
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <returns></returns>
        protected abstract WordIdentityBlockIndex<T> createDataBlockIndex();
        /// <summary>
        /// 更新分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        public void UpdateLoadPersistence(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            if (key != null && !datas.ContainsKey(key)) datas.Add(key, createDataBlockIndex());
        }
        /// <summary>
        /// 更新分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        public void Update(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback)
        {
            WordIdentityBlockIndexUpdateStateEnum state = WordIdentityBlockIndexUpdateStateEnum.Unknown;
            bool isCallback = false;
            try
            {
                if (key != null)
                {
                    var data = default(WordIdentityBlockIndex<T>);
                    if (datas.TryGetValue(key, out data))
                    {
                        isCallback = true;
                        data.Update(this, key, callback).NotWait();
                    }
                    else
                    {
                        datas.Add(key, data = createDataBlockIndex());
                        isCallback = true;
                        data.Create(this, key, callback).NotWait();
                    }
                }
                else state = WordIdentityBlockIndexUpdateStateEnum.NullKey;
            }
            finally
            {
                if (!isCallback) callback.Callback(state);
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
                if (datas.TryGetValue(key, out data))
                {
                    if (deleteLoadPersistenceDatas == null) deleteLoadPersistenceDatas = new HashSet<T>();
                    deleteLoadPersistenceDatas.Add(key);
                }
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
            bool isCallback = false;
            try
            {
                if (key != null)
                {
                    var data = default(WordIdentityBlockIndex<T>);
                    if (datas.TryGetValue(key, out data))
                    {
                        isCallback = true;
                        data.Delete(this, key, callback).NotWait();
                    }
                    else state = WordIdentityBlockIndexUpdateStateEnum.Success;
                }
                else state = WordIdentityBlockIndexUpdateStateEnum.NullKey;
            }
            finally
            {
                if (!isCallback) callback.Callback(state);
            }
        }
        /// <summary>
        /// 分词结果磁盘块索引信息完成更新操作
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <param name="version">操作版本号</param>
        public void CompletedLoadPersistence(T key, BlockIndex blockIndex, int version)
        {
            var data = default(WordIdentityBlockIndex<T>);
            if (datas.TryGetValue(key, out data)) data.BlockIndex = blockIndex;
        }
        /// <summary>
        /// 分词结果磁盘块索引信息完成更新操作
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <param name="version">操作版本号</param>
        public void Completed(T key, BlockIndex blockIndex, int version)
        {
            var data = default(WordIdentityBlockIndex<T>);
            if (datas.TryGetValue(key, out data)) data.Completed(blockIndex, version).NotWait();
        }
        /// <summary>
        /// 删除分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        public void DeletedLoadPersistence(T key)
        {
            datas.Remove(key);
        }
        /// <summary>
        /// 删除分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        public void Deleted(T key)
        {
            var data = default(WordIdentityBlockIndex<T>);
            if (datas.Remove(key, out data)) data.Deleted().NotWait();
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
        public abstract Task<string?> GetText(T key);
#else
        public abstract Task<string> GetText(T key);
#endif
        /// <summary>
        /// 获取分词词语编号标识集合
        /// </summary>
        /// <param name="text">分词文本</param>
        /// <returns>词语编号标识集合</returns>
        public abstract Task<ResponseResult<int[]>> GetWordIdentitys(string text);
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
    }
}
