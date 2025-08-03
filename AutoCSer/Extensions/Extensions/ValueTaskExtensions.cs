using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Task expansion operation
    /// Task 扩展操作
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ValueTaskExtensions
    {
        /// <summary>
        /// Task
        /// </summary>
        private readonly ValueTask task;
        /// <summary>
        /// Task expansion operation
        /// Task 扩展操作
        /// </summary>
        /// <param name="task"></param>
        public ValueTaskExtensions(ValueTask task)
        {
            this.task = task;
        }
        /// <summary>
        /// Getting the Result from the new thread prevents subsequent operations from blocking the Task scheduler thread synchronously
        /// 从新线程中获取 Result 防止后续操作出现同步阻塞 Task 调度线程
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Wait()
        {
            task.wait();
        }
        /// <summary>
        /// Capture and output the exception log
        /// 捕获并输出异常日志
        /// </summary>
        /// <param name="isQueue">The default is false, indicating that the unfinished queue will not be added
        /// 默认为 false 表示不加入未完成队列</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void Catch(bool isQueue = false, [CallerFilePath] string? callerFilePath = null, [CallerMemberName] string? callerMemberName = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public void Catch(bool isQueue = false, [CallerFilePath] string callerFilePath = null, [CallerMemberName] string callerMemberName = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            task.Catch(isQueue, callerFilePath, callerMemberName, callerLineNumber);
        }
    }
}
