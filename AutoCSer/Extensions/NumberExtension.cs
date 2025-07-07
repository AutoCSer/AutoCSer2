using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// Integer correlation extension operations
    /// 数值相关扩展操作
    /// </summary>
    public unsafe static class NumberExtension
    {
        /// <summary>
        /// The multiplier of a 16-bit integer divided by 10
        /// 16 位整数除以 10 转乘法的乘数
        /// </summary>
        public const uint Div10_16Mul = ((1 << Div10_16Shift) + 9) / 10;
        /// <summary>
        /// The number of shifts in the multiplication method of a 16-bit integer divided by 10
        /// 16 位整数除以 10 转乘法的位移
        /// </summary>
        public const int Div10_16Shift = 19;
        /// <summary>
        /// The multiplier of a 32-bit integer divided by 10,000
        /// 32 位整数除以 10000 转乘法的乘数
        /// </summary>
        public const ulong Div10000Mul = ((1L << 45) + 9999) / 10000;
        /// <summary>
        /// The number of shifts in the multiplication method of a 32-bit integer divided by 10,000
        /// 32 位整数除以 10000 转乘法的位移
        /// </summary>
        public const int Div10000Shift = 45;
        /// <summary>
        /// The multiplier of a 32-bit integer divided by 100,000,000
        /// 32 位整数除以 100000000 转乘法的乘数
        /// </summary>
        public const ulong Div100000000Mul = ((1L << 58) + 99999999) / 100000000;
        /// <summary>
        /// The number of shifts in the multiplication method of a 32-bit integer divided by 100,000,000
        /// 32 位整数除以 100000000 转乘法的位移
        /// </summary>
        public const int Div100000000Shift = 58;

        /// <summary>
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <returns>Integer string
        /// 整数字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static string toString(this byte value)
        {
            long chars;
            return new string((char*)&chars, 0, ToString(value, (char*)&chars));
        }
        /// <summary>
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <param name="chars">String output buffer
        /// 字符串输出缓冲区</param>
        /// <returns>String length</returns>
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
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <returns>Integer string
        /// 整数字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static string toString(this sbyte value)
        {
            long chars;
            return new string((char*)&chars, 0, ToString(value, (char*)&chars));
        }
        /// <summary>
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <param name="chars">String output buffer
        /// 字符串输出缓冲区</param>
        /// <returns>String length</returns>
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
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <returns>Integer string
        /// 整数字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static string toString(this ushort value)
        {
            char* chars = stackalloc char[5 + 3];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <param name="chars">String output buffer
        /// 字符串输出缓冲区</param>
        /// <returns>String length</returns>
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
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <returns>Integer string
        /// 整数字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static string toString(this short value)
        {
            char* chars = stackalloc char[6 + 2];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <param name="chars">String output buffer
        /// 字符串输出缓冲区</param>
        /// <returns>String length</returns>
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
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <returns>Integer string
        /// 整数字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static string toString(this uint value)
        {
            char* chars = stackalloc char[10 + 2];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <param name="chars">String output buffer
        /// 字符串输出缓冲区</param>
        /// <returns>String length</returns>
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
        /// Convert positive integers less than 100,000,000 to strings
        /// 小于 100000000 的正整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <param name="chars">String output buffer
        /// 字符串输出缓冲区</param>
        /// <returns>String length</returns>
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
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <returns>Integer string
        /// 整数字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static string toString(this int value)
        {
            char* chars = stackalloc char[12];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <param name="chars">String output buffer
        /// 字符串输出缓冲区</param>
        /// <returns>String length</returns>
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
        /// Convert a negative integer with an absolute value less than 100,000,000 to a string
        /// 绝对值小于 100000000 的负整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <param name="chars">String output buffer
        /// 字符串输出缓冲区</param>
        /// <returns>String length</returns>
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
        /// Convert 4-digit decimal values to strings
        /// 4 位十进制数值转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <param name="chars">String output buffer
        /// 字符串输出缓冲区</param>
        private unsafe static void toString4(uint value, char* chars)
        {
            uint value10 = (value * Div10_16Mul) >> Div10_16Shift, value100 = (value10 * Div10_16Mul) >> Div10_16Shift;
            *(uint*)(chars + 2) = ((value - value10 * 10) << 16) | (value10 - value100 * 10) | 0x300030U;
            value10 = (value100 * Div10_16Mul) >> Div10_16Shift;
            *(uint*)chars = ((value100 - value10 * 10) << 16) | value10 | 0x300030U;
        }
        /// <summary>
        /// 8-digit decimal value to string
        /// 8 位十进制数值转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <param name="chars">String output buffer
        /// 字符串输出缓冲区</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private unsafe static void toString8(uint value, char* chars)
        {
            uint value10000 = (uint)((value * Div10000Mul) >> Div10000Shift);
            toString4(value10000, chars);
            toString4(value - value10000 * 10000U, chars + 4);
        }
        /// <summary>
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <returns>Integer string
        /// 整数字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static string toString(this ulong value)
        {
            char* chars = stackalloc char[20];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <param name="chars">String output buffer
        /// 字符串输出缓冲区</param>
        /// <returns>String length</returns>
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
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <returns>Integer string
        /// 整数字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static string toString(this long value)
        {
            char* chars = stackalloc char[22 + 2];
            return new string(chars, 0, ToString(value, chars));
        }
        /// <summary>
        /// Integer to string conversion
        /// 整数转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <param name="chars">String output buffer
        /// 字符串输出缓冲区</param>
        /// <returns>String length</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal unsafe static int ToString(long value, char* chars)
        {
            if (value >= 0) return ToString((ulong)value, chars);
            *chars = '-';
            return ToString((ulong)-value, chars + 1) + 1;
        }
        /// <summary>
        /// Convert 16-digit decimal values to strings
        /// 16 位十进制数值转字符串
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <param name="chars">String output buffer
        /// 字符串输出缓冲区</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private unsafe static void toString16(ulong value, char* chars)
        {
            ulong value100000000 = value / 100000000;
            toString8((uint)value100000000, chars);
            toString8((uint)(value - value100000000 * 100000000), chars + 8);
        }

        /// <summary>
        /// Convert to a string of 16 hexadecimal characters (capital letters)
        /// 转换为 16 个十六进制字符的字符串（大写字母）
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <returns>A string of 16 hexadecimal characters
        /// 16 个十六进制字符的字符串</returns>
        public static string toHex(this ulong value)
        {
            string hexs = AutoCSer.Common.AllocateString(16);
            fixed (char* hexFixed = hexs) toHex(value, hexFixed);
            return hexs;
        }
        /// <summary>
        /// Convert to a string of 16 hexadecimal characters (capital letters)
        /// 转换为 16 个十六进制字符的字符串（大写字母）
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <param name="hexs">A string of 16 hexadecimal characters
        /// 16 个十六进制字符的字符串</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal unsafe static void toHex(this ulong value, char* hexs)
        {
            toHex((uint)value, hexs + 8);
            toHex((uint)(value >> 32), hexs);
        }
        /// <summary>
        /// Convert to a string of 8 hexadecimal characters (capital letters)
        /// 转换为 8 个十六进制字符的字符串（大写字母）
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <returns>A string of 8 hexadecimal characters
        /// 8 个十六进制字符的字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string toHex(this uint value)
        {
            string hexs = AutoCSer.Common.AllocateString(8);
            fixed (char* hexFixed = hexs) toHex(value, hexFixed);
            return hexs;
        }
        /// <summary>
        /// Convert to a string of 8 hexadecimal characters (capital letters)
        /// 转换为 8 个十六进制字符的字符串（大写字母）
        /// </summary>
        /// <param name="value">Integer value
        /// 整数值</param>
        /// <param name="hexs">A string of 8 hexadecimal characters
        /// 8 个十六进制字符的字符串</param>
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
        /// 4-bit to hexadecimal characters (capital letters)
        /// 4 位转十六进制字符（大写字母）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static uint ToHex(uint data)
        {
            return data < 10 ? data + '0' : (data + ('0' + 'A' - '9' - 1));
        }
        /// <summary>
        /// Convert 16 bits to 4 hexadecimal strings (capital letters)
        /// 16 位转换成 4 个 16进制字符串（大写字母）
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
        /// Convert to a hexadecimal string (capital letters)
        /// 转换成 16 进制字符串（大写字母）
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
        /// Hexadecimal string to integer conversion
        /// 十六进制字符串转整数
        /// </summary>
        /// <param name="hex"></param>
        /// <returns>Return uint.MaxValue in case of failure
        /// 失败返回 uint.MaxValue</returns>
        private static uint fromHex(char hex)
        {
            uint value = (uint)(hex - '0');
            if (value < 10) return value;
            value = (value - ('A' - '0')) & 0xffdfU;
            if (value < 6) return value + 10;
            return uint.MaxValue;
        }
        /// <summary>
        /// Hexadecimal string to integer conversion
        /// 十六进制字符串转整数
        /// </summary>
        /// <param name="chars"></param>
        /// <returns>If the high 16 bits are not 0, it indicates failure
        /// 高 16 位 不为 0 表示失败</returns>
        private static uint fromHex4(char* chars)
        {
            return (fromHex(chars[0]) << 12) | (fromHex(chars[1]) << 8) | (fromHex(chars[2]) << 4) | fromHex(chars[3]);
        }
        /// <summary>
        /// Hexadecimal string to integer conversion
        /// 十六进制字符串转整数
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
        /// Hexadecimal string to integer conversion
        /// 十六进制字符串转整数
        /// </summary>
        /// <param name="chars"></param>
        /// <returns>If the high 8 bits are not 0, it indicates failure
        /// 高 8 位 不为 0 表示失败</returns>
        private static uint fromHex6(char* chars)
        {
            return (fromHex(chars[0]) << 20) | (fromHex(chars[1]) << 16) | (fromHex(chars[2]) << 12) | (fromHex(chars[3]) << 8) | (fromHex(chars[4]) << 4) | fromHex(chars[5]);
        }
        /// <summary>
        /// Hexadecimal string to integer conversion
        /// 十六进制字符串转整数
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
        /// Logical inversion: 0 to 1, non-0 to 0
        /// 逻辑取反，0 转 1，非 0 转 0
        /// </summary>
        /// <param name="value">Negative numbers are not allowed
        /// 不允许负数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int logicalInversion(this int value)
        {
            return (int)(((uint)value - 1) >> 31);
        }
        /// <summary>
        /// Convert logical values, converting non-0 to 1
        /// 转逻辑值，非 0 转 1
        /// </summary>
        /// <param name="value">Negative numbers are not allowed
        /// 不允许负数</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int toLogical(this int value)
        {
            return logicalInversion(value) ^ 1;
        }
        ///// <summary>
        ///// 逻辑取反，0 转 1，非 0 转 0
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public static uint logicalInversion(this uint value)
        //{
        //    return ((value - 1) & ~value) >> 31;
        //}
        ///// <summary>
        ///// 转逻辑值，非 0 转 1
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public static uint toLogical(this uint value)
        //{
        //    return logicalInversion(value) ^ 1;
        //}

        /// <summary>
        /// Fill the empty space after the first valid binary bit
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
        /// Take the power of 2 upwards
        /// 向上取 2 的幂次方
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static uint upToPower2(this uint value)
        {
            return (value & (value - 1)) != 0 ? value.fullBit() + 1 : value;
        }
        /// <summary>
        /// Get the number of valid bits
        /// 获取有效位数量
        /// </summary>
        /// <param name="value">data</param>
        /// <returns>Number of valid bits
        /// 有效位数量</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int bits(this uint value)
        {
            return (value & 0x80000000U) == 0 ? NumberExtension.DeBruijn32.Byte[((value.fullBit() + 1) * NumberExtension.DeBruijn32Number) >> 27] : 32;
        }
        ///// <summary>
        ///// 获取二进制1位的个数
        ///// </summary>
        ///// <param name="value">data</param>
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
        /// A collection of 2^n related 32-bit deBruijn sequences
        /// 2^n 相关 32 位 deBruijn 序列集合
        /// </summary>
        internal static AutoCSer.Memory.Pointer DeBruijn32;
        /// <summary>
        /// 2^n related 32-bit deBruijn sequence
        /// 2^n 相关 32位 deBruijn 序列
        /// </summary>
        public const uint DeBruijn32Number = 0x04653adfU;
        /// <summary>
        /// Find 2 to the power of x
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
