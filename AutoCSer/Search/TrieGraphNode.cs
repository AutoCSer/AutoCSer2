using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
#if NetStandard21
        internal Dictionary<T, TrieGraphNode<T>>? Nodes;
#else
        internal Dictionary<T, TrieGraphNode<T>> Nodes;
#endif
        /// <summary>
        /// 失败节点
        /// </summary>
#if NetStandard21
        internal TrieGraphNode<T>? Link;
#else
        internal TrieGraphNode<T> Link;
#endif
        /// <summary>
        /// 节点值长度，0 表示没有节点值
        /// </summary>
        internal int Length;
        /// <summary>
        /// 分布式搜索分词编号
        /// </summary>
        internal uint Identity;
        /// <summary>
        /// 创建子节点
        /// </summary>
        /// <param name="letter">当前字符</param>
        /// <returns>子节点</returns>
        internal TrieGraphNode<T> Create(T letter)
        {
            var node = default(TrieGraphNode<T>);
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
        /// 设置节点值长度与分词编号
        /// </summary>
        /// <param name="length"></param>
        /// <param name="identity"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(int length, int identity)
        {
            Length = length;
            Identity = (uint)identity;
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
#if NetStandard21
        internal int GetLinkWhereNull(T letter, ref TrieGraphNode<T>? node, ref TrieGraphNode<T>? link)
#else
        internal int GetLinkWhereNull(T letter, ref TrieGraphNode<T> node, ref TrieGraphNode<T> link)
#endif
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
        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="node"></param>
        /// <returns>false 返回失败节点</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal bool GetNode(T letter, [MaybeNullWhen(false)] out TrieGraphNode<T> node)
#else
        internal bool GetNode(T letter, out TrieGraphNode<T> node)
#endif
        {
            if (Nodes.notNull().TryGetValue(letter, out node)) return true;
            node = Link;
            return false;
        }
        /// <summary>
        /// 检查节点匹配
        /// </summary>
        /// <param name="matchs"></param>
        /// <returns>返回 true 表示需要匹配失败节点</returns>
        internal bool CheckMatch(ref LeftArray<AutoCSer.Search.TrieGraphNode<T>> matchs)
        {
            if (Length != 0)
            {
                if ((Identity & 0x80000000U) == 0)
                {
                    matchs.Add(this);
                    Identity |= 0x80000000U;
                }
                return Nodes == null;
            }
            return false;
        }
        /// <summary>
        /// 取消节点匹配
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int FreeMatch()
        {
            Identity &= int.MaxValue;
            return (int)Identity;
        }
    }
}
