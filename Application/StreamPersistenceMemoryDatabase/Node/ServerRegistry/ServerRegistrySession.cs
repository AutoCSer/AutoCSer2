﻿using System;
using System.Diagnostics;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务会话信息
    /// </summary>
    internal sealed class ServerRegistrySession
    {
        /// <summary>
        /// 服务注册回调委托
        /// </summary>
#if NetStandard21
        private MethodKeepCallback<ServerRegistryOperationTypeEnum>? callback;
#else
        private MethodKeepCallback<ServerRegistryOperationTypeEnum> callback;
#endif
        /// <summary>
        /// 服务是否在线
        /// </summary>
        private bool isOnline;
        /// <summary>
        /// 服务注册回调委托是否有效
        /// </summary>
        internal bool IsCallback
        {
            get
            {
                return callback != null && !callback.IsCancelKeepCallback;
            }
        }
        /// <summary>
        /// 服务会话信息
        /// </summary>
        /// <param name="callback"></param>
#if NetStandard21
        internal ServerRegistrySession(MethodKeepCallback<ServerRegistryOperationTypeEnum>? callback = null)
#else
        internal ServerRegistrySession(MethodKeepCallback<ServerRegistryOperationTypeEnum> callback = null)
#endif
        {
            this.callback = callback;
            isOnline = true;
        }
        /// <summary>
        /// 设置服务注册回调委托
        /// </summary>
        /// <param name="callback"></param>

        internal void Set(MethodKeepCallback<ServerRegistryOperationTypeEnum> callback)
        {
            var oldCallback = this.callback;
            this.callback = callback;
            isOnline = true;
            oldCallback?.CancelKeep();
        }
        /// <summary>
        /// 检查服务是否在线
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal bool Check(ServerRegistryNode node)
        {
            if (callback != null)
            {
                if (callback.Callback(ServerRegistryOperationTypeEnum.CheckOnline)) return true;
                callback = null;
                isOnline = false;
                return false;
            }
            return isOnline && Stopwatch.GetTimestamp() < node.LoadTimeoutTimestamp;
        }
        /// <summary>
        /// 通知单例服务下线
        /// </summary>
        /// <returns></returns>
        internal bool Offline()
        {
            if (callback == null) return true;
            if (!callback.Callback(ServerRegistryOperationTypeEnum.Offline))
            {
                callback = null;
                isOnline = false;
                return true;
            }
            return false;
        }
    }
}