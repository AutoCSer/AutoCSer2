using AutoCSer.CommandService.DiskBlock;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 分词结果磁盘块索引信息节点接口
    /// </summary>
    /// <typeparam name="T">分词数据关键字类型</typeparam>
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true, IsMethodParameterCreator = true)]
    public partial interface IWordIdentityBlockIndexNode<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(BinarySerializeKeyValue<T, BlockIndex> value);
        /// <summary>
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void CreateLoadPersistence(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// 创建分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Create(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// 更新分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void UpdateLoadPersistence(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// 更新分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Update(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// 删除分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void DeleteLoadPersistence(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// 删除分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Delete(T key, MethodCallback<WordIdentityBlockIndexUpdateStateEnum> callback);
        /// <summary>
        /// 分词结果磁盘块索引信息完成更新操作
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <param name="version">操作版本号</param>
        void CompletedLoadPersistence(T key, BlockIndex blockIndex, int version);
        /// <summary>
        /// 分词结果磁盘块索引信息完成更新操作
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        /// <param name="blockIndex">磁盘块索引信息</param>
        /// <param name="version">操作版本号</param>
        [ServerMethod(IsClientCall = false)]
        void Completed(T key, BlockIndex blockIndex, int version);
        /// <summary>
        /// 删除分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        void DeletedLoadPersistence(T key);
        /// <summary>
        /// 删除分词结果磁盘块索引信息
        /// </summary>
        /// <param name="key">分词数据关键字</param>
        [ServerMethod(IsClientCall = false)]
        void Deleted(T key);
    }
}
