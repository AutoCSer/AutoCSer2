using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// long expansion operation
    /// long 扩展操作
    /// </summary>
    public partial struct LongExtensions
    {
        /// <summary>
        /// 逻辑取反，0 转 1，非 0 转 0（不允许负数）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public long LogicalInversion()
        {
            return value.logicalInversion();
        }
        /// <summary>
        /// 转逻辑值，非 0 转 1（不允许负数）
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public long ToLogical()
        {
            return value.toLogical();
        }
    }
}
