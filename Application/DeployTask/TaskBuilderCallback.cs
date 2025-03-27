using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService.DeployTask
{
    /// <summary>
    /// 发布任务状态变更回调日志回调
    /// </summary>
    internal sealed class TaskBuilderCallback : ReadWriteQueueNode
    {
        /// <summary>
        /// 发布任务创建器
        /// </summary>
        private readonly TaskBuilder builder;
        /// <summary>
        /// 发布任务状态变更回调日志
        /// </summary>
        private readonly DeployTaskLog log;
        /// <summary>
        /// 发布任务状态变更回调日志回调
        /// </summary>
        /// <param name="builder">发布任务创建器</param>
        /// <param name="state">发布任务状态变更操作状态</param>
        internal TaskBuilderCallback(TaskBuilder builder, OperationStateEnum state)
        {
            this.builder = builder;
            log = builder.Data.GetDeployTaskLog(state);
        }
        /// <summary>
        /// 发布任务状态变更回调日志回调
        /// </summary>
        public override void RunTask()
        {
            builder.Callback(log);
        }
    }
}
