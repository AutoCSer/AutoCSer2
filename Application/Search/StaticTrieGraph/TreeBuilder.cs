using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
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
        /// 三级及以下节点累计数组大小
        /// </summary>
        internal int NodeArraySize;
        /// <summary>
        /// 二级节点
        /// </summary>
        internal readonly Dictionary<uint, TreeNode> Nodes;
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
            Nodes = DictionaryCreator.CreateUInt<TreeNode>();
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
                uint key = ((uint)nextCharacter << 16) ^ (uint)character;
                var node = default(TreeNode);
                if (word.Length == 2)
                {
                    charType[nextCharacter] |= (byte)WordTypeEnum.TrieGraphEnd;
                    if (this.Nodes.TryGetValue(key, out node))
                    {
                        if (!node.Set(ref CurrentIdentity)) return;
                    }
                    else this.Nodes.Add(key, new TreeNode(key, ref CurrentIdentity));
                    Words.Add(word);
                    return;
                }
                if (!this.Nodes.TryGetValue(key, out node)) this.Nodes.Add(key, node = new TreeNode(key));
                char* start = wordFixed + 2, end = wordFixed + word.Length;
                do
                {
                    node = node.GetNode(nextCharacter = replaceFixed[*start], ref NodeArraySize);
                    if (nextCharacter != ' ') charType[nextCharacter] |= (byte)WordTypeEnum.TrieGraph;
                }
                while (++start != end);
                if (node.Set(ref CurrentIdentity)) Words.Add(word);
                charType[replaceFixed[*(end - 1)]] |= (byte)WordTypeEnum.TrieGraphEnd;
            }
        }
    }
}
