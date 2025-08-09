using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// await enforces task.run operations (executing in async Task Main also prevents UI thread deadlock)
    /// await 强制 Task.Run 操作（在 async Task Main 中执行也可以防止 UI 线程死锁）
    /// </summary>
    public sealed class SwitchAwaiter : INotifyCompletion
    {
        /// <summary>
        /// Completed status
        /// 完成状态
        /// </summary>
        private readonly bool isCompleted;
        /// <summary>
        /// Completed status
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get { return isCompleted; } }
        /// <summary>
        /// await 强制 Task.Run 操作
        /// </summary>
        private SwitchAwaiter() { }
        /// <summary>
        /// 已完成任务，不执行 Task.Run 操作
        /// </summary>
        /// <param name="isCompleted"></param>
        private SwitchAwaiter(bool isCompleted) { this.isCompleted = isCompleted; }
        /// <summary>
        /// await support
        /// await 支持
        /// </summary>
        /// <returns></returns>
        public async Task Wait()
        {
            await this;
        }
        /// <summary>
        /// Only for supporting await
        /// 仅用于支持 await
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void GetResult() { }
        /// <summary>
        /// await support
        /// await 支持
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public SwitchAwaiter GetAwaiter()
        {
            return this;
        }
        /// <summary>
        /// Set asynchronous callback
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            Task.Run(continuation);
        }

        /// <summary>
        /// await enforces task.run operations (executing in async Task Main also prevents UI thread deadlock)
        /// await 强制 Task.Run 操作（在 async Task Main 中执行也可以防止 UI 线程死锁）
        /// </summary>
        public static readonly SwitchAwaiter Default = new SwitchAwaiter();
        /// <summary>
        /// The task. Run operation is not executed because the Task is complete
        /// 已完成任务，不执行 Task.Run 操作
        /// </summary>
        public static readonly SwitchAwaiter Completed = new SwitchAwaiter(true);
    }
}
