using AutoCSer.Extensions;
using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Distributed lock node
    /// 分布式锁节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DistributedLockNode<T> : ContextNode<IDistributedLockNode<T>>, IDistributedLockNode<T>, IEnumerableSnapshot<DistributedLockIdentity<T>>, IEnumerableSnapshot<long>
        where T : IEquatable<T>
    {
        /// <summary>
        /// 锁信息集合
        /// </summary>
        private readonly SnapshotDictionary<T, DistributedLock<T>> locks;
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<DistributedLockIdentity<T>> IEnumerableSnapshot<DistributedLockIdentity<T>>.SnapshotEnumerable { get { return locks.Nodes.Cast<DistributedLock<T>, DistributedLockIdentity<T>>(p => p.Identity); } }
        /// <summary>
        /// 当前分配锁操作标识
        /// </summary>
        internal long Identity;
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<long> IEnumerableSnapshot<long>.SnapshotEnumerable { get { return new SnapshotGetValue<long>(getIdentity); } }
        /// <summary>
        /// Distributed lock node
        /// 分布式锁节点
        /// </summary>
        public DistributedLockNode()
        {
            locks = new SnapshotDictionary<T, DistributedLock<T>>();
            Identity = 1;
        }
        /// <summary>
        /// 获取当前分配锁操作标识
        /// </summary>
        /// <returns></returns>
        private long getIdentity()
        {
            return Identity;
        }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        public void SnapshotSet(DistributedLockIdentity<T> value)
        {
            if (value.Timeout > AutoCSer.Threading.SecondTimer.UtcNow) locks.TryAdd(value.Key, new DistributedLock<T>(this, ref value));
        }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        public void SnapshotSetIdentity(long value)
        {
            Identity = value;
        }
        /// <summary>
        /// 移除锁信息
        /// </summary>
        /// <param name="key"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Remove(T key)
        {
            locks.Remove(key);
        }
        /// <summary>
        /// 移除锁信息
        /// </summary>
        /// <param name="distributedLock"></param>
        internal void Remove(DistributedLock<T> distributedLock)
        {
            T key = distributedLock.Identity.Key;
            var removeLock = default(DistributedLock<T>);
            if (locks.TryGetValue(key, (uint)key.GetHashCode(), out removeLock) && object.ReferenceEquals(distributedLock, removeLock)) locks.Remove(key);
        }
        /// <summary>
        /// Apply for a lock
        /// 申请锁
        /// </summary>
        /// <param name="key">Keyword of lock
        /// 锁关键字</param>
        /// <param name="timeoutSeconds">Timeout seconds
        /// 超时秒数</param>
        /// <param name="callback">Lock request identity. Return 0 if failed
        /// 锁请求标识，失败返回 0</param>
        public void Enter(T key, ushort timeoutSeconds, MethodCallback<long> callback)
        {
            if (key != null && timeoutSeconds != 0)
            {
                var distributedLock = default(DistributedLock<T>);
                uint hashCode = (uint)key.GetHashCode();
                if (!locks.TryGetValue(key, hashCode, out distributedLock))
                {
                    locks.Add(hashCode, new BinarySerializeKeyValue<T, DistributedLock<T>>(key, distributedLock = new DistributedLock<T>(this, key, timeoutSeconds)));
                    StreamPersistenceMemoryDatabaseNode.IsPersistenceCallbackChanged = true;
                    callback.SynchronousCallback(distributedLock.Identity.Identity);
                }
                else
                {
                    StreamPersistenceMemoryDatabaseNode.IsPersistenceCallbackChanged = true;
                    callback.Reserve = timeoutSeconds;
                    distributedLock.Enter(callback);
                }
            }
            else callback.SynchronousCallback((long)0);
        }
        /// <summary>
        /// Try to apply for a lock
        /// 尝试申请锁
        /// </summary>
        /// <param name="key">Keyword of lock
        /// 锁关键字</param>
        /// <param name="timeoutSeconds">Timeout seconds
        /// 超时秒数</param>
        /// <returns>Lock request identity. Return 0 if failed
        /// 锁请求标识，失败返回 0</returns>
        public long TryEnter(T key, ushort timeoutSeconds)
        {
            if (key != null && timeoutSeconds != 0)
            {
                var distributedLock = default(DistributedLock<T>);
                uint hashCode = (uint)key.GetHashCode();
                if (!locks.TryGetValue(key, hashCode, out distributedLock))
                {
                    locks.Add(hashCode, new BinarySerializeKeyValue<T, DistributedLock<T>>(key, distributedLock = new DistributedLock<T>(this, key, timeoutSeconds)));
                    return distributedLock.Identity.Identity;
                }
            }
            return 0;
        }
        /// <summary>
        /// Release the lock
        /// 释放锁
        /// </summary>
        /// <param name="key">Keyword of lock
        /// 锁关键字</param>
        /// <param name="identity">Lock request identity
        /// 锁请求标识</param>
        public void Release(T key, long identity)
        {
            if (key != null)
            {
                var distributedLock = default(DistributedLock<T>);
                if (locks.TryGetValue(key, (uint)key.GetHashCode(), out distributedLock)) distributedLock.Release(identity);
            }
        }
    }
}
