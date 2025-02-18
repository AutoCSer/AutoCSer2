using System;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 接口实时调用监视服务会话对象接口
    /// </summary>
    public interface IInterfaceMonitorSession
    {
        /// <summary>
        /// 实时调用监视信息
        /// </summary>
#if NetStandard21
        InterfaceMonitor? InterfaceMonitor { get; set; }
#else
        InterfaceMonitor InterfaceMonitor { get; set; }
#endif
    }
}
