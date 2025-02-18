using System;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 调用异常统计信息
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CallExceptionStatistics
    {
        /// <summary>
        /// 调用接口类型
        /// </summary>
        public readonly string CallType;
        /// <summary>
        /// 调用接口方法名称
        /// </summary>
        public readonly string CallName;
        /// <summary>
        /// 异常调用统计信息
        /// </summary>
        public readonly ExceptionStatistics Statistics;
        /// <summary>
        /// 调用异常统计信息
        /// </summary>
        /// <param name="callType">调用接口类型</param>
        /// <param name="callName">调用接口方法名称</param>
        /// <param name="statistics">异常调用统计信息</param>
        internal CallExceptionStatistics(string callType, string callName, ExceptionStatistics statistics)
        {
            CallType = callType;
            CallName = callName;
            Statistics = statistics;
        }
    }
}
