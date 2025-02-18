using AutoCSer.Diagnostics;
using System;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 实时调用时间戳信息
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct CallTimestamp
    {
        /// <summary>
        /// 服务端时间戳
        /// </summary>
        public readonly ServerTimestamp ServerTimestamp;
        /// <summary>
        /// 实时调用信息序列化数据
        /// </summary>
        public readonly CallData CallData;
        /// <summary>
        /// 实时调用时间戳信息
        /// </summary>
        /// <param name="service">服务端时间戳</param>
        /// <param name="callData">实时调用信息序列化数据</param>
        internal CallTimestamp(InterfaceRealTimeCallMonitorService service, CallData callData)
        {
            ServerTimestamp = service.ServerTimestamp;
            CallData = callData;
        }
    }
}
