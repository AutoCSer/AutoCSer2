using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 获取服务注册日志回调委托
    /// </summary>
    internal struct ServiceRegistrySessionCallback
    {
        /// <summary>
        /// 服务注册会话对象
        /// </summary>
        internal readonly ServiceRegistrySession Session;
        /// <summary>
        /// 获取服务注册日志回调委托
        /// </summary>
        internal readonly CommandServerKeepCallback<ServiceRegisterLog> callback;
        /// <summary>
        /// 获取服务注册日志回调委托
        /// </summary>
        /// <param name="session">服务注册会话对象</param>
        /// <param name="callback">获取服务注册日志回调委托</param>
        internal ServiceRegistrySessionCallback(ServiceRegistrySession session, CommandServerKeepCallback<ServiceRegisterLog> callback)
        {
            Session = session;
            this.callback = callback;
        }
        /// <summary>
        /// 日志回调
        /// </summary>
        /// <param name="log"></param>
        /// <returns>调线会话ID</returns>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal long Callback(ServiceRegisterLog log)
        {
            return callback.Callback(log) ? 0 : Session.SetDroppedSessionID();
        }
        /// <summary>
        /// 取消保持回调
        /// </summary>
        /// <param name="state"></param>
        [MethodImpl(AutoCSer.MethodImpl.AggressiveInlining)]
        internal void CancelKeep(ServiceRegisterState state)
        {
            callback.CancelKeep(CommandServerCall.GetCustom((byte)state));
        }
    }
}
