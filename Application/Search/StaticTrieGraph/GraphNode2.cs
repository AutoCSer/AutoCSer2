using AutoCSer.Extensions;
using System;
using System.Text;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// 字符串 Trie 图二级节点
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct GraphNode2
    {
        /// <summary>
        /// 词语编号
        /// </summary>
        internal int Identity;
        /// <summary>
        /// 文字
        /// </summary>
        internal char Character;
        /// <summary>
        /// 失败节点类型
        /// </summary>
        private LinkTypeEnum linkType;
        /// <summary>
        /// 分布式节点编号
        /// </summary>
        internal byte DistributedIndex;
        /// <summary>
        /// 子节点集合数组起始位置
        /// </summary>
        internal int NodeStartIndex;
        /// <summary>
        /// 子节点集合数组结束位置
        /// </summary>
        internal int NodeEndIndex;
        /// <summary>
        /// 是否存在子节点
        /// </summary>
        internal bool IsNodeArray
        {
            get { return NodeStartIndex != NodeEndIndex; }
        }
        /// <summary>
        /// 设置 Trie 树节点数据
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="node">Trie 树节点</param>
        internal void Set(GraphBuilder builder, ref TreeNode node)
        {
            Identity = node.Identity;
            Character = node.Character;
            int nodeCount = node.NodeCount;
            if (nodeCount != 0)
            {
                NodeStartIndex = builder.NodeIndex;
                int index = 0;
                GraphNode[] nodeArray = builder.NodeArray;
                TreeNode[] nodes = node.Nodes;
                builder.NodeIndex = NodeEndIndex = NodeStartIndex + nodeCount;
                if (nodeCount > 7) nodes.QuickSort(nodeCount, TreeNode.GetCharacter);
                do
                {
                    nodeArray[NodeStartIndex + index].Set(builder, 3, ref nodes[index]);
                }
                while (++index != nodeCount);
            }
        }
        /// <summary>
        /// 建图
        /// </summary>
        /// <param name="builder"></param>
        internal void BuildGraph(GraphBuilder builder)
        {
            if (NodeStartIndex != NodeEndIndex)
            {
                GraphNode[] nodeArray = builder.NodeArray;
                Range link = builder.GetLinkRange(Character);
                if (link.EndIndex != 0) linkType = LinkTypeEnum.Range;
                for (int index = NodeStartIndex; index != NodeEndIndex; ++index)
                {
                    if (nodeArray[index].BuildGraph(builder, link)) builder.BuildGraphIndexs.Add(index);
                }
            }
        }
        /// <summary>
        /// 获取匹配的三级节点索引位置
        /// </summary>
        /// <param name="nodeArray"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        internal int GetIndex(GraphNode[] nodeArray, char character)
        {
            int startIndex = NodeStartIndex;
            switch (NodeEndIndex - NodeStartIndex)
            {
                case 0: return -1;
                case 1: goto COUNT1;
                case 2: goto COUNT2;
                case 3: goto COUNT3;
                case 4: goto COUNT4;
                case 5: goto COUNT5;
                case 6: goto COUNT6;
                case 7:
                    if (nodeArray[startIndex + 6].Character == character) return startIndex + 6;
                    COUNT6:
                    if (nodeArray[startIndex + 5].Character == character) return startIndex + 5;
                    COUNT5:
                    if (nodeArray[startIndex + 4].Character == character) return startIndex + 4;
                    COUNT4:
                    if (nodeArray[startIndex + 3].Character == character) return startIndex + 3;
                    COUNT3:
                    if (nodeArray[startIndex + 2].Character == character) return startIndex + 2;
                    COUNT2:
                    if (nodeArray[startIndex + 1].Character == character) return startIndex + 1;
                    COUNT1:
                    if (nodeArray[startIndex].Character == character) return startIndex;
                    return -1;
                default:
                    int endIndex = NodeEndIndex, average;
                    do
                    {
                        if (character > nodeArray[average = startIndex + ((endIndex - startIndex) >> 1)].Character) startIndex = average + 1;
                        else endIndex = average;
                    }
                    while (startIndex != endIndex);
                    return startIndex != NodeEndIndex && nodeArray[startIndex].Character == character ? startIndex : -1;
            }
            //for (int index = NodeStartIndex; index != NodeEndIndex; ++index)
            //{
            //    if (nodeArray[index].Character == character)
            //    {
            //        if (index == NodeStartIndex) return index;
            //        int newIndex = (index + NodeStartIndex) >> 1;
            //        GraphNode node = nodeArray[index];
            //        nodeArray[index] = nodeArray[newIndex];
            //        nodeArray[newIndex] = node;
            //        return newIndex;
            //    }
            //}
            //return -1;
        }
    }
}
