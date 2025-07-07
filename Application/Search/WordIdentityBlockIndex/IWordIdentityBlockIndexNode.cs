using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// Word segmentation result disk block index information node interface
    /// 分词结果磁盘块索引信息节点接口
    /// </summary>
    /// <typeparam name="T">Keyword type for word segmentation data
    /// 分词数据关键字类型</typeparam>
    [ServerNode(IsLocalClient = true, IsMethodParameterCreator = true)]
    public partial interface IWordIdentityBlockIndexNode<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(BinarySerializeKeyValue<T, BlockIndex> value);
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 2)]
        void SnapshotSetLoaded(bool value);
        /// <summary>
        /// The initialization data loading has been completed
        /// 初始化数据加载完成
        /// </summary>
        [ServerMethod(IsClientCall = false)]
        void Loaded();
        /// <summary>
        /// Create the disk block index information of the word segmentation result (Check the input parameters before the persistence operation)
        /// 创建分词结果磁盘块索引信息（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="key">The keyword of the word segmentation data
        /// 分词数据关键字</param>
        /// <param name="text">Word segmentation text data
        /// 分词文本数据</param>
        /// <returns></returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ValueResult<WordIdentityBlockIndexUpdateStateEnum> LoadCreateBeforePersistence(T key, string text);
        /// <summary>
        /// Create the disk block index information of the word segmentation result (Initialize and load the persistent data)
        /// 创建分词结果磁盘块索引信息（初始化加载持久化数据）
        /// </summary>
        /// <param name="key">The keyword of the word segmentation data
        /// 分词数据关键字</param>
        /// <param name="text">Word segmentation text data
        /// 分词文本数据</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void LoadCreateLoadPersistence(T key, string text, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// Create the disk block index information of the word segmentation result
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">The keyword of the word segmentation data
        /// 分词数据关键字</param>
        /// <param name="text">Word segmentation text data
        /// 分词文本数据</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void LoadCreate(T key, string text, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// Create the disk block index information of the word segmentation result (Initialize and load the persistent data)
        /// 创建分词结果磁盘块索引信息（初始化加载持久化数据）
        /// </summary>
        /// <param name="key">The keyword of the word segmentation data
        /// 分词数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void CreateLoadPersistence(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// Create the disk block index information of the word segmentation result
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">The keyword of the word segmentation data
        /// 分词数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Create(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// Update the disk block index information of the word segmentation result (Initialize and load the persistent data)
        /// 更新分词结果磁盘块索引信息（初始化加载持久化数据）
        /// </summary>
        /// <param name="key">The keyword of the word segmentation data
        /// 分词数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void UpdateLoadPersistence(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// Update the disk block index information of the word segmentation result
        /// 更新分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">The keyword of the word segmentation data
        /// 分词数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Update(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// Delete the disk block index information of the word segmentation result (Initialize and load the persistent data)
        /// 删除分词结果磁盘块索引信息（初始化加载持久化数据）
        /// </summary>
        /// <param name="key">The keyword of the word segmentation data
        /// 分词数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void DeleteLoadPersistence(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// Delete the disk block index information of the word segmentation result
        /// 删除分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">The keyword of the word segmentation data
        /// 分词数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Delete(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// The disk block index information of the word segmentation result has completed the update operation (Initialize and load the persistent data)
        /// 分词结果磁盘块索引信息完成更新操作（初始化加载持久化数据）
        /// </summary>
        /// <param name="key">The keyword of the word segmentation data
        /// 分词数据关键字</param>
        /// <param name="blockIndex">Disk block index information
        /// 磁盘块索引信息</param>
        /// <param name="callback"></param>
        void CompletedLoadPersistence(T key, BlockIndex blockIndex, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// The disk block index information of the word segmentation result has completed the update operation
        /// 分词结果磁盘块索引信息完成更新操作
        /// </summary>
        /// <param name="key">The keyword of the word segmentation data
        /// 分词数据关键字</param>
        /// <param name="blockIndex">Disk block index information
        /// 磁盘块索引信息</param>
        /// <param name="callback"></param>
        [ServerMethod(IsClientCall = false)]
        void Completed(T key, BlockIndex blockIndex, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// Delete the disk block index information of the word segmentation result (Initialize and load the persistent data)
        /// 删除分词结果磁盘块索引信息（初始化加载持久化数据）
        /// </summary>
        /// <param name="key">The keyword of the word segmentation data
        /// 分词数据关键字</param>
        /// <param name="callback"></param>
        void DeletedLoadPersistence(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// Delete the disk block index information of the word segmentation result
        /// 删除分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">The keyword of the word segmentation data
        /// 分词数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsClientCall = false)]
        void Deleted(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
    }
}
