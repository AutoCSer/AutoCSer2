using System;
using System.Runtime.InteropServices;

namespace System
{
    /// <summary>
    /// .NET8 类型定义（用于二进制序列化兼容操作）
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = sizeof(ulong) * 2)]
    public struct Int128 : IEquatable<Int128>
    {
        /// <summary>
        /// 低 64b
        /// </summary>
        private readonly ulong lower;
        /// <summary>
        /// 高 64b
        /// </summary>
        private readonly ulong upper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="upper"></param>
        /// <param name="lower"></param>
        internal Int128(ulong upper, ulong lower)
        {
            this.lower = lower;
            this.upper = upper;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Int128 other)
        {
            return lower == other.lower && upper == other.upper;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((Int128)obj);
        }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (lower ^ upper).GetHashCode();
        }
    }
}
