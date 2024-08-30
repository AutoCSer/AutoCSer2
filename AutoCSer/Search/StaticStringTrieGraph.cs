using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定静态节点池的字符串 Trie 图
    /// </summary>
    public unsafe sealed class StaticStringTrieGraph : StaticTrieGraph<char>
    {
        /// <summary>
        /// 分词字符类型数据访问锁
        /// </summary>
        private readonly object charTypeDataLock;
        /// <summary>
        /// 分词字符类型数据
        /// </summary>
        internal Pointer CharTypeData;
        /// <summary>
        /// 任意字符，用于搜索哨岗
        /// </summary>
        internal readonly char AnyHeadChar;
        /// <summary>
        /// 绑定静态节点池的字符串 Trie 图
        /// </summary>
        private StaticStringTrieGraph()
        {
            CharTypeData = new AutoCSer.Memory.Pointer(StringTrieGraph.DefaultCharTypeData.Data, 0);
        }
        /// <summary>
        /// 绑定静态节点池的字符串 Trie 图
        /// </summary>
        /// <param name="trieGraph">字符串 Trie 图</param>
        /// <param name="isCopyCharTypeData">是否复制分词字符类型数据</param>
        internal StaticStringTrieGraph(StringTrieGraph trieGraph, bool isCopyCharTypeData)
        {
            charTypeDataLock = new object();
            CharTypeData = new AutoCSer.Memory.Pointer(StringTrieGraph.DefaultCharTypeData.Data, 0);
            AnyHeadChar = trieGraph.AnyHeadChar;

            Dictionary<char, TrieGraphNode<char>> nodes = trieGraph.Boot.Nodes;
            if (nodes != null && nodes.Count != 0) new StaticStringTrieGraphBuilder().Create(this, trieGraph);
            CharTypeData = trieGraph.GetCharTypeData();
            if (isCopyCharTypeData)
            {
                bool isCharTypeData = false;
                Pointer copyCharTypeData = CharTypeData;
                try
                {
                    CharTypeData = Unmanaged.GetPointer(1 << 16, false);
                    isCharTypeData = true;
                    AutoCSer.Memory.Common.CopyNotNull(copyCharTypeData.Byte, CharTypeData.Byte, 1 << 16);
                }
                finally
                {
                    if (copyCharTypeData.Data != StringTrieGraph.DefaultCharTypeData.Data) Unmanaged.Free(ref copyCharTypeData);
                    if (!isCharTypeData) CharTypeData = new AutoCSer.Memory.Pointer(StringTrieGraph.DefaultCharTypeData.Data, 0);
                }
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            Monitor.Enter(charTypeDataLock);
            try
            {
                if (CharTypeData.Data != StringTrieGraph.DefaultCharTypeData.Data)
                {
                    Unmanaged.Free(ref CharTypeData);
                    CharTypeData = new Pointer(StringTrieGraph.DefaultCharTypeData.Data, 0);
                }
            }
            finally { Monitor.Exit(charTypeDataLock); }
        }
        /// <summary>
        /// 设置根节点
        /// </summary>
        /// <param name="boot"></param>
        internal void SetBoot(int boot)
        {
            this.boot = boot;
            nodes = Unmanaged.GetPointer((1 << 16) * sizeof(int), true);
            foreach (KeyValue<char, int> node in NodePool.Pool[boot >> ArrayPool.ArraySizeBit][boot & ArrayPool.ArraySizeAnd].Nodes) nodes.Int[node.Key] = node.Value;
        }
        /// <summary>
        /// 从左到右匹配
        /// </summary>
        /// <param name="start">匹配起始位置</param>
        /// <param name="end">匹配结束位置</param>
        /// <param name="matchs">匹配结果集合</param>
        internal void Match(char* start, char* end, ref LeftArray<Range> matchs)
        {
            if (nodes.Data != null && end - start > 1)
            {
                StaticTrieGraphNode<char> poolNode;
                StaticTrieGraphNode<char>[][] pool = NodePool.Pool;
                char* read = start;
                do
                {
                    int node = nodes.Int[*read];
                    if (++read != end)
                    {
                        if (node != 0)
                        {
                            node = pool[node >> ArrayPool.ArraySizeBit][node & ArrayPool.ArraySizeAnd].GetNode(*read);
                            if (node != 0)
                            {
                                do
                                {
                                    poolNode = pool[node >> ArrayPool.ArraySizeBit][node & ArrayPool.ArraySizeAnd];
                                    if (poolNode.Length == 0)
                                    {
                                        if (++read == end) return;
                                        node = poolNode.GetNode(*read);
                                    }
                                    else
                                    {
                                        matchs.PrepLength(1);
                                        matchs.Array[matchs.Length++].Set((int)(++read - start) - poolNode.Length, poolNode.Length);
                                        if (read == end) return;
                                        node = poolNode.Nodes.Length == 0 ? 0 : poolNode.GetNode(*read);
                                    }
                                    if (node == 0)
                                    {
                                        do
                                        {
                                            if ((node = poolNode.Link) == 0) break;
                                            poolNode = pool[node >> ArrayPool.ArraySizeBit][node & ArrayPool.ArraySizeAnd];
                                            if (poolNode.Length == 0) node = poolNode.GetNode(*read);
                                            else
                                            {
                                                matchs.PrepLength(1);
                                                matchs.Array[matchs.Length++].Set((int)(read - start) - poolNode.Length, poolNode.Length);
                                                node = poolNode.Nodes.Length == 0 ? 0 : poolNode.GetNode(*read);
                                            }
                                        }
                                        while (node == 0);
                                        if (node == 0) break;
                                    }
                                }
                                while (true);
                            }
                        }
                    }
                    else return;
                }
                while (true);
            }
        }

        /// <summary>
        /// 空 Trie 图
        /// </summary>
        internal static readonly StaticStringTrieGraph Null = new StaticStringTrieGraph();
    }
}
