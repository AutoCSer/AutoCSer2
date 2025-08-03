using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// uint expansion operation
    /// uint 扩展操作
    /// </summary>
    public partial struct UIntExtensions
    {
        /// <summary>
        /// Convert integer values to time: Year[23b] + Month[4b] + Day[5b]
        /// 整数值转时间：Year[23b] + Month[4b] + Day[5b]
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public DateTime ToDate(DateTimeKind kind = DateTimeKind.Utc)
        {
            return value.fromIntDate(kind);
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
        /// 32b 除法转位移乘法误差检查（除数必须大于 0）
        /// </summary>
        /// <param name="shiftBit">位移数量</param>
        /// <param name="maxValue">最大被除数</param>
        /// <returns>误差是否满足要求</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool CheckDivMul(int shiftBit, uint maxValue = uint.MaxValue)
        {
            return value.checkDivMul(shiftBit, maxValue);
        }
        /// <summary>
        /// Convert to a string of 8 hexadecimal characters (capital letters)
        /// 转换为 8 个十六进制字符的字符串（大写字母）
        /// </summary>
        /// <returns>A string of 8 hexadecimal characters
        /// 8 个十六进制字符的字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public string ToHex()
        {
            return value.toHex();
        }
        /// <summary>
        /// Get the number of valid bits
        /// 获取有效位数量
        /// </summary>
        /// <returns>Number of valid bits
        /// 有效位数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int Bits()
        {
            return value.bits();
        }
    }
}
