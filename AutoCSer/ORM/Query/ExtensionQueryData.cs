using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.ORM
{
    /// <summary>
    /// 扩展查询 SQL 信息
    /// </summary>
    internal struct ExtensionQueryData
    {
        /// <summary>
        /// 查询列名称集合
        /// </summary>
        public LeftArray<string> QueryNames;
        /// <summary>
        /// 分组名称集合
        /// </summary>
        public LeftArray<string> GroupBys;
        /// <summary>
        /// 分组条件集合
        /// </summary>
        public LeftArray<string> Havings;
        /// <summary>
        /// 置空并释放数组
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetEmpty()
        {
            QueryNames.SetEmpty();
            GroupBys.SetEmpty();
            Havings.SetEmpty();
        }
        /// <summary>
        /// 添加 GROUP BY 子项
        /// </summary>
        /// <param name="groupBy"></param>
        /// <param name="queryName"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void GroupBy(string groupBy, string? queryName)
#else
        internal void GroupBy(string groupBy, string queryName)
#endif
        {
            GroupBys.Add(groupBy);
            if (queryName != null) QueryNames.Add($"{groupBy} as {queryName}");
        }
        

        /// <summary>
        /// 默认 COUNT(*) 查询
        /// </summary>
        internal static ExtensionQueryData Count = new ExtensionQueryData { QueryNames = new LeftArray<string>(new string[] { $"count(*){nameof(ValueResult<long>.Value)}" }) };
    }
}
