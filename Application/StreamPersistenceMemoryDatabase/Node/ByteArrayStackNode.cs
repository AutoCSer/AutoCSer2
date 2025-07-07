using AutoCSer.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Stack node interface (Last in, first out)
    /// 栈节点接口（后进先出）
    /// </summary>
    public sealed class ByteArrayStackNode : IByteArrayStackNode
#if NetStandard21
        , ISnapshot<byte[]?>
#else
        , ISnapshot<byte[]>
#endif
    {
        /// <summary>
        /// 栈
        /// </summary>
#if NetStandard21
        private Stack<byte[]?> stack;
#else
        private Stack<byte[]> stack;
#endif
        /// <summary>
        /// 栈节点
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        public ByteArrayStackNode(int capacity)
        {
#if NetStandard21
            stack = new Stack<byte[]?>(capacity);
#else
            stack = new Stack<byte[]>(capacity);
#endif
        }
        /// <summary>
        /// Get the snapshot data collection container size for pre-applying snapshot data containers
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>The size of the snapshot data collection container
        /// 快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return stack.Count;
        }
        /// <summary>
        /// Get the snapshot data collection. If the data object may be modified, the cloned data object should be returned to prevent the data from being modified during the snapshot establishment
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="customObject">Custom objects for pre-generating auxiliary data
        /// 自定义对象，用于预生成辅助数据</param>
        /// <returns>Snapshot data
        /// 快照数据</returns>
#if NetStandard21
        public SnapshotResult<byte[]?> GetSnapshotResult(byte[]?[] snapshotArray, object customObject)
#else
        public SnapshotResult<byte[]> GetSnapshotResult(byte[][] snapshotArray, object customObject)
#endif
        {
#if NetStandard21
            return new SnapshotResult<byte[]?>(snapshotArray, stack, stack.Count);
#else
            return new SnapshotResult<byte[]>(snapshotArray, stack, stack.Count);
#endif
        }
        /// <summary>
        /// Reorganize the snapshot data before persistence
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="newArray">Snapshot data collection that exceed the pre-application scope
        /// 超出预申请范围的快照数据集合</param>
#if NetStandard21
        public void SetSnapshotResult(ref LeftArray<byte[]?> array, ref LeftArray<byte[]?> newArray) { }
#else
        public void SetSnapshotResult(ref LeftArray<byte[]> array, ref LeftArray<byte[]> newArray) { }
#endif
        /// <summary>
        /// Add snapshot data
        /// 添加快照数据
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public void SnapshotAdd(byte[]? value)
#else
        public void SnapshotAdd(byte[] value)
#endif
        {
            stack.Push(value);
        }
        /// <summary>
        /// Get the quantity of data
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return stack.Count;
        }
        /// <summary>
        /// Clear all data
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            stack.Clear();
        }
        /// <summary>
        /// Add the data to the stack
        /// 将数据添加到栈
        /// </summary>
        /// <param name="value"></param>
        public void Push(ServerByteArray value)
        {
            stack.Push(value);
        }
        /// <summary>
        /// Pop a piece of data from the stack
        /// 从栈中弹出一个数据
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public ValueResult<byte[]?> TryPop()
#else
        public ValueResult<byte[]> TryPop()
#endif
        {
            var value = default(byte[]);
            if (stack.TryPop(out value)) return value;
#if NetStandard21
            return default(ValueResult<byte[]?>);
#else
            return default(ValueResult<byte[]>);
#endif
        }
        /// <summary>
        /// Pop a piece of data from the stack
        /// 从栈中弹出一个数据
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        public ResponseParameter TryPopResponseParameter()
        {
            var value = default(byte[]);
            if (stack.TryPop(out value)) return (ResponseServerByteArray)value;
            return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
        }
        /// <summary>
        /// Get the next popped data in the stack (no popped data, only view)
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
#if NetStandard21
        public ValueResult<byte[]?> TryPeek()
#else
        public ValueResult<byte[]> TryPeek()
#endif
        {
            var value = default(byte[]);
            if (stack.TryPeek(out value)) return value;
#if NetStandard21
            return default(ValueResult<byte[]?>);
#else
            return default(ValueResult<byte[]>);
#endif
        }
        /// <summary>
        /// Get the next popped data in the stack (no popped data, only view)
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        public ResponseParameter TryPeekResponseParameter()
        {
            var value = default(byte[]);
            if (stack.TryPeek(out value)) return (ResponseServerByteArray)value;
            return (ResponseServerByteArray)CallStateEnum.NullResponseParameter;
        }
    }
}
