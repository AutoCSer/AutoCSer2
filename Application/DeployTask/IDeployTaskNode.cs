using AutoCSer.CommandService.DeployTask;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 发布任务节点接口
    /// </summary>
    [ServerNode(IsAutoMethodIndex = false, IsMethodParameterCreator = true)]
    public partial interface IDeployTaskNode
    {
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(TaskData value);
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <returns>任务标识ID</returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        long Create();
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="identity">任务标识ID</param>
        /// <param name="startTime">运行任务时间 Utc</param>
        /// <returns></returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        OperationStateEnum StartLoadPersistence(long identity, DateTime startTime);
        /// <summary>
        /// 启动任务
        /// </summary>
        /// <param name="identity">任务标识ID</param>
        /// <param name="startTime">运行任务时间</param>
        /// <returns></returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        OperationStateEnum Start(long identity, DateTime startTime);
        /// <summary>
        /// 添加子任务
        /// </summary>
        /// <param name="identity">任务标识ID</param>
        /// <param name="task">执行程序任务</param>
        /// <returns></returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        OperationStateEnum AppendStepTask(long identity, StepTaskData task);
        /// <summary>
        /// 获取发布任务状态变更回调日志
        /// </summary>
        /// <param name="identity">任务标识ID</param>
        /// <param name="callback">任务状态变更回调委托</param>
        [ServerMethod(IsPersistence = false)]
        void GetLog(long identity, MethodKeepCallback<DeployTaskLog> callback);
        /// <summary>
        /// 移除已结束或者未开始任务
        /// </summary>
        /// <param name="identity">任务标识ID</param>
        /// <param name="closeTime">关闭任务时间</param>
        /// <returns></returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        OperationStateEnum RemoveLoadPersistence(long identity, DateTime closeTime);
        /// <summary>
        /// 移除已结束或者未开始任务
        /// </summary>
        /// <param name="identity">任务标识ID</param>
        /// <param name="closeTime">关闭任务时间</param>
        /// <returns></returns>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        OperationStateEnum Remove(long identity, DateTime closeTime);
        /// <summary>
        /// 关闭任务
        /// </summary>
        /// <param name="identity">任务标识ID</param>
        /// <param name="closeTime">关闭任务时间</param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true, IsClientCall = false)]
        void Close(long identity, DateTime closeTime);
    }
}
