using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.Threading
{
    /// <summary>
    /// 锁缓存（用于重用）
    /// </summary>
    internal sealed class SemaphoreSlimCache : Link<SemaphoreSlimCache>
    {
        /// <summary>
        /// 数据加载访问锁
        /// </summary>
        internal readonly SemaphoreSlim Lock;
        /// <summary>
        /// 当前申请数量
        /// </summary>
        internal int Count;
        /// <summary>
        /// 节点缓存加载访问锁
        /// </summary>
        internal SemaphoreSlimCache()
        {
            Lock = new SemaphoreSlim(1, 1);
        }

        /// <summary>
        /// 锁缓存
        /// </summary>
        private static LinkStack<SemaphoreSlimCache> cache;
        /// <summary>
        /// 获取锁缓存
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static SemaphoreSlimCache Get()
        {
            return cache.Pop() ?? new SemaphoreSlimCache();
        }
        /// <summary>
        /// 释放锁缓存
        /// </summary>
        /// <param name="semaphoreSlim"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Free(SemaphoreSlimCache semaphoreSlim)
        {
            cache.Push(semaphoreSlim);
        }
    }
}
