using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// Trie 树节点
    /// </summary>
    internal sealed class TreeNode
    {
        /// <summary>
        /// 一二级节点哈希索引位置
        /// </summary>
        internal int HashIndex;
        /// <summary>
        /// 一二级节点关键字
        /// </summary>
        internal readonly uint HashKey;
        /// <summary>
        /// 词语编号
        /// </summary>
        internal int Identity;
        /// <summary>
        /// 文字
        /// </summary>
        internal int Character;
        /// <summary>
        /// 子节点集合
        /// </summary>
#if NetStandard21
        internal TreeNode?[] Nodes;
#else
        internal TreeNode[] Nodes;
#endif
        /// <summary>
        /// 有效子节点数量
        /// </summary>
        internal int NodeCount
        {
            get
            {
                int count = 0;
                foreach (var node in Nodes)
                {
                    if (node == null) break;
                    ++count;
                }
                return count;
            }
        }
        /// <summary>
        /// Trie 树节点
        /// </summary>
        /// <param name="hashKey"></param>
        internal TreeNode(uint hashKey)
        {
            this.HashKey = hashKey;
            Nodes = EmptyArray<TreeNode>.Array;
        }
        /// <summary>
        /// Trie 树节点
        /// </summary>
        /// <param name="hashKey"></param>
        /// <param name="identity"></param>
        internal TreeNode(uint hashKey, ref int identity)
        {
            this.HashKey = hashKey;
            this.Identity = ++identity;
            Nodes = EmptyArray<TreeNode>.Array;
        }
        /// <summary>
        /// Trie 树节点
        /// </summary>
        /// <param name="character"></param>
        private TreeNode(char character)
        {
            this.Character = character;
            Nodes = EmptyArray<TreeNode>.Array;
        }
        /// <summary>
        /// 设置词语编号
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>是否新词语</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool Set(ref int identity)
        {
            if (this.Identity == 0)
            {
                this.Identity = ++identity;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取子节点
        /// </summary>
        /// <param name="character"></param>
        /// <param name="nodeArraySize"></param>
        /// <returns></returns>
        internal TreeNode GetNode(char character, ref int nodeArraySize)
        {
            int index = 0;
            foreach (var node in Nodes)
            {
                if (node == null) break;
                if (node.Character == character) return node;
                ++index;
            }
            if (index == Nodes.Length) Nodes = index == 0 ? new TreeNode[sizeof(int)] : AutoCSer.Common.GetCopyArray(Nodes, index << 1);
            TreeNode newNode = new TreeNode(character);
            ++nodeArraySize;
            Nodes[index] = newNode;
            return newNode;
        }

        /// <summary>
        /// 获取节点文字
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static int getCharacter(TreeNode node)
        {
            return node.Character;
        }
        /// <summary>
        /// 获取节点文字
        /// </summary>
        internal static readonly Func<TreeNode, int> GetCharacter = getCharacter;
    }
}
