using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Search
{
    /// <summary>
    /// 字符串 Trie 图
    /// </summary>
    public unsafe sealed class StringTrieGraph : TrieGraph<char>, IDisposable
    {
        /// <summary>
        /// 分词字符类型数据访问锁
        /// </summary>
        private readonly object charTypeDataLock;
        /// <summary>
        /// 分词字符类型数据
        /// </summary>
        private Pointer charTypeData;
        /// <summary>
        /// 任意字符，用于搜索哨岗
        /// </summary>
        internal readonly char AnyHeadChar;
        /// <summary>
        /// 字符串 Trie 图
        /// </summary>
        /// <param name="words">分词集合，词语中间不能存在空格</param>
        /// <param name="threadCount">建图并行线程数量，默认为 0 表示 CPU 逻辑处理器数量</param>
        public StringTrieGraph(IEnumerable<string> words, int threadCount = 0)
        {
            charTypeData = new Pointer(DefaultCharTypeData.Data, 0);
            charTypeDataLock = new object();
            int wordCount = 0;
            char* simplified = Simplified.Chars.Char;
            foreach (string word in words)
            {
                if (word != null && word.Length > 1)
                {
                    if (charTypeData.Data == DefaultCharTypeData.Data)
                    {
                        AutoCSer.Memory.Common.CopyNotNull(DefaultCharTypeData.Byte, (charTypeData = Unmanaged.GetPointer(1 << 16, false)).Byte, 1 << 16);
                    }
                    TrieGraphNode<char> node = Boot;
                    fixed (char* wordFixed = word)
                    {
                        char* start = wordFixed, end = wordFixed + word.Length;
                        char letter = simplified[*wordFixed];
                        charTypeData.Byte[letter] |= (byte)WordTypeEnum.TrieGraphHead;
                        do
                        {
                            node = node.Create(letter);
                            if (letter != ' ') charTypeData.Byte[letter] |= (byte)WordTypeEnum.TrieGraph;
                            if (++start != end) letter = simplified[*start];
                            else break;
                        }
                        while (true);
                        charTypeData.Byte[letter] |= (byte)WordTypeEnum.TrieGraphEnd;
                    }
                    node.Length = word.Length;
                    ++wordCount;
                }
            }
            if (wordCount != 0) BuildGraph(threadCount);
            if (Boot.Nodes == null) Boot.Nodes = AutoCSer.Extensions.DictionaryCreator.CreateChar<TrieGraphNode<char>>();
            else
            {
                foreach (char key in Boot.Nodes.Keys)
                {
                    AnyHeadChar = key;
                    break;
                }
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Monitor.Enter(charTypeDataLock);
            try
            {
                if (charTypeData.Data != DefaultCharTypeData.Data)
                {
                    Unmanaged.Free(ref charTypeData);
                    charTypeData = new Pointer(DefaultCharTypeData.Data, 0);
                }
            }
            finally { Monitor.Exit(charTypeDataLock); }
        }
        /// <summary>
        /// 获取分词字符类型数据
        /// </summary>
        /// <returns></returns>
        internal Pointer GetCharTypeData()
        {
            Monitor.Enter(charTypeDataLock);
            Pointer charTypeData = this.charTypeData;
            if (charTypeData.Data != DefaultCharTypeData.Data) this.charTypeData = new Pointer(DefaultCharTypeData.Data, 0);
            Monitor.Exit(charTypeDataLock);
            return charTypeData;
        }
        /// <summary>
        /// 创建绑定静态节点池的字符串 Trie 图
        /// </summary>
        /// <param name="isCopyCharTypeData">默认为 true 表示复制分词字符类型数据；字符串 Trie 图无需重用的场景可以设置为 false 释放字符串 Trie 图分词字符类型数据</param>
        /// <returns>绑定静态节点池的字符串 Trie 图</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public StaticStringTrieGraph CreateStaticGraph(bool isCopyCharTypeData = true)
        {
            return new StaticStringTrieGraph(this, isCopyCharTypeData);
        }

        /// <summary>
        /// 默认分词字符类型数据
        /// </summary>
        internal static Pointer DefaultCharTypeData;
        static StringTrieGraph()
        {
            DefaultCharTypeData = AutoCSer.Search.Memory.Unmanaged.GetDefaultCharType();
            AutoCSer.Common.Config.Clear(DefaultCharTypeData.ULong, (1 << 16) >> 3);
            byte* start = DefaultCharTypeData.Byte;
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
                    case UnicodeCategory.OtherLetter: *start = (byte)WordTypeEnum.OtherLetter; break;
                    default:
                        if (code == '&' || code == '.' || code == '+' || code == '#' || code == '-' || code == '_') *start = (byte)WordTypeEnum.Keep;
                        break;
                }
                ++start;
            }
            while (++code != 0);
            Simplified.SetChinese(start = DefaultCharTypeData.Byte);
            start[' '] = 0;
            start['0'] |= (byte)WordTypeEnum.Number;
        }
    }
}
