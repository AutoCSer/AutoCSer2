using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Data
{
    /// <summary>
    /// 分页查询参数
    /// </summary>
    public abstract class PageParameter
    {
        /// <summary>
        /// 查询分页编号（从 0 开始）
        /// </summary>
        public int PageIndex;
        /// <summary>
        /// 查询分页数据记录数量，默认为 10
        /// </summary>
        public int PageSize = 10;
        /// <summary>
        /// 获取当前分页数据记录数量
        /// </summary>
        /// <param name="totalCount">记录总数</param>
        /// <returns>当前分页数据记录数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int GetPageSize(int totalCount)
        {
            return Math.Min((int)(totalCount - (long)PageIndex * PageSize), PageSize);
        }
    }
}
