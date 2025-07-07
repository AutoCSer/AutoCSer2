using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server-side byte array message
    /// 服务端字节数组消息
    /// </summary>
    public sealed class ServerByteArrayMessage : Message<ServerByteArrayMessage>
    {
        /// <summary>
        /// Server-side byte array message data
        /// 服务端字节数组 消息数据
        /// </summary>
        private ServerByteArray message;
        /// <summary>
        /// Server-side byte array message
        /// 服务端字节数组消息
        /// </summary>
        /// <param name="message">Server-side byte array message data
        /// 服务端字节数组 消息数据</param>
        public ServerByteArrayMessage(ServerByteArray message)
        {
            this.message = message;
        }
        /// <summary>
        /// Server-side implicit conversion
        /// 服务端隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ServerByteArrayMessage(ServerByteArray value) { return new ServerByteArrayMessage(value); }
        /// <summary>
        /// Client-side implicit conversion
        /// 客户端隐式转换
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator ServerByteArrayMessage(byte[]? value) { return new ServerByteArrayMessage(value); }
#else
        public static implicit operator ServerByteArrayMessage(byte[] value) { return new ServerByteArrayMessage(value); }
#endif
        /// <summary>
        /// Client-side implicit conversion
        /// 客户端隐式转换
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator ServerByteArrayMessage(string? value) { return new ServerByteArrayMessage(value); }
#else
        public static implicit operator ServerByteArrayMessage(string value) { return new ServerByteArrayMessage(value); }
#endif
        /// <summary>
        /// Get a binary serialization object
        /// 获取二进制序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ServerByteArrayMessage BinarySerialize<T>(T? value)
#else
        public static ServerByteArrayMessage BinarySerialize<T>(T value)
#endif
        {
            return ServerByteArray.BinarySerialize(value);
        }
        /// <summary>
        /// Get the JSON serialization object
        /// 获取 JSON 序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ServerByteArrayMessage JsonSerialize<T>(T? value)
#else
        public static ServerByteArrayMessage JsonSerialize<T>(T value)
#endif
        {
            return ServerByteArray.JsonSerialize(value);
        }

        /// <summary>
        /// Implicit conversion
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator byte[]?(ServerByteArrayMessage value) { return value.message.Buffer; }
#else
        public static implicit operator byte[](ServerByteArrayMessage value) { return value.message.Buffer; }
#endif
        /// <summary>
        /// Get string data
        /// 获取字符串数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Whether the string parsing was successful
        /// 字符串解析是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public bool GetString(ref string? value)
#else
        public bool GetString(ref string value)
#endif
        {
            return message.GetString(ref value);
        }
        /// <summary>
        /// Get the JSON deserialized object
        /// 获取 JSON 反序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Whether deserialization was successful
        /// 反序列化是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public bool JsonDeserialize<T>(ref T? value)
#else
        public bool JsonDeserialize<T>(ref T value)
#endif
        {
            return message.JsonDeserialize(ref value);
        }
        /// <summary>
        /// Get the binary deserialized object
        /// 获取二进制反序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Whether deserialization was successful
        /// 反序列化是否成功</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public bool BinaryDeserialize<T>(ref T? value)
#else
        public bool BinaryDeserialize<T>(ref T value)
#endif
        {
            return message.BinaryDeserialize(ref value);
        }
    }
}
