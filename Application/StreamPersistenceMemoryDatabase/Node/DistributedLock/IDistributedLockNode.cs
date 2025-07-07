using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Distributed lock node interface
    /// 分布式锁节点接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ServerNode(IsLocalClient = true)]
    public partial interface IDistributedLockNode<T>
    {
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(DistributedLockIdentity<T> value);
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 2)]
        void SnapshotSetIdentity(long value);
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
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Enter(T key, ushort timeoutSeconds, MethodCallback<long> callback);
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
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        long TryEnter(T key, ushort timeoutSeconds);
        /// <summary>
        /// Release the lock
        /// 释放锁
        /// </summary>
        /// <param name="key">Keyword of lock
        /// 锁关键字</param>
        /// <param name="identity">Lock request identity
        /// 锁请求标识</param>
        [ServerMethod(IsSendOnly = true)]
        void Release(T key, long identity);
    }
}
