using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端二进制序列化数据 / 客户端对象 消息
    /// </summary>
    /// <typeparam name="T">消息数据对象类型</typeparam>
    public sealed class ServerBinaryMessage<T> : Message<ServerBinaryMessage<T>>
    {
        /// <summary>
        /// 服务端二进制序列化数据 / 客户端对象 消息数据
        /// </summary>
        private ServerBinary<T> message;
        /// <summary>
        /// 服务端二进制序列化数据 / 客户端对象 消息
        /// </summary>
        /// <param name="message">服务端二进制序列化数据 / 客户端对象 消息数据</param>
        public ServerBinaryMessage(T message)
        {
            this.message = message;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator ServerBinaryMessage<T>(T message) { return new ServerBinaryMessage<T>(message); }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator T(ServerBinaryMessage<T> message) { return message.message.Value; }
    }
}
