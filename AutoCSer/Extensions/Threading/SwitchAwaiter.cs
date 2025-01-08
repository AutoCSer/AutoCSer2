using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// await 强制 Task.Run 操作
    /// </summary>
    public sealed class SwitchAwaiter : INotifyCompletion
    {
        /// <summary>
        /// 完成状态
        /// </summary>
        public bool IsCompleted { get { return false; } }
        /// <summary>
        /// await 强制 Task.Run 操作
        /// </summary>
        private SwitchAwaiter() { }
        /// <summary>
        /// await 支持
        /// </summary>
        /// <returns></returns>
        public async Task Wait()
        {
            await this;
        }
        /// <summary>
        /// await 支持
        /// </summary>
        /// <returns></returns>
        public void GetResult() { }
        /// <summary>
        /// await 支持
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public SwitchAwaiter GetAwaiter()
        {
            return this;
        }
        /// <summary>
        /// 设置异步回调
        /// </summary>
        /// <param name="continuation"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        void INotifyCompletion.OnCompleted(Action continuation)
        {
            Task.Run(continuation);
        }

        /// <summary>
        /// await 强制 Task.Run 操作
        /// </summary>
        public static readonly SwitchAwaiter Default = new SwitchAwaiter();
    }
}
