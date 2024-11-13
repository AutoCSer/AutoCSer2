using System;

namespace AutoCSer.CommandService.ServiceRegistry
{
    /// <summary>
    /// 服务注册日志与会话信息
    /// </summary>
    internal class SessionLog
    {
        /// <summary>
        /// 服务注册会话对象
        /// </summary>
        internal readonly ServiceRegisterSocketSession Session;
        /// <summary>
        /// 服务注册日志
        /// </summary>
        internal readonly ServiceRegisterLog Log;
        /// <summary>
        /// 服务注册日志与会话信息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="log"></param>
        internal SessionLog(ServiceRegisterSocketSession session, ServiceRegisterLog log)
        {
            Session = session;
            Log = log;
            log.ServiceID = Session.Service.IdentityGenerator.GetNext();
        }

        /// <summary>
        /// 检查会话是否掉线
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        private static bool checkSessionDropped(SessionLog log) { return log.Session.CheckDropped(); }
        /// <summary>
        /// 检查会话是否掉线
        /// </summary>
        internal static readonly Func<SessionLog, bool> CheckSessionDropped = checkSessionDropped;
    }
}
