using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Keep callback return parameter binary deserialization
    /// 保持回调返回参数二进制反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class KeepCallbackResponseParameterBinarySerializer<T> : ResponseParameterSerializer
    {
        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        public override void Serialize(AutoCSer.BinarySerializer serializer)
        {
            throw new InvalidOperationException();
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
            ServerReturnValue<T> value = default(ServerReturnValue<T>);
            if (AutoCSer.SimpleSerializeType<T>.IsSimple ? deserializer.SimpleDeserialize(ref value) : deserializer.InternalIndependentDeserializeNotReference(ref value))
            {
                return new KeepCallbackResponseDeserializeValue<T>(value.ReturnValue);
            }
            return null;
        }

        /// <summary>
        /// Default deserialization
        /// 默认反序列化
        /// </summary>
        internal static readonly KeepCallbackResponseParameterBinarySerializer<T> Default = new KeepCallbackResponseParameterBinarySerializer<T>();
    }
}
