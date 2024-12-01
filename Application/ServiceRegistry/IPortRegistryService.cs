using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 端口注册服务
    /// </summary>
    [AutoCSer.Net.CommandServerControllerInterface]
    public interface IPortRegistryService
    {
        /// <summary>
        /// 获取一个空闲端口标识
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        PortIdentity GetPort(CommandServerSocket socket, CommandServerCallQueue queue);
        /// <summary>
        /// 断线重连设置端口标识
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="portIdentity"></param>
        /// <returns></returns>
        PortIdentity SetPort(CommandServerSocket socket, CommandServerCallQueue queue, PortIdentity portIdentity);
        /// <summary>
        /// 设置端口标识在线检查回调委托
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="portIdentity"></param>
        /// <param name="callback">端口标识在线检查回调委托</param>
        [CommandServerMethod(AutoCancelKeep = false)]
        void SetCallback(CommandServerSocket socket, CommandServerCallQueue queue, PortIdentity portIdentity, CommandServerKeepCallback callback);
        /// <summary>
        /// 释放端口标识
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="portIdentity"></param>
        /// <returns></returns>
        CommandServerSendOnly FreePort(CommandServerSocket socket, CommandServerCallQueue queue, PortIdentity portIdentity);
    }
}
