using System;
/*ulong,ULong;long,Long;uint,UInt;int,Int*/

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 排序索引
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal partial struct ULongSortIndex
    {
        /// <summary>
        /// 数值
        /// </summary>
        internal ulong Value;
        /// <summary>
        /// 位置索引
        /// </summary>
        internal int Index;
        /// <summary>
        /// 设置排序索引
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="index">位置索引</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Set(ulong value, int index)
        {
            Value = value;
            Index = index;
        }
    }
}
