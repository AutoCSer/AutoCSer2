using AutoCSer.CommandService.Search.RemoveMarkHashIndexCache;
using System;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 索引节点数据
    /// </summary>
    /// <typeparam name="T">查询数据关键字类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct IndexNode<T>
#if NetStandard21
        where T : notnull, IEquatable<T>
#else
        where T : IEquatable<T>
#endif
    {
        /// <summary>
        /// 关键字数据磁盘块索引信息节点
        /// </summary>
        internal BlockIndexDataCacheNode<T> Node;
        /// <summary>
        /// 索引
        /// </summary>
        internal IIndex<T> Index;
        /// <summary>
        /// 预估数据数量
        /// </summary>
        internal int EstimatedCount { get { return !object.ReferenceEquals(Index, ArrayIndex<T>.Empty) ? Index.Count : Node.EstimatedCount; } }
        /// <summary>
        /// 是否已经加载数据
        /// </summary>
        internal bool IsLoaded { get { return !object.ReferenceEquals(Index, ArrayIndex<T>.Empty); } }
        /// <summary>
        /// 索引节点数据
        /// </summary>
        /// <param name="node">关键字数据磁盘块索引信息节点</param>
        public IndexNode(BlockIndexDataCacheNode<T> node)
        {
            Node = node;
            Index = node.GetLoadedIndex() ?? ArrayIndex<T>.Empty;
        }
        /// <summary>
        /// 设置关键字数据磁盘块索引信息节点
        /// </summary>
        /// <param name="node">关键字数据磁盘块索引信息节点</param>
        internal void Set(BlockIndexDataCacheNode<T> node)
        {
            Node = node;
            Index = node.GetLoadedIndex() ?? ArrayIndex<T>.Empty;
        }
    }
}
