using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Return parameter simple serialization
    /// 返回参数简单序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class ResponseParameterSimpleSerializer<T> : ResponseParameterSerializer
    {
        /// <summary>
        /// Return data
        /// </summary>
        internal ServerReturnValue<T> Value;
        /// <summary>
        /// Return parameter simple serialization
        /// 返回参数简单序列化
        /// </summary>
        internal ResponseParameterSimpleSerializer() { }
        /// <summary>
        /// Return parameter simple serialization
        /// 返回参数简单序列化
        /// </summary>
        /// <param name="value">Return data</param>
#if NetStandard21
        internal ResponseParameterSimpleSerializer(T? value)
#else
        internal ResponseParameterSimpleSerializer(T value)
#endif
        {
            this.Value.ReturnValue = value;
        }
        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        public override void Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.SimpleSerialize(ref Value);
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
            return deserializer.SimpleDeserialize(ref Value) ? this : null;
        }
    }
}
