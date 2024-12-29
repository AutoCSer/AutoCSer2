using AutoCSer.Net;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 分布式锁节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DistributedLockNode<T> : ContextNode<IDistributedLockNode<T>, DistributedLockIdentity<T>>, IDistributedLockNode<T>, ISnapshot<DistributedLockIdentity<T>>
        where T : IEquatable<T>
    {
        /// <summary>
        /// 锁信息集合
        /// </summary>
        private readonly Dictionary<T, DistributedLock<T>> locks;
        /// <summary>
        /// 当前分配锁操作标识
        /// </summary>
        internal long Identity;
        /// <summary>
        /// 分布式锁节点
        /// </summary>
        public DistributedLockNode()
        {
            locks = DictionaryCreator<T>.Create<DistributedLock<T>>();
            Identity = 1;
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return locks.Count + 1;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        public SnapshotResult<DistributedLockIdentity<T>> GetSnapshotResult(DistributedLockIdentity<T>[] snapshotArray, object customObject)
        {
            SnapshotResult<DistributedLockIdentity<T>> result = new SnapshotResult<DistributedLockIdentity<T>>(locks.Count + 1, snapshotArray.Length);
            snapshotArray[0].Set(Identity);
            result.Count = 1;
            foreach (DistributedLock<T> value in locks.Values) result.Add(snapshotArray, value.Identity);
            return result;
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSet(DistributedLockIdentity<T> value)
        {
            if (value.Timeout > DateTime.UtcNow) locks.Add(value.Key, new DistributedLock<T>(this, ref value));
            else if (value.Timeout == default(DateTime)) Identity = value.Identity;
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
            if (locks.TryGetValue(key, out removeLock) && object.ReferenceEquals(distributedLock, removeLock)) locks.Remove(key);
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="timeoutSeconds">超时秒数</param>
        /// <param name="callback">失败返回 0</param>
        public void Enter(T key, ushort timeoutSeconds, MethodCallback<long> callback)
        {
            if (key != null && timeoutSeconds != 0)
            {
                var distributedLock = default(DistributedLock<T>);
                if (!locks.TryGetValue(key, out distributedLock))
                {
                    locks.Add(key, distributedLock = new DistributedLock<T>(this, key, timeoutSeconds));
                    callback.SynchronousCallback(distributedLock.Identity.Identity);
                }
                else
                {
                    callback.Reserve = timeoutSeconds;
                    distributedLock.Enter(callback);
                }
            }
            else callback.SynchronousCallback(0);
        }
        /// <summary>
        /// 尝试申请锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="timeoutSeconds">超时秒数</param>
        /// <returns>失败返回 0</returns>
        public long TryEnter(T key, ushort timeoutSeconds)
        {
            if (key != null && timeoutSeconds != 0)
            {
                var distributedLock = default(DistributedLock<T>);
                if (!locks.TryGetValue(key, out distributedLock))
                {
                    locks.Add(key, distributedLock = new DistributedLock<T>(this, key, timeoutSeconds));
                    return distributedLock.Identity.Identity;
                }
            }
            return 0;
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="identity">锁操作标识</param>
        public void Release(T key, long identity)
        {
            if (key != null)
            {
                var distributedLock = default(DistributedLock<T>);
                if (locks.TryGetValue(key, out distributedLock)) distributedLock.Release(identity);
            }
        }
    }
}
