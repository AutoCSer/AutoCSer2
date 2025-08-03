using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase
{
    /// <summary>
    /// Server-side byte array/client-side serialized object (Client-side string/byte[] parameters can be implicitly converted, and serialization can call the static method BinarySerialize/JsonSerialize)
    /// 服务端字节数组 / 客户端序列化对象（客户端 string / byte[] 传参可隐式转换，序列化可调用静态方法 BinarySerialize / JsonSerialize）
    /// </summary>
    [RemoteType]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    [AutoCSer.BinarySerialize(IsReferenceMember = false, CustomReferenceTypes = new Type[0])]
    public struct ServerByteArray : AutoCSer.BinarySerialize.ICustomSerialize<ServerByteArray>
    {
        /// <summary>
        /// Server-side byte array
        /// 服务端字节数组
        /// </summary>
#if NetStandard21
        internal byte[]? Buffer;
#else
        internal byte[] Buffer;
#endif
        /// <summary>
        /// Client object serialization
        /// 客户端对象序列化
        /// </summary>
#if NetStandard21
        private readonly RequestParameterSerializer? serializer;
#else
        private readonly RequestParameterSerializer serializer;
#endif
        /// <summary>
        /// Server-side byte array
        /// 服务端字节数组
        /// </summary>
        /// <param name="buffer"></param>
#if NetStandard21
        private ServerByteArray(byte[]? buffer)
#else
        private ServerByteArray(byte[] buffer)
#endif
        {
            Buffer = buffer;
            serializer = null;
        }
        /// <summary>
        /// Client-side serialized object
        /// 客户端序列化对象
        /// </summary>
        /// <param name="serializer"></param>
        private ServerByteArray(RequestParameterSerializer serializer)
        {
            this.serializer = serializer;
            Buffer = null;
        }
        /// <summary>
        /// Server-side implicit conversion
        /// 服务端隐式转换
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator byte[]?(ServerByteArray value) { return value.Buffer; }
#else
        public static implicit operator byte[](ServerByteArray value) { return value.Buffer; }
#endif
        /// <summary>
        /// Client-side implicit conversion
        /// 客户端隐式转换
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator ServerByteArray(byte[]? value) { return new ServerByteArray(value); }
#else
        public static implicit operator ServerByteArray(byte[] value) { return new ServerByteArray(value); }
#endif
        /// <summary>
        /// Client-side implicit conversion
        /// 客户端隐式转换
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public static implicit operator ServerByteArray(string? value)
#else
        public static implicit operator ServerByteArray(string value) 
#endif
        {
            return new ServerByteArray((StringRequestParameterSerializer)value);
        }
        /// <summary>
        /// Get string data
        /// 获取字符串数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Whether the string parsing was successful
        /// 字符串解析是否成功</returns>
#if NetStandard21
        public unsafe bool GetString(ref string? value)
#else
        public unsafe bool GetString(ref string value)
#endif
        {
            if (Buffer?.Length > 0)
            {
                fixed (byte* bufferFixed = Buffer) return AutoCSer.BinaryDeserializer.DeserializeString(bufferFixed, Buffer.Length + (-Buffer.Length & 3), out value);
            }
            return false;
        }

        /// <summary>
        /// Serialization
        /// 序列化
        /// </summary>
        /// <param name="serializer"></param>
        void AutoCSer.BinarySerialize.ICustomSerialize<ServerByteArray>.Serialize(AutoCSer.BinarySerializer serializer)
        {
            if (this.serializer != null) this.serializer.Serialize(serializer);
            else serializer.SerializeBuffer(Buffer);
        }
        /// <summary>
        /// Deserialization
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        unsafe void AutoCSer.BinarySerialize.ICustomSerialize<ServerByteArray>.Deserialize(AutoCSer.BinaryDeserializer deserializer)
        {
            deserializer.DeserializeBuffer(ref Buffer);
        }


        /// <summary>
        /// Get a binary serialization object
        /// 获取二进制序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ServerByteArray BinarySerialize<T>(T? value)
#else
        public static ServerByteArray BinarySerialize<T>(T value)
#endif
        {
            if (AutoCSer.SimpleSerializeType<T>.IsSimple)
            {
                return new ServerByteArray(new RequestParameterSimpleSerializer<ServerReturnValue<T>>(new ServerReturnValue<T>(value)));
            }
            return new ServerByteArray(new RequestParameterBinarySerializer<ServerReturnValue<T>>(new ServerReturnValue<T>(value)));
        }
        /// <summary>
        /// Get the binary deserialized object
        /// 获取二进制反序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Whether deserialization was successful
        /// 反序列化是否成功</returns>
#if NetStandard21
        public unsafe bool BinaryDeserialize<T>(ref T? value)
#else
        public unsafe bool BinaryDeserialize<T>(ref T value)
#endif
        {
            if (Buffer?.Length > 0)
            {
                ServerReturnValue<T> returnValue = new ServerReturnValue<T>(value);
                if (AutoCSer.SimpleSerializeType<T>.IsSimple)
                {
                    if (AutoCSer.BinaryDeserializer.SimpleDeserializeBuffer<ServerReturnValue<T>>(Buffer, ref returnValue))
                    {
                        value = returnValue.ReturnValue;
                        return true;
                    }
                }
                else if (AutoCSer.BinaryDeserializer.IndependentDeserializeBuffer<ServerReturnValue<T>>(Buffer, ref returnValue))
                {
                    value = returnValue.ReturnValue;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Get the JSON mixed binary serialization object
        /// 获取 JSON 混杂二进制序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static ServerByteArray JsonSerialize<T>(T? value)
#else
        public static ServerByteArray JsonSerialize<T>(T value)
#endif
        {
            return new ServerByteArray(new RequestParameterJsonSerializer<T>(value));
        }
        /// <summary>
        /// Get the JSON mixed binary deserialized object
        /// 获取 JSON 混杂二进制反序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Whether deserialization was successful
        /// 反序列化是否成功</returns>
#if NetStandard21
        public unsafe bool JsonDeserialize<T>(ref T? value)
#else
        public unsafe bool JsonDeserialize<T>(ref T value)
#endif
        {
            if (Buffer != null)
            {
                if (Buffer.Length != 0)
                {
                    fixed (byte* bufferFixed = Buffer) return AutoCSer.BinaryDeserializer.DeserializeJsonString(bufferFixed, Buffer.Length + (-Buffer.Length & 3), out value);
                }
                value = default(T);
                return true;
            }
            return false;
        }
    }
}
