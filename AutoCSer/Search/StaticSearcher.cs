using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 绑定结果池的分词搜索器
    /// </summary>
    public abstract class StaticSearcher
    {
        /// <summary>
        /// 绑定静态节点池的字符串 Trie 图
        /// </summary>
        internal readonly StaticStringTrieGraph TrieGraph;
        /// <summary>
        /// 绑定结果池的分词搜索器
        /// </summary>
        /// <param name="trieGraph">绑定静态节点池的字符串 Trie 图</param>
#if NetStandard21
        protected StaticSearcher(StaticStringTrieGraph? trieGraph)
#else
        protected StaticSearcher(StaticStringTrieGraph trieGraph)
#endif
        {
            TrieGraph = trieGraph ?? StaticStringTrieGraph.Null;
        }
    }
    /// <summary>
    /// 绑定结果池的分词搜索器
    /// </summary>
    /// <typeparam name="T">数据标识类型</typeparam>
    /// <typeparam name="RT">搜索结果类型</typeparam>
    public abstract class StaticSearcher<T, RT> : StaticSearcher
        where T : IEquatable<T>
        where RT : class
    {
        /// <summary>
        /// 原始分词文本信息集合
        /// </summary>
        internal Dictionary<T, string> Texts;
        /// <summary>
        /// 关键字数据结果集合
        /// </summary>
        protected Dictionary<HashSubString, RT> results;
        /// <summary>
        /// 单字符数据结果集合
        /// </summary>
        protected readonly HashSet<T>[] characters;
        /// <summary>
        /// 是否支持单字符搜索结果
        /// </summary>
        public bool IsSingleCharacter { get { return characters.Length != 0; } }
        /// <summary>
        /// 绑定结果池的分词搜索器
        /// </summary>
        /// <param name="trieGraph">绑定静态节点池的字符串 Trie 图</param>
        /// <param name="isSingleCharacter">是否支持单字符搜索结果</param>
#if NetStandard21
        protected StaticSearcher(StaticStringTrieGraph? trieGraph, bool isSingleCharacter) : base(trieGraph)
#else
        protected StaticSearcher(StaticStringTrieGraph trieGraph, bool isSingleCharacter) : base(trieGraph)
#endif
        {
            Texts = DictionaryCreator<T>.Create<string>();
            results = AutoCSer.Extensions.DictionaryCreator.CreateHashSubString<RT>();
            characters = isSingleCharacter ? new HashSet<T>[1 << 16] : EmptyArray<HashSet<T>>.Array;
        }
        /// <summary>
        /// 清除所有数据
        /// </summary>
        internal void Clear()
        {
            if (Texts.Count != 0) Texts = DictionaryCreator<T>.Create<string>();
            if (results.Count != 0) results = AutoCSer.Extensions.DictionaryCreator.CreateHashSubString<RT>();
            Array.Clear(characters, 0, characters.Length);
        }
        /// <summary>
        /// 添加新的数据
        /// </summary>
        /// <param name="key">数据标识</param>
        /// <param name="text">原始分词文本</param>
        /// <param name="wordIndexs">分词结果</param>
        /// <param name="words">单字符分词集合</param>
        /// <param name="formatedText">格式化以后的分词文本</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal unsafe void Add(T key, string text, ReusableDictionary<HashSubString, ResultIndexs> wordIndexs, ref LeftArray<KeyValue<SubString, WordTypeEnum>> words, string? formatedText)
#else
        internal unsafe void Add(T key, string text, ReusableDictionary<HashSubString, ResultIndexs> wordIndexs, ref LeftArray<KeyValue<SubString, WordTypeEnum>> words, string formatedText)
#endif
        {
            Texts.Add(key, text);
            if (wordIndexs.Count != 0) add(key, wordIndexs);
            int count = words.Length;
            if (count != 0)
            {
                foreach (KeyValue<SubString, WordTypeEnum> word in words.Array)
                {
                    char code = word.Key[0];
                    HashSet<T> keyTypes = characters[code];
                    if (keyTypes == null) characters[code] = keyTypes = HashSetCreator<T>.Create();
                    keyTypes.Add(key);
                    if (--count == 0) break;
                }
            }
            if (formatedText != null)
            {
                byte* charTypeData = TrieGraph.CharTypeData.Byte;
                fixed (char* textFixed = formatedText)
                {
                    char* start = textFixed, end = textFixed + text.Length;
                    do
                    {
                        char chineseCharacter = *start;
                        if ((charTypeData[chineseCharacter] & (byte)WordTypeEnum.Chinese) != 0)
                        {
                            HashSet<T> keyTypes = characters[chineseCharacter];
                            if (keyTypes == null) characters[chineseCharacter] = keyTypes = HashSetCreator<T>.Create();
                            keyTypes.Add(key);
                        }
                    }
                    while (++start != end);
                }
            }
        }
        /// <summary>
        /// 添加新的数据
        /// </summary>
        /// <param name="key">数据标识</param>
        /// <param name="values">分词结果</param>
        protected abstract void add(T key, ReusableDictionary<HashSubString, ResultIndexs> values);
        /// <summary>
        /// 删除数据结果
        /// </summary>
        /// <param name="key">数据标识</param>
        /// <param name="words">文本分词结果</param>
        /// <param name="formatedText">格式化以后的分词文本</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal unsafe void Remove(T key, ref LeftArray<KeyValue<SubString, WordTypeEnum>> words, SubString formatedText)
        {
            remove(key, ref words);
            if (formatedText.Length != 0)
            {
                fixed (char* textFixed = formatedText.GetFixedBuffer())
                {
                    char* start = textFixed, end = textFixed + formatedText.Length;
                    do
                    {
                        char chineseCharacter = *start;
                        HashSet<T> keyTypes = characters[chineseCharacter];
                        if (keyTypes != null) keyTypes.Remove(key);
                    }
                    while (++start != end);
                }
            }
        }
        /// <summary>
        /// 删除数据结果
        /// </summary>
        /// <param name="key">数据标识</param>
        /// <param name="words">文本分词结果</param>
        protected abstract void remove(T key, ref LeftArray<KeyValue<SubString, WordTypeEnum>> words);
        /// <summary>
        /// 根据关键字获取数据结果
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal RT? GetResult(ref HashSubString word)
#else
        internal RT GetResult(ref HashSubString word)
#endif
        {
            var counter = default(RT);
            return results.TryGetValue(word, out counter) ? counter : null;
        }
        /// <summary>
        /// 根据关键字获取数据结果
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal HashSet<T>? GetResult(char code)
#else
        internal HashSet<T> GetResult(char code)
#endif
        {
            HashSet<T> value = characters[code];
            return value != null && value.Count != 0 ? value : null;
        }
    }
    /// <summary>
    /// 绑定结果池的分词搜索器
    /// </summary>
    /// <typeparam name="T">数据标识类型</typeparam>
    public sealed class StaticSearcher<T> : StaticSearcher<T, HashSet<T>>, IDisposable
        where T : IEquatable<T>
    {
        /// <summary>
        /// 队列操作上下文
        /// </summary>
        public readonly StaticSearcherQueueContext<T> QueueContext;
        /// <summary>
        /// 绑定结果池的分词搜索器
        /// </summary>
        /// <param name="trieGraph">绑定静态节点池的字符串 Trie 图</param>
        /// <param name="isSingleCharacter">默认为 true 表示支持单字符搜索结果，但是会造成占用大量内存；设置为 false 则应该设置中文词库，否则无法搜索中文内容</param>
#if NetStandard21
        public StaticSearcher(StaticStringTrieGraph? trieGraph = null, bool isSingleCharacter = true)
#else
        public StaticSearcher(StaticStringTrieGraph trieGraph = null, bool isSingleCharacter = true)
#endif
            : base(trieGraph, isSingleCharacter)
        {
            QueueContext = new StaticSearcherQueueContext<T>(this);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            QueueContext.Dispose();
        }
        /// <summary>
        /// 添加新的数据
        /// </summary>
        /// <param name="key">数据标识</param>
        /// <param name="values">分词结果</param>
        protected override void add(T key, ReusableDictionary<HashSubString, ResultIndexs> values)
        {
            var keys = default(HashSet<T>);
            foreach (KeyValue<HashSubString, ResultIndexs> result in values.KeyValues)
            {
                if (!results.TryGetValue(result.Key, out keys)) results.Add(result.Key.String.ToString().notNull(), keys = HashSetCreator<T>.Create());
                keys.Add(key);
            }
        }
        /// <summary>
        /// 删除数据结果
        /// </summary>
        /// <param name="key">数据标识</param>
        /// <param name="words">文本分词结果</param>
        protected override void remove(T key, ref LeftArray<KeyValue<SubString, WordTypeEnum>> words)
        {
            var keys = default(HashSet<T>);
            int count = words.Length;
            foreach (KeyValue<SubString, WordTypeEnum> word in words.Array)
            {
                if (word.Key.Length != 1)
                {
                    if (results.TryGetValue(word.Key, out keys))
                    {
                        keys.Remove(key);
                        if (keys.Count == 0) results.Remove(word.Key);
                    }
                }
                else
                {
                    HashSet<T> keyTypes = characters[word.Key[0]];
                    if (keyTypes != null) keyTypes.Remove(key);
                }
                if (--count == 0) break;
            }
        }
    }
}
