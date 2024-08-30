using AutoCSer.CommandService.DistributedLock;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 异步可重入锁客户端（非线程安全，不支持多线程并发操作同一个异步上下文）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DistributedLockAsynchronousReentrantClient<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 分布式锁客户端
        /// </summary>
        private readonly DistributedLockClient<T> client;
        /// <summary>
        /// 异步可重入锁管理器
        /// </summary>
        private readonly AsynchronousReentrantLockManager reentrantLockManager;
        /// <summary>
        /// 异步可重入锁客户端，注意调用点要在第一次申请锁之前的异步上下文，不允许向外层异步上下文传递此参数
        /// </summary>
        /// <param name="client">分布式锁客户端</param>
        public DistributedLockAsynchronousReentrantClient(DistributedLockClient<T> client)
        {
            this.client = client;
            reentrantLockManager = AsynchronousReentrantLockManager.Get();
        }
        /// <summary>
        /// 请求可重入锁（非线程安全，不支持多线程并发操作同一个异步上下文）
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="keepSeconds">心跳间隔秒数</param>
        /// <returns>异步可重入锁释放对象</returns>
        public async Task<CommandClientReturnValue<DistributedLockAsynchronousReentrant>> Enter(T key, int releaseSeconds, int keepSeconds)
        {
            ReentrantKey reentrantKey = new ReentrantKey(typeof(T), key);
            DistributedLockAsynchronousReentrant reentrantLock = reentrantLockManager.Get(reentrantKey);
            if (reentrantLock != null) return reentrantLock;
            CommandClientReturnValue<DistributedLockKeepRequest<T>> request = await client.Enter(key, releaseSeconds, keepSeconds);
            if (!request.IsSuccess) return request.ReturnValue;
            return await reentrantLockManager.CreateAsync(reentrantKey, request.Value).ConfigureAwait(false);
        }
        /// <summary>
        /// 尝试请求可重入锁（非线程安全，不支持多线程并发操作同一个异步上下文）
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="keepSeconds">心跳间隔秒数</param>
        /// <returns>异步可重入锁释放对象，失败返回 null</returns>
        public async Task<CommandClientReturnValue<DistributedLockAsynchronousReentrant>> TryEnter(T key, int releaseSeconds, int keepSeconds)
        {
            ReentrantKey reentrantKey = new ReentrantKey(typeof(T), key);
            DistributedLockAsynchronousReentrant reentrantLock = reentrantLockManager.Get(reentrantKey);
            if (reentrantLock != null) return reentrantLock;
            CommandClientReturnValue<DistributedLockKeepRequest<T>> request = await client.TryEnter(key, releaseSeconds, keepSeconds);
            if (!request.IsSuccess) return request.ReturnValue;
            return await reentrantLockManager.CreateAsync(reentrantKey, request.Value).ConfigureAwait(false);
        }
        /// <summary>
        /// 尝试请求可重入锁（非线程安全，不支持多线程并发操作同一个异步上下文）
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="timeoutSeconds">请求超时时间</param>
        /// <param name="keepSeconds">心跳间隔秒数</param>
        /// <returns>异步可重入锁释放对象，失败返回 null</returns>
        public async Task<CommandClientReturnValue<DistributedLockAsynchronousReentrant>> TryEnter(T key, int releaseSeconds, int timeoutSeconds, int keepSeconds)
        {
            ReentrantKey reentrantKey = new ReentrantKey(typeof(T), key);
            DistributedLockAsynchronousReentrant reentrantLock = reentrantLockManager.Get(reentrantKey);
            if (reentrantLock != null) return reentrantLock;
            CommandClientReturnValue<DistributedLockKeepRequest<T>> request = await client.TryEnterTimeout(key, releaseSeconds, timeoutSeconds, keepSeconds);
            if (!request.IsSuccess) return request.ReturnValue;
            return await reentrantLockManager.CreateAsync(reentrantKey, request.Value).ConfigureAwait(false);
        }
    }
}
