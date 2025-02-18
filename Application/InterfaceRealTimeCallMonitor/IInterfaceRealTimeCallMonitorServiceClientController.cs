using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 接口实时调用监视服务接口 客户端接口
    /// </summary>
    public partial interface IInterfaceRealTimeCallMonitorServiceClientController
    {
        /// <summary>
        /// 接口监视服务在线检查
        /// </summary>
        /// <param name="callback">在线检查回调</param>
        /// <returns></returns>
        KeepCallbackCommand Check(Action<CommandClientReturnValue, KeepCallbackCommand> callback);
        /// <summary>
        /// 获取异常调用数据
        /// </summary>
        /// <param name="callback">实时调用时间戳信息回调</param>
        /// <returns></returns>
        KeepCallbackCommand GetException(Action<CommandClientReturnValue<InterfaceRealTimeCallMonitor.CallTimestamp>, KeepCallbackCommand> callback);
        /// <summary>
        /// 获取超时调用数据
        /// </summary>
        /// <param name="callback">实时调用时间戳信息回调</param>
        /// <returns></returns>
        KeepCallbackCommand GetTimeout(Action<CommandClientReturnValue<InterfaceRealTimeCallMonitor.CallTimestamp>, KeepCallbackCommand> callback);
    }
}
