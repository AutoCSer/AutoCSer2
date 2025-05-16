using AutoCSer.Extensions;
using AutoCSer.Threading;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.Log
{
    /// <summary>
    /// 文件日志
    /// </summary>
    public class File : ILog, IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// 日志文件名
        /// </summary>
        protected readonly string fileName;
        /// <summary>
        /// 文件字符编码
        /// </summary>
        protected readonly Encoding encoding;
        /// <summary>
        /// 日志文件流
        /// </summary>
#if NetStandard21
        private FileStream? fileStream;
#else
        private FileStream fileStream;
#endif
        /// <summary>
        /// 日志文件流文件名称
        /// </summary>
        private string fileStreamName;
        /// <summary>
        /// 日志文件流
        /// </summary>
#if NetStandard21
        private StreamWriter? streamWriter;
#else
        private StreamWriter streamWriter;
#endif
        /// <summary>
        /// 日志队列访问锁
        /// </summary>
        private readonly SemaphoreSlimLock logLock = new SemaphoreSlimLock(1);
        /// <summary>
        /// 允许日志级别
        /// </summary>
        public readonly LogLevelEnum Level;
        /// <summary>
        /// 是否需要检查文件写入状态
        /// </summary>
        private bool isCheckFlush;
        /// <summary>
        /// 是否已经释放资源
        /// </summary>
        private bool isDisposed;
        /// <summary>
        /// 日志处理
        /// </summary>
        /// <param name="level">允许日志级别</param>
        /// <param name="fileName">日志文件</param>
        /// <param name="encoding">默认为 UTF-8</param>
#if NetStandard21
        public File(LogLevelEnum level = LogLevelEnum.All, string fileName = AutoCSer.Common.NamePrefix + ".log", Encoding? encoding = null)
#else
        public File(LogLevelEnum level = LogLevelEnum.All, string fileName = AutoCSer.Common.NamePrefix + ".log", Encoding encoding = null)
#endif
        {
            Level = level;
            this.fileName = fileName;
            this.encoding = encoding ?? Encoding.UTF8;
            fileStreamName = string.Empty;
            open().wait();
        }
        /// <summary>
        /// 打开日志文件
        /// </summary>
        private async Task open()
        {
            bool isOpen = false;
            string fileName = this.fileName;
            try
            {
                do
                {
                    try
                    {
                        fileStream = await AutoCSer.Common.CreateFileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 4 << 10);
                        fileStreamName = fileName;
                        await AutoCSer.Common.Seek(fileStream, 0, SeekOrigin.End);
                        streamWriter = new StreamWriter(fileStream, encoding, 4 << 10);
                        isOpen = true;
                        return;
                    }
                    catch (Exception exception)
                    {
                        if (streamWriter != null)
                        {
                            await streamWriter.DisposeAsync();
                            streamWriter = null;
                        }
                        if (fileStream != null)
                        {
                            await fileStream.DisposeAsync();
                            fileStream = null;
                        }
                        await AutoCSer.Common.Config.OnLogFileException(this, exception);
                    }
                    if (!await AutoCSer.Common.FileExists(fileName)) return;
                    fileName = this.fileName + "." + AutoCSer.Threading.SecondTimer.Now.ToString("yyyyMMdd-HHmmss") + "_" + ((uint)Random.Default.Next()).toHex() + ".log";
                }
                while (true);
            }
            finally
            {
                if (!isOpen)
                {
                    isDisposed = true;
                    if (streamWriter != null)
                    {
                        await streamWriter.DisposeAsync();
                        streamWriter = null;
                    }
                    if (fileStream != null)
                    {
                        await fileStream.DisposeAsync();
                        fileStream = null;
                    }
                    GC.SuppressFinalize(this);
                }
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!isDisposed) AutoCSer.Common.Wait(DisposeAsync());
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            if (!isDisposed)
            {
                await logLock.EnterAsync();
                try
                {
                    isCheckFlush = false;
                    if (!isDisposed)
                    {
                        isDisposed = true;
                        var streamWriter = this.streamWriter;
                        if (streamWriter != null)
                        {
                            await streamWriter.FlushAsync();
                            await streamWriter.DisposeAsync();
                            streamWriter = null;
                        }
                        var fileStream = this.fileStream;
                        if (fileStream != null)
                        {
                            await fileStream.FlushAsync();
                            await fileStream.DisposeAsync();
                            fileStream = null;
                        }
                        GC.SuppressFinalize(this);
                    }
                }
                finally { logLock.Exit(); }
            }
        }
        /// <summary>
        /// 判断是否支持任意级别
        /// </summary>
        /// <param name="logLevel">日志级别</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool IsAnyLevel(LogLevelEnum logLevel)
        {
            return (Level & logLevel) != 0;
        }
        /// <summary>
        /// 写日志文件
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        /// <returns>是否写入日志</returns>
#if NetStandard21
        private async Task<bool> write(string? message, Exception? exception, string? callerMemberName, string? callerFilePath, int callerLineNumber)
