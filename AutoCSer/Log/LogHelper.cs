using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer
{
    /// <summary>
    /// 公共日志配置
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// 公共配置日志，默认为文件日志
        /// </summary>
        public static readonly ILog Default = Configuration.Common.Get<ILog>(string.Empty)?.Value ?? new AutoCSer.Log.File(LogLevelEnum.All, AutoCSer.Log.File.GetDefaultFileName());
        /// <summary>
        /// 添加普通日志
        /// </summary>
        /// <param name="message">普通日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        /// <returns>是否写入日志</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<bool> Info(string message, LogLevelEnum level = LogLevelEnum.Info, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static Task<bool> Info(string message, LogLevelEnum level = LogLevelEnum.Info, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            return Default.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加普通日志
        /// </summary>
        /// <param name="message">普通日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void InfoIgnoreException(string message, LogLevelEnum level = LogLevelEnum.Info, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void InfoIgnoreException(string message, LogLevelEnum level = LogLevelEnum.Info, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            Default.DebugIgnoreException(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加调试日志
        /// </summary>
        /// <param name="message">调试日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        /// <returns>是否写入日志</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<bool> Debug(string message, LogLevelEnum level = LogLevelEnum.Debug, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static Task<bool> Debug(string message, LogLevelEnum level = LogLevelEnum.Debug, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            return Default.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加调试日志
        /// </summary>
        /// <param name="message">调试日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void DebugIgnoreException(string message, LogLevelEnum level = LogLevelEnum.Debug, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void DebugIgnoreException(string message, LogLevelEnum level = LogLevelEnum.Debug, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            Default.DebugIgnoreException(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
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
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<bool> Exception(Exception exception, string? message = null, LogLevelEnum level = LogLevelEnum.Exception, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static Task<bool> Exception(Exception exception, string message = null, LogLevelEnum level = LogLevelEnum.Exception, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            return Default.Exception(exception, message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加异常日志
        /// </summary>
        /// <param name="exception">异常信息</param>
        /// <param name="message">附加信息</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void ExceptionIgnoreException(Exception exception, string? message = null, LogLevelEnum level = LogLevelEnum.Exception, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void ExceptionIgnoreException(Exception exception, string message = null, LogLevelEnum level = LogLevelEnum.Exception, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            Default.ExceptionIgnoreException(exception, message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加一般错误日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        /// <returns>是否写入日志</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<bool> Error(string message, LogLevelEnum level = LogLevelEnum.Error, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static Task<bool> Error(string message, LogLevelEnum level = LogLevelEnum.Error, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            return Default.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加一般错误日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void ErrorIgnoreException(string message, LogLevelEnum level = LogLevelEnum.Error, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void ErrorIgnoreException(string message, LogLevelEnum level = LogLevelEnum.Error, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            Default.DebugIgnoreException(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加警告日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        /// <returns>是否写入日志</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<bool> Warn(string message, LogLevelEnum level = LogLevelEnum.Warn, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static Task<bool> Warn(string message, LogLevelEnum level = LogLevelEnum.Warn, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            return Default.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加警告日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void WarnIgnoreException(string message, LogLevelEnum level = LogLevelEnum.Warn, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void WarnIgnoreException(string message, LogLevelEnum level = LogLevelEnum.Warn, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            Default.DebugIgnoreException(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加致命错误日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        /// <returns>是否写入日志</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<bool> Fatal(string message, LogLevelEnum level = LogLevelEnum.Fatal, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static Task<bool> Fatal(string message, LogLevelEnum level = LogLevelEnum.Fatal, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            return Default.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加致命错误日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void FatalIgnoreException(string message, LogLevelEnum level = LogLevelEnum.Fatal, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void FatalIgnoreException(string message, LogLevelEnum level = LogLevelEnum.Fatal, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            Default.DebugIgnoreException(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加测试断点日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        /// <returns>是否写入日志</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Task<bool> Breakpoint(string message, LogLevelEnum level = LogLevelEnum.Breakpoint, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static Task<bool> Breakpoint(string message, LogLevelEnum level = LogLevelEnum.Breakpoint, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            return Default.Debug(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 添加测试断点日志
        /// </summary>
        /// <param name="message">错误日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void BreakpointIgnoreException(string message, LogLevelEnum level = LogLevelEnum.Breakpoint, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void BreakpointIgnoreException(string message, LogLevelEnum level = LogLevelEnum.Breakpoint, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            Default.DebugIgnoreException(message, level, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// 等待写入完成
        /// </summary>
        /// <returns>写盘是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<bool> Flush()
        {
            return Default.Flush();
        }
        /// <summary>
        /// 等待写入完成
        /// </summary>
        /// <returns>写盘是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void FlushIgnoreException()
        {
            Default.FlushIgnoreException();
        }

        //        /// <summary>
        //        /// 异常信息输出到控制台
        //        /// </summary>
        //        /// <param name="exception"></param>
        //        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //        internal static void ConsoleException(Exception exception)
        //        {
        //#if DEBUG
        //            if (exception != null) Console.WriteLine(exception.ToString());
        //#endif
        //        }

#if !AOT
        static LogHelper()
        {
            AutoCSer.Memory.ObjectRoot.ScanType.Add(typeof(LogHelper));
        }
#endif
    }
}
