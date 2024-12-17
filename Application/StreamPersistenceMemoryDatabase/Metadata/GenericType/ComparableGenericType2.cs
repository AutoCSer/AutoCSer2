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
    internal abstract class ComparableGenericType2 : AutoCSer.Metadata.GenericTypeCache2<ComparableGenericType2>
    {
        /// <summary>
        /// 创建二叉搜索树字典 ISearchTreeDictionaryNode{KT,T}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal abstract NodeIndex CreateSearchTreeDictionaryNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建排序字典节点 ISortedDictionaryNode{KT,T}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal abstract NodeIndex CreateSortedDictionaryNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo);
        /// <summary>
        /// 创建排序列表节点 ISortedListNode{KT,T}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal abstract NodeIndex CreateSortedListNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo, int capacity);

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static ComparableGenericType2 create<T1, T2>()
            where T1 : IComparable<T1>
        {
            return new ComparableGenericType2<T1, T2>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static ComparableGenericType2? lastGenericType;
#else
        protected static ComparableGenericType2 lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type1"></param>
        /// <param name="type2"></param>
        /// <returns></returns>
        public static ComparableGenericType2 Get(Type type1, Type type2)
        {
            var value = lastGenericType;
            if (value?.CurrentType2 == type2 && value.CurrentType1 == type1) return value;

            value = get(type1, type2);
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    internal sealed class ComparableGenericType2<T1, T2> : ComparableGenericType2
        where T1 : IComparable<T1>
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType1 { get { return typeof(T1); } }
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType2 { get { return typeof(T2); } }
        /// <summary>
        /// 创建二叉搜索树字典 ISearchTreeDictionaryNode{KT,T}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal override NodeIndex CreateSearchTreeDictionaryNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return node.CreateNode<ISearchTreeDictionaryNode<T1, T2>, SearchTreeDictionaryNode<T1, T2>, KeyValue<T1, T2>>(index, key, nodeInfo, () => new SearchTreeDictionaryNode<T1, T2>());
        }
        /// <summary>
        /// 创建排序字典节点 ISortedDictionaryNode{KT,T}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal override NodeIndex CreateSortedDictionaryNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo)
        {
            return node.CreateNode<ISortedDictionaryNode<T1, T2>, SortedDictionaryNode<T1, T2>, KeyValue<T1, T2>>(index, key, nodeInfo, () => new SortedDictionaryNode<T1, T2>());
        }
        /// <summary>
        /// 创建排序列表节点 ISortedListNode{KT,T}
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">节点索引信息</param>
        /// <param name="key">节点全局关键字</param>
        /// <param name="nodeInfo">节点信息</param>
        /// <param name="capacity">容器初始化大小</param>
        /// <returns>节点标识，已经存在节点则直接返回</returns>
        internal override NodeIndex CreateSortedListNode(ServiceNode node, NodeIndex index, string key, NodeInfo nodeInfo, int capacity)
        {
            return node.CreateNode<ISortedListNode<T1, T2>, SortedListNode<T1, T2>, KeyValue<T1, T2>>(index, key, nodeInfo, () => new SortedListNode<T1, T2>(capacity));
        }
    }
}
