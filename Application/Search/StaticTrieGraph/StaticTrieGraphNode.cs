using AutoCSer;
using AutoCSer.Algorithm;
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
    public sealed unsafe class StaticTrieGraphNode : ContextNode<IStaticTrieGraphNode>, IStaticTrieGraphNode, IEnumerableSnapshot<GraphData>, IEnumerableSnapshot<string>, IEnumerableSnapshot<BinarySerializeKeyValue<SubString, int>>
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
        /// Trie 词语最大文字长度
        /// </summary>
        private readonly byte maxTrieWordSize;
        /// <summary>
        /// 未知词语最大文字长度
        /// </summary>
        private readonly byte maxWordSize;
        /// <summary>
        /// 分词选项
        /// </summary>
        private readonly WordSegmentFlags wordSegmentFlags;
        /// <summary>
        /// 分词类型
        /// </summary>
        private WordSegmentTypeEnum wordSegmentType;
        /// <summary>
        /// 当前未知词语字符串位置
        /// </summary>
        private int wordStringIndex;
        /// <summary>
        /// Trie 图序列化数据
        /// </summary>
        private GraphData graphData;
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<GraphData> IEnumerableSnapshot<GraphData>.SnapshotEnumerable { get { return new SnapshotGetValueEmpty<GraphData>(getGraphData); } }
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
        private readonly FragmentSnapshotDictionary256<HashSubString, int> words;
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<BinarySerializeKeyValue<SubString, int>> IEnumerableSnapshot<BinarySerializeKeyValue<SubString, int>>.SnapshotEnumerable
        {
            get
            {
                return words.GetBinarySerializeKeyValueSnapshot().Cast<BinarySerializeKeyValue<HashSubString, int>, BinarySerializeKeyValue<SubString, int>>(p => new BinarySerializeKeyValue<SubString, int>(p.Key.String, p.Value), IsGraph);
            }
        }
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
        /// Trie 树初始化创建器
        /// </summary>
#if NetStandard21
        private TreeBuilder? builder;
#else
        private TreeBuilder builder;
#endif
        /// <summary>
        /// Snapshot collection
        /// 快照集合
        /// </summary>
        ISnapshotEnumerable<string> IEnumerableSnapshot<string>.SnapshotEnumerable { get { return new SnapshotGetEnumerable<string>(getWords); } }
        /// <summary>
        /// 字符串 Trie 图节点
        /// </summary>
        /// <param name="maxTrieWordSize">Trie 词语最大文字长度</param>
        /// <param name="maxWordSize">未知词语最大文字长度</param>
        /// <param name="wordSegmentFlags">分词选项</param>
        /// <param name="replaceChars">替换文字集合</param>
#if NetStandard21
        public StaticTrieGraphNode(byte maxTrieWordSize, byte maxWordSize, WordSegmentFlags wordSegmentFlags = 0, string? replaceChars = null)
#else
        public StaticTrieGraphNode(byte maxTrieWordSize, byte maxWordSize, WordSegmentFlags wordSegmentFlags = 0, string replaceChars = null)
#endif
        {
            this.maxTrieWordSize = Math.Max(maxTrieWordSize, (byte)2);
            this.maxWordSize = Math.Max(maxWordSize, (byte)2);
            ReplaceChars = replaceChars ?? Simplified.Chars;
            this.wordSegmentFlags = wordSegmentFlags;
            formatedText = string.Empty;
            wordSegments.SetEmpty();
            wordSegmentIdentityMap = EmptyArray<ulong>.Array;
            wordString = AutoCSer.Common.AllocateString(wordStringLength);
            words = new FragmentSnapshotDictionary256<HashSubString, int>();
        }
        /// <summary>
        /// Initialization loading is completed and processed
        /// 初始化加载完毕处理
        /// </summary>
        /// <returns>The new node that has been loaded and replaced
        /// 加载完毕替换的新节点</returns>
#if NetStandard21
        public override IStaticTrieGraphNode? StreamPersistenceMemoryDatabaseServiceLoaded()
#else
        public override IStaticTrieGraphNode StreamPersistenceMemoryDatabaseServiceLoaded()
