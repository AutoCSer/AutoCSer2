using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 计数等待
    /// </summary>
    internal sealed class AutoWaitCount : IDisposable
    {
        /// <summary>
        /// Current count
        /// 当前计数
        /// </summary>
        private int count;
        /// <summary>
        /// 等待事件
        /// </summary>
        private readonly System.Threading.AutoResetEvent waitHandle;
        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose()
        {
            waitHandle.setDispose();
        }
        /// <summary>
        /// 计数等待
        /// </summary>
        /// <param name="count">当前计数</param>
        public AutoWaitCount(int count)
        {
            waitHandle = new System.Threading.AutoResetEvent(false);
            this.count = count + 1;
        }
        /// <summary>
        /// 减少计数
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Free()
        {
            if (System.Threading.Interlocked.Decrement(ref count) == 0) waitHandle.Set();
        }
        /// <summary>
        /// 等待计数完成并重置计数
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void WaitSet(int count)
        {
            if (System.Threading.Interlocked.Decrement(ref this.count) != 0) waitHandle.WaitOne();
            this.count = count + 1;
        }
    }
}
