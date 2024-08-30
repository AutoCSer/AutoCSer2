using AutoCSer.Net;
using System;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// 服务端 JSON 字符串 / 客户端对象，用于客户端对象与服务端 JSON 字符串适配，减少不必要的序列化开销（比如服务端存 JSON 字符串，客户端处理为对象）
    /// 如果服务端不需要使用 JSON 字符串建议使用 AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerJsonBinary{T} 以减少内存空间占用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsMemberMap = false, IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    public struct ServerJson<T> : AutoCSer.BinarySerialize.ICustomSerialize<ServerJson<T>>
    {
        /// <summary>
        /// 服务端 JSON 字符串
        /// </summary>
        public string Json { get; private set; }
        /// <summary>
        /// 客户端对象
        /// </summary>
        public T Value;
        /// <summary>
        /// 客户端对象
        /// </summary>
        /// <param name="value"></param>
        public ServerJson(T value)
        {
            Value = value;
            Json = null;
        }
        /// <summary>
        /// 服务端 JSON 字符串
        /// </summary>
        /// <param name="json"></param>
        internal ServerJson(string json)
        {
            this.Json = json;
            Value = default(T);
        }
        /// <summary>
        /// 服务端隐式转换为字符串
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator string(ServerJson<T> value) { return value.Json; }
        /// <summary>
        /// 客户端隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ServerJson<T>(T value) { return new ServerJson<T>(value); }
        /// <summary>
        /// 客户端隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator T(ServerJson<T> value) { return value.Value; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<ServerJson<T>>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            int index = serializer.SerializeBufferStart();
            if (index >= 0)
            {
                Type type = serializer.Context?.GetType();
                if (type == typeof(CommandServerSocket)) serializer.SerializeBufferEnd(index, serializer.SerializeBuffer(Json));
                else if (type == typeof(CommandClientSocket)) serializer.SerializeBufferEnd(index, ((CommandClientSocket)serializer.Context).JsonSerializeBuffer(ref Value, serializer.Stream));
                else serializer.SerializeBufferEnd(index, serializer.GetJsonSerializer().SerializeCommandServerBuffer(ref Value, serializer.Stream));
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<ServerJson<T>>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            byte* end = deserializer.DeserializeBufferStart();
            if (end != null)
            {
                Type type = deserializer.Context?.GetType();
                if (type == typeof(CommandServerSocket))
                {
                    string json = Json;
                    deserializer.Deserialize(ref json);
                    Json = json;
                }
                else if (type == typeof(CommandClientSocket)) deserializer.DeserializeJson(((CommandClientSocket)deserializer.Context).ReceiveJsonDeserializer, out Value);
                else deserializer.DeserializeJson(out Value);
                deserializer.DeserializeBufferEnd(end);
            }
        }
    }
}
