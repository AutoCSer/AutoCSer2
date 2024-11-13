using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端二进制序列化数据 / 客户端对象，用于客户端对象与二进制序列化数据适配，减少不必要的序列化开销（比如服务端存二进制序列化数据，客户端处理为对象）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    public struct ServerBinary<T> : AutoCSer.BinarySerialize.ICustomSerialize<ServerBinary<T>>
    {
        /// <summary>
        /// 服务端二进制序列化数据
        /// </summary>
#if NetStandard21
        private byte[]? buffer;
#else
        private byte[] buffer;
#endif
        /// <summary>
        /// 客户端对象
        /// </summary>
#if NetStandard21
        public T? Value;
#else
        public T Value;
#endif
        /// <summary>
        /// 客户端对象
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public ServerBinary(T? value)
#else
        public ServerBinary(T value)
#endif
        {
            Value = value;
            buffer = null;
        }
        /// <summary>
        /// 客户端隐式转换
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator ServerBinary<T>(T? value) { return new ServerBinary<T>(value); }
#else
        public static implicit operator ServerBinary<T>(T value) { return new ServerBinary<T>(value); }
#endif
        /// <summary>
        /// 客户端隐式转换
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator T?(ServerBinary<T> value) { return value.Value; }
#else
        public static implicit operator T(ServerBinary<T> value) { return value.Value; }
#endif

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<ServerBinary<T>>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            var type = serializer.Context?.GetType();
            if (type == typeof(CommandServerSocket)) serializer.SerializeBuffer(buffer);
            else serializer.InternalIndependentSerializeNotReference(ref Value);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<ServerBinary<T>>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            var type = deserializer.Context?.GetType();
            if (type == typeof(CommandServerSocket)) deserializer.DeserializeBuffer(ref buffer);
            else deserializer.InternalIndependentDeserializeNotReference(ref Value);
        }
    }
}
