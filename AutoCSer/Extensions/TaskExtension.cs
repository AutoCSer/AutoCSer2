using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Task expansion operation
    /// 任务扩展操作
    /// </summary>
    public static class TaskExtension
    {
        /// <summary>
        /// A warning used to clear an await inside async without waiting for the task to execute
        /// 不等待任务执行的情况下，用于清除 async 内部提示 await 的警告
        /// </summary>
        /// <param name="task"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void NotWait(this Task task) { }
        /// <summary>
        /// A warning used to clear an await inside async without waiting for the task to execute
        /// 不等待任务执行的情况下，用于清除 async 内部提示 await 的警告
        /// </summary>
        /// <param name="task"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void NotWait(this ValueTask task) { }
        /// <summary>
        /// Capture and output the exception log
        /// 捕获并输出异常日志
        /// </summary>
        /// <param name="task"></param>
        /// <param name="isQueue">The default is false, indicating that the unfinished queue will not be added
        /// 默认为 false 表示不加入未完成队列</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
#if NetStandard21
        public static void Catch(this Task task, bool isQueue = false, [CallerFilePath] string? callerFilePath = null, [CallerMemberName] string? callerMemberName = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void Catch(this Task task, bool isQueue = false, [CallerFilePath] string callerFilePath = null, [CallerMemberName] string callerMemberName = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            if (task.IsCompleted)
            {
                var exception = task.Exception;
                if (exception != null) AutoCSer.LogHelper.Default.ExceptionIgnoreException(exception, null, LogLevelEnum.Exception, callerFilePath, callerMemberName, callerLineNumber);
            }
            else new AutoCSer.Threading.CatchTask(task, isQueue, callerFilePath ?? string.Empty, callerMemberName ?? string.Empty, callerLineNumber);
        }
        /// <summary>
        /// Capture and output the exception log
        /// 捕获并输出异常日志
        /// </summary>
        /// <param name="task"></param>
        /// <param name="isQueue">The default is false, indicating that the unfinished queue will not be added
        /// 默认为 false 表示不加入未完成队列</param>
        /// <param name="callerFilePath">Caller the source code file path</param>
        /// <param name="callerMemberName">Caller member name</param>
        /// <param name="callerLineNumber">Caller the line number of the source code</param>
#if NetStandard21
        public static void Catch(this ValueTask task, bool isQueue = false, [CallerFilePath] string? callerFilePath = null, [CallerMemberName] string? callerMemberName = null, [CallerLineNumber] int callerLineNumber = 0)
#else
        public static void Catch(this ValueTask task, bool isQueue = false, [CallerFilePath] string callerFilePath = null, [CallerMemberName] string callerMemberName = null, [CallerLineNumber] int callerLineNumber = 0)
#endif
        {
            Catch(task.AsTask(), isQueue, callerFilePath, callerMemberName, callerLineNumber);
        }
        //        /// <summary>
        //        /// ValueTask 兼容
        //        /// </summary>
        //        /// <param name="task"></param>
        //        /// <returns></returns>
        //        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //        public static ValueTask ToValueTask(this Task task)
        //        {
        //#if NetStandard21
        //            return new ValueTask(task);
        //#else
        //            return task;
        //#endif
        //        }
        /// <summary>
        /// Getting the Result from the new thread prevents subsequent operations from blocking the Task scheduler thread synchronously
        /// 从新线程中获取 Result 防止后续操作出现同步阻塞 Task 调度线程
        /// </summary>
        /// <typeparam name="T">Return value type
        /// 返回值类型</typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public static T getResult<T>(this Task<T> task)
        {
            if (task.IsCompleted) return task.Result;
            var exception = default(Exception);
            T value = new WaitTask<T>(task).GetResult(out exception);
            if (exception == null) return value;
            throw exception;
        }
        /// <summary>
        /// Getting the Result from the new thread prevents subsequent operations from blocking the Task scheduler thread synchronously
        /// 从新线程中获取 Result 防止后续操作出现同步阻塞 Task 调度线程
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static void wait(this Task task)
        {
            if (task.IsCompleted) return;
            var exception = new WaitTask(task).Wait();
            if (exception != null) throw exception;
        }
        /// <summary>
        /// Getting the Result from the new thread prevents subsequent operations from blocking the Task scheduler thread synchronously
        /// 从新线程中获取 Result 防止后续操作出现同步阻塞 Task 调度线程
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static void wait(this ValueTask task)
        {
            if (task.IsCompleted) return;
            var exception = new WaitTask(task.AsTask()).Wait();
            if (exception != null) throw exception;
        }
    }
}
