using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 发布任务状态变更回调日志
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct DeployTaskLog
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public long TaskIdentity;
        /// <summary>
        /// 任务索引编号
        /// </summary>
        public int TaskIndex;
        /// <summary>
        /// 操作类型
        /// </summary>
        public DeployTaskOperationTypeEnum OperationType;
        /// <summary>
        /// 操作状态
        /// </summary>
        public DeployTaskOperationStateEnum OperationState;
        /// <summary>
        /// 附加提示信息
        /// </summary>
        public string Message;

        /// <summary>
        /// 检查任务执行状态
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        internal bool CheckRun(DeployTaskLog log)
        {
            if (log.OperationState == DeployTaskOperationStateEnum.Completed) return true;
            OperationState = log.OperationState;
            Message = log.Message;
            return false;
        }

        /// <summary>
        /// 错误状态回调
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="errorState"></param>
        internal static void CallbackError(CommandServerKeepCallback<DeployTaskLog> callback, DeployTaskOperationStateEnum errorState)
        {
            callback.CallbackCancelKeep(new DeployTaskLog { OperationState = errorState });
        }
    }
}
