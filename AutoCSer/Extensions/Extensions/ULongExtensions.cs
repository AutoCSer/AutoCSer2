using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// ulong expansion operation
    /// ulong 扩展操作
    /// </summary>
    public partial struct ULongExtensions
    {
        /// <summary>
        /// 获取二进制1位的个数
        /// </summary>
        /// <returns>二进制1位的个数</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int BitCount()
        {
            return value.bitCount();
        }
        /// <summary>
        /// 获取有效位长度
        /// </summary>
        /// <returns>有效位长度</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int Bits()
        {
            return value.bits();
        }
        /// <summary>
        /// 获取最后二进制0位的长度
        /// </summary>
        /// <returns>最后二进制0位的长度</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int EndBits()
        {
            return value.endBits();
        }
        /// <summary>
        /// Convert to a string of 16 hexadecimal characters (capital letters)
        /// 转换为 16 个十六进制字符的字符串（大写字母）
        /// </summary>
        /// <returns>A string of 16 hexadecimal characters
        /// 16 个十六进制字符的字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public string ToHex()
        {
            return value.toHex();
        }
    }
}
