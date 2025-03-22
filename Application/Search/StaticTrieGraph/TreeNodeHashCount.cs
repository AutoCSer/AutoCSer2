using System;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// 哈希索引冲突计数
    /// </summary>
    internal sealed class TreeNodeHashCount
    {
        /// <summary>
        /// 冲突数量
        /// </summary>
        internal readonly int Count;
        /// <summary>
        /// 冲突节点集合
        /// </summary>
        internal LeftArray<TreeNode> Nodes;
        /// <summary>
        /// 哈希索引冲突计数
        /// </summary>
        /// <param name="count">冲突数量</param>
        internal TreeNodeHashCount(int count)
        {
            this.Count = count;
            Nodes = new LeftArray<TreeNode>(count);
        }
        /// <summary>
        /// 设置哈希节点位置
        /// </summary>
        /// <param name="hashNodeIndex"></param>
        /// <returns></returns>
        internal bool SetHashNodeIndex(int hashNodeIndex)
        {
            TreeNode[] nodeArray = Nodes.Array;
            int endIndex = Nodes.Length, index = (Nodes.Length -= Count);
            do
            {
                nodeArray[index].Character = hashNodeIndex++;
            }
            while (++index != endIndex);
            return Nodes.Length == 0;
        }
    }
}
