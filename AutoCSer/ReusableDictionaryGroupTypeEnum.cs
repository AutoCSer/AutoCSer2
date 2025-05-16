using System;

namespace AutoCSer
{
    /// <summary>
    /// 可重用字典重组操作类型
    /// </summary>
    public enum ReusableDictionaryGroupTypeEnum : byte
    {
        /// <summary>
        /// 重组索引范围内无冲突数据位置直接匹配哈希索引位置，避免二次随机内存访问
        /// </summary>
        HashIndex,
        /// <summary>
        /// 按照哈希索引位置排序，可以保证冲突数据内存的连续，但是会产生二次随机内存访问
        /// </summary>
        HashIndexSort,
        /// <summary>
        /// 滚动索引位置（用于优先级淘汰策略），既不保证匹配哈希索引位置，也不保证内存的连续性，可能需要 n 次随机内存访问
        /// </summary>
        Roll,
    }
}
