using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CustomNode
{
    /// <summary>
    /// Timeout task message node interface (for distributed transaction data consistency check)
    /// 超时任务消息节点接口（用于分布式事务数据一致性检查）
    /// </summary>
    /// <typeparam name="T">Task message data type
    /// 任务消息数据类型</typeparam>
    [ServerNode(IsLocalClient = true, IsMethodParameterCreator = true, IsReturnValueNode = ServerNodeAttribute.DefaultIsReturnValueNode)]
    public partial interface ITimeoutMessageNode<T>
    {
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotAdd(TimeoutMessageData<T> value);
        /// <summary>
        /// Get the total number of tasks
        /// 获取任务总数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetCount();
        /// <summary>
        /// Get the number of failed tasks executed
        /// 获取执行失败任务数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int GetFailedCount();
        /// <summary>
        /// Add the task node (Check the input parameters before the persistence operation)
        /// 添加任务节点（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        ValueResult<long> AppendBeforePersistence(TimeoutMessage<T> task);
        /// <summary>
        /// Add the task node
        /// 添加任务节点
        /// </summary>
        /// <param name="task"></param>
        /// <returns>Task identifier. Return 0 upon failure
        /// 任务标识，失败返回 0</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        long Append(TimeoutMessage<T> task);
        /// <summary>
        /// Add immediate execution tasks (Check the input parameters before the persistence operation)
        /// 添加立即执行任务（持久化操作之前检查输入参数）
        /// </summary>
        /// <param name="task"></param>
        /// <returns>Returning true indicates that a persistence operation is required
        /// 返回 true 表示需要持久化操作</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        bool AppendRunBeforePersistence(TimeoutMessage<T> task);
        /// <summary>
        /// Add immediate execution tasks (Initialize and load the persistent data)
        /// 添加立即执行任务（初始化加载持久化数据）
        /// </summary>
        /// <param name="task"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void AppendRunLoadPersistence(TimeoutMessage<T> task);
        /// <summary>
        /// Add immediate execution tasks
        /// 添加立即执行任务
        /// </summary>
        /// <param name="task"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void AppendRun(TimeoutMessage<T> task);
        /// <summary>
        /// Trigger task execution (Initialize and load the persistent data)
        /// 触发任务执行（初始化加载持久化数据）
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void RunTaskLoadPersistence(long identity);
        /// <summary>
        /// Trigger task execution
        /// 触发任务执行
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void RunTask(long identity);
        /// <summary>
        /// Complete the completed task
        /// 完成已任务
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="isSuccess"></param>
        [ServerMethod(IsClientCall = false, IsIgnorePersistenceCallbackException = true)]
        void Completed(long identity, bool isSuccess);
        /// <summary>
        /// Cancel the task
        /// 取消任务
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Cancel(long identity);
        /// <summary>
        /// Failed task retry
        /// 失败任务重试
        /// </summary>
        [ServerMethod(IsPersistence = false)]
        void RetryFailed();
        /// <summary>
        /// Get the execution task message data
        /// 获取执行任务消息数据
        /// </summary>
        /// <param name="callback"></param>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true, IsCallbackClient = true)]
        void GetRunTask(MethodKeepCallback<T> callback);
    }
}
