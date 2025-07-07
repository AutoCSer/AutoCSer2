using System;
using System.Threading.Tasks;

namespace AutoCSer
{
    /// <summary>
    /// The default value has completed the task
    /// 默认值已完成任务
    /// </summary>
    /// <typeparam name="T">Return value type</typeparam>
    public static class CompletedTask<T>
    {
        /// <summary>
        /// The default value has completed the task
        /// 默认值已完成任务
        /// </summary>
#if NetStandard21
        public static readonly Task<T?> Default = Task.FromResult(default(T));
#else
        public static readonly Task<T> Default = Task.FromResult(default(T));
#endif
    }
}
