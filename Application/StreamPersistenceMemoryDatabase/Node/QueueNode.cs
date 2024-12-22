using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 队列节点（先进先出）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueueNode<T> : IQueueNode<T>, ISnapshot<T>
    {
        /// <summary>
        /// 队列
        /// </summary>
        private Queue<T> queue;
        /// <summary>
        /// 队列节点
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        public QueueNode(int capacity)
        {
            queue = new Queue<T>(capacity);
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
        public LeftArray<T> GetSnapshotArray()
        {
            return new LeftArray<T>(queue.ToArray());
        }
        /// <summary>
        /// 获取队列数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return queue.Count;
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            queue.Clear();
        }
        /// <summary>
        /// 判断队列中是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return queue.Contains(value);
        }
        /// <summary>
        /// 将数据添加到队列
        /// </summary>
        /// <param name="value"></param>
        public void Enqueue(T value)
        {
            queue.Enqueue(value);
        }
        /// <summary>
        /// 从队列中弹出一个数据
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        public ValueResult<T> TryDequeue()
        {
            var value = default(T);
            if (queue.TryDequeue(out value)) return value;
            return default(ValueResult<T>);
        }
        /// <summary>
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        public ValueResult<T> TryPeek()
        {
            var value = default(T);
            if (queue.TryPeek(out value)) return value;
            return default(ValueResult<T>);
        }
    }
}
