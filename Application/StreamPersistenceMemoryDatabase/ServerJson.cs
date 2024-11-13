using AutoCSer.Extensions;
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
#if NetStandard21
        public string? Json { get; private set; }
#else
        public string Json { get; private set; }
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
        public ServerJson(T? value)
#else
        public ServerJson(T value)
#endif
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
#if NetStandard21
        public static implicit operator string?(ServerJson<T> value) { return value.Json; }
#else
        public static implicit operator string(ServerJson<T> value) { return value.Json; }
#endif
        /// <summary>
        /// 客户端隐式转换
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator ServerJson<T>(T? value) { return new ServerJson<T>(value); }
#else
        public static implicit operator ServerJson<T>(T value) { return new ServerJson<T>(value); }
#endif
        /// <summary>
        /// 客户端隐式转换
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator T?(ServerJson<T> value) { return value.Value; }
#else
        public static implicit operator T(ServerJson<T> value) { return value.Value; }
#endif

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<ServerJson<T>>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            int index = serializer.SerializeBufferStart();
            if (index >= 0)
            {
                var type = serializer.Context?.GetType();
                if (type == typeof(CommandServerSocket)) serializer.SerializeBufferEnd(index, serializer.SerializeBuffer(Json));
                else if (type == typeof(CommandClientSocket)) serializer.SerializeBufferEnd(index, serializer.Context.castType<CommandClientSocket>().notNull().JsonSerializeBuffer(ref Value, serializer.Stream));
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
                var type = deserializer.Context?.GetType();
                if (type == typeof(CommandServerSocket))
                {
                    var json = Json;
                    deserializer.Deserialize(ref json);
                    Json = json;
                }
                else if (type == typeof(CommandClientSocket)) deserializer.DeserializeJson(deserializer.Context.castType<CommandClientSocket>().notNull().ReceiveJsonDeserializer, out Value);
                else deserializer.DeserializeJson(out Value);
                deserializer.DeserializeBufferEnd(end);
            }
        }
    }
}
