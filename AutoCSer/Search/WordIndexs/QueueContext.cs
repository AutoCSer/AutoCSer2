using System;
using System.Collections.Generic;

namespace AutoCSer.Search.WordIndexs
{
    /// <summary>
    /// 绑定结果池的分词搜索器 队列操作上下文（不允许并发操作，仅允许串行操作）
    /// </summary>
    /// <typeparam name="T">数据标识类型</typeparam>
    public class QueueContext<T> : StaticSearcherQueueContext<T, Dictionary<T, HeadLeftArray<int>>>
        where T : IEquatable<T>
    {
        /// <summary>
        /// 队列操作上下文
        /// </summary>
        /// <param name="searcher">绑定结果池的分词搜索器</param>
        internal QueueContext(StaticSearcher<T> searcher) : base(searcher) { }
        /// <summary>
        /// 设置分词位置结果
        /// </summary>
        protected override void setWordResults()
        {
            setResultIndexs();
        }
        /// <summary>
        /// 关键字搜索
        /// </summary>
        /// <param name="text">关键字文本</param>
        /// <param name="size">关键字文本长度</param>
        /// <returns>搜索结果</returns>
        public SearchResult<T> Search(string text, int size)
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
        internal bool GetWordResults(int version, out LeftArray<KeyValue<SubString, Dictionary<T, HeadLeftArray<int>>>> results)
        {
            if (version == this.version)
            {
                wordResultArray.Length = 0;
                if (wordResults.Count != 0)
                {
                    wordResultArray.PrepLength(wordResults.Count);
                    foreach (KeyValue<HashSubString, Dictionary<T, HeadLeftArray<int>>> keys in wordResults.KeyValues) wordResultArray.Add(new KeyValue<SubString, Dictionary<T, HeadLeftArray<int>>>(keys.Key.String, keys.Value));
                    if (wordResultArray.Length > 1) wordResultArray.Sort(wordResultSortHandle);
                }
                results = wordResultArray;
                return true;
            }
            results = new LeftArray<KeyValue<SubString, Dictionary<T, HeadLeftArray<int>>>>(0);
            return false;
        }

        /// <summary>
        /// 分词搜索结果按结果数量排序
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int wordResultSort(KeyValue<SubString, Dictionary<T, HeadLeftArray<int>>> left, KeyValue<SubString, Dictionary<T, HeadLeftArray<int>>> right)
        {
            int value = left.Value.Count - right.Value.Count;
            if (value == 0) return left.Key.Start - right.Key.Start;
            return value;
        }
        /// <summary>
        /// 分词搜索结果按结果数量排序
        /// </summary>
        private static readonly Func<KeyValue<SubString, Dictionary<T, HeadLeftArray<int>>>, KeyValue<SubString, Dictionary<T, HeadLeftArray<int>>>, int> wordResultSortHandle = wordResultSort;
    }
}
