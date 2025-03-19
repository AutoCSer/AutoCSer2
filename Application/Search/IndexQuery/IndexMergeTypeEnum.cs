using System;

namespace AutoCSer.CommandService.Search.IndexQuery
{
    /// <summary>
    /// 索引合并操作类型
    /// </summary>
    public enum IndexMergeTypeEnum : byte
    {
        /// <summary>
        /// 交集 AND
        /// </summary>
        Intersection,
        /// <summary>
        /// 并集 OR
        /// </summary>
        Union,
        /// <summary>
        /// 交集 AND（忽略空集，必须存在一个非空集）
        /// </summary>
        IntersectionNotEmpty,
    }
}
