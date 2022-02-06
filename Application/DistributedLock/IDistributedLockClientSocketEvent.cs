using System;

namespace AutoCSer.CommandService
{
    /// <summary>
    /// 分布式锁客户端套接字事件
    /// </summary>
    public interface IDistributedLockClientSocketEvent
    {
        /// <summary>
        /// 添加新的锁请求对象
        /// </summary>
        /// <param name="request"></param>
        void AppendRequest(IDistributedLockRequest request);
        /// <summary>
        /// 删除锁请求对象
        /// </summary>
        /// <param name="request"></param>
        void RemoveRequest(IDistributedLockRequest request);
    }
    /// <summary>
    /// 分布式锁客户端套接字事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDistributedLockClientSocketEvent<T> : IDistributedLockClientSocketEvent
        where T : IEquatable<T>
    {
        /// <summary>
        /// 分布式锁客户端接口
        /// </summary>
        IDistributedLockClient<T> DistributedLockClient { get; }
    }
}
