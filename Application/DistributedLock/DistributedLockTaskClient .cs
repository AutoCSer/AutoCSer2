using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 分布式锁客户端接口转换封装
    /// </summary>
    /// <typeparam name="T">锁关键字类型</typeparam>
    public sealed class DistributedLockTaskClient<T> : IDistributedLockClient<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 分布式锁客户端接口
        /// </summary>
        private readonly IDistributedLockTaskClient<T> client;
        /// <summary>
        /// 分布式锁客户端接口转换封装
        /// </summary>
        /// <param name="client">分布式锁客户端接口</param>
        public DistributedLockTaskClient(IDistributedLockTaskClient<T> client)
        {
            this.client = client;
        }
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="callback">锁请求标识回调</param>
        /// <returns>await</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CallbackCommand Enter(T key, int releaseSeconds, Action<CommandClientReturnValue<long>> callback)
        {
            return client.Enter(key, releaseSeconds, callback);
        }
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <returns>锁请求标识</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<long> Enter(T key, int releaseSeconds)
        {
            return client.Enter(key, releaseSeconds);
        }

        /// <summary>
        /// 尝试请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="callback">锁请求标识回调，失败返回 0</param>
        /// <returns>await</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CallbackCommand TryEnter(T key, int releaseSeconds, Action<CommandClientReturnValue<long>> callback)
        {
            return client.TryEnter(key, releaseSeconds, callback);
        }
        /// <summary>
        /// 尝试请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <returns>锁请求标识，失败返回 0</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<long> TryEnter(T key, int releaseSeconds)
        {
            return client.TryEnter(key, releaseSeconds);
        }

        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="timeoutSeconds">请求超时时间</param>
        /// <param name="callback">锁请求标识回调，失败返回 0</param>
        /// <returns>await</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CallbackCommand TryEnter(T key, int releaseSeconds, int timeoutSeconds, Action<CommandClientReturnValue<long>> callback)
        {
            return client.TryEnter(key, releaseSeconds, timeoutSeconds, callback);
        }
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="timeoutSeconds">请求超时时间</param>
        /// <returns>锁请求标识，失败返回 0</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<long> TryEnter(T key, int releaseSeconds, int timeoutSeconds)
        {
            return client.TryEnter(key, releaseSeconds, timeoutSeconds);
        }

        /// <summary>
        /// 锁请求断线重连
        /// </summary>
        /// <param name="requestID">锁请求标识</param>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="callback">锁请求标识回调，失败返回 0</param>
        /// <returns>await</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CallbackCommand EnterAgain(long requestID, T key, int releaseSeconds, Action<CommandClientReturnValue<long>> callback)
        {
            return client.EnterAgain(requestID, key, releaseSeconds, callback);
        }

        /// <summary>
        /// 保持心跳延长锁自动超时
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        /// <param name="callback">回调委托，失败表示锁已经被释放</param>
        /// <returns>await</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CallbackCommand Keep(T key, long requestID, Action<CommandClientReturnValue<bool>> callback)
        {
            return client.Keep(key, requestID, callback);
        }
        /// <summary>
        /// 保持心跳延长锁自动超时
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        /// <returns>失败表示锁已经被释放</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand<bool> Keep(T key, long requestID)
        {
            return client.Keep(key, requestID);
        }
        /// <summary>
        /// 保持心跳延长锁自动超时（仅通知服务端）
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        /// <returns>await</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public SendOnlyCommand TryKeep(T key, long requestID)
        {
            return client.TryKeep(key, requestID);
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        /// <param name="callback">回调委托</param>
        /// <returns>await</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CallbackCommand Release(T key, long requestID, Action<CommandClientReturnValue> callback)
        {
            return client.Release(key, requestID, callback);
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ReturnCommand Release(T key, long requestID)
        {
            return client.Release(key, requestID);
        }
    }
}
