using AutoCSer.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AutoCSer.Search.WordCountIndexs
{
    /// <summary>
    /// 关键字索引结果+词频
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class WordCounter<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 数据结果集合
        /// </summary>
        internal readonly Dictionary<T, HeadLeftArray<int>> Result;
        /// <summary>
        /// 搜索结果数量
        /// </summary>
        public int ResultCount { get { return Result.Count; } }
        /// <summary>
        /// 数据结果集合
        /// </summary>
        public IEnumerable<KeyValuePair<T, HeadLeftArray<int>>> Results { get { return Result;  } }
        /// <summary>
        /// 词频
        /// </summary>
        internal int Count;
        /// <summary>
        /// 词频
        /// </summary>
        public int WordCount { get { return Count; } }
        /// <summary>
        /// 分词类型
        /// </summary>
        public readonly WordTypeEnum WordType;
        /// <summary>
        /// 关键字索引结果+词频
        /// </summary>
        /// <param name="key"></param>
        /// <param name="indexs"></param>
        /// <returns></returns>
        internal WordCounter(T key, ResultIndexs indexs)
        {
            Result = DictionaryCreator<T>.Create<HeadLeftArray<int>>();
            Count = indexs.IndexCount;
            WordType = indexs.WordType;
            Result.Add(key, indexs.Indexs);
        }
        /// <summary>
        /// 添加结果
        /// </summary>
        /// <param name="key"></param>
        /// <param name="indexs"></param>
        /// <returns></returns>
        internal int Add(T key, ResultIndexs indexs)
        {
            int count = indexs.IndexCount;
            Result.Add(key, indexs.Indexs);
            Count += count;
            return count;
        }
        /// <summary>
        /// 删除结果
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal int Remove(T key)
        {
            HeadLeftArray<int> indexs;
            if (Result.Remove(key, out indexs))
            {
                int count = indexs.Count;
                Count -= count;
                return count;
            }
            return 0;
        }
    }
}
