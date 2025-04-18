using AutoCSer.Metadata;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class ComparableGenericType : AutoCSer.Metadata.GenericTypeCache<ComparableGenericType>
    {
        /// <summary>
        /// 创建二叉搜索树集合节点 ISearchTreeSetNode{KT}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal abstract NodeIndex CreateSearchTreeSetNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建排序集合节点 ISortedSetNode{KT}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal abstract NodeIndex CreateSortedSetNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo);

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static ComparableGenericType create<T>()
            where T : IComparable<T>
        {
            return new ComparableGenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static ComparableGenericType? lastGenericType;
#else
        protected static ComparableGenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ComparableGenericType Get(Type type)
        {
            var value = lastGenericType;
            if (value?.CurrentType == type) return value;
            value = get(type);
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ComparableGenericType<T> : ComparableGenericType
        where T : IComparable<T>
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }
        /// <summary>
        /// 创建二叉搜索树集合节点 ISearchTreeSetNode{KT}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal override NodeIndex CreateSearchTreeSetNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return node.CreateSnapshotNode<ISearchTreeSetNode<T>>(index, key, nodeInfo, () => new SearchTreeSetNode<T>());
        }
        /// <summary>
        /// 创建排序集合节点 ISortedSetNode{KT}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal override NodeIndex CreateSortedSetNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return node.CreateSnapshotNode<ISortedSetNode<T>>(index, key, nodeInfo, () => new SortedSetNode<T>());
        }
    }
}
