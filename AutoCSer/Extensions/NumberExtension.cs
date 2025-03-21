﻿using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// 数值相关扩展操作
    /// </summary>
    public unsafe static class NumberExtension
    {
        /// <summary>
        /// 16位除以10转乘法的乘数
        /// </summary>
        public const uint Div10_16Mul = ((1 << Div10_16Shift) + 9) / 10;
        /// <summary>
        /// 16位除以10转乘法的位移
        /// </summary>
        public const int Div10_16Shift = 19;
        /// <summary>
        /// 32位除以10000转乘法的乘数
        /// </summary>
        public const ulong Div10000Mul = ((1L << 45) + 9999) / 10000;
        /// <summary>
        /// 32位除以10000转乘法的位移
        /// </summary>
        public const int Div10000Shift = 45;
        /// <summary>
        /// 32位除以100000000转乘法的乘数
        /// </summary>
        public const ulong Div100000000Mul = ((1L << 58) + 99999999) / 100000000;
        /// <summary>
        /// 32位除以100000000转乘法的位移
        /// </summary>
        public const int Div100000000Shift = 58;

        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static string toString(this byte value)
        {
            long chars;
            return new string((char*)&chars, 0, ToString(value, (char*)&chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        internal unsafe static int ToString(byte value, char* chars)
        {
            if (value < 10)
            {
                *chars = (char)(value + '0');
                return 1;
            }
            if (value >= 100)
            {
                int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                chars[2] = (char)((value - value10 * 10) + '0');
                int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)chars = ((value10 - value100 * 10) << 16) | value100 | 0x300030;
                return 3;
            }
            else
            {
                int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030;
                return 2;
            }
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static string toString(this sbyte value)
        {
            long chars;
            return new string((char*)&chars, 0, ToString(value, (char*)&chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        internal unsafe static int ToString(sbyte value, char* chars)
        {
            if (value >= 0)
            {
                if (value < 10)
                {
                    *chars = (char)(value + '0');
                    return 1;
                }
                if (value >= 100)
                {
                    value -= 100;
                    *chars = '1';
                    int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 1) = ((value - value10 * 10) << 16) | value10 | 0x300030;
                    return 3;
                }
                else
                {
                    int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030;
                    return 2;
                }
            }
            int value32 = -value;
            if (value32 < 10)
            {
                *(int*)chars = '-' + ((value32 + '0') << 16);
                return 2;
            }
            if (value32 >= 100)
            {
                value32 -= 100;
                *(int*)chars = '-' + ('1' << 16);
                int value10 = (value32 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)(chars + 2) = ((value32 - value10 * 10) << 16) | value10 | 0x300030;
                return 4;
            }
            else
            {
                *chars = '-';
                int value10 = (value32 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)(chars + 1) = ((value32 - value10 * 10) << 16) | value10 | 0x300030;
                return 3;
            }
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static string toString(this ushort value)
        {
            char* chars = stackalloc char[5 + 3];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        internal unsafe static int ToString(ushort value, char* chars)
        {
            if (value < 10)
            {
                *chars = (char)(value + '0');
                return 1;
            }
            if (value >= 10000)
            {
                int value10 = (int)((uint)(value * Div10_16Mul) >> Div10_16Shift);
                int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)(chars + 3) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030;
                value10 = (value100 * (int)Div10_16Mul) >> Div10_16Shift;
                value = (ushort)((value10 * Div10_16Mul) >> Div10_16Shift);
                *(int*)(chars + 1) = ((value100 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030;
                *chars = (char)(value + '0');
                return 5;
            }
            if (value >= 100)
            {
                int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                if (value >= 1000)
                {
                    int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030;
                    value10 = (value100 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)chars = ((value100 - value10 * 10) << 16) | value10 | 0x300030;
                    return 4;
                }
                else
                {
                    chars[2] = (char)((value - value10 * 10) + '0');
                    int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)chars = ((value10 - value100 * 10) << 16) | value100 | 0x300030;
                    return 3;
                }
            }
            else
            {
                int value10 = (value * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030;
                return 2;
            }
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static string toString(this short value)
        {
            char* chars = stackalloc char[6 + 2];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        internal unsafe static int ToString(short value, char* chars)
        {
            if (value >= 0) return ToString((ushort)value, chars);
            int value32 = -value;
            if (value32 < 10)
            {
                *(int*)chars = '-' + ((value32 + '0') << 16);
                return 2;
            }
            if (value32 >= 10000)
            {
                int value10 = (int)((uint)(value32 * Div10_16Mul) >> Div10_16Shift);
                int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)(chars + 4) = ((value32 - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030;
                value10 = (value100 * (int)Div10_16Mul) >> Div10_16Shift;
                value32 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)(chars + 2) = ((value100 - value10 * 10) << 16) | (value10 - value32 * 10) | 0x300030;
                *(int*)chars = '-' + ((value32 + '0') << 16);
                return 6;
            }
            if (value32 >= 100)
            {
                if (value32 >= 1000)
                {
                    *chars = '-';
                    int value10 = (value32 * (int)Div10_16Mul) >> Div10_16Shift;
                    int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 3) = ((value32 - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030;
                    value10 = (value100 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 1) = ((value100 - value10 * 10) << 16) | value10 | 0x300030;
                    return 5;
                }
                else
                {
                    int value10 = (value32 * (int)Div10_16Mul) >> Div10_16Shift;
                    int value100 = (value10 * (int)Div10_16Mul) >> Div10_16Shift;
                    *(int*)(chars + 2) = ((value32 - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030;
                    *(int*)chars = '-' + ((value100 + '0') << 16);
                    return 4;
                }
            }
            else
            {
                *chars = '-';
                int value10 = (value32 * (int)Div10_16Mul) >> Div10_16Shift;
                *(int*)(chars + 1) = ((value32 - value10 * 10) << 16) | value10 | 0x300030;
                return 3;
            }
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static string toString(this uint value)
        {
            char* chars = stackalloc char[10 + 2];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        internal unsafe static int ToString(uint value, char* chars)
        {
            if (value >= 100000000)
            {
                uint value100000000 = (uint)((value * (ulong)Div100000000Mul) >> Div100000000Shift);
                if (value100000000 >= 10)
                {
                    uint value10000 = (value100000000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value100000000 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    toString8(value - value100000000 * 100000000, chars + 2);
                    return 10;
                }
                *chars = (char)(value100000000 + '0');
                toString8(value - value100000000 * 100000000, chars + 1);
                return 9;
            }
            return toString99999999U(value, chars);
        }
        /// <summary>
        /// 小于100000000的正整数转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        private unsafe static int toString99999999U(uint value, char* chars)
        {
            if (value < 10)
            {
                *chars = (char)(value + '0');
                return 1;
            }
            if (value >= 10000)
            {
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                if (value10000 >= 100)
                {
                    uint value10 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    if (value10000 >= 1000)
                    {
                        toString4(value - value10000 * 10000, chars + 4);
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 2) = ((value10000 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030U;
                        value10 = (value * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                        return 8;
                    }
                    else
                    {
                        toString4(value - value10000 * 10000, chars + 3);
                        chars[2] = (char)((value10000 - value10 * 10) + '0');
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value10 - value * 10) << 16) | value | 0x300030U;
                        return 7;
                    }
                }
                if (value10000 >= 10)
                {
                    toString4(value - value10000 * 10000, chars + 2);
                    value = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value10000 - value * 10) << 16) | value | 0x300030U;
                    return 6;
                }
                toString4(value - value10000 * 10000, chars + 1);
                *chars = (char)(value10000 + '0');
                return 5;
            }
            if (value >= 100)
            {
                uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                if (value >= 1000)
                {
                    uint value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
                    value10 = (value100 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value100 - value10 * 10) << 16) | value10 | 0x300030U;
                    return 4;
                }
                else
                {
                    chars[2] = (char)((value - value10 * 10) + '0');
                    uint value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value10 - value100 * 10) << 16) | value100 | 0x300030U;
                    return 3;
                }
            }
            else
            {
                uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                *(uint*)chars = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                return 2;
            }
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static string toString(this int value)
        {
            char* chars = stackalloc char[12];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        internal unsafe static int ToString(int value, char* chars)
        {
            if (value >= 0) return ToString((uint)value, chars);
            uint value32 = (uint)-value;
            if (value32 >= 100000000)
            {
                uint value100000000 = (uint)((value32 * (ulong)Div100000000Mul) >> Div100000000Shift);
                if (value100000000 >= 10)
                {
                    uint value10000 = (value100000000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 1) = ((value100000000 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    *chars = '-';
                    toString8(value32 - value100000000 * 100000000, chars + 3);
                    return 11;
                }
                *(uint*)chars = '-' + ((value100000000 + '0') << 16);
                toString8(value32 - value100000000 * 100000000, chars + 2);
                return 10;
            }
            return toString99999999S(value32, chars);
        }
        /// <summary>
        /// 绝对值小于100000000的负整数转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>字符串长度</returns>
        private unsafe static int toString99999999S(uint value, char* chars)
        {
            if (value < 10)
            {
                *(uint*)chars = '-' + ((value + '0') << 16);
                return 2;
            }
            if (value >= 10000)
            {
                uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
                if (value10000 >= 100)
                {
                    uint value10 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    if (value10000 >= 1000)
                    {
                        toString4(value - value10000 * 10000, chars + 5);
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 3) = ((value10000 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030U;
                        value10 = (value * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 1) = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                        *chars = '-';
                        return 9;
                    }
                    else
                    {
                        toString4(value - value10000 * 10000, chars + 4);
                        value = (value10 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)(chars + 2) = ((value10000 - value10 * 10) << 16) | (value10 - value * 10) | 0x300030U;
                        *(uint*)chars = '-' + ((value + '0') << 16);
                        return 8;
                    }
                }
                if (value10000 >= 10)
                {
                    toString4(value - value10000 * 10000, chars + 3);
                    value = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 1) = ((value10000 - value * 10) << 16) | value | 0x300030U;
                    *chars = '-';
                    return 7;
                }
                toString4(value - value10000 * 10000, chars + 2);
                *(uint*)chars = '-' + ((value10000 + '0') << 16);
                return 6;
            }
            if (value >= 100)
            {
                if (value >= 1000)
                {
                    uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 3) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
                    value10 = (value100 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 1) = ((value100 - value10 * 10) << 16) | value10 | 0x300030U;
                    *chars = '-';
                    return 5;
                }
                else
                {
                    uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
                    *(uint*)chars = '-' + ((value100 + '0') << 16);
                    return 4;
                }
            }
            else
            {
                *chars = '-';
                uint value10 = (value * Div10_16Mul) >> Div10_16Shift;
                *(uint*)(chars + 1) = ((value - value10 * 10) << 16) | value10 | 0x300030U;
                return 3;
            }
        }
        /// <summary>
        /// 4位十进制数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        private unsafe static void toString4(uint value, char* chars)
        {
            uint value10 = (value * Div10_16Mul) >> Div10_16Shift, value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
            *(uint*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
            value10 = (value100 * Div10_16Mul) >> Div10_16Shift;
            *(uint*)chars = ((value100 - value10 * 10) << 16) | value10 | 0x300030U;
        }
        /// <summary>
        /// 8位十进制数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private unsafe static void toString8(uint value, char* chars)
        {
            uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
            toString4(value10000, chars);
            toString4(value - value10000 * 10000U, chars + 4);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static string toString(this ulong value)
        {
            char* chars = stackalloc char[20];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>起始位置+字符串长度</returns>
        internal unsafe static int ToString(ulong value, char* chars)
        {
            if (value >= 10000000000000000L)
            {
                ulong value10000000000000000 = value / 10000000000000000UL;
                value -= value10000000000000000 * 10000000000000000UL;
                uint value32 = (uint)value10000000000000000;
                if (value32 >= 100)
                {
                    uint value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    if (value32 >= 1000)
                    {
                        *(uint*)(chars + 2) = ((value32 - value10000 * 10) << 16) | (value10000 - value100 * 10) | 0x300030U;
                        value10000 = (value100 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value100 - value10000 * 10) << 16) | value10000 | 0x300030U;
                        toString16(value, chars + 4);
                        return 20;
                    }
                    *(uint*)(chars + 1) = ((value32 - value10000 * 10) << 16) | (value10000 - value100 * 10) | 0x300030U;
                    *chars = (char)(value100 + '0');
                    toString16(value, chars + 3);
                    return 19;
                }
                if (value32 >= 10)
                {
                    uint value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value32 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    toString16(value, chars + 2);
                    return 18;
                }
                *chars = (char)(value32 + '0');
                toString16(value, chars + 1);
                return 17;
            }
            if (value >= 100000000)
            {
                ulong value100000000 = value / 100000000;
                value -= value100000000 * 100000000;
                uint value32 = (uint)value100000000;
                if (value32 >= 10000)
                {
                    uint value10000 = (uint)((value100000000 * Div10000Mul) >> Div10000Shift), value8 = value32 - value10000 * 10000;
                    if (value10000 >= 100)
                    {
                        value32 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                        uint value100 = (value32 * Div10_16Mul) >> Div10_16Shift;
                        if (value10000 >= 1000)
                        {
                            *(uint*)(chars + 2) = ((value10000 - value32 * 10) << 16) | (value32 - value100 * 10) | 0x300030U;
                            value32 = (value100 * Div10_16Mul) >> Div10_16Shift;
                            *(uint*)chars = ((value100 - value32 * 10) << 16) | value32 | 0x300030U;
                            toString4(value8, chars + 4);
                            toString8((uint)value, chars + 8);
                            return 16;
                        }
                        else
                        {
                            chars[2] = (char)((value10000 - value32 * 10) + '0');
                            *(uint*)chars = ((value32 - value100 * 10) << 16) | value100 | 0x300030U;
                            toString4(value8, chars + 3);
                            toString8((uint)value, chars + 7);
                            return 15;
                        }
                    }
                    if (value10000 >= 10)
                    {
                        value32 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value10000 - value32 * 10) << 16) | value32 | 0x300030U;
                        toString4(value8, chars + 2);
                        toString8((uint)value, chars + 6);
                        return 14;
                    }
                    *chars = (char)(value10000 + '0');
                    toString4(value8, chars + 1);
                    toString8((uint)value, chars + 5);
                    return 13;
                }
                if (value32 >= 100)
                {
                    uint value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    uint value100 = (value10000 * Div10_16Mul) >> Div10_16Shift;
                    if (value32 >= 1000)
                    {
                        *(uint*)(chars + 2) = ((value32 - value10000 * 10) << 16) | (value10000 - value100 * 10) | 0x300030U;
                        value10000 = (value100 * Div10_16Mul) >> Div10_16Shift;
                        *(uint*)chars = ((value100 - value10000 * 10) << 16) | value10000 | 0x300030U;
                        toString8((uint)value, chars + 4);
                        return 12;
                    }
                    else
                    {
                        chars[2] = (char)((value32 - value10000 * 10) + '0');
                        *(uint*)chars = ((value10000 - value100 * 10) << 16) | value100 | 0x300030U;
                        toString8((uint)value, chars + 3);
                        return 11;
                    }
                }
                if (value32 >= 10)
                {
                    uint value10000 = (value32 * Div10_16Mul) >> Div10_16Shift;
                    *(uint*)chars = ((value32 - value10000 * 10) << 16) | value10000 | 0x300030U;
                    toString8((uint)value, chars + 2);
                    return 10;
                }
                *chars = (char)(value32 + '0');
                toString8((uint)value, chars + 1);
                return 9;
            }
            return toString99999999U((uint)value, chars);
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns>字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static string toString(this long value)
        {
            char* chars = stackalloc char[22 + 2];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// 数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        /// <returns>起始位置+字符串长度</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal unsafe static int ToString(long value, char* chars)
        {
            if (value >= 0) return ToString((ulong)value, chars);
            *chars = '-';
            return ToString((ulong)-value, chars + 1) + 1;
        }
        /// <summary>
        /// 16位十进制数值转字符串
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="chars">字符串</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private unsafe static void toString16(ulong value, char* chars)
        {
            ulong value100000000 = value / 100000000;
            toString8((uint)value100000000, chars);
            toString8((uint)(value - value100000000 * 100000000), chars + 8);
        }

        /// <summary>
        /// 转换16位十六进制字符串（大写字母）
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>16位十六进制字符串</returns>
        public static string toHex(this ulong value)
        {
            string hexs = AutoCSer.Common.AllocateString(16);
            fixed (char* hexFixed = hexs) toHex(value, hexFixed);
            return hexs;
        }
        /// <summary>
        /// 转换16位十六进制字符串（大写字母）
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="hexs">16位十六进制字符串</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal unsafe static void toHex(this ulong value, char* hexs)
        {
            toHex((uint)value, hexs + 8);
            toHex((uint)(value >> 32), hexs);
        }
        /// <summary>
        /// 转换8位十六进制字符串（大写字母）
        /// </summary>
        /// <param name="value">数字值</param>
        /// <returns>8位十六进制字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string toHex(this uint value)
        {
            string hexs = AutoCSer.Common.AllocateString(8);
            fixed (char* hexFixed = hexs) toHex(value, hexFixed);
            return hexs;
        }
        /// <summary>
        /// 数字值转换为十六进制字符串（大写字母）
        /// </summary>
        /// <param name="value">数字值</param>
        /// <param name="hexs">十六进制字符串</param>
        private static void toHex(uint value, char* hexs)
        {
            *hexs = (char)ToHex(value >> 28);
            *(hexs + 1) = (char)ToHex((value >> 24) & 15);
            *(hexs + 2) = (char)ToHex((value >> 20) & 15);
            *(hexs + 3) = (char)ToHex((value >> 16) & 15);
            *(hexs + 4) = (char)ToHex((value >> 12) & 15);
            *(hexs + 5) = (char)ToHex((value >> 8) & 15);
            *(hexs + 6) = (char)ToHex((value >> 4) & 15);
            *(hexs + 7) = (char)ToHex(value & 15);
        }
        /// <summary>
        /// 半字节转十六进制字符（大写字母）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static uint ToHex(uint data)
        {
            return data < 10 ? data + '0' : (data + ('0' + 'A' - '9' - 1));
        }
        ///// <summary>
        ///// 半字节转十六进制字符（大写字母）
        ///// </summary>
        ///// <param name="data">大于等于 10</param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal static uint ToHexLetter(uint data)
        //{
        //    return data + ('0' + 'A' - '9' - 1);
        //}
        /// <summary>
        /// 16b 数字转换成4个16进制字符串（大写字母）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="chars"></param>
        internal static void ToHex4(uint value, char* chars)
        {
            *chars = (char)ToHex(value >> 12);
            *(chars + 1) = (char)ToHex((value >> 8) & 15);
            *(chars + 2) = (char)ToHex((value >> 4) & 15);
            *(chars + 3) = (char)ToHex(value & 15);
        }
        /// <summary>
        /// 数字转换成16进制字符串（大写字母）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        internal unsafe static char* GetToHex(uint value, char* chars)
        {
            if (value >= 0x100)
            {
                if (value >= 0x1000)
                {
                    ToHex4(value, chars);
                    return chars + 4;
                }
                *chars = (char)ToHex(value >> 8);
                *(chars + 1) = (char)ToHex((value >> 4) & 15);
                *(chars + 2) = (char)ToHex(value & 15);
                return chars + 3;
            }
            if (value >= 0x10)
            {
                *chars = (char)ToHex(value >> 4);
                *(chars + 1) = (char)ToHex(value & 15);
                return chars + 2;
            }
            *chars = (char)ToHex(value);
            return chars + 1;
        }
        /// <summary>
        /// 十六进制字符串转数字
        /// </summary>
        /// <param name="hex"></param>
        /// <returns>是否返回 uint.MaxValue</returns>
        private static uint fromHex(char hex)
        {
            uint value = (uint)(hex - '0');
            if (value < 10) return value;
            value = (value - ('A' - '0')) & 0xffdfU;
            if (value < 6) return value + 10;
            return uint.MaxValue;
        }
        /// <summary>
        /// 十六进制字符串转数字
        /// </summary>
        /// <param name="chars"></param>
        /// <returns>失败则高16b不为0</returns>
        private static uint fromHex4(char* chars)
        {
            return (fromHex(chars[0]) << 12) | (fromHex(chars[1]) << 8) | (fromHex(chars[2]) << 4) | fromHex(chars[3]);
        }
        /// <summary>
        /// 十六进制字符串转数字
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool FromHex(char* chars, out uint value)
        {
            uint high = fromHex4(chars), low = fromHex4(chars + 4);
            if (((high | low) & 0xffff0000U) == 0)
            {
                value = low | (high << 16);
                return true;
            }
            value = 0;
            return false;
        }
        /// <summary>
        /// 十六进制字符串转数字
        /// </summary>
        /// <param name="chars"></param>
        /// <returns>失败则高8b不为0</returns>
        private static uint fromHex6(char* chars)
        {
            return (fromHex(chars[0]) << 20) | (fromHex(chars[1]) << 16) | (fromHex(chars[2]) << 12) | (fromHex(chars[3]) << 8) | (fromHex(chars[4]) << 4) | fromHex(chars[5]);
        }
        /// <summary>
        /// 十六进制字符串转数字
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool FromHex(char* chars, out ulong value)
        {
            uint high = fromHex6(chars), middle = fromHex6(chars + 6), low = fromHex4(chars + 12);
            if (((high | middle | low) & 0xff000000U) == 0)
            {
                value = low | ((ulong)middle << 16) | ((ulong)high << 40);
                return true;
            }
            value = 0;
            return false;
        }

        /// <summary>
        /// 填充第一个有效二进制位后面的空位
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static uint fullBit(this uint value)
        {
            value |= value >> 16;
            value |= value >> 8;
            value |= value >> 4;
            value |= value >> 2;
            return value | (value >> 1);
        }
        /// <summary>
        /// 向上去 2 的幂次方
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static uint upToPower2(this uint value)
        {
            return (value & (value - 1)) != 0 ? value.fullBit() + 1 : value;
        }
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
        ///// <summary>
        ///// 获取二进制1位的个数
        ///// </summary>
        ///// <param name="value">数据</param>
        ///// <returns>二进制1位的个数</returns>
        //public static int bitCount(this uint value)
        //{
        //    //return bitCounts[(byte)value] + bitCounts[(byte)(value >> 8)] + bitCounts[(byte)(value >> 16)] + bitCounts[value >> 24];

        //    //value = (value & 0x49249249) + ((value >> 1) & 0x49249249) + ((value >> 2) & 0x49249249);
        //    //value = (value & 0xc71c71c7) + ((value >> 3) & 0xc71c71c7);
        //    //uint div = (uint)(((ulong)value * (((1UL << 37) + 62) / 63)) >> 37);
        //    //return (int)(value - (div << 6) + div);

        //    //value = (value & 0x49249249) + ((value >> 1) & 0x49249249) + ((value >> 2) & 0x49249249);
        //    //value = (value & 0x71c71c7) + ((value >> 3) & 0x71c71c7) + (value >> 30);
        //    //uint nextValue = (uint)((value * 0x41041042UL) >> 36);
        //    //return (int)(value - (nextValue << 6) + nextValue);

        //    value -= ((value >> 1) & 0x55555555U);//2:2
        //    value = (value & 0x33333333U) + ((value >> 2) & 0x33333333U);//4:4
        //    value += value >> 4;
        //    value &= 0x0f0f0f0f;//8:8

        //    //uint div = (uint)(((ulong)value * (((1UL << 39) + 254) / 255)) >> 39);
        //    //return (int)(value - (div << 8) + div);
        //    value += (value >> 8);
        //    return (byte)(value + (value >> 16));
        //}
        /// <summary>
        /// 2^n相关32位deBruijn序列集合
        /// </summary>
        internal static AutoCSer.Memory.Pointer DeBruijn32;
        /// <summary>
        /// 2^n相关32位deBruijn序列
        /// </summary>
        public const uint DeBruijn32Number = 0x04653adfU;
        /// <summary>
        /// 求 2 的 x 次方
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int deBruijnLog2(this uint value)
        {
            return DeBruijn32.Byte[(value * DeBruijn32Number) >> 27];
        }

        static NumberExtension()
        {
            DeBruijn32 = Unmanaged.GetDeBruijn32Number();
            byte* deBruijn32Data = DeBruijn32.Byte;
            for (byte bit = 0; bit != 32; ++bit) deBruijn32Data[((1U << bit) * DeBruijn32Number) >> 27] = bit;
        }
    }
}
