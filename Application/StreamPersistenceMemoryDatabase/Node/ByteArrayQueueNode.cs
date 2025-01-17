using AutoCSer.Extensions;
using AutoCSer.SearchTree;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 队列节点（先进先出）
    /// </summary>
    public sealed class ByteArrayQueueNode : IByteArrayQueueNode
#if NetStandard21
        , ISnapshot<byte[]?>
#else
        , ISnapshot<byte[]>
#endif
    {
        /// <summary>
        /// 队列
        /// </summary>
#if NetStandard21
        private Queue<byte[]?> queue;
#else
        private Queue<byte[]> queue;
#endif
        /// <summary>
        /// 队列节点
        /// </summary>
        /// <param name="capacity">容器初始化大小</param>
        public ByteArrayQueueNode(int capacity)
        {
#if NetStandard21
            queue = new Queue<byte[]?>(capacity);
#else
            queue = new Queue<byte[]>(capacity);
#endif
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return queue.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
#if NetStandard21
        public SnapshotResult<byte[]?> GetSnapshotResult(byte[]?[] snapshotArray, object customObject)
#else
        public SnapshotResult<byte[]> GetSnapshotResult(byte[][] snapshotArray, object customObject)
#endif
        {
#if NetStandard21
            return new SnapshotResult<byte[]?>(snapshotArray, queue, queue.Count);
#else
            return new SnapshotResult<byte[]>(snapshotArray, queue, queue.Count);
#endif
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
#if NetStandard21
        public void SetSnapshotResult(ref LeftArray<byte[]?> array, ref LeftArray<byte[]?> newArray) { }
#else
        public void SetSnapshotResult(ref LeftArray<byte[]> array, ref LeftArray<byte[]> newArray) { }
#endif
        /// <summary>
        /// 快照添加数据
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public void SnapshotAdd(byte[]? value)
#else
        public void SnapshotAdd(byte[] value)
#endif
        {
            queue.Enqueue(value);
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
        /// 将数据添加到队列
        /// </summary>
        /// <param name="value"></param>
        public void Enqueue(ServerByteArray value)
        {
            queue.Enqueue(value);
        }
        /// <summary>
        /// 从队列中弹出一个数据
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public ValueResult<byte[]?> TryDequeue()
#else
        public ValueResult<byte[]> TryDequeue()
#endif
        {
            var value = default(byte[]);
            if (queue.TryDequeue(out value)) return value;
#if NetStandard21
            return default(ValueResult<byte[]?>);
#else
            return default(ValueResult<byte[]>);
#endif
        }
        /// <summary>
        /// 从队列中弹出一个数据
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        public ResponseParameter TryDequeueResponseParameter()
        {
            var value = default(byte[]);
            if (queue.TryDequeue(out value)) return (ResponseServerByteArray)value;
            return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
        }
        /// <summary>
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public ValueResult<byte[]?> TryPeek()
#else
        public ValueResult<byte[]> TryPeek()
#endif
        {
            var value = default(byte[]);
            if (queue.TryPeek(out value)) return value;
#if NetStandard21
            return default(ValueResult<byte[]?>);
#else
            return default(ValueResult<byte[]>);
#endif
        }
        /// <summary>
        /// 获取队列中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        public ResponseParameter TryPeekResponseParameter()
        {
            var value = default(byte[]);
            if (queue.TryPeek(out value)) return (ResponseServerByteArray)value;
            return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
        }
    }
}
