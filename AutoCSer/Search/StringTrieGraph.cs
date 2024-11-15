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
    public unsafe class StringTrieGraph : TrieGraph<char>, IDisposable
    {
        /// <summary>
        /// 分词字符类型数据访问锁
        /// </summary>
        protected readonly object charTypeDataLock;
        /// <summary>
        /// 分词字符类型数据
        /// </summary>
        internal Pointer CharTypeData;
        /// <summary>
        /// 任意字符，用于搜索哨岗
        /// </summary>
        internal char AnyHeadChar;
        /// <summary>
        /// 字符串 Trie 图
        /// </summary>
        protected StringTrieGraph()
        {
            CharTypeData = new Pointer(DefaultCharTypeData.Data, 0);
            charTypeDataLock = new object();
        }
        /// <summary>
        /// 字符串 Trie 图
        /// </summary>
        /// <param name="words">分词集合，词语中间不能存在空格</param>
        /// <param name="threadCount">建图并行线程数量，默认为 0 表示 CPU 逻辑处理器数量</param>
        public StringTrieGraph(IEnumerable<string> words, int threadCount = 0) : this()
        {
            int wordCount = 0;
            char* simplified = Simplified.Chars.Char;
            foreach (string word in words)
            {
                if (word?.Length > 1)
                {
                    Append(word, simplified).Length = word.Length;
                    ++wordCount;
                }
            }
            AnyHeadChar = buildGraph(wordCount, threadCount);
        }
        /// <summary>
        /// 添加分词
        /// </summary>
        /// <param name="word"></param>
        /// <param name="simplified"></param>
        internal TrieGraphNode<char> Append(string word, char* simplified)
        {
            if (CharTypeData.Data == DefaultCharTypeData.Data)
            {
                AutoCSer.Memory.Common.Copy(DefaultCharTypeData.Byte, (CharTypeData = Unmanaged.GetPointer(1 << 16, false)).Byte, 1 << 16);
            }
            TrieGraphNode<char> node = Boot;
            fixed (char* wordFixed = word)
            {
                char* start = wordFixed, end = wordFixed + word.Length;
                char letter = simplified[*wordFixed];
                CharTypeData.Byte[letter] |= (byte)WordTypeEnum.TrieGraphHead;
                do
                {
                    node = node.Create(letter);
                    if (letter != ' ') CharTypeData.Byte[letter] |= (byte)WordTypeEnum.TrieGraph;
                    if (++start != end) letter = simplified[*start];
                    else break;
                }
                while (true);
                CharTypeData.Byte[letter] |= (byte)WordTypeEnum.TrieGraphEnd;
            }
            return node;
        }
        /// <summary>
        /// 建图
        /// </summary>
        /// <param name="wordCount"></param>
        /// <param name="threadCount"></param>
        /// <returns></returns>
        protected char buildGraph(int wordCount, int threadCount)
        {
            if (wordCount != 0) BuildGraph(threadCount);
            if (Boot.Nodes != null)
            {
                foreach (char key in Boot.Nodes.Keys) return key;
            }
            Boot.Nodes = AutoCSer.Extensions.DictionaryCreator.CreateChar<TrieGraphNode<char>>();
            return (char)0;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Monitor.Enter(charTypeDataLock);
            try
            {
                if (CharTypeData.Data != DefaultCharTypeData.Data)
                {
                    Unmanaged.Free(ref CharTypeData);
                    CharTypeData = new Pointer(DefaultCharTypeData.Data, 0);
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
            Pointer charTypeData = this.CharTypeData;
            if (charTypeData.Data != DefaultCharTypeData.Data) this.CharTypeData = new Pointer(DefaultCharTypeData.Data, 0);
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
        /// 判断是否存在中文字符
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="charTypeData"></param>
        /// <returns></returns>
        internal static bool IsChineseCharacter(char* start, char* end, byte* charTypeData)
        {
            *end = '肖';
            while ((charTypeData[*start] & (byte)WordTypeEnum.Chinese) == 0) ++start;
            return start != end;
        }

        /// <summary>
        /// 默认分词字符类型数据
        /// </summary>
        internal static Pointer DefaultCharTypeData;
        static StringTrieGraph()
        {
            DefaultCharTypeData = AutoCSer.Search.Memory.Unmanaged.GetDefaultCharType();
            AutoCSer.Common.Clear(DefaultCharTypeData.ULong, (1 << 16) >> 3);
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
            Simplified.SetChinese(start = DefaultCharTypeData.Byte);
            start[' '] = 0;
            start['0'] |= (byte)WordTypeEnum.Number;
        }
    }
}
