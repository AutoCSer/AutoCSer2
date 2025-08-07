using AutoCSer.BinarySerialize;
using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using AutoCSer.Net;
using AutoCSer.SimpleSerialize;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

namespace AutoCSer
{
    /// <summary>
    /// Binary data serialization
    /// 二进制数据序列化
    /// </summary>
    public sealed partial class BinarySerializer : AutoCSer.Threading.Link<BinarySerializer>, IDisposable
    {
        /// <summary>
        /// null object
        /// </summary>
        public const int NullValue = int.MinValue;
        /// <summary>
        /// not null object
        /// </summary>
        public const int NotNullValue = NullValue + 1;
        /// <summary>
        /// Real type
        /// 真实类型
        /// </summary>
        public const int RealTypeValue = NotNullValue + 1;
        /// <summary>
        /// Array reference execution type
        /// 数组引用执行类型
        /// </summary>
        private const AutoCSer.BinarySerialize.SerializePushTypeEnum arraySerializePushType = AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference;
        /// <summary>
        /// Custom global configuration
        /// 自定义全局配置
        /// </summary>
        public static readonly CustomConfig CustomConfig = AutoCSer.Configuration.Common.Get<CustomConfig>()?.Value ?? new CustomConfig();
#if AOT
        /// <summary>
        /// Default binary data serialization type configuration
        /// 默认二进制数据序列化类型配置
        /// </summary>
        internal static readonly BinarySerializeAttribute DefaultAttribute = new BinarySerializeAttribute { IsBaseType = false };
#else
        /// <summary>
        /// Default binary data serialization type configuration
        /// 默认二进制数据序列化类型配置
        /// </summary>
        internal static readonly BinarySerializeAttribute DefaultAttribute = AutoCSer.Configuration.Common.Get<BinarySerializeAttribute>()?.Value ?? new BinarySerializeAttribute { IsBaseType = false };
#endif
        /// <summary>
        /// Public default configuration parameters
        /// 公共默认配置参数
        /// </summary>
        internal static readonly BinarySerializeConfig DefaultConfig = AutoCSer.Configuration.Common.Get<BinarySerializeConfig>()?.Value ?? new BinarySerializeConfig();
        /// <summary>
        /// Copy the public default configuration parameters
        /// 复制公共默认配置参数
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static BinarySerializeConfig CloneDefaultConfig()
        {
            return DefaultConfig.Clone();
        }

