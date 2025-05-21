using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer
{
    /// <summary>
    /// 测试断点
    /// </summary>
    public static class Breakpoint
    {
        /// <summary>
        /// 测试断点信息添加到输出队列
        /// </summary>
        /// <param name="message">断点信息</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
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
        /// 返回值状态非成功则添加测试断点信息
        /// </summary>
        /// <param name="returnValue">返回值</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        /// <returns>返回值状态是否成功</returns>
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
        /// 返回值状态非成功则添加测试断点信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnValue">返回值</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        /// <returns>返回值状态是否成功</returns>
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
        /// 添加测试断点信息并返回 false
        /// </summary>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        /// <returns>返回值状态是否成功</returns>
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
        /// 添加测试断点信息并返回 false
        /// </summary>
        /// <param name="message">附加信息</param>
        /// <param name="callerMemberName">调用成员名称</param>
        /// <param name="callerFilePath">调用源代码文件路径</param>
        /// <param name="callerLineNumber">调用源代码行号</param>
        /// <returns>返回值状态是否成功</returns>
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
        /// 读取标准输入延时时间为 100 毫秒
        /// </summary>
        private static readonly TimeSpan readLineDelayTime = new TimeSpan(0, 0, 0, 0, 100);
        /// <summary>
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
