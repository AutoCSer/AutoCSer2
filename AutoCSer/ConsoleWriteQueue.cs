using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer
{
    /// <summary>
    /// 控制台输出队列
    /// </summary>
    public static class ConsoleWriteQueue
    {
        /// <summary>
        /// 控制台输出信息
        /// </summary>
        private struct Message
        {
            /// <summary>
            /// 输出信息
            /// </summary>
            private readonly string message;
            /// <summary>
            /// 文字颜色
            /// </summary>
            private readonly byte foregroundColor;
            /// <summary>
            /// 背景颜色
            /// </summary>
            private readonly byte backgroundColor;
            /// <summary>
            /// 输出信息以后是否恢复文字与背景颜色
            /// </summary>
            private readonly bool restoreColor;
            /// <summary>
            /// 是否换行
            /// </summary>
            private readonly bool isWriteLine;
            /// <summary>
            /// 控制台输出信息
            /// </summary>
            /// <param name="message">输出信息</param>
            /// <param name="foregroundColor">文字颜色</param>
            /// <param name="backgroundColor">背景颜色</param>
            /// <param name="restoreColor">输出信息以后是否恢复文字与背景颜色</param>
            /// <param name="isWriteLine">是否换行</param>
            internal Message(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor, bool restoreColor, bool isWriteLine)
            {
                this.message = message;
                this.foregroundColor = (byte)(int)foregroundColor;
                this.backgroundColor = (byte)(int)backgroundColor;
                this.restoreColor = restoreColor;
                this.isWriteLine = isWriteLine;
            }
            /// <summary>
            /// 控制台输出
            /// </summary>
            internal void Write()
            {
                ConsoleColor foregroundColor = Console.ForegroundColor, backgroundColor = Console.BackgroundColor;
                if (foregroundColor != (ConsoleColor)(int)this.foregroundColor) Console.ForegroundColor = (ConsoleColor)(int)this.foregroundColor;
                if (backgroundColor != (ConsoleColor)(int)this.backgroundColor) Console.BackgroundColor = (ConsoleColor)(int)this.backgroundColor;
                if (isWriteLine)
                {
                    if (string.IsNullOrEmpty(message)) Console.WriteLine();
                    else Console.WriteLine(message);
                }
                else Console.Write(message);
                if (restoreColor)
                {
                    if (foregroundColor != (ConsoleColor)(int)this.foregroundColor) Console.ForegroundColor = foregroundColor;
                    if (backgroundColor != (ConsoleColor)(int)this.backgroundColor) Console.BackgroundColor = backgroundColor;
                }
            }
        }
        /// <summary>
        /// 当前输出队列
        /// </summary>
        private static LeftArray<Message> outputQueue = new LeftArray<Message>(0);
        /// <summary>
        /// 当前添加队列
        /// </summary>
        private static LeftArray<Message> appendQueue = new LeftArray<Message>(0);
        /// <summary>
        /// 队列访问锁
        /// </summary>
        private static readonly object writeLock = new object();
        /// <summary>
        /// 是否已经启动输出线程
        /// </summary>
        private static bool isThread;
        /// <summary>
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
                LeftArray<Message> queue = appendQueue;
                appendQueue = outputQueue;
                outputQueue = queue;
                Monitor.Exit(writeLock);

                foreach (Message message in outputQueue) message.Write();
                outputQueue.ClearLength();
            }
            while (true);
        }
        /// <summary>
        /// 添加到输出队列
        /// </summary>
        /// <param name="message"></param>
        private static void append(Message message)
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
        /// 添加到输出队列
        /// </summary>
        /// <param name="message">输出信息</param>
        /// <param name="foregroundColor">文字颜色</param>
        /// <param name="backgroundColor">背景颜色</param>
        /// <param name="restoreColor">输出信息以后是否恢复文字与背景颜色</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Write(string message, ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black, bool restoreColor = true)
        {
            append(new Message(message, foregroundColor, backgroundColor, restoreColor, false));
        }
        /// <summary>
        /// 添加到输出队列（最后增加换行输出）
        /// </summary>
        /// <param name="message">输出信息</param>
        /// <param name="foregroundColor">文字颜色</param>
        /// <param name="backgroundColor">背景颜色</param>
        /// <param name="restoreColor">输出信息以后是否恢复文字与背景颜色</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void WriteLine(string? message = null, ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black, bool restoreColor = true)
#else
        public static void WriteLine(string message = null, ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black, bool restoreColor = true)
#endif
        {
            append(new Message(message ?? string.Empty, foregroundColor, backgroundColor, restoreColor, true));
        }
        /// <summary>
        /// 测试断点信息添加到输出队列
        /// </summary>
        /// <param name="message">断点信息</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
#if !DEBUG
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
#if NetStandard21
        public static void Breakpoint(string? message = null, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void Breakpoint(string message = null, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            append(new Message($"{callerFilePath}+{callerMemberName}[{callerLineNumber}] 测试断点信息 {message}", ConsoleColor.Red, ConsoleColor.Black, true, true));
        }
        /// <summary>
        /// 测试断点信息添加到输出队列
        /// </summary>
        /// <param name="message">断点信息</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
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
        /// 返回值状态非成功则添加测试断点信息
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        /// <returns>返回值状态是否成功</returns>
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
        /// 返回值状态非成功则添加测试断点信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnValue">返回值</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        /// <returns>返回值状态是否成功</returns>
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
