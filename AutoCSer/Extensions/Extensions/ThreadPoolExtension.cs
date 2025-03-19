using AutoCSer.Threading;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 线程池相关扩展
    /// </summary>
    public static class ThreadPoolExtension
    {
        /// <summary>
        /// 线程池运行任务并等待返回结果
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="threadPool">线程池</param>
        /// <param name="task">任务</param>
        /// <returns>返回结果</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Task<T> RunTask<T>(this ThreadPool threadPool, Func<T> task)
        {
            return new ThreadPoolTask<T>(threadPool, task).Wait();
        }
    }
}
