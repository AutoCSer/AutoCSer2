using AutoCSer.CommandService.DistributedLock;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 分布式锁服务端
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DistributedLock<T> : DistributedLockController, IDistributedLock<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 分布式锁管理器
        /// </summary>
        protected readonly IDistributedLockManager<T> manager;
        /// <summary>
        /// 分布式锁服务端
        /// </summary>
        /// <param name="identityGenerator">请求标识生成器</param>
        /// <param name="manager">分布式锁管理器</param>
        public DistributedLock(DistributedMillisecondIdentityGenerator identityGenerator = null, IDistributedLockManager<T> manager = null) : base(identityGenerator)
        {
            this.manager = manager ?? new FragmentDictionaryManager<T>(this);
        }
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="callback">锁请求标识</param>
        public virtual void Enter(CommandServerSocket socket, CommandServerCallQueue queue, T key, int releaseSeconds, CommandServerCallback<long> callback)
        {
            manager.Enter(key, releaseSeconds, callback);
        }
        /// <summary>
        /// 尝试请求锁
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <returns>锁请求标识，失败返回 0</returns>
        public virtual long TryEnter(CommandServerSocket socket, CommandServerCallQueue queue, T key, int releaseSeconds)
        {
            return manager.TryEnter(key, releaseSeconds);
        }
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="timeoutSeconds">请求超时时间</param>
        /// <param name="callback">锁请求标识，失败返回 0</param>
        public virtual void TryEnter(CommandServerSocket socket, CommandServerCallQueue queue, T key, int releaseSeconds, int timeoutSeconds, CommandServerCallback<long> callback)
        {
            manager.TryEnter(key, releaseSeconds, timeoutSeconds, callback);
        }
        /// <summary>
        /// 锁请求断线重连
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="requestID">锁请求标识</param>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <returns>锁请求标识，失败返回 0</returns>
        public virtual long EnterAgain(CommandServerSocket socket, CommandServerCallQueue queue, long requestID, T key, int releaseSeconds)
        {
            return manager.EnterAgain(requestID, key, releaseSeconds);
        }
        /// <summary>
        /// 保持心跳延长锁自动超时
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        /// <returns>失败表示锁已经被释放</returns>
        public virtual bool Keep(CommandServerSocket socket, CommandServerCallQueue queue, T key, long requestID)
        {
            return manager.Keep(key, requestID);
        }
        /// <summary>
        /// 保持心跳延长锁自动超时
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        /// <returns></returns>
        CommandServerSendOnly IDistributedLock<T>.TryKeep(CommandServerSocket socket, CommandServerCallQueue queue, T key, long requestID)
        {
            Keep(socket, queue, key, requestID);
            return null;
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="queue"></param>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        public virtual void Release(CommandServerSocket socket, CommandServerCallQueue queue, T key, long requestID)
        {
            manager.Release(key, requestID);
        }
    }
}
