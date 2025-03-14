﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
#if !NetStandard21
using ValueTask = System.Threading.Tasks.Task;
#endif

namespace AutoCSer.Extensions
{
    /// <summary>
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
#if NetStandard21
        /// <summary>
        /// A warning used to clear an await inside async without waiting for the task to execute
        /// 不等待任务执行的情况下，用于清除 async 内部提示 await 的警告
        /// </summary>
        /// <param name="task"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void NotWait(this ValueTask task) { }
#endif
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
    }
}
