using AutoCSer.Extensions;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 端口数据
    /// </summary>
    internal struct PortRegistryData
    {
        /// <summary>
        /// 端口标识
        /// </summary>
        private uint identity;
        /// <summary>
        /// 在线检查回调委托
        /// </summary>
#if NetStandard21
        private CommandServerKeepCallback? callback;
#else
        private CommandServerKeepCallback callback;
#endif
        /// <summary>
        /// 允许获取端口标识时间
        /// </summary>
        private DateTime getTime;
        /// <summary>
        /// 获取端口标识
        /// </summary>
        /// <returns></returns>
        internal uint GetIdentity()
        {
            if (getTime > AutoCSer.Threading.SecondTimer.Now) return 0;
            if (callback != null)
            {
                if (!callback.Callback())
                {
                    getTime = AutoCSer.Threading.SecondTimer.Now.AddMinutes(1);
                    callback = null;
                }
                return 0;
            }
            getTime = AutoCSer.Threading.SecondTimer.Now.AddMinutes(1);
            if (++identity == 0) identity = 1;
            return identity;
        }
        /// <summary>
        /// 断线重连检查端口标识
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Check(uint identity)
        {
            if (this.identity == identity)
            {
                getTime = AutoCSer.Threading.SecondTimer.Now.AddMinutes(1);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 端口注册服务重连设置端口标识
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        internal bool Set(uint identity)
        {
            if (callback != null)
            {
                if (callback.Callback()) return false;
                callback = null;
            }
            this.identity = identity;
            getTime = AutoCSer.Threading.SecondTimer.Now.AddMinutes(1);
            return true;
        }
        /// <summary>
        /// 设置端口标识在线检查回调委托
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool Set(uint identity, ref CommandServerKeepCallback? callback)
#else
        internal bool Set(uint identity, ref CommandServerKeepCallback callback)
#endif
        {
            if (this.identity == identity)
            {
                var oldCallback = this.callback;
                this.callback = callback;
                callback = oldCallback;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 释放端口标识
        /// </summary>
        /// <param name="identity"></param>
        internal void Free(uint identity)
        {
            if (this.identity == identity)
            {
                var callback = this.callback;
                this.callback = null;
                getTime = AutoCSer.Threading.SecondTimer.Now.AddMinutes(1);
                callback?.CancelKeep();
            }
        }
    }
}
