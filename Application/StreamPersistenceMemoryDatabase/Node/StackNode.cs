using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Stack node interface (Last in, first out)
    /// 栈节点接口（后进先出）
    /// </summary>
    /// <typeparam name="T"></typeparam>
#if AOT
    public abstract class StackNode<T> : ISnapshot<T>
#else
    public sealed class StackNode<T> : IStackNode<T>, ISnapshot<T>
#endif
    {
        /// <summary>
        /// 栈
        /// </summary>
        private Stack<T> stack;
        /// <summary>
        /// 栈节点
        /// </summary>
        /// <param name="capacity">Container initialization size
        /// 容器初始化大小</param>
        public StackNode(int capacity)
        {
            stack = new Stack<T>(capacity);
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
        public SnapshotResult<T> GetSnapshotResult(T[] snapshotArray, object customObject)
        {
            return new SnapshotResult<T>(snapshotArray, stack, stack.Count);
        }
        /// <summary>
        /// Reorganize the snapshot data before persistence
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">Pre-applied snapshot data container
        /// 预申请的快照数据容器</param>
        /// <param name="newArray">Snapshot data collection that exceed the pre-application scope
        /// 超出预申请范围的快照数据集合</param>
        public void SetSnapshotResult(ref LeftArray<T> array, ref LeftArray<T> newArray) { }
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
        /// Determine whether there is matching data (Since the cached data is a serialized copy of the object, the prerequisite for determining whether the objects are equal is to implement IEquatable{VT})
        /// 判断是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value">Data to be matched
        /// 待匹配数据</param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return stack.Contains(value);
        }
        /// <summary>
        /// Add the data to the stack
        /// 将数据添加到栈
        /// </summary>
        /// <param name="value"></param>
        public void Push(T value)
        {
            stack.Push(value);
        }
        /// <summary>
        /// Pop a piece of data from the stack
        /// 从栈中弹出一个数据
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        public ValueResult<T> TryPop()
        {
            var value = default(T);
            if (stack.TryPop(out value)) return value;
            return default(ValueResult<T>);
        }
        /// <summary>
        /// Get the next popped data in the stack (no popped data, only view)
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>If there is no pop-up data, no data will be returned
        /// 没有可弹出数据则返回无数据</returns>
        public ValueResult<T> TryPeek()
        {
            var value = default(T);
            if (stack.TryPeek(out value)) return value;
            return default(ValueResult<T>);
        }
    }
}