#endif
        {
            if (!graphData.IsGraph) getTreeBuilder();
            return null;
        }
        /// <summary>
        /// 获取 Trie 树初始化创建器
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private TreeBuilder getTreeBuilder()
        {
            if (builder != null) return builder;
            return builder = new TreeBuilder(this);
        }
        /// <summary>
        /// 获取 Trie 图序列化数据
        /// </summary>
        /// <returns></returns>
        private KeyValue<bool, GraphData> getGraphData()
        {
            if (graphData.IsGraph) return new KeyValue<bool, GraphData>(true, graphData);
            return default(KeyValue<bool, GraphData>);
        }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        public void SnapshotSetGraph(GraphData value)
        {
            graphData = value;
            builder = null;
        }
        /// <summary>
        /// 获取 Trie 图词语集合
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> getWords()
        {
            return graphData.IsGraph ? (IEnumerable<string>)EmptyArray<string>.Array : builder.notNull().Words;
        }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        public void SnapshotSetWord(string value)
        {
            getTreeBuilder().Append(value);
        }
        /// <summary>
        /// Load snapshot data (recover memory data from snapshot data)
        /// 加载快照数据（从快照数据恢复内存数据）
        /// </summary>
        /// <param name="value">data</param>
        public void SnapshotSetWordIdentity(BinarySerializeKeyValue<SubString, int> value)
        {
            getWord(ref value.Key, true);
            words.TryAdd(value.Key, value.Value);
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
        /// Add trie graph word
        /// 添加 Trie 图词语
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public AppendWordStateEnum AppendWord(string word)
        {
            if (word?.Length > 1)
            {
                if (word.Length <= maxTrieWordSize)
                {
                    if (!graphData.IsGraph)
                    {
                        getTreeBuilder().Append(word);
                        return AppendWordStateEnum.Success;
                    }
                    return AppendWordStateEnum.GraphBuilded;
                }
                return AppendWordStateEnum.WordSizeLimit;
            }
            return AppendWordStateEnum.WordSizeLess;
        }
        /// <summary>
        /// Has the trie graph been created
        /// 是否已经创建 Trie 图
        /// </summary>
        /// <returns></returns>
        public bool IsGraph()
        {
            return graphData.IsGraph;
        }
        /// <summary>
        /// Get the number of words in the trie graph
        /// 获取 Trie 图词语数量
        /// </summary>
        /// <returns>The number of words in the trie graph
        /// Trie 图词语数量</returns>
        public int GetWordCount()
        {
            return graphData.WordCount;
        }
        /// <summary>
        /// Create the trie graph
        /// 创建 Trie 图
        /// </summary>
        /// <returns>The number of words in the trie graph
        /// Trie 图词语数量</returns>
        public int BuildGraph()
        {
            if(!graphData.IsGraph)
            {
                graphData.Build(getTreeBuilder());
                builder = null;
            }
            return graphData.WordCount;
        }
        /// <summary>
        /// Adds text and returns a collection of word numbers (Check the input parameters before the persistence operation)
        /// 添加文本并返回词语编号集合（持久化操作之前检查输入参数）
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
        /// Adds text and returns a collection of word numbers
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
                while (count != 0);
                return identitys;
            }
            return EmptyArray<int>.Array;
        }
        /// <summary>
        /// Get the collection of query word numbers (ignore unmatched words)
        /// 获取查询词语编号集合（忽略未匹配词语）
        /// </summary>
        /// <param name="text">The text content of the search
        /// 搜索文本内容</param>
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
        /// Get the query word segmentation result
        /// 获取查询分词结果
        /// </summary>
        /// <param name="text">The text content of the search
        /// 搜索文本内容</param>
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
                    while (count != 0);
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
                        while (count != 0);
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
                if (graphData.WordCount != 0)
                {
                    this.formatTextFixed = formatTextFixed;
                    start = formatTextFixed;
                    do
                    {
                        if (((type = wordTypeFixed[*start]) & WordTypeEnum.TrieGraphHead) == 0)
                        {
                            *end = graphData.AnyTrieHeadChar;
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
                if ((wordSegmentFlags & WordSegmentFlags.SingleCharacter) != 0)
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
            if (length > 1 || (wordSegmentFlags & WordSegmentFlags.SingleCharacter) != 0)
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
                        if (graphData.IsCurrentIdentity && length <= maxWordSize)
                        {
                            getWord(ref word, false);
                            words.TryAdd(word, identity = graphData.CurrentIdentity);
                            --graphData.CurrentIdentity;
                            wordSegments.Array[wordSegments.Length++].Set(word, identity);
                            setIdentityMap(identity);
                        }
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
        /// <param name="startChar"></param>
        /// <param name="length"></param>
        internal void AddStartWord(int identity, char* startChar, int length)
        {
            wordSegments.PrepLength(1);
            if (wordSegmentType == WordSegmentTypeEnum.Query || trySetIdentityMap(identity))
            {
                wordSegments.Array[wordSegments.Length++].Set(new SubString((int)(startChar - formatTextFixed), length, formatedText), identity);
            }
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
    }
}
