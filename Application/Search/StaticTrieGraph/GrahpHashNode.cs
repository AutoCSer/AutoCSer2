using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// 字符串 Trie 图哈希节点（二级节点）
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct GrahpHashNode
    {
        /// <summary>
        /// 哈希数据起始位置，最高位表示是否最后一个节点
        /// </summary>
        internal uint HashNodeIndex;
        /// <summary>
        /// 子节点集合数组起始位置
        /// </summary>
        internal int NodeIndex;
        /// <summary>
        /// 前两个字符，也是 HashCode
        /// </summary>
        internal uint Characters;
        /// <summary>
        /// 词语编号
        /// </summary>
        internal int Identity;
        /// <summary>
        /// 首节点设置数据位置
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void TrySetHashIndex(TreeNode node)
        {
            if ((HashNodeIndex & int.MaxValue) == 0) HashNodeIndex |= (uint)node.Character;
        }
        /// <summary>
        /// 设置节点数据
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeIndex"></param>
        /// <param name="isLastHashIndex"></param>
        internal void Set(TreeNode node, int nodeIndex, bool isLastHashIndex)
        {
            if (isLastHashIndex) HashNodeIndex |= 0x80000000U;
            this.NodeIndex = nodeIndex;
            Characters = node.HashKey;
            Identity = node.Identity;
        }
        /// <summary>
        /// 获取失败节点哈希关键字
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal uint GetLinkCharacters(char character)
        {
            return ((uint)character << 16) | (Characters >> 16);
        }
        /// <summary>
        /// 检查前两个字符是否匹配
        /// </summary>
        /// <param name="characters"></param>
        /// <param name="isNext">是否需要匹配下一个节点</param>
        /// <returns></returns>
        internal bool Check(uint characters, out bool isNext)
        {
            if (Characters == characters)
            {
                isNext = false;
                return true;
            }
            isNext = Characters < characters && (HashNodeIndex & 0x80000000U) == 0;
            return false;
        }
        /// <summary>
        /// 获取失败节点索引位置
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        internal int GetLinkIndex(GrahpNode[] nodes, char character)
        {
            if (NodeIndex != 0)
            {
                int nodeIndex = NodeIndex;
                bool isNext;
                while (!nodes[nodeIndex].Check(character, out isNext))
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
