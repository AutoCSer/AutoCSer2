using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer
{
    /// <summary>
    /// 日志处理接口
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 判断是否支持任意级别
        /// </summary>
        /// <param name="logLevel">日志级别</param>
        /// <returns></returns>
        bool IsAnyLevel(AutoCSer.LogLevelEnum logLevel);
        /// <summary>
        /// 添加调试日志
        /// </summary>
        /// <param name="message">调试日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        /// <returns>是否写入日志</returns>
#if NetStandard21
        Task<bool> Debug(string message, LogLevelEnum level = LogLevelEnum.Debug, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0);
#else
        Task<bool> Debug(string message, LogLevelEnum level = LogLevelEnum.Debug, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0);
#endif
        /// <summary>
        /// 添加异常日志
        /// </summary>
        /// <param name="exception">异常信息</param>
        /// <param name="message">附加信息</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        /// <returns>是否写入日志</returns>
#if NetStandard21
        Task<bool> Exception(Exception exception, string? message = null, LogLevelEnum level = LogLevelEnum.Exception, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0);
#else
        Task<bool> Exception(Exception exception, string message = null, LogLevelEnum level = LogLevelEnum.Exception, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0);
#endif
        /// <summary>
        /// 等待写入完成
        /// </summary>
        /// <returns>写盘是否成功</returns>
        Task<bool> Flush();
    }
}
