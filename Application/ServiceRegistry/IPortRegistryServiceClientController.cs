using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 端口注册客户端
    /// </summary>
    public partial interface IPortRegistryServiceClientController
    {
        /// <summary>
        /// 设置端口标识在线检查回调委托
        /// </summary>
        /// <param name="portIdentity"></param>
        /// <param name="callback">端口标识在线检查回调委托</param>
        KeepCallbackCommand SetCallback(PortIdentity portIdentity, Action<CommandClientReturnValue, KeepCallbackCommand> callback);
    }
}
