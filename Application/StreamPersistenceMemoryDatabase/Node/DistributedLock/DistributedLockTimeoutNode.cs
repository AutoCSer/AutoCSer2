﻿using AutoCSer.Net.CommandServer;
using AutoCSer.Threading;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 分布式锁超时检查队列节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class DistributedLockTimeoutNode<T> : ReadWriteQueueNode
        where T : IEquatable<T>
    {
        /// <summary>
        /// 分布式锁超时
        /// </summary>
        private readonly DistributedLockTimeout<T> timeout;
        /// <summary>
        /// 分布式锁超时检查队列节点
        /// </summary>
        /// <param name="timeout">分布式锁超时</param>
        internal DistributedLockTimeoutNode(DistributedLockTimeout<T> timeout)
        {
            this.timeout = timeout;
        }
        /// <summary>
        /// Timeout check
        /// 超时检查
        /// </summary>
        public override void RunTask()
        {
            timeout.DistributedLock.Timeout(timeout);
        }
    }
}
