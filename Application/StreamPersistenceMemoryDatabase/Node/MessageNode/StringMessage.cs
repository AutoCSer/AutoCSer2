using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 字符串消息，如果是确定类型的 JSON 字符串建议使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerJsonBinaryMessage{T} 或者 ServerJsonMessage{T} 以减少反序列化开销
    /// </summary>
    public sealed class StringMessage : Message<StringMessage>
    {
        /// <summary>
        /// 字符串消息数据
        /// </summary>
        private string message;
        /// <summary>
        /// 字符串消息
        /// </summary>
        /// <param name="message">字符串消息数据</param>
        public StringMessage(string message)
        {
            this.message = message;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator StringMessage(string message) { return new StringMessage(message); }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator string(StringMessage message) { return message.message; }
    }
}
