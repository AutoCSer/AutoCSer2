using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 日志扩展操作
    /// </summary>
    public static class LogExtension
    {
        /// <summary>
        /// 添加普通日志
        /// </summary>
        /// <param name="log">日志处理接口</param>
        /// <param name="message">普通日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        /// <returns>是否写入日志</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<bool> Info(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Info, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static Task<bool> Info(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Info, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            return log.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加普通日志
        /// </summary>
        /// <param name="log">日志处理接口</param>
        /// <param name="message">普通日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
#if NetStandard21
        public static void InfoIgnoreException(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Info, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void InfoIgnoreException(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Info, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            try
            {
                log.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber).NotWait();
            }
            catch { }
        }
        /// <summary>
        /// 添加调试日志
        /// </summary>
        /// <param name="log">日志处理接口</param>
        /// <param name="message">调试日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
#if NetStandard21
        public static void DebugIgnoreException(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Debug, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void DebugIgnoreException(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Debug, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            try
            {
                log.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber).NotWait();
            }
            catch { }
        }
        /// <summary>
        /// 添加异常日志
        /// </summary>
        /// <param name="log">日志处理接口</param>
        /// <param name="exception">异常信息</param>
        /// <param name="message">附加信息</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
#if NetStandard21
        public static void ExceptionIgnoreException(this ILog log, Exception exception, string? message = null, LogLevelEnum level = LogLevelEnum.Exception, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void ExceptionIgnoreException(this ILog log, Exception exception, string message = null, LogLevelEnum level = LogLevelEnum.Exception, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            try
            {
                log.Exception(exception, message, level, callerMemberName, callerFilePath, callerLineNumber).NotWait();
            }
            catch { }
        }
        /// <summary>
        /// 添加一般错误日志
        /// </summary>
        /// <param name="log">日志处理接口</param>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        /// <returns>是否写入日志</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<bool> Error(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Error, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static Task<bool> Error(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Error, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            return log.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加一般错误日志
        /// </summary>
        /// <param name="log">日志处理接口</param>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
#if NetStandard21
        public static void ErrorIgnoreException(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Error, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void ErrorIgnoreException(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Error, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            try
            {
                log.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber).NotWait();
            }
            catch { }
        }
        /// <summary>
        /// 添加警告日志
        /// </summary>
        /// <param name="log">日志处理接口</param>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        /// <returns>是否写入日志</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<bool> Warn(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Warn, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static Task<bool> Warn(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Warn, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            return log.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加警告日志
        /// </summary>
        /// <param name="log">日志处理接口</param>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
#if NetStandard21
        public static void WarnIgnoreException(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Warn, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void WarnIgnoreException(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Warn, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            try
            {
                log.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber).NotWait();
            }
            catch { }
        }
        /// <summary>
        /// 添加致命错误日志
        /// </summary>
        /// <param name="log">日志处理接口</param>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        /// <returns>是否写入日志</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<bool> Fatal(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Fatal, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static Task<bool> Fatal(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Fatal, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            return log.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加致命错误日志
        /// </summary>
        /// <param name="log">日志处理接口</param>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
#if NetStandard21
        public static void FatalIgnoreException(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Fatal, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void FatalIgnoreException(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Fatal, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            try
            {
                log.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber).NotWait();
            }
            catch { }
        }
        /// <summary>
        /// 添加测试断点日志
        /// </summary>
        /// <param name="log">日志处理接口</param>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        /// <returns>是否写入日志</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<bool> Breakpoint(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Breakpoint, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static Task<bool> Breakpoint(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Breakpoint, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            return log.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加测试断点日志
        /// </summary>
        /// <param name="log">日志处理接口</param>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
#if NetStandard21
        public static void BreakpointIgnoreException(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Breakpoint, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void BreakpointIgnoreException(this ILog log, string message, LogLevelEnum level = LogLevelEnum.Breakpoint, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            try
            {
                log.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber).NotWait();
            }
            catch { }
        }
        /// <summary>
        /// 等待写入完成
        /// </summary>
        public static void FlushIgnoreException(this ILog log)
        {
            try
            {
                log.Flush().NotWait();
            }
            catch { }
        }
    }
}
