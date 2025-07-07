using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Return parameter binary serialization
    /// 返回参数二进制序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ResponseParameterBinarySerializer<T> : ResponseParameterSerializer
    {
        /// <summary>
        /// Return data
        /// </summary>
        internal ServerReturnValue<T> Value;
        /// <summary>
        /// Return parameter binary serialization
        /// 返回参数二进制序列化
        /// </summary>
        internal ResponseParameterBinarySerializer() { }
        /// <summary>
        /// Return parameter binary serialization
        /// 返回参数二进制序列化
        /// </summary>
        /// <param name="value">Return data</param>
#if NetStandard21
        internal ResponseParameterBinarySerializer(T? value)
#else
        internal ResponseParameterBinarySerializer(T value)
#endif
        {
            this.Value = new ServerReturnValue<T>(value);
        }
        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        public override void Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.InternalIndependentSerializeNotNull(ref Value);
        }
        /// <summary>
        /// Deserialization
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns>Target object</returns>
#if NetStandard21
        internal override object? Deserialize(AutoCSer.BinaryDeserializer deserializer)
#else
        internal override object Deserialize(AutoCSer.BinaryDeserializer deserializer)
#endif
        {
            return deserializer.InternalIndependentDeserializeNotReference(ref Value) ? this : null;
        }
    }
}
