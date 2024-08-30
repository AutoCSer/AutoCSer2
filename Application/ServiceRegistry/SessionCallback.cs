using AutoCSer.CommandService.ServiceRegister;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.ServiceRegistry
{
    /// <summary>
    /// 获取服务注册日志回调委托
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct SessionCallback
    {
        /// <summary>
        /// 服务注册会话对象
        /// </summary>
        internal readonly ServiceRegisterSession Session;
        /// <summary>
        /// 获取服务注册日志回调委托
        /// </summary>
        internal readonly CommandServerKeepCallback<ServiceRegisterLog> callback;
        /// <summary>
        /// 获取服务注册日志回调委托
        /// </summary>
        /// <param name="session">服务注册会话对象</param>
        /// <param name="callback">获取服务注册日志回调委托</param>
        internal SessionCallback(ServiceRegisterSession session, CommandServerKeepCallback<ServiceRegisterLog> callback)
        {
            Session = session;
            this.callback = callback;
        }
        /// <summary>
        /// 日志回调
        /// </summary>
        /// <param name="log"></param>
        /// <returns>调线会话ID</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal long Callback(ServiceRegisterLog log)
        {
            return callback.Callback(log) ? 0 : Session.SetDroppedSessionID();
        }
        /// <summary>
        /// 取消保持回调
        /// </summary>
        /// <param name="state"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CancelKeep(ServiceRegisterStateEnum state)
        {
            callback.CancelKeep(CommandServerCall.GetCustom((byte)state));
        }
    }
}
