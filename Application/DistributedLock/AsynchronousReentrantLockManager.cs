using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DistributedLock
{
    /// <summary>
    /// 异步可重入锁管理器（非线程安全，不支持多线程并发操作同一个异步上下文）
    /// </summary>
    internal class AsynchronousReentrantLockManager
    {
        /// <summary>
        /// 异步可重入锁管理器
        /// </summary>
        private static readonly AsyncLocal<AsynchronousReentrantLockManager> manager = new AsyncLocal<AsynchronousReentrantLockManager>();
        /// <summary>
        /// 获取异步可重入锁管理器，第一次（最外层）调用该方法的调用点为当前异步可重入锁的异步上下文，对于上层异步调用无效
        /// </summary>
        /// <returns></returns>
        internal static AsynchronousReentrantLockManager Get()
        {
            AsynchronousReentrantLockManager lockManager = manager.Value;
            if (lockManager == null) manager.Value = lockManager = new AsynchronousReentrantLockManager();
            return lockManager;
        }

        /// <summary>
        /// 异步可重入锁计数集合
        /// </summary>
        internal LeftArray<AsynchronousReentrantLockCount> Locks = new LeftArray<AsynchronousReentrantLockCount>(0);
        /// <summary>
        /// 获取异步可重入锁计数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal DistributedLockAsynchronousReentrant Get(ReentrantKey key)
        {
            int index = 0;
            foreach (AsynchronousReentrantLockCount count in Locks)
            {
                DistributedLockAsynchronousReentrant reentrantLock = count.Enter(ref key, out bool isKey);
                if (reentrantLock != null) return reentrantLock;
                if (isKey)
                {
                    Locks.RemoveAt(index);
                    return null;
                }
                ++index;
            }
            return null;
        }
        /// <summary>
        /// 创建可重入锁对象，用于该接口在 Standard 2.0 版本下存在同步调用，调用方请调用 ConfigureAwait(false)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        internal async Task<DistributedLockAsynchronousReentrant> CreateAsync<T>(ReentrantKey key, DistributedLockKeepRequest<T> request)
            where T : IEquatable<T>
        {
            bool isLock = false;
            try
            {
                AsynchronousReentrantLockCount<T> reentrantLockCount = new AsynchronousReentrantLockCount<T>(this, key, request);
                Locks.Add(reentrantLockCount);
                DistributedLockAsynchronousReentrant reentrantLock = reentrantLockCount.Enter();
                if (reentrantLock != null)
                {
                    isLock = true;
                    return reentrantLock;
                }
                Locks.Remove(reentrantLockCount);
                return null;
            }
            finally
            {
#if NetStandard2
                if (!isLock) request.Dispose();
#else
                if (!isLock) await request.DisposeAsync();
#endif
            }
        }
    }
}
