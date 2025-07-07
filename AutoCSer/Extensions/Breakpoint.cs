using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer
{
    /// <summary>
    /// Test breakpoint
    /// 测试断点
    /// </summary>
    public static class Breakpoint
    {
        /// <summary>
        /// Test breakpoint information is added to the output queue
        /// 测试断点信息添加到输出队列
        /// </summary>
        /// <param name="message">Breakpoint message</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void ConsoleWriteQueue(string? message = null, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void ConsoleWriteQueue(string message = null, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            AutoCSer.ConsoleWriteQueue.Breakpoint(message, callerMemberName, callerFilePath, callerLineNumber);
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
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static bool ConsoleWriteQueue(CommandClientReturnValue returnValue, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static bool ConsoleWriteQueue(CommandClientReturnValue returnValue, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            return AutoCSer.ConsoleWriteQueue.Breakpoint(returnValue, callerMemberName, callerFilePath, callerLineNumber);
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
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static bool ConsoleWriteQueue<T>(CommandClientReturnValue<T> returnValue, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static bool ConsoleWriteQueue<T>(CommandClientReturnValue<T> returnValue, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            return AutoCSer.ConsoleWriteQueue.Breakpoint(returnValue, callerMemberName, callerFilePath, callerLineNumber);
        }
        /// <summary>
        /// Add the test breakpoint message and return false
        /// 添加测试断点信息并返回 false
        /// </summary>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        /// <returns>false</returns>
#if NetStandard21
        public static bool ReturnFalse([CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static bool ReturnFalse([CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            AutoCSer.ConsoleWriteQueue.Breakpoint(nameof(ReturnFalse), callerMemberName, callerFilePath, callerLineNumber);
            return false;
        }
        /// <summary>
        /// Add the test breakpoint message and return false
        /// 添加测试断点信息并返回 false
        /// </summary>
        /// <param name="message">附加信息</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        /// <returns>false</returns>
#if NetStandard21
        public static bool ReturnFalse<T>(T message, [CallerMemberName] string? callerMemberName = null, [CallerFilePath] string? callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static bool ReturnFalse<T>(T message, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            AutoCSer.ConsoleWriteQueue.Breakpoint(AutoCSer.JsonSerializer.Serialize(message), callerMemberName, callerFilePath, callerLineNumber);
            return false;
        }

        /// <summary>
        /// The delay time for reading the standard input is 100 milliseconds
        /// 读取标准输入延时时间为 100 毫秒
        /// </summary>
        private static readonly TimeSpan readLineDelayTime = new TimeSpan(0, 0, 0, 0, 100);
        /// <summary>
        /// Read the standard input and delay to avoid running the CPU at 100% in the Linux background
        /// 读取标准输入并延时，避免 Linux 后台运行 CPU 100%
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        public static async Task<string?> ReadLineDelay()
#else
        public static async Task<string> ReadLineDelay()
#endif
        {
            var value = Console.ReadLine();
            await Task.Delay(readLineDelayTime);
            return value;
        }
    }
}
