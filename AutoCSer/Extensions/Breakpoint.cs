using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

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
        public static void ConsoleWriteQueue(string message = null, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
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
        public static bool ConsoleWriteQueue(CommandClientReturnValue returnValue, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
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
        public static bool ConsoleWriteQueue<T>(CommandClientReturnValue<T> returnValue, [CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            return AutoCSer.ConsoleWriteQueue.Breakpoint(returnValue, callerMemberName, callerFilePath, callerLineNumber);
        }
    }
}
