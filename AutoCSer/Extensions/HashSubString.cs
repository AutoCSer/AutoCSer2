using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 字符串 HASH
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct HashSubString : IEquatable<HashSubString>
    {
        /// <summary>
        /// 字符子串
        /// </summary>
        internal SubString String;
        /// <summary>
        /// 哈希值
        /// </summary>
        internal ulong HashCode;
        /// <summary>
        /// 字符串 HASH
        /// </summary>
        /// <param name="value"></param>
        public HashSubString(SubString value) : this(ref value) { }
        /// <summary>
        /// 字符串 HASH
        /// </summary>
        /// <param name="value"></param>
        public HashSubString(ref SubString value)
        {
            String = value;
            HashCode = value.GetHashCode64() ^ Random.Hash64;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>字符串</returns>
        public static implicit operator HashSubString(string value) { return new HashSubString(value); }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>字符串</returns>
        public static implicit operator HashSubString(SubString value) { return new HashSubString(ref value); }
        /// <summary>
        /// 清空数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetEmpty()
        {
            String.SetEmpty();
            HashCode = Random.Hash64;
        }
        /// <summary>
        /// HASH值
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)((uint)HashCode ^ (uint)(HashCode >> 32));
        }
        /// <summary>
        /// 判断字符串是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Equals(HashSubString other)
        {
            return HashCode == other.HashCode && String.Equals(ref other.String);
        }
        /// <summary>
        /// 判断字符串是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Equals(ref HashSubString other)
        {
            return HashCode == other.HashCode && String.Equals(ref other.String);
        }
        /// <summary>
        /// 判断字符串是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((HashSubString)obj);
        }
        /// <summary>
        /// 判断字符串是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Equals(ref SubString other)
        {
            return String.Equals(ref other);
        }
        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.ToString();
        }

        /// <summary>
        /// 长度为 0 的字符串
        /// </summary>
        public static readonly HashSubString Empty = new HashSubString(string.Empty);
    }
}
