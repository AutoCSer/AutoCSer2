using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.DistributedLock
{
    /// <summary>
    /// 分段字典锁管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FragmentDictionaryManager<T> : FragmentDictionary256<T, DistributedLockManager<T>>, IDistributedLockManager<T>
        where T : IEquatable<T>
    {
        /// <summary>
        /// 分布式锁服务端
        /// </summary>
        private readonly DistributedLockController controller;
        /// <summary>
        /// 分段字典锁管理器
        /// </summary>
        /// <param name="controller">分布式锁服务端</param>
        public FragmentDictionaryManager(DistributedLockController controller)
        {
            this.controller = controller;
        }
        /// <summary>
        /// 创建分布式锁管理器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual DistributedLockManager<T> createManager(T key)
        {
            return new DistributedLockManager<T>(this, key);
        }
        /// <summary>
        /// 根据关键字获取锁管理器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private DistributedLockManager<T> get(T key)
        {
            DistributedLockManager<T> lockManager;
            if (!TryGetValue(key, out lockManager)) Add(key, lockManager = createManager(key));
            return lockManager;
        }
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="callback">锁请求标识，失败返回 0</param>
        public virtual void Enter(T key, int releaseSeconds, CommandServerCallback<long> callback)
        {
            try
            {
                get(key).Enter(controller, releaseSeconds, ref callback);
            }
            finally { callback?.Callback(0); }
        }
        /// <summary>
        /// 尝试请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <returns>锁请求标识，失败返回 0</returns>
        public virtual long TryEnter(T key, int releaseSeconds)
        {
            return get(key).TryEnter(controller, releaseSeconds);
        }
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="timeoutSeconds">请求超时时间</param>
        /// <param name="callback">锁请求标识，失败返回 0</param>
        public virtual void TryEnter(T key, int releaseSeconds, int timeoutSeconds, CommandServerCallback<long> callback)
        {
            try
            {
                get(key).Enter(controller, releaseSeconds, timeoutSeconds, ref callback);
            }
            finally { callback?.Callback(0); }
        }
        /// <summary>
        /// 锁请求断线重连
        /// </summary>
        /// <param name="requestID">锁请求标识</param>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <returns>锁请求标识，失败返回 0</returns>
        public virtual long EnterAgain(long requestID, T key, int releaseSeconds)
        {
            return get(key).EnterAgain(controller, requestID, releaseSeconds);
        }
        /// <summary>
        /// 保持心跳延长锁自动超时
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        /// <returns>失败表示锁已经被释放</returns>
        public virtual bool Keep(T key, long requestID)
        {
            DistributedLockManager<T> lockManager;
            return TryGetValue(key, out lockManager) && lockManager.Keep(requestID);
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        public virtual void Release(T key, long requestID)
        {
            DistributedLockManager<T> lockManager;
            if (TryGetValue(key, out lockManager)) lockManager.Release(requestID);
        }
    }
}
