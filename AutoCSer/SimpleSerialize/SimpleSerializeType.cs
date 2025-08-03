using System;

namespace AutoCSer
{
    /// <summary>
    /// 简单序列化类型信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class SimpleSerializeType<T>
    {
        /// <summary>
        /// 是否简单序列化
        /// </summary>
        internal static readonly bool IsSimple = SimpleSerialize.Serializer.IsType(typeof(T));
    }
}
