using AutoCSer.CommandService.DistributedLock;
using AutoCSer.Net;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 分布式锁客户端
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DistributedLockClient<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 分布式锁客户端套接字事件
        /// </summary>
        private readonly IDistributedLockClientSocketEvent<T> client;
        /// <summary>
        /// 分布式锁客户端
        /// </summary>
        /// <param name="client">分布式锁客户端套接字事件</param>
        public DistributedLockClient(IDistributedLockClientSocketEvent<T> client)
        {
            this.client = client;
        }

        /// <summary>
        /// 创建锁请求保持心跳对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="requestID"></param>
        /// <param name="releaseSeconds"></param>
        /// <param name="keepSeconds"></param>
        /// <returns></returns>
        private async Task<DistributedLockKeepRequest<T>> create(T key, long requestID, int releaseSeconds, int keepSeconds)
        {
            bool isRequest = false;
            try
            {
                DistributedLockKeepRequest<T> request = new DistributedLockKeepRequest<T>(client, key, requestID, releaseSeconds, keepSeconds);
                await request.StartKeep();
                isRequest = true;
                return request;
            }
            finally
            {
                if (!isRequest) await client.DistributedLockClient.Release(key, requestID);
            }
        }
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="keepSeconds">心跳间隔秒数</param>
        /// <returns>锁请求保持心跳对象</returns>
        public async Task<CommandClientReturnValue<DistributedLockKeepRequest<T>>> Enter(T key, int releaseSeconds, int keepSeconds)
        {
            CommandClientReturnValue<long> requestID = await client.DistributedLockClient.Enter(key, releaseSeconds);
            if (!requestID.IsSuccess) return requestID.ReturnValue;
            return await create(key, requestID.Value, releaseSeconds, keepSeconds);
        }
        /// <summary>
        /// 锁请求对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="requestID"></param>
        /// <param name="releaseSeconds"></param>
        /// <returns></returns>
        private async Task<DistributedLockRequest<T>> create(T key, long requestID, int releaseSeconds)
        {
            bool isRequest = false;
            try
            {
                DistributedLockRequest<T> request = new DistributedLockRequest<T>(client, key, requestID, releaseSeconds);
                isRequest = true;
                return request;
            }
            finally
            {
                if (!isRequest) await client.DistributedLockClient.Release(key, requestID);
            }
        }
        /// <summary>
        /// 请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <returns>锁请求对象</returns>
        public async Task<CommandClientReturnValue<DistributedLockRequest<T>>> Enter(T key, int releaseSeconds)
        {
            CommandClientReturnValue<long> requestID = await client.DistributedLockClient.Enter(key, releaseSeconds);
            if (!requestID.IsSuccess) return requestID.ReturnValue;
            return await create(key, requestID.Value, releaseSeconds);
        }

        /// <summary>
        /// 尝试请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="keepSeconds">心跳间隔秒数</param>
        /// <returns>锁请求保持心跳对象，失败返回 null</returns>
        public async Task<CommandClientReturnValue<DistributedLockKeepRequest<T>>> TryEnter(T key, int releaseSeconds, int keepSeconds)
        {
            CommandClientReturnValue<long> requestID = await client.DistributedLockClient.TryEnter(key, releaseSeconds);
            if (!requestID.IsSuccess) return requestID.ReturnValue;
            if (requestID.Value != 0) return await create(key, requestID.Value, releaseSeconds, keepSeconds);
            return null;
        }
        /// <summary>
        /// 尝试请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <returns>锁请求对象，失败返回 null</returns>
        public async Task<CommandClientReturnValue<DistributedLockRequest<T>>> TryEnter(T key, int releaseSeconds)
        {
            CommandClientReturnValue<long> requestID = await client.DistributedLockClient.TryEnter(key, releaseSeconds);
            if (!requestID.IsSuccess) return requestID.ReturnValue;
            if (requestID.Value != 0) return await create(key, requestID.Value, releaseSeconds);
            return null;
        }

        /// <summary>
        /// 尝试请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="timeoutSeconds">请求超时时间</param>
        /// <param name="keepSeconds">心跳间隔秒数</param>
        /// <returns>锁请求保持心跳对象，失败返回 null</returns>
        public async Task<CommandClientReturnValue<DistributedLockKeepRequest<T>>> TryEnterTimeout(T key, int releaseSeconds, int timeoutSeconds, int keepSeconds)
        {
            CommandClientReturnValue<long> requestID = await client.DistributedLockClient.TryEnter(key, releaseSeconds, timeoutSeconds);
            if (!requestID.IsSuccess) return requestID.ReturnValue;
            if (requestID.Value != 0) return await create(key, requestID.Value, releaseSeconds, keepSeconds);
            return null;
        }
        /// <summary>
        /// 尝试请求锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="releaseSeconds">自动释放锁超时秒数，用于客户端掉线没有释放锁的情况</param>
        /// <param name="timeoutSeconds">请求超时时间</param>
        /// <returns>锁请求对象，失败返回 null</returns>
        public async Task<CommandClientReturnValue<DistributedLockRequest<T>>> TryEnterTimeout(T key, int releaseSeconds, int timeoutSeconds)
        {
            CommandClientReturnValue<long> requestID = await client.DistributedLockClient.TryEnter(key, releaseSeconds, timeoutSeconds);
            if (!requestID.IsSuccess) return requestID.ReturnValue;
            if (requestID.Value != 0) return await create(key, requestID.Value, releaseSeconds);
            return null;
        }

#if !DotNet45
        /// <summary>
        /// 获取异步可重入锁客户端，注意调用点要在第一次申请锁之前的异步上下文，不允许向外层异步上下文传递此参数
        /// </summary>
        /// <returns></returns>
        public DistributedLockAsynchronousReentrantClient<T> GetAsynchronousReentrant()
        {
            return new DistributedLockAsynchronousReentrantClient<T>(this);
        }
#endif
    }
}
