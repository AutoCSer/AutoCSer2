using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer
{
    /// <summary>
    /// 开启线程调用 Task.Wait() 防止后续操作出现同步阻塞 Task 调度线程
    /// </summary>
    internal sealed class WaitTask
    {
        /// <summary>
        /// 被调度任务
        /// </summary>
        private readonly Task task;
        /// <summary>
        /// 返回值等待锁
        /// </summary>
        private System.Threading.ManualResetEvent waitLock;
        /// <summary>
        /// 获取 Task.Result
        /// </summary>
        /// <param name="task"></param>
        internal WaitTask(Task task)
        {
            this.task = task;
            waitLock = new System.Threading.ManualResetEvent(false);
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(wait);
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        private void wait()
        {
            try
            {
                task.Wait();
            }
            finally { waitLock.setDispose(); }
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal Exception? Wait()
#else
        internal Exception Wait()
#endif
        {
            waitLock.WaitOne();
            return task.Exception;
        }
    }
    /// <summary>
    /// 开启线程获取 Task.Result 防止后续操作出现同步阻塞 Task 调度线程
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    internal sealed class WaitTask<T>
    {
        /// <summary>
        /// 被调度任务
        /// </summary>
        private readonly Task<T> task;
        /// <summary>
        /// 返回值等待锁
        /// </summary>
        private System.Threading.ManualResetEvent waitLock;
        /// <summary>
        /// 任务返回值
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private T result;
        /// <summary>
        /// 获取 Task.Result
        /// </summary>
        /// <param name="task"></param>
        internal WaitTask(Task<T> task)
        {
            this.task = task;
            waitLock = new System.Threading.ManualResetEvent(false);
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(getResult);
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        private void getResult()
        {
            try
            {
                result = task.Result;
            }
            finally { waitLock.setDispose(); }
        }
        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal T GetResult(out Exception? exception)
#else
        internal T GetResult(out Exception exception)
#endif
        {
            waitLock.WaitOne();
            exception = task.Exception;
            return result;
        }
    }
}