#else
        private async Task<bool> write(string message, Exception exception, string callerMemberName, string callerFilePath, int callerLineNumber)
#endif
        {
            string logTime = DateTime.Now.toString();
            await logLock.EnterAsync();
            try
            {
                var streamWriter = this.streamWriter;
                if (streamWriter != null)
                {
                    await streamWriter.WriteAsync(logTime);
                    if (!string.IsNullOrEmpty(callerMemberName))
                    {
                        await streamWriter.WriteAsync(@"
调用成员信息 : ");
                        await streamWriter.WriteAsync(callerMemberName);
                        if (!string.IsNullOrEmpty(callerFilePath))
                        {
                            await streamWriter.WriteAsync(" in ");
                            await streamWriter.WriteAsync(callerFilePath);
                            if (callerLineNumber != 0)
                            {
                                await streamWriter.WriteAsync(" line ");
                                await streamWriter.WriteAsync(callerLineNumber.toString());
                            }
                        }

                    }
                    if (!string.IsNullOrEmpty(message))
                    {
                        await streamWriter.WriteAsync(@"
");
                        await streamWriter.WriteAsync(message);
                    }
                    if (exception != null)
                    {
                        await streamWriter.WriteAsync(@"
");
                        await streamWriter.WriteAsync(exception.ToString());
                    }
                    await streamWriter.WriteAsync(@"
");
                    if (!isCheckFlush)
                    {
                        isCheckFlush = true;
                        AutoCSer.Threading.SecondTimer.InternalTaskArray.Append(checkFlush, 1);
                    }
                    return true;
                }
            }
            finally { logLock.Exit(); }
            return false;
        }
        /// <summary>
        /// 检查文件写入状态
        /// </summary>
        private void checkFlush()
        {
            if (isCheckFlush) Flush().NotWait();
        }
        /// <summary>
        /// 添加调试日志
        /// </summary>
        /// <param name="message">调试日志内容</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
#if NetStandard21
        public virtual Task<bool> Debug(string message, LogLevelEnum level = LogLevelEnum.Debug, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public virtual Task<bool> Debug(string message, LogLevelEnum level = LogLevelEnum.Debug, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            if ((Level & level) != 0) return write(message, null, callerMemberName, callerFilePath, callerLineNumber);
            return AutoCSer.Common.GetCompletedTask(false);
        }
        /// <summary>
        /// 添加异常日志
        /// </summary>
        /// <param name="exception">异常信息</param>
        /// <param name="message">附加信息</param>
        /// <param name="level">日志级别</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        /// <returns>是否写入日志</returns>
#if NetStandard21
        public virtual Task<bool> Exception(Exception exception, string? message = null, LogLevelEnum level = LogLevelEnum.Exception, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public virtual Task<bool> Exception(Exception exception, string message = null, LogLevelEnum level = LogLevelEnum.Exception, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            if (exception is IgnoreException) return AutoCSer.Common.GetCompletedTask(true);
            var aggregateException = exception as AggregateException;
            if (aggregateException != null && aggregateException.InnerException is IgnoreException) return AutoCSer.Common.GetCompletedTask(true);
            if ((Level & level) != 0) return write(message, exception, callerMemberName, callerFilePath, callerLineNumber);
            return AutoCSer.Common.GetCompletedTask(false);
        }
        /// <summary>
        /// 等待写入完成
        /// </summary>
        /// <returns>写盘是否成功</returns>
        public virtual async Task<bool> Flush()
        {
            if (!isDisposed)
            {
                await logLock.EnterAsync();
                try
                {
                    isCheckFlush = false;
                    var streamWriter = this.streamWriter;
                    if (streamWriter != null)
                    {
                        await streamWriter.FlushAsync();
                        var fileStream = this.fileStream;
                        if (fileStream != null)
                        {
                            await fileStream.FlushAsync();
                            return true;
                        }
                    }
                }
                finally { logLock.Exit(); }
            }
            return false;
        }
        /// <summary>
        /// 移动日志文件
        /// </summary>
        /// <returns>新的日志文件名称</returns>
#if NetStandard21
        internal async Task<string?> MoveBak()
#else
        internal async Task<string> MoveBak()
#endif
        {
            await logLock.EnterAsync();
            try
            {
                if (fileStream != null)
                {
                    isCheckFlush = false;
                    if (streamWriter != null)
                    {
                        await streamWriter.FlushAsync();
                        await streamWriter.DisposeAsync();
                        streamWriter = null;
                    }
                    await fileStream.DisposeAsync();
                    fileStream = null;
                    try
                    {
                        return await AutoCSer.IO.File.MoveBak(fileStreamName);
                    }
                    finally { await open(); }
                }
            }
            finally { logLock.Exit(); }
            return null;
        }
    }
}
