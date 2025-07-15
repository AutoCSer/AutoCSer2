#if !NetStandard21
using System;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    /// <summary>
    /// Task 封装
    /// </summary>
    public sealed class ValueTask : Task
    {
        /// <summary>
        /// Task 封装
        /// </summary>
        /// <param name="action"></param>
        public ValueTask(Action action) : base(action) { }
        /// <summary>
        /// 转换为 Task
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task AsTask()
        {
            return this;
        }

        /// <summary>
        /// Get the completed task
        /// 获取已完成任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public new static ValueTask<T> FromResult<T>(T value)
        {
            return new ValueTask<T>(value);
        }
        /// <summary>
        /// The task is completed by default
        /// 默认已完成任务
        /// </summary>
#if DotNet45
        public static readonly ValueTask CompletedTask;
#else
        public new static readonly ValueTask CompletedTask;
#endif

        static ValueTask()
        {
            CompletedTask = new ValueTask(AutoCSer.Common.EmptyAction);
            CompletedTask.RunSynchronously();
        }
    }
    /// <summary>
    /// Task 封装
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ValueTask<T> : Task<T>
    {
        /// <summary>
        /// 已完成任务
        /// </summary>
        /// <param name="value"></param>
        internal ValueTask(T value) : base(() => value)
        {
            RunSynchronously();
        }
        /// <summary>
        /// 转换为 Task
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Task<T> AsTask()
        {
            return this;
        }
    }
}
#endif
