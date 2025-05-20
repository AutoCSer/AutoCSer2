using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// UI 线程同步上下文调用 async Task 避免死锁操作
    /// </summary>
    public sealed class UISynchronousTask
    {
        /// <summary>
        /// 获取任务委托
        /// </summary>
        private readonly Func<Task> getTask;
        /// <summary>
        /// 等待任务执行结果
        /// </summary>
        private readonly ManualResetEvent wait;
        /// <summary>
        /// 任务执行异常信息
        /// </summary>
#if NetStandard21
        private Exception? exception;
#else
        private Exception exception;
#endif
        /// <summary>
        /// 任务执行异常信息，正常执行无异常返回 null
        /// </summary>
#if NetStandard21
        public Exception? Exception
#else
        public Exception Exception
#endif
        {
            get
            {
                wait.WaitOne();
                return exception;
            }
        }
        /// <summary>
        /// UI 线程同步上下文调用 async Task 避免死锁操作
        /// </summary>
        /// <param name="getTask">获取任务委托</param>
        public UISynchronousTask(Func<Task> getTask)
        {
            this.getTask = getTask;
            wait = new ManualResetEvent(false);
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(run);
        }
        /// <summary>
        /// 执行任务线程
        /// </summary>
        private async void run()
        {
            try
            {
                await getTask();
            }
            catch (Exception exception)
            {
                this.exception = exception;
            }
            finally { wait.setDispose(); }
        }
        /// <summary>
        /// 任务执行结果
        /// </summary>
        public void Wait()
        {
            wait.WaitOne();
            if (exception != null) throw exception;
        }
        /// <summary>
        /// UI 线程同步上下文调用 async Task 避免死锁操作
        /// </summary>
        /// <param name="getTask"></param>
        /// <returns>任务执行异常信息，正常执行无异常返回 null</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static Exception? Wait(Func<Task> getTask)
#else
        public static Exception Wait(Func<Task> getTask)
#endif
        {
            return new UISynchronousTask(getTask).Exception;
        }

        /// <summary>
        /// UI 线程同步上下文调用 async Task 避免死锁操作
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="getTask">获取任务委托</param>
        /// <returns>任务执行结果</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T GetResult<T>(Func<Task<T>> getTask)
        {
            return new UISynchronousTask<T>(getTask).Result;
        }
        /// <summary>
        /// UI 线程同步上下文调用 async Task 避免死锁操作
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="getTask">获取任务委托</param>
        /// <param name="exception">任务执行异常信息，正常执行无异常返回 null</param>
        /// <returns>任务执行结果</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T GetResult<T>(Func<Task<T>> getTask, out Exception? exception)
#else
        public static T GetResult<T>(Func<Task<T>> getTask, out Exception exception)
#endif
        {
            return new UISynchronousTask<T>(getTask).GetResult(out exception);
        }
    }
    /// <summary>
    /// UI 线程同步上下文调用 async Task 避免死锁操作
    /// </summary>
    /// <typeparam name="T">返回数据类型</typeparam>
    public sealed class UISynchronousTask<T>
    {
        /// <summary>
        /// 获取任务委托
        /// </summary>
        private readonly Func<Task<T>> getTask;
        /// <summary>
        /// 等待任务执行结果
        /// </summary>
        private readonly ManualResetEvent wait;
        /// <summary>
        /// 任务执行异常信息
        /// </summary>
#if NetStandard21
        private Exception? exception;
#else
        private Exception exception;
#endif
        /// <summary>
        /// 任务执行异常信息，正常执行无异常返回 null
        /// </summary>
#if NetStandard21
        public Exception? Exception
#else
        public Exception Exception
#endif
        {
            get
            {
                wait.WaitOne();
                return exception;
            }
        }
        /// <summary>
        /// 任务执行结果
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private T result;
        /// <summary>
        /// UI 线程同步上下文调用 async Task 避免死锁操作
        /// </summary>
        /// <param name="getTask">获取任务委托</param>
        public UISynchronousTask(Func<Task<T>> getTask)
        {
            this.getTask = getTask;
            wait = new ManualResetEvent(false);
            AutoCSer.Threading.ThreadPool.TinyBackground.Start(run);
        }
        /// <summary>
        /// 执行任务线程
        /// </summary>
        private async void run()
        {
            try
            {
                result = await getTask();
            }
            catch (Exception exception)
            {
                this.exception = exception;
            }
            finally { wait.setDispose(); }
        }
        /// <summary>
        /// 任务执行结果
        /// </summary>
        public T Result
        {
            get
            {
                wait.WaitOne();
                if (exception == null) return result;
                throw exception;
            }
        }
        /// <summary>
        /// 获取任务执行结果与任务执行异常信息
        /// </summary>
        /// <param name="exception">任务执行异常信息，正常执行无异常返回 null</param>
        /// <returns>任务执行结果</returns>
#if NetStandard21
        public T GetResult(out Exception? exception)
#else
        public T GetResult(out Exception exception)
#endif
        {
            wait.WaitOne();
            exception = this.exception;
            return result;
        }
    }
}
