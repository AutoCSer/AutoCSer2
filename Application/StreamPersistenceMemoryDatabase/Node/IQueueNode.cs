﻿using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 队列节点接口（先进先出）
    /// </summary>
    /// <typeparam name="T"></typeparam>
#if !AOT
    [ServerNode(IsAutoMethodIndex = false, IsLocalClient = true)]
#endif
    public partial interface IQueueNode<T>
    {
        /// <summary>
        /// 获取队列数据数量
        /// </summary>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        int Count();
        /// <summary>
        /// 清除所有数据
        /// </summary>
        void Clear();
        /// <summary>
        /// 判断队列中是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns></returns>
        [ServerMethod(IsPersistence = false)]
        bool Contains(T value);
        /// <summary>
        /// 将数据添加到队列
        /// </summary>
        /// <param name="value"></param>
        [ServerMethod(SnapshotMethodSort = 1, IsIgnorePersistenceCallbackException = true)]
        void Enqueue(T value);
        /// <summary>
        /// 从队列中弹出一个数据
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        ValueResult<T> TryDequeue();
        /// <summary>
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        [ServerMethod(IsPersistence = false)]
        ValueResult<T> TryPeek();
    }
}
