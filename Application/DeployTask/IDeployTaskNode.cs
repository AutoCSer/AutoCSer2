using AutoCSer.CommandService.DeployTask;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// The interface of the deploy task node
    /// 发布任务节点接口
    /// </summary>
    [ServerNode(IsMethodParameterCreator = true)]
    public partial interface IDeployTaskNode
    {
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(TaskData value);
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 2)]
        void SnapshotSetIdentity(long value);
        /// <summary>
        /// Create a task
        /// 创建任务
        /// </summary>
        /// <returns>Task identity
        /// 任务标识ID</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        long Create();
        /// <summary>
        /// Start the task (Initialize and load the persistent data)
        /// 启动任务（初始化加载持久化数据）
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="startTime">The Utc time for running the task
        /// 运行任务 Utc 时间</param>
        /// <returns></returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        OperationStateEnum StartLoadPersistence(long identity, DateTime startTime);
        /// <summary>
        /// Start the task
        /// 启动任务
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="startTime">The Utc time for running the task
        /// 运行任务 Utc 时间</param>
        /// <returns></returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        OperationStateEnum Start(long identity, DateTime startTime);
        /// <summary>
        /// Add a sub-task
        /// 添加子任务
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="task">The task of executing a program
        /// 执行程序任务</param>
        /// <returns></returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        OperationStateEnum AppendStepTask(long identity, StepTaskData task);
        /// <summary>
        /// Get the callback log of the status change of the published task
        /// 获取发布任务状态变更回调日志
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="callback">Callback delegate for task state changes
        /// 任务状态变更回调委托</param>
        [ServerMethod(IsPersistence = false, IsWriteQueue = true)]
        void GetLog(long identity, MethodKeepCallback<DeployTaskLog> callback);
        /// <summary>
        /// Remove completed or un-started task (Initialize and load the persistent data)
        /// 移除已结束或者未开始任务（初始化加载持久化数据）
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="closeTime">The Utc time for closing the task
        /// 关闭任务 Utc 时间</param>
        /// <returns></returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        OperationStateEnum RemoveLoadPersistence(long identity, DateTime closeTime);
        /// <summary>
        /// Remove completed or un-started task
        /// 移除已结束或者未开始任务
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="closeTime">The Utc time for closing the task
        /// 关闭任务 Utc 时间</param>
        /// <returns></returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        OperationStateEnum Remove(long identity, DateTime closeTime);
        /// <summary>
        /// Close the task
        /// 关闭任务
        /// </summary>
        /// <param name="identity">Task identity
        /// 任务标识ID</param>
        /// <param name="closeTime">The Utc time for closing the task
        /// 关闭任务 Utc 时间</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true, IsClientCall = false)]
        void Close(long identity, DateTime closeTime);
    }
}
