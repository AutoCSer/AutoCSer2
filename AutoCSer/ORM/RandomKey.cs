using System;

namespace AutoCSer
{
    /// <summary>
    /// 随机防 HASH 构造关键字
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    public struct RandomKey<T> : IEquatable<RandomKey<T>>
        where T : IEquatable<T>
    {
        /// <summary>
        /// 关键字
        /// </summary>
        internal T Key;
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator RandomKey<T>(T value)
        {
            return new RandomKey<T> { Key = value };
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator T(RandomKey<T> value)
        {
            return value.Key;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Key.GetHashCode() ^ Random.Hash;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(RandomKey<T> other)
        {
            return Key.Equals(other.Key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(T other)
        {
            return Key.Equals(other);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((RandomKey<T>)obj);
        }
    }
}
