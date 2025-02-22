using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Algorithm
{
    /// <summary>
    /// 整数取余，可用于哈希表操作
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ModMultiplier
    {
        /// <summary>
        /// 取余转位移乘法乘数
        /// </summary>
        private readonly ulong multiplier;
        /// <summary>
        /// 取余的除数
        /// </summary>
        private readonly uint divisor;
        /// <summary>
        /// 整数取余
        /// </summary>
        /// <param name="divisor">取余的除数，必须大于 0</param>
        public ModMultiplier(int divisor)
        {
            multiplier = ulong.MaxValue / (uint)divisor + 1;//被除数乘以 multiplier 后，商的整数部分在 64b 以上溢出，得到商的 64b 浮点小数
            this.divisor = (uint)divisor;
        }
        /// <summary>
        /// 取余
        /// </summary>
        /// <param name="value">被除数</param>
        /// <returns>模数（余数）</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public uint GetMod(uint value)
        {
            return (uint)(((((multiplier * value) >> 32) + 1) * divisor) >> 32);//商的 64b 浮点小数转换为 32b 浮点小数（+1 补精度），然后乘以除数，高 32b 为余数
        }
    }
}
