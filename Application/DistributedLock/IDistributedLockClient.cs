using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 分布式锁客户端接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDistributedLockClient<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="callback">锁请求标识回调</param>
        /// <returns>await</returns>
        CallbackCommand Enter(T key, int releaseSeconds, Action<CommandClientReturnValue<long>> callback);
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <returns>锁请求标识</returns>
        [CommandClientMethod(MatchMethodName = nameof(Enter))]
        ReturnCommand<long> EnterAsync(T key, int releaseSeconds);
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <returns>锁请求标识</returns>
        CommandClientReturnValue<long> Enter(T key, int releaseSeconds);

        /// <summary>
        /// 尝试请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="callback">锁请求标识回调，失败返回 0</param>
        /// <returns>await</returns>
        CallbackCommand TryEnter(T key, int releaseSeconds, Action<CommandClientReturnValue<long>> callback);
        /// <summary>
        /// 尝试请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <returns>锁请求标识，失败返回 0</returns>
        [CommandClientMethod(MatchMethodName = nameof(TryEnter))]
        ReturnCommand<long> TryEnterAsync(T key, int releaseSeconds);
        /// <summary>
        /// 尝试请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <returns>锁请求标识，失败返回 0</returns>
        CommandClientReturnValue<long> TryEnter(T key, int releaseSeconds);

        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="timeoutSeconds">请求超时时间</param>
        /// <param name="callback">锁请求标识回调，失败返回 0</param>
        /// <returns>await</returns>
        CallbackCommand TryEnter(T key, int releaseSeconds, int timeoutSeconds, Action<CommandClientReturnValue<long>> callback);
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="timeoutSeconds">请求超时时间</param>
        /// <returns>锁请求标识，失败返回 0</returns>
        [CommandClientMethod(MatchMethodName = nameof(TryEnter))]
        ReturnCommand<long> TryEnterAsync(T key, int releaseSeconds, int timeoutSeconds);
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="timeoutSeconds">请求超时时间</param>
        /// <returns>锁请求标识，失败返回 0</returns>
        CommandClientReturnValue<long> TryEnter(T key, int releaseSeconds, int timeoutSeconds);

        /// <summary>
        /// 锁请求断线重连
        /// </summary>
        /// <param name="requestID">锁请求标识</param>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="callback">锁请求标识回调，失败返回 0</param>
        /// <returns>await</returns>
        CallbackCommand EnterAgain(long requestID, T key, int releaseSeconds, Action<CommandClientReturnValue<long>> callback);

        /// <summary>
        /// 保持心跳延长锁自动超时
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        /// <param name="callback">回调委托，失败表示锁已经被释放</param>
        /// <returns>await</returns>
        CallbackCommand Keep(T key, long requestID, Action<CommandClientReturnValue<bool>> callback);
        /// <summary>
        /// 保持心跳延长锁自动超时
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        /// <returns>失败表示锁已经被释放</returns>
        [CommandClientMethod(MatchMethodName = nameof(Keep))]
        ReturnCommand<bool> KeepAsync(T key, long requestID);
        /// <summary>
        /// 保持心跳延长锁自动超时
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        /// <returns>失败表示锁已经被释放</returns>
        CommandClientReturnValue<bool> Keep(T key, long requestID);
        /// <summary>
        /// 保持心跳延长锁自动超时（仅通知服务端）
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        /// <returns>await</returns>
        SendOnlyCommand TryKeep(T key, long requestID);

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        /// <param name="callback">回调委托</param>
        /// <returns>await</returns>
        CallbackCommand Release(T key, long requestID, CommandClientCallback callback);
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        /// <returns></returns>
        [CommandClientMethod(MatchMethodName = nameof(Release))]
        ReturnCommand ReleaseAsync(T key, long requestID);
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="requestID">锁请求标识</param>
        /// <returns></returns>
        CommandClientReturnValue Release(T key, long requestID);
    }
}
