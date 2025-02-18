using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.InterfaceRealTimeCallMonitor
{
    /// <summary>
    /// 实时调用监视信息
    /// </summary>
    public sealed class InterfaceMonitor
    {
        /// <summary>
        /// 实时调用监视标识
        /// </summary>
        internal readonly long Identity;
        /// <summary>
        /// 接口监视服务在线检查回调委托
        /// </summary>
#if NetStandard21
        private CommandServerKeepCallback? callback;
#else
        private CommandServerKeepCallback callback;
#endif
        /// <summary>
        /// 实时调用监视信息
        /// </summary>
        /// <param name="service"></param>
        internal InterfaceMonitor(InterfaceRealTimeCallMonitorService service)
        {
            Identity = service.CurrentMonitorIdentity++;
        }
        /// <summary>
        /// 设置接口监视服务在线检查回调委托
        /// </summary>
        /// <param name="callback">接口监视服务在线检查回调委托</param>
        /// <returns>需要取消的原委托</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal CommandServerKeepCallback? Set(CommandServerKeepCallback callback)
#else
        internal CommandServerKeepCallback Set(CommandServerKeepCallback callback)
#endif
        {
            var cancelCallback = this.callback;
            this.callback = callback;
            return cancelCallback;
        }
        /// <summary>
        /// 取消在线检查回调
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CancelCallback()
        {
            if (callback != null)
            {
                var callback = this.callback;
                this.callback = null;
                callback.CancelKeep();
            }
        }
        /// <summary>
        /// 在线检查回调
        /// </summary>
        /// <returns>接口监视服务是否在线</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool IsCallback()
        {
            if (callback != null)
            {
                if (callback.Callback()) return true;
                callback = null;
            }
            return false;
        }
    }
}
