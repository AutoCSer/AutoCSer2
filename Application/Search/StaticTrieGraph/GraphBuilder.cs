using AutoCSer.Algorithm;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// Trie 树转数组创建器
    /// </summary>
    internal sealed class GraphBuilder
    {
        /// <summary>
        /// 哈希取余
        /// </summary>
        private readonly IntegerDivision hashCapacityDivision;
        /// <summary>
        /// 二级节点哈希数组
        /// </summary>
        internal readonly GrahpHashNode[] HashNodes;
        /// <summary>
        /// 三级及以下节点数组
        /// </summary>
        internal readonly GrahpNode[] Nodes;
        /// <summary>
        /// 等待建图节点数组索引位置集合
        /// </summary>
        internal LeftArray<int> BuildGraphIndexs;
        /// <summary>
        /// 正在建图的节点数组索引位置集合
        /// </summary>
        private LeftArray<int> buildGraphIndexs;
        /// <summary>
        /// Trie 树转数组创建器
        /// </summary>
        /// <param name="data"></param>
        /// <param name="hashCapacityDivision"></param>
        internal GraphBuilder(ref GraphData data, ref IntegerDivision hashCapacityDivision)
        {
            this.hashCapacityDivision = hashCapacityDivision;
            HashNodes = data.HashNodes;
            Nodes = data.Nodes;
            BuildGraphIndexs.SetEmpty();
            buildGraphIndexs.SetEmpty();
        }
        /// <summary>
        /// 建图
        /// </summary>
        internal void BuildGraph()
        {
            foreach (GrahpHashNode node in HashNodes)
            {
                int nodeIndex = node.NodeIndex;
                if (nodeIndex != 0)
                {
                    int nextNodeIndex;
                    while (Nodes[nodeIndex].BuildGraph(this, node, out nextNodeIndex))
                    {
                        if (nextNodeIndex != 0) BuildGraphIndexs.Add(nodeIndex);
                        ++nodeIndex;
                    }
                }
            }
            while (BuildGraphIndexs.Count != 0)
            {
                LeftArray<int> indexs = BuildGraphIndexs;
                BuildGraphIndexs = buildGraphIndexs;
                buildGraphIndexs = indexs;
                BuildGraphIndexs.Length = 0;
                int count = indexs.Length;
                foreach (int nodeIndex in indexs.Array)
                {
                    Nodes[nodeIndex].BuildGraph(this);
                    if (--count == 0) break;
                }
            }
        }
        /// <summary>
        /// 获取失败节点位置
        /// </summary>
        /// <param name="characters"></param>
        /// <returns></returns>
        internal int GetLinkIndex(uint characters)
        {
            int nodeIndex = (int)(HashNodes[(int)hashCapacityDivision.GetMod(characters)].HashNodeIndex & int.MaxValue);
            if (nodeIndex != 0)
            {
                bool isNext;
                while (!HashNodes[nodeIndex].Check(characters, out isNext))
                {
                    if (!isNext) return 0;
                    ++nodeIndex;
                }
                return nodeIndex;
            }
            return 0;
        }
    }
}
