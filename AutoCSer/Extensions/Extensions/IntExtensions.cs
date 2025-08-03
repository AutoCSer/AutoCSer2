using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// int expansion operation
    /// int 扩展操作
    /// </summary>
    public partial struct IntExtensions
    {
        /// <summary>
        /// Logical inversion: 0 to 1, non-0 to 0 (Negative numbers are not allowed)
        /// 逻辑取反，0 转 1，非 0 转 0（不允许负数）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int LogicalInversion()
        {
            return value.logicalInversion();
        }
        /// <summary>
        /// Convert logical values, converting non-0 to 1 (Negative numbers are not allowed)
        /// 转逻辑值，非 0 转 1（不允许负数）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int ToLogical()
        {
            return value.toLogical();
        }
    }
}
