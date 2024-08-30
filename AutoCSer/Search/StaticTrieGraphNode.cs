using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定静态节点池的 Trie 图节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal struct StaticTrieGraphNode<T>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        /// <summary>
        /// 子节点
        /// </summary>
        internal KeyValue<T, int>[] Nodes;
        /// <summary>
        /// 失败节点
        /// </summary>
        internal int Link;
        /// <summary>
        /// 节点值长度，0 表示没有节点值
        /// </summary>
        internal int Length;
        /// <summary>
        /// 初始化重置数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Reset()
        {
            Nodes = EmptyArray<KeyValue<T, int>>.Array;
            Link = 0;
        }
        /// <summary>
        /// 设置节点数据
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="length"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(KeyValue<T, int>[] nodes, int length)
        {
            Nodes = nodes;
            Length = length;
            //Link = 0;
        }
        /// <summary>
        /// 创建错误取消节点
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void CancelBuilder()
        {
            Nodes = EmptyArray<KeyValue<T, int>>.Array;
        }
        /// <summary>
        /// 释放节点
        /// </summary>
        internal void Free()
        {
            if (Nodes.Length != 0)
            {
                ArrayPool<StaticTrieGraphNode<T>> nodePool = StaticTrieGraph<T>.NodePool;
                foreach (KeyValue<T, int> node in Nodes) nodePool.Pool[node.Value >> ArrayPool.ArraySizeBit][node.Value & ArrayPool.ArraySizeAnd].Free();
                nodePool.FreeNoLock(Nodes);
            }
            CancelBuilder();
        }
        /// <summary>
        /// 二分查找子节点
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        internal int GetNode(T letter)
        {
            int start = 0, length = Nodes.Length, average;
            do
            {
                if (letter.CompareTo(Nodes[average = start + ((length - start) >> 1)].Key) > 0) start = average + 1;
                else length = average;
            }
            while (start != length);
            return start != Nodes.Length && letter.CompareTo(Nodes[start].Key) == 0 ? Nodes[start].Value : 0;
        }
        ///// <summary>
        ///// 子节点不存在时获取失败节点
        ///// </summary>
        ///// <param name="letter">当前字符</param>
        ///// <param name="node">子节点</param>
        ///// <param name="link">失败节点</param>
        ///// <returns>是否成功</returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal int GetLinkWhereNull(KT letter, ref int node, ref int link)
        //{
        //    if (Nodes.Length == 0 || (node = GetNode(letter)) == 0)
        //    {
        //        link = Link;
        //        return 0;
        //    }
        //    return 1;
        //}
        ///// <summary>
        ///// 子节点不存在时获取失败节点
        ///// </summary>
        ///// <param name="letter">当前字符</param>
        ///// <param name="node">子节点</param>
        ///// <param name="link">失败节点</param>
        ///// <param name="value">节点值</param>
        ///// <returns>是否成功</returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal int GetNodeOrLink(KT letter, ref int node, ref int link, out VT value)
        //{
        //    value = Value;
        //    if (Nodes.Length == 0 || (node = GetNode(letter)) == 0)
        //    {
        //        link = Link;
        //        return 0;
        //    }
        //    return 1;
        //}
        ///// <summary>
        ///// 获取失败节点
        ///// </summary>
        ///// <param name="link">失败节点</param>
        ///// <returns>节点值</returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal VT GetLink(ref int link)
        //{
        //    link = Link;
        //    return Value;
        //}
    }
}
