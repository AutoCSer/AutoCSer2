using AutoCSer.Extensions;
using System;
using System.Collections.Generic;

namespace AutoCSer.Search.WordIndexs
{
    /// <summary>
    /// 绑定结果池的分词搜索器（搜索结果包含关键字位置索引）
    /// </summary>
    /// <typeparam name="T">数据标识类型</typeparam>
    public sealed class StaticSearcher<T> : AutoCSer.Search.StaticSearcher<T, Dictionary<T, HeadLeftArray<int>>>, IDisposable
        where T : IEquatable<T>
    {
        /// <summary>
        /// 队列操作上下文
        /// </summary>
        public readonly QueueContext<T> QueueContext;
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
            QueueContext = new QueueContext<T>(this);
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
            var indexs = default(Dictionary<T, HeadLeftArray<int>>);
            foreach (KeyValue<HashSubString, ResultIndexs> result in values.KeyValues)
            {
                if (!results.TryGetValue(result.Key, out indexs)) results.Add(result.Key.String.ToString().notNull(), indexs = DictionaryCreator<T>.Create<HeadLeftArray<int>>());
                indexs.Add(key, result.Value.Indexs);
            }
        }
        /// <summary>
        /// 删除数据结果
        /// </summary>
        /// <param name="key">数据标识</param>
        /// <param name="words">文本分词结果</param>
        protected override void remove(T key, ref LeftArray<KeyValue<SubString, WordTypeEnum>> words)
        {
            int count = words.Length;
            var indexs = default(Dictionary<T, HeadLeftArray<int>>);
            foreach (KeyValue<SubString, WordTypeEnum> word in words.Array)
            {
                if (word.Key.Length != 1)
                {
                    if (results.TryGetValue(word.Key, out indexs))
                    {
                        indexs.Remove(key);
                        if (indexs.Count == 0) results.Remove(word.Key);
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
