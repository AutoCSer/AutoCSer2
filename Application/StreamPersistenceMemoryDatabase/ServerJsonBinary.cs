using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端 JSON 字符串二进制序列化数据 / 客户端对象，用于客户端对象与服务端 JSON 字符串二进制序列化数据适配，减少不必要的序列化与内存空间开销（比如服务端存 JSON 字符串二进制序列化数据，客户端处理为对象）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    public struct ServerJsonBinary<T> : AutoCSer.BinarySerialize.ICustomSerialize<ServerJsonBinary<T>>
    {
        /// <summary>
        /// 服务端 JSON 字符串二进制序列化数据
        /// </summary>
        private byte[] buffer;
        /// <summary>
        /// 客户端对象
        /// </summary>
        public T Value;
        /// <summary>
        /// 客户端对象
        /// </summary>
        /// <param name="value"></param>
        public ServerJsonBinary(T value)
        {
            Value = value;
            buffer = null;
        }
        /// <summary>
        /// 客户端隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ServerJsonBinary<T>(T value) { return new ServerJsonBinary<T>(value); }
        /// <summary>
        /// 客户端隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator T(ServerJsonBinary<T> value) { return value.Value; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<ServerJsonBinary<T>>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            Type type = serializer.Context?.GetType();
            if (type == typeof(CommandServerSocket)) serializer.SerializeBuffer(buffer);
            else
            {
                int index = serializer.SerializeBufferStart();
                if (index >= 0)
                {
                    if (type == typeof(CommandClientSocket)) serializer.SerializeBufferEnd(index, ((CommandClientSocket)serializer.Context).JsonSerializeBuffer(ref Value, serializer.Stream));
                    else serializer.SerializeBufferEnd(index, serializer.GetJsonSerializer().SerializeCommandServerBuffer(ref Value, serializer.Stream));
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<ServerJsonBinary<T>>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            Type type = deserializer.Context?.GetType();
            if (type == typeof(CommandServerSocket)) deserializer.DeserializeBuffer(ref buffer);
            else
            {
                byte* end = deserializer.DeserializeBufferStart();
                if (end != null)
                {
                    if (type == typeof(CommandClientSocket)) deserializer.DeserializeJson(((CommandClientSocket)deserializer.Context).ReceiveJsonDeserializer, out Value);
                    else deserializer.DeserializeJson(out Value);
                    deserializer.DeserializeBufferEnd(end);
                }
            }
        }
    }
}
