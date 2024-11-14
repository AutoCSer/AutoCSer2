using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 端口注册客户端数据
    /// </summary>
    internal struct PortRegistryClientData
    {
        /// <summary>
        /// 注册的命令服务
        /// </summary>
        internal CommandListenerBase CommandListener;
        /// <summary>
        /// 端口标识
        /// </summary>
        private PortIdentity portIdentity;
        /// <summary>
        /// 端口标识在线检查回调命令保持回调对象
        /// </summary>
        private CommandKeepCallback commandKeepCallback;
        /// <summary>
        /// 端口注册客户端数据
        /// </summary>
        /// <param name="commandListener"></param>
        /// <param name="portIdentity"></param>
        /// <param name="commandKeepCallback"></param>
        internal PortRegistryClientData(CommandListenerBase commandListener, PortIdentity portIdentity, CommandKeepCallback commandKeepCallback)
        {
            this.CommandListener = commandListener;
            this.portIdentity = portIdentity;
            this.commandKeepCallback = commandKeepCallback;
        }

        /// <summary>
        /// 获取端口标识
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal PortIdentity GetPortIdentity()
        {
            return CommandListener.IsDisposed ? default(PortIdentity) : portIdentity;
        }
        /// <summary>
        /// 释放命令保持回调对象
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void DisposeKeepCallback()
        {
            commandKeepCallback.Dispose();
        }
        /// <summary>
        /// 释放端口标识
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal PortIdentity Free()
        {
            commandKeepCallback.Dispose();
            return portIdentity;
        }
    }
}
