using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Queue node Interface (First In, First Out)
    /// 队列节点接口（先进先出）
    /// </summary>
    [ServerNode]
    public partial interface IByteArrayQueueNode
    {
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsClientCall = false, SnapshotMethodSort = 1)]
#if NetStandard21
        void SnapshotAdd(byte[]? value);
#else
        void SnapshotAdd(byte[] value);
#endif
        /// <summary>
        /// Get the number of queue data
        /// 获取队列数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// Clear all data
        /// 清除所有数据
        /// </summary>
        void Clear();
        /// <summary>
        /// Add the data to the queue
        /// 将数据添加到队列
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(IsIgnorePersistenceCallbackException = true)]
        void Enqueue(ServerByteArray value);
        /// <summary>
        /// Pop a piece of data from the queue
        /// 从队列中弹出一个数据
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
#if NetStandard21
        ValueResult<byte[]?> TryDequeue();
#else
        ValueResult<byte[]> TryDequeue();
#endif
        /// <summary>
        /// Pop a piece of data from the queue
        /// 从队列中弹出一个数据
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        ResponseParameter TryDequeueResponseParameter();
        /// <summary>
        /// Get the next pop-up data in the queue (no pop-up data, only view)
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        [ServerMethod(IsPersistence = false)]
#if NetStandard21
        ValueResult<byte[]?> TryPeek();
#else
        ValueResult<byte[]> TryPeek();
#endif
        /// <summary>
        /// Get the next pop-up data in the queue (no pop-up data, only view)
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        [ServerMethod(IsPersistence = false)]
        ResponseParameter TryPeekResponseParameter();
    }
}
