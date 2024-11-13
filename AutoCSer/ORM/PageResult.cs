using System;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 分页查询结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct PageResult<T>
    {
        /// <summary>
        /// 查询分页号，从 1 开始
        /// </summary>
        public readonly int PageIndex;
        /// <summary>
        /// 查询分页记录数量
        /// </summary>
        public readonly int PageSize;
        /// <summary>
        /// 查询跳过记录数量
        /// </summary>
        internal long SkipCount
        {
            get { return PageIndex * (long)PageSize; }
        }
        /// <summary>
        /// 查询记录总数
        /// </summary>
        public long TotalCount;
        /// <summary>
        /// 分页总数
        /// </summary>
        public long PageCount
        {
            get
            {
                return (TotalCount + (PageSize - 1)) / PageSize;
            }
        }
        /// <summary>
        ///  分页查询结果
        /// </summary>
        public LeftArray<T> Result;
        /// <summary>
        /// 分页查询结果
        /// </summary>
        /// <param name="pageIndex">查询分页号，从 1 开始</param>
        /// <param name="pageSize">查询分页记录数量</param>
        public PageResult(int pageIndex, int pageSize = 10)
        {
            PageIndex = Math.Max(pageIndex, 1);
            PageSize = Math.Max(pageSize, 1);
            TotalCount = 0;
            Result = new LeftArray<T>(0);
        }
        /// <summary>
        /// 设置记录总数
        /// </summary>
        /// <param name="count"></param>
        /// <returns>是否需要查询分页数据</returns>
#if NetStandard21
        internal bool Set(ValueResult<long>? count)
#else
        internal bool Set(ValueResult<long> count)
#endif
        {
            if (count == null) return false;
            TotalCount = count.Value;
            return TotalCount > SkipCount;
        }
    }
}
