using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.DistributedLock
{
    /// <summary>
    /// 异步可重入锁计数
    /// </summary>
    internal abstract class AsynchronousReentrantLockCount
    {
        /// <summary>
        /// 分布式锁管理器
        /// </summary>
        private readonly AsynchronousReentrantLockManager reentrantLockManager;
        /// <summary>
        /// 可重入锁关键字
        /// </summary>
        private readonly ReentrantKey key;
        /// <summary>
        /// 锁客户端状态是否有效
        /// </summary>
        protected abstract bool isLock { get; }
        /// <summary>
        /// 异步可重入锁计数
        /// </summary>
        /// <param name="reentrantLockManager">分布式锁管理器</param>
        /// <param name="key">可重入锁关键字</param>
        internal AsynchronousReentrantLockCount(AsynchronousReentrantLockManager reentrantLockManager, ReentrantKey key)
        {
            this.reentrantLockManager = reentrantLockManager;
            this.key = key;
        }
        /// <summary>
        /// 锁重入计数
        /// </summary>
        private int count;
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isKey"></param>
        /// <returns></returns>
        internal DistributedLockAsynchronousReentrant Enter(ref ReentrantKey key, out bool isKey)
        {
            return (isKey = key.Equals(ref key)) ? Enter() : null;
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <returns></returns>
        internal DistributedLockAsynchronousReentrant Enter()
        {
            return isLock ? enter() : null;
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <returns></returns>
        private DistributedLockAsynchronousReentrant enter()
        {
            DistributedLockAsynchronousReentrant reentrantLock = new DistributedLockAsynchronousReentrant(this);
            ++count;
            return reentrantLock;
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        internal void Release()
        {
            if (--count == 0)
            {
                reentrantLockManager.Locks.Remove(this);
                release();
            }
        }
        /// <summary>
        /// 释放锁对象
        /// </summary>
        protected abstract void release();
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <returns></returns>
        internal async Task ReleaseAsync()
        {
            if (--count == 0)
            {
                reentrantLockManager.Locks.Remove(this);
                await releaseAsync().ConfigureAwait(false);
            }
        }
        /// <summary>
        /// 释放锁请求保持心跳对象，用于该接口在 Standard 2.0 版本下存在同步调用，调用方请调用 ConfigureAwait(false)
        /// </summary>
        /// <returns></returns>
        protected abstract Task releaseAsync();
    }
    /// <summary>
    /// 异步可重入锁计数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class AsynchronousReentrantLockCount<T> : AsynchronousReentrantLockCount
        where T : IEquatable<T>
    {
        /// <summary>
        /// 锁请求保持心跳对象
        /// </summary>
        private readonly DistributedLockKeepRequest<T> request;
        /// <summary>
        /// 锁客户端状态是否有效
        /// </summary>
        protected override bool isLock { get { return request.IsLock; } }
        /// <summary>
        /// 异步可重入锁计数
        /// </summary>
        /// <param name="reentrantLockManager">分布式锁管理器</param>
        /// <param name="key">可重入锁关键字</param>
        /// <param name="request">锁请求保持心跳对象</param>
        internal AsynchronousReentrantLockCount(AsynchronousReentrantLockManager reentrantLockManager, ReentrantKey key, DistributedLockKeepRequest<T> request)
            : base(reentrantLockManager, key)
        {
            this.request = request;
        }
        /// <summary>
        /// 释放锁对象
        /// </summary>
        protected override void release()
        {
            request.Dispose();
        }
        /// <summary>
        /// 释放锁对象
        /// </summary>
        /// <returns></returns>
        protected override async Task releaseAsync()
        {
#if NetStandard2
            request.Dispose();
#else
            await request.DisposeAsync();
#endif
        }
    }
}
