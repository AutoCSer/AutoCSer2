using AutoCSer;
using AutoCSer.CommandService.Search.StaticTrieGraph;
using AutoCSer.CommandService.StreamPersistenceMemoryDatabase;
using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.Search
{
    /// <summary>
    /// 字符串 Trie 图节点
    /// </summary>
    public sealed unsafe class StaticTrieGraphNode : ContextNode<IStaticTrieGraphNode, GraphData>, IStaticTrieGraphNode, ISnapshot<GraphData>, ISnapshot<string>, ISnapshot<BinarySerializeKeyValue<SubString, int>>
    {
        /// <summary>
        /// 未知词语长度
        /// </summary>
        private const int wordStringLength = 64 << 10;

        /// <summary>
        /// 替换文字集合
        /// </summary>
        internal readonly string ReplaceChars;
        /// <summary>
        /// 词语最大文字长度
        /// </summary>
        private readonly int maxWordSize;
        /// <summary>
        /// 是否支持单字符搜索结果
        /// </summary>
        private readonly bool isSingleCharacter;
        /// <summary>
        /// 分词类型
        /// </summary>
        private WordSegmentTypeEnum wordSegmentType;
        /// <summary>
        /// Trie 图序列化数据
        /// </summary>
        private GraphData graphData;
        /// <summary>
        /// 分词格式化文本
        /// </summary>
        private string formatedText;
        /// <summary>
        /// 分词格式化文本
        /// </summary>
        private char* formatTextFixed;
        /// <summary>
        /// 未知词语集合
        /// </summary>
        private readonly FragmentDictionary256<HashSubString, int> words;
        /// <summary>
        /// 文本分词结果
        /// </summary>
        private LeftArray<KeyValue<SubString, int>> wordSegments;
        /// <summary>
        /// 文本分词词语编号位图
        /// </summary>
        private ulong[] wordSegmentIdentityMap;
        /// <summary>
        /// 当前未知词语字符串
        /// </summary>
        private string wordString;
        /// <summary>
        /// 当前未知词语字符串位置
        /// </summary>
        private int wordStringIndex;
        /// <summary>
        /// Trie 树初始化创建器
        /// </summary>
#if NetStandard21
        private TreeBuilder? builder;
#else
        private TreeBuilder builder;
#endif
        /// <summary>
        /// 字符串 Trie 图节点
        /// </summary>
        /// <param name="maxWordSize">词语最大文字长度</param>
        /// <param name="replaceChars">替换文字集合</param>
        /// <param name="isSingleCharacter">默认为 true 表示支持单字符搜索结果，但是会造成占用大量内存；设置为 false 则应该设置中文词库，否则无法搜索中文内容</param>
#if NetStandard21
        public StaticTrieGraphNode(int maxWordSize, bool isSingleCharacter = true, string? replaceChars = null)
#else
        public StaticTrieGraphNode(int maxWordSize, bool isSingleCharacter = true, string replaceChars = null)
#endif
        {
            this.maxWordSize = Math.Max(maxWordSize, 2);
            ReplaceChars = replaceChars ?? Simplified.Chars;
            this.isSingleCharacter = isSingleCharacter;
            formatedText = string.Empty;
            wordSegments.SetEmpty();
            wordSegmentIdentityMap = EmptyArray<ulong>.Array;
            wordString = AutoCSer.Common.AllocateString(wordStringLength);
            words = new FragmentDictionary256<HashSubString, int>(); ;
        }
        /// <summary>
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>加载完毕替换的新节点</returns>
#if NetStandard21
        public override IStaticTrieGraphNode? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override IStaticTrieGraphNode StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            if (!graphData.IsGraph) builder = new TreeBuilder(this);
            return null;
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int ISnapshot<GraphData>.GetSnapshotCapacity(ref object customObject)
        {
            return graphData.IsGraph ? 1 : 0;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        SnapshotResult<GraphData> ISnapshot<GraphData>.GetSnapshotResult(GraphData[] snapshotArray, object customObject)
        {
            if (graphData.IsGraph)
            {
                snapshotArray[0] = graphData;
                return new SnapshotResult<GraphData>(1);
            }
            return new SnapshotResult<GraphData>(0);
        }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSetGraph(GraphData value)
        {
            graphData = value;
            builder = null;
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int ISnapshot<string>.GetSnapshotCapacity(ref object customObject)
        {
            if (graphData.IsGraph) return 0;
            return builder.notNull().Words.Length;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        SnapshotResult<string> ISnapshot<string>.GetSnapshotResult(string[] snapshotArray, object customObject)
        {
            if (graphData.IsGraph) return new SnapshotResult<string>(0);
            LeftArray<string> words = builder.notNull().Words;
            return new SnapshotResult<string>(snapshotArray, words, words.Length);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        void ISnapshot<string>.SetSnapshotResult(ref LeftArray<string> array, ref LeftArray<string> newArray) { }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSetWord(string value)
        {
            if (builder == null) builder = new TreeBuilder(this);
            builder.Append(value);
        }
        /// <summary>
        /// 获取快照数据集合容器大小，用于预申请快照数据容器
        /// </summary>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据集合容器大小</returns>
        int ISnapshot<BinarySerializeKeyValue<SubString, int>>.GetSnapshotCapacity(ref object customObject)
        {
            if (graphData.IsGraph) return words.Count;
            return 0;
        }
        /// <summary>
        /// 获取快照数据集合，如果数据对象可能被修改则应该返回克隆数据对象防止建立快照期间数据被修改
        /// </summary>
        /// <param name="snapshotArray">预申请的快照数据容器</param>
        /// <param name="customObject">自定义对象，用于预生成辅助数据</param>
        /// <returns>快照数据信息</returns>
        SnapshotResult<BinarySerializeKeyValue<SubString, int>> ISnapshot<BinarySerializeKeyValue<SubString, int>>.GetSnapshotResult(BinarySerializeKeyValue<SubString, int>[] snapshotArray, object customObject)
        {
            if (graphData.IsGraph)
            {
                SnapshotResult<BinarySerializeKeyValue<SubString, int>> result = new SnapshotResult<BinarySerializeKeyValue<SubString, int>>(words.Count, snapshotArray.Length);
                foreach (KeyValuePair<HashSubString, int> word in words.KeyValues) result.Add(snapshotArray, new BinarySerializeKeyValue<SubString, int>(word.Key.String, word.Value));
                return result;
            }
            return new SnapshotResult<BinarySerializeKeyValue<SubString, int>>(0);
        }
        /// <summary>
        /// 持久化之前重组快照数据
        /// </summary>
        /// <param name="array">预申请快照容器数组</param>
        /// <param name="newArray">超预申请快照数据</param>
        void ISnapshot<BinarySerializeKeyValue<SubString, int>>.SetSnapshotResult(ref LeftArray<BinarySerializeKeyValue<SubString, int>> array, ref LeftArray<BinarySerializeKeyValue<SubString, int>> newArray) { }
        /// <summary>
        /// 快照设置数据
        /// </summary>
        /// <param name="value">数据</param>
        public void SnapshotSetWordIdentity(BinarySerializeKeyValue<SubString, int> value)
        {
            getWord(ref value.Key, true);
            words.Add(value.Key, value.Value);
        }
        /// <summary>
        /// 获取未知词语字符串
        /// </summary>
        /// <param name="word">未知词语</param>
        /// <param name="isSnapshot"></param>
        private unsafe void getWord(ref SubString word, bool isSnapshot)
        {
            if (wordStringIndex + word.Length <= wordStringLength)
            {
                fixed (char* write = wordString) AutoCSer.Common.CopyTo(ref word, write + wordStringIndex);
                word.Set(wordString, wordStringIndex, word.Length);
                wordStringIndex += word.Length;
                return;
            }
            if (word.Length < wordStringIndex)
            {
                wordString = AutoCSer.Common.AllocateString(wordStringLength);
                fixed (char* write = wordString) AutoCSer.Common.CopyTo(ref word, write);
                word.Set(wordString, 0, wordStringIndex = word.Length);
                return;
            }
            if (!isSnapshot)
            {
                fixed (char* read = word.GetFixedBuffer()) word.Set(new string(read, word.Start, word.Length), 0, word.Length);
            }
        }
        /// <summary>
        /// 添加 Trie 图词语
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public AppendWordStateEnum AppendWord(string word)
        {
            if (word?.Length > 1)
            {
                if (word.Length > maxWordSize)
                {
                    if (!graphData.IsGraph)
                    {
                        builder.notNull().Append(word);
                        return AppendWordStateEnum.Success;
                    }
                    return AppendWordStateEnum.GraphBuilded;
                }
                return AppendWordStateEnum.WordSizeLimit;
            }
            return AppendWordStateEnum.WordSizeLess;
        }
        /// <summary>
        /// 建图
        /// </summary>
        /// <returns>Trie 图词语数量</returns>
        public int BuildGraph()
        {
            if(!graphData.IsGraph)
            {
                graphData.Build(builder.notNull());
                builder = null;
            }
            return graphData.WordIdentity;
        }
        /// <summary>
        /// 添加文本并返回词语编号集合 持久化前检查
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public ValueResult<int[]> GetAddTextIdentityBeforePersistence(string text)
        {
            if (graphData.IsGraph && !string.IsNullOrEmpty(text))
            {
                setWordSegmentType(WordSegmentTypeEnum.AddTextBeforePersistence);
                if (wordSegment(text, text.Length)) return getWordSegmentIdentityArray();
                return default(ValueResult<int[]>);
            }
            return EmptyArray<int>.Array;
        }
        /// <summary>
        /// 添加文本并返回词语编号集合
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int[] GetAddTextIdentity(string text)
        {
            if (graphData.IsGraph && !string.IsNullOrEmpty(text))
            {
                setWordSegmentType(WordSegmentTypeEnum.AddText);
                wordSegment(text, text.Length);
                return getWordSegmentIdentityArray();
            }
            return EmptyArray<int>.Array;
        }
        /// <summary>
        /// 获取分词结果词语编号集合
        /// </summary>
        /// <returns></returns>
        private int[] getWordSegmentIdentityArray()
        {
            if (wordSegments.Length != 0)
            {
                int count = wordSegments.Length;
                int[] identitys = AutoCSer.Common.GetUninitializedArray<int>(count);
                KeyValue<SubString, int>[] wordArray = wordSegments.Array;
                do
                {
                    --count;
                    identitys[count] = wordArray[count].Value;
                }
                while (count >= 0);
                return identitys;
            }
            return EmptyArray<int>.Array;
        }
        /// <summary>
        /// 获取查询词语编号集合（忽略未匹配词语）
        /// </summary>
        /// <param name="text">搜索文本内容</param>
        /// <returns></returns>
        public int[] GetWordSegmentIdentity(string text)
        {
            if (graphData.IsGraph && !string.IsNullOrEmpty(text))
            {
                setWordSegmentType(WordSegmentTypeEnum.QueryIdentity);
                wordSegment(text, text.Length);
                return getWordSegmentIdentityArray();
            }
            return EmptyArray<int>.Array;
        }
        /// <summary>
        /// 获取查询分词结果
        /// </summary>
        /// <param name="text">搜索文本内容</param>
        /// <returns></returns>
        public WordSegmentResult[] GetWordSegmentResult(string text)
        {
            if (graphData.IsGraph && !string.IsNullOrEmpty(text))
            {
                setWordSegmentType(WordSegmentTypeEnum.Query);
                wordSegment(text, text.Length);
                if (wordSegments.Length != 0)
                {
                    int count = wordSegments.Length;
                    WordSegmentResult[] results = AutoCSer.Common.GetUninitializedArray<WordSegmentResult>(count);
                    KeyValue<SubString, int>[] wordArray = wordSegments.Array;
                    do
                    {
                        --count;
                        results[count].Set(ref wordArray[count]);
                    }
                    while (count >= 0);
                    return results;
                }
            }
            return EmptyArray<WordSegmentResult>.Array;
        }
        /// <summary>
        /// 设置分词类型
        /// </summary>
        /// <param name="wordSegmentType"></param>
        private void setWordSegmentType(WordSegmentTypeEnum wordSegmentType)
        {
            int count = wordSegments.Length;
            if (count != 0)
            {
                switch (this.wordSegmentType)
                {
                    case WordSegmentTypeEnum.QueryIdentity:
                    case WordSegmentTypeEnum.AddTextBeforePersistence:
                    case WordSegmentTypeEnum.AddText:
                        KeyValue<SubString, int>[] wordArray = wordSegments.Array;
                        do
                        {
                            clearIdentityMap(wordArray[--count].Value);
                        }
                        while (count >= 0);
                        break;
                }
                wordSegments.Length = 0;
            }
            this.wordSegmentType = wordSegmentType;
        }
        /// <summary>
        /// 文本分词
        /// </summary>
        /// <param name="text"></param>
        /// <param name="formatLength"></param>
        /// <returns></returns>
        private bool wordSegment(string text, int formatLength)
        {
            if (formatedText.Length <= formatLength) formatedText = AutoCSer.Common.AllocateString(Math.Max(formatLength + 1, formatedText.Length << 1));
            fixed (WordTypeEnum* wordTypeFixed = graphData.WordTypes)
            fixed (char* textFixed = text, formatTextFixed = formatedText, replaceFixed = ReplaceChars)
            {
                char* read = textFixed, start = formatTextFixed, end = formatTextFixed + formatLength, segment, segmentEnd;
                do
                {
                    *start = replaceFixed[*read++];
                }
                while (++start != end);

                WordTypeEnum type, nextType, wordType;
                if (graphData.WordIdentity != 0)
                {
                    this.formatTextFixed = formatTextFixed;
                    start = formatTextFixed;
                    do
                    {
                        if (((type = wordTypeFixed[*start]) & WordTypeEnum.TrieGraphHead) == 0)
                        {
                            *end = graphData.MinCharacter;
                            do
                            {
                                if (((nextType = wordTypeFixed[*++start]) & WordTypeEnum.TrieGraphHead) != 0)
                                {
                                    if (start == end) goto TRIEGRAPHEND;
                                    if ((nextType & WordTypeEnum.Chinese) != 0
                                        || (type & nextType & (WordTypeEnum.OtherLetter | WordTypeEnum.Letter | WordTypeEnum.Number | WordTypeEnum.Keep)) == 0)
                                    {
                                        break;
                                    }
                                }
                                type = nextType;
                            }
                            while (true);
                        }
                        *end = ' ';
                        segment = start;
                        segmentEnd = (type & WordTypeEnum.TrieGraphEnd) == 0 ? start++ : ++start;
                        while (((type = wordTypeFixed[*start]) & WordTypeEnum.TrieGraph) != 0)
                        {
                            ++start;
                            if ((type & WordTypeEnum.TrieGraphEnd) != 0) segmentEnd = start;
                        }
                        graphData.Match(segment, segmentEnd, this);
                    }
                    while (start != end);
                }
            TRIEGRAPHEND:
                start = formatTextFixed;
                do
                {
                    type = wordTypeFixed[*start];
                    if ((type &= (WordTypeEnum.OtherLetter | WordTypeEnum.Letter | WordTypeEnum.Number | WordTypeEnum.Keep)) == 0)
                    {
                        *end = '0';
                        do
                        {
                            type = wordTypeFixed[*++start];
                            if ((type &= (WordTypeEnum.OtherLetter | WordTypeEnum.Letter | WordTypeEnum.Number | WordTypeEnum.Keep)) != 0)
                            {
                                if (start != end) break;
                                goto SINGLE;
                            }
                        }
                        while (true);
                    }
                    *end = ' ';
                    segment = start;
                    if ((type & WordTypeEnum.OtherLetter) == 0)
                    {
                        segmentEnd = start;
                        wordType = type;
                        for (nextType = wordTypeFixed[*++start]; (nextType &= (WordTypeEnum.Letter | WordTypeEnum.Number | WordTypeEnum.Keep)) != 0; nextType = wordTypeFixed[*++start])
                        {
                            if (type != nextType)
                            {
                                if (type != WordTypeEnum.Keep)
                                {
                                    if (!addWord((int)(segmentEnd - formatTextFixed), (int)(start - segmentEnd))) return false;
                                }
                                wordType |= nextType;
                                type = nextType;
                                segmentEnd = start;
                            }
                        }
                        if (segmentEnd != segment && type != WordTypeEnum.Keep)
                        {
                            if (!addWord((int)(segmentEnd - formatTextFixed), (int)(start - segmentEnd))) return false;
                        }
                        if (!addWord((int)(segment - formatTextFixed), (int)(start - segment))) return false;
                    }
                    else
                    {
                        while ((wordTypeFixed[*++start] & WordTypeEnum.OtherLetter) != 0) ;
                        if (!addWord((int)(segment - formatTextFixed), (int)(start - segment))) return false;
                    }
                }
                while (start != end);
            SINGLE:
                if (isSingleCharacter)
                {
                    start = formatTextFixed;
                    do
                    {
                        if ((wordTypeFixed[*start] & WordTypeEnum.Chinese) != 0)
                        {
                            if (!addWord((int)(start - formatTextFixed), 1)) return false;
                        }
                    }
                    while (++start != end);
                }
            }
            return true;
        }
        /// <summary>
        /// 清除词语编号位图
        /// </summary>
        /// <param name="identity"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void clearIdentityMap(int identity)
        {
            wordSegmentIdentityMap[(uint)identity >> 6] ^= 1UL << (identity & 63);
        }
        /// <summary>
        /// 添加文本分词结果
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private bool addWord(int start, int length)
        {
            if (length > 1 || isSingleCharacter)
            {
                wordSegments.PrepLength(1);
                SubString word = new SubString(start, length, formatedText);
                int identity;
                if (words.TryGetValue(word, out identity))
                {
                    if (wordSegmentType == WordSegmentTypeEnum.Query || trySetIdentityMap(identity))
                    {
                        wordSegments.Array[wordSegments.Length++].Set(word, identity);
                    }
                    return true;
                }
                switch (wordSegmentType)
                {
                    case WordSegmentTypeEnum.QueryIdentity: return true;
                    case WordSegmentTypeEnum.Query:
                        wordSegments.Array[wordSegments.Length++].Set(word, 0);
                        return true;
                    case WordSegmentTypeEnum.AddText:
                        getWord(ref word, false);
                        words.Add(word, identity = graphData.CurrentIdentity);
                        ++graphData.CurrentIdentity;
                        wordSegments.Array[wordSegments.Length++].Set(word, identity);
                        setIdentityMap(identity);
                        return true;
                    case WordSegmentTypeEnum.AddTextBeforePersistence: return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 尝试设置词语编号位图
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>新词语返回 true</returns>
        private bool trySetIdentityMap(int identity)
        {
            int index = (int)((uint)identity >> 6);
            ulong bit = 1UL << (identity & 63);
            if (index < wordSegmentIdentityMap.Length)
            {
                if ((wordSegmentIdentityMap[index] & bit) == 0)
                {
                    wordSegmentIdentityMap[index] |= bit;
                    return true;
                }
                return false;
            }
            newWordSegmentIdentityMap(index);
            wordSegmentIdentityMap[index] |= bit;
            return true;
        }
        /// <summary>
        /// 文本分词词语编号位图扩容
        /// </summary>
        /// <param name="index"></param>
        private void newWordSegmentIdentityMap(int index)
        {
            int length = Math.Max(wordSegmentIdentityMap.Length, sizeof(int));
            while (length <= index) length <<= 1;
            wordSegmentIdentityMap = AutoCSer.Common.GetCopyArray(wordSegmentIdentityMap, length);
        }
        /// <summary>
        /// 设置词语编号位图
        /// </summary>
        /// <param name="identity"></param>
        private void setIdentityMap(int identity)
        {
            int index = (int)((uint)identity >> 6);
            if (index >= wordSegmentIdentityMap.Length) newWordSegmentIdentityMap(index);
            wordSegmentIdentityMap[index] |= 1UL << (identity & 63);
        }
        /// <summary>
        /// 添加 Trie 图分词结果
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="endChar"></param>
        /// <param name="length"></param>
        internal void AddWord(int identity, char* endChar, int length)
        {
            wordSegments.PrepLength(1);
            if (wordSegmentType == WordSegmentTypeEnum.Query || trySetIdentityMap(identity))
            {
                wordSegments.Array[wordSegments.Length++].Set(new SubString((int)(endChar - formatTextFixed) + 1 - length, length, formatedText), identity);
            }
        }
        /// <summary>
        /// 添加 Trie 图分词结果
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="endChar"></param>
        /// <param name="length"></param>
        internal void AddWordEnd(int identity, char* endChar, int length)
        {
            wordSegments.PrepLength(1);
            if (wordSegmentType == WordSegmentTypeEnum.Query || trySetIdentityMap(identity))
            {
                wordSegments.Array[wordSegments.Length++].Set(new SubString((int)(endChar - formatTextFixed) - length, length, formatedText), identity);
            }
        }
    }
}
