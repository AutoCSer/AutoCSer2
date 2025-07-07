using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer
{
    /// <summary>
    /// Console output queue
    /// 控制台输出队列
    /// </summary>
    public static class ConsoleWriteQueue
    {
        /// <summary>
        /// Current output queue
        /// 当前输出队列
        /// </summary>
        private static LeftArray<ConsoleWriteMessage> outputQueue = new LeftArray<ConsoleWriteMessage>(0);
        /// <summary>
        /// The queue for the current added output
        /// 当前添加输出队列
        /// </summary>
        private static LeftArray<ConsoleWriteMessage> appendQueue = new LeftArray<ConsoleWriteMessage>(0);
        /// <summary>
        /// Queue access lock
        /// 队列访问锁
        /// </summary>
        private static readonly object writeLock = new object();
        /// <summary>
        /// Whether the output thread has been started
        /// 是否已经启动输出线程
        /// </summary>
        private static bool isThread;
        /// <summary>
        /// Output thread processing
        /// 输出线程处理
        /// </summary>
        private static void write()
        {
            do
            {
                Monitor.Enter(writeLock);
                if (appendQueue.Count == 0)
                {
                    isThread = false;
                    Monitor.Exit(writeLock);
                    return;
                }
                LeftArray<ConsoleWriteMessage> queue = appendQueue;
                appendQueue = outputQueue;
                outputQueue = queue;
                Monitor.Exit(writeLock);

                foreach (ConsoleWriteMessage message in outputQueue) message.Write();
                outputQueue.ClearLength();
            }
            while (true);
        }
        /// <summary>
        /// Add to the output queue
        /// 添加到输出队列
        /// </summary>
        /// <param name="message"></param>
        private static void append(ConsoleWriteMessage message)
        {
            bool isThread = false;
            Monitor.Enter(writeLock);
            if (appendQueue.TryAdd(message))
            {
                if (appendQueue.Count == 1 && !ConsoleWriteQueue.isThread) ConsoleWriteQueue.isThread = isThread = true;
                Monitor.Exit(writeLock);
            }
            else
            {
                try
                {
                    appendQueue.Add(message);
                    if (appendQueue.Count == 1 && !ConsoleWriteQueue.isThread) ConsoleWriteQueue.isThread = isThread = true;
                }
                finally { Monitor.Exit(writeLock); }
            }
            if (isThread) AutoCSer.Threading.ThreadPool.TinyBackground.Start(write);
        }
        /// <summary>
        /// Add to the output queue
        /// 添加到输出队列
        /// </summary>
        /// <param name="message">Output message
        /// 输出信息</param>
        /// <param name="foregroundColor">Text color
        /// 文字颜色</param>
        /// <param name="backgroundColor">Background color
        /// 背景颜色</param>
        /// <param name="restoreColor">Whether to restore the text and background color after outputting the message
        /// 输出信息以后是否恢复文字与背景颜色</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Write(string message, ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black, bool restoreColor = true)
        {
            append(new ConsoleWriteMessage(message, foregroundColor, backgroundColor, restoreColor, false));
        }
        /// <summary>
        /// Add to the output queue (finally add line break output)
        /// 添加到输出队列（最后增加换行输出）
        /// </summary>
        /// <param name="message">Output message
        /// 输出信息</param>
        /// <param name="foregroundColor">Text color
        /// 文字颜色</param>
        /// <param name="backgroundColor">Background color
        /// 背景颜色</param>
        /// <param name="restoreColor">Whether to restore the text and background color after outputting the message
        /// 输出信息以后是否恢复文字与背景颜色</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void WriteLine(string? message = null, ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black, bool restoreColor = true)
#else
        public static void WriteLine(string message = null, ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black, bool restoreColor = true)
#endif
        {
            append(new ConsoleWriteMessage(message ?? string.Empty, foregroundColor, backgroundColor, restoreColor, true));
        }
        /// <summary>
        /// Test breakpoint information is added to the output queue
        /// 测试断点信息添加到输出队列
        /// </summary>
        /// <param name="message">Breakpoint message
        /// 断点信息</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
#if !DEBUG
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
#if NetStandard21
        public static void Breakpoint(string? message = null, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void Breakpoint(string message = null, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            append(new ConsoleWriteMessage($"{callerFilePath}+{callerMemberName}[{callerLineNumber}] 测试断点信息 {message}", ConsoleColor.Red, ConsoleColor.Black, true, true));
        }
        /// <summary>
        /// Test breakpoint information is added to the output queue
        /// 测试断点信息添加到输出队列
        /// </summary>
        /// <param name="message">Breakpoint message
        /// 断点信息</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
#if NetStandard21
        public static void BreakpointLog(string message, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void BreakpointLog(string message, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            Breakpoint(message, callerMemberName, callerFilePath, callerLineNumber);
            LogHelper.BreakpointIgnoreException(message, LogLevelEnum.Breakpoint, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// If the return value status is not successful, add the test breakpoint message
        /// 返回值状态非成功则添加测试断点信息
        /// </summary>
        /// <param name="returnValue">Return value</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        /// <returns>Return whether the value status is successful
        /// 返回值状态是否成功</returns>
#if NetStandard21
        public static bool Breakpoint(CommandClientReturnValue returnValue, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static bool Breakpoint(CommandClientReturnValue returnValue, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            if (returnValue.IsSuccess) return true;
            Breakpoint(string.IsNullOrEmpty(returnValue.ErrorMessage) ? returnValue.ReturnType.ToString() : $"[{returnValue.ReturnType}]{returnValue.ErrorMessage}", callerMemberName, callerFilePath, callerLineNumber);
            return false;
        }
        /// <summary>
        /// If the return value status is not successful, add the test breakpoint message
        /// 返回值状态非成功则添加测试断点信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnValue">Return value</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        /// <returns>Return whether the value status is successful
        /// 返回值状态是否成功</returns>
#if NetStandard21
        public static bool Breakpoint<T>(CommandClientReturnValue<T> returnValue, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static bool Breakpoint<T>(CommandClientReturnValue<T> returnValue, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            if (returnValue.IsSuccess) return true;
            Breakpoint(string.IsNullOrEmpty(returnValue.ErrorMessage) ? returnValue.ReturnType.ToString() : $"[{returnValue.ReturnType}]{returnValue.ErrorMessage}", callerMemberName, callerFilePath, callerLineNumber);
            return false;
        }
    }
}
