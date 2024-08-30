using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 添加任务步骤结果
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct DeployTaskAppendResult
    {
        /// <summary>
        /// 添加任务步骤状态
        /// </summary>
        public DeployTaskAppendStateEnum AppendState;
        /// <summary>
        /// 附加提示信息
        /// </summary>
        public string Message;
    }
}
