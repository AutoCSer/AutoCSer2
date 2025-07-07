using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务注册会话与日志信息
    /// </summary>
    internal sealed class ServerRegistrySessionLog
    {
        /// <summary>
        /// Server Registration Log
        /// 服务注册日志
        /// </summary>
        internal readonly ServerRegistryLog Log;
        /// <summary>
        /// 服务会话信息
        /// </summary>
        internal readonly ServerRegistrySession Session;
        /// <summary>
        /// 服务注册会话与日志信息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="log"></param>
        internal ServerRegistrySessionLog(ServerRegistrySession session, ServerRegistryLog log)
        {
            this.Log = log;
            this.Session = session;
        }
        /// <summary>
        /// Determine whether it is the server restart log
        /// 判断是否服务重启日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool IsNewEquals(ServerRegistryLog log)
        {
            return !Session.IsCallback && Log.IsNewEquals(log);
        }
    }
}
