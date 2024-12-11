using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 字节数组消息，如果是确定类型的二进制序列化数据建议使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerBinaryMessage{T} 以减少反序列化开销
    /// </summary>
    public sealed class ByteArrayMessage : Message<ByteArrayMessage>
    {
        /// <summary>
        /// 字节数组消息数据
        /// </summary>
        private byte[] message;
        /// <summary>
        /// 字节数组消息
        /// </summary>
        /// <param name="message">字节数组消息数据</param>
        public ByteArrayMessage(byte[] message)
        {
            this.message = message;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator ByteArrayMessage(byte[] message) { return new ByteArrayMessage(message); }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator byte[](ByteArrayMessage message) { return message.message; }
    }
}
