using AutoCSer.CommandService.DeployTask;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 发布任务状态变更回调日志
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct DeployTaskLog
    {
        /// <summary>
        /// 任务表示ID
        /// </summary>
        public readonly long Identity;
        /// <summary>
        /// 子任务索引编号
        /// </summary>
        public readonly int TaskIndex;
        /// <summary>
        /// 子任务类型
        /// </summary>
        public readonly StepTypeEnum TaskType;
        /// <summary>
        /// 操作状态
        /// </summary>
        public readonly OperationStateEnum OperationState;
        /// <summary>
        /// 日志回调是否已结束
        /// </summary>
        public readonly bool IsFinished;
        /// <summary>
        /// 发布任务状态变更回调日志
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="state"></param>
        /// <param name="isFinished"></param>
        internal DeployTaskLog(long identity, OperationStateEnum state, bool isFinished = false)
        {
            Identity = identity;
            TaskIndex = int.MinValue;
            TaskType = StepTypeEnum.Unknown;
            OperationState = state;
            IsFinished = isFinished;
        }
        /// <summary>
        /// 发布任务状态变更回调日志
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="taskIndex"></param>
        /// <param name="taskType"></param>
        /// <param name="state"></param>
        /// <param name="isFinished"></param>
        internal DeployTaskLog(long identity, int taskIndex, StepTypeEnum taskType, OperationStateEnum state, bool isFinished)
        {
            Identity = identity;
            TaskIndex = taskIndex;
            TaskType = taskType;
            OperationState = state;
            IsFinished = isFinished;
        }
    }
}
