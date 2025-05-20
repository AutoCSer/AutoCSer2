using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 事件扩展
    /// </summary>
    internal static class EventWaitHandleExtension
    {
        /// <summary>
        /// 调用 Set
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void setDispose(this EventWaitHandle value)
        {
            value?.Set();
            //if (value != null && !value.GetSafeWaitHandle().IsClosed)
            //{
            //    value.Set();
            //    value.Dispose();
            //}
        }
    }
}
