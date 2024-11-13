using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// Trie 图创建器
    /// </summary>
    /// <typeparam name="T">关键字类型</typeparam>
    internal class TrieGraphBuilder<T>
        where T : struct, IEquatable<T>
    {
        /// <summary>
        /// 根节点
        /// </summary>
        private readonly Dictionary<T, TrieGraphNode<T>> boot;
        /// <summary>
        /// 当前处理结果节点集合
        /// </summary>
        internal LeftArray<TrieGraphNode<T>> Writer;
        /// <summary>
        /// 当前处理节点集合
        /// </summary>
        protected TrieGraphNode<T>[] reader;
        /// <summary>
        /// 处理节点起始索引位置
        /// </summary>
        protected int startIndex;
        /// <summary>
        /// 处理节点数量
        /// </summary>
        protected int count;
        /// <summary>
        /// Trie 图创建器
        /// </summary>
        /// <param name="boot">根节点</param>
        internal TrieGraphBuilder(TrieGraphNode<T> boot)
        {
            this.boot = boot.Nodes.notNull();
            Writer = new LeftArray<TrieGraphNode<T>>(0);
            reader = EmptyArray<TrieGraphNode<T>>.Array;
        }
        /// <summary>
        /// 设置当前处理节点集合
        /// </summary>
        /// <param name="reader">当前处理节点集合</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(ref LeftArray<TrieGraphNode<T>> reader)
        {
            this.reader = reader.Array;
            startIndex = 0;
            count = reader.Length;
        }
        /// <summary>
        /// 建图
        /// </summary>
        internal unsafe void Build()
        {
            Writer.Length = 0;
            int endIndex = startIndex + count;
            while (startIndex != endIndex)
            {
                TrieGraphNode<T> fatherNode = reader[startIndex++];
                if (fatherNode.Link == null)
                {
                    foreach (KeyValuePair<T, TrieGraphNode<T>> nextNode in fatherNode.Nodes.notNull())
                    {
                        if (nextNode.Value.GetNodeCount(boot, nextNode.Key) != 0) Writer.Add(nextNode.Value);
                    }
                }
                else
                {
                    foreach (KeyValuePair<T, TrieGraphNode<T>> nextNode in fatherNode.Nodes.notNull())
                    {
                        var link = fatherNode.Link;
                        var linkNode = default(TrieGraphNode<T>);
                        do
                        {
                            if (link.GetLinkWhereNull(nextNode.Key, ref linkNode, ref link) == 0)
                            {
                                if (link == null)
                                {
                                    if ((boot.TryGetValue(nextNode.Key, out linkNode) ? nextNode.Value.GetNodeCount(linkNode) : nextNode.Value.GetNodeCount()) != 0) Writer.Add(nextNode.Value);
                                    break;
                                }
                            }
                            else
                            {
                                if (nextNode.Value.GetNodeCount(linkNode.notNull()) != 0) Writer.Add(nextNode.Value);
                                break;
                            }
                        }
                        while (true);
                    }
                }
            }
        }
    }
}
