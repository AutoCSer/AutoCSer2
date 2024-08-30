using System;

namespace AutoCSer.SearchTree
{
    /// <summary>
    /// 查找计数
    /// </summary>
    internal sealed class EnumerableCount
    {
        /// <summary>
        /// 跳过记录数量
        /// </summary>
        public int SkipCount;
        /// <summary>
        /// 获取记录数量
        /// </summary>
        public int GetCount;
        /// <summary>
        /// 查找计数
        /// </summary>
        /// <param name="skipCount">跳过记录数量</param>
        /// <param name="getCount">获取记录数量</param>
        internal EnumerableCount(int skipCount, int getCount)
        {
            SkipCount = skipCount;
            GetCount = getCount;
        }
    }
}
