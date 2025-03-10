using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode
{
    /// <summary>
    /// 超时任务消息节点接口（用于分布式事务数据一致性检查）
    /// </summary>
    /// <typeparam name="T">任务消息数据类型</typeparam>
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true, IsMethodParameterCreator = true)]
    public partial interface ITimeoutMessageNode<T>
    {
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotAdd(TimeoutMessageData<T> value);
        /// <summary>
        /// 获取任务总数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetCount();
        /// <summary>
        /// 获取执行失败任务数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetFailedCount();
        /// <summary>
        /// 添加任务 持久化前检查
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ValueResult<long> AppendBeforePersistence(TimeoutMessage<T> task);
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns>任务标识，失败返回 0</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        long Append(TimeoutMessage<T> task);
        /// <summary>
        /// 添加立即执行任务 持久化前检查
        /// </summary>
        /// <param name="task"></param>
        /// <returns>是否继续持久化操作</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool AppendRunBeforePersistence(TimeoutMessage<T> task);
        /// <summary>
        /// 添加立即执行任务
        /// </summary>
        /// <param name="task"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void AppendRunLoadPersistence(TimeoutMessage<T> task);
        /// <summary>
        /// 添加立即执行任务
        /// </summary>
        /// <param name="task"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void AppendRun(TimeoutMessage<T> task);
        /// <summary>
        /// 触发任务执行
        /// </summary>
        /// <param name="identity">任务标识</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void RunTaskLoadPersistence(long identity);
        /// <summary>
        /// 触发任务执行
        /// </summary>
        /// <param name="identity">任务标识</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void RunTask(long identity);
        /// <summary>
        /// 完成任务
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="isSuccess"></param>
        [ServerMethod(IsClientCall = false, IsIgnorePersistenceCallbackException = true)]
        void Completed(long identity, bool isSuccess);
        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="identity">任务标识</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Cancel(long identity);
        /// <summary>
        /// 失败任务重试
        /// </summary>
        [ServerMethod(IsPersistence = false)]
        void RetryFailed();
        /// <summary>
        /// 获取执行任务消息数据
        /// </summary>
        /// <param name="callback">获取执行任务消息数据回调</param>
        [ServerMethod(IsPersistence = false, IsCallbackClient = true)]
        void GetRunTask(MethodKeepCallback<T> callback);
    }
}
