using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 分布式锁节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true)]
    public partial interface IDistributedLockNode<T>
    {
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
        void SnapshotSet(DistributedLockIdentity<T> value);
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="timeoutSeconds">超时秒数</param>
        /// <param name="callback">失败返回 0</param>
        void Enter(T key, ushort timeoutSeconds, MethodCallback<long> callback);
        /// <summary>
        /// 尝试申请锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="timeoutSeconds">超时秒数</param>
        /// <returns>失败返回 0</returns>
        long TryEnter(T key, ushort timeoutSeconds);
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">锁关键字</param>
        /// <param name="identity">锁操作标识</param>
        [ServerMethod(IsSendOnly = true)]
        void Release(T key, long identity);
    }
}
