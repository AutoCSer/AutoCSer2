using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 实时调用信息
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct CallInfo
    {
        /// <summary>
        /// 实时调用监视信息
        /// </summary>
        internal readonly InterfaceMonitor InterfaceMonitor;
        /// <summary>
        /// 实时调用信息序列化数据
        /// </summary>
        public readonly CallData CallData;
        /// <summary>
        /// 实时调用信息
        /// </summary>
        /// <param name="interfaceMonitor">实时调用监视信息</param>
        /// <param name="callData">实时调用信息序列化数据</param>
        internal CallInfo(InterfaceMonitor interfaceMonitor, CallData callData)
        {
            InterfaceMonitor = interfaceMonitor;
            CallData = callData;
        }
    }
}
