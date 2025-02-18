using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 分布式锁
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class DistributedLock<T>
        where T : IEquatable<T>
    {
        /// <summary>
        /// 分布式锁节点
        /// </summary>
        internal readonly DistributedLockNode<T> Node;
        /// <summary>
        /// 分布式锁标识信息
        /// </summary>
        internal DistributedLockIdentity<T> Identity;
        /// <summary>
        /// 锁等待队列头节点
        /// </summary>
#if NetStandard21
        private MethodCallback<long>? callbackHead;
#else
        private MethodCallback<long> callbackHead;
#endif
        /// <summary>
        /// 锁等待队列尾节点
        /// </summary>
#if NetStandard21
        private MethodCallback<long>? callbackEnd;
#else
        private MethodCallback<long> callbackEnd;
#endif
        /// <summary>
        /// 当前超时
        /// </summary>
#if NetStandard21
        internal DistributedLockTimeout<T>? LockTimeout;
#else
        internal DistributedLockTimeout<T> LockTimeout;
#endif
        /// <summary>
        /// 分布式锁
        /// </summary>
        /// <param name="node">分布式锁节点</param>
        /// <param name="identity">分布式锁标识信息</param>
        internal DistributedLock(DistributedLockNode<T> node, ref DistributedLockIdentity<T> identity)
        {
            Node = node;
            this.Identity = identity;
        }
        /// <summary>
        /// 分布式锁
        /// </summary>
        /// <param name="node">分布式锁节点</param>
        /// <param name="key">锁关键字</param>
        /// <param name="timeoutSeconds">超时秒数</param>
        internal DistributedLock(DistributedLockNode<T> node, T key, ushort timeoutSeconds)
        {
            Node = node;
            Identity.Set(key, AutoCSer.Threading.SecondTimer.UtcNow.AddSeconds(timeoutSeconds), node.Identity++);
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <param name="callback"></param>
        internal void Enter(MethodCallback<long> callback)
        {
            if (callbackHead == null)
            {
                callbackHead = callbackEnd = callback;
                checkTimeout();
            }
            else
            {
                callbackEnd.notNull().LinkNext = callback;
                callbackEnd = callback;
            }
        }
        /// <summary>
        /// 超时检查
        /// </summary>
        private void checkTimeout()
        {
            double second = (int)(Identity.Timeout - AutoCSer.Threading.SecondTimer.UtcNow).TotalSeconds;
            if (second > 0)
            {
                LockTimeout = new DistributedLockTimeout<T>(this, (long)second + 1);
                LockTimeout.AppendTaskArray();
            }
            else if (!next()) Node.Remove(this);
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="identity"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Release(long identity)
        {
            if (Identity.Identity == identity)
            {
                LockTimeout = null;
                if (callbackHead == null || !next()) Node.Remove(Identity.Key);
            }
        }
        /// <summary>
        /// 处理下一个等待节点
        /// </summary>
        /// <returns></returns>
        private bool next()
        {
            do
            {
                Identity.Set(AutoCSer.Threading.SecondTimer.UtcNow.AddSeconds(callbackEnd.notNull().Reserve16), Node.Identity++);
                var head = callbackHead.notNull();
                if (head.SynchronousCallback(Identity.Identity))
                {
                    callbackHead = head.LinkNext;
                    if (callbackHead != null) checkTimeout();
                    return true;
                }
            }
            while ((callbackHead = callbackHead.notNull().LinkNext) != null);
            return false;
        }
        /// <summary>
        /// 超时检查
        /// </summary>
        /// <param name="timeout"></param>
        internal void Timeout(DistributedLockTimeout<T> timeout)
        {
            if (object.ReferenceEquals(timeout, LockTimeout) && !next()) Node.Remove(this);
        }
    }
}
