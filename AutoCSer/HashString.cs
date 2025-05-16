using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Net;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// 字符串 HASH
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    public struct HashString : IEquatable<HashString>, AutoCSer.BinarySerialize.ICustomSerialize<HashString>
    {
        /// <summary>
        /// 哈希值
        /// </summary>
        internal ulong HashCode;
        /// <summary>
        /// 字符子串
        /// </summary>
        internal string String;
        /// <summary>
        /// 字符串 HASH
        /// </summary>
        /// <param name="value"></param>
        public HashString(string value)
        {
            String = value;
            HashCode = getHashCode64(value) ^ Random.Hash64;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>字符串</returns>
        public static implicit operator HashString(string value) { return new HashString(value); }
        /// <summary>
        /// 清空数据
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetEmpty()
        {
            String = string.Empty;
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
        /// HASH值
        /// </summary>
        /// <returns>HASH值</returns>
        private static unsafe ulong getHashCode64(string value)
        {
            if (string.IsNullOrEmpty(value)) return 0;
            fixed (char* valueFixed = value) return AutoCSer.Memory.Common.GetHashCode64((byte*)valueFixed, value.Length << 1);
        }
        /// <summary>
        /// 判断字符串是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Equals(HashString other)
        {
            return HashCode == other.HashCode && String == other.String;
        }
        /// <summary>
        /// 判断字符串是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Equals(ref HashString other)
        {
            return HashCode == other.HashCode && String == other.String;
        }
        /// <summary>
        /// 判断字符串是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
#if NetStandard21
        public override bool Equals(object? obj)
#else
        public override bool Equals(object obj)
#endif
        {
            return Equals(obj.castValue<HashString>());
        }
        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<HashString>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.Stream.Write(HashCode ^ Random.Hash64);
            serializer.PrimitiveSerialize(String);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<HashString>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            if (deserializer.Read(out HashCode))
            {
                var stringValue = default(string);
                deserializer.PrimitiveDeserialize(ref stringValue);
                HashCode ^= Random.Hash64;
                String = stringValue ?? string.Empty;
            }
        }

        /// <summary>
        /// 长度为 0 的字符串
        /// </summary>
        public static readonly HashString Empty = new HashString(string.Empty);
    }
}
