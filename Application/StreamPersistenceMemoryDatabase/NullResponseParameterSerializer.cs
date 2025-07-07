using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// The default empty return parameter serialization
    /// 默认空返回参数序列化
    /// </summary>
    internal sealed class NullResponseParameterSerializer : ResponseParameterSerializer
    {
        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        public override void Serialize(AutoCSer.BinarySerializer serializer) { throw new InvalidOperationException(); }
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
            throw new InvalidOperationException();
        }

        /// <summary>
        /// The default empty return parameter serialization
        /// 默认空返回参数序列化
        /// </summary>
        internal static readonly NullResponseParameterSerializer Null = new NullResponseParameterSerializer();
    }
}
