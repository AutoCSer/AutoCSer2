using AutoCSer.Extensions;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 31b 整数除法
    /// </summary>
    [AutoCSer.BinarySerialize(IsReferenceMember = false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal partial struct IntegerDivision
    {
        /// <summary>
        /// 31b 除数
        /// </summary>
        internal uint Divisor;
        /// <summary>
        /// 除 divisor 转位移乘法位移数量
        /// </summary>
        internal int ShiftBit;
        /// <summary>
        /// 除 divisor 转位移乘法乘数
        /// </summary>
        internal ulong Multiplier;
        /// <summary>
        /// 最高位 % divisor 模数（余数）
        /// </summary>
        internal uint HighBitMod;
        /// <summary>
        /// 最高位 / divisor 商
        /// </summary>
        internal uint HighBitQuotient;
        /// <summary>
        /// 31b 整数除法 a/b = a*(s/b)/s
        /// </summary>
        /// <param name="divisor">31b 除数，必须大于 0</param>
        /// <param name="maxValue">最大被除数</param>
        internal IntegerDivision(int divisor, uint maxValue = uint.MaxValue)
        {
            Divisor = (uint)divisor;
            ShiftBit = 31 + Divisor.bits();//产生 32b multiplier
            ulong shiftValue = 1UL << ShiftBit;
            Multiplier = (shiftValue - 1) / Divisor + 1;
            if (maxValue * (Multiplier * Divisor - shiftValue) < shiftValue) HighBitQuotient = HighBitMod = 0;
            else
            {
                HighBitQuotient = (uint)(((Multiplier << 31) - Multiplier) >> ShiftBit);
                HighBitMod = (1U << 31) - HighBitQuotient * Divisor;
                if (Divisor == HighBitMod)
                {
                    ++HighBitQuotient;
                    HighBitMod = 0;
                }
            }
        }
        /// <summary>
        /// 设置计算参数 a/b = a*(s/b)/s
        /// </summary>
        /// <param name="divisor">31b 除数，必须大于 0</param>
        /// <param name="maxValue">最大被除数</param>
        internal void Set(int divisor, uint maxValue = uint.MaxValue)
        {
            Divisor = (uint)divisor;
            ShiftBit = 31 + Divisor.bits();//产生 32b multiplier
            ulong shiftValue = 1UL << ShiftBit;
            Multiplier = (shiftValue - 1) / Divisor + 1;
            if (maxValue * (Multiplier * Divisor - shiftValue) < shiftValue) HighBitQuotient = HighBitMod = 0;
            else
            {
                HighBitQuotient = (uint)(((Multiplier << 31) - Multiplier) >> ShiftBit);
                HighBitMod = (1U << 31) - HighBitQuotient * Divisor;
                if (Divisor == HighBitMod)
                {
                    ++HighBitQuotient;
                    HighBitMod = 0;
                }
            }
        }
        /// <summary>
        /// 获取商
        /// </summary>
        /// <param name="dividend">被除数</param>
        /// <returns>商</returns>
        internal uint GetQuotient(uint dividend)
        {
            if (HighBitQuotient == 0) return (uint)((dividend * Multiplier) >> ShiftBit);
            if (dividend <= int.MaxValue) return (uint)((dividend * Multiplier) >> ShiftBit);
            uint quotient = (uint)(((dividend &= int.MaxValue) * Multiplier) >> ShiftBit) + HighBitQuotient;
            dividend -= quotient * Divisor;
            return dividend < Divisor ? quotient : (quotient + 1);
        }
        /// <summary>
        /// 获取商
        /// </summary>
        /// <param name="dividend">被除数，必须大于 0</param>
        /// <returns>商</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetQuotient(int dividend)
        {
            return (int)(((uint)dividend * Multiplier) >> ShiftBit);
        }
        /// <summary>
        /// 获取模数（余数）
        /// </summary>
        /// <param name="dividend">被除数</param>
        /// <returns>模数（余数）</returns>
        internal uint GetMod(uint dividend)
        {
            if (HighBitQuotient == 0 || dividend <= int.MaxValue) return dividend - (uint)((dividend * Multiplier) >> ShiftBit) * Divisor;
            dividend &= int.MaxValue;
            uint mod = dividend - (uint)((dividend * Multiplier) >> ShiftBit) * Divisor + HighBitMod, highMod = mod - Divisor;
            return (int)highMod < 0 ? mod : highMod;
        }
        /// <summary>
        /// 获取模数（余数）
        /// </summary>
        /// <param name="dividend">被除数，必须大于 0</param>
        /// <returns>模数（余数）</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int GetMod(int dividend)
        {
            return dividend - (int)(((uint)dividend * Multiplier) >> ShiftBit) * (int)Divisor;
        }
    }
}
