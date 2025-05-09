using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// 字符串 Trie 图三级及以下节点
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct GrahpNode
    {
        /// <summary>
        /// 词语编号
        /// </summary>
        internal int Identity;
        /// <summary>
        /// 失败节点数组索引位置
        /// </summary>
        internal int LinkIndex;
        /// <summary>
        /// 子节点集合数组起始位置
        /// </summary>
        internal int NodeIndex;
        /// <summary>
        /// 文字
        /// </summary>
        internal char Character;
        /// <summary>
        /// 词语长度
        /// </summary>
        internal byte Length;
        /// <summary>
        /// 低2b表示失败节点类型，最高位表示是否最后一个节点
        /// </summary>
        internal byte LinkType;
        /// <summary>
        /// 初始化节点数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeIndex"></param>
        /// <param name="length"></param>
        /// <param name="isLastNode"></param>
        internal void Set(TreeNode node, int nodeIndex, byte length, bool isLastNode)
        {
            Identity = node.Identity;
            this.NodeIndex = nodeIndex;
            Character = (char)node.Character;
            this.Length = length;
            if (isLastNode) LinkType |= 0x80;
        }
        /// <summary>
        /// 建图
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="node"></param>
        /// <param name="nodeIndex"></param>
        /// <returns></returns>
        internal bool BuildGraph(GraphBuilder builder, GrahpHashNode node, out int nodeIndex)
        {
            setLinkTypeHashNode(builder, builder.GetLinkIndex(node.GetLinkCharacters(Character)));
            nodeIndex = this.NodeIndex;
            return (LinkType & 0x80) == 0;
        }
        /// <summary>
        /// 设置失败节点类型
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="linkIndex"></param>
        private void setLinkTypeHashNode(GraphBuilder builder, int linkIndex)
        {
            if (linkIndex != 0)
            {
                GrahpHashNode node = builder.HashNodes[linkIndex];
                if (Identity == 0)
                {
                    Identity = node.Identity;
                    if (Identity != 0) Length = 2;
                }
                if (node.NodeIndex != 0)
                {
                    LinkIndex = linkIndex;
                    LinkType |= (byte)LinkTypeEnum.HashNode;
                }
            }
        }
        /// <summary>
        /// 建图
        /// </summary>
        /// <param name="builder"></param>
        internal void BuildGraph(GraphBuilder builder)
        {
            GrahpNode[] nodes = builder.Nodes;
            for (int nodeIndex = NodeIndex, nextNodeIndex; nodes[nodeIndex].buildGraph(builder, ref this, out nextNodeIndex); ++nodeIndex)
            {
                if (nextNodeIndex != 0) builder.BuildGraphIndexs.Add(nodeIndex);
            }
        }
        /// <summary>
        /// 建图
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="parent"></param>
        /// <param name="nodeIndex"></param>
        /// <returns></returns>
        private bool buildGraph(GraphBuilder builder, ref GrahpNode parent, out int nodeIndex)
        {
            buildGraph(builder, ref parent);
            nodeIndex = this.NodeIndex;
            return (LinkType & 0x80) == 0;
        }
        /// <summary>
        /// 建图
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="parent"></param>
        private void buildGraph(GraphBuilder builder, ref GrahpNode parent)
        {
            if (parent.LinkIndex != 0)
            {
                int linkIndex;
                switch (parent.LinkType & 0x7f)
                {
                    case (byte)LinkTypeEnum.HashNode: linkIndex = builder.HashNodes[parent.LinkIndex].GetLinkIndex(builder.Nodes, Character); break;
                    case (byte)LinkTypeEnum.Node: linkIndex = builder.Nodes[parent.LinkIndex].getLinkIndex(builder, Character); break;
                    default: linkIndex = 0; break;
                }
                if (linkIndex != 0)
                {
                    GrahpNode node = builder.Nodes[linkIndex];
                    if (Identity == 0) node.copyLinkWord(ref this);
                    if (node.NodeIndex != 0)
                    {
                        LinkIndex = linkIndex;
                        LinkType |= (byte)LinkTypeEnum.Node;
                    }
                    else node.copyLinkIndex(ref this);
                    return;
                }
            }
            setLinkTypeHashNode(builder, builder.GetLinkIndex(((uint)Character << 16) | (uint)parent.Character));
        }
        /// <summary>
        /// 获取失败节点的词语信息
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void copyLinkWord(ref GrahpNode node)
        {
            node.Identity = Identity;
            node.Length = Length;
        }
        /// <summary>
        /// 获取失败节点的失败节点数据
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void copyLinkIndex(ref GrahpNode node)
        {
            if (LinkIndex != 0)
            {
                node.LinkIndex = LinkIndex;
                node.LinkType |= (byte)(LinkType & 0x7f);
            }
        }
        /// <summary>
        /// 检查字符是否匹配
        /// </summary>
        /// <param name="character"></param>
        /// <param name="isNext">是否需要匹配下一个节点</param>
        /// <returns></returns>
        internal bool Check(char character, out bool isNext)
        {
            if (Character == character)
            {
                isNext = false;
                return true;
            }
            isNext = Character < character && (LinkType & 0x80) == 0;
            return false;
        }
        /// <summary>
        /// 获取失败节点索引位置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        private int getLinkIndex(GraphBuilder builder, char character)
        {
            if (NodeIndex != 0)
            {
                int nodeIndex = NodeIndex;
                bool isNext;
                for (GrahpNode[] nodes = builder.Nodes; !nodes[nodeIndex].Check(character, out isNext); ++nodeIndex)
                {
                    if (!isNext) return 0;
                }
                return nodeIndex;
            }
            if (LinkIndex != 0)
            {
                switch (LinkType & 0x7f)
                {
                    case (byte)LinkTypeEnum.HashNode: return builder.HashNodes[LinkIndex].GetLinkIndex(builder.Nodes, Character);
                    case (byte)LinkTypeEnum.Node: return builder.Nodes[LinkIndex].getLinkIndex(builder, character);
                }
            }
            return 0;
        }
    }
}
