using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 分布式锁超时
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class DistributedLockTimeout<T> : SecondTimerArrayNode
        where T : IEquatable<T>
    {
        /// <summary>
        /// 分布式锁
        /// </summary>
        internal readonly DistributedLock<T> DistributedLock;
        /// <summary>
        /// 分布式锁超时
        /// </summary>
        /// <param name="distributedLock">分布式锁</param>
        /// <param name="timeoutSeconds">超时时间</param>
        internal DistributedLockTimeout(DistributedLock<T> distributedLock, long timeoutSeconds) : base(SecondTimer.InternalTaskArray, timeoutSeconds)
        {
            DistributedLock = distributedLock;
            AppendTaskArray();
        }
        /// <summary>
        /// Trigger the timed operation
        /// 触发定时操作
        /// </summary>
        protected internal override void OnTimer()
        {
            if (object.ReferenceEquals(this, DistributedLock.LockTimeout)) DistributedLock.Node.StreamPersistenceMemoryDatabaseCallQueue.AppendWriteOnly(new DistributedLockTimeoutNode<T>(this));
        }
    }
}
