using System;
using System.Collections.Generic;
using System.Threading;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定静态节点池的字符串 Trie 图创建器
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct StaticStringTrieGraphBuilder
    {
        /// <summary>
        /// 已创建缓存集合
        /// </summary>
        private Dictionary<TrieGraphNode<char>, int> cache;
        /// <summary>
        /// 当前创建的节点索引
        /// </summary>
        private int nodeIndex;
        /// <summary>
        /// 创建 Trie 图
        /// </summary>
        /// <param name="staticGraph"></param>
        /// <param name="graph"></param>
        internal void Create(StaticStringTrieGraph staticGraph, StringTrieGraph graph)
        {
            cache = AutoCSer.Extensions.DictionaryCreator.CreateOnly<TrieGraphNode<char>, int>();
            TrieGraphNode<char> boot = graph.Boot;
            ArrayPool<StaticTrieGraphNode<char>> nodePool = StaticTrieGraph<char>.NodePool;
            object nodePoolLock = nodePool.Lock;
            Monitor.Enter(nodePoolLock);
            try
            {
                staticGraph.SetBoot(create(boot));
                createLink(boot);
                boot = null;
            }
            finally
            {
                if (boot == null) Monitor.Exit(nodePoolLock);
                else
                {
                    try
                    {
                        staticGraph.CancelBuilder();
                        foreach (int index in cache.Values)
                        {
                            if (index == nodeIndex) nodeIndex = 0;
                            nodePool.Pool[index >> ArrayPool.ArraySizeBit][index & ArrayPool.ArraySizeAnd].CancelBuilder();
                        }
                        nodePool.FreeNoLock(cache.Values);
                        if (nodeIndex != 0)
                        {
                            nodePool.Pool[nodeIndex >> ArrayPool.ArraySizeBit][nodeIndex & ArrayPool.ArraySizeAnd].CancelBuilder();
                            nodePool.FreeNoLock(nodeIndex);
                        }
                    }
                    finally { Monitor.Exit(nodePoolLock); }
                }
            }
        }
        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private int create(TrieGraphNode<char> node)
        {
            //if (node == null) return 0;
            if (cache.TryGetValue(node, out this.nodeIndex)) return this.nodeIndex;
            StaticTrieGraphNode<char>[] nodes;
            cache.Add(node, this.nodeIndex = StaticTrieGraph<char>.GetNodeIndex(out nodes));
            int nodeIndex = this.nodeIndex;
            nodes[nodeIndex & ArrayPool.ArraySizeAnd].Set(create(node.Nodes), node.Length);
            return nodeIndex;
        }
        /// <summary>
        /// 创建节点集合
        /// </summary>
        /// <param name="nodes">节点集合</param>
        /// <returns></returns>
        private KeyValue<char, int>[] create(Dictionary<char, TrieGraphNode<char>> nodes)
        {
            if (nodes == null || nodes.Count == 0) return EmptyArray<KeyValue<char, int>>.Array;
            int index = 0;
            KeyValue<char, int>[] array = new KeyValue<char, int>[nodes.Count];
            foreach (KeyValuePair<char, TrieGraphNode<char>> value in nodes) array[index++].Set(value.Key, create(value.Value));
            if (array.Length > 1) Array.Sort(array, sortHandle);
            return array;
        }
        /// <summary>
        /// 创建失败节点
        /// </summary>
        /// <param name="node"></param>
        private void createLink(TrieGraphNode<char> node)
        {
            if (node != null)
            {
                if (node.Link != null)
                {
                    this.nodeIndex = cache[node];
                    StaticTrieGraph<char>.NodePool.Pool[this.nodeIndex >> ArrayPool.ArraySizeBit][this.nodeIndex & ArrayPool.ArraySizeAnd].Link = cache[node.Link];
                }
                createLink(node.Nodes);
            }
        }
        /// <summary>
        /// 创建失败节点
        /// </summary>
        /// <param name="nodes">节点集合</param>
        private void createLink(Dictionary<char, TrieGraphNode<char>> nodes)
        {
            if (nodes != null && nodes.Count != 0)
            {
                foreach (TrieGraphNode<char> value in nodes.Values) createLink(value);
            }
        }

        /// <summary>
        /// 节点字符排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int sort(KeyValue<char, int> left, KeyValue<char, int> right)
        {
            return (int)left.Key - (int)right.Key;
        }
        /// <summary>
        /// 节点字符排序委托
        /// </summary>
        private static readonly Comparison<KeyValue<char, int>> sortHandle = sort;
    }
}
