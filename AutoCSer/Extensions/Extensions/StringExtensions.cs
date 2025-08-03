using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions
{
    /// <summary>
    /// String expansion operation
    /// 字符串扩展操作
    /// </summary>
    public partial struct StringExtensions
    {
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="splitChar">分割符</param>
        /// <returns>字符子串集合</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public LeftArray<SubString> Split(char splitChar)
        {
            return value.split(splitChar);
        }
        /// <summary>
        /// 计算 64 位稳定 HASH 值
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ulong GetHashCode64()
        {
            return value.getHashCode64();
        }
    }
}
