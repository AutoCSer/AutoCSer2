using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数值相关扩展操作
    /// </summary>
    public unsafe static class IntegerExtension
    {
        /// <summary>
        /// 获取有效位长度
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>有效位长度</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int bits(this uint value)
        {
            return (value & 0x80000000U) == 0 ? NumberExtension.DeBruijn32.Byte[((value.fullBit() + 1) * NumberExtension.DeBruijn32Number) >> 27] : 32;
        }
        /// <summary>
        /// 获取有效位长度
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>有效位长度</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int bits(this ulong value)
        {
            return (value & 0xffffffff00000000UL) == 0 ? bits((uint)value) : (bits((uint)(value >> 32)) + 32);
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
        /// <param name="value">数据</param>
        /// <returns>最后二进制0位的长度</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int endBits(this uint value)
        {
            return value != 0 ? NumberExtension.DeBruijn32.Byte[((value & (0U - value)) * NumberExtension.DeBruijn32Number) >> 27] : 0;
        }
        /// <summary>
        /// 获取最后二进制0位的长度
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns>最后二进制0位的长度</returns>
        public static int endBits(this ulong value)
        {
            return (value & 0xffffffff00000000UL) == 0
                ? (value != 0 ? endBits((uint)(value >> 32)) + 32 : 0)
                : endBits((uint)value);
            //return value != 0 ? DeBruijn64[((value & (0UL - value)) * DeBruijn64Number) >> 58] : 0;
        }
    }
}
