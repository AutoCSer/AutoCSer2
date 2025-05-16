using AutoCSer.Extensions;
using System;

namespace AutoCSer
{
    /// <summary>
    /// 对象引用（用于序列化循环引用比较）
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    internal struct ObjectReferenceType : IEquatable<ObjectReferenceType>
    {
        /// <summary>
        /// 对象
        /// </summary>
        private object value;
        /// <summary>
        /// 序列化类型
        /// </summary>
        private Type type;
        /// <summary>
        /// 对象引用
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        internal ObjectReferenceType(object value, Type type)
        {
            this.value = value;
            this.type = type;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ObjectReferenceType other)
        {
            if (object.ReferenceEquals(value, other.value)) return type == other.type;
            return type == typeof(string) && other.type == typeof(string) && (string)value == (string)other.value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            try
            {
                return value.GetHashCode() ^ type.GetHashCode();
            }
            catch
            {
                return type.GetHashCode();
            }
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
            return Equals(obj.castValue<ObjectReferenceType>());
        }
    }
}
