using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// Trie 图节点
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    internal unsafe sealed class TrieGraphNode<T>
        where T : struct, IEquatable<T>
    {
        /// <summary>
        /// 子节点
        /// </summary>
        internal Dictionary<T, TrieGraphNode<T>> Nodes;
        /// <summary>
        /// 失败节点
        /// </summary>
        internal TrieGraphNode<T> Link;
        /// <summary>
        /// 节点值长度，0 表示没有节点值
        /// </summary>
        internal int Length;
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="letter">当前字符</param>
        /// <returns>子节点</returns>
        internal TrieGraphNode<T> Create(T letter)
        {
            TrieGraphNode<T> node;
            if (Nodes == null)
            {
                Nodes = DictionaryCreator<T>.Create<TrieGraphNode<T>>();
                Nodes[letter] = node = new TrieGraphNode<T>();
            }
            else if (!Nodes.TryGetValue(letter, out node))
            {
                Nodes.Add(letter, node = new TrieGraphNode<T>());
            }
            return node;
        }
        /// <summary>
        /// 设置失败节点并获取子节点数量
        /// </summary>
        /// <param name="boot">根节点</param>
        /// <param name="letter">当前字符</param>
        /// <returns>子节点数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetNodeCount(Dictionary<T, TrieGraphNode<T>> boot, T letter)
        {
            boot.TryGetValue(letter, out Link);
            return Nodes == null ? 0 : Nodes.Count;
        }
        /// <summary>
        /// 子节点不存在时获取失败节点
        /// </summary>
        /// <param name="letter">当前字符</param>
        /// <param name="node">子节点</param>
        /// <param name="link">失败节点</param>
        /// <returns>是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetLinkWhereNull(T letter, ref TrieGraphNode<T> node, ref TrieGraphNode<T> link)
        {
            if (Nodes == null || Nodes.Count == 0 || !Nodes.TryGetValue(letter, out node))
            {
                link = Link;
                return 0;
            }
            return 1;
        }
        /// <summary>
        /// 设置失败节点并获取子节点数量
        /// </summary>
        /// <param name="link">失败节点</param>
        /// <returns>子节点数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetNodeCount(TrieGraphNode<T> link)
        {
            Link = link;
            return Nodes == null ? 0 : Nodes.Count;
        }
        /// <summary>
        /// 获取子节点数量
        /// </summary>
        /// <returns>子节点数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetNodeCount()
        {
            return Nodes == null ? 0 : Nodes.Count;
        }
    }
}
