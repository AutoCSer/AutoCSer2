using AutoCSer.Extensions;
using System;
using System.Globalization;

namespace AutoCSer.CommandService.Search.StaticTrieGraph
{
    /// <summary>
    /// Trie 树初始化创建器
    /// </summary>
    internal sealed class TreeBuilder
    {
        /// <summary>
        /// 字符串 Trie 图节点
        /// </summary>
        private StaticTrieGraphNode node;
        /// <summary>
        /// 词语集合
        /// </summary>
        internal LeftArray<string> Words;
        /// <summary>
        /// 当前已分配词语编号
        /// </summary>
        internal int CurrentIdentity;
        /// <summary>
        /// 一级节点最小文字
        /// </summary>
        internal char MinCharacter;
        /// <summary>
        /// 一级节点最大文字
        /// </summary>
        private char maxCharacter;
        /// <summary>
        /// 二级节点累计数组大小
        /// </summary>
        internal int ArraySize2;
        /// <summary>
        /// 三级及以下节点累计数组大小
        /// </summary>
        internal int NodeArraySize;
        /// <summary>
        /// 一级节点
        /// </summary>
        private readonly TreeNode[][] nodes;
        /// <summary>
        /// 文字类型数据
        /// </summary>
        internal readonly WordTypeEnum[] WordTypes;
        /// <summary>
        /// Trie 树初始化创建器
        /// </summary>
        /// <param name="node"></param>
        internal unsafe TreeBuilder(StaticTrieGraphNode node)
        {
            this.node = node;
            nodes = new TreeNode[1 << 16][];
            MinCharacter = char.MaxValue;
            maxCharacter = char.MinValue;
            AutoCSer.Common.Fill(nodes, EmptyArray<TreeNode>.Array);
            Words.SetEmpty();
            WordTypes = new WordTypeEnum[1 << 16];
            fixed (WordTypeEnum* wordTypeFixed = WordTypes)
            {
                byte* start = (byte*)wordTypeFixed;
                char code = (char)0;
                do
                {
                    UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(code);
                    switch (category)
                    {
                        case UnicodeCategory.LowercaseLetter:
                        case UnicodeCategory.UppercaseLetter:
                        case UnicodeCategory.TitlecaseLetter:
                        case UnicodeCategory.ModifierLetter:
                            *start = (byte)WordTypeEnum.Letter;
                            break;
                        case UnicodeCategory.DecimalDigitNumber:
                        case UnicodeCategory.LetterNumber:
                        case UnicodeCategory.OtherNumber:
                            *start = (byte)WordTypeEnum.Number;
                            break;
                        case UnicodeCategory.OtherLetter:
                            //包括中文、日文、韩文字母等
                            *start = (byte)WordTypeEnum.OtherLetter;
                            break;
                        default:
                            if (code == '&' || code == '.' || code == '+' || code == '#' || code == '-' || code == '_') *start = (byte)WordTypeEnum.Keep;
                            break;
                    }
                    ++start;
                }
                while (++code != 0);
                Simplified.SetChinese(start = start = (byte*)wordTypeFixed);
                start[' '] = start[0] = 0;
                start['0'] |= (byte)WordTypeEnum.Number;
            }
        }
        /// <summary>
        /// 添加词语
        /// </summary>
        /// <param name="word"></param>
        internal unsafe void Append(string word)
        {
            fixed (WordTypeEnum* wordTypeFixed = WordTypes)
            fixed (char* wordFixed = word, replaceFixed = node.ReplaceChars)
            {
                byte* charType = (byte*)wordTypeFixed;
                char character = replaceFixed[*wordFixed], nextCharacter = replaceFixed[wordFixed[1]];
                charType[character] |= (byte)WordTypeEnum.TrieGraphHead;
                if (character != ' ') charType[character] |= (byte)WordTypeEnum.TrieGraph;
                if (nextCharacter != ' ') charType[nextCharacter] |= (byte)WordTypeEnum.TrieGraph;
                if (character < MinCharacter) MinCharacter = character;
                else if (character > maxCharacter) maxCharacter = character;
                TreeNode[] nodes = this.nodes[character];
                int newCharacter = 0, index = TreeNode.GetAppendIndex(nodes, nextCharacter, ref newCharacter);
                if (index == nodes.Length)
                {
                    if (index == 0) nodes = new TreeNode[sizeof(int)];
                    else nodes = AutoCSer.Common.GetCopyArray(nodes, index << 1);
                    nodes[index].Set(nextCharacter);
                }
                ArraySize2 += newCharacter;
                char* end = wordFixed + word.Length;
                TreeNode[] nextNodes = nodes;
                for (char* start = wordFixed + 2; start != end; ++start)
                {
                    newCharacter = 0;
                    index = nextNodes[index].GetAppendIndex(nextCharacter = replaceFixed[*start], ref newCharacter, out nextNodes);
                    NodeArraySize += newCharacter;
                    if (nextCharacter != ' ') charType[nextCharacter] |= (byte)WordTypeEnum.TrieGraph;
                }
                charType[replaceFixed[*(end - 1)]] |= (byte)WordTypeEnum.TrieGraphEnd;
                if (nextNodes[index].Set(ref CurrentIdentity)) Words.Add(word);
                this.nodes[character] = nodes;
            }
        }
        /// <summary>
        /// 获取 Trie 树转数组创建器
        /// </summary>
        /// <returns></returns>
        internal GraphBuilder GetGraphBuilder()
        {
            Range[] ranges = new Range[(int)maxCharacter - MinCharacter + 1];
            GraphBuilder builder = new GraphBuilder(this, ranges);
            for (int character = MinCharacter; character <= maxCharacter; ++character)
            {
                TreeNode[] nodes = this.nodes[character];
                int nodeCount = TreeNode.GetCount(nodes);
                if (nodeCount != 0)
                {
                    int index2 = builder.NodeIndex2, index = 0;
                    ranges[character - MinCharacter].Set(index2, index2 + nodeCount);
                    if (nodeCount > 7) nodes.QuickSort(nodeCount, TreeNode.GetCharacter);
                    do
                    {
                        builder.Node2(ref nodes[index]);
                    }
                    while (++index != nodeCount);
                }
            }
            builder.BuildGraph();
            return builder;
        }
    }
}
