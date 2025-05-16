using AutoCSer.Extensions;
using System;

namespace AutoCSer
{
    /// <summary>
    /// 引用哈希关键字
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct ReferenceHashKey<T> : IEquatable<ReferenceHashKey<T>> where T : class
    {
        /// <summary>
        /// 哈希关键字
        /// </summary>
        internal readonly T Value;
        /// <summary>
        /// 哈希关键字
        /// </summary>
        /// <param name="value">关键字</param>
        private ReferenceHashKey(T value)
        {
            Value = value;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value">哈希关键字</param>
        /// <returns>引用哈希关键字</returns>
        public static implicit operator ReferenceHashKey<T>(T value) { return new ReferenceHashKey<T>(value); }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ReferenceHashKey<T> other)
        {
            return object.ReferenceEquals(Value, other.Value);
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
            return Equals(obj.castValue<ReferenceHashKey<T>>());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
