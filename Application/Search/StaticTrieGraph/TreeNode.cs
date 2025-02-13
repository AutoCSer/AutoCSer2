using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// Trie 树节点
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]

    internal struct TreeNode
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
        /// 节点信息是否有效
        /// </summary>
        private ushort isNode;
        /// <summary>
        /// 子节点集合
        /// </summary>
        internal TreeNode[] Nodes;
        /// <summary>
        /// 有效子节点数量
        /// </summary>
        internal int NodeCount
        {
            get { return GetCount(Nodes); }
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
        /// 初始化设置节点文字
        /// </summary>
        /// <param name="character"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(char character)
        {
            this.Character = character;
            Nodes = EmptyArray<TreeNode>.Array;
            isNode = 1;
        }
        /// <summary>
        /// 检查节点文字是否匹配
        /// </summary>
        /// <param name="character">匹配文字</param>
        /// <param name="newCharacter">是否新节点</param>
        /// <returns>是否匹配</returns>
        private bool checkAppend(char character, ref int newCharacter)
        {
            if (this.Character != character)
            {
                if (isNode != 0) return false;
                Set(character);
                newCharacter = 1;
            }
            return true;
        }
        /// <summary>
        /// 添加文字节点并返回节点编号
        /// </summary>
        /// <param name="character">添加文字</param>
        /// <param name="newCharacter">是否新节点</param>
        /// <param name="nodes">文字节点集合</param>
        /// <returns>节点编号</returns>
        internal int GetAppendIndex(char character, ref int newCharacter, out TreeNode[] nodes)
        {
            int index = GetAppendIndex(this.Nodes, character, ref newCharacter);
            if (index == this.Nodes.Length)
            {
                if (index == 0) this.Nodes = new TreeNode[1];
                else this.Nodes = AutoCSer.Common.GetCopyArray(this.Nodes, index << 1);
                this.Nodes[index].Set(character);
            }
            nodes = this.Nodes;
            return index;
        }

        /// <summary>
        /// 添加文字节点并返回节点编号
        /// </summary>
        /// <param name="nodes">文字节点集合</param>
        /// <param name="character">添加文字</param>
        /// <param name="newCharacter">是否新节点</param>
        /// <returns>节点编号</returns>
        internal static int GetAppendIndex(TreeNode[] nodes, char character, ref int newCharacter)
        {
            for (int index = 0; index != nodes.Length; ++index)
            {
                if (nodes[index].checkAppend(character, ref newCharacter)) return index;
            }
            newCharacter = 1;
            return nodes.Length;
        }
        /// <summary>
        /// 获取有效子节点数量
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        internal static int GetCount(TreeNode[] nodes)
        {
            int count = 0;
            foreach (TreeNode node in nodes)
            {
                if (node.isNode == 0) return count;
                ++count;
            }
            return count;
        }
    }
}
