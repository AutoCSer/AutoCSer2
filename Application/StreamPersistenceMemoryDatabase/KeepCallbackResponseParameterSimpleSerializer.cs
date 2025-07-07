using AutoCSer.Net.CommandServer;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Keep callback return parameters simply deserialized
    /// 保持回调返回参数简单反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class KeepCallbackResponseParameterSimpleSerializer<T> : ResponseParameterSerializer
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
            return deserializer.SimpleDeserialize(ref value) ? new KeepCallbackResponseDeserializeValue<T>(value.ReturnValue) : null;
        }

        /// <summary>
        /// Default deserialization
        /// 默认反序列化
        /// </summary>
        internal static readonly KeepCallbackResponseParameterSimpleSerializer<T> Default = new KeepCallbackResponseParameterSimpleSerializer<T>();
    }
}
