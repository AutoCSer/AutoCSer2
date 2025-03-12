using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 非索引条件查询数据节点接口
    /// </summary>
    /// <typeparam name="KT">关键字类型</typeparam>
    /// <typeparam name="VT">数据类型</typeparam>
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true, IsMethodParameterCreator = true)]
    public partial interface IConditionDataNode<KT, VT>
#if NetStandard21
        where KT : notnull, IEquatable<KT>
#else
        where KT : IEquatable<KT>
#endif
    {
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(VT value);
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 2)]
        void SnapshotSetLoaded(bool value);
        /// <summary>
        /// 初始化数据加载完成
        /// </summary>
        [ServerMethod(IsClientCall = false)]
        void Loaded();
        /// <summary>
        /// 创建非索引条件查询数据 持久化前检查
        /// </summary>
        /// <param name="value">非索引条件查询数据</param>
        /// <returns>是否继续持久化操作</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool LoadCreateBeforePersistence(VT value);
        /// <summary>
        /// 创建非索引条件查询数据
        /// </summary>
        /// <param name="value">非索引条件查询数据</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void LoadCreateLoadPersistence(VT value);
        /// <summary>
        /// 创建非索引条件查询数据
        /// </summary>
        /// <param name="value">非索引条件查询数据</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void LoadCreate(VT value);
        /// <summary>
        /// 创建非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsPersistence = false)]
        void Create(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback);
        /// <summary>
        /// 更新非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsPersistence = false)]
        void Update(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback);
        /// <summary>
        /// 删除非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void DeleteLoadPersistence(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback);
        /// <summary>
        /// 删除非索引条件查询数据
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="callback"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Delete(KT key, MethodCallback<ConditionDataUpdateStateEnum> callback);
        /// <summary>
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="value">非索引条件查询数据</param>
        /// <returns></returns>
        ValueResult<ConditionDataUpdateStateEnum> CompletedBeforePersistence(KT key, VT value);
        /// <summary>
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="value">非索引条件查询数据</param>
        /// <param name="callback"></param>
        void CompletedLoadPersistence(KT key, VT value, MethodCallback<ConditionDataUpdateStateEnum> callback);
        /// <summary>
        /// 非索引条件查询数据完成更新操作
        /// </summary>
        /// <param name="key">数据关键字</param>
        /// <param name="value">非索引条件查询数据</param>
        /// <param name="callback"></param>
        [ServerMethod(IsClientCall = false)]
        void Completed(KT key, VT value, MethodCallback<ConditionDataUpdateStateEnum> callback);
    }
}
