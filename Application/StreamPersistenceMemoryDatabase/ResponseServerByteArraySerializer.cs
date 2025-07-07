using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server-side byte array serialization
    /// 服务端字节数组序列化
    /// </summary>
    public sealed class ResponseServerByteArraySerializer : ResponseParameterSerializer
    {
        /// <summary>
        /// Server-side byte array
        /// 服务端字节数组
        /// </summary>
#if NetStandard21
        private readonly byte[]? buffer;
#else
        private readonly byte[] buffer;
#endif
        /// <summary>
        /// Server-side byte array serialization
        /// 服务端字节数组序列化
        /// </summary>
        /// <param name="buffer"></param>
#if NetStandard21
        private ResponseServerByteArraySerializer(byte[]? buffer)
#else
        private ResponseServerByteArraySerializer(byte[] buffer)
#endif
        {
            this.buffer = buffer;
        }
        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value">Server-side byte array
        /// 服务端字节数组</param>
        /// <returns></returns>
#if NetStandard21
        public static implicit operator ResponseServerByteArraySerializer(byte[]? value)
#else
        public static implicit operator ResponseServerByteArraySerializer(byte[] value)
#endif
        {
            if (value != null)
            {
                if (value.Length != 0) return new ResponseServerByteArraySerializer(value);
                return EmptyArray;
            }
            return Null;
        }
        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        public override void Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.SerializeBuffer(buffer);
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
            throw new InvalidOperationException();
        }
        /// <summary>
        /// A null byte array
        /// null 的字节数组
        /// </summary>
        public static readonly ResponseServerByteArraySerializer Null = new ResponseServerByteArraySerializer(null);
        /// <summary>
        /// The byte array of the 0-length array
        /// 0 长度数组的字节数组
        /// </summary>
        public static readonly ResponseServerByteArraySerializer EmptyArray = new ResponseServerByteArraySerializer(EmptyArray<byte>.Array);
    }
}
