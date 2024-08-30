using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Search
{
    /// <summary>
    /// 搜索结果
    /// </summary>
    /// <typeparam name="T">数据标识类型</typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct SearchResult<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 队列操作上下文
        /// </summary>
        private readonly StaticSearcherQueueContext<T> context;
        /// <summary>
        /// 搜索结果版本
        /// </summary>
        private readonly int version;
        /// <summary>
        /// 搜索结果数量与单字符搜索结果
        /// </summary>
        public SearchCharResult<T, HashSet<T>> CharResult { get { return new SearchCharResult<T, HashSet<T>>(context, version); } }
        /// <summary>
        /// 搜索结果
        /// </summary>
        /// <param name="context">队列操作上下文</param>
        internal SearchResult(StaticSearcherQueueContext<T> context)
        {
            this.context = context;
            version = context.Version;
        }
        /// <summary>
        /// 获取分词搜索结果
        /// </summary>
        /// <param name="results">按结果数量排序的分词搜索结果</param>
        /// <returns>返回 false 表示检测到并发操作冲突错误</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool GetWordResults(out LeftArray<KeyValue<SubString, HashSet<T>>> results)
        {
            return context.GetWordResults(version, out results);
        }
    }
}
