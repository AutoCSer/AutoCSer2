using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端 JSON 字符串 / 客户端对象 消息，如果服务端不需要使用 JSON 字符串建议使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerJsonBinaryMessage{T} 以减少内存空间占用
    /// </summary>
    /// <typeparam name="T">消息数据对象类型</typeparam>
    public sealed class ServerJsonMessage<T> : Message<ServerJsonMessage<T>>
    {
        /// <summary>
        /// 服务端 JSON 字符串 / 客户端对象 消息数据
        /// </summary>
        private ServerJson<T> message;
        /// <summary>
        /// 服务端 JSON 字符串 / 客户端对象 消息
        /// </summary>
        /// <param name="message">服务端 JSON 字符串 / 客户端对象 消息数据</param>
        public ServerJsonMessage(T message)
        {
            this.message = message;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator ServerJsonMessage<T>(T message) { return new ServerJsonMessage<T>(message); }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator T(ServerJsonMessage<T> message) { return message.message.Value; }
    }
}
