using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 线程池回调委托
    /// </summary>
    internal sealed class ThreadPoolQueueUserWorkItem
    {
        /// <summary>
        /// 线程池回调委托
        /// </summary>
        private readonly Action callback;
        /// <summary>
        /// 线程池回调委托
        /// </summary>
        /// <param name="callback"></param>
        internal ThreadPoolQueueUserWorkItem(Action callback)
        {
            this.callback = callback;
        }
        /// <summary>
        /// 线程池回调委托
        /// </summary>
        /// <param name="state"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void Callback(object? state)
#else
        internal void Callback(object state)
#endif
        {
            callback();
        }
    }

}
