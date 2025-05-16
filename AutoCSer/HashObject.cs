using AutoCSer.Extensions;
using System;

namespace AutoCSer
{
    /// <summary>
    /// 包装 IEquatable 对象，用于 Hash 比较
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct HashObject<T> : IEquatable<HashObject<T>>
        where T : class
    {
        /// <summary>
        /// 类型
        /// </summary>
        public T Value;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(HashObject<T> other)
        {
            return Value == other.Value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
#if NetStandard21
        public override bool Equals(object? obj)
#else
        public override bool Equals(object obj)
#endif
        {
            return Equals(obj.castValue<HashObject<T>>());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator HashObject<T>(T value) { return new HashObject<T> { Value = value }; }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator T(HashObject<T> value) { return value.Value; }
    }
}
