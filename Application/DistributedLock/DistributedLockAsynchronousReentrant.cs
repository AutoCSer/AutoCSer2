using AutoCSer.CommandService.DistributedLock;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 异步可重入锁释放对象（非线程安全，不支持多线程并发操作同一个异步上下文）
    /// </summary>
    public sealed class DistributedLockAsynchronousReentrant : IDisposable
#if !NetStandard2
        , IAsyncDisposable
#endif
    {
        /// <summary>
        /// 异步可重入锁计数
        /// </summary>
        private readonly AsynchronousReentrantLockCount reentrantLockCount;
        /// <summary>
        /// 是否释放锁
        /// </summary>
        private bool isRelease;
        /// <summary>
        /// 异步可重入锁释放对象
        /// </summary>
        /// <param name="reentrantLockCount"></param>
        internal DistributedLockAsynchronousReentrant(AsynchronousReentrantLockCount reentrantLockCount)
        {
            this.reentrantLockCount = reentrantLockCount;
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        public void Dispose()
        {
            if (!isRelease)
            {
                isRelease = true;
                reentrantLockCount.Release();
            }
        }
#if !NetStandard2
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            if (!isRelease)
            {
                isRelease = true;
                await reentrantLockCount.ReleaseAsync();
            }
        }
#endif
    }
}
