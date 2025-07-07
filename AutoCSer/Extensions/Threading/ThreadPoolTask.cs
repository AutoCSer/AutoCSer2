using AutoCSer.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 线程池任务
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    public sealed class ThreadPoolTask<T>
    {
        /// <summary>
        /// 返回结果等待锁
        /// </summary>
        private readonly System.Threading.SemaphoreSlim wait;
        /// <summary>
        /// 任务
        /// </summary>
        private readonly Func<T> task;
        /// <summary>
        /// Return value
        /// </summary>
#if NetStandard21
        [AllowNull]
#endif
        private T returnValue;
        /// <summary>
        /// 执行任务异常信息
        /// </summary>
#if NetStandard21
        private Exception? exception;
#else
        private Exception exception;
#endif
        /// <summary>
        /// 是否已完成
        /// </summary>
        private bool isCompleted;
        /// <summary>
        /// 线程池任务
        /// </summary>
        /// <param name="threadPool">线程池</param>
        /// <param name="task">任务</param>
        public ThreadPoolTask(ThreadPool threadPool, Func<T> task)
        {
            this.task = task;
            wait = new System.Threading.SemaphoreSlim(0, 1);
            threadPool.FastStart(run);
        }
        /// <summary>
        /// Execute the task
        /// 执行任务
        /// </summary>
        private void run()
        {
            try
            {
                returnValue = task();
            }
            catch(Exception exception)
            {
                this.exception = exception;
            }
            finally
            {
                isCompleted = true;
                wait.Release();
            }
        }
        /// <summary>
        /// 等待任务执行完成
        /// </summary>
        /// <returns></returns>
        public async Task<T> Wait()
        {
            if (!isCompleted)
            {
                await wait.WaitAsync();
                wait.Release();
            }
            if (exception == null) return returnValue;
            throw exception;
        }
    }
}
