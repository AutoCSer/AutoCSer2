using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 分布式锁管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDistributedLockManager<T>
    {
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是否存在关键字</returns>
        bool Remove(T key);
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="callback">锁请求标识</param>
        void Enter(T key, int releaseSeconds, CommandServerCallback<long> callback);
        /// <summary>
        /// 尝试请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <returns>锁请求标识，失败返回 0</returns>
        long TryEnter(T key, int releaseSeconds);
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="timeoutSeconds">请求超时时间</param>
        /// <param name="callback">锁请求标识，失败返回 0</param>
        void TryEnter(T key, int releaseSeconds, int timeoutSeconds, CommandServerCallback<long> callback);
        /// <summary>
        /// 锁请求断线重连
        /// </summary>
        /// <param name="requestID">锁请求标识</param>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <returns>锁请求标识，失败返回 0</returns>
        long EnterAgain(long requestID, T key, int releaseSeconds);
        /// <summary>
        /// 保持心跳延长锁自动超时
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        /// <returns>失败表示锁已经被释放</returns>
        bool Keep(T key, long requestID);
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        void Release(T key, long requestID);
    }
}