        /// <summary>
        /// Output buffer stream
        /// 序列化输出缓冲区流
        /// </summary>
        public readonly UnmanagedStream Stream;
        /// <summary>
        /// Custom context
        /// 自定义上下文
        /// </summary>
#if NetStandard21
        internal object? Context;
#else
        internal object Context;
#endif
        /// <summary>
        /// Serialization configuration parameters
        /// 序列化配置参数
        /// </summary>
        internal BinarySerializeConfig Config = DefaultConfig;
        /// <summary>
        /// Member bitmap
        /// 成员位图
        /// </summary>
#if NetStandard21
        internal MemberMap? MemberMap;
#else
        internal MemberMap MemberMap;
#endif
        /// <summary>
        /// Current member bitmap
        /// 成员位图
        /// </summary>
#if NetStandard21
        internal MemberMap? CurrentMemberMap;
#else
        internal MemberMap CurrentMemberMap;
#endif
        /// <summary>
        /// JSON serialization member bitmap
        /// JSON 序列化成员位图
        /// </summary>
#if NetStandard21
        internal MemberMap? JsonMemberMap;
#else
        internal MemberMap JsonMemberMap;
#endif
        /// <summary>
        /// JSON serialization
        /// </summary>
#if NetStandard21
        private JsonSerializer? jsonSerializer;
#else
        private JsonSerializer jsonSerializer;
#endif
        /// <summary>
        /// Historically reference the position of the object pointer
        /// 历史引用对象指针位置
        /// </summary>
#if NetStandard21
        private ReusableDictionary<ObjectReferenceType, int>? points;
#else
        private ReusableDictionary<ObjectReferenceType, int> points;
#endif
        /// <summary>
        /// Node nesting depth counting
        /// 节点嵌套层级计数
        /// </summary>
        internal int CurrentDepth;
        /// <summary>
        /// The starting position of the data stream
        /// 数据流起始位置
        /// </summary>
        private int streamStartIndex;
        /// <summary>
        /// Non-reference counting
        /// 增加非引用计数
        /// </summary>
        private bool notReferenceCount;
        /// <summary>
        /// Is any object that are referenced
        /// 是否存在对象被引用
        /// </summary>
        private bool isObjectReference;
        /// <summary>
        /// Warning prompt status
        /// 警告提示状态
        /// </summary>
        public SerializeWarningEnum Warning { get; internal set; }
        /// <summary>
        /// Is serialization operations are being processed
        /// 是否正在处理序列化操作
        /// </summary>
        private bool isProcessing;
        /// <summary>
        /// Binary data serialization
        /// 二进制数据序列化
        /// </summary>
        /// <param name="isThreadStatic">Is static instance mode of the thread
        /// 是否线程静态实例模式</param>
        internal BinarySerializer(bool isThreadStatic = false)
        {
            Stream = isThreadStatic ? new UnmanagedStream(AutoCSer.Common.Config.SerializeUnmanagedPool) : new UnmanagedStream(default(AutoCSer.Memory.UnmanagedPoolPointer));
        }
        /// <summary>
        /// Object serialization
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="config">Configuration parameters</param>
        /// <param name="warning"></param>
        /// <returns>Serialized data
        /// 序列化数据</returns>
#if NetStandard21
        private byte[] serialize<T>(ref T value, BinarySerializeConfig? config, out SerializeWarningEnum warning)
#else
        private byte[] serialize<T>(ref T value, BinarySerializeConfig config, out SerializeWarningEnum warning)
#endif
        {
            Config = config ?? DefaultConfig;
            Stream.TrySetDataCanResize(AutoCSer.Common.Config.SerializeUnmanagedPool);
            using (Stream)
            {
                if (Config.StreamSeek > 0) Stream.PrepSize(Config.StreamSeek);
                serialize(ref value);
                warning = Warning;
                return Stream.Data.Pointer.GetArray();
            }
        }
        /// <summary>
        /// Object serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        private void serialize<T>(BinarySerializer serializer, ref T value)
        {
            Config = DefaultConfig;
            UnmanagedStreamExchangeBuffer buffer;
            Stream.ExchangeToBuffer(serializer.Stream, out buffer);
            try
            {
                if (Config.StreamSeek <= 0 || Stream.PrepSize(Config.StreamSeek)) serialize(ref value);
                if (Stream.IsResizeError) Warning |= SerializeWarningEnum.ResizeError;
                serializer.Warning |= Warning;
            }
            finally { Stream.ExchangeFromBuffer(serializer.Stream, ref buffer); }
        }
        /// <summary>
        /// Object serialization (Thread static Instance pattern)
        /// 对象序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="config">Configuration parameters</param>
        /// <param name="warning"></param>
        /// <returns></returns>
#if NetStandard21
        private byte[] serializeThreadStatic<T>(ref T value, BinarySerializeConfig? config, out SerializeWarningEnum warning)
#else
        private byte[] serializeThreadStatic<T>(ref T value, BinarySerializeConfig config, out SerializeWarningEnum warning)
#endif
        {
            Config = config ?? DefaultConfig;
            Stream.ClearCanResize();
            if (Config.StreamSeek > 0) Stream.PrepSize(Config.StreamSeek);
            serialize(ref value);
            warning = Warning;
            return Stream.Data.Pointer.GetArray();
        }
        /// <summary>
        /// Object serialization
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        private unsafe void serialize<T>(ref T value)
        {
            notReferenceCount = TypeSerializer<T>.SerializeDelegateReference.NotReferenceCount;
            isProcessing = true;
            Warning = SerializeWarningEnum.None;
            CurrentDepth = Config.CheckDepth;
            MemberMap = Config.MemberMap;
            isObjectReference = false;

            streamStartIndex = Config.WriteHeader(Stream);
            TypeSerializer<T>.Serialize(this, ref value);
            if (!isObjectReference) Stream.Write(Stream.Data.Pointer.CurrentIndex - streamStartIndex);
            else
            {
                writePoints();
                Stream.Write(Stream.Data.Pointer.CurrentIndex - streamStartIndex);
                *(int*)(Stream.Data.Pointer.Byte + streamStartIndex) |= BinarySerializeConfig.ObjectReference;
            }
        }
        /// <summary>
        /// Write the pointer to the historical object
        /// 写入历史对象指针
        /// </summary>
        /// <returns></returns>
        private unsafe void writePoints()
        {
            int pointSize = 0;
            foreach (int point in points.notNull().Values)
            {
                if (point < 0)
                {
                    if (Stream.Write(point)) pointSize += sizeof(int);
                    else return;
                }
            }
            if (pointSize > sizeof(int))
            {
                byte* pointEnd = Stream.Data.Pointer.Current;
                AutoCSer.Algorithm.QuickSort.SortInt(pointEnd - pointSize, pointEnd - sizeof(int));
            }
            Stream.Write(pointSize);
        }
        /// <summary>
        /// Set the custom context
        /// 设置自定义上下文
        /// </summary>
        /// <param name="context"></param>
        /// <param name="isSerializeCopyString"></param>
        /// <returns></returns>
        internal UnmanagedStream SetContext(object context, ref bool isSerializeCopyString)
        {
            this.Context = context;
            Config = DefaultConfig;
            isProcessing = true;
            Stream.ExchangeIsSerializeCopyString(ref isSerializeCopyString);
            return Stream;
        }
        /// <summary>
        /// Set the custom context
        /// 设置自定义上下文
        /// </summary>
        /// <param name="context"></param>
        /// <param name="isSerializeCopyString"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal UnmanagedStream SetDefaultContext(object context, ref bool isSerializeCopyString)
        {
            Warning = SerializeWarningEnum.None;
            CurrentDepth = Config.CheckDepth;
            return SetContext(context, ref isSerializeCopyString);
        }
        /// <summary>
        /// Release resources
        /// </summary>
        void IDisposable.Dispose()
        {
            if (isProcessing) throw new InvalidOperationException(AutoCSer.Common.Culture.NotAllowDisposeSerializer);
            //MemberMap = CurrentMemberMap = JsonMemberMap = null;
            if (jsonSerializer != null)
            {
                jsonSerializer.FreeBinaryMix();
                jsonSerializer = null;
            }
            Stream.Dispose();
        }
        /// <summary>
        /// Release resources (Thread static instance mode)
        /// 释放资源（线程静态实例模式）
        /// </summary>
        private void freeThreadStatic()
        {
            MemberMap = CurrentMemberMap = null;
            if (points != null) points.ClearCount();
            isProcessing = false;
        }
        /// <summary>
        /// Release resources
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free()
        {
            freeThreadStatic();
            AutoCSer.Threading.LinkPool<BinarySerializer>.Default.Push(this);
        }
        /// <summary>
        /// Release resources
        /// </summary>
        /// <param name="isSerializeCopyString"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void FreeContext(bool isSerializeCopyString)
        {
            Context = null;
            Stream.Close(isSerializeCopyString);
            Free();
        }
        ///// <summary>
        ///// 获取序列化引用检查类型
        ///// </summary>
        ///// <param name="pushType"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal SerializePushType Check(SerializePushType pushType)
        //{
        //    return pushType == SerializePushType.Primitive ? SerializePushType.Primitive : CheckDepth(pushType);
        //}
        /// <summary>
        /// Gets the serialization reference check type
        /// 获取序列化引用检查类型
        /// </summary>
        /// <param name="pushType"></param>
        /// <returns></returns>
        internal SerializePushTypeEnum CheckDepth(SerializePushTypeEnum pushType)
        {
            if (--CurrentDepth > 0)
            {
                if (!notReferenceCount && Config.CheckReference)
                {
                    switch (pushType)
                    {
                        case SerializePushTypeEnum.NotReferenceCount:
                            notReferenceCount = true;
                            return SerializePushTypeEnum.NotReferenceCount;
                        case SerializePushTypeEnum.TryReference: return SerializePushTypeEnum.TryReference;
                    }
                }
                return SerializePushTypeEnum.DepthCount;
            }
            ++CurrentDepth;
            Warning |= SerializeWarningEnum.DepthOutOfRange;
            return SerializePushTypeEnum.DepthOutOfRange;
        }
        /// <summary>
        /// Gets the serialization reference check type
        /// 获取序列化引用检查类型
        /// </summary>
        /// <param name="pushType"></param>
        /// <returns></returns>
        internal SerializePushTypeEnum CheckDepthWriteNotNull(SerializePushTypeEnum pushType)
        {
            if (--CurrentDepth > 0)
            {
                if (!notReferenceCount && Config.CheckReference)
                {
                    switch (pushType)
                    {
                        case SerializePushTypeEnum.NotReferenceCount:
                            notReferenceCount = true;
                            Stream.Write(NotNullValue);
                            return SerializePushTypeEnum.NotReferenceCount;
                        case SerializePushTypeEnum.TryReference: return SerializePushTypeEnum.TryReference;
                    }
                }
                Stream.Write(NotNullValue);
                return SerializePushTypeEnum.DepthCount;
            }
            ++CurrentDepth;
            Warning |= SerializeWarningEnum.DepthOutOfRange;
            Stream.Write(NullValue);
            return SerializePushTypeEnum.DepthOutOfRange;
        }
        /// <summary>
        /// Clear the non-reference count
        /// 清除非引用计数
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void ClearNotReferenceCount()
        {
            ++CurrentDepth;
            notReferenceCount = false;
        }
        /// <summary>
        /// Gets the serialization reference check type
        /// 获取序列化引用检查类型
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal SerializePushTypeEnum CheckTryReference()
        {
            return !notReferenceCount && Config.CheckReference ? SerializePushTypeEnum.TryReference : SerializePushTypeEnum.DepthCount;
        }
        /// <summary>
        /// Add historical objects
        /// 添加历史对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns>Is continues to serialize objects
        /// 是否继续序列化对象</returns>
#if NET8
        [MemberNotNull(nameof(points))]
#endif
        internal bool CheckPoint<T>(T value)
        {
#if DEBUG
            if (value == null) throw new ArgumentNullException();
#endif
#pragma warning disable CS8604
            ObjectReferenceType key = new ObjectReferenceType(value, typeof(T));
#pragma warning restore CS8604
            if (points == null)
            {
                points = new ReusableDictionary<ObjectReferenceType, int>();
                points.Set(key, Stream.Data.Pointer.CurrentIndex - streamStartIndex);
                return true;
            }
            int point;
            if (!points.TryGetValue(key, out point))
            {
                points.Set(key, Stream.Data.Pointer.CurrentIndex - streamStartIndex);
                return true;
            }
            isObjectReference = true;
            if (point > 0)
            {
                point = -point;
                points[key] = point;
            }
            Stream.Write(point);
            return false;
        }
        /// <summary>
        /// Gets the serialization member bitmap
        /// 获取序列化成员位图
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
#if NetStandard21
        public MemberMap<T>? SerializeMemberMap<T>()
#else
        public MemberMap<T> SerializeMemberMap<T>()
#endif
        {
            if (MemberMap != null)
            {
                CurrentMemberMap = MemberMap;
                MemberMap = null;
                var memberMap = CurrentMemberMap as MemberMap<T>;
                if (memberMap != null)
                {
                    memberMap.MemberMapData.Serialize(Stream);
                    return memberMap;
                }
                Warning |= SerializeWarningEnum.MemberMap;
            }
            return null;
        }
        /// <summary>
        /// Serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value">Data object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Serialize<T>(BinarySerializer binarySerializer, T value)
        {
            if (value != null) TypeSerializer<T>.Serialize(binarySerializer, ref value);
            else binarySerializer.Stream.Write(NullValue);
        }
        /// <summary>
        /// Custom serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void ICustom<T>(ICustomSerialize<T> value)
        {
            value.Serialize(this);
        }
        /// <summary>
        /// Custom serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void ICustom<T>(AutoCSer.BinarySerializer serializer, T value) where T : ICustomSerialize<T>
        {
            value.Serialize(serializer);
        }
        /// <summary>
        /// Get the JSON serialization
        /// 获取 JSON 序列化
        /// </summary>
        /// <returns></returns>
        private JsonSerializer createJsonSerializer()
        {
            jsonSerializer = AutoCSer.Threading.LinkPool<JsonSerializer>.Default.Pop() ?? new JsonSerializer();
            jsonSerializer.SetBinaryMix();
            return jsonSerializer;
        }
        /// <summary>
        /// JSON serialization to the buffer
        /// JSON 序列化到缓冲区
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        internal void JsonSerializeBufferNotNull<T>(ref T value)
        {
            int index = SerializeBufferStart();
            if (index >= 0) SerializeBufferEnd(index, (jsonSerializer ?? createJsonSerializer()).SerializeBufferNotNull(ref value, Stream));
        }
        /// <summary>
        /// JSON serialization to the buffer
        /// JSON 序列化到缓冲区
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void JsonSerializeBuffer<T>(ref T value)
        {
            if (value != null) JsonSerializeBufferNotNull(ref value);
            else Stream.Write(0);
        }
        /// <summary>
        /// JSON mixed binary serialization
        /// JSON 混杂二进制序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="memberMap"></param>
#if NetStandard21
        internal void JsonSerialize<T>(ref T value, MemberMap<T>? memberMap)
#else
        internal void JsonSerialize<T>(ref T value, MemberMap<T> memberMap)
#endif
        {
            int index = Stream.GetMoveSize(sizeof(int));
            if (index != 0)
            {
                (jsonSerializer ?? createJsonSerializer()).Serialize(ref value, Stream, memberMap);
                Stream.JsonSerializeFill(index);
            }
        }
        //        /// <summary>
        //        /// JSON 序列化
        //        /// </summary>
        //        /// <typeparam name="T"></typeparam>
        //        /// <param name="value"></param>
        //        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //#if NetStandard21
        //        public void JsonSerialize<T>(ref T? value)
        //#else
        //        public void JsonSerialize<T>(ref T value)
        //#endif
        //        {
        //            if (value != null) JsonSerialize(ref value, null);
        //            else Stream.Write(0);
        //        }
        /// <summary>
        /// JSON mixed binary serialization
        /// JSON 混杂二进制序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void Json<T>(T value)
        {
            if (value != null)
            {
                switch (CheckDepth(AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        ++CurrentDepth;
                        JsonSerialize(ref value, null);
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        ++CurrentDepth;
                        if (CheckPoint(value)) JsonSerialize(ref value, null);
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// JSON mixed binary serialization
        /// JSON 混杂二进制序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Json<T>(BinarySerializer binarySerializer, T value)
        {
            binarySerializer.Json(value);
        }
        /// <summary>
        /// JSON mixed binary serialization
        /// JSON 混杂二进制序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StructJson<T>(BinarySerializer binarySerializer, T value)
        {
            binarySerializer.JsonSerialize(ref value, null);
        }
        /// <summary>
        /// Binary serialization to simple serialization (for AOT code generation, not allowed for developers to call)
        /// 二进制序列化转简单序列化（用于 AOT 代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public static void Simple<T>(BinarySerializer binarySerializer, T value) where T : struct
#else
        internal static void Simple<T>(BinarySerializer binarySerializer, T value) where T : struct
#endif
        {
            binarySerializer.SimpleSerialize(ref value);
        }
        /// <summary>
        /// Custom serialization does not support types
        /// 自定义序列化不支持的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        public static void NotSupport<T>(BinarySerializer binarySerializer, T value)
        {
            int size = CustomConfig.NotSupport(binarySerializer, value);
            if (size > 0) binarySerializer.Stream.Data.Pointer.MoveSize(size);
        }
        /// <summary>
        /// Real type serialization
        /// 真实类型序列化
        /// </summary>
        /// <param name="value"></param>
        private void realTypeObject<T>(T value)
        {
            Type type = typeof(T);
            switch (CheckTryReference())
            {
                case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                    primitiveSerializeOnly(type);
                    break;
                case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                    if (CheckPoint(type)) primitiveSerializeOnly(type);
                    break;
            }
            TypeSerializer<T>.Serialize(this, ref value);
        }
        /// <summary>
        /// Real type serialization
        /// 真实类型序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void RealTypeObject<T>(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.realTypeObject((T)value);
        }
        /// <summary>
        /// Base type serialization
        /// 基类序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="BT"></typeparam>
        /// <param name="value"></param>
        private void baseSerialize<T, BT>(T value)
            where T : class, BT
        {
            SerializePushTypeEnum pushType = TypeSerializer<BT>.SerializeDelegateReference.PushType;
            if (pushType != SerializePushTypeEnum.Primitive)
            {
                switch (CheckDepthWriteNotNull(pushType))
                {
                    case SerializePushTypeEnum.DepthCount:
                        TypeSerializer<BT>.DefaultSerializer(this, value);
                        ++CurrentDepth;
                        return;
                    case SerializePushTypeEnum.NotReferenceCount:
                        TypeSerializer<BT>.DefaultSerializer(this, value);
                        ClearNotReferenceCount();
                        return;
                    case SerializePushTypeEnum.TryReference:
                        if (CheckPoint(value))
                        {
                            Stream.Write(NotNullValue);
                            TypeSerializer<BT>.DefaultSerializer(this, value);
                        }
                        ++CurrentDepth;
                        return;
                }
            }
            else TypeSerializer<BT>.DefaultSerializer(this, value);
        }
        /// <summary>
        /// Base type serialization
        /// 基类序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="BT"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Base<T, BT>(BinarySerializer binarySerializer, T value)
            where T : class, BT
        {
            binarySerializer.baseSerialize<T, BT>(value);
        }
        /// <summary>
        /// Object serialization
        /// </summary>
        /// <param name="binarySerializer">Binary data serialization</param>
        /// <param name="value">Data object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Nullable<T>(BinarySerializer binarySerializer, T? value) where T : struct
        {
            if (value.HasValue) TypeSerializer<T>.SerializeNullable(binarySerializer, value.Value);
            else binarySerializer.Stream.Write(NullValue);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="array">Array object</param>
        /// <param name="count"></param>
        private unsafe void arrayOnly<T>(T[] array, int count)
        {
            count = Math.Min(array.Length, count);
            if (count > 0)
            {
                SerializeArrayMap arrayMap = new SerializeArrayMap(Stream, count);
                if (arrayMap.WriteIndex != -1)
                {
                    foreach (T value in array)
                    {
                        if (value != null)
                        {
                            arrayMap.NextTrue();
                            TypeSerializer<T>.Serialize(this, value);
                        }
                        else arrayMap.NextFalse();
                        if (--count == 0)
                        {
                            arrayMap.End();
                            return;
                        }
                    }
                }
            }
            else Stream.Write(0);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="array">Array object</param>
#if NetStandard21
        public void BinarySerialize<T>(T[]? array) where T : class
#else
        public void BinarySerialize<T>(T[] array) where T : class
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case SerializePushTypeEnum.DepthCount:
                        arrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) arrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Array<T>(BinarySerializer binarySerializer, T[] array) where T : class
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="array">Array object</param>
#if NetStandard21
        public void BinarySerialize<T>(ListArray<T>? array) where T : class
#else
        public void BinarySerialize<T>(ListArray<T> array) where T : class
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case SerializePushTypeEnum.DepthCount:
                        arrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) arrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void ListArray<T>(BinarySerializer binarySerializer, ListArray<T> array) where T : class
        {
            binarySerializer.BinarySerialize(array);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void LeftArray<T>(BinarySerializer binarySerializer, LeftArray<T> array) where T : class
        {
            binarySerializer.arrayOnly(array.Array ?? EmptyArray<T>.Array, array.Length);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="array">Array object</param>
        /// <param name="count"></param>
        private void structArrayOnly<T>(T[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                Stream.Write(count);
                for (int index = 0; index != count; TypeSerializer<T>.Serialize(this, ref array[index++])) ;
            }
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="array">Array object</param>
#if NetStandard21
        private void structArray<T>(T[]? array)
#else
        private void structArray<T>(T[] array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case SerializePushTypeEnum.DepthCount:
                        structArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) structArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StructArray<T>(BinarySerializer binarySerializer, T[] array)
        {
            binarySerializer.structArray(array);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="array">Array object</param>
#if NetStandard21
        private void structArray<T>(ListArray<T>? array)
#else
        private void structArray<T>(ListArray<T> array)
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case SerializePushTypeEnum.DepthCount:
                        structArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) structArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StructListArray<T>(BinarySerializer binarySerializer, ListArray<T> array)
        {
            binarySerializer.structArray(array);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StructLeftArray<T>(BinarySerializer binarySerializer, LeftArray<T> array)
        {
            binarySerializer.structArrayOnly(array.Array ?? EmptyArray<T>.Array, array.Length);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="array">Array object</param>
        /// <param name="count"></param>
        private unsafe void nullableArrayOnly<T>(T?[] array, int count) where T : struct
        {
            count = Math.Min(array.Length, count);
            if (count > 0)
            {
                SerializeArrayMap arrayMap = new SerializeArrayMap(Stream, count);
                if (arrayMap.WriteIndex != -1)
                {
                    foreach (T? value in array)
                    {
                        if (value.HasValue)
                        {
                            arrayMap.NextTrue();
                            TypeSerializer<T>.Serialize(this, value.Value);
                        }
                        else arrayMap.NextFalse();
                        if (--count == 0)
                        {
                            arrayMap.End();
                            return;
                        }
                    }
                }
                return;
            }
            Stream.Write(0);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="array">Array object</param>
#if NetStandard21
        public void NullableArray<T>(T?[]? array) where T : struct
#else
        public void NullableArray<T>(T?[] array) where T : struct
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case SerializePushTypeEnum.DepthCount:
                        nullableArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                    case SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) nullableArrayOnly(array, array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void NullableArray<T>(BinarySerializer binarySerializer, T?[] array) where T : struct
        {
            binarySerializer.NullableArray(array);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="array">Array object</param>
#if NetStandard21
        public void NullableArray<T>(ListArray<T?>? array) where T : struct
#else
        public void NullableArray<T>(ListArray<T?> array) where T : struct
#endif
        {
            if (array != null)
            {
                switch (CheckDepth(arraySerializePushType))
                {
                    case SerializePushTypeEnum.DepthCount:
                        nullableArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                    case SerializePushTypeEnum.TryReference:
                        if (CheckPoint(array)) nullableArrayOnly(array.Array.Array, array.Array.Length);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void NullableListArray<T>(BinarySerializer binarySerializer, ListArray<T?> array) where T : struct
        {
            binarySerializer.NullableArray(array);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void NullableLeftArray<T>(BinarySerializer binarySerializer, LeftArray<T?> array) where T : struct
        {
            binarySerializer.nullableArrayOnly(array.Array ?? EmptyArray<T?>.Array, array.Length);
        }
        /// <summary>
        /// Collection serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="collection">Collection object</param>
#if NetStandard21
        private unsafe void collectionOnly<T, VT>(T collection) where T : ICollection<VT?>
#else
        private unsafe void collectionOnly<T, VT>(T collection) where T : ICollection<VT>
#endif
        {
            do
            {
                int count = collection.Count;
                if (count > 0)
                {
                    int startIndex = Stream.Data.Pointer.CurrentIndex;
                    if (typeof(VT).IsValueType)
                    {
                        Stream.Write(count);
                        foreach (var value in collection)
                        {
                            if (count == 0)
                            {
                                count = -1;
                                break;
                            }
#pragma warning disable CS8604
                            TypeSerializer<VT>.Serialize(this, value);
#pragma warning restore CS8604
                            --count;
                        }
                        if (count == 0) return;
                    }
                    else
                    {
                        SerializeArrayMap arrayMap = new SerializeArrayMap(Stream, count);
                        if (arrayMap.WriteIndex != -1)
                        {
                            foreach (var value in collection)
                            {
                                if (count == 0)
                                {
                                    count = -1;
                                    break;
                                }
                                if (value != null)
                                {
                                    arrayMap.NextTrue();
                                    TypeSerializer<VT>.Serialize(this, value);
                                }
                                else arrayMap.NextFalse();
                                --count;
                            }
                            if (count == 0)
                            {
                                arrayMap.End();
                                return;
                            }
                        }
                    }
                    if (Stream.IsResizeError) return;
                    Stream.Data.Pointer.CurrentIndex = startIndex;
                }
                else
                {
                    Stream.Write(0);
                    return;
                }
            }
            while (true);
        }
        /// <summary>
        /// Collection serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="collection">Collection object</param>
#if NetStandard21
        public void Collection<T, VT>(T collection) where T : ICollection<VT?>
#else
        public void Collection<T, VT>(T collection) where T : ICollection<VT>
#endif
        {
            if (typeof(T).IsValueType)
            {
                if (CurrentDepth > 1)
                {
                    collectionOnly<T, VT>(collection);
                    ++CurrentDepth;
                    return;
                }
            }
            else
            {
                if (collection != null)
                {
                    switch (CheckDepth(SerializePushTypeEnum.TryReference))
                    {
                        case SerializePushTypeEnum.DepthCount:
                            collectionOnly<T, VT>(collection);
                            ++CurrentDepth;
                            return;
                        case SerializePushTypeEnum.TryReference:
                            if (CheckPoint(collection)) collectionOnly<T, VT>(collection);
                            ++CurrentDepth;
                            return;
                    }
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// Collection serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="collection">Collection object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void Collection<T, VT>(BinarySerializer binarySerializer, T collection) where T : ICollection<VT?>
#else
        internal static void Collection<T, VT>(BinarySerializer binarySerializer, T collection) where T : ICollection<VT>
#endif
        {
            binarySerializer.Collection<T, VT>(collection);
        }
        /// <summary>
        /// Dictionary serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="dictionary"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void dictionaryOnly<T, KT, VT>(T dictionary)
            where T : IDictionary<KT, VT>
        {
            do
            {
                int count = dictionary.Count;
                if (count != 0)
                {
                    int startIndex = Stream.Data.Pointer.CurrentIndex;
                    Stream.Write(count);
                    KeyValue<KT, VT> keyValue = default(KeyValue<KT, VT>);
                    foreach (KeyValuePair<KT, VT> value in dictionary)
                    {
                        if (count == 0)
                        {
                            count = -1;
                            break;
                        }
                        keyValue.Set(value.Key, value.Value);
                        TypeSerializer<KeyValue<KT, VT>>.Serialize(this, keyValue);
                        --count;
                    }
                    if (count == 0 || Stream.IsResizeError) return;
                    Stream.Data.Pointer.CurrentIndex = startIndex;
                }
                else
                {
                    Stream.Write(0);
                    return;
                }
            }
            while (true);
        }
        /// <summary>
        /// Dictionary serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="dictionary"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Dictionary<T, KT, VT>(T dictionary)
            where T : IDictionary<KT, VT>
        {
            if (typeof(T).IsValueType)
            {
                if (CurrentDepth > 1)
                {
                    dictionaryOnly<T, KT, VT>(dictionary);
                    ++CurrentDepth;
                    return;
                }
            }
            else
            {
                if (dictionary != null)
                {
                    switch (CheckDepth(SerializePushTypeEnum.TryReference))
                    {
                        case SerializePushTypeEnum.DepthCount:
                            dictionaryOnly<T, KT, VT>(dictionary);
                            ++CurrentDepth;
                            return;
                        case SerializePushTypeEnum.TryReference:
                            if (CheckPoint(dictionary)) dictionaryOnly<T, KT, VT>(dictionary);
                            ++CurrentDepth;
                            return;
                    }
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// Dictionary serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="dictionary"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Dictionary<T, KT, VT>(BinarySerializer binarySerializer, T dictionary)
            where T : IDictionary<KT, VT>
        {
            binarySerializer.Dictionary<T, KT, VT>(dictionary);
        }

        /// <summary>
        /// Serialization of enumeration values
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer">Binary data serialization</param>
        /// <param name="value">Enumeration value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static void EnumByte<T>(BinarySerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.Stream.Write((uint)AutoCSer.Metadata.EnumGenericType<T, byte>.ToInt(value));
        }
        /// <summary>
        /// Serialization of enumeration values
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer">Binary data serialization</param>
        /// <param name="value">Enumeration value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static void EnumSByte<T>(BinarySerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.Stream.Write((int)AutoCSer.Metadata.EnumGenericType<T, sbyte>.ToInt(value));
        }
        /// <summary>
        /// Serialization of enumeration values
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer">Binary data serialization</param>
        /// <param name="value">Enumeration value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static void EnumShort<T>(BinarySerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.Stream.Write((int)AutoCSer.Metadata.EnumGenericType<T, short>.ToInt(value));
        }
        /// <summary>
        /// Serialization of enumeration values
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer">Binary data serialization</param>
        /// <param name="value">Enumeration value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static void EnumUShort<T>(BinarySerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.Stream.Write((uint)AutoCSer.Metadata.EnumGenericType<T, ushort>.ToInt(value));
        }
        /// <summary>
        /// Serialization of enumeration values
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer">Binary data serialization</param>
        /// <param name="value">Enumeration value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static void EnumInt<T>(BinarySerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.Stream.Write(AutoCSer.Metadata.EnumGenericType<T, int>.ToInt(value));
        }
        /// <summary>
        /// Serialization of enumeration values
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer">Binary data serialization</param>
        /// <param name="value">Enumeration value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static void EnumUInt<T>(BinarySerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.Stream.Write(AutoCSer.Metadata.EnumGenericType<T, uint>.ToInt(value));
        }
        /// <summary>
        /// Serialization of enumeration values
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer">Binary data serialization</param>
        /// <param name="value">Enumeration value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static void EnumLong<T>(BinarySerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.Stream.Write(AutoCSer.Metadata.EnumGenericType<T, long>.ToInt(value));
        }
        /// <summary>
        /// Serialization of enumeration values
        /// 枚举值序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer">Binary data serialization</param>
        /// <param name="value">Enumeration value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe static void EnumULong<T>(BinarySerializer serializer, T value) where T : struct, IConvertible
        {
            serializer.Stream.Write(AutoCSer.Metadata.EnumGenericType<T, ulong>.ToInt(value));
        }

        /// <summary>
        /// Logical value serialization
        /// 逻辑值序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value">Logical value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, bool value)
        {
            binarySerializer.Stream.Write(value ? (int)1 : 0);
        }
        /// <summary>
        /// Logical value serialization
        /// 逻辑值序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value">Logical value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, bool? value)
        {
            binarySerializer.Stream.Write(value.HasValue ? (value.Value ? 1 : 0) : NullValue);
        }
        /// <summary>
        /// Serialization of logical value arrays
        /// 逻辑值数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(bool[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                SerializeArrayMap arrayMap = new SerializeArrayMap(Stream, count);
                if (arrayMap.WriteIndex != -1)
                {
                    foreach (bool value in array)
                    {
                        arrayMap.Next(value);
                        if (--count == 0)
                        {
                            arrayMap.End();
                            return;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Serialization of logical value arrays
        /// 逻辑值数组序列化
        /// </summary>
        /// <param name="array"></param>
        private unsafe void primitiveSerializeOnly(bool?[] array)
        {
            if (array.Length != 0)
            {
                SerializeArrayMap arrayMap = new SerializeArrayMap(Stream, array.Length << 1);
                if (arrayMap.WriteIndex != -1)
                {
                    foreach (bool? value in array) arrayMap.Next(value);
                    arrayMap.End();
                }
            }
            else Stream.Write(0);
        }
        /// <summary>
        /// Integer value serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, byte value)
        {
            binarySerializer.Stream.Write((uint)value);
        }
        /// <summary>
        /// Integer value serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, byte? value)
        {
            if(value.HasValue) binarySerializer.Stream.Write((uint)value.Value);
            else binarySerializer.Stream.Write(NullValue);
        }
        /// <summary>
        /// Integer value serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, sbyte value)
        {
            binarySerializer.Stream.Write((int)value);
        }
        /// <summary>
        /// Integer value serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, sbyte? value)
        {
            binarySerializer.Stream.Write(value.HasValue ? value.Value : NullValue);
        }
        /// <summary>
        /// Integer value serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, short value)
        {
            binarySerializer.Stream.Write((int)value);
        }
        /// <summary>
        /// Integer value serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, short? value)
        {
            binarySerializer.Stream.Write(value.HasValue ? value.Value : NullValue);
        }
        /// <summary>
        /// Integer value serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, ushort value)
        {
            binarySerializer.Stream.Write((uint)value);
        }
        /// <summary>
        /// Integer value serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, ushort? value)
        {
            if (value.HasValue) binarySerializer.Stream.Write((uint)value.Value);
            else binarySerializer.Stream.Write(NullValue);
        }
        /// <summary>
        /// Integer value serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, char value)
        {
            binarySerializer.Stream.Write((uint)value);
        }
        /// <summary>
        /// Integer value serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, char? value)
        {
            if (value.HasValue) binarySerializer.Stream.Write((uint)value.Value);
            else binarySerializer.Stream.Write(NullValue);
        }
        /// <summary>
        /// Serialization of floating-point numbers
        /// 浮点数序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private unsafe static void primitiveSerialize(BinarySerializer binarySerializer, Half value)
        {
            binarySerializer.Stream.Write((uint)*(ushort*)&value);
        }
        /// <summary>
        /// Serialization of floating-point number groups
        /// 浮点数数组序列化
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        private unsafe void primitiveSerializeOnly(Half[] array, int count)
        {
            if (count == 0) Stream.Write(0);
            else
            {
                fixed (Half* arrayFixed = array) Stream.Serialize(arrayFixed, count, count * sizeof(Half));
            }
        }
        /// <summary>
        /// Integer value serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, Guid value)
        {
            binarySerializer.Stream.Write(ref value);
        }
        /// <summary>
        /// Member serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, Guid value)
        {
            binarySerializer.Stream.Data.Pointer.Write(ref value);
        }
        /// <summary>
        /// String serialization
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public unsafe void BinarySerialize(string? value)
#else
        public unsafe void BinarySerialize(string value)
#endif
        {
            if (value != null)
            {
                if (value.Length == 0)
                {
                    Stream.Write(0);
                    return;
                }
                switch (CheckDepth(AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        ++CurrentDepth;
                        fixed (char* stringFixed = value) Stream.Serialize(stringFixed, value.Length);
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        ++CurrentDepth;
                        if (CheckPoint(value))
                        {
                            fixed (char* stringFixed = value) Stream.Serialize(stringFixed, value.Length);
                        }
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// String serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, string value)
        {
            binarySerializer.BinarySerialize(value);
        }
        /// <summary>
        /// String serialization
        /// </summary>
        /// <param name="value"></param>
        public unsafe void BinarySerialize(SubString value)
        {
            if (value.Length != 0)
            {
                fixed (char* stringFixed = value.GetFixedBuffer()) Stream.Serialize(stringFixed + value.Start, value.Length);
            }
            else Stream.Write(0);
        }
        /// <summary>
        /// String serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, SubString value)
        {
            binarySerializer.BinarySerialize(value);
        }
        /// <summary>
        /// Type serialization
        /// </summary>
        /// <param name="value"></param>
        private void primitiveSerializeOnly(Type value)
        {
            Stream.Write(RealTypeValue);
            AutoCSer.Reflection.RemoteType remoteType = value;
            BinarySerialize(remoteType.AssemblyName);
            BinarySerialize(remoteType.Name);
        }
        /// <summary>
        /// Type serialization
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public void BinarySerialize(Type? value)
#else
        public void BinarySerialize(Type value)
#endif
        {
            if (value != null)
            {
                switch (CheckDepth(AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        primitiveSerializeOnly(value);
                        ++CurrentDepth;
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        if (CheckPoint(value)) primitiveSerializeOnly(value);
                        ++CurrentDepth;
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// Type serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveSerialize(BinarySerializer binarySerializer, Type? value)
#else
        private static void primitiveSerialize(BinarySerializer binarySerializer, Type value)
#endif
        {
            binarySerializer.BinarySerialize(value);
        }
        /// <summary>
        /// object serialization
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public void BinarySerialize(object? value)
#else
        public void BinarySerialize(object value)
#endif
        {
            if (value != null)
            {
                switch (CheckDepth(AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference))
                {
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.DepthCount:
                        ++CurrentDepth;
                        Stream.Write(NotNullValue);
                        return;
                    case AutoCSer.BinarySerialize.SerializePushTypeEnum.TryReference:
                        ++CurrentDepth;
                        if (CheckPoint(value)) Stream.Write(NotNullValue);
                        return;
                }
            }
            Stream.Write(NullValue);
        }
        /// <summary>
        /// object serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveSerialize(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.BinarySerialize(value);
        }

        /// <summary>
        /// Get and set the custom serialization member bitmap
        /// </summary>
        /// <param name="memberMap">The custom serialization member bitmap set</param>
        /// <returns>Serialization member bitmap</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public BinarySerializeMemberMap GetCustomMemberMap(MemberMap? memberMap)
#else
        public BinarySerializeMemberMap GetCustomMemberMap(MemberMap memberMap)
#endif
        {
            var oldMemberMap = MemberMap;
            MemberMap = memberMap;
            return new BinarySerializeMemberMap(oldMemberMap, CurrentMemberMap, JsonMemberMap);
        }
        /// <summary>
        /// Restore the custom serialization member bitmap
        /// 恢复自定义序列化成员位图
        /// </summary>
        /// <param name="memberMap">Serialization member bitmap</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetCustomMemberMap(ref BinarySerializeMemberMap memberMap)
        {
            MemberMap = memberMap.MemberMap;
            CurrentMemberMap = memberMap.CurrentMemberMap;
            JsonMemberMap = memberMap.JsonMemberMap;
        }
        /// <summary>
        /// Custom serialization calls
        /// 自定义序列化调用
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
            if (value != null) TypeSerializer<T>.Serialize(this, ref value);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// Custom serialization calls
        /// 自定义序列化调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void CustomSerialize<T>(ref T value)
        {
            if (value != null) TypeSerializer<T>.Serialize(this, ref value);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// Serialize into a data buffer (write directly without checking the object reference)
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="buffer"></param>
        public unsafe void SerializeBuffer(ref SubArray<byte> buffer)
        {
            if (buffer.Length != 0)
            {
                fixed (byte* dataFixed = buffer.GetFixedBuffer()) Stream.Serialize(dataFixed + buffer.Start, buffer.Length, buffer.Length);
            }
            else Stream.Write(0);
        }
        /// <summary>
        /// Serialize into a data buffer (write directly without checking the object reference)
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="buffer"></param>
        public unsafe void SerializeBuffer(ref ByteArrayBuffer buffer)
        {
            if (buffer.CurrentIndex != 0)
            {
                fixed (byte* dataFixed = buffer.GetFixedBuffer()) Stream.Serialize(dataFixed + buffer.StartIndex, buffer.CurrentIndex, buffer.CurrentIndex);
            }
            else Stream.Write(0);
        }
        /// <summary>
        /// Serialize into a data buffer (write directly without checking the object reference)
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Excluding the number of bytes for padding, 0 indicates a serialization failure
        /// 不包括补白的字节数，0 表示序列化失败</returns>
#if NetStandard21
        public void SerializeBuffer(string? value)
#else
        public void SerializeBuffer(string value)
#endif
        {
            int index = SerializeBufferStart();
            if (index >= 0) SerializeBufferEnd(index, SerializeOnly(value));
        }
        /// <summary>
        /// Serialize into a data buffer (write directly without checking the object reference)
        /// 序列化为数据缓冲区（不检查对象引用直接写入）
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Excluding the number of bytes for padding, 0 indicates a serialization failure
        /// 不包括补白的字节数，0 表示序列化失败</returns>
#if NetStandard21
        internal unsafe int SerializeOnly(string? value)
#else
        internal unsafe int SerializeOnly(string value)
#endif
        {
            if (value != null)
            {
                if (value.Length == 0)
                {
                    Stream.Write(0);
                    return sizeof(int);
                }
                fixed (char* stringFixed = value) return Stream.Serialize(stringFixed, value.Length);
            }
            Stream.Write(NullValue);
            return sizeof(int);
        }
        /// <summary>
        /// The custom serialization data buffer begins processing
        /// 自定义序列化数据缓冲区开始处理
        /// </summary>
        /// <returns>Starting position. Return -1 if failed
        /// 起始位置，失败返回-1</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int SerializeBufferStart()
        {
            int index = Stream.Data.Pointer.CurrentIndex;
            return Stream.MoveSize(sizeof(int)) ? index : -1;
        }
        /// <summary>
        /// The custom serialization data buffer has completed processing
        /// 自定义序列化数据缓冲区结束处理
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        internal unsafe void SerializeBufferEnd(int index, int size)
        {
            int dataSize = Stream.Data.Pointer.CurrentIndex - (index + sizeof(int));
            if ((uint)(dataSize - size) < 4) dataSize = size;
            else if (!Stream.IsResizeError) Warning |= SerializeWarningEnum.BufferSize;
            *(int*)(Stream.Data.Pointer.Byte + index) = dataSize;
        }
        ///// <summary>
        ///// The custom serialization data buffer has completed processing
        ///// 自定义序列化数据缓冲区结束处理
        ///// </summary>
        ///// <param name="index"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal unsafe void SerializeBufferEnd(int index)
        //{
        //    *(int*)(Stream.Data.Pointer.Byte + index) = Stream.Data.Pointer.CurrentIndex - (index + sizeof(int));
        //}
        ///// <summary>
        ///// 自定义序列化数据缓冲区
        ///// </summary>
        ///// <param name="serializer">返回不包括补白的字节数</param>
        //public unsafe void SerializeBuffer(Func<BinarySerializer, int> serializer)
        //{
        //    int index = Stream.Data.Pointer.CurrentIndex;
        //    if (Stream.MoveSize(sizeof(int)))
        //    {
        //        int size = serializer(this), dataSize = Stream.Data.Pointer.CurrentIndex - (index + sizeof(int));
        //        if ((uint)(dataSize - size) < 4) dataSize = size;
        //        else if (!Stream.IsResizeError) Warning |= SerializeWarningEnum.BufferSize;
        //        *(int*)(Stream.Data.Pointer.Byte + index) = dataSize;
        //    }
        //}
        /// <summary>
        /// Independent objects are serialization into a piece of data that can be deserialization independently
        /// 独立对象序列化为一个可独立反序列化的数据（需确定为非简单序列化类型）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型（需确定为非简单序列化类型）</typeparam>
        /// <param name="value">Data object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal unsafe void IndependentSerialize<T>(ref T value)
            where T : struct
        {
            notReferenceCount = TypeSerializer<T>.SerializeDelegateReference.NotReferenceCount;
            points?.ClearCount();
            CurrentDepth = Config.CheckDepth;
            isObjectReference = false;

            streamStartIndex = Config.WriteHeader(Stream);
            TypeSerializer<T>.SerializeCommandServer(this, ref value);
            if (!isObjectReference) Stream.Write(Stream.Data.Pointer.CurrentIndex - streamStartIndex);
            else
            {
                writePoints();
                Stream.Write(Stream.Data.Pointer.CurrentIndex - streamStartIndex);
                *(int*)(Stream.Data.Pointer.Byte + streamStartIndex) |= BinarySerializeConfig.ObjectReference;
            }
        }
        ///// <summary>
        ///// The internal member object is serialization into an independently deserializable piece of data (with no reference check on the outer layer)
        ///// 内部成员对象序列化为一个可独立反序列化的数据（外层无引用检查）
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="value"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal unsafe void InternalIndependentSerializeNotReference<T>(ref T value)
        //{
        //    if (value != null) InternalIndependentSerializeNotNull(ref value);
        //    else Stream.Write(((long)NullValue << 32) + sizeof(int));
        //}
        /// <summary>
        /// The internal member object is serialization into an independently deserializable data (with no reference check on the outer layer) (used for AOT code generation and not allowed for developers to call)
        /// 内部成员对象序列化为一个可独立反序列化的数据（外层无引用检查）（用于 AOT 代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T">需确定为非简单序列化类型</typeparam>
        /// <param name="value"></param>
#if AOT
        public unsafe void InternalIndependentSerializeNotNull<T>(ref T value)
#else
        internal unsafe void InternalIndependentSerializeNotNull<T>(ref T value)
#endif
        {
            int index = Stream.GetMoveSize(sizeof(int));
            if (index != 0)
            {
                int streamStartIndex = this.streamStartIndex;
                notReferenceCount = TypeSerializer<T>.SerializeDelegateReference.NotReferenceCount;
                isObjectReference = false;
                this.streamStartIndex = Config.WriteHeader(Stream);
                TypeSerializer<T>.SerializeCommandServer(this, ref value);
                if (!isObjectReference) Stream.Write(Stream.Data.Pointer.CurrentIndex - this.streamStartIndex);
                else
                {
                    writePoints();
                    Stream.Write(Stream.Data.Pointer.CurrentIndex - this.streamStartIndex);
                    *(int*)(Stream.Data.Pointer.Byte + this.streamStartIndex) |= BinarySerializeConfig.ObjectReference;
                }
                this.streamStartIndex = streamStartIndex;
                notReferenceCount = true;
                points?.ClearCount();
                Stream.Data.Pointer.WriteSizeData(index);
            }
        }
        ///// <summary>
        ///// 内部成员对象序列化为一个可独立反序列化的数据
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="value"></param>
        //public unsafe void InternalIndependentSerialize<T>(ref T value)
        //{
        //    if (value != null)
        //    {
        //        int index = Stream.GetMoveSize(sizeof(int));
        //        if (index != 0)
        //        {
        //            BinarySerializer serializer = YieldPool.Default.Pop() ?? new BinarySerializer();
        //            try
        //            {
        //                serializer.SetContext(Context);
        //                serializer.serialize(this, ref value);
        //            }
        //            finally { serializer.FreeContext(); }
        //            *(int*)(Stream.Data.Pointer.Byte + (index - sizeof(int))) = Stream.Data.Pointer.CurrentIndex - index;
        //        }
        //    }
        //    else Stream.Write(((long)NullValue << 32) + sizeof(int));
        //}
        /// <summary>
        /// The internal member object is serialization into an independently deserializable data (for AOT code generation and not allowed for developers to call)
        /// 内部成员对象序列化为一个可独立反序列化的数据（用于 AOT 代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
#if AOT
        public unsafe void SimpleSerialize<T>(ref T value) where T : struct
#else
        internal unsafe void SimpleSerialize<T>(ref T value) where T : struct
#endif
        {
            int index = Stream.GetMoveSize(sizeof(int));
            if (index != 0)
            {
                AutoCSer.SimpleSerialize.Serializer<T>.DefaultSerializer(Stream, ref value);
                Stream.Data.Pointer.WriteSizeData(index);
                //*(int*)(Stream.Data.Pointer.Byte + (index - sizeof(int))) = Stream.Data.Pointer.CurrentIndex - index;
            }
        }
        /// <summary>
        /// Recalculate the serialization byte length (4-byte alignment)
        /// 重新计算序列化字节长度（4字节对齐）
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static long GetSize4(long size)
        {
            return (size + 3) & (long.MaxValue - 3);
        }
        /// <summary>
        /// Recalculate the serialization byte length (4-byte alignment)
        /// 重新计算序列化字节长度（4字节对齐）
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int GetSize4(int size)
        {
            return (size + 3) & (int.MaxValue - 3);
        }
        /// <summary>
        /// Recalculate the serialization byte length (4-byte alignment)
        /// 重新计算序列化字节长度（4字节对齐）
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static long GetSize(long size)
        {
            return size;
        }
        /// <summary>
        /// Recalculate the serialization byte length (4-byte alignment)
        /// 重新计算序列化字节长度（4字节对齐）
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int GetSize(int size)
        {
            return size;
        }
        /// <summary>
        /// Serialize and fill the blank bytes
        /// 序列化填充空白字节
        /// </summary>
        /// <param name="write"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal unsafe void FillSize(byte* write, int size) { }
        /// <summary>
        /// short/ushort serialization padding blank bytes (4-byte alignment)
        /// short / ushort 序列化填充空白字节（4字节对齐）
        /// </summary>
        /// <param name="write"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal unsafe void FillSize2(byte* write, int size)
        {
            if ((size & 1) != 0) *(short*)write = 0;
        }
        /// <summary>
        /// byte/sbyte serialization fills blank bytes (4-byte alignment)
        /// byte / sbyte 序列化填充空白字节（4字节对齐）
        /// </summary>
        /// <param name="write"></param>
        /// <param name="size"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal unsafe void FillSize4(byte* write, int size)
        {
            switch (size & 3)
            {
                case 1:
                    *write = 0;
                    *(short*)(write + 1) = 0;
                    return;
                case 2: *(short*)write = 0; return;
                case 3: *write = 0; return;
            }
        }
        /// <summary>
        /// Write as much data as possible into the serialization output buffer
        /// 在序列化输出缓存区尽可能写入多的数据
        /// </summary>
        /// <param name="data">Output data</param>
        /// <param name="startIndex">Output the starting position of the data
        /// 输出数据起始位置</param>
        /// <param name="count">The maximum number of bytes written
        /// 最大写入字节数量</param>
        /// <returns>Write the number of bytes. Return -1 in case of write failure
        /// 写入字节数量，写入失败返回 -1</returns>
        internal unsafe int CustomWriteFree(byte[] data, int startIndex, int count)
        {
            int size = Stream.FreeSize - sizeof(int) * 2;
            if (isObjectReference)
            {
                size -= sizeof(int);
                foreach (int point in points.notNull().Values)
                {
                    if (point < 0) size -= sizeof(int);
                }
            }
            if (size > 0)
            {
                if (size > count) size = count;
                byte* write = Stream.Data.Pointer.GetBeforeMove((size + (3 + sizeof(int))) & (int.MaxValue - 3));
                *(int*)write = size;
                AutoCSer.Common.CopyTo(data, startIndex, write + sizeof(int), size);
                //Stream.Data.Pointer.SerializeFillLeftByteSize(size);
                return size;
            }
            Stream.SetResizeError();
            return -1;
        }
        /// <summary>
        /// Gets the serialization buffer move position
        /// 获取序列化缓冲区移动位置
        /// </summary>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public MoveSize GetBufferMoveSize()
        {
            return new MoveSize(Stream, sizeof(int));
        }
#if AOT
        /// <summary>
        /// Serialization of logical value members
        /// 逻辑值成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue">Logical value</param>
        private static void primitiveMemberSerializeBool(BinarySerializer binarySerializer, object objectValue)
        {
            bool? value = (bool?)objectValue;
            binarySerializer.Stream.Data.Pointer.Write(value.HasValue ? (value.Value ? (byte)2 : (byte)1) : (byte)0);
        }
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        private static void primitiveMemberSerializeByte(BinarySerializer binarySerializer, object objectValue)
        {
            byte? value = (byte?)objectValue;
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.Write((ushort)value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(short.MinValue);
        }
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        private static void primitiveMemberSerializeSByte(BinarySerializer binarySerializer, object objectValue)
        {
            sbyte? value = (sbyte?)objectValue;
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.Write((ushort)(byte)value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(short.MinValue);
        }
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        private static void primitiveMemberSerializeShort(BinarySerializer binarySerializer, object objectValue)
        {
            short? value = (short?)objectValue;
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.Write((uint)(ushort)value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        private static void primitiveMemberSerializeUShort(BinarySerializer binarySerializer, object objectValue)
        {
            ushort? value = (ushort?)objectValue;
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.Write((uint)value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
        /// <summary>
        /// Serialization of character members
        /// 字符成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        private static void primitiveMemberSerializeChar(BinarySerializer binarySerializer, object objectValue)
        {
            char? value = (char?)objectValue;
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.Write((uint)value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
        /// <summary>
        /// Guid member serialization
        /// Guid 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        private static void primitiveMemberSerializeGuid(BinarySerializer binarySerializer, object objectValue)
        {
            binarySerializer.BinarySerialize((Guid)objectValue);
        }
        /// <summary>
        /// Guid member serialization
        /// Guid 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        private static void primitiveMemberSerializeNullableGuid(BinarySerializer binarySerializer, object objectValue)
        {
            Guid? value = (Guid?)objectValue;
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
        /// <summary>
        /// String member serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeString(BinarySerializer binarySerializer, object? value)
        {
            binarySerializer.BinarySerialize((string?)value);
        }
        /// <summary>
        /// String member serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeSubString(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.BinarySerialize((SubString)value);
        }
        /// <summary>
        /// Type member serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeType(BinarySerializer binarySerializer, object? value)
        {
            binarySerializer.BinarySerialize((Type?)value);
        }
        /// <summary>
        /// String member serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        private static void primitiveMemberSerializeObject(BinarySerializer binarySerializer, object? value)
        {
            binarySerializer.BinarySerialize(value);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void NullableListArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct
        {
            binarySerializer.NullableArray((ListArray<T?>?)array);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue">Array object</param>
        public static void NullableLeftArrayReflection<T>(BinarySerializer binarySerializer, object objectValue) where T : struct
        {
            LeftArray<T?> array = (LeftArray<T?>)objectValue;
            binarySerializer.nullableArrayOnly(array.Array ?? EmptyArray<T?>.Array, array.Length);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Array object</param>
        public static void StructListArrayReflection<T>(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.structArray((ListArray<T>?)array);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue">Array object</param>
        public static void StructLeftArrayReflection<T>(BinarySerializer binarySerializer, object objectValue)
        {
            LeftArray<T> array = (LeftArray<T>)objectValue;
            binarySerializer.structArrayOnly(array.Array ?? EmptyArray<T>.Array, array.Length);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Array object</param>
        public static void ListArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : class
        {
            binarySerializer.BinarySerialize((ListArray<T>?)array);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue">Array object</param>
        public static void LeftArrayReflection<T>(BinarySerializer binarySerializer, object objectValue) where T : class
        {
            LeftArray<T> array = (LeftArray<T>)objectValue;
            binarySerializer.arrayOnly(array.Array ?? EmptyArray<T>.Array, array.Length);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Array object</param>
        public static void NullableArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : struct
        {
            binarySerializer.NullableArray((T?[]?)array);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Array object</param>
        public static void StructArrayReflection<T>(BinarySerializer binarySerializer, object? array)
        {
            binarySerializer.structArray((T[]?)array);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="array">Array object</param>
        public static void ArrayReflection<T>(BinarySerializer binarySerializer, object? array) where T : class
        {
            binarySerializer.BinarySerialize((T[]?)array);
        }
        /// <summary>
        /// Custom serialization does not support types
        /// 自定义序列化不支持的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        public static void NotSupportReflection<T>(BinarySerializer binarySerializer, object value)
        {
            NotSupport(binarySerializer, (T)value);
        }
        /// <summary>
        /// Object serialization
        /// </summary>
        /// <param name="binarySerializer">Binary data serialization</param>
        /// <param name="value">Data object</param>
        public static void NullableReflection<T>(BinarySerializer binarySerializer, object value) where T : struct
        {
            Nullable(binarySerializer, (T?)value);
        }
        /// <summary>
        /// Dictionary serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="dictionary"></param>
        public static void DictionaryReflection<T, KT, VT>(BinarySerializer binarySerializer, object dictionary)
            where T : IDictionary<KT, VT>
        {
            binarySerializer.Dictionary<T, KT, VT>((T)dictionary);
        }
        /// <summary>
        /// Collection serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="collection">Collection object</param>
        public static void CollectionReflection<T, VT>(BinarySerializer binarySerializer, object collection) where T : ICollection<VT?>
        {
            binarySerializer.Collection<T, VT>((T)collection);
        }
        /// <summary>
        /// Base type serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="BT"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        public static void BaseReflection<T, BT>(BinarySerializer binarySerializer, object value)
            where T : class, BT
        {
            binarySerializer.baseSerialize<T, BT>((T)value);
        }
        /// <summary>
        /// JSON mixed binary serialization
        /// JSON 混杂二进制序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        public static void JsonReflection<T>(BinarySerializer binarySerializer, object value)
        {
            binarySerializer.Json((T)value);
        }
        /// <summary>
        /// JSON mixed binary serialization
        /// JSON 混杂二进制序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        public static void StructJsonReflection<T>(BinarySerializer binarySerializer, object objectValue)
        {
            T value = (T)objectValue;
            binarySerializer.JsonSerialize(ref value, null);
        }
        /// <summary>
        /// Simple serialization (for AOT code generation, not allowed for developers to call)
        /// 简单序列化（用于 AOT 代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="binarySerializer"></param>
        /// <param name="objectValue"></param>
        public static void SimpleReflection<T>(BinarySerializer binarySerializer, object objectValue) where T : struct
        {
            T value = (T)objectValue;
            binarySerializer.SimpleSerialize(ref value);
        }
        /// <summary>
        /// Serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value">Data object</param>
        public static void SerializeReflection<T>(BinarySerializer binarySerializer, object value)
        {
            Serialize(binarySerializer, (T)value);
        }
        /// <summary>
        /// Custom serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        public static void ICustomReflection<T>(AutoCSer.BinarySerializer serializer, object value) where T : ICustomSerialize<T>
        {
            ((T)value).Serialize(serializer);
        }
        /// <summary>
        /// Serialization of logical value members
        /// 逻辑值成员序列化
        /// </summary>
        /// <param name="value">Logical value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(bool? value)
        {
            Stream.Write(value.HasValue ? (value.Value ? (byte)2 : (byte)1) : (byte)0);
        }
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(byte? value)
        {
            if (value.HasValue) Stream.Write((ushort)value.Value);
            else Stream.Write(short.MinValue);
        }
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(sbyte? value)
        {
            if (value.HasValue) Stream.Write((ushort)(byte)value.Value);
            else Stream.Write(short.MinValue);
        }
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(short? value)
        {
            if (value.HasValue) Stream.Write((uint)(ushort)value.Value);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(ushort? value)
        {
            if (value.HasValue) Stream.Write((uint)value.Value);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(char? value)
        {
            if (value.HasValue) Stream.Write((uint)value.Value);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// Guid member serialization
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize(Guid value)
        {
            Stream.Write(ref value);
        }
        /// <summary>
        /// Guid member serialization
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public unsafe void BinarySerialize(Guid? value)
        {
            if (value.HasValue)
            {
                if (Stream.PrepSize(sizeof(Guid) + sizeof(int))) Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            }
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void NullableArray<T>(LeftArray<T?> array) where T : struct
        {
            nullableArrayOnly(array.Array ?? EmptyArray<T?>.Array, array.Length);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void StructArray<T>(LeftArray<T> array) where T : struct
        {
            structArrayOnly(array.Array ?? EmptyArray<T>.Array, array.Length);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize<T>(LeftArray<T> array) where T : class
        {
            arrayOnly(array.Array ?? EmptyArray<T>.Array, array.Length);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void StructArray<T>(ListArray<T> array) where T : struct
        {
            structArray(array);
        }
        /// <summary>
        /// Array serialization
        /// </summary>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void StructArray<T>(T[] array) where T : struct
        {
            structArray(array);
        }
        /// <summary>
        /// Object serialization
        /// </summary>
        /// <param name="value">Data object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize<T>(T? value) where T : struct
        {
            if (value.HasValue) TypeSerializer<T>.SerializeNullable(this, value.Value);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// Serialization
        /// </summary>
        /// <param name="value">Data object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinarySerialize<T>(T value)
        {
            if (value != null) TypeSerializer<T>.Serialize(this, ref value);
            else Stream.Write(NullValue);
        }
        /// <summary>
        /// JSON serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void StructJson<T>(T value) where T : struct
        {
            JsonSerialize(ref value, null);
        }
        /// <summary>
        /// Binary serialization to simple serialization (for AOT code generation, not allowed for developers to call)
        /// 二进制序列化转简单序列化（用于 AOT 代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Simple<T>(T value) where T : struct
        {
            SimpleSerialize(ref value);
        }
        /// <summary>
        /// Custom serialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo ICustomMethod;
        /// <summary>
        /// Base type serialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo BaseMethod;
        /// <summary>
        /// Unsupported type serialization
        /// 不支持类型序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NotSupportMethod;
        /// <summary>
        /// Nullable array serialization
        /// 可空数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableArrayMethod;
        /// <summary>
        /// Nullable array serialization
        /// 可空数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableLeftArrayMethod;
        /// <summary>
        /// Nullable array serialization
        /// 可空数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableListArrayMethod;
        /// <summary>
        /// Serialization of value type arrays
        /// 值类型数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo StructArrayMethod;
        /// <summary>
        /// Serialization of value type arrays
        /// 值类型数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo StructLeftArrayMethod;
        /// <summary>
        /// Serialization of value type arrays
        /// 值类型数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo StructListArrayMethod;
        /// <summary>
        /// Serialization of reference type arrays
        /// 引用类型数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo ArrayMethod;
        /// <summary>
        /// Serialization of reference type arrays
        /// 引用类型数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo LeftArrayMethod;
        /// <summary>
        /// Serialization of reference type arrays
        /// 引用类型数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo ListArrayMethod;
        /// <summary>
        /// Nullable data serialization
        /// 可空数据序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableMethod;
        /// <summary>
        /// Dictionary serialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo DictionaryMethod;
        /// <summary>
        /// Collection serialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo CollectionMethod;
        /// <summary>
        /// JSON serialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo JsonMethod;
        /// <summary>
        /// JSON serialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo StructJsonMethod;
        /// <summary>
        /// Simple data serialization
        /// 简单数据序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo SimpleMethod;
        /// <summary>
        /// Custom serialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo ICustomReflectionMethod;
        /// <summary>
        /// Base type serialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo BaseReflectionMethod;
        /// <summary>
        /// Unsupported type serialization
        /// 不支持的类型序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NotSupportReflectionMethod;
        /// <summary>
        /// Nullable array serialization
        /// 可空数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableArrayReflectionMethod;
        /// <summary>
        /// Nullable array serialization
        /// 可空数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableLeftArrayReflectionMethod;
        /// <summary>
        /// Nullable array serialization
        /// 可空数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableListArrayReflectionMethod;
        /// <summary>
        /// Serialization of value type arrays
        /// 值类型数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo StructArrayReflectionMethod;
        /// <summary>
        /// Serialization of value type arrays
        /// 值类型数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo StructLeftArrayReflectionMethod;
        /// <summary>
        /// Serialization of value type arrays
        /// 值类型数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo StructListArrayReflectionMethod;
        /// <summary>
        /// Serialization of reference type arrays
        /// 引用类型数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo ArrayReflectionMethod;
        /// <summary>
        /// Serialization of reference type arrays
        /// 引用类型数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo LeftArrayReflectionMethod;
        /// <summary>
        /// Serialization of reference type arrays
        /// 引用类型数组序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo ListArrayReflectionMethod;
        /// <summary>
        /// Nullable data serialization
        /// 可空数据序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableReflectionMethod;
        /// <summary>
        /// Dictionary serialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo DictionaryReflectionMethod;
        /// <summary>
        /// Collection serialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo CollectionReflectionMethod;
        /// <summary>
        /// JSON serialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo JsonReflectionMethod;
        /// <summary>
        /// JSON serialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo StructJsonReflectionMethod;
        /// <summary>
        /// Simple data serialization
        /// 简单数据序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo SimpleReflectionMethod;
        /// <summary>
        /// Serialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo SerializeReflectionMethod;
        /// <summary>
        /// object reflection serialization
        /// object 反射序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo RealTypeObjectMethod;
        /// <summary>
        /// AOT serialization template invocation
        /// AOT 序列化模板调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        internal void SerializeMethodName<T>(object value) { }
        /// <summary>
        /// AOT serialization template invocation
        /// AOT 序列化模板调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        internal static void ReflectionMethodName<T>(params object[] values) { }
#else
        /// <summary>
        /// Serialization of logical value members
        /// 逻辑值成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value">Logical value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, bool? value)
        {
            binarySerializer.Stream.Data.Pointer.Write(value.HasValue ? (value.Value ? (byte)2 : (byte)1) : (byte)0);
        }
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, byte? value)
        {
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.Write((ushort)value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(short.MinValue);
        }
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, sbyte? value)
        {
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.Write((ushort)(byte)value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(short.MinValue);
        }
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, short? value)
        {
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.Write((uint)(ushort)value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, ushort? value)
        {
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.Write((uint)value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
        /// <summary>
        /// Integer member serialization
        /// 整数成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, char? value)
        {
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.Write((uint)value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
        /// <summary>
        /// Guid member serialization
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberSerialize(BinarySerializer binarySerializer, Guid? value)
        {
            if (value.HasValue) binarySerializer.Stream.Data.Pointer.SerializeWriteNullable(value.Value);
            else binarySerializer.Stream.Data.Pointer.Write(NullValue);
        }
        /// <summary>
        /// Fill the blank bytes
        /// 填充空白字节
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="fixedFillSize"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void FixedFillSize(BinarySerializer serializer, int fixedFillSize)
        {
            serializer.FixedFillSize(fixedFillSize);
        }
        /// <summary>
        /// Gets the current stream location
        /// 获取当前流位置
        /// </summary>
        /// <param name="serializer"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int GetStreamCurrentIndex(BinarySerializer serializer)
        {
            return serializer.Stream.Data.Pointer.CurrentIndex;
        }
        /// <summary>
        /// Align the blank space by 4 bytes
        /// 补白对齐 4 字节
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="startIndex"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SerializeFill(BinarySerializer serializer, int startIndex)
        {
            serializer.SerializeFill(startIndex);
        }
        /// <summary>
        /// Get the JSON member bitmap
        /// 获取 JSON 成员位图
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberMap"></param>
        /// <param name="memberIndexs"></param>
        /// <returns></returns>
#if NetStandard21
        internal unsafe MemberMap<T>? GetJsonMemberMap<T>(MemberMap<T> memberMap, ref Pointer memberIndexs)
#else
        internal unsafe MemberMap<T> GetJsonMemberMap<T>(MemberMap<T> memberMap, ref Pointer memberIndexs)
#endif
        {
            int count = 0;
            var jsonMemberMap = default(MemberMap<T>);
            int* memberIndex = memberIndexs.Int, memberIndexEnd = (int*)memberIndexs.End;
            do
            {
                if (memberMap.MemberMapData.IsMember(*memberIndex))
                {
                    if (jsonMemberMap == null)
                    {
                        if (JsonMemberMap == null) JsonMemberMap = jsonMemberMap = new MemberMap<T>();
                        else jsonMemberMap = (MemberMap<T>)JsonMemberMap;
                    }
                    jsonMemberMap.MemberMapData.SetMember(*memberIndex);
                    ++count;
                }
            }
            while (++memberIndex != memberIndexEnd);
            return count == 0 ? null : jsonMemberMap;
        }
#endif
        /// <summary>
        /// Align the blank space by 4 bytes (for AOT code generation and not allowed for developers to call)
        /// 补白对齐 4 字节（用于 AOT 代码生成，不允许开发者调用）
        /// </summary>
        /// <param name="startIndex"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public void SerializeFill(int startIndex)
#else
        internal void SerializeFill(int startIndex)
#endif
        {
            Stream.Data.Pointer.SerializeFillWithStartIndex(startIndex);
        }
        /// <summary>
        /// Fill the blank bytes (for AOT code generation and not allowed for developers to call)
        /// 填充空白字节（用于 AOT 代码生成，不允许开发者调用）
        /// </summary>
        /// <param name="fixedFillSize"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public void FixedFillSize(int fixedFillSize)
#else
        internal void FixedFillSize(int fixedFillSize)
#endif
        {
            Stream.Data.Pointer.SerializeFill(fixedFillSize);
        }
        /// <summary>
        /// Write the number of serialized members (for AOT code generation and not allowed for developers to call)
        /// 写入序列化成员数量（用于 AOT 代码生成，不允许开发者调用）
        /// </summary>
        /// <param name="prepSize"></param>
        /// <param name="memberCountVerify"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public bool WriteMemberCountVerify(int prepSize, int memberCountVerify)
#else
        internal bool WriteMemberCountVerify(int prepSize, int memberCountVerify)
#endif
        {
            if (Stream.PrepSize(prepSize))
            {
                Stream.Data.Pointer.Write(memberCountVerify);
                return true;
            }
            return false;
        }
        ///// <summary>
        ///// 整数数组序列化
        ///// </summary>
        ///// <param name="array"></param>
        //public unsafe void Serialize(SubArray<byte> array)
        //{
        //    if (array.Length == 0) Stream.Write(0);
        //    else
        //    {
        //        fixed (byte* arrayFixed = array.GetFixedBuffer()) serialize(arrayFixed + array.Start, array.Length, array.Length);
        //    }
        //}

        /// <summary>
        /// Serialization
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Serialized data</returns>
#if NetStandard21
        public static byte[] Serialize<T>(T? value, BinarySerializeConfig? config = null)
#else
        public static byte[] Serialize<T>(T value, BinarySerializeConfig config = null)
#endif
        {
            SerializeWarningEnum warning;
            return Serialize(ref value, out warning, config);
        }
        /// <summary>
        /// Serialization
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Serialized data</returns>
#if NetStandard21
        public static byte[] Serialize<T>(ref T? value, BinarySerializeConfig? config = null)
#else
        public static byte[] Serialize<T>(ref T value, BinarySerializeConfig config = null)
#endif
        {
            SerializeWarningEnum warning;
            return Serialize(ref value, out warning, config);
        }
        /// <summary>
        /// Serialization
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="warning">Warning prompt status
        /// 警告提示状态</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Serialized data</returns>
#if NetStandard21
        public static byte[] Serialize<T>(T? value, out SerializeWarningEnum warning, BinarySerializeConfig? config = null)
#else
        public static byte[] Serialize<T>(T value, out SerializeWarningEnum warning, BinarySerializeConfig config = null)
#endif
        {
            return Serialize(ref value, out warning, config);
        }
        /// <summary>
        /// Serialization
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="warning">Warning prompt status
        /// 警告提示状态</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Serialized data</returns>
#if NetStandard21
        public static byte[] Serialize<T>(ref T? value, out SerializeWarningEnum warning, BinarySerializeConfig? config = null)
#else
        public static byte[] Serialize<T>(ref T value, out SerializeWarningEnum warning, BinarySerializeConfig config = null)
#endif
        {
            if (value != null)
            {
                BinarySerializer serializer = AutoCSer.Threading.LinkPool<BinarySerializer>.Default.Pop() ?? new BinarySerializer();
                try
                {
                    return serializer.serialize(ref value, config, out warning);
                }
                finally { serializer.Free(); }
            }
            warning = SerializeWarningEnum.None;
            return BitConverter.GetBytes(NullValue);
        }

        /// <summary>
        /// Serialization (Thread static instance pattern)
        /// 序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Serialized data</returns>
#if NetStandard21
        public static byte[] ThreadStaticSerialize<T>(T? value, BinarySerializeConfig? config = null)
#else
        public static byte[] ThreadStaticSerialize<T>(T value, BinarySerializeConfig config = null)
#endif
        {
            SerializeWarningEnum warning;
            return ThreadStaticSerialize(ref value, out warning, config);
        }
        /// <summary>
        /// Serialization (Thread static instance pattern)
        /// 序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Serialized data</returns>
#if NetStandard21
        public static byte[] ThreadStaticSerialize<T>(ref T? value, BinarySerializeConfig? config = null)
#else
        public static byte[] ThreadStaticSerialize<T>(ref T value, BinarySerializeConfig config = null)
#endif
        {
            SerializeWarningEnum warning;
            return ThreadStaticSerialize(ref value, out warning, config);
        }
        /// <summary>
        /// Serialization (Thread static instance pattern)
        /// 序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="warning">Warning prompt status
        /// 警告提示状态</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Serialized data</returns>
#if NetStandard21
        public static byte[] ThreadStaticSerialize<T>(T? value, out SerializeWarningEnum warning, BinarySerializeConfig? config = null)
#else
        public static byte[] ThreadStaticSerialize<T>(T value, out SerializeWarningEnum warning, BinarySerializeConfig config = null)
#endif
        {
            return ThreadStaticSerialize(ref value, out warning, config);
        }
        /// <summary>
        /// Serialization (Thread static instance pattern)
        /// 序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Target data type
        /// 目标数据类型</typeparam>
        /// <param name="value">Data object</param>
        /// <param name="warning">Warning prompt status
        /// 警告提示状态</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Serialized data</returns>
#if NetStandard21
        public static byte[] ThreadStaticSerialize<T>(ref T? value, out SerializeWarningEnum warning, BinarySerializeConfig? config = null)
#else
        public static byte[] ThreadStaticSerialize<T>(ref T value, out SerializeWarningEnum warning, BinarySerializeConfig config = null)
#endif
        {
            if (value != null)
            {
                BinarySerializer serializer = ThreadStaticSerializer.Get().Serializer;
                try
                {
                    return serializer.serializeThreadStatic(ref value, config, out warning);
                }
                finally { serializer.freeThreadStatic(); }
            }
            warning = SerializeWarningEnum.None;
            return BitConverter.GetBytes(NullValue);
        }

        /// <summary>
        /// Basic type serialized delegate collection
        /// 基本类型序列化委托集合
        /// </summary>
        internal static readonly Dictionary<HashObject<System.Type>, SerializeDelegateReference> SerializeDelegates;
        static BinarySerializer()
        {
            SerializeDelegates = AutoCSer.DictionaryCreator.CreateHashObject<System.Type, SerializeDelegateReference>();
#if AOT
            SerializeDelegates.Add(typeof(bool), new SerializeDelegateReference((Action<BinarySerializer, bool>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberBoolReflection));
            SerializeDelegates.Add(typeof(bool?), new SerializeDelegateReference((Action<BinarySerializer, bool?>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeBool));
            SerializeDelegates.Add(typeof(bool[]), new SerializeDelegateReference((Action<BinarySerializer, bool[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeBoolArray, typeof(bool[])));
            SerializeDelegates.Add(typeof(bool?[]), new SerializeDelegateReference((Action<BinarySerializer, bool?[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeNullableBoolArray, typeof(bool?[])));
            SerializeDelegates.Add(typeof(LeftArray<bool>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<bool>>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeBoolLeftArray));
            SerializeDelegates.Add(typeof(ListArray<bool>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<bool>>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeBoolListArray, typeof(ListArray<bool>)));
            SerializeDelegates.Add(typeof(byte), new SerializeDelegateReference((Action<BinarySerializer, byte>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberByteReflection));
            SerializeDelegates.Add(typeof(byte?), new SerializeDelegateReference((Action<BinarySerializer, byte?>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeByte));
            SerializeDelegates.Add(typeof(byte[]), new SerializeDelegateReference((Action<BinarySerializer, byte[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeByteArray, typeof(byte[])));
            SerializeDelegates.Add(typeof(byte?[]), new SerializeDelegateReference((Action<BinarySerializer, byte?[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeNullableByteArray, typeof(byte?[])));
            SerializeDelegates.Add(typeof(LeftArray<byte>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<byte>>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeByteLeftArray));
            SerializeDelegates.Add(typeof(ListArray<byte>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<byte>>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeByteListArray, typeof(ListArray<byte>)));
            SerializeDelegates.Add(typeof(sbyte), new SerializeDelegateReference((Action<BinarySerializer, sbyte>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberSByteReflection));
            SerializeDelegates.Add(typeof(sbyte?), new SerializeDelegateReference((Action<BinarySerializer, sbyte?>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeSByte));
            SerializeDelegates.Add(typeof(sbyte[]), new SerializeDelegateReference((Action<BinarySerializer, sbyte[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeSByteArray, typeof(sbyte[])));
            SerializeDelegates.Add(typeof(sbyte?[]), new SerializeDelegateReference((Action<BinarySerializer, sbyte?[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeNullableSByteArray, typeof(sbyte?[])));
            SerializeDelegates.Add(typeof(LeftArray<sbyte>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<sbyte>>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeSByteLeftArray));
            SerializeDelegates.Add(typeof(ListArray<sbyte>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<sbyte>>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeSByteListArray, typeof(ListArray<sbyte>)));
            SerializeDelegates.Add(typeof(short), new SerializeDelegateReference((Action<BinarySerializer, short>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberShortReflection));
            SerializeDelegates.Add(typeof(short?), new SerializeDelegateReference((Action<BinarySerializer, short?>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeShort));
            SerializeDelegates.Add(typeof(short[]), new SerializeDelegateReference((Action<BinarySerializer, short[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeShortArray, typeof(short[])));
            SerializeDelegates.Add(typeof(short?[]), new SerializeDelegateReference((Action<BinarySerializer, short?[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeNullableShortArray, typeof(short?[])));
            SerializeDelegates.Add(typeof(LeftArray<short>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<short>>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeShortLeftArray));
            SerializeDelegates.Add(typeof(ListArray<short>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<short>>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeShortListArray, typeof(ListArray<short>)));
            SerializeDelegates.Add(typeof(ushort), new SerializeDelegateReference((Action<BinarySerializer, ushort>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberUShortReflection));
            SerializeDelegates.Add(typeof(ushort?), new SerializeDelegateReference((Action<BinarySerializer, ushort?>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeUShort));
            SerializeDelegates.Add(typeof(ushort[]), new SerializeDelegateReference((Action<BinarySerializer, ushort[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeUShortArray, typeof(ushort[])));
            SerializeDelegates.Add(typeof(ushort?[]), new SerializeDelegateReference((Action<BinarySerializer, ushort?[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeNullableUShortArray, typeof(ushort?[])));
            SerializeDelegates.Add(typeof(LeftArray<ushort>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<ushort>>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeUShortLeftArray));
            SerializeDelegates.Add(typeof(ListArray<ushort>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<ushort>>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeUShortListArray, typeof(ListArray<ushort>)));
            SerializeDelegates.Add(typeof(int), new SerializeDelegateReference((Action<BinarySerializer, int>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberIntReflection));
            SerializeDelegates.Add(typeof(int?), new SerializeDelegateReference((Action<BinarySerializer, int?>)Nullable<int>, (Action<BinarySerializer, object>)primitiveMemberSerializeInt));
            SerializeDelegates.Add(typeof(int[]), new SerializeDelegateReference((Action<BinarySerializer, int[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeIntArray, typeof(int[])));
            SerializeDelegates.Add(typeof(int?[]), new SerializeDelegateReference((Action<BinarySerializer, int?[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeNullableIntArray, typeof(int?[])));
            SerializeDelegates.Add(typeof(LeftArray<int>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<int>>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeIntLeftArray));
            SerializeDelegates.Add(typeof(ListArray<int>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<int>>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeIntListArray, typeof(ListArray<int>)));
            SerializeDelegates.Add(typeof(uint), new SerializeDelegateReference((Action<BinarySerializer, uint>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberUIntReflection));
            SerializeDelegates.Add(typeof(uint?), new SerializeDelegateReference((Action<BinarySerializer, uint?>)Nullable<uint>, (Action<BinarySerializer, object>)primitiveMemberSerializeUInt));
            SerializeDelegates.Add(typeof(uint[]), new SerializeDelegateReference((Action<BinarySerializer, uint[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeUIntArray, typeof(uint[])));
            SerializeDelegates.Add(typeof(uint?[]), new SerializeDelegateReference((Action<BinarySerializer, uint?[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeNullableUIntArray, typeof(uint?[])));
            SerializeDelegates.Add(typeof(LeftArray<uint>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<uint>>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeUIntLeftArray));
            SerializeDelegates.Add(typeof(ListArray<uint>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<uint>>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeUIntListArray, typeof(ListArray<uint>)));
            SerializeDelegates.Add(typeof(long), new SerializeDelegateReference((Action<BinarySerializer, long>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberLongReflection));
            SerializeDelegates.Add(typeof(long?), new SerializeDelegateReference((Action<BinarySerializer, long?>)Nullable<long>, (Action<BinarySerializer, object>)primitiveMemberSerializeLong));
            SerializeDelegates.Add(typeof(long[]), new SerializeDelegateReference((Action<BinarySerializer, long[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeLongArray, typeof(long[])));
            SerializeDelegates.Add(typeof(long?[]), new SerializeDelegateReference((Action<BinarySerializer, long?[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeNullableLongArray, typeof(long?[])));
            SerializeDelegates.Add(typeof(LeftArray<long>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<long>>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeLongLeftArray));
            SerializeDelegates.Add(typeof(ListArray<long>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<long>>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeLongListArray, typeof(ListArray<long>)));
            SerializeDelegates.Add(typeof(ulong), new SerializeDelegateReference((Action<BinarySerializer, ulong>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberULongReflection));
            SerializeDelegates.Add(typeof(ulong?), new SerializeDelegateReference((Action<BinarySerializer, ulong?>)Nullable<ulong>, (Action<BinarySerializer, object>)primitiveMemberSerializeULong));
            SerializeDelegates.Add(typeof(ulong[]), new SerializeDelegateReference((Action<BinarySerializer, ulong[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeULongArray, typeof(ulong[])));
            SerializeDelegates.Add(typeof(ulong?[]), new SerializeDelegateReference((Action<BinarySerializer, ulong?[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeNullableULongArray, typeof(ulong?[])));
            SerializeDelegates.Add(typeof(LeftArray<ulong>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<ulong>>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeULongLeftArray));
            SerializeDelegates.Add(typeof(ListArray<ulong>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<ulong>>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeULongListArray, typeof(ListArray<ulong>)));
            SerializeDelegates.Add(typeof(float), new SerializeDelegateReference((Action<BinarySerializer, float>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberFloatReflection));
            SerializeDelegates.Add(typeof(float?), new SerializeDelegateReference((Action<BinarySerializer, float?>)Nullable<float>, (Action<BinarySerializer, object>)primitiveMemberSerializeFloat));
            SerializeDelegates.Add(typeof(float[]), new SerializeDelegateReference((Action<BinarySerializer, float[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeFloatArray, typeof(float[])));
            SerializeDelegates.Add(typeof(float?[]), new SerializeDelegateReference((Action<BinarySerializer, float?[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeNullableFloatArray, typeof(float?[])));
            SerializeDelegates.Add(typeof(LeftArray<float>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<float>>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeFloatLeftArray));
            SerializeDelegates.Add(typeof(ListArray<float>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<float>>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeFloatListArray, typeof(ListArray<float>)));
            SerializeDelegates.Add(typeof(double), new SerializeDelegateReference((Action<BinarySerializer, double>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberDoubleReflection));
            SerializeDelegates.Add(typeof(double?), new SerializeDelegateReference((Action<BinarySerializer, double?>)Nullable<double>, (Action<BinarySerializer, object>)primitiveMemberSerializeDouble));
            SerializeDelegates.Add(typeof(double[]), new SerializeDelegateReference((Action<BinarySerializer, double[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeDoubleArray, typeof(double[])));
            SerializeDelegates.Add(typeof(double?[]), new SerializeDelegateReference((Action<BinarySerializer, double?[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeNullableDoubleArray, typeof(double?[])));
            SerializeDelegates.Add(typeof(LeftArray<double>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<double>>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeDoubleLeftArray));
            SerializeDelegates.Add(typeof(ListArray<double>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<double>>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeDoubleListArray, typeof(ListArray<double>)));
            SerializeDelegates.Add(typeof(decimal), new SerializeDelegateReference((Action<BinarySerializer, decimal>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberDecimalReflection));
            SerializeDelegates.Add(typeof(decimal?), new SerializeDelegateReference((Action<BinarySerializer, decimal?>)Nullable<decimal>, (Action<BinarySerializer, object>)primitiveMemberSerializeDecimal));
            SerializeDelegates.Add(typeof(decimal[]), new SerializeDelegateReference((Action<BinarySerializer, decimal[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeDecimalArray, typeof(decimal[])));
            SerializeDelegates.Add(typeof(decimal?[]), new SerializeDelegateReference((Action<BinarySerializer, decimal?[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeNullableDecimalArray, typeof(decimal?[])));
            SerializeDelegates.Add(typeof(LeftArray<decimal>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<decimal>>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeDecimalLeftArray));
            SerializeDelegates.Add(typeof(ListArray<decimal>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<decimal>>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeDecimalListArray, typeof(ListArray<decimal>)));
            SerializeDelegates.Add(typeof(char), new SerializeDelegateReference((Action<BinarySerializer, char>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberCharReflection));
            SerializeDelegates.Add(typeof(char?), new SerializeDelegateReference((Action<BinarySerializer, char?>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeChar));
            SerializeDelegates.Add(typeof(char[]), new SerializeDelegateReference((Action<BinarySerializer, char[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeCharArray, typeof(char[])));
            SerializeDelegates.Add(typeof(char?[]), new SerializeDelegateReference((Action<BinarySerializer, char?[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeNullableCharArray, typeof(char?[])));
            SerializeDelegates.Add(typeof(LeftArray<char>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<char>>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeCharLeftArray));
            SerializeDelegates.Add(typeof(ListArray<char>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<char>>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeCharListArray, typeof(ListArray<char>)));
            SerializeDelegates.Add(typeof(DateTime), new SerializeDelegateReference((Action<BinarySerializer, DateTime>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberDateTimeReflection));
            SerializeDelegates.Add(typeof(DateTime?), new SerializeDelegateReference((Action<BinarySerializer, DateTime?>)Nullable<DateTime>, (Action<BinarySerializer, object>)primitiveMemberSerializeDateTime));
            SerializeDelegates.Add(typeof(DateTime[]), new SerializeDelegateReference((Action<BinarySerializer, DateTime[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeDateTimeArray, typeof(DateTime[])));
            SerializeDelegates.Add(typeof(DateTime?[]), new SerializeDelegateReference((Action<BinarySerializer, DateTime?[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeNullableDateTimeArray, typeof(DateTime?[])));
            SerializeDelegates.Add(typeof(LeftArray<DateTime>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<DateTime>>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeDateTimeLeftArray));
            SerializeDelegates.Add(typeof(ListArray<DateTime>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<DateTime>>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeDateTimeListArray, typeof(ListArray<DateTime>)));
            SerializeDelegates.Add(typeof(TimeSpan), new SerializeDelegateReference((Action<BinarySerializer, TimeSpan>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberTimeSpanReflection));
            SerializeDelegates.Add(typeof(TimeSpan?), new SerializeDelegateReference((Action<BinarySerializer, TimeSpan?>)Nullable<TimeSpan>, (Action<BinarySerializer, object>)primitiveMemberSerializeTimeSpan));
            SerializeDelegates.Add(typeof(TimeSpan[]), new SerializeDelegateReference((Action<BinarySerializer, TimeSpan[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeTimeSpanArray, typeof(TimeSpan[])));
            SerializeDelegates.Add(typeof(TimeSpan?[]), new SerializeDelegateReference((Action<BinarySerializer, TimeSpan?[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeNullableTimeSpanArray, typeof(TimeSpan?[])));
            SerializeDelegates.Add(typeof(LeftArray<TimeSpan>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<TimeSpan>>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeTimeSpanLeftArray));
            SerializeDelegates.Add(typeof(ListArray<TimeSpan>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<TimeSpan>>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeTimeSpanListArray, typeof(ListArray<TimeSpan>)));
            SerializeDelegates.Add(typeof(Guid), new SerializeDelegateReference((Action<BinarySerializer, Guid>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeGuid));
            SerializeDelegates.Add(typeof(Guid?), new SerializeDelegateReference((Action<BinarySerializer, Guid?>)Nullable<Guid>, (Action<BinarySerializer, object>)primitiveMemberSerializeNullableGuid));
            SerializeDelegates.Add(typeof(Guid[]), new SerializeDelegateReference((Action<BinarySerializer, Guid[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeGuidArray, typeof(Guid[])));
            SerializeDelegates.Add(typeof(Guid?[]), new SerializeDelegateReference((Action<BinarySerializer, Guid?[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeNullableGuidArray, typeof(Guid?[])));
            SerializeDelegates.Add(typeof(LeftArray<Guid>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<Guid>>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeGuidLeftArray));
            SerializeDelegates.Add(typeof(ListArray<Guid>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<Guid>>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeGuidListArray, typeof(ListArray<Guid>)));
            SerializeDelegates.Add(typeof(string), new SerializeDelegateReference((Action<BinarySerializer, string>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeString, typeof(string)));
            SerializeDelegates.Add(typeof(SubString), new SerializeDelegateReference((Action<BinarySerializer, SubString>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeSubString));
            SerializeDelegates.Add(typeof(Type), new SerializeDelegateReference((Action<BinarySerializer, Type>)primitiveSerialize, (Action<BinarySerializer, object>)primitiveMemberSerializeType, typeof(Type)));
            SerializeDelegates.Add(typeof(object), new SerializeDelegateReference((Action<BinarySerializer, object>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeObject, typeof(object)));

            SerializeDelegates.Add(typeof(Half), new SerializeDelegateReference((Action<BinarySerializer, Half>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberHalfReflection));
            SerializeDelegates.Add(typeof(Half[]), new SerializeDelegateReference((Action<BinarySerializer, Half[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeHalfArray, typeof(Half[])));
            SerializeDelegates.Add(typeof(UInt128), new SerializeDelegateReference((Action<BinarySerializer, UInt128>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberULongReflection));
            SerializeDelegates.Add(typeof(UInt128[]), new SerializeDelegateReference((Action<BinarySerializer, UInt128[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeULongArray, typeof(UInt128[])));
            SerializeDelegates.Add(typeof(Int128), new SerializeDelegateReference((Action<BinarySerializer, Int128>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberULongReflection));
            SerializeDelegates.Add(typeof(Int128[]), new SerializeDelegateReference((Action<BinarySerializer, Int128[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeULongArray, typeof(Int128[])));
            SerializeDelegates.Add(typeof(System.Numerics.Complex), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Complex>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberULongReflection));
            SerializeDelegates.Add(typeof(System.Numerics.Complex[]), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Complex[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeULongArray, typeof(System.Numerics.Complex[])));
            SerializeDelegates.Add(typeof(System.Numerics.Plane), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Plane>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberULongReflection));
            SerializeDelegates.Add(typeof(System.Numerics.Plane[]), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Plane[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeULongArray, typeof(System.Numerics.Plane[])));
            SerializeDelegates.Add(typeof(System.Numerics.Quaternion), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Quaternion>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberULongReflection));
            SerializeDelegates.Add(typeof(System.Numerics.Quaternion[]), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Quaternion[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeULongArray, typeof(System.Numerics.Quaternion[])));
            SerializeDelegates.Add(typeof(System.Numerics.Matrix3x2), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Matrix3x2>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberULongReflection));
            SerializeDelegates.Add(typeof(System.Numerics.Matrix3x2[]), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Matrix3x2[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeULongArray, typeof(System.Numerics.Matrix3x2[])));
            SerializeDelegates.Add(typeof(System.Numerics.Matrix4x4), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Matrix4x4>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberULongReflection));
            SerializeDelegates.Add(typeof(System.Numerics.Matrix4x4[]), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Matrix4x4[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeULongArray, typeof(System.Numerics.Matrix4x4[])));
            SerializeDelegates.Add(typeof(System.Numerics.Vector2), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Vector2>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberULongReflection));
            SerializeDelegates.Add(typeof(System.Numerics.Vector2[]), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Vector2[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeULongArray, typeof(System.Numerics.Vector2[])));
            SerializeDelegates.Add(typeof(System.Numerics.Vector3), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Vector3>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberULongReflection));
            SerializeDelegates.Add(typeof(System.Numerics.Vector3[]), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Vector3[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeULongArray, typeof(System.Numerics.Vector3[])));
            SerializeDelegates.Add(typeof(System.Numerics.Vector4), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Vector4>)primitiveSerialize, (Action<BinarySerializer, object>)PrimitiveMemberULongReflection));
            SerializeDelegates.Add(typeof(System.Numerics.Vector4[]), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Vector4[]>)primitiveSerialize, (Action<BinarySerializer, object?>)primitiveMemberSerializeULongArray, typeof(System.Numerics.Vector4[])));

            SimpleMethod = SimpleReflectionMethod = PrimitiveMemberUShortReflectionMethod = PrimitiveMemberSByteReflectionMethod = PrimitiveMemberShortReflectionMethod = PrimitiveMemberULongReflectionMethod = EnumUShortListArrayReflectionMethod = EnumUShortLeftArrayReflectionMethod = PrimitiveMemberUIntReflectionMethod = PrimitiveMemberLongReflectionMethod = PrimitiveMemberByteReflectionMethod = EnumSByteListArrayReflectionMethod = EnumSByteLeftArrayReflectionMethod = EnumShortListArrayReflectionMethod = EnumShortLeftArrayReflectionMethod = EnumULongListArrayReflectionMethod = EnumULongLeftArrayReflectionMethod = PrimitiveMemberIntReflectionMethod = EnumLongListArrayReflectionMethod = EnumLongLeftArrayReflectionMethod = EnumUIntListArrayReflectionMethod = EnumUIntLeftArrayReflectionMethod = EnumByteListArrayReflectionMethod = EnumByteLeftArrayReflectionMethod = NullableListArrayReflectionMethod = NullableLeftArrayReflectionMethod = EnumIntListArrayReflectionMethod = EnumIntLeftArrayReflectionMethod = EnumUShortArrayReflectionMethod = StructListArrayReflectionMethod = StructLeftArrayReflectionMethod = EnumSByteArrayReflectionMethod = EnumShortArrayReflectionMethod = EnumULongArrayReflectionMethod = EnumLongArrayReflectionMethod = EnumUIntArrayReflectionMethod = EnumByteArrayReflectionMethod = NullableArrayReflectionMethod = EnumIntArrayReflectionMethod = StructArrayReflectionMethod = NotSupportReflectionMethod = StructJsonReflectionMethod = DictionaryReflectionMethod = CollectionReflectionMethod = EnumUShortListArrayMethod = EnumUShortLeftArrayMethod = ListArrayReflectionMethod = LeftArrayReflectionMethod = SerializeReflectionMethod = EnumSByteListArrayMethod = EnumSByteLeftArrayMethod = EnumShortListArrayMethod = EnumShortLeftArrayMethod = EnumULongListArrayMethod = EnumULongLeftArrayMethod = NullableReflectionMethod = EnumLongListArrayMethod = EnumLongLeftArrayMethod = EnumUIntListArrayMethod = EnumUIntLeftArrayMethod = EnumByteListArrayMethod = EnumByteLeftArrayMethod = NullableListArrayMethod = NullableLeftArrayMethod = ICustomReflectionMethod = EnumIntListArrayMethod = EnumIntLeftArrayMethod = EnumUShortArrayMethod = StructListArrayMethod = StructLeftArrayMethod = ArrayReflectionMethod = RealTypeObjectMethod = EnumSByteArrayMethod = EnumShortArrayMethod = EnumULongArrayMethod = JsonReflectionMethod = BaseReflectionMethod = EnumLongArrayMethod = EnumUIntArrayMethod = EnumByteArrayMethod = NullableArrayMethod = EnumIntArrayMethod = StructArrayMethod = NotSupportMethod = EnumUShortMethod = StructJsonMethod = DictionaryMethod = CollectionMethod = ListArrayMethod = LeftArrayMethod = EnumSByteMethod = EnumShortMethod = EnumULongMethod = NullableMethod = EnumUIntMethod = EnumLongMethod = EnumByteMethod = EnumIntMethod = ICustomMethod = ArrayMethod = JsonMethod = BaseMethod = AutoCSer.Common.NullMethodInfo;
            foreach (System.Reflection.MethodInfo method in typeof(BinarySerializer).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
            {
                switch (method.Name.Length)
                {
                    case 4:
                        if (method.Name == nameof(Base)) BaseMethod = method;
                        else if (method.Name == nameof(Json)) JsonMethod = method;
                        break;
                    case 5:
                        if (method.Name == nameof(Array)) ArrayMethod = method;
                        break;
                    case 6:
                        if (method.Name == nameof(Simple)) SimpleMethod = method;
                        break;
                    case 7:
                        if (method.Name == nameof(ICustom)) ICustomMethod = method;
                        else if (method.Name == nameof(EnumInt)) EnumIntMethod = method;
                        break;
                    case 8:
                        if (method.Name == nameof(EnumByte)) EnumByteMethod = method;
                        else if (method.Name == nameof(EnumLong)) EnumLongMethod = method;
                        else if (method.Name == nameof(EnumUInt)) EnumUIntMethod = method;
                        else if (method.Name == nameof(Nullable)) NullableMethod = method;
                        break;
                    case 9:
                        if (method.Name == nameof(EnumULong)) EnumULongMethod = method;
                        else if (method.Name == nameof(EnumShort)) EnumShortMethod = method;
                        else if (method.Name == nameof(EnumSByte)) EnumSByteMethod = method;
                        else if (method.Name == nameof(LeftArray)) LeftArrayMethod = method;
                        else if (method.Name == nameof(ListArray)) ListArrayMethod = method;
                        break;
                    case 10:
                        if (method.Name == nameof(Collection)) CollectionMethod = method;
                        else if (method.Name == nameof(Dictionary)) DictionaryMethod = method;
                        else if (method.Name == nameof(StructJson)) StructJsonMethod = method;
                        else if (method.Name == nameof(EnumUShort)) EnumUShortMethod = method;
                        else if (method.Name == nameof(NotSupport)) NotSupportMethod = method;
                        break;
                    case 11:
                        if (method.Name == nameof(StructArray)) StructArrayMethod = method;
                        break;
                    case 12:
                        if (method.Name == nameof(EnumIntArray)) EnumIntArrayMethod = method;
                        break;
                    case 13:
                        if (method.Name == nameof(NullableArray)) NullableArrayMethod = method;
                        else if (method.Name == nameof(EnumByteArray)) EnumByteArrayMethod = method;
                        else if (method.Name == nameof(EnumUIntArray)) EnumUIntArrayMethod = method;
                        else if (method.Name == nameof(EnumLongArray)) EnumLongArrayMethod = method;
                        break;
                    case 14:
                        if (method.Name == nameof(BaseReflection)) BaseReflectionMethod = method;
                        else if (method.Name == nameof(JsonReflection)) JsonReflectionMethod = method;
                        else if (method.Name == nameof(EnumULongArray)) EnumULongArrayMethod = method;
                        else if (method.Name == nameof(EnumShortArray)) EnumShortArrayMethod = method;
                        else if (method.Name == nameof(EnumSByteArray)) EnumSByteArrayMethod = method;
                        else if (method.Name == nameof(RealTypeObject)) RealTypeObjectMethod = method;
                        break;
                    case 15:
                        if (method.Name == nameof(ArrayReflection)) ArrayReflectionMethod = method;
                        else if (method.Name == nameof(StructLeftArray)) StructLeftArrayMethod = method;
                        else if (method.Name == nameof(StructListArray)) StructListArrayMethod = method;
                        else if (method.Name == nameof(EnumUShortArray)) EnumUShortArrayMethod = method;
                        break;
                    case 16:
                        if (method.Name == nameof(SimpleReflection)) SimpleReflectionMethod = method;
                        else if (method.Name == nameof(EnumIntLeftArray)) EnumIntLeftArrayMethod = method;
                        else if (method.Name == nameof(EnumIntListArray)) EnumIntListArrayMethod = method;
                        break;
                    case 17:
                        if (method.Name == nameof(ICustomReflection)) ICustomReflectionMethod = method;
                        else if (method.Name == nameof(NullableLeftArray)) NullableLeftArrayMethod = method;
                        else if (method.Name == nameof(NullableListArray)) NullableListArrayMethod = method;
                        else if (method.Name == nameof(EnumByteLeftArray)) EnumByteLeftArrayMethod = method;
                        else if (method.Name == nameof(EnumByteListArray)) EnumByteListArrayMethod = method;
                        else if (method.Name == nameof(EnumUIntLeftArray)) EnumUIntLeftArrayMethod = method;
                        else if (method.Name == nameof(EnumUIntListArray)) EnumUIntListArrayMethod = method;
                        else if (method.Name == nameof(EnumLongLeftArray)) EnumLongLeftArrayMethod = method;
                        else if (method.Name == nameof(EnumLongListArray)) EnumLongListArrayMethod = method;
                        break;
                    case 18:
                        if (method.Name == nameof(NullableReflection)) NullableReflectionMethod = method;
                        else if (method.Name == nameof(EnumULongLeftArray)) EnumULongLeftArrayMethod = method;
                        else if (method.Name == nameof(EnumULongListArray)) EnumULongListArrayMethod = method;
                        else if (method.Name == nameof(EnumShortLeftArray)) EnumShortLeftArrayMethod = method;
                        else if (method.Name == nameof(EnumShortListArray)) EnumShortListArrayMethod = method;
                        else if (method.Name == nameof(EnumSByteLeftArray)) EnumSByteLeftArrayMethod = method;
                        else if (method.Name == nameof(EnumSByteListArray)) EnumSByteListArrayMethod = method;
                        break;
                    case 19:
                        if (method.Name == nameof(SerializeReflection)) SerializeReflectionMethod = method;
                        else if (method.Name == nameof(LeftArrayReflection)) LeftArrayReflectionMethod = method;
                        else if (method.Name == nameof(ListArrayReflection)) ListArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumUShortLeftArray)) EnumUShortLeftArrayMethod = method;
                        else if (method.Name == nameof(EnumUShortListArray)) EnumUShortListArrayMethod = method;
                        break;
                    case 20:
                        if (method.Name == nameof(CollectionReflection)) CollectionReflectionMethod = method;
                        else if (method.Name == nameof(DictionaryReflection)) DictionaryReflectionMethod = method;
                        else if (method.Name == nameof(StructJsonReflection)) StructJsonReflectionMethod = method;
                        else if (method.Name == nameof(NotSupportReflection)) NotSupportReflectionMethod = method;
                        break;
                    case 21:
                        if (method.Name == nameof(StructArrayReflection)) StructArrayReflectionMethod = method;
                        break;
                    case 22:
                        if (method.Name == nameof(EnumIntArrayReflection)) EnumIntArrayReflectionMethod = method;
                        break;
                    case 23:
                        if (method.Name == nameof(NullableArrayReflection)) NullableArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumByteArrayReflection)) EnumByteArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumUIntArrayReflection)) EnumUIntArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumLongArrayReflection)) EnumLongArrayReflectionMethod = method;
                        break;
                    case 24:
                        if (method.Name == nameof(EnumULongArrayReflection)) EnumULongArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumShortArrayReflection)) EnumShortArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumSByteArrayReflection)) EnumSByteArrayReflectionMethod = method;
                        break;
                    case 25:
                        if (method.Name == nameof(StructLeftArrayReflection)) StructLeftArrayReflectionMethod = method;
                        else if (method.Name == nameof(StructListArrayReflection)) StructListArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumUShortArrayReflection)) EnumUShortArrayReflectionMethod = method;
                        break;
                    case 26:
                        if (method.Name == nameof(EnumIntLeftArrayReflection)) EnumIntLeftArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumIntListArrayReflection)) EnumIntListArrayReflectionMethod = method;
                        break;
                    case 27:
                        if (method.Name == nameof(NullableLeftArrayReflection)) NullableLeftArrayReflectionMethod = method;
                        else if (method.Name == nameof(NullableListArrayReflection)) NullableListArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumByteLeftArrayReflection)) EnumByteLeftArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumByteListArrayReflection)) EnumByteListArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumUIntLeftArrayReflection)) EnumUIntLeftArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumUIntListArrayReflection)) EnumUIntListArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumLongLeftArrayReflection)) EnumLongLeftArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumLongListArrayReflection)) EnumLongListArrayReflectionMethod = method;
                        break;
                    case 28:
                        if (method.Name == nameof(PrimitiveMemberIntReflection)) PrimitiveMemberIntReflectionMethod = method;
                        else if (method.Name == nameof(EnumULongLeftArrayReflection)) EnumULongLeftArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumULongListArrayReflection)) EnumULongListArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumShortLeftArrayReflection)) EnumShortLeftArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumShortListArrayReflection)) EnumShortListArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumSByteLeftArrayReflection)) EnumSByteLeftArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumSByteListArrayReflection)) EnumSByteListArrayReflectionMethod = method;
                        break;
                    case 29:
                        if (method.Name == nameof(PrimitiveMemberByteReflection)) PrimitiveMemberByteReflectionMethod = method;
                        else if (method.Name == nameof(PrimitiveMemberLongReflection)) PrimitiveMemberLongReflectionMethod = method;
                        else if (method.Name == nameof(PrimitiveMemberUIntReflection)) PrimitiveMemberUIntReflectionMethod = method;
                        else if (method.Name == nameof(EnumUShortLeftArrayReflection)) EnumUShortLeftArrayReflectionMethod = method;
                        else if (method.Name == nameof(EnumUShortListArrayReflection)) EnumUShortListArrayReflectionMethod = method;
                        break;
                    case 30:
                        if (method.Name == nameof(PrimitiveMemberULongReflection)) PrimitiveMemberULongReflectionMethod = method;
                        else if (method.Name == nameof(PrimitiveMemberShortReflection)) PrimitiveMemberShortReflectionMethod = method;
                        else if (method.Name == nameof(PrimitiveMemberSByteReflection)) PrimitiveMemberSByteReflectionMethod = method;
                        break;
                    case 31:
                        if (method.Name == nameof(PrimitiveMemberUShortReflection)) PrimitiveMemberUShortReflectionMethod = method;
                        break;
                }
            }
#else
            SerializeDelegates.Add(typeof(bool), new SerializeDelegateReference((Action<BinarySerializer, bool>)primitiveSerialize, (Action<BinarySerializer, bool>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(bool?), new SerializeDelegateReference((Action<BinarySerializer, bool?>)primitiveSerialize, (Action<BinarySerializer, bool?>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(bool[]), new SerializeDelegateReference((Action<BinarySerializer, bool[]>)primitiveSerialize, new GenericType<bool[]>()));
            SerializeDelegates.Add(typeof(bool?[]), new SerializeDelegateReference((Action<BinarySerializer, bool?[]>)primitiveSerialize, new GenericType<bool?[]>()));
            SerializeDelegates.Add(typeof(LeftArray<bool>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<bool>>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ListArray<bool>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<bool>>)primitiveSerialize, new GenericType<ListArray<bool>>()));
            SerializeDelegates.Add(typeof(byte), new SerializeDelegateReference((Action<BinarySerializer, byte>)primitiveSerialize, (Action<BinarySerializer, byte>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(byte?), new SerializeDelegateReference((Action<BinarySerializer, byte?>)primitiveSerialize, (Action<BinarySerializer, byte?>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(byte[]), new SerializeDelegateReference((Action<BinarySerializer, byte[]>)primitiveSerialize, new GenericType<byte[]>()));
            SerializeDelegates.Add(typeof(byte?[]), new SerializeDelegateReference((Action<BinarySerializer, byte?[]>)primitiveSerialize, new GenericType<byte?[]>()));
            SerializeDelegates.Add(typeof(LeftArray<byte>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<byte>>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ListArray<byte>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<byte>>)primitiveSerialize, new GenericType<ListArray<byte>>()));
            SerializeDelegates.Add(typeof(sbyte), new SerializeDelegateReference((Action<BinarySerializer, sbyte>)primitiveSerialize, (Action<BinarySerializer, sbyte>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(sbyte?), new SerializeDelegateReference((Action<BinarySerializer, sbyte?>)primitiveSerialize, (Action<BinarySerializer, sbyte?>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(sbyte[]), new SerializeDelegateReference((Action<BinarySerializer, sbyte[]>)primitiveSerialize, new GenericType<sbyte[]>()));
            SerializeDelegates.Add(typeof(sbyte?[]), new SerializeDelegateReference((Action<BinarySerializer, sbyte?[]>)primitiveSerialize, new GenericType<sbyte?[]>()));
            SerializeDelegates.Add(typeof(LeftArray<sbyte>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<sbyte>>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ListArray<sbyte>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<sbyte>>)primitiveSerialize, new GenericType<ListArray<sbyte>>()));
            SerializeDelegates.Add(typeof(short), new SerializeDelegateReference((Action<BinarySerializer, short>)primitiveSerialize, (Action<BinarySerializer, short>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(short?), new SerializeDelegateReference((Action<BinarySerializer, short?>)primitiveSerialize, (Action<BinarySerializer, short?>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(short[]), new SerializeDelegateReference((Action<BinarySerializer, short[]>)primitiveSerialize, new GenericType<short[]>()));
            SerializeDelegates.Add(typeof(short?[]), new SerializeDelegateReference((Action<BinarySerializer, short?[]>)primitiveSerialize, new GenericType<short?[]>()));
            SerializeDelegates.Add(typeof(LeftArray<short>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<short>>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ListArray<short>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<short>>)primitiveSerialize, new GenericType<ListArray<short>>()));
            SerializeDelegates.Add(typeof(ushort), new SerializeDelegateReference((Action<BinarySerializer, ushort>)primitiveSerialize, (Action<BinarySerializer, ushort>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(ushort?), new SerializeDelegateReference((Action<BinarySerializer, ushort?>)primitiveSerialize, (Action<BinarySerializer, ushort?>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(ushort[]), new SerializeDelegateReference((Action<BinarySerializer, ushort[]>)primitiveSerialize, new GenericType<ushort[]>()));
            SerializeDelegates.Add(typeof(ushort?[]), new SerializeDelegateReference((Action<BinarySerializer, ushort?[]>)primitiveSerialize, new GenericType<ushort?[]>()));
            SerializeDelegates.Add(typeof(LeftArray<ushort>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<ushort>>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ListArray<ushort>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<ushort>>)primitiveSerialize, new GenericType<ListArray<ushort>>()));
            SerializeDelegates.Add(typeof(int), new SerializeDelegateReference((Action<BinarySerializer, int>)primitiveSerialize, (Action<BinarySerializer, int>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(int?), new SerializeDelegateReference((Action<BinarySerializer, int?>)Nullable<int>, (Action<BinarySerializer, int?>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(int[]), new SerializeDelegateReference((Action<BinarySerializer, int[]>)primitiveSerialize, new GenericType<int[]>()));
            SerializeDelegates.Add(typeof(int?[]), new SerializeDelegateReference((Action<BinarySerializer, int?[]>)primitiveSerialize, new GenericType<int?[]>()));
            SerializeDelegates.Add(typeof(LeftArray<int>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<int>>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ListArray<int>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<int>>)primitiveSerialize, new GenericType<ListArray<int>>()));
            SerializeDelegates.Add(typeof(uint), new SerializeDelegateReference((Action<BinarySerializer, uint>)primitiveSerialize, (Action<BinarySerializer, uint>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(uint?), new SerializeDelegateReference((Action<BinarySerializer, uint?>)Nullable<uint>, (Action<BinarySerializer, uint?>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(uint[]), new SerializeDelegateReference((Action<BinarySerializer, uint[]>)primitiveSerialize, new GenericType<uint[]>()));
            SerializeDelegates.Add(typeof(uint?[]), new SerializeDelegateReference((Action<BinarySerializer, uint?[]>)primitiveSerialize, new GenericType<uint?[]>()));
            SerializeDelegates.Add(typeof(LeftArray<uint>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<uint>>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ListArray<uint>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<uint>>)primitiveSerialize, new GenericType<ListArray<uint>>()));
            SerializeDelegates.Add(typeof(long), new SerializeDelegateReference((Action<BinarySerializer, long>)primitiveSerialize, (Action<BinarySerializer, long>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(long?), new SerializeDelegateReference((Action<BinarySerializer, long?>)Nullable<long>, (Action<BinarySerializer, long?>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(long[]), new SerializeDelegateReference((Action<BinarySerializer, long[]>)primitiveSerialize, new GenericType<long[]>()));
            SerializeDelegates.Add(typeof(long?[]), new SerializeDelegateReference((Action<BinarySerializer, long?[]>)primitiveSerialize, new GenericType<long?[]>()));
            SerializeDelegates.Add(typeof(LeftArray<long>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<long>>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ListArray<long>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<long>>)primitiveSerialize, new GenericType<ListArray<long>>()));
            SerializeDelegates.Add(typeof(ulong), new SerializeDelegateReference((Action<BinarySerializer, ulong>)primitiveSerialize, (Action<BinarySerializer, ulong>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(ulong?), new SerializeDelegateReference((Action<BinarySerializer, ulong?>)Nullable<ulong>, (Action<BinarySerializer, ulong?>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(ulong[]), new SerializeDelegateReference((Action<BinarySerializer, ulong[]>)primitiveSerialize, new GenericType<ulong[]>()));
            SerializeDelegates.Add(typeof(ulong?[]), new SerializeDelegateReference((Action<BinarySerializer, ulong?[]>)primitiveSerialize, new GenericType<ulong?[]>()));
            SerializeDelegates.Add(typeof(LeftArray<ulong>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<ulong>>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ListArray<ulong>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<ulong>>)primitiveSerialize, new GenericType<ListArray<ulong>>()));
            SerializeDelegates.Add(typeof(float), new SerializeDelegateReference((Action<BinarySerializer, float>)primitiveSerialize, (Action<BinarySerializer, float>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(float?), new SerializeDelegateReference((Action<BinarySerializer, float?>)Nullable<float>, (Action<BinarySerializer, float?>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(float[]), new SerializeDelegateReference((Action<BinarySerializer, float[]>)primitiveSerialize, new GenericType<float[]>()));
            SerializeDelegates.Add(typeof(float?[]), new SerializeDelegateReference((Action<BinarySerializer, float?[]>)primitiveSerialize, new GenericType<float?[]>()));
            SerializeDelegates.Add(typeof(LeftArray<float>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<float>>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ListArray<float>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<float>>)primitiveSerialize, new GenericType<ListArray<float>>()));
            SerializeDelegates.Add(typeof(double), new SerializeDelegateReference((Action<BinarySerializer, double>)primitiveSerialize, (Action<BinarySerializer, double>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(double?), new SerializeDelegateReference((Action<BinarySerializer, double?>)Nullable<double>, (Action<BinarySerializer, double?>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(double[]), new SerializeDelegateReference((Action<BinarySerializer, double[]>)primitiveSerialize, new GenericType<double[]>()));
            SerializeDelegates.Add(typeof(double?[]), new SerializeDelegateReference((Action<BinarySerializer, double?[]>)primitiveSerialize, new GenericType<double?[]>()));
            SerializeDelegates.Add(typeof(LeftArray<double>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<double>>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ListArray<double>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<double>>)primitiveSerialize, new GenericType<ListArray<double>>()));
            SerializeDelegates.Add(typeof(decimal), new SerializeDelegateReference((Action<BinarySerializer, decimal>)primitiveSerialize, (Action<BinarySerializer, decimal>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(decimal?), new SerializeDelegateReference((Action<BinarySerializer, decimal?>)Nullable<decimal>, (Action<BinarySerializer, decimal?>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(decimal[]), new SerializeDelegateReference((Action<BinarySerializer, decimal[]>)primitiveSerialize, new GenericType<decimal[]>()));
            SerializeDelegates.Add(typeof(decimal?[]), new SerializeDelegateReference((Action<BinarySerializer, decimal?[]>)primitiveSerialize, new GenericType<decimal?[]>()));
            SerializeDelegates.Add(typeof(LeftArray<decimal>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<decimal>>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ListArray<decimal>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<decimal>>)primitiveSerialize, new GenericType<ListArray<decimal>>()));
            SerializeDelegates.Add(typeof(char), new SerializeDelegateReference((Action<BinarySerializer, char>)primitiveSerialize, (Action<BinarySerializer, char>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(char?), new SerializeDelegateReference((Action<BinarySerializer, char?>)primitiveSerialize, (Action<BinarySerializer, char?>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(char[]), new SerializeDelegateReference((Action<BinarySerializer, char[]>)primitiveSerialize, new GenericType<char[]>()));
            SerializeDelegates.Add(typeof(char?[]), new SerializeDelegateReference((Action<BinarySerializer, char?[]>)primitiveSerialize, new GenericType<char?[]>()));
            SerializeDelegates.Add(typeof(LeftArray<char>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<char>>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ListArray<char>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<char>>)primitiveSerialize, new GenericType<ListArray<char>>()));
            SerializeDelegates.Add(typeof(DateTime), new SerializeDelegateReference((Action<BinarySerializer, DateTime>)primitiveSerialize, (Action<BinarySerializer, DateTime>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(DateTime?), new SerializeDelegateReference((Action<BinarySerializer, DateTime?>)Nullable<DateTime>, (Action<BinarySerializer, DateTime?>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(DateTime[]), new SerializeDelegateReference((Action<BinarySerializer, DateTime[]>)primitiveSerialize, new GenericType<DateTime[]>()));
            SerializeDelegates.Add(typeof(DateTime?[]), new SerializeDelegateReference((Action<BinarySerializer, DateTime?[]>)primitiveSerialize, new GenericType<DateTime?[]>()));
            SerializeDelegates.Add(typeof(LeftArray<DateTime>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<DateTime>>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ListArray<DateTime>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<DateTime>>)primitiveSerialize, new GenericType<ListArray<DateTime>>()));
            SerializeDelegates.Add(typeof(TimeSpan), new SerializeDelegateReference((Action<BinarySerializer, TimeSpan>)primitiveSerialize, (Action<BinarySerializer, TimeSpan>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(TimeSpan?), new SerializeDelegateReference((Action<BinarySerializer, TimeSpan?>)Nullable<TimeSpan>, (Action<BinarySerializer, TimeSpan?>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(TimeSpan[]), new SerializeDelegateReference((Action<BinarySerializer, TimeSpan[]>)primitiveSerialize, new GenericType<TimeSpan[]>()));
            SerializeDelegates.Add(typeof(TimeSpan?[]), new SerializeDelegateReference((Action<BinarySerializer, TimeSpan?[]>)primitiveSerialize, new GenericType<TimeSpan?[]>()));
            SerializeDelegates.Add(typeof(LeftArray<TimeSpan>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<TimeSpan>>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ListArray<TimeSpan>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<TimeSpan>>)primitiveSerialize, new GenericType<ListArray<TimeSpan>>()));
            SerializeDelegates.Add(typeof(Guid), new SerializeDelegateReference((Action<BinarySerializer, Guid>)primitiveSerialize, (Action<BinarySerializer, Guid>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(Guid?), new SerializeDelegateReference((Action<BinarySerializer, Guid?>)Nullable<Guid>, (Action<BinarySerializer, Guid?>)primitiveMemberSerialize));
            SerializeDelegates.Add(typeof(Guid[]), new SerializeDelegateReference((Action<BinarySerializer, Guid[]>)primitiveSerialize, new GenericType<Guid[]>()));
            SerializeDelegates.Add(typeof(Guid?[]), new SerializeDelegateReference((Action<BinarySerializer, Guid?[]>)primitiveSerialize, new GenericType<Guid?[]>()));
            SerializeDelegates.Add(typeof(LeftArray<Guid>), new SerializeDelegateReference((Action<BinarySerializer, LeftArray<Guid>>)primitiveSerialize));
            SerializeDelegates.Add(typeof(ListArray<Guid>), new SerializeDelegateReference((Action<BinarySerializer, ListArray<Guid>>)primitiveSerialize, new GenericType<ListArray<Guid>>()));
            SerializeDelegates.Add(typeof(string), new SerializeDelegateReference((Action<BinarySerializer, string>)primitiveSerialize, new GenericType<string>()));
            SerializeDelegates.Add(typeof(SubString), new SerializeDelegateReference((Action<BinarySerializer, SubString>)primitiveSerialize, (Action<BinarySerializer, SubString>)primitiveSerialize));
            SerializeDelegates.Add(typeof(Type), new SerializeDelegateReference((Action<BinarySerializer, Type>)primitiveSerialize, new GenericType<Type>()));
            SerializeDelegates.Add(typeof(object), new SerializeDelegateReference((Action<BinarySerializer, object>)primitiveSerialize, new GenericType<object>()));

            SerializeDelegates.Add(typeof(Half), new SerializeDelegateReference((Action<BinarySerializer, Half>)primitiveSerialize, (Action<BinarySerializer, Half>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(Half[]), new SerializeDelegateReference((Action<BinarySerializer, Half[]>)primitiveSerialize, new GenericType<Half[]>()));
            SerializeDelegates.Add(typeof(UInt128), new SerializeDelegateReference((Action<BinarySerializer, UInt128>)primitiveSerialize, (Action<BinarySerializer, UInt128>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(UInt128[]), new SerializeDelegateReference((Action<BinarySerializer, UInt128[]>)primitiveSerialize, new GenericType<UInt128[]>()));
            SerializeDelegates.Add(typeof(Int128), new SerializeDelegateReference((Action<BinarySerializer, Int128>)primitiveSerialize, (Action<BinarySerializer, Int128>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(Int128[]), new SerializeDelegateReference((Action<BinarySerializer, Int128[]>)primitiveSerialize, new GenericType<Int128[]>()));
            SerializeDelegates.Add(typeof(System.Numerics.Complex), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Complex>)primitiveSerialize, (Action<BinarySerializer, System.Numerics.Complex>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(System.Numerics.Complex[]), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Complex[]>)primitiveSerialize, new GenericType<System.Numerics.Complex[]>()));
            SerializeDelegates.Add(typeof(System.Numerics.Plane), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Plane>)primitiveSerialize, (Action<BinarySerializer, System.Numerics.Plane>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(System.Numerics.Plane[]), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Plane[]>)primitiveSerialize, new GenericType<System.Numerics.Plane[]>()));
            SerializeDelegates.Add(typeof(System.Numerics.Quaternion), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Quaternion>)primitiveSerialize, (Action<BinarySerializer, System.Numerics.Quaternion>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(System.Numerics.Quaternion[]), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Quaternion[]>)primitiveSerialize, new GenericType<System.Numerics.Quaternion[]>()));
            SerializeDelegates.Add(typeof(System.Numerics.Matrix3x2), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Matrix3x2>)primitiveSerialize, (Action<BinarySerializer, System.Numerics.Matrix3x2>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(System.Numerics.Matrix3x2[]), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Matrix3x2[]>)primitiveSerialize, new GenericType<System.Numerics.Matrix3x2[]>()));
            SerializeDelegates.Add(typeof(System.Numerics.Matrix4x4), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Matrix4x4>)primitiveSerialize, (Action<BinarySerializer, System.Numerics.Matrix4x4>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(System.Numerics.Matrix4x4[]), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Matrix4x4[]>)primitiveSerialize, new GenericType<System.Numerics.Matrix4x4[]>()));
            SerializeDelegates.Add(typeof(System.Numerics.Vector2), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Vector2>)primitiveSerialize, (Action<BinarySerializer, System.Numerics.Vector2>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(System.Numerics.Vector2[]), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Vector2[]>)primitiveSerialize, new GenericType<System.Numerics.Vector2[]>()));
            SerializeDelegates.Add(typeof(System.Numerics.Vector3), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Vector3>)primitiveSerialize, (Action<BinarySerializer, System.Numerics.Vector3>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(System.Numerics.Vector3[]), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Vector3[]>)primitiveSerialize, new GenericType<System.Numerics.Vector3[]>()));
            SerializeDelegates.Add(typeof(System.Numerics.Vector4), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Vector4>)primitiveSerialize, (Action<BinarySerializer, System.Numerics.Vector4>)PrimitiveMemberSerialize));
            SerializeDelegates.Add(typeof(System.Numerics.Vector4[]), new SerializeDelegateReference((Action<BinarySerializer, System.Numerics.Vector4[]>)primitiveSerialize, new GenericType<System.Numerics.Vector4[]>()));

            foreach (SerializeDelegate serializeDelegate in CustomConfig.PrimitiveSerializeDelegates)
            {
                var type = default(Type);
                SerializeDelegateReference serializeDelegateReference;
                if (serializeDelegate.Check(out type, out serializeDelegateReference))
                {
                    SerializeDelegates[type] = serializeDelegateReference;
                }
            }
#endif
        }
    }
}
