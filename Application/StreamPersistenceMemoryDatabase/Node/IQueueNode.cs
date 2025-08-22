using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Queue node interface (First In, First Out)
    /// 队列节点接口（先进先出）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ServerNode(IsLocalClient = true, IsReturnValueNode = ServerNodeAttribute.DefaultIsReturnValueNode)]
    public partial interface IQueueNode<T>
    {
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
        /// Determine whether there is matching data in the queue (Since the cached data is a serialized copy of the object, the prerequisite for determining whether the objects are equal is to implement IEquatable{VT})
        /// 判断队列中是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value">Data to be matched
        /// 待匹配数据</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool Contains(T value);
        /// <summary>
        /// Add the data to the queue
        /// 将数据添加到队列
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(SnapshotMethodSort = 1, IsIgnorePersistenceCallbackException = true)]
        void Enqueue(T value);
        /// <summary>
        /// Pop a piece of data from the queue
        /// 从队列中弹出一个数据
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        ValueResult<T> TryDequeue();
        /// <summary>
        /// Get the next pop-up data in the queue (no pop-up data, only view)
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> TryPeek();
    }
}
