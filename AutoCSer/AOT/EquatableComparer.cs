using System;

namespace AutoCSer
{
    /// <summary>
    /// Dictionary key comparator
    /// 字典关键字比较器
    /// </summary>
    public sealed class EquatableComparer<T> : System.Collections.Generic.IEqualityComparer<T>
        where T : IEquatable<T>
    {
        /// <summary>
        /// Compare whether they are equal
        /// 比较是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        bool System.Collections.Generic.IEqualityComparer<T>.Equals(T? left, T? right)
        {
            return left != null ? left.Equals(right) : right == null;
        }
        /// <summary>
        /// Calculate the hash value
        /// 计算哈希值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int System.Collections.Generic.IEqualityComparer<T>.GetHashCode(T value)
        {
            return value.GetHashCode();
        }
        /// <summary>
        /// Default comparator
        /// 默认比较器
        /// </summary>
        public static readonly EquatableComparer<T> Default = new EquatableComparer<T>();
    }
}
