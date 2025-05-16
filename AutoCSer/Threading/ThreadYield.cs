using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 线程操作
    /// </summary>
    public static class ThreadYield
    {
        /// <summary>
        /// .NET 4.0 之前的版本不做任何事
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void YieldOnly()
        {
            System.Threading.Thread.Yield();
        }
        /// <summary>
        /// .NET 4.0 之前的版本调用 System.Threading.Thread.Sleep(0)
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Yield()
        {
            System.Threading.Thread.Yield();
        }
    }
}
