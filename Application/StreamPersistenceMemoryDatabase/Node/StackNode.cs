using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 栈节点（后进先出）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StackNode<T> : IStackNode<T>, ISnapshot<T>
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
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
        public LeftArray<T> GetSnapshotArray()
        {
            return new LeftArray<T>(stack.ToArray());
        }
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
        /// 从栈中弹出一个数据 持久化参数检查
        /// </summary>
        /// <returns>无返回值表示需要继续调用持久化方法</returns>
        public ValueResult<ValueResult<T>> TryPopBeforePersistence()
        {
            if (stack.Count != 0) return default(ValueResult<ValueResult<T>>);
            return default(ValueResult<T>);
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
