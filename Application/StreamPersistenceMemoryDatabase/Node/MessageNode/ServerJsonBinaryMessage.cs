using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端 JSON 字符串二进制序列化数据 / 客户端对象 消息
    /// </summary>
    /// <typeparam name="T">消息数据对象类型</typeparam>
    public sealed class ServerJsonBinaryMessage<T> : Message<ServerJsonBinaryMessage<T>>
    {
        /// <summary>
        /// 服务端 JSON 字符串二进制序列化数据 / 客户端对象 消息数据
        /// </summary>
        private ServerJsonBinary<T> message;
        /// <summary>
        /// 服务端 JSON 字符串二进制序列化数据 / 客户端对象 消息
        /// </summary>
        /// <param name="message">服务端 JSON 字符串二进制序列化数据 / 客户端对象 消息数据</param>
        public ServerJsonBinaryMessage(T message)
        {
            this.message = message;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator ServerJsonBinaryMessage<T>(T message) { return new ServerJsonBinaryMessage<T>(message); }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator T(ServerJsonBinaryMessage<T> message) { return message.message.Value; }
    }
}
