using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 排序集合节点
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    public class SortedSetNode<T> : ISortedSetNode<T>, ISnapshot<T>
        where T : IComparable<T>
    {
        /// <summary>
        /// 排序集合
        /// </summary>
        private SortedSet<T> sortedSet;
        /// <summary>
        /// 排序集合节点
        /// </summary>
        public SortedSetNode()
        {
            sortedSet = new SortedSet<T>();
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <returns>快照数据集合</returns>
        public LeftArray<T> GetSnapshotArray()
        {
            return sortedSet.getLeftArray();
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return sortedSet.Count;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        public bool Add(T value)
        {
            return value != null && sortedSet.Add(value);
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            sortedSet.Clear();
        }
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return value != null && sortedSet.Contains(value);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="value"></param>
        /// <returns>是否删除成功</returns>
        public bool Remove(T value)
        {
            return value != null && sortedSet.Remove(value);
        }
        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <returns>没有数据时返回无返回值</returns>
        public ValueResult<T> GetMin()
        {
#pragma warning disable CS8604
            if (sortedSet.Count != 0) return sortedSet.Min;
#pragma warning restore CS8604
            return default(ValueResult<T>);
        }
        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <returns>没有数据时返回无返回值</returns>
        public ValueResult<T> GetMax()
        {
#pragma warning disable CS8604
            if (sortedSet.Count != 0) return sortedSet.Max;
#pragma warning restore CS8604
            return default(ValueResult<T>);
        }
    }
}
