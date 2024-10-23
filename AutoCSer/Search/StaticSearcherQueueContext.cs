using AutoCSer.Extensions;
using AutoCSer.Memory;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定结果池的分词搜索器 队列操作上下文（不允许并发操作，仅允许串行操作）
    /// </summary>
    /// <typeparam name="T">数据标识类型</typeparam>
    /// <typeparam name="RT">搜索结果类型</typeparam>
    public unsafe abstract class StaticSearcherQueueContext<T, RT>
        where T : IEquatable<T>
        where RT : class
    {
        /// <summary>
        /// 绑定结果池的分词搜索器
        /// </summary>
        private readonly StaticSearcher<T, RT> searcher;
        /// <summary>
        /// 分词字符类型集合
        /// </summary>
        private readonly byte* charTypeData;
        /// <summary>
        /// 分词格式化文本
        /// </summary>
        private string formatedText;
        /// <summary>
        /// 文本分词结果
        /// </summary>
        protected LeftArray<KeyValue<SubString, WordTypeEnum>> words;
        /// <summary>
        /// 分词索引位置
        /// </summary>
        private LeftArray<Range> matchs;
        /// <summary>
        /// 更新版本编号
        /// </summary>
        protected int version;
        /// <summary>
        /// 更新版本编号
        /// </summary>
        internal int Version { get { return version; } }
        /// <summary>
        /// 是否支持单字符搜索结果
        /// </summary>
        private readonly bool isSingleCharacter;
        /// <summary>
        /// 分词文本是否存在中文字符
        /// </summary>
        protected bool chineseCharacter;
        /// <summary>
        /// 文本分词位置结果
        /// </summary>
        private readonly ReusableDictionary<HashSubString, ResultIndexs> wordIndexs;
        /// <summary>
        /// 分词搜索结果集合
        /// </summary>
        protected readonly ReusableDictionary<HashSubString, RT> wordResults;
        /// <summary>
        /// 分词搜索结果集合
        /// </summary>
        protected LeftArray<KeyValue<SubString, RT>> wordResultArray;
        /// <summary>
        /// 分词搜索结果数量
        /// </summary>
        internal int WordResultCount { get { return wordResults.Count; } }
        /// <summary>
        /// 单字符搜索结果集合
        /// </summary>
        private readonly ReusableDictionary<char, HashSet<T>> charResults;
        /// <summary>
        /// 单字符搜索结果集合
        /// </summary>
        private LeftArray<KeyValue<char, HashSet<T>>> charResultArray;
        /// <summary>
        /// 单字符搜索结果数量
        /// </summary>
        internal int CharResultCount { get { return charResults.Count; } }
        /// <summary>
        /// 未匹配分词数量
        /// </summary>
        internal int LessResultCount { get { return words.Count; } }
        /// <summary>
        /// 未匹配分词集合
        /// </summary>
        internal IEnumerable<SubString> LessWords
        {
            get
            {
                int count = words.Length;
                foreach (KeyValue<SubString, WordTypeEnum> word in words)
                {
                    yield return word.Key;
                    if (--count == 0) break;
                }
            }
        }
        /// <summary>
        /// 匹配位置位图
        /// </summary>
        private Pointer matchMapData;
        /// <summary>
        /// 匹配位置位图
        /// </summary>
        private BitMap matchMap;
        /// <summary>
        /// 队列操作上下文
        /// </summary>
        /// <param name="searcher">绑定结果池的分词搜索器</param>
        protected StaticSearcherQueueContext(StaticSearcher<T, RT> searcher)
        {
            this.searcher = searcher;
            isSingleCharacter = searcher.IsSingleCharacter;
            charTypeData = searcher.TrieGraph.CharTypeData.Byte;
            formatedText = string.Empty;
            wordIndexs = new ReusableDictionary<HashSubString, ResultIndexs>();
            words = new LeftArray<KeyValue<SubString, WordTypeEnum>>(0);
            matchs = new LeftArray<Range>(0);
            wordResults = new ReusableDictionary<HashSubString, RT>();
            charResults = new ReusableDictionary<char, HashSet<T>>();
            wordResultArray = new LeftArray<KeyValue<SubString, RT>>(0);
            charResultArray = new LeftArray<KeyValue<char, HashSet<T>>>(0);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Dispose()
        {
            Unmanaged.Free(ref matchMapData);
        }
        /// <summary>
        /// 添加新的数据
        /// </summary>
        /// <param name="key">添加关键字</param>
        /// <param name="text">添加搜索文本内容</param>
        public void Add(T key, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                ++version;
                string removeText;
                if (searcher.Texts.TryGetValue(key, out removeText))
                {
                    if (removeText.Length == text.Length && Simplified.IsMatch(text, removeText)) return;
                    searcher.Texts.Remove(key);
                    wordSegmenter(removeText, removeText.Length);
                    searcher.Remove(key, ref words, new SubString(0, chineseCharacter ? text.Length : 0, formatedText));
                }
                wordSegmenter(text, text.Length);
                if (chineseCharacter || words.Length != 0)
                {
                    wordIndexs.Empty();
                    if (words.Length != 0) setWordResults();
                    searcher.Add(key, text, wordIndexs, ref words, chineseCharacter ? formatedText : null);
                }
            }
            else Remove(key);
        }
        /// <summary>
        /// 根据关键字删除数据
        /// </summary>
        /// <param name="key">删除关键字</param>
        public void Remove(T key)
        {
            string text;
            if (searcher.Texts.Remove(key, out text))
            {
                ++version;
                wordSegmenter(text, text.Length);
                searcher.Remove(key, ref words, new SubString(0, chineseCharacter ? text.Length : 0, formatedText));
            }
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void Clear()
        {
            searcher.Clear();
        }
        /// <summary>
        /// 获取文本分词结果
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public LeftArray<KeyValue<SubString, WordTypeEnum>> WordSegment(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                ++version;
                wordSegmenter(text, text.Length);
                return words;
            }
            return new LeftArray<KeyValue<SubString, WordTypeEnum>>(0);
        }
        /// <summary>
        /// 文本分词
        /// </summary>
        /// <param name="text"></param>
        /// <param name="formatLength"></param>
        protected unsafe void wordSegmenter(string text, int formatLength)
        {
            if (formatedText.Length <= formatLength) formatedText = AutoCSer.Common.Config.AllocateString(Math.Max(formatLength + 1, formatedText.Length << 1));
            fixed (char* textFixed = text, formatTextFixed = formatedText)
            {
                Simplified.FormatNotEmpty(textFixed, formatTextFixed, formatLength);
                words.Length = matchs.Length = 0;
                char* start = formatTextFixed, end = formatTextFixed + formatLength;
                byte type, nextType, wordType;
                chineseCharacter = isSingleCharacter && AutoCSer.Search.StringTrieGraph.IsChineseCharacter(formatTextFixed, end, charTypeData);
                if (charTypeData != StringTrieGraph.DefaultCharTypeData.Byte)
                {
                    StaticStringTrieGraph trieGraph = searcher.TrieGraph;
                    int count, startIndex;
                    char trieGraphHeadChar = trieGraph.AnyHeadChar;
                    do
                    {
                        if (((type = charTypeData[*start]) & (byte)WordTypeEnum.TrieGraphHead) == 0)
                        {
                            *end = trieGraphHeadChar;
                            do
                            {
                                if (((nextType = charTypeData[*++start]) & (byte)WordTypeEnum.TrieGraphHead) != 0)
                                {
                                    if (start == end) goto TRIEGRAPHEND;
                                    if ((nextType & (byte)WordTypeEnum.Chinese) != 0
                                        || (type & nextType & ((byte)WordTypeEnum.OtherLetter | (byte)WordTypeEnum.Letter | (byte)WordTypeEnum.Number | (byte)WordTypeEnum.Keep)) == 0)
                                    {
                                        break;
                                    }
                                }
                                type = nextType;
                            }
                            while (true);
                        }
                        *end = ' ';
                        char* segment = start, segmentEnd = (type & (byte)WordTypeEnum.TrieGraphEnd) == 0 ? start++ : ++start;
                        while (((type = charTypeData[*start]) & (byte)WordTypeEnum.TrieGraph) != 0)
                        {
                            ++start;
                            if ((type & (byte)WordTypeEnum.TrieGraphEnd) != 0) segmentEnd = start;
                        }
                        if (segment != segmentEnd)
                        {
                            matchs.Length = 0;
                            trieGraph.Match(segment, segmentEnd, ref matchs);
                            if ((count = matchs.Length) == 0) segmentEnd = segment;
                            else
                            {
                                startIndex = (int)(segment - formatTextFixed);
                                foreach (Range value in matchs.Array)
                                {
                                    addWord(value.StartIndex + startIndex, value.EndIndex, WordTypeEnum.TrieGraph);
                                    if (--count == 0) break;
                                }
                            }
                        }
                    }
                    while (start != end);
                TRIEGRAPHEND:
                    start = formatTextFixed;
                }
                do
                {
                    type = charTypeData[*start];
                    if ((type &= ((byte)WordTypeEnum.OtherLetter | (byte)WordTypeEnum.Letter | (byte)WordTypeEnum.Number | (byte)WordTypeEnum.Keep)) == 0)
                    {
                        *end = '0';
                        do
                        {
                            type = charTypeData[*++start];
                            if ((type &= ((byte)WordTypeEnum.OtherLetter | (byte)WordTypeEnum.Letter | (byte)WordTypeEnum.Number | (byte)WordTypeEnum.Keep)) != 0)
                            {
                                if (start != end) break;
                                return;
                            }
                        }
                        while (true);
                    }
                    *end = ' ';
                    char* segment = start;
                    if ((type & (byte)WordTypeEnum.OtherLetter) == 0)
                    {
                        char* word = start;
                        wordType = type;
                        for (nextType = charTypeData[*++start]; (nextType &= ((byte)WordTypeEnum.Letter | (byte)WordTypeEnum.Number | (byte)WordTypeEnum.Keep)) != 0; nextType = charTypeData[*++start])
                        {
                            if (type != nextType)
                            {
                                if (type != (byte)WordTypeEnum.Keep) addWord((int)(word - formatTextFixed), (int)(start - word), (WordTypeEnum)type);
                                wordType |= nextType;
                                type = nextType;
                                word = start;
                            }
                        }
                        if (word != segment && type != (byte)WordTypeEnum.Keep) addWord((int)(word - formatTextFixed), (int)(start - word), (WordTypeEnum)type);
                        addWord((int)(segment - formatTextFixed), (int)(start - segment), (WordTypeEnum)wordType);
                    }
                    else
                    {
                        while ((charTypeData[*++start] & (byte)WordTypeEnum.OtherLetter) != 0) ;
                        addWord((int)(segment - formatTextFixed), (int)(start - segment), WordTypeEnum.OtherLetter);
                    }
                }
                while (start != end);
            }
        }
        /// <summary>
        /// 添加文本分词结果
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="wordType"></param>
        private void addWord(int start, int length, WordTypeEnum wordType)
        {
            if (length > 1 || isSingleCharacter)
            {
                words.PrepLength(1);
                words.Array[words.Length++].Set(new SubString(start, length, formatedText), wordType);
            }
        }
        /// <summary>
        /// 设置分词位置结果（默认不记录位置信息）
        /// </summary>
        protected virtual void setWordResults()
        {
            int count = words.Length;
            KeyValue<SubString, WordTypeEnum>[] wordArray = words.Array;
            words.Length = 0;
            foreach (KeyValue<SubString, WordTypeEnum> word in wordArray)
            {
                if (word.Key.Length != 1) wordIndexs.Set(word.Key, new ResultIndexs(word.Value, word.Key.Start));
                else wordArray[words.Length++] = word;
                if (--count == 0) break;
            }
        }
        /// <summary>
        /// 设置分词位置结果
        /// </summary>
        protected void setResultIndexs()
        {
            int count = words.Length;
            KeyValue<SubString, WordTypeEnum>[] wordArray = words.Array;
            words.Length = 0;
            ResultIndexs indexs;
            foreach (KeyValue<SubString, WordTypeEnum> word in wordArray)
            {
                if (word.Key.Length != 1)
                {
                    HashSubString wordKey = word.Key;
                    if (wordIndexs.TryGetValue(ref wordKey, out indexs))
                    {
                        indexs.Add(word.Key.Start);
                        wordIndexs.Set(ref wordKey, indexs);
                    }
                    else
                    {
                        indexs.Set(word.Value, word.Key.Start);
                        wordIndexs.Set(ref wordKey, indexs);
                    }
                }
                else wordArray[words.Length++] = word;
                if (--count == 0) break;
            }
            foreach (ResultIndexs indexArray in wordIndexs.Values) indexArray.SortIndexs();
        }
        /// <summary>
        /// 关键字搜索
        /// </summary>
        /// <param name="text">关键字文本</param>
        /// <param name="size">关键字文本长度</param>
        protected void search(string text, int size)
        {
            ++version;
            wordResults.Empty();
            charResults.Empty();
            words.Length = 0;
            if (!string.IsNullOrEmpty(text))
            {
                if (size <= 0 || size > text.Length) size = text.Length;
                wordSegmenter(text, size);
                int count = words.Length;
                if (count > 1) words.Sort(wordSortHandle);
                if (chineseCharacter || count > 1)
                {
                    checkMatchMap(size);
                    if (count != 0)
                    {
                        KeyValue<SubString, WordTypeEnum>[] wordArray = words.Array;
                        words.Length = 0;
                        foreach (KeyValue<SubString, WordTypeEnum> word in wordArray)
                        {
                            if (word.Key.Length != 1)
                            {
                                HashSubString wordKey = word.Key;
                                RT counter = searcher.GetResult(ref wordKey);
                                if (counter != null)
                                {
                                    bool isWord = false;
                                    int index = word.Key.Start, endIndex = index + word.Key.Length;
                                    do
                                    {
                                        isWord |= matchMap.IsSet(index);
                                    }
                                    while (++index != endIndex);
                                    if (isWord) wordResults.Set(wordKey, counter);
                                }
                                else wordArray[words.Length++] = word;
                            }
                            else
                            {
                                char code = word.Key[0];
                                HashSet<T> keys = searcher.GetResult(code);
                                if (keys != null)
                                {
                                    if (matchMap.IsSet(word.Key.Start)) charResults.Set(code, keys);
                                }
                                else wordArray[words.Length++] = word;
                            }
                            if (--count == 0) break;
                        }
                    }
                    if (chineseCharacter)
                    {
                        fixed (char* formatTextFixed = formatedText)
                        {
                            byte* charTypeData = searcher.TrieGraph.CharTypeData.Byte;
                            char* start = formatTextFixed, end = formatTextFixed + size;
                            do
                            {
                                char code = *start;
                                if ((charTypeData[code] & (byte)WordTypeEnum.Chinese) != 0)
                                {
                                    int index = (int)(start - formatTextFixed);
                                    if (matchMap.Get(index) == 0)
                                    {
                                        HashSet<T> keys = searcher.GetResult(code);
                                        if (keys != null)
                                        {
                                            charResults.Set(code, keys);
                                            matchMap.Set(index);
                                        }
                                        else words.Add(new KeyValue<SubString, WordTypeEnum>(new SubString(index, 1, formatedText), WordTypeEnum.Chinese));
                                    }
                                }
                            }
                            while (++start != end);
                        }
                    }
                    if ((count = words.Length) != 0)
                    {
                        KeyValue<SubString, WordTypeEnum>[] wordArray = words.Array;
                        words.Length = 0;
                        foreach (KeyValue<SubString, WordTypeEnum> word in wordArray)
                        {
                            bool isLess = false;
                            int index = word.Key.Start, endIndex = index + word.Key.Length;
                            do
                            {
                                isLess |= matchMap.IsSet(index);
                            }
                            while (++index != endIndex);
                            if(isLess) wordArray[words.Length++] = word;
                            if (--count == 0) break;
                        }
                    }
                }
                else if (count != 0)
                {
                    KeyValue<SubString, WordTypeEnum> word = words.Array[0];
                    if (word.Key.Length != 1)
                    {
                        HashSubString wordKey = word.Key;
                        RT counter = searcher.GetResult(ref wordKey);
                        if (counter != null)
                        {
                            wordResults.Set(wordKey, counter);
                            words.Length = 0;
                        }
                    }
                    else
                    {
                        char code = word.Key[0];
                        HashSet<T> keys = searcher.GetResult(code);
                        if (keys != null)
                        {
                            charResults.Set(code, keys);
                            words.Length = 0;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 检测匹配位置位图
        /// </summary>
        private void checkMatchMap(int formatLength)
        {
            int matchMapSize = (formatLength + 7) >> 3, matchMapDataSize = Math.Max((int)((uint)matchMapSize).upToPower2(), 8);
            if (matchMapData.ByteSize < matchMapDataSize)
            {
                Unmanaged.Free(ref matchMapData);
                matchMapData = Unmanaged.GetPointer(matchMapDataSize, false);
            }
            matchMap.Set(matchMapData.ULong, (matchMapSize + 7) >> 3);
        }
        /// <summary>
        /// 获取单字符搜索结果
        /// </summary>
        /// <param name="version">更新版本编号</param>
        /// <param name="results">按结果数量排序的单字符搜索结果</param>
        /// <returns>返回 false 表示检测到并发操作冲突错误</returns>
        internal bool GetCharResults(int version, out LeftArray<KeyValue<char, HashSet<T>>> results)
        {
            if (version == this.version)
            {
                charResultArray.Length = 0;
                if (charResults.Count != 0)
                {
                    charResultArray.PrepLength(charResults.Count);
                    foreach (KeyValue<char, HashSet<T>> keys in charResults.KeyValues) charResultArray.Add(keys);
                    if (charResultArray.Length > 1) charResultArray.Sort(charResultSortHandle);
                }
                results = charResultArray;
                return true;
            }
            results = new LeftArray<KeyValue<char, HashSet<T>>>(0);
            return false;
        }

        /// <summary>
        /// 文本分词结果按照分词长度逆序排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int wordSort(KeyValue<SubString, WordTypeEnum> left, KeyValue<SubString, WordTypeEnum> right)
        {
            int value = right.Key.Length - left.Key.Length;
            return value == 0 ? left.Key.Start - right.Key.Start : value;
        }
        /// <summary>
        /// 文本分词结果按照分词长度逆序排序
        /// </summary>
        protected static readonly Func<KeyValue<SubString, WordTypeEnum>, KeyValue<SubString, WordTypeEnum>, int> wordSortHandle = wordSort;
        /// <summary>
        /// 单字符搜索结果按结果数量排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int charResultSort(KeyValue<char, HashSet<T>> left, KeyValue<char, HashSet<T>> right)
        {
            return left.Value.Count - right.Value.Count;
        }
        /// <summary>
        /// 单字符搜索结果按结果数量排序
        /// </summary>
        private static readonly Func<KeyValue<char, HashSet<T>>, KeyValue<char, HashSet<T>>, int> charResultSortHandle = charResultSort;
    }
    /// <summary>
    /// 绑定结果池的分词搜索器 队列操作上下文（不允许并发操作，仅允许串行操作）
    /// </summary>
    /// <typeparam name="T">数据标识类型</typeparam>
    public class StaticSearcherQueueContext<T> : StaticSearcherQueueContext<T, HashSet<T>>
        where T : IEquatable<T>
    {
        /// <summary>
        /// 队列操作上下文
        /// </summary>
        /// <param name="searcher">绑定结果池的分词搜索器</param>
        internal StaticSearcherQueueContext(StaticSearcher<T> searcher) : base(searcher) { }
        /// <summary>
        /// 关键字搜索
        /// </summary>
        /// <param name="text">关键字文本</param>
        /// <param name="size">搜索关键字文本长度，默认为 0 表示不限长度，否则取关键字文本字串</param>
        /// <returns>搜索结果</returns>
        public SearchResult<T> Search(string text, int size = 0)
        {
            search(text, size);
            return new SearchResult<T>(this);
        }
        /// <summary>
        /// 获取分词搜索结果
        /// </summary>
        /// <param name="version">更新版本编号</param>
        /// <param name="results">按结果数量排序的分词搜索结果</param>
        /// <returns>返回 false 表示检测到并发操作冲突错误</returns>
        internal bool GetWordResults(int version, out LeftArray<KeyValue<SubString, HashSet<T>>> results)
        {
            if (version == this.version)
            {
                wordResultArray.Length = 0;
                if (wordResults.Count != 0)
                {
                    wordResultArray.PrepLength(wordResults.Count);
                    foreach (KeyValue<HashSubString, HashSet<T>> keys in wordResults.KeyValues) wordResultArray.Add(new KeyValue<SubString, HashSet<T>>(keys.Key.String, keys.Value));
                    if (wordResultArray.Length > 1) wordResultArray.Sort(wordResultSortHandle);
                }
                results = wordResultArray;
                return true;
            }
            results = new LeftArray<KeyValue<SubString, HashSet<T>>>(0);
            return false;
        }

        /// <summary>
        /// 分词搜索结果按结果数量排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int wordResultSort(KeyValue<SubString, HashSet<T>> left, KeyValue<SubString, HashSet<T>> right)
        {
            int value = left.Value.Count - right.Value.Count;
            if (value == 0) return left.Key.Start - right.Key.Start;
            return value;
        }
        /// <summary>
        /// 分词搜索结果按结果数量排序
        /// </summary>
        private static readonly Func<KeyValue<SubString, HashSet<T>>, KeyValue<SubString, HashSet<T>>, int> wordResultSortHandle = wordResultSort;
    }
}
