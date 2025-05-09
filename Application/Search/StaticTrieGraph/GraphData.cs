using AutoCSer.Algorithm;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// Trie 图序列化数据
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct GraphData
    {
        /// <summary>
        /// 二级节点哈希数组
        /// </summary>
        internal GrahpHashNode[] HashNodes;
        /// <summary>
        /// 三级及以下节点数组
        /// </summary>
        internal GrahpNode[] Nodes;
        /// <summary>
        /// 文字类型数据
        /// </summary>
        internal WordTypeEnum[] WordTypes;
        /// <summary>
        /// 哈希取余
        /// </summary>
        internal IntegerDivision HashCapacityDivision;
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
        /// 哈希取余空间大小
        /// </summary>
        internal int HashCapacity;
        /// <summary>
        /// 任意 Trie 图首节点字符
        /// </summary>
        internal char AnyTrieHeadChar;
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
            HashCapacityDivision = new IntegerDivision(ReusableDictionary.GetCapacity(treeBuilder.Nodes.Count));
            if (treeBuilder.Nodes.Count != 0)
            {
                TreeNode[] treeNodes = treeBuilder.Nodes.Values.getArray();
                foreach (TreeNode treeNode in treeNodes) treeNode.HashIndex = (int)HashCapacityDivision.GetMod(treeNode.HashKey);
                if (treeNodes.Length > 1) treeNodes.QuickSort(treeNodes.Length, p => ((long)p.HashIndex << 32) + p.HashKey);
                Dictionary<int, TreeNodeHashCount> counts = DictionaryCreator.CreateInt<TreeNodeHashCount>();
                var hashCount = default(TreeNodeHashCount);
                int nodeIndex = 0, hashIndex = treeNodes[0].HashIndex, hashEndIndex;
                AnyTrieHeadChar = (char)treeNodes[0].HashKey;
                AutoCSer.SearchTree.Dictionary<int, int> freeIndexs = new SearchTree.Dictionary<int, int>();
                freeIndexs.Set(int.MaxValue, 1);
                for (int index = 1; index <= treeNodes.Length; ++index)
                {
                    if (index == treeNodes.Length || treeNodes[index].HashIndex != hashIndex)
                    {
                        KeyValue<int, int> freeIndex = freeIndexs.GetThanNodeKeyValue(hashIndex);
                        if (freeIndex.Value <= hashIndex && freeIndex.Key >= (hashEndIndex = hashIndex + index - nodeIndex))
                        {
                            if (freeIndex.Value != hashIndex)
                            {
                                if (freeIndex.Key == hashEndIndex) freeIndexs.Remove(freeIndex.Key);
                                else freeIndexs.Set(freeIndex.Key, hashEndIndex);
                                freeIndexs.Set(hashIndex, freeIndex.Value);
                            }
                            else
                            {
                                if (freeIndex.Key != hashEndIndex) freeIndexs.Set(freeIndex.Key, hashEndIndex);
                                else freeIndexs.Remove(freeIndex.Key);
                            }
                            do
                            {
                                treeNodes[nodeIndex].Character = hashIndex++;
                            }
                            while (++nodeIndex != index);
                        }
                        if ((hashEndIndex = index - nodeIndex) != 0)
                        {
                            if (!counts.TryGetValue(hashEndIndex, out hashCount)) counts.Add(hashEndIndex, hashCount = new TreeNodeHashCount(hashEndIndex));
                            hashCount.Nodes.Add(treeNodes, nodeIndex, hashEndIndex);
                            nodeIndex = index;
                        }
                        if (index != treeNodes.Length) hashIndex = treeNodes[index].HashIndex;
                    }
                }
                if (counts.Count != 0)
                {
                    AutoCSer.SearchTree.Set<long> countIndexs = new SearchTree.Set<long>();
                    countIndexs.Add(0);
                    freeIndexs.Remove(int.MaxValue, out hashEndIndex);
                    foreach (KeyValue<int, int> freeIndex in freeIndexs.KeyValues)
                    {
                        int count = freeIndex.Key - freeIndex.Value;
                        if (counts.TryGetValue(count, out hashCount))
                        {
                            if (hashCount.SetHashNodeIndex(freeIndex.Value))
                            {
                                counts.Remove(count);
                                if (counts.Count == 0) break;
                            }
                        }
                        else countIndexs.Add(((long)count << 32) | (long)freeIndex.Value);
                    }
                    if (counts.Count != 0)
                    {
                        TreeNodeHashCount[] countArray = counts.Values.getArray();
                        if (countArray.Length > 1) countArray.QuickSortDesc(countArray.Length, p => p.Count);
                        foreach (TreeNodeHashCount count in countArray)
                        {
                            long countKey = (long)count.Count << 32;
                            do
                            {
                                var node = countIndexs.Boot.notNull().GetThanNode(countKey);
                                if (node != null)
                                {
                                    long nodeKey = node.Key;
                                    if (count.SetHashNodeIndex((int)nodeKey)) countKey = 0;
                                    countIndexs.Remove(nodeKey);
                                    if (((nodeKey -= countKey) & (long.MaxValue ^ uint.MaxValue)) != 0) countIndexs.Add(nodeKey + count.Count);
                                }
                                else
                                {
                                    if (count.SetHashNodeIndex(hashEndIndex)) countKey = 0;
                                    hashEndIndex += count.Count;
                                }
                            }
                            while (countKey != 0);
                        }
                    }
                }
                else hashEndIndex = (int)HashCapacityDivision.Divisor;
                HashNodes = new GrahpHashNode[hashEndIndex];
                Nodes = new GrahpNode[treeBuilder.NodeArraySize + 1];
                nodeIndex = 1;
                hashEndIndex = 1;
                foreach (TreeNode node in treeNodes)
                {
                    HashNodes[hashIndex = node.HashIndex].TrySetHashIndex(node);
                    int nodeCount = node.NodeCount;
                    HashNodes[node.Character].Set(node, nodeCount != 0 ? nodeIndex : 0, hashEndIndex == treeNodes.Length || hashIndex != treeNodes[hashEndIndex].HashIndex);
                    if (nodeCount != 0) tree(node, nodeCount, ref nodeIndex, 3);
                    ++hashEndIndex;
                }
                new GraphBuilder(ref this, ref HashCapacityDivision).BuildGraph();
                WordCount = treeBuilder.CurrentIdentity;
            }
            else
            {
                HashNodes = new GrahpHashNode[HashCapacityDivision.Divisor];
                Nodes = EmptyArray<GrahpNode>.Array;
                WordCount = 0;
            }
            WordTypes = treeBuilder.WordTypes;
            CurrentIdentity = -1;
            IsGraph = true;
        }
        /// <summary>
        /// 生成树节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeCount"></param>
        /// <param name="nodeIndex"></param>
        /// <param name="length"></param>
        private void tree(TreeNode node, int nodeCount, ref int nodeIndex, int length)
        {
            int startIndex = nodeIndex, endIndex = (nodeIndex += nodeCount) - 1;
            var nodeArray = node.Nodes;
#pragma warning disable CS8620
            if (nodeCount > 1) nodeArray.QuickSort(nodeCount, TreeNode.GetCharacter);
#pragma warning restore CS8620
            foreach (var nextNode in nodeArray)
            {
                if (nextNode != null)
                {
                    nodeCount = nextNode.NodeCount;
                    Nodes[startIndex].Set(nextNode, nodeCount != 0 ? nodeIndex : 0, (byte)length, startIndex == endIndex);
                    if (nodeCount != 0) tree(nextNode, nodeCount, ref nodeIndex, length + 1);
                    ++startIndex;
                }
                else break;
            }
        }
        /// <summary>
        /// 从左到右匹配
        /// </summary>
        /// <param name="start">匹配起始位置</param>
        /// <param name="end">匹配结束位置</param>
        /// <param name="staticTrieGraphNode">字符串 Trie 图节点</param>
        internal unsafe void Match(char* start, char* end, StaticTrieGraphNode staticTrieGraphNode)
        {
            if (--end - start > 0)
            {
                GrahpHashNode hashNode;
                GrahpNode node;
                int hashNodeIndex, nodeIndex;
                uint characters;
                char character;
                bool isNext;
                do
                {
                GETHASHNODE:
                    hashNode = HashNodes[HashCapacityDivision.GetMod(characters = *(uint*)start)];
                    if ((hashNodeIndex = (int)(hashNode.HashNodeIndex & int.MaxValue)) != 0)
                    {
                        while (!HashNodes[hashNodeIndex].Check(characters, out isNext))
                        {
                            if (!isNext)
                            {
                                if (++start != end) goto GETHASHNODE;
                                return;
                            }
                            ++hashNodeIndex;
                        }
                        hashNode = HashNodes[hashNodeIndex];
                        if (hashNode.Identity != 0) staticTrieGraphNode.AddStartWord(hashNode.Identity, start, 2);
                        if (hashNode.NodeIndex != 0)
                        {
                            start += 2;
                        CHECKEND:
                            if (start <= end)
                            {
                                character = *start;
                            CHECKHASHNODE:
                                for (nodeIndex = hashNode.NodeIndex; !Nodes[nodeIndex].Check(character, out isNext); ++nodeIndex)
                                {
                                    if (!isNext)
                                    {
                                        --start;
                                        goto GETHASHNODE;
                                    }
                                }
                                node = Nodes[nodeIndex];
                                if (node.Identity != 0) staticTrieGraphNode.AddWord(node.Identity, start, node.Length);
                                if (node.NodeIndex == 0)
                                {
                                    if (node.LinkIndex == 0)
                                    {
                                        if (start != end) goto GETHASHNODE;
                                        return;
                                    }
                                    ++start;
                                    hashNode = HashNodes[node.LinkIndex];
                                    goto CHECKEND;
                                }
                                while (++start <= end)
                                {
                                    for (nodeIndex = node.NodeIndex, character = *start; !Nodes[nodeIndex].Check(character, out isNext); ++nodeIndex)
                                    {
                                        if (!isNext)
                                        {
                                            if (node.LinkIndex == 0)
                                            {
                                                if (start <= end)
                                                {
                                                    --start;
                                                    goto GETHASHNODE;
                                                }
                                                return;
                                            }
                                            switch (node.LinkType & 0x7f)
                                            {
                                                case (byte)LinkTypeEnum.HashNode:
                                                    hashNode = HashNodes[node.LinkIndex];
                                                    goto CHECKHASHNODE;
                                                case (byte)LinkTypeEnum.Node:
                                                    node = Nodes[node.LinkIndex];
                                                    nodeIndex = node.NodeIndex - 1;
                                                    break;
                                            }
                                        }
                                    }
                                    node = Nodes[nodeIndex];
                                    if (node.Identity != 0) staticTrieGraphNode.AddWord(node.Identity, start, node.Length);
                                    if (node.NodeIndex == 0)
                                    {
                                        if (node.LinkIndex == 0)
                                        {
                                            if (start != end) goto GETHASHNODE;
                                            return;
                                        }
                                        switch (node.LinkType & 0x7f)
                                        {
                                            case (byte)LinkTypeEnum.HashNode:
                                                ++start;
                                                hashNode = HashNodes[node.LinkIndex];
                                                goto CHECKEND;
                                            case (byte)LinkTypeEnum.Node: node = Nodes[node.LinkIndex]; break;
                                        }
                                    }
                                }
                            }
                            return;
                        }
                    }
                }
                while (++start != end);
            }
        }
    }
}
