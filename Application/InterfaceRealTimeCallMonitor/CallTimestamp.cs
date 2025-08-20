using AutoCSer.Diagnostics;
using System;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 实时调用时间戳信息
    /// </summary>
    [AutoCSer.CodeGenerator.BinarySerialize(IsSerialize = false)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public partial struct CallTimestamp
    {
        /// <summary>
        /// 服务端时间戳
        /// </summary>
        public ServerTimestamp ServerTimestamp;
        /// <summary>
        /// 实时调用信息序列化数据
        /// </summary>
        public CallData CallData;
#if !AOT
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
#endif
    }
}
