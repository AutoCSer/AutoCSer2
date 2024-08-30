using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
#if DotNet45 || NetStandard2
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
        /// 不等待任务执行的情况下，用于清除 async 内部提示 await 的警告
        /// </summary>
        /// <param name="task">任务</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void NotWait(this Task task) { }
        /// <summary>
        /// ValueTask 兼容
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ValueTask ToValueTask(this Task task)
        {
#if DotNet45 || NetStandard2
            return task;
#else
            return new ValueTask(task);
#endif
        }
    }
}
