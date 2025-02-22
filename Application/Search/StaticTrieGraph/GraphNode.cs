using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// 字符串 Trie 图三级及以下节点
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct GraphNode
    {
        /// <summary>
        /// 词语编号
        /// </summary>
        private int identity;
        /// <summary>
        /// 失败节点数组索引位置
        /// </summary>
        private int linkIndex;
        /// <summary>
        /// 子节点集合数组起始位置
        /// </summary>
        private int nodeStartIndex;
        /// <summary>
        /// 子节点集合数组结束位置
        /// </summary>
        private int nodeEndIndex;
        /// <summary>
        /// 是否存在子节点
        /// </summary>
        private bool isNodeArray
        {
            get { return nodeStartIndex != nodeEndIndex; }
        }
        /// <summary>
        /// 词语长度
        /// </summary>
        private int length;
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
        /// 设置 Trie 树节点数据
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="length">词语长度</param>
        /// <param name="node">Trie 树节点</param>
        internal void Set(GraphBuilder builder, int length, ref TreeNode node)
        {
            identity = node.Identity;
            this.length = length;
            Character = node.Character;
            int nodeCount = node.NodeCount;
            if (nodeCount != 0)
            {
                nodeStartIndex = builder.NodeIndex;
                int index = 0, nextLength = length + 1;
                GraphNode[] nodeArray = builder.NodeArray;
                TreeNode[] nodes = node.Nodes;
                builder.NodeIndex = nodeEndIndex = nodeStartIndex + nodeCount;
                do
                {
                    nodeArray[nodeStartIndex + index].Set(builder, nextLength, ref nodes[index]);
                }
                while (++index != nodeCount);
            }
        }
        /// <summary>
        /// 三级节点建图
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="link">二级节点索引范围</param>
        /// <returns>是否存在子节点</returns>
        internal bool BuildGraph(GraphBuilder builder, Range link)
        {
            if (link.EndIndex != 0)
            {
                linkIndex = builder.GetLink2(link.StartIndex, link.EndIndex, Character);
                if (linkIndex >= 0)
                {
                    linkType = LinkTypeEnum.Node2;
                    return nodeStartIndex != nodeEndIndex;
                }
            }
            linkType = builder.GetLinkType(Character);
            return nodeStartIndex != nodeEndIndex;
        }
        /// <summary>
        /// 建图
        /// </summary>
        /// <param name="builder"></param>
        internal void BuildGraph(GraphBuilder builder)
        {
            if (nodeStartIndex != nodeEndIndex)
            {
                GraphNode[] nodeArray = builder.NodeArray;
                for (int index = nodeStartIndex; index != nodeEndIndex; ++index)
                {
                    if (nodeArray[index].buildGraph(builder, ref this)) builder.BuildGraphIndexs.Add(index);
                }
            }
        }
        /// <summary>
        /// 建图
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="parent">父节点</param>
        /// <returns>是否存在子节点</returns>
        private bool buildGraph(GraphBuilder builder, ref GraphNode parent)
        {
            switch (parent.linkType)
            {
                case LinkTypeEnum.Node:
                    int parentLinkIndex = parent.linkIndex;
                    do
                    {
                        GraphNode node = builder.NodeArray[parentLinkIndex];
                        if (node.isNodeArray)
                        {
                            int index = node.nodeStartIndex, endIndex = node.nodeStartIndex;
                            GraphNode[] nodeArray = builder.NodeArray;
                            do
                            {
                                GraphNode checkNode = nodeArray[index];
                                if (checkNode.Character == Character)
                                {
                                    if (checkNode.isNodeArray)
                                    {
                                        linkIndex = index;
                                        linkType = LinkTypeEnum.Node;
                                        return nodeStartIndex != nodeEndIndex;
                                    }
                                    break;
                                }
                            }
                            while (++index != endIndex);
                        }
                        switch (node.linkType)
                        {
                            case LinkTypeEnum.None: goto NONE;
                            case LinkTypeEnum.Range: goto RANGE;
                            case LinkTypeEnum.Node2:
                                parentLinkIndex = node.linkIndex;
                                goto NODE2;
                            case LinkTypeEnum.Node:
                                parentLinkIndex = node.linkIndex;
                                break;
                        }
                    }
                    while (true);
                case LinkTypeEnum.Node2:
                    parentLinkIndex = parent.linkIndex;
                NODE2:
                    GraphNode2 node2 = builder.NodeArray2[parentLinkIndex];
                    if (node2.IsNodeArray)
                    {
                        int index = node2.NodeStartIndex, endIndex = node2.NodeStartIndex;
                        GraphNode[] nodeArray = builder.NodeArray;
                        do
                        {
                            GraphNode node = nodeArray[index];
                            if (node.Character == Character)
                            {
                                if (node.isNodeArray)
                                {
                                    linkIndex = index;
                                    linkType = LinkTypeEnum.Node;
                                    return nodeStartIndex != nodeEndIndex;
                                }
                                goto RANGE;
                            }
                        }
                        while (++index != endIndex);
                    }
                    goto RANGE;
                case LinkTypeEnum.Range:
                RANGE:
                    linkIndex = builder.GetLink2(parent.Character, Character);
                    if (linkIndex >= 0)
                    {
                        linkType = LinkTypeEnum.Node2;
                        return nodeStartIndex != nodeEndIndex;
                    }
                    break;
            }
        NONE:
            linkType = builder.GetLinkType(Character);
            return nodeStartIndex != nodeEndIndex;
        }
        /// <summary>
        /// 获取词语编号
        /// </summary>
        /// <param name="length">词语长度</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetIdentity(ref int length)
        {
            length = this.length;
            return identity;
        }
        /// <summary>
        /// 获取失败节点类型
        /// </summary>
        /// <param name="linkIndex">失败节点数组索引位置</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal LinkTypeEnum GetLinkType(ref int linkIndex)
        {
            linkIndex = this.linkIndex;
            return linkType;
        }
        /// <summary>
        /// 获取匹配的节点索引位置
        /// </summary>
        /// <param name="nodeArray"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        internal int GetIndex(GraphNode[] nodeArray, char character)
        {
            for (int index = nodeStartIndex; index != nodeEndIndex; ++index)
            {
                if (nodeArray[index].Character == character)
                {
                    if (index == nodeStartIndex) return index;
                    int newIndex = (index + nodeStartIndex) >> 1;
                    GraphNode node = nodeArray[index];
                    nodeArray[index] = nodeArray[newIndex];
                    nodeArray[newIndex] = node;
                    return newIndex;
                }
            }
            return -1;
        }
    }
}
