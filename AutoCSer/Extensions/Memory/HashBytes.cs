using AutoCSer.Extensions;
using System;

namespace AutoCSer.Memory
{
    /// <summary>
    /// 用于 HASH 的字节数组
    /// </summary>
    [RemoteType]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct HashBytes : IEquatable<HashBytes>
    {
        /// <summary>
        /// 字节数组
        /// </summary>
        internal SubArray<byte> SubArray;
        /// <summary>
        /// HASH 值
        /// </summary>
        internal ulong HashCode;
        /// <summary>
        /// 字节数组 HASH
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="hashCode"></param>
        internal unsafe HashBytes(SubArray<byte> data, ulong hashCode)
        {
            SubArray = data;
            HashCode = hashCode;
        }
        /// <summary>
        /// 字节数组 HASH
        /// </summary>
        /// <param name="data">字节数组</param>
        internal unsafe HashBytes(SubArray<byte> data)
        {
            SubArray = data;
            if (data.Length == 0) HashCode = Random.Hash64;
            else
            {
                fixed (byte* dataFixed = data.GetFixedBuffer()) HashCode = AutoCSer.Memory.Common.GetHashCode64(dataFixed + data.Start, data.Length) ^ Random.Hash64;
            }
        }
        /// <summary>
        /// HASH字节数组隐式转换
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns>HASH字节数组</returns>
        public static implicit operator HashBytes(SubArray<byte> data) { return new HashBytes(data); }
        /// <summary>
        /// HASH字节数组隐式转换
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <returns>HASH字节数组</returns>
        public static implicit operator HashBytes(byte[] data) { return new HashBytes(new SubArray<byte>(data)); }
        /// <summary>
        /// HASH字节数组隐式转换
        /// </summary>
        /// <param name="data">HASH字节数组</param>
        /// <returns>字节数组</returns>
        public static implicit operator SubArray<byte>(HashBytes data) { return data.SubArray; }
        /// <summary>
        /// 比较字节数组是否相等
        /// </summary>
        /// <param name="other">用于HASH的字节数组</param>
        /// <returns>Is it equal
        /// 是否相等</returns>
        public unsafe bool Equals(HashBytes other)
        {
            return HashCode == other.HashCode && AutoCSer.Common.SequenceEqual(ref SubArray, ref other.SubArray);
        }
        /// <summary>
        /// 获取 HASH 值
        /// </summary>
        /// <returns>HASH 值</returns>
        public override int GetHashCode()
        {
            return (int)((uint)HashCode ^ (uint)(HashCode >> 32));
        }
        /// <summary>
        /// 比较字节数组是否相等
        /// </summary>
        /// <param name="other">字节数组HASH</param>
        /// <returns>Is it equal
        /// 是否相等</returns>
#if NetStandard21
        public override bool Equals(object? other)
#else
        public override bool Equals(object other)
#endif
        {
            return Equals(other.castValue<HashBytes>());
        }
    }
}
