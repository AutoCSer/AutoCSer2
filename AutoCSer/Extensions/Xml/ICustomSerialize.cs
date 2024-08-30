using System;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 仅用于快速判断是否可能实现接口 ICustomSerialize{T}
    /// </summary>
    public interface ICustomSerialize { }
    /// <summary>
    /// 自定义序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICustomSerialize<T> : ICustomSerialize
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void Serialize(AutoCSer.XmlSerializer serializer);
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        void Deserialize(AutoCSer.XmlDeserializer deserializer);
    }
}
