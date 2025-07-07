using AutoCSer.Extensions;
using AutoCSer.Json;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using AutoCSer.SimpleSerialize;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// JSON 序列化
    /// </summary>
    public unsafe sealed partial class JsonSerializer : TextSerializer<JsonSerializer, JsonSerializeConfig>
    {
        /// <summary>
        /// 字符串 "null"
        /// </summary>
        public const string NullString = "null";
        /// <summary>
        /// 最大整数值
        /// </summary>
        internal const long MaxInteger = (1L << 52) - 1;
        /// <summary>
        /// JSON 自定义全局配置
        /// </summary>
        public static readonly CustomConfig CustomConfig = AutoCSer.Configuration.Common.Get<CustomConfig>()?.Value ?? new CustomConfig();
        /// <summary>
        /// 默认序列化类型配置
        /// </summary>
#if NetStandard21
        internal static readonly JsonSerializeAttribute? ConfigurationAttribute = ((ConfigObject<JsonSerializeAttribute>?)AutoCSer.Configuration.Common.Get(typeof(JsonSerializeAttribute)))?.Value;
#else
        internal static readonly JsonSerializeAttribute ConfigurationAttribute = ((ConfigObject<JsonSerializeAttribute>)AutoCSer.Configuration.Common.Get(typeof(JsonSerializeAttribute)))?.Value;
#endif
        /// <summary>
        /// 默认序列化类型配置
        /// </summary>
        internal static readonly JsonSerializeAttribute AllMemberAttribute = ConfigurationAttribute ?? new JsonSerializeAttribute { IsBaseType = false };
        /// <summary>
        /// 公共默认配置参数
        /// </summary>
        internal static readonly JsonSerializeConfig DefaultConfig = AutoCSer.Configuration.Common.Get<JsonSerializeConfig>()?.Value ?? new JsonSerializeConfig();

        /// <summary>
        /// 获取 JSON 字符串输出缓冲区
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static CharStream GetCharStream(JsonSerializer jsonSerializer) { return jsonSerializer.CharStream; }
        /// <summary>
        /// JSON 序列化
        /// </summary>
        /// <param name="isThreadStatic">是否单线程模式</param>
        internal JsonSerializer(bool isThreadStatic = false) : base(DefaultConfig, isThreadStatic) { }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Json字符串</returns>
#if NetStandard21
        private string serialize<T>(ref T value, JsonSerializeConfig? config)
#else
        private string serialize<T>(ref T value, JsonSerializeConfig config)
#endif
        {
            isProcessing = true;
            Config = config ?? DefaultConfig;
            CharStream.TrySetDataCanResize(AutoCSer.Common.Config.SerializeUnmanagedPool);
            using (CharStream)
            {
                serialize(ref value);
                return CharStream.ToString();
            }
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="jsonStream">Json输出缓冲区</param>
        /// <param name="config">Configuration parameters</param>
#if NetStandard21
        private void serialize<T>(ref T value, CharStream jsonStream, JsonSerializeConfig? config)
#else
        private void serialize<T>(ref T value, CharStream jsonStream, JsonSerializeConfig config)
#endif
        {
            isProcessing = true;
            Config = config ?? DefaultConfig;
            UnmanagedStreamExchangeBuffer buffer;
            CharStream.ExchangeToBuffer(jsonStream, out buffer);
            try
            {
                serialize(ref value);
                if (CharStream.IsResizeError) Warning |= AutoCSer.TextSerialize.WarningEnum.ResizeError;
            }
            finally { CharStream.ExchangeFromBuffer(jsonStream, ref buffer); }
        }
        /// <summary>
        /// 对象转换JSON字符串（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Json字符串</returns>
#if NetStandard21
        private string serializeThreadStatic<T>(ref T value, JsonSerializeConfig? config)
#else
        private string serializeThreadStatic<T>(ref T value, JsonSerializeConfig config)
#endif
        {
            isProcessing = true;
            Config = config ?? DefaultConfig;
            CharStream.ClearCanResize();
            serialize(ref value);
            return CharStream.ToString();
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void serialize<T>(ref T value)
        {
            Warning = AutoCSer.TextSerialize.WarningEnum.None;
            forefather.Reserve = 0;
            CheckDepth = Config.CheckDepth;
            TypeSerializer<T>.Serialize(this, ref value);
        }
        /// <summary>
        /// JSON 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="config"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void SerializeNext<T>(ref T value, JsonSerializeConfig? config)
#else
        internal void SerializeNext<T>(ref T value, JsonSerializeConfig config)
#endif
        {
            isProcessing = true;
            Config = config ?? DefaultConfig;
            serialize(ref value);
            if (CharStream.IsResizeError) Warning |= AutoCSer.TextSerialize.WarningEnum.ResizeError;
        }
        /// <summary>
        /// Release resources（线程静态实例模式）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void freeThreadStatic()
        {
            Config = DefaultConfig;
            free();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free()
        {
            freeThreadStatic();
            AutoCSer.Threading.LinkPool<JsonSerializer>.Default.Push(this);
        }
        /// <summary>
        /// Release resources
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void FreeBinaryMix()
        {
            IsBinaryMix = false;
            CharStream.Reset();
            Free();
        }
        /// <summary>
        /// 设置二进制混杂模式
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetBinaryMix()
        {
            IsBinaryMix = true;
            Config = DefaultConfig;
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="stream">二进制缓冲区</param>
        /// <param name="memberMap"></param>
#if NetStandard21
        internal void Serialize<T>(ref T value, UnmanagedStream stream, MemberMap<T>? memberMap)
#else
        internal void Serialize<T>(ref T value, UnmanagedStream stream, MemberMap<T> memberMap)
#endif
        {
            Config.MemberMap = memberMap;
            CharStream.CopyFromStart(stream);
            try
            {
                serialize(ref value);
                if (CharStream.IsResizeError) Warning |= AutoCSer.TextSerialize.WarningEnum.ResizeError;
            }
            finally { stream.CopyFromEnd(CharStream); }
        }
        /// <summary>
        /// 命令服务序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="stream"></param>
        private void serializeNotNull<T>(ref T value, UnmanagedStream stream)
        {
            Warning = AutoCSer.TextSerialize.WarningEnum.None;
            forefather.ClearLength();
            forefather.Reserve = 0;
            CheckDepth = Config.CheckDepth;

            CharStream.CopyFromStart(stream);
            try
            {
                TypeSerializer<T>.SerializeCommandServer(this, ref value);
            }
            finally { stream.CopyFromEnd(CharStream); }
        }
        /// <summary>
        /// 命令服务序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="stream"></param>
        /// <returns>返回不包括补白的字节数</returns>
        internal int SerializeBufferNotNull<T>(ref T value, UnmanagedStream stream)
        {
            int index = stream.GetMoveSize(sizeof(int));
            if (index != 0)
            {
                serializeNotNull(ref value, stream);
                if (!stream.IsResizeError)
                {
                    int size = stream.Data.Pointer.CurrentIndex - index;
                    if (size != 0)
                    {
                        int prepLength = (size + (3 + sizeof(int))) & (int.MaxValue - 3);
                        if (CharStream.Data.Pointer.Size <= prepLength)
                        {
                            CharStream.Clear();
                            return binarySerializeString(stream, index, size);
                        }
                        byte* bufferData = null;
                        ByteArrayBuffer buffer = ByteArrayPool.GetBuffer(prepLength
#if DEBUG
            , false
#endif
                );
                        try
                        {
                            fixed (byte* bufferFixed = buffer.GetFixedBuffer())
                            {
                                bufferData = bufferFixed + buffer.StartIndex;
                                CharStream.Reset(bufferData, prepLength);
                                return binarySerializeString(stream, index, size);
                            }
                        }
                        finally
                        {
                            buffer.Free();
                            CharStream.Data.Pointer.SetNull(bufferData);
                        }
                    }
                    *(int*)(stream.Data.Pointer.Current - sizeof(int)) = 0;
                    return sizeof(int);
                }
            }
            return 0;
        }
        /// <summary>
        /// 字符串二进制序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns>返回不包括补白的字节数</returns>
        private int binarySerializeString(UnmanagedStream stream, int index, int size)
        {
            byte* json = stream.Data.Pointer.Byte + index;
            int dataSize = CharStream.Serialize((char*)json, size >> 1, false);
            if (dataSize != 0)
            {
                stream.Data.Pointer.CurrentIndex = index - sizeof(int);
                stream.Write(ref CharStream.Data.Pointer);
                return dataSize;
            }
            *(int*)(json - sizeof(int)) = size;
            if ((size & 2) != 0) stream.Write(' ');
            return size + sizeof(int);
        }
        ///// <summary>
        ///// 获取序列化循环引用检查类型
        ///// </summary>
        ///// <param name="pushType"></param>
        ///// <returns></returns>
        //internal SerializePushType CheckObject(SerializePushType pushType)
        //{
        //    if (--CheckDepth > 0)
        //    {
        //        if (Config.CheckLoop)
        //        {
        //            switch (pushType)
        //            {
        //                case SerializePushType.DepthCount: return SerializePushType.DepthCount;
        //                case SerializePushType.UnknownNode:
        //                    if (forefather.Reserve != 0) return SerializePushType.UnknownNode;
        //                    return SerializePushType.DepthCount;
        //                case SerializePushType.UnknownDepthCount: ++forefather.Reserve; return SerializePushType.UnknownDepthCount;
        //                case SerializePushType.Push: return SerializePushType.Push;
        //            }
        //        }
        //        return SerializePushType.DepthCount;
        //    }
        //    ++CheckDepth;
        //    this.Warning |= SerializeWarning.DepthOutOfRange;
        //    return SerializePushType.DepthOutOfRange;
        //}
        /// <summary>
        /// 循环引用对象处理
        /// </summary>
        protected override void WriteLoopReference() { CharStream.WriteJsonObject(); }
        /// <summary>
        /// 输出 null 值
        /// </summary>
        /// <param name="jsonSerializer"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void WriteJsonNull(JsonSerializer jsonSerializer)
        {
            jsonSerializer.CharStream.WriteJsonNull();
        }
        /// <summary>
        /// 转换JSON字符串
        /// </summary>
        /// <param name="value">Data object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerializeType<T>(T value)
        {
            TypeSerializer<T>.Serialize(this, ref value);
        }
        /// <summary>
        /// 转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">Data object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Serialize<T>(JsonSerializer jsonSerializer, T value)
        {
            TypeSerializer<T>.Serialize(jsonSerializer, ref value);
        }
        /// <summary>
        /// Custom serialization不支持类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        public static void NotSupport<T>(JsonSerializer jsonSerializer, T value)
        {
            int size = CustomConfig.NotSupport(jsonSerializer, value);
            if (size > 0) jsonSerializer.CharStream.Data.Pointer.CheckMoveSize(size << 1);
        }
        /// <summary>
        /// 基类序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="BT"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Base<T, BT>(JsonSerializer jsonSerializer, T value)
            where T : class, BT
        {
            TypeSerializer<BT>.Serialize(jsonSerializer, value);
        }
        /// <summary>
        /// Custom serialization
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void ICustom<T>(AutoCSer.JsonSerializer serializer, T value) where T : ICustomSerialize<T>
        {
            if (value != null) value.Serialize(serializer);
            else serializer.CharStream.WriteJsonNull();
        }
        /// <summary>
        /// object 对象转换JSON字符串
        /// </summary>
        /// <param name="value">Data object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void Object<T>(T value)
        {
            ++forefather.Reserve;
            TypeSerializer<T>.Serialize(this, value);
            --forefather.Reserve;
        }
        /// <summary>
        /// object 对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">Data object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Object<T>(JsonSerializer jsonSerializer, object value)
        {
            jsonSerializer.Object((T)value);
        }
        /// <summary>
        /// 可空类型序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void JsonSerialize<T>(T? value)
            where T : struct
        {
            if (value.HasValue) TypeSerializer<T>.Serialize(this, value.Value);
            else CharStream.WriteJsonNull();
        }
        /// <summary>
        /// 可空类型序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Nullable<T>(JsonSerializer jsonSerializer, T? value) where T : struct
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 二进制混杂序列化数组长度
        /// </summary>
        /// <param name="length"></param>
        private void binaryMixArrayLength(int length)
        {
            if (length <= byte.MaxValue) CharStream.Write((ushort)(((ushort)length << 8) | (byte)BinaryMixTypeEnum.ArrayByte));
            else if (length < 1 << 24) CharStream.Write(((uint)length << 8) | (byte)BinaryMixTypeEnum.ArrayByte3);
            else
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(int));
                if (write != null)
                {
                    *(ushort*)write = (byte)BinaryMixTypeEnum.Array;
                    *(int*)(write + sizeof(ushort)) = length;
                }
            }
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array object</param>
        public void JsonSerialize<T>(T[] array)
        {
            if (array.Length != 0)
            {
                if (IsBinaryMix)
                {
                    binaryMixArrayLength(array.Length);
                    foreach(T value in array)
                    {
                        if (value != null) TypeSerializer<T>.Serialize(this, value);
                        else CharStream.WriteJsonNull();
                    }
                }
                else
                {
                    CharStream.WriteJsonArrayStart(array.Length << 2);
                    bool isNext = false;
                    foreach (T value in array)
                    {
                        if (isNext) CharStream.Write(',');
                        else isNext = true;
                        if (value != null) TypeSerializer<T>.Serialize(this, value);
                        else CharStream.WriteJsonNull();
                    }
                    CharStream.Write(']');
                }
            }
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Array<T>(JsonSerializer jsonSerializer, T[] array)
        {
            jsonSerializer.JsonSerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array object</param>
        public void JsonSerialize<T>(LeftArray<T> array)
        {
            int length = array.Length;
            if (length != 0)
            {
                if (IsBinaryMix)
                {
                    binaryMixArrayLength(length);
                    foreach (T value in array.Array)
                    {
                        if (value != null) TypeSerializer<T>.Serialize(this, value);
                        else CharStream.WriteJsonNull();
                        if (--length == 0) break;
                    }
                }
                else
                {
                    CharStream.WriteJsonArrayStart(length << 2);
                    foreach (T value in array.Array)
                    {
                        if (length != array.Length) CharStream.Write(',');
                        if (value != null) TypeSerializer<T>.Serialize(this, value);
                        else CharStream.WriteJsonNull();
                        if (--length == 0) break;
                    }
                    CharStream.Write(']');
                }
            }
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array object</param>
        public void JsonSerialize<T>(T?[] array)
            where T : struct
        {
            if (array.Length != 0)
            {
                if (IsBinaryMix)
                {
                    binaryMixArrayLength(array.Length);
                    foreach (T? value in array)
                    {
                        if (value.HasValue) TypeSerializer<T>.Serialize(this, value.Value);
                        else CharStream.WriteJsonNull();
                    }
                }
                else
                {
                    CharStream.WriteJsonArrayStart(array.Length << 2);
                    bool isNext = false;
                    foreach (T? value in array)
                    {
                        if (isNext) CharStream.Write(',');
                        else isNext = true;
                        if (value.HasValue) TypeSerializer<T>.Serialize(this, value.Value);
                        else CharStream.WriteJsonNull();
                    }
                    CharStream.Write(']');
                }
            }
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void NullableArray<T>(JsonSerializer jsonSerializer, T?[] array)
            where T : struct
        {
            jsonSerializer.JsonSerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="array"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void LeftArray<T>(JsonSerializer jsonSerializer, LeftArray<T> array)
        {
            jsonSerializer.JsonSerialize(array);
        }
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void ListArray<T>(JsonSerializer jsonSerializer, ListArray<T>? array)
#else
        internal static void ListArray<T>(JsonSerializer jsonSerializer, ListArray<T> array)
#endif
        {
            if (array != null) jsonSerializer.JsonSerialize(array.Array);
            else jsonSerializer.CharStream.WriteJsonNull();
        }
        /// <summary>
        /// 集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
#if NetStandard21
        public void JsonSerialize<T>(ICollection<T?> array)
#else
        public void JsonSerialize<T>(ICollection<T> array)
#endif
        {
            if (array.Count != 0)
            {
                CharStream.WriteJsonArrayStart(array.Count << 2);
                TypeSerializer<T>.Collection(this, array);
                CharStream.Write(']');
            }
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 集合序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void Collection<T, VT>(JsonSerializer jsonSerializer, T value) where T : ICollection<VT?>
#else
        public static void Collection<T, VT>(JsonSerializer jsonSerializer, T value) where T : ICollection<VT>
#endif
        {
            if (value != null) jsonSerializer.JsonSerialize(value);
            else jsonSerializer.CharStream.WriteJsonNull();
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        public void JsonSerialize<T>(IDictionary<string, T> dictionary)
        {
            if (dictionary.Count != 0)
            {
                if (Config.IsStringDictionaryToObject || Config.IsDictionaryToObject)
                {
                    CharStream.Write('{');
                    TypeSerializer<T>.StringDictionary(this, dictionary);
                    CharStream.Write('}');
                }
                else
                {
                    CharStream.Write('[');
                    TypeSerializer<T>.StringDictionaryToArray(this, dictionary);
                    CharStream.Write(']');
                }
            }
            else if (Config.IsStringDictionaryToObject || Config.IsDictionaryToObject) CharStream.WriteJsonObject();
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="dictionary"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StringDictionary<T>(JsonSerializer jsonSerializer, Dictionary<string, T> dictionary)
        {
            jsonSerializer.JsonSerialize(dictionary);
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="dictionary"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StringIDictionary<T, VT>(JsonSerializer jsonSerializer, T dictionary)
        {
            if (dictionary != null) jsonSerializer.JsonSerialize((IDictionary<string, VT>)(object)dictionary);
            else jsonSerializer.CharStream.WriteJsonNull();
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="dictionary"></param>
#if NetStandard21
        public void JsonSerialize<KT, VT>(IDictionary<KT, VT?> dictionary)
#else
        public void JsonSerialize<KT, VT>(IDictionary<KT, VT> dictionary)
#endif
        {
            if (dictionary.Count != 0)
            {
                if (Config.IsDictionaryToObject)
                {
                    CharStream.Write('{');
                    TypeSerializer<VT>.Dictionary(this, dictionary);
                    CharStream.Write('}');
                }
                else
                {
                    CharStream.Write('[');
                    TypeSerializer<VT>.DictionaryToArray(this, dictionary);
                    CharStream.Write(']');
                }
            }
            else if (Config.IsDictionaryToObject) CharStream.WriteJsonObject();
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="dictionary"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void Dictionary<KT, VT>(JsonSerializer jsonSerializer, Dictionary<KT, VT?> dictionary) where KT : notnull
#else
        internal static void Dictionary<KT, VT>(JsonSerializer jsonSerializer, Dictionary<KT, VT> dictionary)
#endif
        {
            jsonSerializer.JsonSerialize(dictionary);
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="dictionary"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void IDictionary<T, KT, VT>(JsonSerializer jsonSerializer, T dictionary)
            where T : IDictionary<KT, VT>
        {
            jsonSerializer.JsonSerialize(dictionary);
        }
        /// <summary>
        /// 键值对序列化
        /// </summary>
        /// <param name="keyValue"></param>
        public void JsonSerialize<KT, VT>(KeyValuePair<KT, VT> keyValue)
        {
            CharStream.WriteJsonKeyValueKey(21);
            TypeSerializer<KT>.SerializeNull(this, keyValue.Key);
            CharStream.WriteJsonKeyValueValue();
            TypeSerializer<VT>.SerializeNull(this, keyValue.Value);
            CharStream.Write('}');
        }
        /// <summary>
        /// 键值对序列化
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="keyValue"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void KeyValuePair<KT, VT>(JsonSerializer jsonSerializer, KeyValuePair<KT, VT> keyValue)
        {
            jsonSerializer.JsonSerialize(keyValue);
        }

        /// <summary>
        /// 逻辑值转换
        /// </summary>
        /// <param name="value">Logical value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(bool value)
        {
            if (IsBinaryMix) CharStream.Write(value ? (char)(byte)BinaryMixTypeEnum.True : (char)(byte)BinaryMixTypeEnum.False);
            else if (Config.IsBoolToInt) CharStream.Write(value ? '1' : '0');
            else CharStream.WriteJsonBool(value);
        }
        /// <summary>
        /// 二进制混杂序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void binaryMix(byte value)
        {
            CharStream.Write((ushort)(((ushort)value << 8) | (byte)BinaryMixTypeEnum.Byte));
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(byte value)
        {
            if (IsBinaryMix) binaryMix(value);
            else primitiveSerialize(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        private void primitiveSerialize(byte value)
        {
            if (!Config.IsIntegerToHex) CharStream.WriteString(value);
            else CharStream.WriteJsonHex(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(sbyte value)
        {
            if (IsBinaryMix) binaryMix((byte)value);
            else primitiveSerialize(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        private void primitiveSerialize(sbyte value)
        {
            if (!Config.IsIntegerToHex) CharStream.WriteString(value);
            else CharStream.WriteJsonHex(value);
        }
        /// <summary>
        /// 二进制混杂序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void binaryMix(ushort value)
        {
            if ((value & 0xff00) == 0) binaryMix((byte)value);
            else CharStream.Write((uint)value << 16 | (byte)BinaryMixTypeEnum.UShort);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(short value)
        {
            if (IsBinaryMix) binaryMix((ushort)value);
            else primitiveSerialize(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        private void primitiveSerialize(short value)
        {
            if (value >= 0) JsonSerialize((ushort)value);
            else
            {
                CharStream.WriteNegative(7);
                JsonSerialize((ushort)-value);
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(ushort value)
        {
            if (IsBinaryMix) binaryMix(value);
            else primitiveSerialize(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        private void primitiveSerialize(ushort value)
        {
            if (!Config.IsIntegerToHex) CharStream.WriteString(value);
            else CharStream.WriteJsonHex(value);
        }
        /// <summary>
        /// 二进制混杂序列化
        /// </summary>
        /// <param name="value"></param>
        private void binaryMix(uint value)
        {
            if ((value & 0xffff0000U) == 0) binaryMix((ushort)value);
            else
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(uint));
                if (write != null)
                {
                    *(ushort*)write = (byte)BinaryMixTypeEnum.UInt;
                    *(uint*)(write + sizeof(ushort)) = value;
                }
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(int value)
        {
            if (IsBinaryMix) binaryMix((uint)value);
            else primitiveSerialize(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        private void primitiveSerialize(int value)
        {
            if (value >= 0) JsonSerialize((uint)value);
            else
            {
                CharStream.WriteNegative(11);
                JsonSerialize((uint)-value);
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(uint value)
        {
            if (IsBinaryMix) binaryMix(value);
            else primitiveSerialize(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        private void primitiveSerialize(uint value)
        {
            if (!Config.IsIntegerToHex)
            {
                if (value <= ushort.MaxValue) JsonSerialize((ushort)value);
                else CharStream.WriteString(value);
            }
            else CharStream.WriteJsonHex(value);
        }
        /// <summary>
        /// 二进制混杂序列化
        /// </summary>
        /// <param name="value"></param>
        private void binaryMix(ulong value)
        {
            if ((value & 0xffffffff00000000UL) == 0) binaryMix((uint)value);
            else
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(ulong));
                if (write != null)
                {
                    *(ushort*)write = (byte)BinaryMixTypeEnum.ULong;
                    *(ulong*)(write + sizeof(ushort)) = value;
                }
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(long value)
        {
            if (IsBinaryMix) binaryMix((ulong)value);
            else primitiveSerialize(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        private void primitiveSerialize(long value)
        {
            if (value >= 0) JsonSerialize((ulong)value);
            else if ((ulong)(value + MaxInteger) <= (ulong)(MaxInteger << 1) || !Config.IsMaxNumberToString)
            {
                CharStream.WriteNegative(19);
                JsonSerialize((ulong)-value);
            }
            else CharStream.WriteJsonString(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(ulong value)
        {
            if (IsBinaryMix) binaryMix(value);
            else primitiveSerialize(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        private void primitiveSerialize(ulong value)
        {
            if (value <= MaxInteger || !Config.IsMaxNumberToString)
            {
                if (!Config.IsIntegerToHex)
                {
                    if (value <= uint.MaxValue) JsonSerialize((uint)value);
                    else CharStream.WriteString(value);
                }
                else CharStream.WriteJsonHex(value);
            }
            else CharStream.WriteJsonString(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        public void JsonSerialize(Int128 value)
        {
            if (IsBinaryMix)
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(Int128));
                if (write != null)
                {
                    *(ushort*)write = (byte)BinaryMixTypeEnum.UInt128;
                    *(Int128*)(write + sizeof(ushort)) = value;
                }
            }
            else
            {
                int size = JsonSerializer.CustomConfig.Write(this, value);
                if (size > 0) CharStream.Data.Pointer.CheckMoveSize(size << 1);
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        public void JsonSerialize(UInt128 value)
        {
            if (IsBinaryMix)
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(UInt128));
                if (write != null)
                {
                    *(ushort*)write = (byte)BinaryMixTypeEnum.UInt128;
                    *(UInt128*)(write + sizeof(ushort)) = value;
                }
            }
            else
            {
                int size = JsonSerializer.CustomConfig.Write(this, value);
                if (size > 0) CharStream.Data.Pointer.CheckMoveSize(size << 1);
            }
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        public void JsonSerialize(Half value)
        {
            if (IsBinaryMix) CharStream.Write((uint)*(ushort*)&value << 16 | (byte)BinaryMixTypeEnum.Half);
#if NET8
            else if (Config.IsInfinityToNaN)
            {
                if (!Half.IsNaN(value) && !Half.IsInfinity(value))
                {
                    int size = JsonSerializer.CustomConfig.Write(this, value);
                    if (size > 0) CharStream.Data.Pointer.CheckMoveSize(size << 1);
                }
                else CharStream.WriteJsonNaN();
            }
            else
            {
                if (!Half.IsNaN(value))
                {
                    if (!Half.IsInfinity(value))
                    {
                        int size = JsonSerializer.CustomConfig.Write(this, value);
                        if (size > 0) CharStream.Data.Pointer.CheckMoveSize(size << 1);
                    }
                    else if (Half.IsPositiveInfinity(value)) CharStream.WritePositiveInfinity();
                    else CharStream.WriteNegativeInfinity();
                }
                else CharStream.WriteJsonNaN();
            }
#else
            else
            {
                int size = JsonSerializer.CustomConfig.Write(this, value);
                if (size > 0) CharStream.Data.Pointer.CheckMoveSize(size << 1);
            }
#endif
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        public void JsonSerialize(float value)
        {
            if (IsBinaryMix)
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(float));
                if (write != null)
                {
                    *(ushort*)write = (byte)BinaryMixTypeEnum.Float;
                    *(float*)(write + sizeof(ushort)) = value;
                }
            }
            else if (Config.IsInfinityToNaN) CharStream.WriteJson(value);
            else CharStream.WriteJsonInfinity(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        public void JsonSerialize(double value)
        {
            if (IsBinaryMix)
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(double));
                if (write != null)
                {
                    *(ushort*)write = (byte)BinaryMixTypeEnum.Double;
                    *(double*)(write + sizeof(ushort)) = value;
                }
            }
            else if (Config.IsInfinityToNaN) CharStream.WriteJson(value);
            else CharStream.WriteJsonInfinity(value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="value">数字</param>
        public void JsonSerialize(decimal value)
        {
            if (IsBinaryMix)
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(decimal));
                if (write != null)
                {
                    *(ushort*)write = (byte)BinaryMixTypeEnum.Decimal;
                    *(decimal*)(write + sizeof(ushort)) = value;
                }
            }
            else
            {
                int size = CustomConfig.Write(CharStream, value);
                if (size > 0) CharStream.Data.Pointer.CheckMoveSize(size << 1);
            }
        }
        /// <summary>
        /// 字符转换
        /// </summary>
        /// <param name="value">字符</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize(char value)
        {
            if (IsBinaryMix) binaryMix((ushort)value);
            else CharStream.WriteJson(value);
        }
        /// <summary>
        /// 二进制混杂序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void binaryMix(DateTime value)
        {
            byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(DateTime));
            if (write != null)
            {
                *(ushort*)write = (byte)BinaryMixTypeEnum.DateTime;
                *(DateTime*)(write + sizeof(ushort)) = value;
            }
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="value">时间</param>
        public void JsonSerialize(DateTime value)
        {
            if (IsBinaryMix) binaryMix(value);
            else
            {
                int size = CustomConfig.Write(this, value);
                if (size > 0) CharStream.Data.Pointer.CheckMoveSize(size << 1);
            }
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="value">时间</param>
        internal void SerializeDateTime(DateTime value)
        {
            if (value == DateTime.MinValue && Config.IsDateTimeMinNull)
            {
                CharStream.WriteJsonNull();
                return;
            }
            switch (Config.DateTimeType)
            {
                case DateTimeTypeEnum.Default: CharStream.WriteJsonString(value); return;
                //case DateTimeTypeEnum.Sql: CharStream.WriteJsonSqlString(value, '"'); return;
                case DateTimeTypeEnum.JavaScript:
                    CharStream.WriteWebViewJson(value);
                    //CharStream.WriteJsonNewDate();
                    //PrimitiveSerialize(((value.Kind == DateTimeKind.Utc ? value.Ticks + Date.LocalTimeTicks : value.Ticks) - AutoCSer.JsonDeserializer.JavaScriptLocalMinTimeTicks) / TimeSpan.TicksPerMillisecond);
                    //CharStream.Data.Pointer.Write(')');
                    return;
                case DateTimeTypeEnum.ThirdParty:
                    CharStream.WriteJsonOtherDate();
                    JsonSerialize(((value.Kind == DateTimeKind.Utc ? value.Ticks + Date.LocalTimeTicks : value.Ticks) - AutoCSer.JsonDeserializer.JavaScriptLocalMinTimeTicks) / TimeSpan.TicksPerMillisecond);
                    CharStream.WriteJsonOtherDateEnd();
                    return;
                case DateTimeTypeEnum.CustomFormat:
                    if (Config.DateTimeCustomFormat == null) JsonSerialize(value.ToString());
                    else JsonSerialize(value.ToString(Config.DateTimeCustomFormat));
                    return;
            }
        }
        /// <summary>
        /// 二进制混杂序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void binaryMix(TimeSpan value)
        {
            byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(TimeSpan));
            if (write != null)
            {
                *(ushort*)write = (byte)BinaryMixTypeEnum.TimeSpan;
                *(TimeSpan*)(write + sizeof(ushort)) = value;
            }
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="value">时间</param>
        public void JsonSerialize(TimeSpan value)
        {
            if (IsBinaryMix) binaryMix(value);
            else
            {
                int size = CustomConfig.Write(this, value);
                if (size > 0) CharStream.Data.Pointer.CheckMoveSize(size << 1);
            }
        }
        /// <summary>
        /// 时间转换
        /// </summary>
        /// <param name="value">时间</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SerializeTimeSpan(TimeSpan value)
        {
            CharStream.WriteJsonString(value);
        }
        /// <summary>
        /// Guid转换
        /// </summary>
        /// <param name="value">Guid</param>
        public void JsonSerialize(System.Guid value)
        {
            if (IsBinaryMix)
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(System.Guid));
                if (write != null)
                {
                    *(ushort*)write = (byte)BinaryMixTypeEnum.Guid;
                    *(System.Guid*)(write + sizeof(ushort)) = value;
                }
            }
            else CharStream.WriteJson(ref value);
        }
        /// <summary>
        /// 数字转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">数字</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, System.Guid value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void JsonSerializeNull(string? value)
#else
        public void JsonSerializeNull(string value)
#endif
        {
            if (value != null) JsonSerialize(value);
            else CharStream.WriteJsonNull();
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value"></param>
        public void JsonSerialize(string value)
        {
            if (value.Length == 0) CharStream.WriteJsonEmptyString();
            else primitiveSerializeNotEmpty(value);
        }
        /// <summary>
        /// 二进制混杂序列化字符串长度
        /// </summary>
        /// <param name="length"></param>
        private void binaryMixStringLength(int length)
        {
            if (length <= byte.MaxValue) CharStream.Write((ushort)(((ushort)length << 8) | (byte)BinaryMixTypeEnum.StringByte));
            else if (length < 1 << 24) CharStream.Write(((uint)length << 8) | (byte)BinaryMixTypeEnum.StringByte3);
            else
            {
                byte* write = CharStream.GetBeforeMove(sizeof(ushort) + sizeof(int));
                if (write != null)
                {
                    *(ushort*)write = (byte)BinaryMixTypeEnum.String;
                    *(int*)(write + sizeof(ushort)) = length;
                }
            }
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value">长度必须大于0</param>
        private void primitiveSerializeNotEmpty(string value)
        {
            if (IsBinaryMix)
            {
                binaryMixStringLength(value.Length);
                CharStream.WriteNotEmpty(value);
            }
            else
            {
                fixed (char* valueFixed = value) CharStream.WriteJson(valueFixed, value.Length);
            }
        }
        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <param name="value"></param>
        public void JsonSerialize(SubString value)
        {
            if (value.Length == 0) CharStream.WriteJsonEmptyString();
            else
            {
                if (IsBinaryMix)
                {
                    binaryMixStringLength(value.Length);
                    CharStream.Write(value.GetFixedBuffer(), value.Start, value.Length);
                }
                else
                {
                    fixed (char* valueFixed = value.GetFixedBuffer()) CharStream.WriteJson(valueFixed + value.Start, value.Length);
                }
            }
        }
        /// <summary>
        /// object 转换
        /// </summary>
        /// <param name="value"></param>
        public void JsonSerializeObject(object value)
        {
            if (value.GetType() == typeof(JsonNode)) JsonSerialize((JsonNode)value);
            else if (Config.IsObject)
            {
                Type type = value.GetType();
                if (type == typeof(object)) CharStream.WriteJsonObject();
                else
                {
#if AOT
                    ObjectMethod.MakeGenericMethod(type).Invoke(null, new object[] { this, value });
#else
                    AutoCSer.Metadata.GenericType.Get(type).JsonSerializeObjectDelegate(this, value);
#endif
                }
            }
            else CharStream.WriteJsonObject();
        }
        /// <summary>
        /// 基础类型转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, object value)
        {
            jsonSerializer.JsonSerializeObject(value);
        }
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="type"></param>
        public void JsonSerialize(Type type)
        {
            TypeSerializer<AutoCSer.Reflection.RemoteType>.MemberSerialize(this, new AutoCSer.Reflection.RemoteType(type));
        }
        /// <summary>
        /// JSON 节点转换
        /// </summary>
        /// <param name="value">JSON节点</param>
        public void JsonSerialize(JsonNode value)
        {
            bool isBinaryMix = IsBinaryMix;
            IsBinaryMix = false;
            try
            {
                serialize(ref value);
            }
            finally { IsBinaryMix = isBinaryMix; }
        }
        /// <summary>
        /// JSON 节点转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value">数字</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, JsonNode value)
        {
            jsonSerializer.JsonSerialize(value);
        }
        /// <summary>
        /// JSON 节点
        /// </summary>
        /// <param name="value"></param>
        private void serialize(ref JsonNode value)
        {
            switch (value.Type)
            {
                case JsonNodeTypeEnum.Dictionary:
                    CharStream.Write('{');
                    if ((int)value.Int64 != 0)
                    {
                        KeyValue<JsonNode, JsonNode>[] array = value.DictionaryArray;
                        for (int index = 0; index != (int)value.Int64; ++index)
                        {
                            if (index != 0) CharStream.Write(',');
                            serialize(ref array[index].Key);
                            CharStream.Write(':');
                            serialize(ref array[index].Value);
                        }
                    }
                    CharStream.Write('}');
                    return;
                case JsonNodeTypeEnum.Array:
                    CharStream.Write('[');
                    if ((int)value.Int64 != 0)
                    {
                        JsonNode[] array = value.ListArray;
                        for (int index = 0; index != (int)value.Int64; ++index)
                        {
                            if (index != 0) CharStream.Write(',');
                            serialize(ref array[index]);
                            CharStream.Write(':');
                            serialize(ref array[index]);
                        }
                    }
                    CharStream.Write(']');
                    return;
                case JsonNodeTypeEnum.String: JsonSerialize(value.SubString); return;
                case JsonNodeTypeEnum.QuoteString:
                case JsonNodeTypeEnum.ErrorQuoteString:
                    if (CharStream.PrepCharSize(value.SubString.Length + 2))
                    {
                        CharStream.Data.Pointer.Write((char)value.Int64);
                        CharStream.Write(ref value.SubString);
                        CharStream.Data.Pointer.Write((char)value.Int64);
                    }
                    return;
                case JsonNodeTypeEnum.NumberString:
                    if ((int)value.Int64 == 0) CharStream.Write(ref value.SubString);
                    else if (CharStream.PrepCharSize(value.SubString.Length + 2))
                    {
                        CharStream.Data.Pointer.Write((char)value.Int64);
                        CharStream.Write(ref value.SubString);
                        CharStream.Data.Pointer.Write((char)value.Int64);
                    }
                    return;
                case JsonNodeTypeEnum.Bool:
                    JsonSerialize((int)value.Int64 != 0);
                    return;
                case JsonNodeTypeEnum.DateTimeTick:
                    JsonSerialize(new DateTime(value.Int64, DateTimeKind.Local));
                    return;
                case JsonNodeTypeEnum.NaN:
                    CharStream.WriteJsonNaN();
                    return;
                case JsonNodeTypeEnum.PositiveInfinity:
                    if (Config.IsInfinityToNaN) CharStream.WriteJsonNaN();
                    else CharStream.WritePositiveInfinity();
                    return;
                case JsonNodeTypeEnum.NegativeInfinity:
                    if (Config.IsInfinityToNaN) CharStream.WriteJsonNaN();
                    else CharStream.WriteNegativeInfinity();
                    return;
                default:
                    CharStream.WriteJsonNull();
                    return;
            }
        }

        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">Array</param>
        public void JsonSerialize(bool[] array)
        {
            if (array.Length != 0)
            {
                if (IsBinaryMix)
                {
                    int length = array.Length;
                    binaryMixArrayLength(length);
                    if((length & 1) == 0)
                    {
                        byte* write = CharStream.GetBeforeMove(length);
                        if (write != null)
                        {
                            foreach (bool value in array) *write++ = value ? (byte)BinaryMixTypeEnum.ArrayTrue : (byte)BinaryMixTypeEnum.ArrayFalse;
                        }
                    }
                    else
                    {
                        byte* write = CharStream.GetBeforeMove(length + 1);
                        if (write != null)
                        {
                            foreach (bool value in array) *write++ = value ? (byte)BinaryMixTypeEnum.ArrayTrue : (byte)BinaryMixTypeEnum.ArrayFalse;
                            *write = (byte)BinaryMixTypeEnum.None;
                        }
                    }
                }
                else
                {
                    bool isNext = false;
                    if (Config.IsBoolToInt)
                    {
                        char* write = (char*)CharStream.GetBeforeMove(((array.Length << 1) + 1) * sizeof(char));
                        if (write != null)
                        {
                            *write++ = '[';
                            foreach (bool value in array)
                            {
                                if (isNext)
                                {
                                    *(int*)write = value ? ',' + ('1' << 16) : (',' + ('0' << 16));
                                    write += 2;
                                }
                                else
                                {
                                    isNext = true;
                                    *write++ = value ? '1' : '0';
                                }
                            }
                            *write = ']';
                        }
                    }
                    else
                    {
                        CharStream.WriteJsonArrayStart(array.Length * 5);
                        foreach (bool value in array)
                        {
                            if (isNext) CharStream.Write(',');
                            else isNext = true;
                            CharStream.WriteJsonBool(value);
                        }
                        CharStream.Write(']');
                    }
                }
            }
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="array">Array</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, bool[] array)
        {
            jsonSerializer.JsonSerialize(array);
        }
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">Array</param>
        public void JsonSerialize(DateTime[] array)
        {
            if (array.Length != 0)
            {
                if (IsBinaryMix)
                {
                    binaryMixArrayLength(array.Length);
                    foreach (DateTime value in array) binaryMix(value);
                }
                else
                {
                    bool isNext = false;
                    CharStream.WriteJsonArrayStart(array.Length * (Config.IsDateTimeMinNull ? 5 : 22));
                    foreach (DateTime value in array)
                    {
                        if (isNext) CharStream.Write(',');
                        else isNext = true;
                        JsonSerialize(value);
                    }
                    CharStream.Write(']');
                }
            }
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="array">Array</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, DateTime[] array)
        {
            jsonSerializer.JsonSerialize(array);
        }
        /// <summary>
        /// 数组转换 
        /// </summary>
        /// <param name="array">Array</param>
        public void JsonSerialize(TimeSpan[] array)
        {
            if (array.Length != 0)
            {
                if (IsBinaryMix)
                {
                    binaryMixArrayLength(array.Length);
                    foreach (TimeSpan value in array) binaryMix(value);
                }
                else
                {
                    bool isNext = false;
                    CharStream.WriteJsonArrayStart(array.Length * 19 + 1);
                    foreach (TimeSpan value in array)
                    {
                        if (isNext) CharStream.Write(',');
                        else isNext = true;
                        JsonSerialize(value);
                    }
                    CharStream.Write(']');
                }
            }
            else CharStream.WriteJsonArray();
        }
        /// <summary>
        /// 数组转换
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="array">Array</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(JsonSerializer jsonSerializer, TimeSpan[] array)
        {
            jsonSerializer.JsonSerialize(array);
        }

        /// <summary>
        /// Custom serialization调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void CustomSerialize<T>(T? value)
#else
        public void CustomSerialize<T>(T value)
#endif
        {
            if (value != null)
            {
                ++forefather.Reserve;
                TypeSerializer<T>.Serialize(this, ref value);
                --forefather.Reserve;
            }
            else CharStream.WriteJsonNull();
        }
#if AOT
        /// <summary>
        /// 数组序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void JsonSerialize<T>(ListArray<T> value)
        {
            JsonSerialize(value.Array);
        }
        /// <summary>
        /// 自定义 JSON 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void ICustom<T>(ICustomSerialize<T> value)
        {
            value.Serialize(this);
        }
        /// <summary>
        /// object 反射序列化
        /// </summary>
        internal static readonly MethodInfo ObjectMethod;
        /// <summary>
        /// 不支持类型序列化
        /// </summary>
        internal static readonly MethodInfo NotSupportMethod;
        /// <summary>
        /// 基类序列化
        /// </summary>
        internal static readonly MethodInfo BaseMethod;
        /// <summary>
        /// Custom serialization
        /// </summary>
        internal static readonly MethodInfo ICustomMethod;
        /// <summary>
        /// 数组序列化
        /// </summary>
        internal static readonly MethodInfo LeftArrayMethod;
        /// <summary>
        /// 数组序列化
        /// </summary>
        internal static readonly MethodInfo ListArrayMethod;
        /// <summary>
        /// 数组序列化
        /// </summary>
        internal static readonly MethodInfo ArrayMethod;
        /// <summary>
        /// 可空数组序列化
        /// </summary>
        internal static readonly MethodInfo NullableArrayMethod;
        /// <summary>
        /// 可空数据序列化
        /// </summary>
        internal static readonly MethodInfo NullableMethod;
        /// <summary>
        /// 集合序列化
        /// </summary>
        internal static readonly MethodInfo CollectionMethod;
        /// <summary>
        /// 字典序列化
        /// </summary>
        internal static readonly MethodInfo StringDictionaryMethod;
        /// <summary>
        /// 字典序列化
        /// </summary>
        internal static readonly MethodInfo DictionaryMethod;
        /// <summary>
        /// 字典序列化
        /// </summary>
        internal static readonly MethodInfo StringIDictionaryMethod;
        /// <summary>
        /// 字典序列化
        /// </summary>
        internal static readonly MethodInfo IDictionaryMethod;
        /// <summary>
        /// 键值对序列化
        /// </summary>
        internal static readonly MethodInfo KeyValuePairMethod;
        /// <summary>
        /// 序列化模板
        /// </summary>
        /// <param name="value"></param>
        internal void EnumJsonSerializeMethodName(object value) { }
        /// <summary>
        /// 序列化模板
        /// </summary>
        /// <param name="value"></param>
        internal void SerializeMethodName(object value) { }
        ///// <summary>
        ///// 自定义 JSON 序列化
        ///// </summary>
        ///// <param name="value"></param>
        //internal void ICustom(object value) { }
        ///// <summary>
        ///// 序列化模板
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        //internal static void JsonSerialize<T>() { }
        ///// <summary>
        ///// 序列化模板
        ///// </summary>
        ///// <returns></returns>
        //internal static void TypeSerialize(Type type) { }
        /// <summary>
        /// 序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        internal static void ReflectionMethodName<T>(params object[] values) { }
#else
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <param name="value">Data object</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>JSON 字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static string SerializeObject(object? value, JsonSerializeConfig? config = null)
#else
        public static string SerializeObject(object value, JsonSerializeConfig config = null)
#endif
        {
            if (value != null) return AutoCSer.Metadata.GenericType.Get(value.GetType()).JsonSerializeObjectGenericDelegate(value, config).Key;
            return NullString;
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <param name="value">Data object</param>
        /// <param name="warning">警告提示状态</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>JSON 字符串</returns>
#if NetStandard21
        public static string SerializeObject(object? value, out AutoCSer.TextSerialize.WarningEnum warning, JsonSerializeConfig? config = null)
#else
        public static string SerializeObject(object value, out AutoCSer.TextSerialize.WarningEnum warning, JsonSerializeConfig config = null)
#endif
        {
            if (value != null)
            {
                KeyValue<string, AutoCSer.TextSerialize.WarningEnum> json = AutoCSer.Metadata.GenericType.Get(value.GetType()).JsonSerializeObjectGenericDelegate(value, config);
                warning = json.Value;
                return json.Key;
            }
            warning = AutoCSer.TextSerialize.WarningEnum.None;
            return NullString;
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="objectValue">数据对象</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>JSON 字符串 + 警告提示状态</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static KeyValue<string, AutoCSer.TextSerialize.WarningEnum> Serialize<T>(object objectValue, JsonSerializeConfig? config)
#else
        internal static KeyValue<string, AutoCSer.TextSerialize.WarningEnum> Serialize<T>(object objectValue, JsonSerializeConfig config)
#endif
        {
            var value = (T)objectValue;
            AutoCSer.TextSerialize.WarningEnum warning;
            return new KeyValue<string, AutoCSer.TextSerialize.WarningEnum>(Serialize(ref value, out warning, config), warning);
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <param name="value">Data object</param>
        /// <param name="jsonStream">JSON 输出缓冲区</param>
        /// <param name="config">Configuration parameters</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static AutoCSer.TextSerialize.WarningEnum SerializeObject(object? value, CharStream jsonStream, JsonSerializeConfig? config = null)
#else
        public static AutoCSer.TextSerialize.WarningEnum SerializeObject(object value, CharStream jsonStream, JsonSerializeConfig config = null)
#endif
        {
            if (value != null) return AutoCSer.Metadata.GenericType.Get(value.GetType()).JsonSerializeStreamObjectDelegate(value, jsonStream, config);
            jsonStream.WriteJsonNull();
            return AutoCSer.TextSerialize.WarningEnum.None;
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="objectValue">数据对象</param>
        /// <param name="jsonStream">JSON 输出缓冲区</param>
        /// <param name="config">Configuration parameters</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static AutoCSer.TextSerialize.WarningEnum Serialize<T>(object objectValue, CharStream jsonStream, JsonSerializeConfig? config)
#else
        internal static AutoCSer.TextSerialize.WarningEnum Serialize<T>(object objectValue, CharStream jsonStream, JsonSerializeConfig config)
#endif
        {
            T value = (T)objectValue;
            return Serialize(ref value, jsonStream, config);
        }
#endif

        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>JSON 字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static string Serialize<T>(T? value, JsonSerializeConfig? config = null)
#else
        public static string Serialize<T>(T value, JsonSerializeConfig config = null)
#endif
        {
            AutoCSer.TextSerialize.WarningEnum warning;
            return Serialize(ref value, out warning, config);
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>JSON 字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static string Serialize<T>(ref T? value, JsonSerializeConfig? config = null)
#else
        public static string Serialize<T>(ref T value, JsonSerializeConfig config = null)
#endif
        {
            AutoCSer.TextSerialize.WarningEnum warning;
            return Serialize(ref value, out warning, config);
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="warning">警告提示状态</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>JSON 字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static string Serialize<T>(T? value, out AutoCSer.TextSerialize.WarningEnum warning, JsonSerializeConfig? config = null)
#else
        public static string Serialize<T>(T value, out AutoCSer.TextSerialize.WarningEnum warning, JsonSerializeConfig config = null)
#endif
        {
            return Serialize(ref value, out warning, config);
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="warning">警告提示状态</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>JSON 字符串</returns>
#if NetStandard21
        public static string Serialize<T>(ref T? value, out AutoCSer.TextSerialize.WarningEnum warning, JsonSerializeConfig? config = null)
#else
        public static string Serialize<T>(ref T value, out AutoCSer.TextSerialize.WarningEnum warning, JsonSerializeConfig config = null)
#endif
        {
            if (value != null)
            {
                JsonSerializer jsonSerializer = AutoCSer.Threading.LinkPool<JsonSerializer>.Default.Pop() ?? new JsonSerializer();
                try
                {
                    string json = jsonSerializer.serialize(ref value, config);
                    warning = jsonSerializer.Warning;
                    return json;
                }
                finally { jsonSerializer.Free(); }
            }
            warning = AutoCSer.TextSerialize.WarningEnum.None;
            return NullString;
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="jsonStream">JSON 输出缓冲区</param>
        /// <param name="config">Configuration parameters</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static AutoCSer.TextSerialize.WarningEnum Serialize<T>(T? value, CharStream jsonStream, JsonSerializeConfig? config = null)
#else
        public static AutoCSer.TextSerialize.WarningEnum Serialize<T>(T value, CharStream jsonStream, JsonSerializeConfig config = null)
#endif
        {
            return Serialize(ref value, jsonStream, config);
        }
        /// <summary>
        /// 对象转换 JSON 字符串
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="jsonStream">JSON 输出缓冲区</param>
        /// <param name="config">Configuration parameters</param>
#if NetStandard21
        public static AutoCSer.TextSerialize.WarningEnum Serialize<T>(ref T value, CharStream jsonStream, JsonSerializeConfig? config = null)
#else
        public static AutoCSer.TextSerialize.WarningEnum Serialize<T>(ref T value, CharStream jsonStream, JsonSerializeConfig config = null)
#endif
        {
            if (value != null)
            {
                if (!jsonStream.IsResizeError)
                {
                    JsonSerializer jsonSerializer = AutoCSer.Threading.LinkPool<JsonSerializer>.Default.Pop() ?? new JsonSerializer();
                    try
                    {
                        jsonSerializer.serialize(ref value, jsonStream, config);
                        return jsonSerializer.Warning;
                    }
                    finally { jsonSerializer.Free(); }
                }
            }
            else jsonStream.WriteJsonNull();
            return jsonStream.IsResizeError ? TextSerialize.WarningEnum.ResizeError : AutoCSer.TextSerialize.WarningEnum.None;
        }

        /// <summary>
        /// 对象转换 JSON 字符串（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>JSON 字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static string ThreadStaticSerialize<T>(T? value, JsonSerializeConfig? config = null)
#else
        public static string ThreadStaticSerialize<T>(T value, JsonSerializeConfig config = null)
#endif
        {
            AutoCSer.TextSerialize.WarningEnum warning;
            return ThreadStaticSerialize(ref value, out warning, config);
        }
        /// <summary>
        /// 对象转换 JSON 字符串（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>JSON 字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static string ThreadStaticSerialize<T>(ref T? value, JsonSerializeConfig? config = null)
#else
        public static string ThreadStaticSerialize<T>(ref T value, JsonSerializeConfig config = null)
#endif
        {
            AutoCSer.TextSerialize.WarningEnum warning;
            return ThreadStaticSerialize(ref value, out warning, config);
        }
        /// <summary>
        /// 对象转换 JSON 字符串（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="warning">警告提示状态</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>JSON 字符串</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static string ThreadStaticSerialize<T>(T? value, out AutoCSer.TextSerialize.WarningEnum warning, JsonSerializeConfig? config = null)
#else
        public static string ThreadStaticSerialize<T>(T value, out AutoCSer.TextSerialize.WarningEnum warning, JsonSerializeConfig config = null)
#endif
        {
            return ThreadStaticSerialize(ref value, out warning, config);
        }
        /// <summary>
        /// 对象转换 JSON 字符串（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="warning">警告提示状态</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>JSON 字符串</returns>
#if NetStandard21
        public static string ThreadStaticSerialize<T>(ref T? value, out AutoCSer.TextSerialize.WarningEnum warning, JsonSerializeConfig? config = null)
#else
        public static string ThreadStaticSerialize<T>(ref T value, out AutoCSer.TextSerialize.WarningEnum warning, JsonSerializeConfig config = null)
#endif
        {
            if (value != null)
            {
                JsonSerializer jsonSerializer = ThreadStaticSerializer.Get().Serializer;
                try
                {
                    string json = jsonSerializer.serializeThreadStatic(ref value, config);
                    warning = jsonSerializer.Warning;
                    return json;
                }
                finally { jsonSerializer.freeThreadStatic(); }
            }
            warning = AutoCSer.TextSerialize.WarningEnum.None;
            return NullString;
        }

        /// <summary>
        /// 基本类型转换函数
        /// </summary>
        internal static readonly Dictionary<HashObject<Type>, AutoCSer.TextSerialize.DelegateReference> SerializeDelegates;

        static JsonSerializer()
        {
            SerializeDelegates = AutoCSer.DictionaryCreator.CreateHashObject<System.Type, AutoCSer.TextSerialize.DelegateReference>();
            SerializeDelegates.Add(typeof(bool), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, bool>)primitiveSerialize));
            SerializeDelegates.Add(typeof(bool?), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, bool?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(byte), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, byte>)primitiveSerialize));
            SerializeDelegates.Add(typeof(byte?), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, byte?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(sbyte), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, sbyte>)primitiveSerialize));
            SerializeDelegates.Add(typeof(sbyte?), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, sbyte?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(short), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, short>)primitiveSerialize));
            SerializeDelegates.Add(typeof(short?), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, short?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ushort), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, ushort>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ushort?), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, ushort?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(int), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, int>)primitiveSerialize));
            SerializeDelegates.Add(typeof(int?), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, int?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(uint), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, uint>)primitiveSerialize));
            SerializeDelegates.Add(typeof(uint?), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, uint?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(long), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, long>)primitiveSerialize));
            SerializeDelegates.Add(typeof(long?), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, long?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ulong), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, ulong>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ulong?), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, ulong?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(float), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, float>)primitiveSerialize));
            SerializeDelegates.Add(typeof(float?), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, float?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(double), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, double>)primitiveSerialize));
            SerializeDelegates.Add(typeof(double?), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, double?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(decimal), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, decimal>)primitiveSerialize));
            SerializeDelegates.Add(typeof(decimal?), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, decimal?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(char), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, char>)primitiveSerialize));
            SerializeDelegates.Add(typeof(char?), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, char?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(DateTime), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, DateTime>)primitiveSerialize));
            SerializeDelegates.Add(typeof(DateTime?), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, DateTime?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(TimeSpan), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, TimeSpan>)primitiveSerialize));
            SerializeDelegates.Add(typeof(TimeSpan?), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, TimeSpan?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(Guid), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, Guid>)primitiveSerialize));
            SerializeDelegates.Add(typeof(Guid?), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, Guid?>)primitiveSerialize));
            SerializeDelegates.Add(typeof(string), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, string>)primitiveSerialize));
            SerializeDelegates.Add(typeof(SubString), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, SubString>)primitiveSerialize));
            SerializeDelegates.Add(typeof(object), new AutoCSer.TextSerialize.DelegateReference { Delegate = new AutoCSer.TextSerialize.SerializeDelegate((Action<JsonSerializer, object>)primitiveSerialize), PushType = AutoCSer.TextSerialize.PushTypeEnum.DepthCount, IsUnknownMember = true, IsCompleted = true });
            SerializeDelegates.Add(typeof(Type), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, Type>)primitiveSerialize));
            SerializeDelegates.Add(typeof(JsonNode), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, JsonNode>)primitiveSerialize));
            SerializeDelegates.Add(typeof(bool[]), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, bool[]>)primitiveSerialize));
            SerializeDelegates.Add(typeof(byte[]), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, byte[]>)primitiveSerialize));
            SerializeDelegates.Add(typeof(sbyte[]), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, sbyte[]>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ushort[]), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, ushort[]>)primitiveSerialize));
            SerializeDelegates.Add(typeof(short[]), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, short[]>)primitiveSerialize));
            SerializeDelegates.Add(typeof(int[]), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, int[]>)primitiveSerialize));
            SerializeDelegates.Add(typeof(uint[]), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, uint[]>)primitiveSerialize));
            SerializeDelegates.Add(typeof(long[]), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, long[]>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ulong[]), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, ulong[]>)primitiveSerialize));
            SerializeDelegates.Add(typeof(DateTime[]), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, DateTime[]>)primitiveSerialize));
            SerializeDelegates.Add(typeof(TimeSpan[]), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, TimeSpan[]>)primitiveSerialize));

            SerializeDelegates.Add(typeof(Half), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, Half>)primitiveSerialize));
            SerializeDelegates.Add(typeof(UInt128), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, UInt128>)primitiveSerialize));
            SerializeDelegates.Add(typeof(Int128), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, Int128>)primitiveSerialize));
            SerializeDelegates.Add(typeof(System.Numerics.Complex), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, System.Numerics.Complex>)primitiveSerialize));
            SerializeDelegates.Add(typeof(System.Numerics.Plane), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, System.Numerics.Plane>)primitiveSerialize));
            SerializeDelegates.Add(typeof(System.Numerics.Quaternion), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, System.Numerics.Quaternion>)primitiveSerialize));
            SerializeDelegates.Add(typeof(System.Numerics.Matrix3x2), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, System.Numerics.Matrix3x2>)primitiveSerialize));
            SerializeDelegates.Add(typeof(System.Numerics.Matrix4x4), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, System.Numerics.Matrix4x4>)primitiveSerialize));
            SerializeDelegates.Add(typeof(System.Numerics.Vector2), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, System.Numerics.Vector2>)primitiveSerialize));
            SerializeDelegates.Add(typeof(System.Numerics.Vector3), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, System.Numerics.Vector3>)primitiveSerialize));
            SerializeDelegates.Add(typeof(System.Numerics.Vector4), new AutoCSer.TextSerialize.DelegateReference((Action<JsonSerializer, System.Numerics.Vector4>)primitiveSerialize));
