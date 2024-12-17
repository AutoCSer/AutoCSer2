using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 返回服务端字节数组序列化
    /// </summary>
    public sealed class ResponseServerByteArraySerializer : ResponseParameterSerializer
    {
        /// <summary>
        /// 服务端字节数组
        /// </summary>
#if NetStandard21
        private readonly byte[]? buffer;
#else
        private readonly byte[] buffer;
#endif
        /// <summary>
        /// 返回服务端字节数组序列化
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
        /// 隐式转换
        /// </summary>
        /// <param name="value">服务端字节数组</param>
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
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        public override void Serialize(AutoCSer.BinarySerializer serializer)
        {
            serializer.SerializeBuffer(buffer);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns>目标对象</returns>
#if NetStandard21
        internal override object? Deserialize(AutoCSer.BinaryDeserializer deserializer)
#else
        internal override object Deserialize(AutoCSer.BinaryDeserializer deserializer)
#endif
        {
            throw new InvalidOperationException();
        }
        /// <summary>
        /// null 字节数组
        /// </summary>
        public static readonly ResponseServerByteArraySerializer Null = new ResponseServerByteArraySerializer(null);
        /// <summary>
        /// 0 字节数组
        /// </summary>
        public static readonly ResponseServerByteArraySerializer EmptyArray = new ResponseServerByteArraySerializer(EmptyArray<byte>.Array);
    }
}
