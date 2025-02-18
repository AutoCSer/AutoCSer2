using System;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 接口实时调用监视服务客户端接口
    /// </summary>
    public interface IInterfaceRealTimeCallMonitorClientSocketEvent
    {
        /// <summary>
        /// 接口实时调用监视服务接口
        /// </summary>
        IInterfaceRealTimeCallMonitorServiceClientController InterfaceRealTimeCallMonitor { get; }
    }
}