#if AOT
            foreach (MethodInfo method in typeof(JsonSerializer).GetMethods(BindingFlags.Static | BindingFlags.Public))
            {
                switch (method.Name.Length)
                {
                    case 4:
                        if (method.Name == nameof(Base)) BaseMethod = method;
                        break;
                    case 5:
                        if (method.Name == nameof(Array)) ArrayMethod = method;
                        break;
                    case 6:
                        if (method.Name == nameof(Object)) ObjectMethod = method;
                        break;
                    case 7:
                        if (method.Name == nameof(EnumInt)) EnumIntMethod = method;
                        else if (method.Name == nameof(ICustom)) ICustomMethod = method;
                        break;
                    case 8:
                        if (method.Name == nameof(EnumByte)) EnumByteMethod = method;
                        else if (method.Name == nameof(EnumLong)) EnumLongMethod = method;
                        else if (method.Name == nameof(EnumUInt)) EnumUIntMethod = method;
                        else if (method.Name == nameof(Nullable)) NullableMethod = method;
                        break;
                    case 9:
                        if (method.Name == nameof(EnumULong)) EnumULongMethod = method;
                        else if (method.Name == nameof(LeftArray)) LeftArrayMethod = method;
                        else if (method.Name == nameof(EnumShort)) EnumShortMethod = method;
                        else if (method.Name == nameof(EnumSByte)) EnumSByteMethod = method;
                        else if (method.Name == nameof(ListArray)) ListArrayMethod = method;
                        break;
                    case 10:
                        if (method.Name == nameof(Collection)) CollectionMethod = method;
                        else if (method.Name == nameof(Dictionary)) DictionaryMethod = method;
                        else if (method.Name == nameof(EnumUShort)) EnumUShortMethod = method;
                        else if (method.Name == nameof(NotSupport)) NotSupportMethod = method;
                        break;
                    case 11:
                        if (method.Name == nameof(IDictionary)) IDictionaryMethod = method;
                        break;
                    case 12:
                        if (method.Name == nameof(KeyValuePair)) KeyValuePairMethod = method;
                        break;
                    case 13:
                        if (method.Name == nameof(NullableArray)) NullableArrayMethod = method;
                        break;
                    case 16:
                        if (method.Name == nameof(StringDictionary)) StringDictionaryMethod = method;
                        break;
                    case 17:
                        if (method.Name == nameof(StringIDictionary)) StringIDictionaryMethod = method;
                        break;
                }
            }
#else
            foreach (AutoCSer.TextSerialize.SerializeDelegate serializeDelegate in CustomConfig.PrimitiveSerializeDelegates)
            {
                var type = default(Type);
                AutoCSer.TextSerialize.DelegateReference serializeDelegateReference;
                if (serializeDelegate.Check(typeof(JsonSerializer), out type, out serializeDelegateReference))
                {
                    SerializeDelegates[type] = serializeDelegateReference;
                }
            }
#endif
        }
    }
}
