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
        /// 当前转换二级节点索引位置
        /// </summary>
        internal int NodeIndex2;
        /// <summary>
        /// 当前转换三级及以下节点索引位置
        /// </summary>
        internal int NodeIndex;
        /// <summary>
        /// 一级节点集合
        /// </summary>
        internal readonly Range[] Ranges;
        /// <summary>
        /// 二级节点数组
        /// </summary>
        internal readonly GraphNode2[] NodeArray2;
        /// <summary>
        /// 三级及以下节点数组
        /// </summary>
        internal readonly GraphNode[] NodeArray;
        /// <summary>
        /// 一级节点最小文字
        /// </summary>
        private readonly char minCharacter;
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
        /// <param name="tree"></param>
        /// <param name="ranges"></param>
        internal GraphBuilder(TreeBuilder tree, Range[] ranges)
        {
            this.Ranges = ranges;
            minCharacter = tree.MinCharacter;
            NodeArray2 = new GraphNode2[tree.ArraySize2];
            NodeArray = new GraphNode[tree.NodeArraySize];
            NodeIndex2 = NodeIndex = 0;
            BuildGraphIndexs.SetEmpty();
            buildGraphIndexs.SetEmpty();
        }
        /// <summary>
        /// 二级节点转换
        /// </summary>
        /// <param name="node"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Node2(ref TreeNode node)
        {
            NodeArray2[NodeIndex2++].Set(this, ref node);
        }
        /// <summary>
        /// 建图
        /// </summary>
        internal void BuildGraph()
        {
            foreach (Range range in Ranges)
            {
                for (int index2 = range.StartIndex; index2 != range.EndIndex; NodeArray2[index2++].BuildGraph(this)) ;
            }
            while (BuildGraphIndexs.Count != 0)
            {
                LeftArray<int> indexs = BuildGraphIndexs;
                BuildGraphIndexs = buildGraphIndexs;
                buildGraphIndexs = indexs;
                BuildGraphIndexs.Length = 0;
                foreach (int index in indexs) NodeArray[index].BuildGraph(this);
            }
        }
        /// <summary>
        /// 二级节点获取失败节点集合
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal Range GetLinkRange(char character)
        {
            int index = (int)character - minCharacter;
            if ((uint)index < (uint)Ranges.Length) return Ranges[index];
            return default(Range);
        }
        /// <summary>
        /// 获取 Trie 图失败节点类型
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal LinkTypeEnum GetLinkType(char character)
        {
            int index = (int)character - minCharacter;
            if ((uint)index < (uint)Ranges.Length && Ranges[index].EndIndex != 0) return LinkTypeEnum.Range;
            return LinkTypeEnum.None;
        }
        /// <summary>
        /// 获取二级失败节点
        /// </summary>
        /// <param name="parentCharacter"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetLink2(char parentCharacter, char character)
        {
            Range range = Ranges[parentCharacter - minCharacter];
            return GetLink2(range.StartIndex, range.EndIndex, character);
        }
        /// <summary>
        /// 获取二级失败节点
        /// </summary>
        /// <param name="linkStartIndex"></param>
        /// <param name="linkEndIndex"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        internal int GetLink2(int linkStartIndex, int linkEndIndex, char character)
        {
            do
            {
                if (NodeArray2[linkStartIndex].Character == character) return linkStartIndex;
            }
            while (++linkStartIndex != linkEndIndex);
            return -1;
        }
    }
}
