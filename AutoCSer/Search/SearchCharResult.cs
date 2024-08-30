using System;
using System.Collections.Generic;

namespace AutoCSer.Search
{
    /// <summary>
    /// 搜索结果数量与单字符搜索结果
    /// </summary>
    /// <typeparam name="T">数据标识类型</typeparam>
    /// <typeparam name="RT">搜索结果类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SearchCharResult<T, RT>
        where T : IEquatable<T>
        where RT : class
    {
        /// <summary>
        /// 队列操作上下文
        /// </summary>
        private readonly StaticSearcherQueueContext<T, RT> context;
        /// <summary>
        /// 搜索结果版本
        /// </summary>
        private readonly int version;
        /// <summary>
        /// 搜索结果数量
        /// </summary>
        /// <param name="context">队列操作上下文</param>
        /// <param name="version">搜索结果版本</param>
        internal SearchCharResult(StaticSearcherQueueContext<T, RT> context, int version)
        {
            this.context = context;
            this.version = version;
        }
        /// <summary>
        /// 分词搜索结果数量
        /// </summary>
        public int WordResultCount { get { return context.WordResultCount; } }
        /// <summary>
        /// 单字符搜索结果数量
        /// </summary>
        public int CharResultCount { get { return context.CharResultCount; } }
        /// <summary>
        /// 未匹配分词数量
        /// </summary>
        public int LessResultCount { get { return context.LessResultCount; } }
        /// <summary>
        /// 未匹配分词集合
        /// </summary>
        public IEnumerable<SubString> LessWords { get { return context.LessWords; } }
        /// <summary>
        /// 获取单字符搜索结果
        /// </summary>
        /// <param name="results">按结果数量排序的单字符搜索结果</param>
        /// <returns>返回 false 表示检测到并发操作冲突错误</returns>
        public bool GetCharResults(out LeftArray<KeyValue<char, HashSet<T>>> results)
        {
            return context.GetCharResults(version, out results);
        }
    }
}
