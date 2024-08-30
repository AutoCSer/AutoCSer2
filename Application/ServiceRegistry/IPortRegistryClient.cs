using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 端口注册客户端
    /// </summary>
    public interface IPortRegistryClient
    {
        /// <summary>
        /// 获取一个空闲端口标识
        /// </summary>
        /// <returns></returns>
        ReturnCommand<PortIdentity> GetPort();
        /// <summary>
        /// 断线重连设置端口标识
        /// </summary>
        /// <param name="portIdentity"></param>
        /// <returns></returns>
        ReturnCommand<PortIdentity> SetPort(PortIdentity portIdentity);
        /// <summary>
        /// 设置端口标识在线检查回调委托
        /// </summary>
        /// <param name="portIdentity"></param>
        /// <param name="callback">端口标识在线检查回调委托</param>
        KeepCallbackCommand SetCallback(PortIdentity portIdentity, Action<CommandClientReturnValue, KeepCallbackCommand> callback);
        /// <summary>
        /// 释放端口标识
        /// </summary>
        /// <param name="portIdentity"></param>
        /// <returns></returns>
        SendOnlyCommand FreePort(PortIdentity portIdentity);
    }
}
