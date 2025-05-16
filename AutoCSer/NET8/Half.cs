using System;
using System.Runtime.InteropServices;

namespace System
{
    /// <summary>
    /// .NET8 类型定义（用于二进制序列化兼容操作）
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = sizeof(ushort))]
    public struct Half : IEquatable<Half>
    {
        /// <summary>
        /// 16b
        /// </summary>
        private readonly ushort value;
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Half other)
        {
            return value == other.value;
        }
        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((Half)obj);
        }
        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return value;
        }
    }
}
