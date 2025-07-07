using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// Non-index condition query data node interface
    /// 非索引条件查询数据节点接口
    /// </summary>
    /// <typeparam name="KT">Keyword type
    /// 关键字类型</typeparam>
    /// <typeparam name="VT">Data type</typeparam>
    [ServerNode(IsLocalClient = true, IsMethodParameterCreator = true)]
    public partial interface IConditionDataNode<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(VT value);
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
        /// Create non-indexed conditional query data (Check the input parameters before the persistence operation)
        /// 创建非索引条件查询数据（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        /// <returns>Returning true indicates that a persistence operation is required
        /// 返回 true 表示需要持久化操作</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool LoadCreateBeforePersistence(VT value);
        /// <summary>
        /// Create non-indexed conditional query data (Initialize and load the persistent data)
        /// 创建非索引条件查询数据（初始化加载持久化数据）
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void LoadCreateLoadPersistence(VT value);
        /// <summary>
        /// Create non-indexed conditional query data
        /// 创建非索引条件查询数据
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void LoadCreate(VT value);
        /// <summary>
        /// Create non-indexed conditional query data
        /// 创建非索引条件查询数据
        /// </summary>
        /// <param name="key">Data keyword</param>
        /// <param name="callback"></param>
        [ServerMethod(IsPersistence = false)]
        void Create(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback);
        /// <summary>
        /// Update non-indexed condition query data
        /// 更新非索引条件查询数据
        /// </summary>
        /// <param name="key">Data keyword</param>
        /// <param name="callback"></param>
        [ServerMethod(IsPersistence = false)]
        void Update(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback);
        /// <summary>
        /// Delete non-indexed condition query data (Initialize and load the persistent data)
        /// 删除非索引条件查询数据（初始化加载持久化数据）
        /// </summary>
        /// <param name="key">Data keyword</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void DeleteLoadPersistence(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback);
        /// <summary>
        /// Delete non-indexed condition query data
        /// 删除非索引条件查询数据
        /// </summary>
        /// <param name="key">Data keyword</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Delete(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback);
        /// <summary>
        /// The non-indexed condition query data has completed the update operation (Check the input parameters before the persistence operation)
        /// 非索引条件查询数据完成更新操作（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        /// <returns></returns>
        ValueResult<ConditionDataUpdateStateEnum> CompletedBeforePersistence(VT value);
        /// <summary>
        /// The non-indexed condition query data has completed the update operation (Initialize and load the persistent data)
        /// 非索引条件查询数据完成更新操作（初始化加载持久化数据）
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        /// <param name="callback"></param>
        void CompletedLoadPersistence(VT value, MethodCallback<ConditionDataUpdateStateEnum> callback);
        /// <summary>
        /// The non-indexed condition query data has completed the update operation
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="value">Non-indexed condition query data
        /// 非索引条件查询数据</param>
        /// <param name="callback"></param>
        [ServerMethod(IsClientCall = false)]
        void Completed(VT value, MethodCallback<ConditionDataUpdateStateEnum> callback);
    }
}
