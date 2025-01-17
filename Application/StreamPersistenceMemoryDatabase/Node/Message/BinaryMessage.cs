using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 二进制序列化消息
    /// </summary>
    /// <typeparam name="T">消息数据对象类型</typeparam>
    public sealed class BinaryMessage<T> : Message<BinaryMessage<T>>
    {
        /// <summary>
        /// 二进制序列化消息数据
        /// </summary>
        private T message;
        /// <summary>
        /// 二进制序列化消息
        /// </summary>
        /// <param name="message">二进制序列化消息数据</param>
        public BinaryMessage(T message)
        {
            this.message = message;
        }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator BinaryMessage<T>(T message) { return new BinaryMessage<T>(message); }
        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="message"></param>
        public static implicit operator T(BinaryMessage<T> message) { return message.message; }
        /// <summary>
        /// 获取消息数据
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public T Get() { return message; }
    }
}
