﻿using System;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// Trie 图序列化数据
    /// </summary>
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct GraphData
    {
        /// <summary>
        /// 一级节点数组
        /// </summary>
        private Range[] ranges;
        /// <summary>
        /// 二级节点数组
        /// </summary>
        private GraphNode2[] nodeArray2;
        /// <summary>
        /// 三级及以下节点数组
        /// </summary>
        private GraphNode[] nodeArray;
        /// <summary>
        /// 文字类型数据
        /// </summary>
        internal WordTypeEnum[] WordTypes;
        /// <summary>
        /// Trie 图词语最大编号（词语数量）
        /// </summary>
        internal int WordCount;
        /// <summary>
        /// 当前分配未知词语编号
        /// </summary>
        internal int CurrentIdentity;
        /// <summary>
        /// 判断是否可以新增未知词语
        /// </summary>
        internal bool IsCurrentIdentity
        {
            get { return CurrentIdentity != WordCount; }
        }
        /// <summary>
        /// 一级节点最小文字
        /// </summary>
        internal char MinCharacter;
        /// <summary>
        /// 是否已经建图
        /// </summary>
        internal bool IsGraph;
        /// <summary>
        /// 建图
        /// </summary>
        /// <param name="treeBuilder"></param>
        internal void Build(TreeBuilder treeBuilder)
        {
            MinCharacter = treeBuilder.MinCharacter;
            if (MinCharacter != char.MaxValue)
            {
                GraphBuilder builder = treeBuilder.GetGraphBuilder();
                ranges = builder.Ranges;
                nodeArray2 = builder.NodeArray2;
                nodeArray = builder.NodeArray;
                WordCount = treeBuilder.CurrentIdentity;
            }
            else
            {
                ranges = EmptyArray<Range>.Array;
                nodeArray2 = EmptyArray<GraphNode2>.Array;
                nodeArray = EmptyArray<GraphNode>.Array;
                WordCount = 0;
            }
            WordTypes = treeBuilder.WordTypes;
            CurrentIdentity = -1;
            IsGraph = true;
        }
        /// <summary>
        /// 获取匹配的二级节点索引位置
        /// </summary>
        /// <param name="range"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        private int getIndex2(Range range, char character)
        {
            GraphNode2[] nodeArray = nodeArray2;
            int startIndex = range.StartIndex;
            switch (range.Length)
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
                    int endIndex = range.EndIndex, average;
                    do
                    {
                        if (character > nodeArray[average = startIndex + ((endIndex - startIndex) >> 1)].Character) startIndex = average + 1;
                        else endIndex = average;
                    }
                    while (startIndex != endIndex);
                    return startIndex != range.EndIndex && nodeArray[startIndex].Character == character ? startIndex : -1;
            }
            //for (int index = range.StartIndex; index != range.EndIndex; ++index)
            //{
            //    if (nodeArray2[index].Character == character)
            //    {
            //        if (index == range.StartIndex) return index;
            //        int newIndex = (index + range.StartIndex) >> 1;
            //        GraphNode2 node = nodeArray2[index];
            //        nodeArray2[index] = nodeArray2[newIndex];
            //        nodeArray2[newIndex] = node;
            //        return newIndex;
            //    }
            //}
            //return -1;
        }
        /// <summary>
        /// 从左到右匹配
        /// </summary>
        /// <param name="start">匹配起始位置</param>
        /// <param name="end">匹配结束位置</param>
        /// <param name="node">字符串 Trie 图节点</param>
        internal unsafe void Match(char* start, char* end, StaticTrieGraphNode node)
        {
            if (end - start > 1)
            {
                char* read = start;
                Range range;
                int index, nextIndex, identity, length = 0;
                do
                {
                    RANGE:
                    index = (int)*read - MinCharacter;
                    if (++read != end)
                    {
                        if ((uint)index < (uint)ranges.Length)
                        {
                            range = ranges[index];
                            if (range.EndIndex != 0)
                            {
                                index = getIndex2(range, *read);
                                if (index >= 0)
                                {
                                    identity = nodeArray2[index].Identity;
                                    if (identity != 0) node.AddWord(identity, read, 2);
                                    if (++read != end)
                                    {
                                        NODE2:
                                        index = nodeArray2[index].GetIndex(nodeArray, *read);
                                        if (index >= 0)
                                        {
                                            do
                                            {
                                                identity = nodeArray[index].GetIdentity(ref length);
                                                if (identity != 0) node.AddWord(identity, read, length);
                                                if (++read != end)
                                                {
                                                    NODE:
                                                    nextIndex = nodeArray[index].GetIndex(nodeArray, *read);
                                                    if (nextIndex >= 0) index = nextIndex;
                                                    else
                                                    {
                                                        switch (nodeArray[index].GetLinkType(ref index))
                                                        {
                                                            case LinkTypeEnum.None: goto RANGE;
                                                            case LinkTypeEnum.Range:
                                                                --read;
                                                                goto RANGE;
                                                            case LinkTypeEnum.Node2:
                                                                identity = nodeArray2[index].Identity;
                                                                if (identity != 0) node.AddWordEnd(identity, read, 2);
                                                                goto NODE2;
                                                            case LinkTypeEnum.Node:
                                                                identity = nodeArray[index].GetIdentity(ref length);
                                                                if (identity != 0) node.AddWordEnd(identity, read, length);
                                                                goto NODE;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    do
                                                    {
                                                        switch (nodeArray[index].GetLinkType(ref index))
                                                        {
                                                            case LinkTypeEnum.Node2:
                                                                identity = nodeArray2[index].Identity;
                                                                if (identity != 0) node.AddWordEnd(identity, read, 2);
                                                                return;
                                                            case LinkTypeEnum.Node:
                                                                identity = nodeArray[index].GetIdentity(ref length);
                                                                if (identity != 0) node.AddWordEnd(identity, read, length);
                                                                break;
                                                            default: return;
                                                        }
                                                    }
                                                    while (true);
                                                }
                                            }
                                            while (true);
                                        }
                                        else --read;
                                    }
                                    else return;
                                }
                            }
                        }
                    }
                    else return;
                }
                while (true);
            }
        }
    }
}
