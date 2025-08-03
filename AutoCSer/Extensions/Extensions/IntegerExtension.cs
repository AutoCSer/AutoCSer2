using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Integer correlation extension operations
    /// 数值相关扩展操作
    /// </summary>
    internal unsafe static class IntegerExtension
    {
        /// <summary>
        /// 获取二进制1位的个数
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>二进制1位的个数</returns>
        internal static int bitCount(this ulong value)
        {
            value -= ((value >> 1) & 0x5555555555555555UL);//2:2
            value = (value & 0x3333333333333333UL) + ((value >> 2) & 0x3333333333333333UL);//4:4
            value += value >> 4;
            value &= 0x0f0f0f0f0f0f0f0fUL;//8:8

            value += (value >> 8);
            value += (value >> 16);
            return (byte)(value + (value >> 32));
        }
        /// <summary>
        /// 获取有效位长度
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>有效位长度</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int bits(this ulong value)
        {
            return (value & 0xffffffff00000000UL) == 0 ? AutoCSer.Extensions.NumberExtension.bits((uint)value) : (AutoCSer.Extensions.NumberExtension.bits((uint)(value >> 32)) + 32);
            //if ((value & 0x8000000000000000UL) == 0)
            //{
            //    ulong code = value;
            //    code |= code >> 32;
            //    code |= code >> 16;
            //    code |= code >> 8;
            //    code |= code >> 4;
            //    code |= code >> 2;
            //    code |= code >> 1;
            //    return DeBruijn64[((++code) * DeBruijn64Number) >> 58];
            //}
            //else return 32;
        }
        /// <summary>
        /// 获取最后二进制0位的长度
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>最后二进制0位的长度</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int endBits(this uint value)
        {
            return value != 0 ? NumberExtension.DeBruijn32.Byte[((value & (0U - value)) * NumberExtension.DeBruijn32Number) >> 27] : 0;
        }
        /// <summary>
        /// 获取最后二进制0位的长度
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>最后二进制0位的长度</returns>
        internal static int endBits(this ulong value)
        {
            return (value & 0xffffffff00000000UL) == 0
                ? (value != 0 ? endBits((uint)(value >> 32)) + 32 : 0)
                : endBits((uint)value);
            //return value != 0 ? DeBruijn64[((value & (0UL - value)) * DeBruijn64Number) >> 58] : 0;
        }
        /// <summary>
        /// 逻辑取反，0 转 1，非 0 转 0
        /// </summary>
        /// <param name="value">不允许负数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static long logicalInversion(this long value)
        {
            return (long)(((ulong)value - 1) >> 63);
        }
        /// <summary>
        /// 转逻辑值，非 0 转 1
        /// </summary>
        /// <param name="value">不允许负数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static long toLogical(this long value)
        {
            return logicalInversion(value) ^ 1;
        }

        /// <summary>
        /// 32b 除法转位移乘法误差检查
        /// </summary>
        /// <param name="divisor">除数，必须大于 0</param>
        /// <param name="shiftBit">位移数量</param>
        /// <param name="maxValue">最大被除数</param>
        /// <returns>误差是否满足要求</returns>
        internal static bool checkDivMul(this uint divisor, int shiftBit, uint maxValue = uint.MaxValue)
        {
            ulong shiftValue = 1UL << shiftBit, multiplier = (shiftValue - 1) / divisor + 1;
            if (maxValue * (multiplier * divisor - shiftValue) < shiftValue) //误差判断
            {
                return maxValue * multiplier / multiplier == maxValue;//溢出判断
            }
            return false;
        }
        /// <summary>
        /// 16b 除法转位移乘法误差检查
        /// </summary>
        /// <param name="divisor">除数，必须大于 0</param>
        /// <param name="shiftBit">位移数量</param>
        /// <param name="maxValue">最大被除数</param>
        /// <returns>误差是否满足要求</returns>
        internal static bool checkDivMul(this ushort divisor, int shiftBit, ushort maxValue = ushort.MaxValue)
        {
            uint shiftValue = 1U << shiftBit, multiplier = (shiftValue - 1) / divisor + 1;
            if (maxValue * (multiplier * divisor - shiftValue) < shiftValue) //误差判断
            {
                return maxValue * multiplier / multiplier == maxValue;//溢出判断
            }
            return false;
        }
    }
}
