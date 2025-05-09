using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 栈节点（后进先出）
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
        /// <param name="capacity">容器初始化大小</param>
        public StackNode(int capacity)
        {
            stack = new Stack<T>(capacity);
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return stack.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        public SnapshotResult<T> GetSnapshotResult(T[] snapshotArray, object customObject)
        {
            return new SnapshotResult<T>(snapshotArray, stack, stack.Count);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        public void SetSnapshotResult(ref LeftArray<T> array, ref LeftArray<T> newArray) { }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return stack.Count;
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            stack.Clear();
        }
        /// <summary>
        /// 判断是否存在匹配数据（由于缓存数据是序列化的对象副本，所以判断是否对象相等的前提是实现 IEquatable{VT} ）
        /// </summary>
        /// <param name="value">匹配数据</param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return stack.Contains(value);
        }
        /// <summary>
        /// 将数据添加到栈
        /// </summary>
        /// <param name="value"></param>
        public void Push(T value)
        {
            stack.Push(value);
        }
        /// <summary>
        /// 从栈中弹出一个数据
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        public ValueResult<T> TryPop()
        {
            var value = default(T);
            if (stack.TryPop(out value)) return value;
            return default(ValueResult<T>);
        }
        /// <summary>
        /// 获取栈中下一个弹出数据（不弹出数据仅查看）
        /// </summary>
        /// <returns>没有可弹出数据则返回无数据</returns>
        public ValueResult<T> TryPeek()
        {
            var value = default(T);
            if (stack.TryPeek(out value)) return value;
            return default(ValueResult<T>);
        }
    }
}
