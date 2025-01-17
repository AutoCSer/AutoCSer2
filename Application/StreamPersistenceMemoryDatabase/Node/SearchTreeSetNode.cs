using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 二叉搜索树集合节点
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    public sealed class SearchTreeSetNode<T> : ISearchTreeSetNode<T>, ISnapshot<T>
        where T : IComparable<T>
    {
        /// <summary>
        /// 二叉搜索树集合
        /// </summary>
        private AutoCSer.SearchTree.Set<T> searchTreeSet;
        /// <summary>
        /// 二叉搜索树集合节点
        /// </summary>
        public SearchTreeSetNode()
        {
            searchTreeSet = new AutoCSer.SearchTree.Set<T>();
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        public int GetSnapshotCapacity(ref object customObject)
        {
            return searchTreeSet.Count;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        public SnapshotResult<T> GetSnapshotResult(T[] snapshotArray, object customObject)
        {
            return new SnapshotResult<T>(snapshotArray, searchTreeSet.Values, searchTreeSet.Count);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        public void SetSnapshotResult(ref LeftArray<T> array, ref LeftArray<T> newArray)
        {
            ServerNode.SetSearchTreeSnapshotResult(ref array, ref newArray);
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return searchTreeSet.Count;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="value">关键字</param>
        /// <returns>是否添加成功，否则表示关键字已经存在</returns>
        public bool Add(T value)
        {
            return value != null && searchTreeSet.Add(value);
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            searchTreeSet.Clear();
        }
        /// <summary>
        /// 判断关键字是否存在
        /// </summary>
        /// <param name="value">关键字</param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return value != null && searchTreeSet.Contains(value);
        }
        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="value">关键字</param>
        /// <returns>是否删除成功</returns>
        public bool Remove(T value)
        {
            return value != null && searchTreeSet.Remove(value);
        }
        /// <summary>
        /// 获取第一个数据
        /// </summary>
        /// <returns>没有数据时返回无返回值</returns>
        public ValueResult<T> GetFrist()
        {
            if (searchTreeSet.Count != 0) return searchTreeSet.Frist;
            return default(ValueResult<T>);
        }
        /// <summary>
        /// 获取最后一个数据
        /// </summary>
        /// <returns>没有数据时返回无返回值</returns>
        public ValueResult<T> GetLast()
        {
            if (searchTreeSet.Count != 0) return searchTreeSet.Last;
            return default(ValueResult<T>);
        }
        /// <summary>
        /// 根据关键字获取一个匹配节点位置
        /// </summary>
        /// <param name="value">关键字</param>
        /// <returns>一个匹配节点位置,失败返回-1</returns>
        public int IndexOf(T value)
        {
            return value != null && searchTreeSet.Count != 0 ? searchTreeSet.IndexOf(value) : -1;
        }
        /// <summary>
        /// 根据关键字比它小的节点数量
        /// </summary>
        /// <param name="value">关键字</param>
        /// <returns>节点数量，失败返回 -1</returns>
        public int CountLess(T value)
        {
            return value != null ? searchTreeSet.CountLess(ref value) : -1;
        }
        /// <summary>
        /// 根据关键字比它大的节点数量
        /// </summary>
        /// <param name="value">关键字</param>
        /// <returns>节点数量，失败返回 -1</returns>
        public int CountThan(T value)
        {
            return value != null ? searchTreeSet.CountThan(ref value) : -1;
        }
        /// <summary>
        /// 根据节点位置获取数据
        /// </summary>
        /// <param name="index">节点位置</param>
        /// <returns>数据</returns>
        public ValueResult<T> GetByIndex(int index)
        {
            if ((uint)index < (uint)searchTreeSet.Count) return searchTreeSet.At(index);
            return default(ValueResult<T>);
        }
    }
}
