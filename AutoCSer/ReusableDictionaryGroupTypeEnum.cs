using System;

namespace AutoCSer
{
    /// <summary>
    /// Reusable dictionary recombination operation types
    /// 可重用字典重组操作类型
    /// </summary>
    public enum ReusableDictionaryGroupTypeEnum : byte
    {
        /// <summary>
        /// The conflict-free data positions within the reorganized index range are directly matched with the hash index positions to avoid secondary random memory access
        /// 重组索引范围内无冲突数据位置直接匹配哈希索引位置，避免二次随机内存访问
        /// </summary>
        HashIndex,
        /// <summary>
        /// Sorting by the position of the hash index can ensure the continuity of conflicting data memory, but it will result in secondary random memory access
        /// 按照哈希索引位置排序，可以保证冲突数据内存的连续，但是会产生二次随机内存访问
        /// </summary>
        HashIndexSort,
        /// <summary>
        /// Rolling the index position (used for the priority elimination strategy) neither guarantees matching the hash index position nor the continuity of memory, and may require n random memory accesses
        /// 滚动索引位置（用于优先级淘汰策略），既不保证匹配哈希索引位置，也不保证内存的连续性，可能需要 n 次随机内存访问
        /// </summary>
        Roll,
    }
}
