using System;

namespace AutoCSer.Json
{
    /// <summary>
    /// Only used for quickly determining whether it is possible to implement the interface ICustomSerialize{T}
    /// 仅用于快速判断是否可能实现接口 ICustomSerialize{T}
    /// </summary>
    public interface ICustomSerialize { }
    /// <summary>
    /// Custom serialization
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICustomSerialize<T> : ICustomSerialize
    {
        /// <summary>
        /// Serialization
        /// </summary>
        /// <param name="serializer"></param>
        void Serialize(AutoCSer.JsonSerializer serializer);
        /// <summary>
        /// Deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        void Deserialize(AutoCSer.JsonDeserializer deserializer);
    }
}
