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
#if NetStandard21
        public ServerJsonBinaryMessage(T? message)
#else
        public ServerJsonBinaryMessage(T message)
#endif
        {
            this.message = message;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="message"></param>
#if NetStandard21
        public static implicit operator ServerJsonBinaryMessage<T>(T? message) { return new ServerJsonBinaryMessage<T>(message); }
#else
        public static implicit operator ServerJsonBinaryMessage<T>(T message) { return new ServerJsonBinaryMessage<T>(message); }
#endif
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="message"></param>
#if NetStandard21
        public static implicit operator T?(ServerJsonBinaryMessage<T> message) { return message.message.Value; }
#else
        public static implicit operator T(ServerJsonBinaryMessage<T> message) { return message.message.Value; }
#endif
    }
}
