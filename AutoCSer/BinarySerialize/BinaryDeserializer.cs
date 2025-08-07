using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using AutoCSer.BinarySerialize;
using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using AutoCSer.Net;
using AutoCSer.SimpleSerialize;

namespace AutoCSer
{
    /// <summary>
    /// Binary data deserialization
    /// 二进制数据反序列化
    /// </summary>
    public sealed unsafe partial class BinaryDeserializer : AutoCSer.Threading.Link<BinaryDeserializer>
    {
        /// <summary>
        /// Public default configuration parameters
        /// 公共默认配置参数
        /// </summary>
        internal static readonly DeserializeConfig DefaultConfig = AutoCSer.Configuration.Common.Get<DeserializeConfig>()?.Value ?? new DeserializeConfig();

        /// <summary>
        /// Custom context
        /// </summary>
#if NetStandard21
        internal object? Context;
#else
        internal object Context;
#endif
        /// <summary>
        /// Deserialization configuration parameters
        /// 反序列化配置参数
        /// </summary>
        internal DeserializeConfig Config = DefaultConfig;
        /// <summary>
        /// JSON deserialization
        /// </summary>
#if NetStandard21
        private JsonDeserializer? jsonDeserializer;
#else
        private JsonDeserializer jsonDeserializer;
#endif
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
        /// Member bitmap type
        /// 成员位图类型
        /// </summary>
#if NetStandard21
        internal Type? MemberMapType;
#else
        internal Type MemberMapType;
#endif
        /// <summary>
        /// The position of the historical object pointer
        /// 历史对象指针位置
        /// </summary>
#if NetStandard21
        private ReusableHashCodeKeyDictionary<object>? points;
#else
        private ReusableHashCodeKeyDictionary<object> points;
#endif
        /// <summary>
        /// Data byte array
        /// 数据字节数组
        /// </summary>
        internal byte[] Buffer = EmptyArray<byte>.Array;
        /// <summary>
        /// Data byte array data starting position
        /// 数据字节数组数据起始位置
        /// </summary>
        private byte* bufferFixed;
        /// <summary>
        /// The starting position of deserialization data
        /// 反序列化数据起始位置
        /// </summary>
        private byte* start;
        /// <summary>
        /// The end position of deserialization data
        /// 反序列化数据结束位置
        /// </summary>
        internal byte* End;
        /// <summary>
        /// The current position for reading data
        /// 当前读取数据位置
        /// </summary>
        internal byte* Current;
        /// <summary>
        /// Real type resolution location
        /// 真实类型解析位置
        /// </summary>
        private byte* realTypeCurrent;
        /// <summary>
        /// The position of the next object reference
        /// 下一个对象引用位置
        /// </summary>
        private byte* objectReference;
        /// <summary>
        /// Fix the starting position of the data
        /// 固定数据起始位置
        /// </summary>
        private byte* fixedCurrent;
        /// <summary>
        /// In-memory database request parameter context
        /// 内存数据库请求参数上下文
        /// </summary>
#if NetStandard21
        internal object? StreamPersistenceMemoryDatabaseServiceRequestParameterContext;
#else
        internal object StreamPersistenceMemoryDatabaseServiceRequestParameterContext;
#endif
        ///// <summary>
        ///// 全局版本编号
        ///// </summary>
        //internal uint GlobalVersion;
        /// <summary>
        /// The position of the next object reference
        /// 下一个对象引用位置
        /// </summary>
        private int objectReferencePoint;
        /// <summary>
        /// Whether it is necessary to call AutoCSer.Common.Config.CheckRemoteType to check the validity of the remote type
        /// 是否需要调用 AutoCSer.Common.Config.CheckRemoteType 检查远程类型的合法性
        /// </summary>
        private bool isCheckRemoteType = true;
        /// <summary>
        /// Deserialization status
        /// 反序列化状态
        /// </summary>
        internal DeserializeStateEnum State;
        /// <summary>
        /// JSON deserialization status
        /// JSON 反序列化状态
        /// </summary>
        private Json.DeserializeStateEnum jsonState;
        /// <summary>
        /// Customize deserialization error messages
        /// 自定义反序列化错误信息
        /// </summary>
#if NetStandard21
        private string? customError;
#else
        private string customError;
#endif
        /// <summary>
        /// Deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <param name="bufferFixed"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <param name="config"></param>
        /// <returns></returns>
#if NetStandard21
        private DeserializeResult deserialize<T>(byte[]? buffer, byte* bufferFixed, byte* start, int size, ref T? value, DeserializeConfig? config)
#else
        private DeserializeResult deserialize<T>(byte[] buffer, byte* bufferFixed, byte* start, int size, ref T value, DeserializeConfig config)
#endif
        {
            if ((size & 3) == 0)
            {
                int length = size - sizeof(int);
                if (length >= 0)
                {
                    if (length != 0)
                    {
                        End = start + length;
                        if (*(int*)End == length)
                        {
                            uint headerValue = *(uint*)start;
                            if ((headerValue & BinarySerializeConfig.HeaderMapAndValue) == BinarySerializeConfig.HeaderMapValue)
                            {
                                this.start = start;
                                this.bufferFixed = bufferFixed;
                                DeserializeStateEnum checkState = checkHeaderValue(headerValue);
                                if (checkState != DeserializeStateEnum.Success) return new DeserializeResult(checkState);
                                Config = config ?? DefaultConfig;
                                Buffer = buffer ?? EmptyArray<byte>.Array;
                                TypeDeserializer<T>.Deserialize(this, ref value);
                                if (State == DeserializeStateEnum.Success)
                                {
                                    if (Current == End) return new DeserializeResult(MemberMap);
                                    return new DeserializeResult(DeserializeStateEnum.EndVerify);
                                }
                                return new DeserializeResult(State, jsonState, customError);
                            }
                            return new DeserializeResult(DeserializeStateEnum.HeaderError);
                        }
                        return new DeserializeResult(DeserializeStateEnum.EndVerify);
                    }
                    if (*(int*)start == BinarySerializer.NullValue)
                    {
                        value = default(T);
                        return new DeserializeResult(DeserializeStateEnum.Success);
                    }
                }
            }
            return new DeserializeResult(DeserializeStateEnum.UnknownData);
        }
        /// <summary>
        /// Check the header data
        /// 检查头部数据
        /// </summary>
        /// <param name="headerValue"></param>
        /// <returns></returns>
        private DeserializeStateEnum checkHeaderValue(uint headerValue)
        {
            Current = start + sizeof(int);
            if ((headerValue & BinarySerializeConfig.ObjectReference) != 0)
            {
                int pointSize = *(int*)(End -= sizeof(int));
                if ((pointSize & 3) != 0 || pointSize < 0 || pointSize >= (int)(End - Current))
                {
                    return DeserializeStateEnum.HeaderError;
                }
                objectReferencePoint = *(int*)(objectReference = End - sizeof(int));
                End -= pointSize;
            }
            else objectReferencePoint = int.MaxValue;
            jsonState = AutoCSer.Json.DeserializeStateEnum.Success;
            State = DeserializeStateEnum.Success;
            return DeserializeStateEnum.Success;
        }
        /// <summary>
        /// Set a custom context
        /// 设置自定义上下文
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
#if NET8
        [MemberNotNull(nameof(Context))]
#endif
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetContext(object context, DeserializeConfig config)
        {
            Context = context;
            Config = config;
        }
        /// <summary>
        /// Set a custom context
        /// 设置自定义上下文
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
#if NET8
        [MemberNotNull(nameof(Context))]
#endif
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetContextNoCheckRemoteType(object context, DeserializeConfig config)
        {
            isCheckRemoteType = false;
            Context = context;
            Config = config;
        }
        /// <summary>
        /// Release resources (Thread static instance mode)
        /// 释放资源（线程静态实例模式）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void freeThreadStatic()
        {
            MemberMap = null;
            points?.ClearArray();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void Free()
        {
            freeThreadStatic();
            AutoCSer.Threading.LinkPool<BinaryDeserializer>.Default.Push(this);
        }
        /// <summary>
        /// Release resources
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void FreeContext()
        {
            Context = null;
            Free();
        }
        /// <summary>
        /// Release resources
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void FreeContextCheckRemoteType()
        {
            this.isCheckRemoteType = true;
            FreeContext();
        }
        /// <summary>
        /// Check whether the data is null
        /// 检查数据是否为 null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal bool CheckNotNull<T>(ref T? value)
#else
        internal bool CheckNotNull<T>(ref T value)
#endif
        {
            if (*(int*)Current == BinarySerializer.NotNullValue)
            {
                Current += sizeof(int);
                return true;
            }
            if (*(int*)Current == BinarySerializer.NullValue)
            {
                value = default(T);
                Current += sizeof(int);
            }
            else State = DeserializeStateEnum.ErrorDataType;
            return false;
        }
        /// <summary>
        /// Getting a history object
        /// 获取历史对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="isRealType"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool CheckTryPush<T>([NotNullWhen(true)]ref T? value, out bool isRealType)
#else
        internal bool CheckTryPush<T>(ref T value, out bool isRealType)
#endif
        {
            if (*(int*)Current == BinarySerializer.NotNullValue)
            {
                if (value == null)
                {
                    value = AutoCSer.Metadata.DefaultConstructor<T>.Constructor();
                    if (value == null)
                    {
                        realTypeCurrent = Current;
                        if (*(int*)(Current += sizeof(int)) == BinarySerializer.RealTypeValue) isRealType = true;
                        else
                        {
                            isRealType = false;
                            State = DeserializeStateEnum.ConstructorNull;
                            customError = $"{typeof(T).fullName()} 默认构造缺少返回值";
                        }
                        return false;
                    }
                }
                TryPush(value);
                isRealType = false;
                return true;
            }
            if (*(int*)Current == BinarySerializer.NullValue)
            {
                value = default(T);
                Current += sizeof(int);
            }
            else if (*(int*)Current < 0) getPoint(ref value);
            else State = DeserializeStateEnum.ErrorDataType;
            return isRealType = false;
        }
        /// <summary>
        /// Constructor call
        /// 构造函数调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool Constructor<T>([NotNullWhen(true)]out T? value)
#else
        internal bool Constructor<T>(out T value)
#endif
        {
            value = AutoCSer.Metadata.DefaultConstructor<T>.Constructor();
            if (value != null) return true;
            State = DeserializeStateEnum.ConstructorNull;
            customError = $"{typeof(T).fullName()} 默认构造缺少返回值";
            return false;
        }
        /// <summary>
        /// Getting a history object
        /// 获取历史对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal bool CheckNullPoint<T>(ref T? value)
#else
        internal bool CheckNullPoint<T>(ref T value)
#endif
        {
            if (*(int*)Current < 0)
            {
                if (*(int*)Current == BinarySerializer.NullValue)
                {
                    value = default(T);
                    Current += sizeof(int);
                }
                else getPoint(ref value);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Getting a history object
        /// 获取历史对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        private void getPoint<T>(ref T value)
        {
            if (points != null)
            {
                var pointValue = default(object);
                if (points.TryGetValue(*(int*)Current, out pointValue))
                {
                    value = (T)pointValue;
                    Current += sizeof(int);
                    return;
                }
            }
            State = DeserializeStateEnum.NoPoint;
            return;
        }
        /// <summary>
        /// Add historical objects
        /// 添加历史对象
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void tryPush(object value)
        {
            if (objectReferencePoint == (int)(start - Current)) push(value);
        }
        /// <summary>
        /// Add historical objects
        /// 添加历史对象
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void TryPush(object value)
        {
            tryPush(value);
            Current += sizeof(int);
        }
        /// <summary>
        /// Add historical objects
        /// 添加历史对象
        /// </summary>
        /// <param name="value"></param>
        private void push(object value)
        {
            if (points == null) points = new ReusableHashCodeKeyDictionary<object>();
            points.Set(objectReferencePoint, value);
            objectReferencePoint = objectReference == End ? int.MaxValue : *(int*)(objectReference -= sizeof(int));
        }
        /// <summary>
        /// Object null value detection
        /// 对象 null 值检测
        /// </summary>
        /// <returns>返回 0 表示 null</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal int CheckNullValue()
        {
            if (*(int*)Current == BinarySerializer.NullValue)
            {
                Current += sizeof(int);
                return 0;
            }
            return 1;
        }
        ///// <summary>
        ///// 检查是否空值
        ///// </summary>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal bool CheckNotNull()
        //{
        //    if (*(int*)Read == BinarySerializer.NullValue)
        //    {
        //        Read += sizeof(int);
        //        return false;
        //    }
        //    return true;
        //}
        /// <summary>
        /// Match the number of members
        /// 匹配成员数量
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool CheckMemberCount(int count)
        {
            if (*(int*)Current == count)
            {
                Current += sizeof(int);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Get the member bitmap
        /// 获取成员位图
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
#if NetStandard21
        internal MemberMap<T>? GetMemberMap<T>()
#else
        internal MemberMap<T> GetMemberMap<T>()
#endif
        {
            if ((*(uint*)Current & 0xc0000000U) == 0)
            {
                var memberMap = default(MemberMap<T>);
                if (MemberMap == null)
                {
                    MemberMap = memberMap = MemberMapType == typeof(T) ? new MemberMap<T>(): MemberMap<T>.Default;
                }
                else
                {
                    memberMap = MemberMap as MemberMap<T>;
                    if (memberMap == null)
                    {
                        State = DeserializeStateEnum.MemberMapType;
                        return null;
                    }
                }
                return MemberMap.Deserialize(this) ? memberMap : null;
            }
            State = DeserializeStateEnum.MemberMap;
            return null;
        }
        /// <summary>
        /// Deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">Target data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void BinaryDeserialize<T>(ref T? value)
#else
        public void BinaryDeserialize<T>(ref T value)
#endif
        {
            TypeDeserializer<T>.Deserialize(this, ref value);
        }
        /// <summary>
        /// Deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value">Target data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static void Deserialize<T>(BinaryDeserializer deserializer, ref T? value)
#else
        internal static void Deserialize<T>(BinaryDeserializer deserializer, ref T value)
#endif
        {
            TypeDeserializer<T>.Deserialize(deserializer, ref value);
        }
        /// <summary>
        /// Custom deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void ICustom<T>(ref T? value) where T : ICustomSerialize<T>
#else
        public void ICustom<T>(ref T value) where T : ICustomSerialize<T>
#endif
        {
            if (value != null || Constructor(out value)) value.Deserialize(this);
        }
        /// <summary>
        /// Custom deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void ICustom<T>(AutoCSer.BinaryDeserializer deserializer, ref T? value) where T : ICustomSerialize<T>
#else
        public static void ICustom<T>(AutoCSer.BinaryDeserializer deserializer, ref T value) where T : ICustomSerialize<T>
#endif
        {
            deserializer.ICustom(ref value);
        }
        /// <summary>
        /// JSON mixed binary deserialization
        /// JSON 混杂二进制 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
#if NetStandard21
        public void Json<T>(ref T? value)
#else
        public void Json<T>(ref T value)
#endif
        {
            if (CheckNullPoint(ref value))
            {
                int size = *(int*)Current;
                if (size == 0)
                {
                    Current += sizeof(int);
                    value = default(T);
                }
                else deserializeJson(ref value, size);
            }
        }
        /// <summary>
        /// JSON mixed binary deserialization
        /// JSON 混杂二进制 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void Json<T>(BinaryDeserializer deserializer, ref T? value)
#else
        public static void Json<T>(BinaryDeserializer deserializer, ref T value)
#endif
        {
            deserializer.Json(ref value);
        }
        /// <summary>
        /// JSON mixed binary deserialization
        /// JSON 混杂二进制 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void StructJson<T>(ref T value)
        {
#pragma warning disable CS8601
            deserializeJson(ref value, *(int*)Current);
#pragma warning restore CS8601
        }
        /// <summary>
        /// JSON mixed binary deserialization
        /// JSON 混杂二进制 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StructJson<T>(BinaryDeserializer deserializer, ref T value)
        {
            deserializer.StructJson(ref value);
        }
        /// <summary>
        /// Binary deserialization to simple deserialization (for AOT code generation, not allowed for developers to call)
        /// 二进制反序列化转简单反序列化（用于 AOT 代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Simple<T>(BinaryDeserializer deserializer, ref T value) where T : struct
        {
            deserializer.SimpleDeserialize(ref value);
        }
        /// <summary>
        /// Real type deserialization
        /// 真实类型反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
#if NetStandard21
        private object? realTypeObject<T>()
#else
        private object realTypeObject<T>()
#endif
        {
            var value = default(T);
            if (Constructor(out value))
            {
                byte* current = Current;
                Current = realTypeCurrent;
                tryPush(value);
                Current = current;
                TypeDeserializer<T>.Deserialize(this, ref value);
                return value;
            }
            return null;
        }
        /// <summary>
        /// Real type deserialization
        /// 真实类型反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static object? RealTypeObject<T>(BinaryDeserializer deserializer)
#else
        internal static object RealTypeObject<T>(BinaryDeserializer deserializer)
#endif
        {
            return deserializer.realTypeObject<T>();
        }
        /// <summary>
        /// Real type deserialization
        /// 真实类型反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
#if NetStandard21
        internal void RealType<T>(ref T? value)
#else
        internal void RealType<T>(ref T value)
#endif
        {
            var type = default(Type);
            BinaryDeserialize(ref type);
            if (State == DeserializeStateEnum.Success)
            {
                if (type != null)
                {
#if AOT
                    value = (T?)RealTypeObjectMethod.MakeGenericMethod(type).Invoke(null, new object[] { this });
#else
                    value = GenericType.Get(type).BinaryDeserializeRealTypeObjectDelegate(this).castType<T>();
#endif
                }
                else State = DeserializeStateEnum.ErrorType;
            }
        }
        /// <summary>
        /// Custom deserialization not supported types
        /// 自定义反序列化不支持的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal void NotSupport<T>(ref T? value)
#else
        internal void NotSupport<T>(ref T value)
#endif
        {
            if (*(int*)Current == BinarySerializer.NullValue)
            {
                value = default(T);
                Current += sizeof(int);
            }
            else State = DeserializeStateEnum.NotSupport;
        }
        /// <summary>
        /// Custom deserialization not supported types
        /// 自定义反序列化不支持的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void NotSupport<T>(BinaryDeserializer deserializer, ref T? value)
#else
        public static void NotSupport<T>(BinaryDeserializer deserializer, ref T value)
#endif
        {
            BinarySerializer.CustomConfig.NotSupport(deserializer, ref value);
        }
        /// <summary>
        /// Base type deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="BT"></typeparam>
        /// <param name="value"></param>
#if NetStandard21
        private void baseDeserialize<T, BT>(ref T? value)
#else
        private void baseDeserialize<T, BT>(ref T value)
#endif
            where T : class, BT
        {
            if (*(int*)Current == BinarySerializer.NotNullValue)
            {
                if (value == null && !Constructor(out value)) return;
                TryPush(value);
                var baseValue = (BT)value;
                TypeDeserializer<BT>.DefaultDeserializer(this, ref baseValue);
                return;
            }
            if (*(int*)Current == BinarySerializer.NullValue)
            {
                value = default(T);
                Current += sizeof(int);
            }
            else if (*(int*)Current < 0) getPoint(ref value);
            else State = DeserializeStateEnum.ErrorDataType;
        }
        /// <summary>
        /// Base type deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="BT"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void Base<T, BT>(BinaryDeserializer deserializer, ref T? value)
#else
        public static void Base<T, BT>(BinaryDeserializer deserializer, ref T value)
#endif
            where T : class, BT
        {
            deserializer.baseDeserialize<T, BT>(ref value);
        }
        /// <summary>
        /// Object serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">Data object</param>
        public void BinaryDeserialize<T>(ref T? value) where T : struct
        {
            if (*(int*)Current <= BinarySerializer.NotNullValue)
            {
                if (*(int*)Current == BinarySerializer.NotNullValue)
                {
                    T newValue = value.HasValue ? value.Value : default(T);
                    Current += sizeof(int);
                    TypeDeserializer<T>.DefaultDeserializer(this, ref newValue);
                    value = newValue;
                }
                else
                {
                    Current += sizeof(int);
                    value = null;
                }
            }
            else State = DeserializeStateEnum.ErrorDataType;
        }
        /// <summary>
        /// Object serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value">Data object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Nullable<T>(BinaryDeserializer deserializer, ref T? value) where T : struct
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns>Array length</returns>
#if NetStandard21
        private int deserializeArray<T>(ref T[]? value)
#else
        private int deserializeArray<T>(ref T[] value)
#endif
        {
            if (CheckNullPoint(ref value))
            {
                if (*(int*)Current != 0) return *(int*)Current;
                TryPush(value = EmptyArray<T>.Array);
            }
            return 0;
        }
        /// <summary>
        /// Create an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="length"></param>
        /// <returns></returns>
#if NetStandard21
        private bool createArray<T>([NotNullWhen(true)] ref T[]? array, int length)
#else
        private bool createArray<T>(ref T[] array, int length)
#endif
        {
            if (length <= Config.MaxArraySize)
            {
                tryPush(array = new T[length]);
                return true;
            }
            State = DeserializeStateEnum.ArraySizeOutOfRange;
            return false;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns>Array length</returns>
#if NetStandard21
        private int deserializeArray<T>(ref ListArray<T>? value)
#else
        private int deserializeArray<T>(ref ListArray<T> value)
#endif
        {
            if (CheckNullPoint(ref value))
            {
                if (*(int*)Current != 0) return *(int*)Current;
                TryPush(value = new ListArray<T>(EmptyArray<T>.Array));
            }
            return 0;
        }
        /// <summary>
        /// Create an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="length"></param>
        /// <returns></returns>
#if NetStandard21
        private bool createArray<T>([NotNullWhen(true)] ref ListArray<T>? array, int length)
#else
        private bool createArray<T>(ref ListArray<T> array, int length)
#endif
        {
            if (length <= Config.MaxArraySize)
            {
                tryPush(array = new ListArray<T>(new T[length]));
                return true;
            }
            State = DeserializeStateEnum.ArraySizeOutOfRange;
            return false;
        }
        /// <summary>
        /// Create an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private bool createArray<T>(ref LeftArray<T> array, int length)
        {
            if ((uint)length <= (uint)Config.MaxArraySize)
            {
                array.Set(new T[length]);
                return true;
            }
            State = DeserializeStateEnum.ArraySizeOutOfRange;
            return false;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array object</param>
#if NetStandard21
        public void BinaryDeserialize<T>(ref T?[]? array) where T : class
#else
        public void BinaryDeserialize<T>(ref T[] array) where T : class
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapLength = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        DeserializeArrayMap arrayMap = new DeserializeArrayMap(Current + sizeof(int));
                        Current += mapLength;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                TypeDeserializer<T>.Deserialize(this, ref array[index]);
                                if (State != DeserializeStateEnum.Success) return;
                            }
                        }
                    }
                }
                else State = DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void Array<T>(BinaryDeserializer deserializer, ref T?[]? array) where T : class
#else
        public static void Array<T>(BinaryDeserializer deserializer, ref T[] array) where T : class
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array object</param>
#if NetStandard21
        public void BinaryDeserialize<T>(ref ListArray<T?>? array) where T : class
#else
        public void BinaryDeserialize<T>(ref ListArray<T> array) where T : class
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapLength = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        var bufferArray = array.Array.Array;
                        DeserializeArrayMap arrayMap = new DeserializeArrayMap(Current + sizeof(int));
                        Current += mapLength;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) bufferArray[index] = null;
                            else
                            {
                                TypeDeserializer<T>.Deserialize(this, ref bufferArray[index]);
                                if (State != DeserializeStateEnum.Success) return;
                            }
                        }
                    }
                }
                else State = DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void ListArray<T>(BinaryDeserializer deserializer, ref ListArray<T?>? array) where T : class
#else
        public static void ListArray<T>(BinaryDeserializer deserializer, ref ListArray<T> array) where T : class
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array object</param>
#if NetStandard21
        public void BinaryDeserialize<T>(ref LeftArray<T?> array) where T : class
#else
        public void BinaryDeserialize<T>(ref LeftArray<T> array) where T : class
#endif
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long mapLength = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        var bufferArray = array.Array;
                        DeserializeArrayMap arrayMap = new DeserializeArrayMap(Current + sizeof(int));
                        Current += mapLength;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) bufferArray[index] = null;
                            else
                            {
                                TypeDeserializer<T>.Deserialize(this, ref bufferArray[index]);
                                if (State != DeserializeStateEnum.Success) return;
                            }
                        }
                    }
                }
                else State = DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void LeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T?> array) where T : class
#else
        public static void LeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : class
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array object</param>
#if NetStandard21
        public void StructArray<T>(ref T[]? array) where T : struct
#else
        public void StructArray<T>(ref T[] array) where T : struct
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                if ((long)(length + 1) * sizeof(int) <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        Current += sizeof(int);
                        for (int index = 0; index != length; ++index)
                        {
                            TypeDeserializer<T>.Deserialize(this, ref array[index]);
                            if (State != DeserializeStateEnum.Success) return;
                        }
                    }
                }
                else State = DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void StructArray<T>(BinaryDeserializer deserializer, ref T[]? array) where T : struct
#else
        public static void StructArray<T>(BinaryDeserializer deserializer, ref T[] array) where T : struct
#endif
        {
            deserializer.StructArray(ref array);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array object</param>
#if NetStandard21
        public void StructArray<T>(ref ListArray<T>? array) where T : struct
#else
        public void StructArray<T>(ref ListArray<T> array) where T : struct
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                if ((long)(length + 1) * sizeof(int) <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        var bufferArray = array.Array.Array;
                        Current += sizeof(int);
                        for (int index = 0; index != length; ++index)
                        {
                            TypeDeserializer<T>.Deserialize(this, ref bufferArray[index]);
                            if (State != DeserializeStateEnum.Success) return;
                        }
                    }
                }
                else State = DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void StructListArray<T>(BinaryDeserializer deserializer, ref ListArray<T>? array) where T : struct
#else
        public static void StructListArray<T>(BinaryDeserializer deserializer, ref ListArray<T> array) where T : struct
#endif
        {
            deserializer.StructArray(ref array);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array object</param>
        public void StructArray<T>(ref LeftArray<T> array) where T : struct
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                if ((long)(length + 1) * sizeof(int) <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        T[] bufferArray = array.Array;
                        Current += sizeof(int);
                        for (int index = 0; index != length; ++index)
                        {
                            TypeDeserializer<T>.Deserialize(this, ref bufferArray[index]);
                            if (State != DeserializeStateEnum.Success) return;
                        }
                    }
                }
                else State = DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void StructLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T> array) where T : struct
        {
            deserializer.StructArray(ref array);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array object</param>
#if NetStandard21
        public void NullableArray<T>(ref T?[]? array) where T : struct
#else
        public void NullableArray<T>(ref T?[] array) where T : struct
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapLength = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        DeserializeArrayMap arrayMap = new DeserializeArrayMap(Current + sizeof(int));
                        Current += mapLength;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) array[index] = null;
                            else
                            {
                                T value = default(T);
                                TypeDeserializer<T>.Deserialize(this, ref value);
                                if (State != DeserializeStateEnum.Success) return;
                                array[index] = value;
                            }
                        }
                    }
                }
                else State = DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void NullableArray<T>(BinaryDeserializer deserializer, ref T?[]? array) where T : struct
#else
        public static void NullableArray<T>(BinaryDeserializer deserializer, ref T?[] array) where T : struct
#endif
        {
            deserializer.NullableArray(ref array);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array object</param>
#if NetStandard21
        public void NullableArray<T>(ref ListArray<T?>? array) where T : struct
#else
        public void NullableArray<T>(ref ListArray<T?> array) where T : struct
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long mapLength = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        T?[] bufferArray = array.Array.Array;
                        DeserializeArrayMap arrayMap = new DeserializeArrayMap(Current + sizeof(int));
                        Current += mapLength;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) bufferArray[index] = null;
                            else
                            {
                                T value = default(T);
                                TypeDeserializer<T>.Deserialize(this, ref value);
                                if (State != DeserializeStateEnum.Success) return;
                                bufferArray[index] = value;
                            }
                        }
                    }
                }
                else State = DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void NullableListArray<T>(BinaryDeserializer deserializer, ref ListArray<T?>? array) where T : struct
#else
        public static void NullableListArray<T>(BinaryDeserializer deserializer, ref ListArray<T?> array) where T : struct
#endif
        {
            deserializer.NullableArray(ref array);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array object</param>
        public void NullableArray<T>(ref LeftArray<T?> array) where T : struct
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                long mapLength = (((long)length + (31 + 32)) >> 5) << 2;
                if (mapLength <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        T?[] bufferArray = array.Array;
                        DeserializeArrayMap arrayMap = new DeserializeArrayMap(Current + sizeof(int));
                        Current += mapLength;
                        for (int index = 0; index != length; ++index)
                        {
                            if (arrayMap.Next() == 0) bufferArray[index] = null;
                            else
                            {
                                T value = default(T);
                                TypeDeserializer<T>.Deserialize(this, ref value);
                                if (State != DeserializeStateEnum.Success) return;
                                bufferArray[index] = value;
                            }
                        }
                    }
                }
                else State = DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="array">Array object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void NullableLeftArray<T>(BinaryDeserializer deserializer, ref LeftArray<T?> array) where T : struct
        {
            deserializer.NullableArray(ref array);
        }
        /// <summary>
        /// Collection deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="collection">Collection object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void Collection<T, VT>(ref T? collection) where T : ICollection<VT?>
#else
        public void Collection<T, VT>(ref T collection) where T : ICollection<VT>
#endif
        {
            if (CheckNullPoint(ref collection))
            {
                if (collection == null && !Constructor(out collection)) return;
                int count = *(int*)Current;
                if (!typeof(T).IsValueType) tryPush(collection);
                Current += sizeof(int);
                if (count != 0)
                {
                    if (typeof(VT).IsValueType)
                    {
                        do
                        {
                            var value = default(VT);
                            TypeDeserializer<VT>.Deserialize(this, ref value);
                            if (State != DeserializeStateEnum.Success) return;
                            collection.Add(value);
                        }
                        while (--count != 0);
                    }
                    else
                    {
                        int mapLength = ((count + 31) >> 5) << 2;
                        if (mapLength < (int)(End - Current))
                        {
                            DeserializeArrayMap arrayMap = new DeserializeArrayMap(Current);
                            Current += mapLength;
                            do
                            {
                                if (arrayMap.Next() == 0) collection.Add(default(VT));
                                else
                                {
                                    var value = default(VT);
                                    TypeDeserializer<VT>.Deserialize(this, ref value);
                                    if (State != DeserializeStateEnum.Success) return;
                                    collection.Add(value);
                                }
                            }
                            while (--count != 0);
                            return;
                        }
                        State = DeserializeStateEnum.IndexOutOfRange;
                    }
                }
            }
        }
        /// <summary>
        /// Collection deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="collection">Collection object</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void Collection<T, VT>(BinaryDeserializer deserializer, ref T? collection) where T : ICollection<VT?>
#else
        public static void Collection<T, VT>(BinaryDeserializer deserializer, ref T collection) where T : ICollection<VT>
#endif
        {
            deserializer.Collection<T, VT>(ref collection);
        }
        /// <summary>
        /// Dictionary deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="dictionary"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void Dictionary<T, KT, VT>(ref T? dictionary)
#else
        public void Dictionary<T, KT, VT>(ref T dictionary)
#endif
            where T : IDictionary<KT, VT>
        {
            if (CheckNullPoint(ref dictionary))
            {
                if (dictionary == null && !Constructor(out dictionary)) return;
                int count = *(int*)Current;
                if (!typeof(T).IsValueType) tryPush(dictionary);
                Current += sizeof(int);
                if (count != 0)
                {
                    do
                    {
                        KeyValue<KT, VT> keyValue = default(KeyValue<KT, VT>);
                        TypeDeserializer<KeyValue<KT, VT>>.Deserialize(this, ref keyValue);
                        if (State != DeserializeStateEnum.Success) return;
                        dictionary.Add(keyValue.Key, keyValue.Value);
                    }
                    while (--count != 0);
                }
            }
        }
        /// <summary>
        /// Dictionary deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="dictionary"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static void Dictionary<T, KT, VT>(BinaryDeserializer deserializer, ref T? dictionary)
#else
        public static void Dictionary<T, KT, VT>(BinaryDeserializer deserializer, ref T dictionary)
#endif
            where T : IDictionary<KT, VT>
        {
            deserializer.Dictionary<T, KT, VT>(ref dictionary);
        }
        /// <summary>
        /// JSON deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
#if NetStandard21
        private bool deserializeJson<T>(ref T? value, int size)
#else
        private bool deserializeJson<T>(ref T value, int size)
#endif
        {
            if (size > 0 && (size & 1) == 0)
            {
                byte* start = Current;
                if ((Current += (size + (2 + sizeof(int))) & (int.MaxValue - 3)) <= End)
                {
                    checkJsonDeserializeResult((jsonDeserializer ?? createJsonDeserializer()).Deserialize((char*)(start + sizeof(int)), size >> 1, ref value));
                    return State == DeserializeStateEnum.Success;
                }
            }
            State = DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
        //        /// <summary>
        //        /// JSON 反序列化
        //        /// </summary>
        //        /// <typeparam name="T"></typeparam>
        //        /// <param name="value"></param>
        //        /// <param name="data"></param>
        //        /// <param name="dataSize"></param>
        //        internal static bool DeserializeJsonNotNull<T>(ref T value, byte* data, int dataSize)
        //        {
        //            int size = *(int*)data;
        //            if (size > 0 && (size & 1) == 0 && ((size + (2 + sizeof(int))) & (int.MaxValue - 3)) == ((dataSize + 2) & (int.MaxValue - 3)))
        //            {
        //#pragma warning disable CS8601
        //                return (jsonDeserializer ?? createJsonDeserializer()).Deserialize((char*)(data + sizeof(int)), size >> 1, ref value).State == AutoCSer.Json.DeserializeStateEnum.Success;
        //#pragma warning restore CS8601
        //            }
        //            return false;
        //        }
        /// <summary>
        /// Check the deserialization status of JSON
        /// 检查 JSON 反序列化状态
        /// </summary>
        /// <param name="result"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void checkJsonDeserializeResult(AutoCSer.Json.DeserializeResult result)
        {
            jsonState = result.State;
            if (jsonState != AutoCSer.Json.DeserializeStateEnum.Success)
            {
                State = DeserializeStateEnum.JsonError;
                if (jsonState == AutoCSer.Json.DeserializeStateEnum.CustomError && string.IsNullOrEmpty(customError)) customError = result.CustomError;
            }
        }
        /// <summary>
        /// JSON deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start"></param>
        /// <param name="size">Multiples of 4
        /// 4 的倍数</param>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        internal unsafe static bool DeserializeJsonString<T>(byte* start, int size, out T? value)
#else
        internal unsafe static bool DeserializeJsonString<T>(byte* start, int size, out T value)
#endif
        {
            int length = *(int*)start;
            if ((length & 1) == 0)
            {
                if (size == (((long)length + (3 + sizeof(int))) & (long.MaxValue - 3)))
                {
                    if (length != 0) return deserializeJson((char*)(start + sizeof(int)), length >> 1, out value);
                    value = default(T);
                    return true;
                }
            }
            else
            {
                int lengthSize = UnmanagedStreamBase.GetSerializeStringLengthSize(length >>= 1);
                if (((lengthSize + length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= size)
                {
                    ByteArrayBuffer buffer = ByteArrayPool.GetBuffer(length << 1);
                    try
                    {
                        fixed (byte* bufferFixed = buffer.GetFixedBuffer())
                        {
                            byte* bufferStart = bufferFixed + buffer.StartIndex, end = start + size;
                            if (BinaryDeserializer.Deserialize(start, end, (char*)bufferStart, length, lengthSize) == end) return deserializeJson((char*)bufferStart, length, out value);
                        }
                    }
                    finally { buffer.Free(); }
                }
            }
            value = default(T);
            return false;
        }
        /// <summary>
        /// Get JSON deserialization
        /// 获取 JSON 反序列化
        /// </summary>
        /// <returns></returns>
        private JsonDeserializer createJsonDeserializer()
        {
            jsonDeserializer = AutoCSer.Threading.LinkPool<JsonDeserializer>.Default.Pop() ?? new AutoCSer.JsonDeserializer();
            jsonDeserializer.SetBinaryMix(isCheckRemoteType);
            return jsonDeserializer;
        }
        /// <summary>
        /// JSON deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        private unsafe static bool deserializeJson<T>(char* start, int length, out T? value)
#else
        private unsafe static bool deserializeJson<T>(char* start, int length, out T value)
#endif
        {
            value = default(T);
            if (length == 4 && *(long*)start == JsonDeserializer.NullStringValue) return true;
            return JsonDeserializer.UnsafeDeserializeBinaryMix(start, length, ref value).State == AutoCSer.Json.DeserializeStateEnum.Success;
        }
        /// <summary>
        /// JSON deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
#if NetStandard21
        private void deserializeJsonBuffer<T>(ref T? value)
#else
        private void deserializeJsonBuffer<T>(ref T value)
#endif
        {
            int length = *(int*)Current;
            if ((length & 1) == 0)
            {
                if (End - Current == (((long)length + (3 + sizeof(int))) & (long.MaxValue - 3)))
                {
                    char* json = (char*)(Current + sizeof(int));
                    if (length != 0 && (length != sizeof(long) || *(long*)json != JsonDeserializer.NullStringValue))
                    {
                        checkJsonDeserializeResult((jsonDeserializer ?? createJsonDeserializer()).Deserialize(json, length >> 1, ref value));
                    }
                    else value = default(T);
                    return;
                }
            }
            else
            {
                int lengthSize = UnmanagedStreamBase.GetSerializeStringLengthSize(length >>= 1);
                long size = End - Current;
                if (((lengthSize + length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= size)
                {
                    ByteArrayBuffer buffer = ByteArrayPool.GetBuffer(length << 1
#if DEBUG
            , false
#endif
            );
                    try
                    {
                        fixed (byte* bufferFixed = buffer.GetFixedBuffer())
                        {
                            byte* start = bufferFixed + buffer.StartIndex;
                            if (Deserialize(Current, End, (char*)start, length, lengthSize) == End)
                            {
                                if (length != (sizeof(long) >> 1) || *(long*)start != JsonDeserializer.NullStringValue)
                                {
                                    checkJsonDeserializeResult((jsonDeserializer ?? createJsonDeserializer()).Deserialize((char*)start, length, ref value));
                                }
                                else value = default(T);
                                return;
                            }
                        }
                    }
                    finally { buffer.Free(); }
                }
            }
            State = DeserializeStateEnum.IndexOutOfRange;
        }
        /// <summary>
        /// JSON deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool DeserializeJsonBuffer<T>(ref T? value)
#else
        internal bool DeserializeJsonBuffer<T>(ref T value)
#endif
        {
            byte* end = DeserializeBufferStart();
            if (end != null)
            {
                deserializeJsonBuffer(ref value);
                return DeserializeBufferEnd(end);
            }
            return false;
        }
        /// <summary>
        /// Deserialization of enumeration values
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value">Enumeration value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumByte<T>(BinaryDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(deserializer.Current);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, byte>.FromInt(*deserializer.Current);
#endif
            deserializer.Current += sizeof(int);
        }
        /// <summary>
        /// Deserialization of enumeration values
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value">Enumeration value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumSByte<T>(BinaryDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
            value = AutoCSer.Metadata.EnumGenericType<T, sbyte>.FromInt((sbyte)*(int*)deserializer.Current);
            deserializer.Current += sizeof(int);
        }
        /// <summary>
        /// Deserialization of enumeration values
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value">Enumeration value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumShort<T>(BinaryDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
            value = AutoCSer.Metadata.EnumGenericType<T, short>.FromInt((short)*(int*)deserializer.Current);
            deserializer.Current += sizeof(int);
        }
        /// <summary>
        /// Deserialization of enumeration values
        /// 枚举值反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value">Enumeration value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumUShort<T>(BinaryDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
#if NET8
            value = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(deserializer.Current);
#else
            value = AutoCSer.Metadata.EnumGenericType<T, ushort>.FromInt(*(ushort*)deserializer.Current);
#endif
            deserializer.Current += sizeof(int);
        }

        ///// <summary>
        ///// 读取4个字节
        ///// </summary>
        ///// <returns></returns>
        //[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //private int readInt()
        //{
        //    int value = *(int*)Read;
        //    Read += sizeof(int);
        //    return value;
        //}
        /// <summary>
        /// Logical value deserialization
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="value">Logical value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref bool value)
        {
            value = *(bool*)Current;
            Current += sizeof(int);
        }
        /// <summary>
        /// Logical value deserialization
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">Logical value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref bool value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="array">Array</param>
#if NetStandard21
        public void BinaryDeserialize(ref bool[]? array)
#else
        public void BinaryDeserialize(ref bool[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                if (((((long)length + (31 + 32)) >> 5) << 2) <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        DeserializeArrayMap arrayMap = new DeserializeArrayMap(Current + sizeof(int));
                        for (int index = 0; index != length; ++index) array[index] = arrayMap.Next() != 0;
                        Current = arrayMap.Read;
                    }
                }
                else State = DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">Array</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref bool[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref bool[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="array">Array</param>
#if NetStandard21
        public void BinaryDeserialize(ref ListArray<bool>? array)
#else
        public void BinaryDeserialize(ref ListArray<bool> array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                if (((((long)length + (31 + 32)) >> 5) << 2) <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        bool[] boolArray = array.Array.Array;
                        DeserializeArrayMap arrayMap = new DeserializeArrayMap(Current + sizeof(int));
                        for (int index = 0; index != length; ++index) boolArray[index] = arrayMap.Next() != 0;
                        Current = arrayMap.Read;
                    }
                }
                else State = DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">Array</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<bool>? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ListArray<bool> array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="array">Array</param>
        public void BinaryDeserialize(ref LeftArray<bool> array)
        {
            int length = *(int*)Current;
            if (length != 0)
            {
                if (((((long)length + (31 + 32)) >> 5) << 2) <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        bool[] boolArray = array.Array;
                        DeserializeArrayMap arrayMap = new DeserializeArrayMap(Current + sizeof(int));
                        for (int index = 0; index != length; ++index) boolArray[index] = arrayMap.Next() != 0;
                        Current = arrayMap.Read;
                    }
                }
                else State = DeserializeStateEnum.IndexOutOfRange;
            }
            else
            {
                array.SetEmpty();
                Current += sizeof(int);
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">Array</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref LeftArray<bool> array)
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// Logical value deserialization
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="value">Logical value</param>
        private void primitiveDeserialize(ref bool? value)
        {
            if (*(int*)Current != BinarySerializer.NullValue) value = *(bool*)Current;
            else value = null;
            Current += sizeof(int);
        }
        /// <summary>
        /// Logical value deserialization
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">Logical value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref bool? value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// Logical value deserialization
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="value">Logical value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref bool? value)
        {
            if (*Current == 0) value = null;
            else value = *Current == 2;
            ++Current;
        }
        /// <summary>
        /// Logical value deserialization
        /// 逻辑值反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">Logical value</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref bool? value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="array">Array</param>
#if NetStandard21
        public void BinaryDeserialize(ref bool?[]? array)
#else
        public void BinaryDeserialize(ref bool?[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                if (((((long)length + (31 + 32)) >> 5) << 2) <= End - Current)
                {
                    if (createArray(ref array, length >>= 1))
                    {
                        DeserializeArrayMap arrayMap = new DeserializeArrayMap(Current + sizeof(int), 2);
                        for (int index = 0; index != length; ++index) array[index] = arrayMap.NextBool();
                        Current = arrayMap.Read;
                    }
                }
                else State = DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">Array</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref bool?[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref bool?[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref byte value)
        {
            value = *Current;
            Current += sizeof(int);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref byte value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref byte? value)
        {
            if (*(Current + sizeof(byte)) == 0) value = *Current;
            else value = null;
            Current += sizeof(ushort);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref byte? value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref sbyte value)
        {
            value = *(sbyte*)Current;
            Current += sizeof(int);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref sbyte value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref sbyte? value)
        {
            if (*(Current + sizeof(byte)) == 0) value = *(sbyte*)Current;
            else value = null;
            Current += sizeof(ushort);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref sbyte? value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref short value)
        {
            value = *(short*)Current;
            Current += sizeof(int);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref short value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref short? value)
        {
            if (*(ushort*)(Current + sizeof(ushort)) == 0) value = *(short*)Current;
            else value = null;
            Current += sizeof(int);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref short? value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref ushort value)
        {
            value = *(ushort*)Current;
            Current += sizeof(int);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref ushort value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref ushort? value)
        {
            if (*(ushort*)(Current + sizeof(ushort)) == 0) value = *(ushort*)Current;
            else value = null;
            Current += sizeof(int);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref ushort? value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref char value)
        {
            value = *(char*)Current;
            Current += sizeof(int);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref char value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref char? value)
        {
            if (*(char*)(Current + sizeof(char)) == 0) value = *(char*)Current;
            else value = null;
            Current += sizeof(int);
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveMemberDeserialize(BinaryDeserializer deserializer, ref char? value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// Floating-point number deserialization
        /// 浮点数反序列化
        /// </summary>
        /// <param name="value">Floating-point number</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void primitiveDeserialize(ref Half value)
        {
            value = *(Half*)Current;
            Current += sizeof(int);
        }
        /// <summary>
        /// Floating-point number deserialization
        /// 浮点数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value">Floating-point number</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Half value)
        {
            deserializer.primitiveDeserialize(ref value);
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="array">Array</param>
#if NetStandard21
        public void BinaryDeserialize(ref Half[]? array)
#else
        public void BinaryDeserialize(ref Half[] array)
#endif
        {
            int length = deserializeArray(ref array);
            if (length != 0)
            {
                long size = ((long)length * sizeof(Half) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    if (createArray(ref array, length))
                    {
                        AutoCSer.Common.CopyTo(Current + sizeof(int), array);
                        Current += size;
                    }
                }
                else State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="array">Array</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Half[]? array)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Half[] array)
#endif
        {
            deserializer.BinaryDeserialize(ref array);
        }
        /// <summary>
        /// Deserialization from the data buffer (reading directly without checking the object reference)
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="getBuffer"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public int DeserializeBuffer(Func<int, Half[]?> getBuffer, out Half[]? buffer)
#else
        public int DeserializeBuffer(Func<int, Half[]> getBuffer, out Half[] buffer)
#endif
        {
            int length = *(int*)Current;
            if (length > 0)
            {
                long size = ((long)length * sizeof(Half) + (3 + sizeof(int))) & (long.MaxValue - 3);
                if (size <= End - Current)
                {
                    buffer = getBuffer(length);
                    if (buffer != null && buffer.Length >= length)
                    {
                        fixed (Half* bufferFixed = buffer) AutoCSer.Common.CopyTo(Current + sizeof(int), bufferFixed, (long)length * sizeof(Half));
                        Current += size;
                    }
                    else State = AutoCSer.BinarySerialize.DeserializeStateEnum.CustomBufferError;
                }
                else
                {
                    buffer = null;
                    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
            }
            else
            {
                if (length == 0) buffer = EmptyArray<Half>.Array;
                else
                {
                    buffer = null;
                    if (length != AutoCSer.BinarySerializer.NullValue) State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
                }
                Current += sizeof(int);
            }
            return length;
        }
        /// <summary>
        /// String deserialization
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="write">Write position</param>
        /// <param name="length">Write length</param>
        /// <param name="lengthSize">Write the byte size of the length data
        /// 写入长度数据的字节大小</param>
        /// <returns>Return null upon failure</returns>
        internal static byte* Deserialize(byte* start, byte* end, char* write, int length, int lengthSize)
        {
            byte* read = start + sizeof(int);
            char* writeEnd = write + length;
            int charCount;
            do
            {
                switch (lengthSize)
                {
                    case 1: length -= (charCount = *read++); break;
                    case sizeof(ushort):
                        length -= (charCount = *(ushort*)read);
                        read += sizeof(ushort);
                        if (length <= byte.MaxValue) lengthSize = 1;
                        break;
                    default:
                        length -= (charCount = *(int*)read);
                        read += sizeof(int);
                        //if (length <= ushort.MaxValue) lengthSize = length <= byte.MaxValue ? 1 : sizeof(ushort);
                        if (length <= ushort.MaxValue) lengthSize = 2 >> (length & (int.MaxValue - byte.MaxValue)).logicalInversion();
                        break;
                }
                byte* readEnd = read + charCount;
                if (length < 0 || readEnd > end || write + charCount > writeEnd) return null;
                while (read != readEnd) *write++ = (char)*read++;
                if (length == 0) return read + ((int)(start - read) & 3);
                switch (lengthSize)
                {
                    case 1: length -= (charCount = *read++); break;
                    case sizeof(ushort):
                        length -= (charCount = *(ushort*)read);
                        read += sizeof(ushort);
                        if (length <= byte.MaxValue) lengthSize = 1;
                        break;
                    default:
                        length -= (charCount = *(int*)read);
                        read += sizeof(int);
                        //if (length <= ushort.MaxValue) lengthSize = length <= byte.MaxValue ? 1 : sizeof(ushort);
                        if (length <= ushort.MaxValue) lengthSize = 2 >> (length & (int.MaxValue - byte.MaxValue)).logicalInversion();
                        break;
                }
                char* readCharEnd = (char*)read + charCount;
                if (length < 0 || readCharEnd > end || write + charCount > writeEnd) return null;
                while (read != readCharEnd)
                {
                    *write++ = *(char*)read;
                    read += sizeof(char);
                }
                if (length == 0) return read + ((int)(start - read) & 3);
            }
            while (true);
        }
        /// <summary>
        /// String deserialization
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        internal void Deserialize(ref string? value)
#else
        internal void Deserialize(ref string value)
#endif
        {
            int length = *(int*)Current;
            if ((length & 1) == 0)
            {
                if (length > 0)
                {
                    long dataLength = ((long)length + (3 + sizeof(int))) & (long.MaxValue - 3);
                    if (dataLength <= End - Current)
                    {
                        tryPush(value = new string((char*)(Current + sizeof(int)), 0, length >> 1));
                        Current += dataLength;
                    }
                    else if (length == BinarySerializer.NullValue)
                    {
                        value = null;
                        Current += sizeof(int);
                    }
                    else State = DeserializeStateEnum.IndexOutOfRange;
                }
                else if (length == 0)
                {
                    value = string.Empty;
                    Current += sizeof(int);
                }
            }
            else if ((length >>= 1) > 0)
            {
                int lengthSize = UnmanagedStreamBase.GetSerializeStringLengthSize(length);
                if (((lengthSize + length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= (int)(End - Current))
                {
                    tryPush(value = AutoCSer.Common.AllocateString(length));
                    fixed (char* valueFixed = value)
                    {
                        byte* read = Deserialize(Current, End, valueFixed, length, lengthSize);
                        if (read != null)
                        {
                            Current = read;
                            return;
                        }
                    }
                }
                State = DeserializeStateEnum.IndexOutOfRange;
            }
        }
        /// <summary>
        /// String deserialization
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void BinaryDeserialize(ref string? value)
#else
        public void BinaryDeserialize(ref string value)
#endif
        {
            if (CheckNullPoint(ref value)) Deserialize(ref value);
        }
        /// <summary>
        /// String deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref string? value)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref string value)
#endif
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// String deserialization
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void BinaryDeserialize(ref SubString value)
        {
            var stringValue = string.Empty;
            Deserialize(ref stringValue);
            if (stringValue != null) value.Set(stringValue, 0, stringValue.Length);
            else value = default(SubString);
        }
        /// <summary>
        /// String deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref SubString value)
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// Type deserialization
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public void BinaryDeserialize(ref Type? value)
#else
        public void BinaryDeserialize(ref Type value)
#endif
        {
            if (*(int*)Current < 0)
            {
                if (*(int*)Current == BinarySerializer.NullValue)
                {
                    value = null;
                    Current += sizeof(int);
                    return;
                }
                if (*(int*)Current == BinarySerializer.RealTypeValue)
                {
                    Current += sizeof(int);
                    var assemblyName = default(string);
                    BinaryDeserialize(ref assemblyName);
                    if (State == DeserializeStateEnum.Success && assemblyName != null)
                    {
                        var typeName = default(string);
                        BinaryDeserialize(ref typeName);
                        if (State == DeserializeStateEnum.Success && typeName != null)
                        {
                            AutoCSer.Reflection.RemoteType remoteType = new AutoCSer.Reflection.RemoteType(assemblyName, typeName);
                            if (State == DeserializeStateEnum.Success && remoteType.TryGet(out value, isCheckRemoteType)) return;
                        }
                    }
                }
                else
                {
                    getPoint(ref value);
                    return;
                }
            }
            State = DeserializeStateEnum.ErrorType;
        }
        /// <summary>
        /// Type deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Type? value)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref Type value)
#endif
        {
            deserializer.BinaryDeserialize(ref value);
        }
        /// <summary>
        /// object deserialization
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public void BinaryDeserialize(ref object? value)
#else
        public void BinaryDeserialize(ref object value)
#endif
        {
            if (*(int*)Current < 0)
            {
                if (*(int*)Current == BinarySerializer.NullValue)
                {
                    value = null;
                    Current += sizeof(int);
                    return;
                }
                if (*(int*)Current == BinarySerializer.NotNullValue)
                {
                    value = new object();
                    Current += sizeof(int);
                    return;
                }
                getPoint(ref value);
            }
            else State = DeserializeStateEnum.NotObject;
        }
        /// <summary>
        /// object deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref object? value)
#else
        private static void primitiveDeserialize(BinaryDeserializer deserializer, ref object value)
#endif
        {
            deserializer.BinaryDeserialize(ref value);
        }

        ///// <summary>
        ///// 获取反序列化数组位图
        ///// </summary>
        ///// <param name="bitCount"></param>
        ///// <param name="arrayMap"></param>
        ///// <returns></returns>
        //internal bool Get(int bitCount, out DeserializeArrayMap arrayMap)
        //{
        //    long bitSize = (((long)bitCount + 31) >> 5) << 2;
        //    if (bitSize <= End - Current)
        //    {
        //        arrayMap = new DeserializeArrayMap(Current);
        //        Current += bitSize;
        //        return true;
        //    }
        //    arrayMap = default(DeserializeArrayMap);
        //    State = DeserializeStateEnum.IndexOutOfRange;
        //    return false;
        //}
        /// <summary>
        /// Get and set the custom deserialization member bitmap type
        /// 获取并设置自定义反序列化成员位图类型
        /// </summary>
        /// <param name="memberMapType">The custom deserialization member bitmap type set
        /// 设置的自定义反序列化成员位图类型</param>
        /// <returns>Deserialize member bitmap type
        /// 反序列化成员位图类型</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public Type? SetCustomMemberMapType(Type memberMapType)
#else
        public Type SetCustomMemberMapType(Type memberMapType)
#endif
        {
            var oldMemberMapType = MemberMapType;
            MemberMapType = memberMapType;
            return oldMemberMapType;
        }
        /// <summary>
        /// Gets the custom deserialized member bitmap
        /// 获取自定义反序列化成员位图
        /// </summary>
        /// <param name="memberMapType">The custom deserialization member bitmap type set
        /// 设置的自定义反序列化成员位图类型</param>
        /// <returns>Deserialize member bitmap
        /// 反序列化成员位图</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public MemberMap? GetCustomMemberMap(Type? memberMapType)
#else
        public MemberMap GetCustomMemberMap(Type memberMapType)
#endif
        {
            var oldMemberMap = MemberMap;
            MemberMapType = memberMapType;
            MemberMap = null;
            return oldMemberMap;
        }
        /// <summary>
        /// Get and set the custom deserialization member bitmap
        /// 获取并设置自定义反序列化成员位图
        /// </summary>
        /// <param name="memberMap">The custom deserialization member bitmap set
        /// 设置的自定义反序列化成员位图</param>
        /// <returns>Deserialize member bitmap
        /// 反序列化成员位图</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public MemberMap? SetCustomMemberMap(MemberMap? memberMap)
#else
        public MemberMap SetCustomMemberMap(MemberMap memberMap)
#endif
        {
            var oldMemberMap = MemberMap;
            MemberMap = memberMap;
            return oldMemberMap;
        }
        /// <summary>
        /// Move the read data position and return to the position before the move
        /// 移动读取数据位置并返回移动之前的位置
        /// </summary>
        /// <param name="size"></param>
        /// <returns>Return null on failure</returns>
        internal byte* GetBeforeMove(int size)
        {
            byte* value = Current;
            if ((Current += size) <= End) return value;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
            return null;
        }
        /// <summary>
        /// Custom deserialization call
        /// 自定义反序列化调用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public bool CustomDeserialize<T>(ref T? value)
#else
        public bool CustomDeserialize<T>(ref T value)
#endif
        {
            if (CheckNullValue() == 0) value = default(T);
            else TypeDeserializer<T>.Deserialize(this, ref value);
            return State == DeserializeStateEnum.Success;
        }
        /// <summary>
        /// Deserialization from the data buffer (reading directly without checking the object reference)
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="isBuffer"></param>
        /// <returns></returns>
        public bool DeserializeBuffer(ref SubArray<byte> buffer, bool isBuffer)
        {
            int size = *(int*)Current;
            if (size > 0)
            {
                int dataSize = (size + 3) & (int.MaxValue - 3);
                if ((Current += sizeof(int)) + dataSize <= End)
                {
                    if (isBuffer && Buffer.Length != 0) buffer.Set(Buffer, (int)(Current - bufferFixed), size);
                    else buffer.Set(AutoCSer.Common.GetArray(Current, size), 0, size);
                    Current += dataSize;
                    return true;
                }
            }
            else if (size == 0)
            {
                buffer.SetEmpty();
                Current += sizeof(int);
                return true;
            }
            State = DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
        /// <summary>
        /// Deserialization from the data buffer (reading directly without checking the object reference)
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
#if NetStandard21
        public bool DeserializeBuffer(ref byte[]? buffer)
#else
        public bool DeserializeBuffer(ref byte[] buffer)
#endif
        {
            int size = *(int*)Current;
            if (size > 0)
            {
                int dataSize = (size + 3) & (int.MaxValue - 3);
                if ((Current += sizeof(int)) + dataSize <= End)
                {
                    buffer = AutoCSer.Common.GetArray(Current, size);
                    Current += dataSize;
                    return true;
                }
            }
            else if (size == 0)
            {
                buffer = EmptyArray<byte>.Array;
                Current += sizeof(int);
                return true;
            }
            else if (size == BinarySerializer.NullValue)
            {
                buffer = null;
                Current += sizeof(int);
                return true;
            }
            State = DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
        /// <summary>
        /// String deserialization
        /// </summary>
        /// <param name="start"></param>
        /// <param name="size">Multiples of 4
        /// 4 的倍数</param>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        internal unsafe static bool DeserializeString(byte* start, int size, out string? value)
#else
        internal unsafe static bool DeserializeString(byte* start, int size, out string value)
#endif
        {
            if (*(int*)start == BinarySerializer.NullValue)
            {
                value = null;
                return size == sizeof(int);
            }
            int length = *(int*)start;
            if ((length & 1) == 0)
            {
                if (length == 0)
                {
                    value = string.Empty;
                    return size == sizeof(int);
                }
                if (length == 10 && size == 14) size = 16;
                if (size == (((long)length + (3 + sizeof(int))) & (long.MaxValue - 3)))
                {
                    value = new string((char*)(start + sizeof(int)), 0, length >> 1);
                    return true;
                }
            }
            else if ((length >>= 1) > 0)
            {
                int lengthSize = UnmanagedStreamBase.GetSerializeStringLengthSize(length);
                if (((lengthSize + length + (3 + sizeof(int))) & (int.MaxValue - 3)) <= size)
                {
                    value = AutoCSer.Common.AllocateString(length);
                    fixed (char* valueFixed = value)
                    {
                        byte* end = start + size;
                        return BinaryDeserializer.Deserialize(start, end, valueFixed, length, lengthSize) == end;
                    }
                }
            }
            value = null;
            return false;
        }
        /// <summary>
        /// Deserialization from the data buffer (reading directly without checking the object reference)
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="value"></param>
#if NetStandard21
        public void DeserializeBuffer(ref string? value)
#else
        public void DeserializeBuffer(ref string value)
#endif
        {
            byte* end = DeserializeBufferStart();
            if (end != null)
            {
                DeserializeOnly(ref value);
                DeserializeBufferEnd(end);
            }
        }
        /// <summary>
        /// Deserialization from the data buffer (reading directly without checking the object reference)
        /// 从数据缓冲区反序列化（不检查对象引用直接读取）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
#if NetStandard21
        internal bool DeserializeOnly(ref string? value)
#else
        internal bool DeserializeOnly(ref string value)
#endif
        {
            int point = *(int*)Current;
            if (point < 0)
            {
                if (point == BinarySerializer.NullValue)
                {
                    value = null;
                    Current += sizeof(int);
                    return true;
                }
                State = DeserializeStateEnum.CustomBufferError;
                return false;
            }
            Deserialize(ref value);
            return State == DeserializeStateEnum.Success;
        }
        /// <summary>
        /// The custom deserialization data buffer begins processing
        /// 自定义反序列化数据缓冲区开始处理
        /// </summary>
        /// <returns></returns>
        internal byte* DeserializeBufferStart()
        {
            int size = *(int*)Current;
            if (size > 0)
            {
                byte* end = End;
                End = (Current += sizeof(int)) + ((size + 3) & (int.MaxValue - 3));
                if (End <= end) return end;
                else End = end;
            }
            State = DeserializeStateEnum.IndexOutOfRange;
            return null;
        }
        /// <summary>
        /// The custom deserialization data buffer has completed processing
        /// 自定义反序列化数据缓冲区结束处理
        /// </summary>
        /// <param name="end"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal bool DeserializeBufferEnd(byte* end)
        {
            Current = End;
            End = end;
            return State == DeserializeStateEnum.Success;
        }
        ///// <summary>
        ///// Custom serialization数据缓冲区
        ///// </summary>
        ///// <param name="deserializer"></param>
        //public bool DeserializeBuffer(Action<BinaryDeserializer> deserializer)
        //{
        //    int size = *(int*)Current;
        //    if (size > 0)
        //    {
        //        byte* end = End;
        //        End = (Current += sizeof(int)) + ((size + 3) & (int.MaxValue - 3));
        //        if (End <= end)
        //        {
        //            deserializer(this);
        //            Current = End;
        //            End = end;
        //            return State == DeserializeStateEnum.Success;
        //        }
        //        else End = end;
        //    }
        //    State = DeserializeStateEnum.IndexOutOfRange;
        //    return false;
        //}
        /// <summary>
        /// Deserialize independent objects from independent data buffers
        /// 从独立数据缓冲区反序列化独立对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool IndependentDeserialize<T>(ref SubArray<byte> data, ref T value) where T : struct
        {
            int size = data.Length;
            if ((size & 3) == 0)
            {
                int length = size - sizeof(int);
                if (length > 0)
                {
                    byte[] buffer = data.GetFixedBuffer();
                    fixed (byte* bufferFixed = buffer)
                    {
                        this.bufferFixed = bufferFixed;
                        start = bufferFixed + data.Start;
                        End = start + length;
                        if (*(int*)End == length)
                        {
                            uint headerValue = *(uint*)start;
                            if ((headerValue & BinarySerializeConfig.HeaderMapAndValue) == BinarySerializeConfig.HeaderMapValue)
                            {
                                DeserializeStateEnum checkState = checkHeaderValue(headerValue);
                                if (checkState != DeserializeStateEnum.Success) return false;
                                Buffer = buffer;
                                freeThreadStatic();
                                TypeDeserializer<T>.DefaultDeserializer(this, ref value);
                                return State == DeserializeStateEnum.Success && Current == End;
                            }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Deserialize independent objects from independent data buffers
        /// 从独立数据缓冲区反序列化独立对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool IndependentDeserialize<T>(byte[] data, ref T value) where T : struct
        {
            int size = data.Length;
            if ((size & 3) == 0)
            {
                int length = size - sizeof(int);
                if (length > 0)
                {
                    fixed (byte* bufferFixed = data)
                    {
                        this.bufferFixed = bufferFixed;
                        start = bufferFixed;
                        End = start + length;
                        if (*(int*)End == length)
                        {
                            uint headerValue = *(uint*)start;
                            if ((headerValue & BinarySerializeConfig.HeaderMapAndValue) == BinarySerializeConfig.HeaderMapValue)
                            {
                                DeserializeStateEnum checkState = checkHeaderValue(headerValue);
                                if (checkState != DeserializeStateEnum.Success) return false;
                                Buffer = data;
                                freeThreadStatic();
                                TypeDeserializer<T>.DefaultDeserializer(this, ref value);
                                return State == DeserializeStateEnum.Success && Current == End;
                            }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Deserialize the internal member object from the independent data buffer (with no reference check on the outer layer)
        /// 从独立数据缓冲区反序列化内部成员对象（外层无引用检查）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
#if NetStandard21
        internal bool InternalIndependentDeserializeNotReference<T>(ref T? value)
#else
        internal bool InternalIndependentDeserializeNotReference<T>(ref T value)
#endif
        {
            if (State == DeserializeStateEnum.Success)
            {
                long data = *(long*)Current;
                if (data != ((long)BinarySerializer.NullValue << 32) + sizeof(int))
                {
                    int size = (int)(uint)(ulong)data;
                    if (size > sizeof(int) && (size & 3) == 0)
                    {
                        byte* end = End;
                        End = (Current += sizeof(int)) + size;
                        if (End <= end && *(int*)(End -= sizeof(int)) == size - sizeof(int))
                        {
                            uint headerValue = (uint)((ulong)data >> 32);
                            if ((headerValue & BinarySerializeConfig.HeaderMapAndValue) == BinarySerializeConfig.HeaderMapValue)
                            {
                                byte* start = this.start;
                                this.start = Current;
                                byte* currentEnd = End;
                                DeserializeStateEnum checkState = checkHeaderValue(headerValue);
                                if (checkState == DeserializeStateEnum.Success)
                                {
                                    if (value == null && !Constructor(out value)) return false;
                                    TypeDeserializer<T>.DefaultDeserializer(this, ref value);
                                    if (State == DeserializeStateEnum.Success && Current == End)
                                    {
                                        this.start = start;
                                        Current = currentEnd + sizeof(int);
                                        End = end;
                                        return true;
                                    }
                                }
                                this.start = start;
                            }
                        }
                        End = end;
                    }
                }
                else if ((Current += sizeof(long)) <= End)
                {
                    value = default(T);
                    return true;
                }
                State = DeserializeStateEnum.IndexOutOfRange;
            }
            return false;
        }
        /// <summary>
        /// Deserialize the internal member object from the independent data buffer
        /// 从独立数据缓冲区反序列化内部成员对象
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="buffer">Data buffer</param>
        /// <param name="value">Target object</param>
        /// <returns></returns>
#if NetStandard21
        internal bool InternalIndependentDeserializeNotReference<T>(ref SubArray<byte> buffer, ref T? value)
#else
        internal bool InternalIndependentDeserializeNotReference<T>(ref SubArray<byte> buffer, ref T value)
#endif
        {
            fixed (byte* bufferFixed = buffer.GetFixedBuffer())
            {
                start = bufferFixed + buffer.Start;
                long data = *(long*)start;
                if (data != ((long)BinarySerializer.NullValue << 32) + sizeof(int))
                {
                    int size = (int)(uint)(ulong)data;
                    if (size > 0 && (size & 3) == 0 && size == buffer.Length - sizeof(int) && *(int*)(End = start + size) == size - sizeof(int))
                    {
                        uint headerValue = (uint)((ulong)data >> 32);
                        if ((headerValue & BinarySerializeConfig.HeaderMapAndValue) == BinarySerializeConfig.HeaderMapValue)
                        {
                            start += sizeof(int);
                            DeserializeStateEnum checkState = checkHeaderValue(headerValue);
                            if (checkState == DeserializeStateEnum.Success)
                            {
                                this.bufferFixed = bufferFixed;
                                this.Buffer = buffer.Array;
                                if (value == null && !Constructor(out value)) return false;
                                TypeDeserializer<T>.DefaultDeserializer(this, ref value);
                                return State == DeserializeStateEnum.Success && Current == End;
                            }
                        }
                    }
                }
                else if (buffer.Length == sizeof(long))
                {
                    value = default(T);
                    return true;
                }
            }
            State = DeserializeStateEnum.IndexOutOfRange;
            return false;
        }
        /// <summary>
        /// Deserialize the internal member object from the independent data buffer
        /// 从独立数据缓冲区反序列化内部成员对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        internal bool SimpleDeserialize<T>(ref T value) where T : struct
        {
            if (State == DeserializeStateEnum.Success)
            {
                int size = *(int*)Current;
                if (size > 0 && (size & 3) == 0)
                {
                    byte* end = (Current += sizeof(int)) + size;
                    if (end <= End && SimpleSerialize.Deserializer<T>.DefaultDeserializer(Current, ref value, end) == end)
                    {
                        Current = end;
                        return true;
                    }
                }
                State = DeserializeStateEnum.IndexOutOfRange;
            }
            return false;
        }
        /// <summary>
        /// Deserialize the internal member object from the independent data buffer
        /// 从独立数据缓冲区反序列化内部成员对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer">Data buffer</param>
        /// <param name="value"></param>
        public bool SimpleDeserialize<T>(ref SubArray<byte> buffer, ref T value) where T : struct
        {
            fixed (byte* bufferFixed = buffer.GetFixedBuffer())
            {
                byte* start = bufferFixed + buffer.Start;
                int size = *(int*)start;
                if (size > 0 && (size & 3) == 0 && size == buffer.Length - sizeof(int))
                {
                    start += sizeof(int);
                    byte* end = start + size;
                    return SimpleSerialize.Deserializer<T>.DefaultDeserializer(start, ref value, end) == end;
                }
            }
            return false;
        }
        /// <summary>
        /// Deserialize the internal member object from the independent data buffer
        /// 从独立数据缓冲区反序列化内部成员对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer">Data buffer</param>
        /// <param name="value"></param>
        internal bool SimpleDeserialize<T>(byte[] buffer, ref T value) where T : struct
        {
            fixed (byte* bufferFixed = buffer)
            {
                byte* start = bufferFixed;
                int size = *(int*)start;
                if (size > 0 && (size & 3) == 0 && size == buffer.Length - sizeof(int))
                {
                    start += sizeof(int);
                    byte* end = start + size;
                    return SimpleSerialize.Deserializer<T>.DefaultDeserializer(start, ref value, end) == end;
                }
            }
            return false;
        }
        ///// <summary>
        ///// 自定义反序列化调用
        ///// </summary>
        ///// <param name="buffer"></param>
        ///// <returns></returns>
        //public unsafe bool CustomDeserialize(out ByteArrayBuffer buffer)
        //{
        //    int size = *(int*)Current;
        //    if (size > 0)
        //    {
        //        if ((Current += sizeof(int)) + size <= End)
        //        {
        //            buffer = ByteArrayPool.GetBuffer(size);
        //            buffer.CopyFrom(Current, size);
        //            Current += size;
        //            return true;
        //        }
        //    }
        //    else if (size == 0)
        //    {
        //        buffer = default(ByteArrayBuffer);
        //        Current += sizeof(int);
        //        return true;
        //    }
        //    State = DeserializeStateEnum.IndexOutOfRange;
        //    buffer = default(ByteArrayBuffer);
        //    return false;
        //}
        ///// <summary>
        ///// 读取字节缓冲区序列化字节数
        ///// </summary>
        ///// <returns>失败返回负数</returns>
        //public int ReadBufferSize()
        //{
        //    int size = *(int*)Current;
        //    long length = ((long)size + (3 + sizeof(int))) & (long.MaxValue - 3);
        //    if (length <= End - Current) return size;
        //    State = AutoCSer.BinarySerialize.DeserializeState.IndexOutOfRange;
        //    return -1;
        //}
        ///// <summary>
        ///// 读取字节缓冲区序列化字节数
        ///// </summary>
        ///// <returns>失败返回-1</returns>
        //public int ReadBufferSizeNull()
        //{
        //    int size = *(int*)Current;
        //    if (size != BinarySerializer.NullValue)
        //    {
        //        long length = ((long)size + (3 + sizeof(int))) & (long.MaxValue - 3);
        //        if (length <= End - Current) return size;
        //        State = AutoCSer.BinarySerialize.DeserializeState.IndexOutOfRange;
        //        return -1;
        //    }
        //    if ((Current += sizeof(int)) <= End) return size;
        //    State = AutoCSer.BinarySerialize.DeserializeState.IndexOutOfRange;
        //    return -1;
        //}
        ///// <summary>
        ///// 获取字节缓冲区序列化数据
        ///// </summary>
        ///// <param name="buffer"></param>
        ///// <param name="size"></param>
        ///// <returns></returns>
        //public bool Read(ref SubArray<byte> buffer, int size)
        //{
        //    long length = ((long)size + 3) & (long.MaxValue - 3);
        //    if (length <= End - Current)
        //    {
        //        AutoCSer.Common.Config.CopyTo(Current, buffer.Array, buffer.Start, size);
        //        Current += length;
        //        return true;
        //    }
        //    State = AutoCSer.BinarySerialize.DeserializeState.IndexOutOfRange;
        //    return false;
        //}
        ///// <summary>
        ///// 获取字节缓冲区序列化数据
        ///// </summary>
        ///// <param name="buffer"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //public bool Read(byte[] buffer)
        //{
        //    return read(buffer, buffer.Length);
        //}
        ///// <summary>
        ///// 获取字节缓冲区序列化数据
        ///// </summary>
        ///// <param name="buffer"></param>
        ///// <param name="size"></param>
        ///// <returns></returns>
        //private bool read(byte[] buffer, int size)
        //{
        //    long length = ((long)size + 3) & (long.MaxValue - 3);
        //    if (length <= End - Current)
        //    {
        //        AutoCSer.Common.Config.CopyTo(Current, buffer);
        //        Current += length;
        //        return true;
        //    }
        //    State = AutoCSer.BinarySerialize.DeserializeState.IndexOutOfRange;
        //    return false;
        //}
        /// <summary>
        /// Set the custom error status for deserialization
        /// 设置反序列化自定义错误状态
        /// </summary>
        /// <param name="customError">Customize deserialization error messages</param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool SetCustomError(string customError)
        {
            if (State == DeserializeStateEnum.Success)
            {
                State = DeserializeStateEnum.CustomError;
                this.customError = customError;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Set the error status
        /// 设置错误状态
        /// </summary>
        /// <param name="state"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetErrorState(DeserializeStateEnum state)
        {
            if (State == DeserializeStateEnum.Success) State = state;
        }
        ///// <summary>
        ///// 设置反序列化自定义错误状态
        ///// </summary>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal void SetCustomError(JsonDeserializer jsonDeserializer)
        //{
        //    if (State == DeserializeStateEnum.Success)
        //    {
        //        State = DeserializeStateEnum.JsonError;
        //        jsonState = jsonDeserializer.State;
        //        customError = jsonDeserializer.customError;
        //    }
        //}
        ///// <summary>
        ///// 读取数据（不移动读取位置）
        ///// </summary>
        ///// <returns></returns>
        //public bool CheckRead(out int value)
        //{
        //    value = *(int*)Current;
        //    if ((Current + sizeof(int)) <= End) return true;
        //    State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
        //    return false;
        //}
#if AOT
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeHalfArray(BinaryDeserializer deserializer)
        {
            var array = default(Half[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// Logical value deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeNullableBool(BinaryDeserializer deserializer)
        {
            bool? value = default(bool?);
            deserializer.BinaryDeserialize(ref value);
            return value.castObject();
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeBoolArray(BinaryDeserializer deserializer)
        {
            var array = default(bool[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeNullableBoolArray(BinaryDeserializer deserializer)
        {
            var array = default(bool?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeBoolLeftArray(BinaryDeserializer deserializer)
        {
            var array = default(LeftArray<bool>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeBoolListArray(BinaryDeserializer deserializer)
        {
            var array = default(ListArray<bool>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeNullableByte(BinaryDeserializer deserializer)
        {
            var value = default(byte?);
            deserializer.BinaryDeserialize(ref value);
            return value.castObject();
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeNullableSByte(BinaryDeserializer deserializer)
        {
            var value = default(sbyte?);
            deserializer.BinaryDeserialize(ref value);
            return value.castObject();
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeNullableShort(BinaryDeserializer deserializer)
        {
            var value = default(short?);
            deserializer.BinaryDeserialize(ref value);
            return value.castObject();
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeNullableUShort(BinaryDeserializer deserializer)
        {
            var value = default(ushort?);
            deserializer.BinaryDeserialize(ref value);
            return value.castObject();
        }
        /// <summary>
        /// Integer deserialization
        /// 整数反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeNullableChar(BinaryDeserializer deserializer)
        {
            var value = default(char?);
            deserializer.BinaryDeserialize(ref value);
            return value.castObject();
        }
        /// <summary>
        /// String deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeString(BinaryDeserializer deserializer)
        {
            var value = default(string);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
        /// <summary>
        /// String deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        private static object primitiveMemberDeserializeSubString(BinaryDeserializer deserializer)
        {
            var value = default(SubString);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
        /// <summary>
        /// Type deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeType(BinaryDeserializer deserializer)
        {
            var value = default(Type);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
        /// <summary>
        /// String deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        private static object? primitiveMemberDeserializeObject(BinaryDeserializer deserializer)
        {
            var value = default(object);
            deserializer.BinaryDeserialize(ref value);
            return value;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object NullableLeftArrayReflection<T>(BinaryDeserializer deserializer) where T : struct
        {
            var array = default(LeftArray<T?>);
            deserializer.NullableArray(ref array);
            return array;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object StructLeftArrayReflection<T>(BinaryDeserializer deserializer) where T : struct
        {
            var array = default(LeftArray<T>);
            deserializer.StructArray(ref array);
            return array;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object LeftArrayReflection<T>(BinaryDeserializer deserializer) where T : class
        {
            var array = default(LeftArray<T?>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? NullableListArrayReflection<T>(BinaryDeserializer deserializer) where T : struct
        {
            var array = default(ListArray<T?>);
            deserializer.NullableArray(ref array);
            return array;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? StructListArrayReflection<T>(BinaryDeserializer deserializer) where T : struct
        {
            var array = default(ListArray<T>);
            deserializer.StructArray(ref array);
            return array;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? ListArrayReflection<T>(BinaryDeserializer deserializer) where T : class
        {
            var array = default(ListArray<T?>);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? NullableArrayReflection<T>(BinaryDeserializer deserializer) where T : struct
        {
            var array = default(T?[]);
            deserializer.NullableArray(ref array);
            return array;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? StructArrayReflection<T>(BinaryDeserializer deserializer) where T : struct
        {
            var array = default(T[]);
            deserializer.StructArray(ref array);
            return array;
        }
        /// <summary>
        /// Array deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? ArrayReflection<T>(BinaryDeserializer deserializer) where T : class
        {
            var array = default(T?[]);
            deserializer.BinaryDeserialize(ref array);
            return array;
        }
        /// <summary>
        /// Custom deserialization not supported types
        /// 自定义反序列化不支持的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? NotSupportReflection<T>(BinaryDeserializer deserializer)
        {
            var value = default(T);
            BinarySerializer.CustomConfig.NotSupport(deserializer, ref value);
            return value;
        }
        /// <summary>
        /// Object deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object NullableReflection<T>(BinaryDeserializer deserializer) where T : struct
        {
            var value = default(T?);
            deserializer.BinaryDeserialize(ref value);
            return value.castObject();
        }
        /// <summary>
        /// Dictionary deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="deserializer"></param>
        public static object? DictionaryReflection<T, KT, VT>(BinaryDeserializer deserializer)
            where T : IDictionary<KT, VT>
        {
            var dictionary = default(T);
            deserializer.Dictionary<T, KT, VT>(ref dictionary);
            return dictionary;
        }
        /// <summary>
        /// Collection deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="deserializer"></param>
        public static object? CollectionReflection<T, VT>(BinaryDeserializer deserializer) where T : ICollection<VT?>
        {
            var collection = default(T);
            deserializer.Collection<T, VT>(ref collection);
            return collection;
        }
        /// <summary>
        /// Base type deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="BT"></typeparam>
        /// <param name="deserializer"></param>
        public static object? BaseReflection<T, BT>(BinaryDeserializer deserializer)
            where T : class, BT
        {
            var value = default(T);
            deserializer.baseDeserialize<T, BT>(ref value);
            return value;
        }
        /// <summary>
        /// JSON mixed binary deserialization
        /// JSON 混杂二进制 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static object? StructJsonReflection<T>(BinaryDeserializer deserializer)
        {
            var value = default(T);
            deserializer.StructJson(ref value);
            return value;
        }
        /// <summary>
        /// Binary deserialization to simple deserialization (for AOT code generation, not allowed for developers to call)
        /// 二进制反序列化转简单反序列化（用于 AOT 代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static object? SimpleReflection<T>(BinaryDeserializer deserializer) where T : struct
        {
            var value = default(T);
            deserializer.SimpleDeserialize(ref value);
            return value;
        }
        /// <summary>
        /// JSON mixed binary deserialization
        /// JSON 混杂二进制 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? JsonReflection<T>(BinaryDeserializer deserializer)
        {
            var value = default(T);
            deserializer.Json(ref value);
            return value;
        }
        /// <summary>
        /// Deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        public static object? DeserializeReflection<T>(BinaryDeserializer deserializer)
        {
            var value = default(T);
            TypeDeserializer<T>.Deserialize(deserializer, ref value);
            return value;
        }
        /// <summary>
        /// Custom deserialization
        /// </summary>
        /// <param name="deserializer"></param>
        public static object ICustomReflection<T>(AutoCSer.BinaryDeserializer deserializer) where T : ICustomSerialize<T>
        {
            var value = default(T);
            if (deserializer.Constructor(out value)) value.Deserialize(deserializer);
            return value.castObject();
        }
        /// <summary>
        /// Enumeration deserialization
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumULong<T>(BinaryDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
            EnumULongMember<T>(deserializer, ref value);
        }
        /// <summary>
        /// Enumeration deserialization
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumLong<T>(BinaryDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
            EnumLongMember<T>(deserializer, ref value);
        }
        /// <summary>
        /// Enumeration deserialization
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumUInt<T>(BinaryDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
            EnumUIntMember<T>(deserializer, ref value);
        }
        /// <summary>
        /// Enumeration deserialization
        /// 枚举反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void EnumInt<T>(BinaryDeserializer deserializer, ref T value) where T : struct, IConvertible
        {
            EnumIntMember<T>(deserializer, ref value);
        }
        /// <summary>
        /// Binary deserialization to simple deserialization (for AOT code generation, not allowed for developers to call)
        /// 二进制反序列化转简单反序列化（用于 AOT 代码生成，不允许开发者调用）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Simple<T>(ref T value) where T : struct
        {
            SimpleDeserialize(ref value);
        }
        /// <summary>
        /// Custom deserialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo ICustomMethod;
        /// <summary>
        /// Base type deserialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo BaseMethod;
        /// <summary>
        /// Unsupported type deserialization
        /// 不支持的类型反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NotSupportMethod;
        /// <summary>
        /// Deserialization of nullable arrays
        /// 可空数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableArrayMethod;
        /// <summary>
        /// Deserialization of nullable arrays
        /// 可空数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableLeftArrayMethod;
        /// <summary>
        /// Deserialization of nullable arrays
        /// 可空数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableListArrayMethod;
        /// <summary>
        /// Deserialization of value type arrays
        /// 值类型数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo StructArrayMethod;
        /// <summary>
        /// Deserialization of value type arrays
        /// 值类型数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo StructLeftArrayMethod;
        /// <summary>
        /// Deserialization of value type arrays
        /// 值类型数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo StructListArrayMethod;
        /// <summary>
        /// Deserialization of reference type arrays
        /// 引用类型数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo ArrayMethod;
        /// <summary>
        /// Deserialization of reference type arrays
        /// 引用类型数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo LeftArrayMethod;
        /// <summary>
        /// Deserialization of reference type arrays
        /// 引用类型数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo ListArrayMethod;
        /// <summary>
        /// Nullable data deserialization
        /// 可空数据反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableMethod;
        /// <summary>
        /// Dictionary deserialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo DictionaryMethod;
        /// <summary>
        /// Collection deserialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo CollectionMethod;
        /// <summary>
        /// JSON deserialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo JsonMethod;
        /// <summary>
        /// JSON deserialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo StructJsonMethod;
        /// <summary>
        /// Simple data deserialization
        /// 简单数据反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo SimpleMethod;
        /// <summary>
        /// Custom deserialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo ICustomReflectionMethod;
        /// <summary>
        /// Base type deserialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo BaseReflectionMethod;
        /// <summary>
        /// Unsupported type deserialization
        /// 不支持的类型反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NotSupportReflectionMethod;
        /// <summary>
        /// Deserialization of nullable arrays
        /// 可空数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableArrayReflectionMethod;
        /// <summary>
        /// Deserialization of nullable arrays
        /// 可空数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableLeftArrayReflectionMethod;
        /// <summary>
        /// Deserialization of nullable arrays
        /// 可空数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableListArrayReflectionMethod;
        /// <summary>
        /// Deserialization of value type arrays
        /// 值类型数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo StructArrayReflectionMethod;
        /// <summary>
        /// Deserialization of value type arrays
        /// 值类型数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo StructLeftArrayReflectionMethod;
        /// <summary>
        /// Deserialization of value type arrays
        /// 值类型数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo StructListArrayReflectionMethod;
        /// <summary>
        /// Deserialization of reference type arrays
        /// 引用类型数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo ArrayReflectionMethod;
        /// <summary>
        /// Deserialization of reference type arrays
        /// 引用类型数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo LeftArrayReflectionMethod;
        /// <summary>
        /// Deserialization of reference type arrays
        /// 引用类型数组反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo ListArrayReflectionMethod;
        /// <summary>
        /// Nullable data deserialization
        /// 可空数据反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo NullableReflectionMethod;
        /// <summary>
        /// Dictionary deserialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo DictionaryReflectionMethod;
        /// <summary>
        /// Collection deserialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo CollectionReflectionMethod;
        /// <summary>
        /// JSON deserialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo JsonReflectionMethod;
        /// <summary>
        /// JSON deserialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo StructJsonReflectionMethod;
        /// <summary>
        /// Simple data deserialization
        /// 简单数据反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo SimpleReflectionMethod;
        /// <summary>
        /// Deserialization
        /// </summary>
        internal static readonly System.Reflection.MethodInfo DeserializeReflectionMethod;
        /// <summary>
        /// object reflection deserialization
        /// object 反射反序列化
        /// </summary>
        internal static readonly System.Reflection.MethodInfo RealTypeObjectMethod;
        /// <summary>
        /// AOT deserialization template call
        /// AOT 反序列化模板调用
        /// </summary>
        /// <returns></returns>
        internal object DeserializeMethodName() { return this; }
        /// <summary>
        /// AOT deserialization template call
        /// AOT 反序列化模板
        /// </summary>
        /// <param name="value"></param>
        internal void DeserializeMethodName(ref string value) { }
        /// <summary>
        /// AOT deserialization template call
        /// AOT 反序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        internal void DeserializeMethodName<T>(ref string value) { }
        /// <summary>
        /// AOT deserialization template call
        /// AOT 反序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer"></param>
        internal static void ReflectionMethodName<T>(JsonDeserializer deserializer) { }
        /// <summary>
        /// AOT deserialization template call
        /// AOT 反序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        internal static void ReflectionMethodName<T, VT>(JsonDeserializer deserializer, ref VT value) { }
        /// <summary>
        /// AOT deserialization template call
        /// AOT 反序列化模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        internal void SimpleMethodName<T>(ref T value) { }
#else
        ///// <summary>
        ///// Set the current position for reading data
        ///// 设置当前读取数据位置
        ///// </summary>
        ///// <param name="serializeInfo"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal void SetCurrent(ref AutoCSer.Net.CommandServer.RemoteExpression.SerializeInfo serializeInfo)
        //{
        //    Current = serializeInfo.End;
        //}
        ///// <summary>
        ///// Set the current position for reading data
        ///// 设置当前读取数据位置
        ///// </summary>
        ///// <param name="current"></param>
        ///// <param name="end"></param>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal void SetCurrent(byte* current, byte* end)
        //{
        //    Current = current;
        //    End = end;
        //}
        ///// <summary>
        ///// Set the current position for reading data
        ///// 设置当前读取数据位置
        ///// </summary>
        ///// <param name="current"></param>
        ///// <returns></returns>
        //[MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal bool TrySetCurrent(byte* current)
        //{
        //    if (current != null && current <= End)
        //    {
        //        Current = current;
        //        return true;
        //    }
        //    SetIndexOutOfRange();
        //    return false;
        //}
        /// <summary>
        /// Set the error of insufficient data length
        /// 设置数据长度不足错误
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal void SetIndexOutOfRange()
        {
            Current = End;
            State = AutoCSer.BinarySerialize.DeserializeStateEnum.IndexOutOfRange;
        }
        /// <summary>
        /// Fix the number of packet padding bytes
        /// 固定分组填充字节数
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="fixedFillSize"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void FixedFillSize(BinaryDeserializer deserializer, int fixedFillSize)
        {
            deserializer.Current += fixedFillSize;
        }
        /// <summary>
        /// Set the starting position of the fixed data
        /// 设置固定数据起始位置
        /// </summary>
        /// <param name="deserializer"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetFixedCurrent(BinaryDeserializer deserializer)
        {
            deserializer.SetFixedCurrent();
        }
        /// <summary>
        /// Set the fixed data end position
        /// 设置固定数据结束位置
        /// </summary>
        /// <param name="deserializer"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SetFixedCurrentEnd(BinaryDeserializer deserializer)
        {
            deserializer.SetFixedCurrentEnd();
        }
        /// <summary>
        /// JSON deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        internal void DeserializeJsonNotNullCheckZore<T>(ref T value)
        {
            int size = *(int*)Current;
#pragma warning disable CS8601
            if (size != 0) deserializeJson(ref value, size);
#pragma warning restore CS8601
            else Current += sizeof(int);
        }
        /// <summary>
        /// AOT code generation template
        /// AOT 代码生成模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal static void BinaryDeserialize<T>() { }
#endif
        /// <summary>
        /// Fixed packet fill byte count (for AOT code generation, not allowed for developers to call)
        /// 固定分组填充字节数（用于 AOT 代码生成，不允许开发者调用）
        /// </summary>
        /// <param name="fixedFillSize"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public void FixedFillSize(int fixedFillSize)
#else
        internal void FixedFillSize(int fixedFillSize)
#endif
        {
            Current += fixedFillSize;
        }
        /// <summary>
        /// Set the fixed data starting position (for AOT code generation and not allowed for developers to call)
        /// 设置固定数据起始位置（用于 AOT 代码生成，不允许开发者调用）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public void SetFixedCurrent()
#else
        internal void SetFixedCurrent()
#endif
        {
            fixedCurrent = Current;
        }
        /// <summary>
        /// Set the fixed data end position (for AOT code generation and not allowed for developers to call)
        /// 设置固定数据结束位置（用于 AOT 代码生成，不允许开发者调用）
        /// </summary>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if AOT
        public void SetFixedCurrentEnd()
#else
        internal void SetFixedCurrentEnd()
#endif
        {
            Current += (int)(fixedCurrent - Current) & 3;
        }
        /// <summary>
        /// 读取数据缓冲区起始与结束位置信息
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal bool ReadBuffer(out byte* start, out byte* end)
        {
            int size = *(int*)Current;
            start = Current + sizeof(int);
            end = start + size;
            if (size > 0 && end <= End) return true;
            State = DeserializeStateEnum.IndexOutOfRange;
            return false;
        }

        /// <summary>
        /// Deserialization
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="buffer">Data buffer</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Target object</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? Deserialize<T>(byte[] buffer, DeserializeConfig? config = null)
#else
        public static T Deserialize<T>(byte[] buffer, DeserializeConfig config = null)
#endif
        {
            var value = default(T);
            return Deserialize(buffer, ref value, config).State == DeserializeStateEnum.Success ? value : default(T);
        }
        /// <summary>
        /// Deserialization
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="buffer">Data buffer</param>
        /// <param name="value">Target object</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns></returns>
#if NetStandard21
        public static DeserializeResult Deserialize<T>(byte[] buffer, ref T? value, DeserializeConfig? config = null)
#else
        public static DeserializeResult Deserialize<T>(byte[] buffer, ref T value, DeserializeConfig config = null)
#endif
        {
            if (buffer == null) return new DeserializeResult(DeserializeStateEnum.UnknownData);
            fixed (byte* bufferFixed = buffer)
            {
                BinaryDeserializer deserializer = AutoCSer.Threading.LinkPool<BinaryDeserializer>.Default.Pop() ?? new BinaryDeserializer();
                try
                {
                    return deserializer.deserialize(buffer, bufferFixed, bufferFixed, buffer.Length, ref value, config);
                }
                finally { deserializer.Free(); }
            }
        }
        /// <summary>
        /// Deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <param name="config"></param>
        /// <returns></returns>
#if NetStandard21
        internal static DeserializeResult UnsafeDeserialize<T>(byte* buffer, int size, ref T? value, DeserializeConfig? config = null)
#else
        internal static DeserializeResult UnsafeDeserialize<T>(byte* buffer, int size, ref T value, DeserializeConfig config = null)
#endif
        {
            BinaryDeserializer deserializer = AutoCSer.Threading.LinkPool<BinaryDeserializer>.Default.Pop() ?? new BinaryDeserializer();
            try
            {
                return deserializer.deserialize(null, buffer, buffer, size, ref value, config);
            }
            finally { deserializer.Free(); }
        }
        /// <summary>
        /// Deserialize independent objects from independent data buffers
        /// 从独立数据缓冲区反序列化独立对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool IndependentDeserializeBuffer<T>(byte[] buffer, ref T value) where T : struct
        {
            BinaryDeserializer deserializer = AutoCSer.Threading.LinkPool<BinaryDeserializer>.Default.Pop() ?? new BinaryDeserializer();
            try
            {
                return deserializer.IndependentDeserialize(buffer, ref value);
            }
            finally { deserializer.Free(); }
        }
        /// <summary>
        /// Deserialize independent objects from independent data buffers
        /// 从独立数据缓冲区反序列化独立对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static bool SimpleDeserializeBuffer<T>(byte[] buffer, ref T value) where T : struct
        {
            BinaryDeserializer deserializer = AutoCSer.Threading.LinkPool<BinaryDeserializer>.Default.Pop() ?? new BinaryDeserializer();
            try
            {
                return deserializer.SimpleDeserialize(buffer, ref value);
            }
            finally { deserializer.Free(); }
        }

        /// <summary>
        /// Deserialization (Thread static Instance pattern)
        /// 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="buffer">Data buffer</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns>Target object</returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        public static T? ThreadStaticDeserialize<T>(byte[] buffer, DeserializeConfig? config = null)
#else
        public static T ThreadStaticDeserialize<T>(byte[] buffer, DeserializeConfig config = null)
#endif
        {
            var value = default(T);
            return ThreadStaticDeserialize(buffer, ref value, config).State == DeserializeStateEnum.Success ? value : default(T);
        }
        /// <summary>
        /// Deserialization (Thread static Instance pattern)
        /// 反序列化（线程静态实例模式）
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="buffer">Data buffer</param>
        /// <param name="value">Target object</param>
        /// <param name="config">Configuration parameters</param>
        /// <returns></returns>
#if NetStandard21
        public static DeserializeResult ThreadStaticDeserialize<T>(byte[] buffer, ref T? value, DeserializeConfig? config = null)
#else
        public static DeserializeResult ThreadStaticDeserialize<T>(byte[] buffer, ref T value, DeserializeConfig config = null)
#endif
        {
            if (buffer == null) return new DeserializeResult(DeserializeStateEnum.UnknownData);
            fixed (byte* bufferFixed = buffer)
            {
                BinaryDeserializer deserializer = ThreadStaticDeserializer.Get().Deserializer;
                try
                {
                    return deserializer.deserialize(buffer, bufferFixed, bufferFixed, buffer.Length, ref value, config);
                }
                finally { deserializer.freeThreadStatic(); }

            }
        }

        /// <summary>
        /// Deserialization delegation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer">Binary data deserialization</param>
        /// <param name="value">Target data</param>
        public delegate void DeserializeDelegate<T>(BinaryDeserializer deserializer, ref T value);
        /// <summary>
        /// Basic type deserialization delegate collection
        /// 基本类型反序列化委托集合
        /// </summary>
        internal static readonly Dictionary<HashObject<System.Type>, DeserializeDelegate> DeserializeDelegates;

        static BinaryDeserializer()
        {
            DeserializeDelegates = DictionaryCreator.CreateHashObject<System.Type, DeserializeDelegate>();
#if AOT
            DeserializeDelegates.Add(typeof(bool), new DeserializeDelegate((DeserializeDelegate<bool>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeBool));
            DeserializeDelegates.Add(typeof(bool?), new DeserializeDelegate((DeserializeDelegate<bool?>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeNullableBool));
            DeserializeDelegates.Add(typeof(bool[]), new DeserializeDelegate((DeserializeDelegate<bool[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeBoolArray, true));
            DeserializeDelegates.Add(typeof(bool?[]), new DeserializeDelegate((DeserializeDelegate<bool?[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeNullableBoolArray, true));
            DeserializeDelegates.Add(typeof(LeftArray<bool>), new DeserializeDelegate((DeserializeDelegate<LeftArray<bool>>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeBoolLeftArray, true));
            DeserializeDelegates.Add(typeof(ListArray<bool>), new DeserializeDelegate((DeserializeDelegate<ListArray<bool>?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeBoolListArray, true));
            DeserializeDelegates.Add(typeof(byte), new DeserializeDelegate((DeserializeDelegate<byte>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeByte));
            DeserializeDelegates.Add(typeof(byte?), new DeserializeDelegate((DeserializeDelegate<byte?>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeNullableByte));
            DeserializeDelegates.Add(typeof(byte[]), new DeserializeDelegate((DeserializeDelegate<byte[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeByteArray, true));
            DeserializeDelegates.Add(typeof(byte?[]), new DeserializeDelegate((DeserializeDelegate<byte?[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeNullableByteArray, true));
            DeserializeDelegates.Add(typeof(LeftArray<byte>), new DeserializeDelegate((DeserializeDelegate<LeftArray<byte>>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeByteLeftArray, true));
            DeserializeDelegates.Add(typeof(ListArray<byte>), new DeserializeDelegate((DeserializeDelegate<ListArray<byte>?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeByteListArray, true));
            DeserializeDelegates.Add(typeof(sbyte), new DeserializeDelegate((DeserializeDelegate<sbyte>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeSByte));
            DeserializeDelegates.Add(typeof(sbyte?), new DeserializeDelegate((DeserializeDelegate<sbyte?>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeNullableSByte));
            DeserializeDelegates.Add(typeof(sbyte[]), new DeserializeDelegate((DeserializeDelegate<sbyte[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeSByteArray, true));
            DeserializeDelegates.Add(typeof(sbyte?[]), new DeserializeDelegate((DeserializeDelegate<sbyte?[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeNullableSByteArray, true));
            DeserializeDelegates.Add(typeof(LeftArray<sbyte>), new DeserializeDelegate((DeserializeDelegate<LeftArray<sbyte>>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeSByteLeftArray, true));
            DeserializeDelegates.Add(typeof(ListArray<sbyte>), new DeserializeDelegate((DeserializeDelegate<ListArray<sbyte>?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeSByteListArray, true));
            DeserializeDelegates.Add(typeof(short), new DeserializeDelegate((DeserializeDelegate<short>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeShort));
            DeserializeDelegates.Add(typeof(short?), new DeserializeDelegate((DeserializeDelegate<short?>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeNullableShort));
            DeserializeDelegates.Add(typeof(short[]), new DeserializeDelegate((DeserializeDelegate<short[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeShortArray, true));
            DeserializeDelegates.Add(typeof(short?[]), new DeserializeDelegate((DeserializeDelegate<short?[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeNullableShortArray, true));
            DeserializeDelegates.Add(typeof(LeftArray<short>), new DeserializeDelegate((DeserializeDelegate<LeftArray<short>>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeShortLeftArray, true));
            DeserializeDelegates.Add(typeof(ListArray<short>), new DeserializeDelegate((DeserializeDelegate<ListArray<short>?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeShortListArray, true));
            DeserializeDelegates.Add(typeof(ushort), new DeserializeDelegate((DeserializeDelegate<ushort>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeUShort));
            DeserializeDelegates.Add(typeof(ushort?), new DeserializeDelegate((DeserializeDelegate<ushort?>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeNullableUShort));
            DeserializeDelegates.Add(typeof(ushort[]), new DeserializeDelegate((DeserializeDelegate<ushort[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeUShortArray, true));
            DeserializeDelegates.Add(typeof(ushort?[]), new DeserializeDelegate((DeserializeDelegate<ushort?[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeNullableUShortArray, true));
            DeserializeDelegates.Add(typeof(LeftArray<ushort>), new DeserializeDelegate((DeserializeDelegate<LeftArray<ushort>>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeUShortLeftArray, true));
            DeserializeDelegates.Add(typeof(ListArray<ushort>), new DeserializeDelegate((DeserializeDelegate<ListArray<ushort>?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeUShortListArray, true));
            DeserializeDelegates.Add(typeof(int), new DeserializeDelegate((DeserializeDelegate<int>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeInt));
            DeserializeDelegates.Add(typeof(int?), new DeserializeDelegate((DeserializeDelegate<int?>)Nullable<int>, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeNullableInt));
            DeserializeDelegates.Add(typeof(int[]), new DeserializeDelegate((DeserializeDelegate<int[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeIntArray, true));
            DeserializeDelegates.Add(typeof(int?[]), new DeserializeDelegate((DeserializeDelegate<int?[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeNullableIntArray, true));
            DeserializeDelegates.Add(typeof(LeftArray<int>), new DeserializeDelegate((DeserializeDelegate<LeftArray<int>>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeIntLeftArray, true));
            DeserializeDelegates.Add(typeof(ListArray<int>), new DeserializeDelegate((DeserializeDelegate<ListArray<int>?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeIntListArray, true));
            DeserializeDelegates.Add(typeof(uint), new DeserializeDelegate((DeserializeDelegate<uint>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeUInt));
            DeserializeDelegates.Add(typeof(uint?), new DeserializeDelegate((DeserializeDelegate<uint?>)Nullable<uint>, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeNullableUInt));
            DeserializeDelegates.Add(typeof(uint[]), new DeserializeDelegate((DeserializeDelegate<uint[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeUIntArray, true));
            DeserializeDelegates.Add(typeof(uint?[]), new DeserializeDelegate((DeserializeDelegate<uint?[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeNullableUIntArray, true));
            DeserializeDelegates.Add(typeof(LeftArray<uint>), new DeserializeDelegate((DeserializeDelegate<LeftArray<uint>>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeUIntLeftArray, true));
            DeserializeDelegates.Add(typeof(ListArray<uint>), new DeserializeDelegate((DeserializeDelegate<ListArray<uint>?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeUIntListArray, true));
            DeserializeDelegates.Add(typeof(long), new DeserializeDelegate((DeserializeDelegate<long>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeLong));
            DeserializeDelegates.Add(typeof(long?), new DeserializeDelegate((DeserializeDelegate<long?>)Nullable<long>, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeNullableLong));
            DeserializeDelegates.Add(typeof(long[]), new DeserializeDelegate((DeserializeDelegate<long[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeLongArray, true));
            DeserializeDelegates.Add(typeof(long?[]), new DeserializeDelegate((DeserializeDelegate<long?[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeNullableLongArray, true));
            DeserializeDelegates.Add(typeof(LeftArray<long>), new DeserializeDelegate((DeserializeDelegate<LeftArray<long>>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeLongLeftArray, true));
            DeserializeDelegates.Add(typeof(ListArray<long>), new DeserializeDelegate((DeserializeDelegate<ListArray<long>?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeLongListArray, true));
            DeserializeDelegates.Add(typeof(ulong), new DeserializeDelegate((DeserializeDelegate<ulong>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeULong));
            DeserializeDelegates.Add(typeof(ulong?), new DeserializeDelegate((DeserializeDelegate<ulong?>)Nullable<ulong>, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeNullableULong));
            DeserializeDelegates.Add(typeof(ulong[]), new DeserializeDelegate((DeserializeDelegate<ulong[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeULongArray, true));
            DeserializeDelegates.Add(typeof(ulong?[]), new DeserializeDelegate((DeserializeDelegate<ulong?[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeNullableULongArray, true));
            DeserializeDelegates.Add(typeof(LeftArray<ulong>), new DeserializeDelegate((DeserializeDelegate<LeftArray<ulong>>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeULongLeftArray, true));
            DeserializeDelegates.Add(typeof(ListArray<ulong>), new DeserializeDelegate((DeserializeDelegate<ListArray<ulong>?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeULongListArray, true));
            DeserializeDelegates.Add(typeof(float), new DeserializeDelegate((DeserializeDelegate<float>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeFloat));
            DeserializeDelegates.Add(typeof(float?), new DeserializeDelegate((DeserializeDelegate<float?>)Nullable<float>, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeNullableFloat));
            DeserializeDelegates.Add(typeof(float[]), new DeserializeDelegate((DeserializeDelegate<float[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeFloatArray, true));
            DeserializeDelegates.Add(typeof(float?[]), new DeserializeDelegate((DeserializeDelegate<float?[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeNullableFloatArray, true));
            DeserializeDelegates.Add(typeof(LeftArray<float>), new DeserializeDelegate((DeserializeDelegate<LeftArray<float>>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeFloatLeftArray, true));
            DeserializeDelegates.Add(typeof(ListArray<float>), new DeserializeDelegate((DeserializeDelegate<ListArray<float>?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeFloatListArray, true));
            DeserializeDelegates.Add(typeof(double), new DeserializeDelegate((DeserializeDelegate<double>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeDouble));
            DeserializeDelegates.Add(typeof(double?), new DeserializeDelegate((DeserializeDelegate<double?>)Nullable<double>, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeNullableDouble));
            DeserializeDelegates.Add(typeof(double[]), new DeserializeDelegate((DeserializeDelegate<double[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeDoubleArray, true));
            DeserializeDelegates.Add(typeof(double?[]), new DeserializeDelegate((DeserializeDelegate<double?[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeNullableDoubleArray, true));
            DeserializeDelegates.Add(typeof(LeftArray<double>), new DeserializeDelegate((DeserializeDelegate<LeftArray<double>>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeDoubleLeftArray, true));
            DeserializeDelegates.Add(typeof(ListArray<double>), new DeserializeDelegate((DeserializeDelegate<ListArray<double>?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeDoubleListArray, true));
            DeserializeDelegates.Add(typeof(decimal), new DeserializeDelegate((DeserializeDelegate<decimal>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeDecimal));
            DeserializeDelegates.Add(typeof(decimal?), new DeserializeDelegate((DeserializeDelegate<decimal?>)Nullable<decimal>, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeNullableDecimal));
            DeserializeDelegates.Add(typeof(decimal[]), new DeserializeDelegate((DeserializeDelegate<decimal[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeDecimalArray, true));
            DeserializeDelegates.Add(typeof(decimal?[]), new DeserializeDelegate((DeserializeDelegate<decimal?[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeNullableDecimalArray, true));
            DeserializeDelegates.Add(typeof(LeftArray<decimal>), new DeserializeDelegate((DeserializeDelegate<LeftArray<decimal>>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeDecimalLeftArray, true));
            DeserializeDelegates.Add(typeof(ListArray<decimal>), new DeserializeDelegate((DeserializeDelegate<ListArray<decimal>?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeDecimalListArray, true));
            DeserializeDelegates.Add(typeof(char), new DeserializeDelegate((DeserializeDelegate<char>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeChar));
            DeserializeDelegates.Add(typeof(char?), new DeserializeDelegate((DeserializeDelegate<char?>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeNullableChar));
            DeserializeDelegates.Add(typeof(char[]), new DeserializeDelegate((DeserializeDelegate<char[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeCharArray, true));
            DeserializeDelegates.Add(typeof(char?[]), new DeserializeDelegate((DeserializeDelegate<char?[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeNullableCharArray, true));
            DeserializeDelegates.Add(typeof(LeftArray<char>), new DeserializeDelegate((DeserializeDelegate<LeftArray<char>>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeCharLeftArray, true));
            DeserializeDelegates.Add(typeof(ListArray<char>), new DeserializeDelegate((DeserializeDelegate<ListArray<char>?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeCharListArray, true));
            DeserializeDelegates.Add(typeof(DateTime), new DeserializeDelegate((DeserializeDelegate<DateTime>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeDateTime));
            DeserializeDelegates.Add(typeof(DateTime?), new DeserializeDelegate((DeserializeDelegate<DateTime?>)Nullable<DateTime>, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeNullableDateTime));
            DeserializeDelegates.Add(typeof(DateTime[]), new DeserializeDelegate((DeserializeDelegate<DateTime[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeDateTimeArray, true));
            DeserializeDelegates.Add(typeof(DateTime?[]), new DeserializeDelegate((DeserializeDelegate<DateTime?[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeNullableDateTimeArray, true));
            DeserializeDelegates.Add(typeof(LeftArray<DateTime>), new DeserializeDelegate((DeserializeDelegate<LeftArray<DateTime>>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeDateTimeLeftArray, true));
            DeserializeDelegates.Add(typeof(ListArray<DateTime>), new DeserializeDelegate((DeserializeDelegate<ListArray<DateTime>?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeDateTimeListArray, true));
            DeserializeDelegates.Add(typeof(TimeSpan), new DeserializeDelegate((DeserializeDelegate<TimeSpan>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeTimeSpan));
            DeserializeDelegates.Add(typeof(TimeSpan?), new DeserializeDelegate((DeserializeDelegate<TimeSpan?>)Nullable<TimeSpan>, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeNullableTimeSpan));
            DeserializeDelegates.Add(typeof(TimeSpan[]), new DeserializeDelegate((DeserializeDelegate<TimeSpan[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeTimeSpanArray, true));
            DeserializeDelegates.Add(typeof(TimeSpan?[]), new DeserializeDelegate((DeserializeDelegate<TimeSpan?[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeNullableTimeSpanArray, true));
            DeserializeDelegates.Add(typeof(LeftArray<TimeSpan>), new DeserializeDelegate((DeserializeDelegate<LeftArray<TimeSpan>>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeTimeSpanLeftArray, true));
            DeserializeDelegates.Add(typeof(ListArray<TimeSpan>), new DeserializeDelegate((DeserializeDelegate<ListArray<TimeSpan>?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeTimeSpanListArray, true));
            DeserializeDelegates.Add(typeof(Guid), new DeserializeDelegate((DeserializeDelegate<Guid>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeGuid));
            DeserializeDelegates.Add(typeof(Guid?), new DeserializeDelegate((DeserializeDelegate<Guid?>)Nullable<Guid>, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeNullableGuid));
            DeserializeDelegates.Add(typeof(Guid[]), new DeserializeDelegate((DeserializeDelegate<Guid[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeGuidArray, true));
            DeserializeDelegates.Add(typeof(Guid?[]), new DeserializeDelegate((DeserializeDelegate<Guid?[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeNullableGuidArray, true));
            DeserializeDelegates.Add(typeof(LeftArray<Guid>), new DeserializeDelegate((DeserializeDelegate<LeftArray<Guid>>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeGuidLeftArray, true));
            DeserializeDelegates.Add(typeof(ListArray<Guid>), new DeserializeDelegate((DeserializeDelegate<ListArray<Guid>?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeGuidListArray, true));
            DeserializeDelegates.Add(typeof(string), new DeserializeDelegate((DeserializeDelegate<string?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeString, true));
            DeserializeDelegates.Add(typeof(SubString), new DeserializeDelegate((DeserializeDelegate<SubString>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeSubString, true));
            DeserializeDelegates.Add(typeof(Type), new DeserializeDelegate((DeserializeDelegate<Type?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeType, true));
            DeserializeDelegates.Add(typeof(object), new DeserializeDelegate((DeserializeDelegate<object?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeObject, true));

            DeserializeDelegates.Add(typeof(Half), new DeserializeDelegate((DeserializeDelegate<Half>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeHalf));
            DeserializeDelegates.Add(typeof(Half[]), new DeserializeDelegate((DeserializeDelegate<Half[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeHalfArray, true));
            DeserializeDelegates.Add(typeof(UInt128), new DeserializeDelegate((DeserializeDelegate<UInt128>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeUInt128));
            DeserializeDelegates.Add(typeof(UInt128[]), new DeserializeDelegate((DeserializeDelegate<UInt128[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeUInt128Array, true));
            DeserializeDelegates.Add(typeof(Int128), new DeserializeDelegate((DeserializeDelegate<Int128>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeInt128));
            DeserializeDelegates.Add(typeof(Int128[]), new DeserializeDelegate((DeserializeDelegate<Int128[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeInt128Array, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Complex), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Complex>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeComplex));
            DeserializeDelegates.Add(typeof(System.Numerics.Complex[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Complex[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeComplexArray, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Plane), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Plane>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializePlane));
            DeserializeDelegates.Add(typeof(System.Numerics.Plane[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Plane[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializePlaneArray, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Quaternion), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Quaternion>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeQuaternion));
            DeserializeDelegates.Add(typeof(System.Numerics.Quaternion[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Quaternion[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeQuaternionArray, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Matrix3x2), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Matrix3x2>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeMatrix3x2));
            DeserializeDelegates.Add(typeof(System.Numerics.Matrix3x2[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Matrix3x2[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeMatrix3x2Array, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Matrix4x4), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Matrix4x4>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeMatrix4x4));
            DeserializeDelegates.Add(typeof(System.Numerics.Matrix4x4[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Matrix4x4[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeMatrix4x4Array, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector2), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector2>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeVector2));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector2[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector2[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeVector2Array, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector3), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector3>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeVector3));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector3[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector3[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeVector3Array, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector4), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector4>)primitiveDeserialize, (Func<BinaryDeserializer, object>)primitiveMemberDeserializeVector4));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector4[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector4[]?>)primitiveDeserialize, (Func<BinaryDeserializer, object?>)primitiveMemberDeserializeVector4Array, true));

            SimpleMethod = SimpleReflectionMethod = PrimitiveMemberUShortReflectionMethod = PrimitiveMemberSByteReflectionMethod = PrimitiveMemberShortReflectionMethod = PrimitiveMemberULongReflectionMethod = EnumUShortListArrayReflectionMethod = EnumUShortLeftArrayReflectionMethod = PrimitiveMemberUIntReflectionMethod = PrimitiveMemberLongReflectionMethod = PrimitiveMemberByteReflectionMethod = EnumSByteListArrayReflectionMethod = EnumSByteLeftArrayReflectionMethod = EnumShortListArrayReflectionMethod = EnumShortLeftArrayReflectionMethod = EnumULongListArrayReflectionMethod = EnumULongLeftArrayReflectionMethod = PrimitiveMemberIntReflectionMethod = EnumLongListArrayReflectionMethod = EnumLongLeftArrayReflectionMethod = EnumUIntListArrayReflectionMethod = EnumUIntLeftArrayReflectionMethod = EnumByteListArrayReflectionMethod = EnumByteLeftArrayReflectionMethod = NullableListArrayReflectionMethod = NullableLeftArrayReflectionMethod = EnumIntListArrayReflectionMethod = EnumIntLeftArrayReflectionMethod = EnumUShortArrayReflectionMethod = StructListArrayReflectionMethod = StructLeftArrayReflectionMethod = EnumSByteArrayReflectionMethod = EnumShortArrayReflectionMethod = EnumULongArrayReflectionMethod = EnumLongArrayReflectionMethod = EnumUIntArrayReflectionMethod = EnumByteArrayReflectionMethod = NullableArrayReflectionMethod = EnumIntArrayReflectionMethod = StructArrayReflectionMethod = NotSupportReflectionMethod = StructJsonReflectionMethod = DictionaryReflectionMethod = CollectionReflectionMethod = EnumUShortListArrayMethod = EnumUShortLeftArrayMethod = ListArrayReflectionMethod = LeftArrayReflectionMethod = DeserializeReflectionMethod = EnumSByteListArrayMethod = EnumSByteLeftArrayMethod = EnumShortListArrayMethod = EnumShortLeftArrayMethod = EnumULongListArrayMethod = EnumULongLeftArrayMethod = NullableReflectionMethod = EnumLongListArrayMethod = EnumLongLeftArrayMethod = EnumUIntListArrayMethod = EnumUIntLeftArrayMethod = EnumByteListArrayMethod = EnumByteLeftArrayMethod = NullableListArrayMethod = NullableLeftArrayMethod = ICustomReflectionMethod = EnumIntListArrayMethod = EnumIntLeftArrayMethod = EnumUShortArrayMethod = StructListArrayMethod = StructLeftArrayMethod = ArrayReflectionMethod = RealTypeObjectMethod = EnumSByteArrayMethod = EnumShortArrayMethod = EnumULongArrayMethod = JsonReflectionMethod = BaseReflectionMethod = EnumLongArrayMethod = EnumUIntArrayMethod = EnumByteArrayMethod = NullableArrayMethod = EnumIntArrayMethod = StructArrayMethod = NotSupportMethod = EnumUShortMethod = StructJsonMethod = DictionaryMethod = CollectionMethod = ListArrayMethod = LeftArrayMethod = EnumSByteMethod = EnumShortMethod = EnumULongMethod = NullableMethod = EnumUIntMethod = EnumLongMethod = EnumByteMethod = EnumIntMethod = ICustomMethod = ArrayMethod = JsonMethod = BaseMethod = AutoCSer.Common.NullMethodInfo;
            foreach (System.Reflection.MethodInfo method in typeof(BinaryDeserializer).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
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
                        if (method.Name == nameof(LeftArrayReflection)) LeftArrayReflectionMethod = method;
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
                        if (method.Name == nameof(DeserializeReflection)) DeserializeReflectionMethod = method;
                        else if (method.Name == nameof(StructArrayReflection)) StructArrayReflectionMethod = method;
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
            DeserializeDelegates.Add(typeof(bool), new DeserializeDelegate((DeserializeDelegate<bool>)primitiveDeserialize, (DeserializeDelegate<bool>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(bool?), new DeserializeDelegate((DeserializeDelegate<bool?>)primitiveDeserialize, (DeserializeDelegate<bool?>)primitiveMemberDeserialize));
#if NetStandard21
            DeserializeDelegates.Add(typeof(bool[]), new DeserializeDelegate((DeserializeDelegate<bool[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(bool?[]), new DeserializeDelegate((DeserializeDelegate<bool?[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<bool>), new DeserializeDelegate((DeserializeDelegate<LeftArray<bool>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<bool>), new DeserializeDelegate((DeserializeDelegate<ListArray<bool>?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(byte), new DeserializeDelegate((DeserializeDelegate<byte>)primitiveDeserialize, (DeserializeDelegate<byte>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(byte?), new DeserializeDelegate((DeserializeDelegate<byte?>)primitiveDeserialize, (DeserializeDelegate<byte?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(byte[]), new DeserializeDelegate((DeserializeDelegate<byte[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(byte?[]), new DeserializeDelegate((DeserializeDelegate<byte?[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<byte>), new DeserializeDelegate((DeserializeDelegate<LeftArray<byte>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<byte>), new DeserializeDelegate((DeserializeDelegate<ListArray<byte>?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(sbyte), new DeserializeDelegate((DeserializeDelegate<sbyte>)primitiveDeserialize, (DeserializeDelegate<sbyte>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(sbyte?), new DeserializeDelegate((DeserializeDelegate<sbyte?>)primitiveDeserialize, (DeserializeDelegate<sbyte?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(sbyte[]), new DeserializeDelegate((DeserializeDelegate<sbyte[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(sbyte?[]), new DeserializeDelegate((DeserializeDelegate<sbyte?[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<sbyte>), new DeserializeDelegate((DeserializeDelegate<LeftArray<sbyte>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<sbyte>), new DeserializeDelegate((DeserializeDelegate<ListArray<sbyte>?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(short), new DeserializeDelegate((DeserializeDelegate<short>)primitiveDeserialize, (DeserializeDelegate<short>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(short?), new DeserializeDelegate((DeserializeDelegate<short?>)primitiveDeserialize, (DeserializeDelegate<short?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(short[]), new DeserializeDelegate((DeserializeDelegate<short[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(short?[]), new DeserializeDelegate((DeserializeDelegate<short?[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<short>), new DeserializeDelegate((DeserializeDelegate<LeftArray<short>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<short>), new DeserializeDelegate((DeserializeDelegate<ListArray<short>?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ushort), new DeserializeDelegate((DeserializeDelegate<ushort>)primitiveDeserialize, (DeserializeDelegate<ushort>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(ushort?), new DeserializeDelegate((DeserializeDelegate<ushort?>)primitiveDeserialize, (DeserializeDelegate<ushort?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(ushort[]), new DeserializeDelegate((DeserializeDelegate<ushort[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ushort?[]), new DeserializeDelegate((DeserializeDelegate<ushort?[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<ushort>), new DeserializeDelegate((DeserializeDelegate<LeftArray<ushort>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<ushort>), new DeserializeDelegate((DeserializeDelegate<ListArray<ushort>?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(int), new DeserializeDelegate((DeserializeDelegate<int>)primitiveDeserialize, (DeserializeDelegate<int>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(int?), new DeserializeDelegate((DeserializeDelegate<int?>)Nullable<int>, (DeserializeDelegate<int?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(int[]), new DeserializeDelegate((DeserializeDelegate<int[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(int?[]), new DeserializeDelegate((DeserializeDelegate<int?[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<int>), new DeserializeDelegate((DeserializeDelegate<LeftArray<int>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<int>), new DeserializeDelegate((DeserializeDelegate<ListArray<int>?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(uint), new DeserializeDelegate((DeserializeDelegate<uint>)primitiveDeserialize, (DeserializeDelegate<uint>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(uint?), new DeserializeDelegate((DeserializeDelegate<uint?>)Nullable<uint>, (DeserializeDelegate<uint?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(uint[]), new DeserializeDelegate((DeserializeDelegate<uint[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(uint?[]), new DeserializeDelegate((DeserializeDelegate<uint?[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<uint>), new DeserializeDelegate((DeserializeDelegate<LeftArray<uint>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<uint>), new DeserializeDelegate((DeserializeDelegate<ListArray<uint>?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(long), new DeserializeDelegate((DeserializeDelegate<long>)primitiveDeserialize, (DeserializeDelegate<long>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(long?), new DeserializeDelegate((DeserializeDelegate<long?>)Nullable<long>, (DeserializeDelegate<long?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(long[]), new DeserializeDelegate((DeserializeDelegate<long[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(long?[]), new DeserializeDelegate((DeserializeDelegate<long?[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<long>), new DeserializeDelegate((DeserializeDelegate<LeftArray<long>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<long>), new DeserializeDelegate((DeserializeDelegate<ListArray<long>?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ulong), new DeserializeDelegate((DeserializeDelegate<ulong>)primitiveDeserialize, (DeserializeDelegate<ulong>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(ulong?), new DeserializeDelegate((DeserializeDelegate<ulong?>)Nullable<ulong>, (DeserializeDelegate<ulong?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(ulong[]), new DeserializeDelegate((DeserializeDelegate<ulong[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ulong?[]), new DeserializeDelegate((DeserializeDelegate<ulong?[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<ulong>), new DeserializeDelegate((DeserializeDelegate<LeftArray<ulong>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<ulong>), new DeserializeDelegate((DeserializeDelegate<ListArray<ulong>?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(float), new DeserializeDelegate((DeserializeDelegate<float>)primitiveDeserialize, (DeserializeDelegate<float>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(float?), new DeserializeDelegate((DeserializeDelegate<float?>)Nullable<float>, (DeserializeDelegate<float?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(float[]), new DeserializeDelegate((DeserializeDelegate<float[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(float?[]), new DeserializeDelegate((DeserializeDelegate<float?[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<float>), new DeserializeDelegate((DeserializeDelegate<LeftArray<float>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<float>), new DeserializeDelegate((DeserializeDelegate<ListArray<float>?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(double), new DeserializeDelegate((DeserializeDelegate<double>)primitiveDeserialize, (DeserializeDelegate<double>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(double?), new DeserializeDelegate((DeserializeDelegate<double?>)Nullable<double>, (DeserializeDelegate<double?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(double[]), new DeserializeDelegate((DeserializeDelegate<double[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(double?[]), new DeserializeDelegate((DeserializeDelegate<double?[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<double>), new DeserializeDelegate((DeserializeDelegate<LeftArray<double>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<double>), new DeserializeDelegate((DeserializeDelegate<ListArray<double>?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(decimal), new DeserializeDelegate((DeserializeDelegate<decimal>)primitiveDeserialize, (DeserializeDelegate<decimal>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(decimal?), new DeserializeDelegate((DeserializeDelegate<decimal?>)Nullable<decimal>, (DeserializeDelegate<decimal?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(decimal[]), new DeserializeDelegate((DeserializeDelegate<decimal[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(decimal?[]), new DeserializeDelegate((DeserializeDelegate<decimal?[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<decimal>), new DeserializeDelegate((DeserializeDelegate<LeftArray<decimal>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<decimal>), new DeserializeDelegate((DeserializeDelegate<ListArray<decimal>?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(char), new DeserializeDelegate((DeserializeDelegate<char>)primitiveDeserialize, (DeserializeDelegate<char>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(char?), new DeserializeDelegate((DeserializeDelegate<char?>)primitiveDeserialize, (DeserializeDelegate<char?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(char[]), new DeserializeDelegate((DeserializeDelegate<char[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(char?[]), new DeserializeDelegate((DeserializeDelegate<char?[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<char>), new DeserializeDelegate((DeserializeDelegate<LeftArray<char>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<char>), new DeserializeDelegate((DeserializeDelegate<ListArray<char>?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(DateTime), new DeserializeDelegate((DeserializeDelegate<DateTime>)primitiveDeserialize, (DeserializeDelegate<DateTime>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(DateTime?), new DeserializeDelegate((DeserializeDelegate<DateTime?>)Nullable<DateTime>, (DeserializeDelegate<DateTime?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(DateTime[]), new DeserializeDelegate((DeserializeDelegate<DateTime[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(DateTime?[]), new DeserializeDelegate((DeserializeDelegate<DateTime?[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<DateTime>), new DeserializeDelegate((DeserializeDelegate<LeftArray<DateTime>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<DateTime>), new DeserializeDelegate((DeserializeDelegate<ListArray<DateTime>?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(TimeSpan), new DeserializeDelegate((DeserializeDelegate<TimeSpan>)primitiveDeserialize, (DeserializeDelegate<TimeSpan>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(TimeSpan?), new DeserializeDelegate((DeserializeDelegate<TimeSpan?>)Nullable<TimeSpan>, (DeserializeDelegate<TimeSpan?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(TimeSpan[]), new DeserializeDelegate((DeserializeDelegate<TimeSpan[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(TimeSpan?[]), new DeserializeDelegate((DeserializeDelegate<TimeSpan?[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<TimeSpan>), new DeserializeDelegate((DeserializeDelegate<LeftArray<TimeSpan>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<TimeSpan>), new DeserializeDelegate((DeserializeDelegate<ListArray<TimeSpan>?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(Guid), new DeserializeDelegate((DeserializeDelegate<Guid>)primitiveDeserialize, (DeserializeDelegate<Guid>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(Guid?), new DeserializeDelegate((DeserializeDelegate<Guid?>)Nullable<Guid>, (DeserializeDelegate<Guid?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(Guid[]), new DeserializeDelegate((DeserializeDelegate<Guid[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(Guid?[]), new DeserializeDelegate((DeserializeDelegate<Guid?[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<Guid>), new DeserializeDelegate((DeserializeDelegate<LeftArray<Guid>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<Guid>), new DeserializeDelegate((DeserializeDelegate<ListArray<Guid>?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(string), new DeserializeDelegate((DeserializeDelegate<string?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(SubString), new DeserializeDelegate((DeserializeDelegate<SubString>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(Type), new DeserializeDelegate((DeserializeDelegate<Type?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(object), new DeserializeDelegate((DeserializeDelegate<object?>)primitiveDeserialize, true));

            DeserializeDelegates.Add(typeof(Half), new DeserializeDelegate((DeserializeDelegate<Half>)primitiveDeserialize, (DeserializeDelegate<Half>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(Half[]), new DeserializeDelegate((DeserializeDelegate<Half[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(UInt128), new DeserializeDelegate((DeserializeDelegate<UInt128>)primitiveDeserialize, (DeserializeDelegate<UInt128>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(UInt128[]), new DeserializeDelegate((DeserializeDelegate<UInt128[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(Int128), new DeserializeDelegate((DeserializeDelegate<Int128>)primitiveDeserialize, (DeserializeDelegate<Int128>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(Int128[]), new DeserializeDelegate((DeserializeDelegate<Int128[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Complex), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Complex>)primitiveDeserialize, (DeserializeDelegate<System.Numerics.Complex>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(System.Numerics.Complex[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Complex[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Plane), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Plane>)primitiveDeserialize, (DeserializeDelegate<System.Numerics.Plane>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(System.Numerics.Plane[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Plane[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Quaternion), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Quaternion>)primitiveDeserialize, (DeserializeDelegate<System.Numerics.Quaternion>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(System.Numerics.Quaternion[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Quaternion[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Matrix3x2), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Matrix3x2>)primitiveDeserialize, (DeserializeDelegate<System.Numerics.Matrix3x2>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(System.Numerics.Matrix3x2[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Matrix3x2[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Matrix4x4), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Matrix4x4>)primitiveDeserialize, (DeserializeDelegate<System.Numerics.Matrix4x4>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(System.Numerics.Matrix4x4[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Matrix4x4[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector2), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector2>)primitiveDeserialize, (DeserializeDelegate<System.Numerics.Vector2>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector2[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector2[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector3), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector3>)primitiveDeserialize, (DeserializeDelegate<System.Numerics.Vector3>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector3[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector3[]?>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector4), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector4>)primitiveDeserialize, (DeserializeDelegate<System.Numerics.Vector4>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector4[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector4[]?>)primitiveDeserialize, true));
#else
            DeserializeDelegates.Add(typeof(bool[]), new DeserializeDelegate((DeserializeDelegate<bool[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(bool?[]), new DeserializeDelegate((DeserializeDelegate<bool?[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<bool>), new DeserializeDelegate((DeserializeDelegate<LeftArray<bool>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<bool>), new DeserializeDelegate((DeserializeDelegate<ListArray<bool>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(byte), new DeserializeDelegate((DeserializeDelegate<byte>)primitiveDeserialize, (DeserializeDelegate<byte>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(byte?), new DeserializeDelegate((DeserializeDelegate<byte?>)primitiveDeserialize, (DeserializeDelegate<byte?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(byte[]), new DeserializeDelegate((DeserializeDelegate<byte[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(byte?[]), new DeserializeDelegate((DeserializeDelegate<byte?[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<byte>), new DeserializeDelegate((DeserializeDelegate<LeftArray<byte>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<byte>), new DeserializeDelegate((DeserializeDelegate<ListArray<byte>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(sbyte), new DeserializeDelegate((DeserializeDelegate<sbyte>)primitiveDeserialize, (DeserializeDelegate<sbyte>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(sbyte?), new DeserializeDelegate((DeserializeDelegate<sbyte?>)primitiveDeserialize, (DeserializeDelegate<sbyte?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(sbyte[]), new DeserializeDelegate((DeserializeDelegate<sbyte[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(sbyte?[]), new DeserializeDelegate((DeserializeDelegate<sbyte?[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<sbyte>), new DeserializeDelegate((DeserializeDelegate<LeftArray<sbyte>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<sbyte>), new DeserializeDelegate((DeserializeDelegate<ListArray<sbyte>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(short), new DeserializeDelegate((DeserializeDelegate<short>)primitiveDeserialize, (DeserializeDelegate<short>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(short?), new DeserializeDelegate((DeserializeDelegate<short?>)primitiveDeserialize, (DeserializeDelegate<short?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(short[]), new DeserializeDelegate((DeserializeDelegate<short[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(short?[]), new DeserializeDelegate((DeserializeDelegate<short?[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<short>), new DeserializeDelegate((DeserializeDelegate<LeftArray<short>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<short>), new DeserializeDelegate((DeserializeDelegate<ListArray<short>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ushort), new DeserializeDelegate((DeserializeDelegate<ushort>)primitiveDeserialize, (DeserializeDelegate<ushort>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(ushort?), new DeserializeDelegate((DeserializeDelegate<ushort?>)primitiveDeserialize, (DeserializeDelegate<ushort?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(ushort[]), new DeserializeDelegate((DeserializeDelegate<ushort[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ushort?[]), new DeserializeDelegate((DeserializeDelegate<ushort?[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<ushort>), new DeserializeDelegate((DeserializeDelegate<LeftArray<ushort>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<ushort>), new DeserializeDelegate((DeserializeDelegate<ListArray<ushort>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(int), new DeserializeDelegate((DeserializeDelegate<int>)primitiveDeserialize, (DeserializeDelegate<int>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(int?), new DeserializeDelegate((DeserializeDelegate<int?>)Nullable<int>, (DeserializeDelegate<int?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(int[]), new DeserializeDelegate((DeserializeDelegate<int[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(int?[]), new DeserializeDelegate((DeserializeDelegate<int?[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<int>), new DeserializeDelegate((DeserializeDelegate<LeftArray<int>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<int>), new DeserializeDelegate((DeserializeDelegate<ListArray<int>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(uint), new DeserializeDelegate((DeserializeDelegate<uint>)primitiveDeserialize, (DeserializeDelegate<uint>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(uint?), new DeserializeDelegate((DeserializeDelegate<uint?>)Nullable<uint>, (DeserializeDelegate<uint?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(uint[]), new DeserializeDelegate((DeserializeDelegate<uint[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(uint?[]), new DeserializeDelegate((DeserializeDelegate<uint?[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<uint>), new DeserializeDelegate((DeserializeDelegate<LeftArray<uint>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<uint>), new DeserializeDelegate((DeserializeDelegate<ListArray<uint>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(long), new DeserializeDelegate((DeserializeDelegate<long>)primitiveDeserialize, (DeserializeDelegate<long>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(long?), new DeserializeDelegate((DeserializeDelegate<long?>)Nullable<long>, (DeserializeDelegate<long?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(long[]), new DeserializeDelegate((DeserializeDelegate<long[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(long?[]), new DeserializeDelegate((DeserializeDelegate<long?[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<long>), new DeserializeDelegate((DeserializeDelegate<LeftArray<long>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<long>), new DeserializeDelegate((DeserializeDelegate<ListArray<long>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ulong), new DeserializeDelegate((DeserializeDelegate<ulong>)primitiveDeserialize, (DeserializeDelegate<ulong>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(ulong?), new DeserializeDelegate((DeserializeDelegate<ulong?>)Nullable<ulong>, (DeserializeDelegate<ulong?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(ulong[]), new DeserializeDelegate((DeserializeDelegate<ulong[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ulong?[]), new DeserializeDelegate((DeserializeDelegate<ulong?[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<ulong>), new DeserializeDelegate((DeserializeDelegate<LeftArray<ulong>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<ulong>), new DeserializeDelegate((DeserializeDelegate<ListArray<ulong>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(float), new DeserializeDelegate((DeserializeDelegate<float>)primitiveDeserialize, (DeserializeDelegate<float>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(float?), new DeserializeDelegate((DeserializeDelegate<float?>)Nullable<float>, (DeserializeDelegate<float?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(float[]), new DeserializeDelegate((DeserializeDelegate<float[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(float?[]), new DeserializeDelegate((DeserializeDelegate<float?[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<float>), new DeserializeDelegate((DeserializeDelegate<LeftArray<float>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<float>), new DeserializeDelegate((DeserializeDelegate<ListArray<float>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(double), new DeserializeDelegate((DeserializeDelegate<double>)primitiveDeserialize, (DeserializeDelegate<double>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(double?), new DeserializeDelegate((DeserializeDelegate<double?>)Nullable<double>, (DeserializeDelegate<double?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(double[]), new DeserializeDelegate((DeserializeDelegate<double[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(double?[]), new DeserializeDelegate((DeserializeDelegate<double?[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<double>), new DeserializeDelegate((DeserializeDelegate<LeftArray<double>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<double>), new DeserializeDelegate((DeserializeDelegate<ListArray<double>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(decimal), new DeserializeDelegate((DeserializeDelegate<decimal>)primitiveDeserialize, (DeserializeDelegate<decimal>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(decimal?), new DeserializeDelegate((DeserializeDelegate<decimal?>)Nullable<decimal>, (DeserializeDelegate<decimal?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(decimal[]), new DeserializeDelegate((DeserializeDelegate<decimal[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(decimal?[]), new DeserializeDelegate((DeserializeDelegate<decimal?[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<decimal>), new DeserializeDelegate((DeserializeDelegate<LeftArray<decimal>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<decimal>), new DeserializeDelegate((DeserializeDelegate<ListArray<decimal>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(char), new DeserializeDelegate((DeserializeDelegate<char>)primitiveDeserialize, (DeserializeDelegate<char>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(char?), new DeserializeDelegate((DeserializeDelegate<char?>)primitiveDeserialize, (DeserializeDelegate<char?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(char[]), new DeserializeDelegate((DeserializeDelegate<char[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(char?[]), new DeserializeDelegate((DeserializeDelegate<char?[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<char>), new DeserializeDelegate((DeserializeDelegate<LeftArray<char>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<char>), new DeserializeDelegate((DeserializeDelegate<ListArray<char>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(DateTime), new DeserializeDelegate((DeserializeDelegate<DateTime>)primitiveDeserialize, (DeserializeDelegate<DateTime>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(DateTime?), new DeserializeDelegate((DeserializeDelegate<DateTime?>)Nullable<DateTime>, (DeserializeDelegate<DateTime?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(DateTime[]), new DeserializeDelegate((DeserializeDelegate<DateTime[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(DateTime?[]), new DeserializeDelegate((DeserializeDelegate<DateTime?[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<DateTime>), new DeserializeDelegate((DeserializeDelegate<LeftArray<DateTime>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<DateTime>), new DeserializeDelegate((DeserializeDelegate<ListArray<DateTime>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(TimeSpan), new DeserializeDelegate((DeserializeDelegate<TimeSpan>)primitiveDeserialize, (DeserializeDelegate<TimeSpan>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(TimeSpan?), new DeserializeDelegate((DeserializeDelegate<TimeSpan?>)Nullable<TimeSpan>, (DeserializeDelegate<TimeSpan?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(TimeSpan[]), new DeserializeDelegate((DeserializeDelegate<TimeSpan[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(TimeSpan?[]), new DeserializeDelegate((DeserializeDelegate<TimeSpan?[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<TimeSpan>), new DeserializeDelegate((DeserializeDelegate<LeftArray<TimeSpan>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<TimeSpan>), new DeserializeDelegate((DeserializeDelegate<ListArray<TimeSpan>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(Guid), new DeserializeDelegate((DeserializeDelegate<Guid>)primitiveDeserialize, (DeserializeDelegate<Guid>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(Guid?), new DeserializeDelegate((DeserializeDelegate<Guid?>)Nullable<Guid>, (DeserializeDelegate<Guid?>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(Guid[]), new DeserializeDelegate((DeserializeDelegate<Guid[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(Guid?[]), new DeserializeDelegate((DeserializeDelegate<Guid?[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(LeftArray<Guid>), new DeserializeDelegate((DeserializeDelegate<LeftArray<Guid>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(ListArray<Guid>), new DeserializeDelegate((DeserializeDelegate<ListArray<Guid>>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(string), new DeserializeDelegate((DeserializeDelegate<string>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(SubString), new DeserializeDelegate((DeserializeDelegate<SubString>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(Type), new DeserializeDelegate((DeserializeDelegate<Type>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(object), new DeserializeDelegate((DeserializeDelegate<object>)primitiveDeserialize, true));
            
            DeserializeDelegates.Add(typeof(Half), new DeserializeDelegate((DeserializeDelegate<Half>)primitiveDeserialize, (DeserializeDelegate<Half>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(Half[]), new DeserializeDelegate((DeserializeDelegate<Half[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(UInt128), new DeserializeDelegate((DeserializeDelegate<UInt128>)primitiveDeserialize, (DeserializeDelegate<UInt128>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(UInt128[]), new DeserializeDelegate((DeserializeDelegate<UInt128[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(Int128), new DeserializeDelegate((DeserializeDelegate<Int128>)primitiveDeserialize, (DeserializeDelegate<Int128>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(Int128[]), new DeserializeDelegate((DeserializeDelegate<Int128[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Complex), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Complex>)primitiveDeserialize, (DeserializeDelegate<System.Numerics.Complex>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(System.Numerics.Complex[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Complex[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Plane), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Plane>)primitiveDeserialize, (DeserializeDelegate<System.Numerics.Plane>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(System.Numerics.Plane[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Plane[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Quaternion), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Quaternion>)primitiveDeserialize, (DeserializeDelegate<System.Numerics.Quaternion>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(System.Numerics.Quaternion[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Quaternion[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Matrix3x2), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Matrix3x2>)primitiveDeserialize, (DeserializeDelegate<System.Numerics.Matrix3x2>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(System.Numerics.Matrix3x2[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Matrix3x2[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Matrix4x4), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Matrix4x4>)primitiveDeserialize, (DeserializeDelegate<System.Numerics.Matrix4x4>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(System.Numerics.Matrix4x4[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Matrix4x4[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector2), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector2>)primitiveDeserialize, (DeserializeDelegate<System.Numerics.Vector2>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector2[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector2[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector3), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector3>)primitiveDeserialize, (DeserializeDelegate<System.Numerics.Vector3>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector3[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector3[]>)primitiveDeserialize, true));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector4), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector4>)primitiveDeserialize, (DeserializeDelegate<System.Numerics.Vector4>)primitiveMemberDeserialize));
            DeserializeDelegates.Add(typeof(System.Numerics.Vector4[]), new DeserializeDelegate((DeserializeDelegate<System.Numerics.Vector4[]>)primitiveDeserialize, true));
#endif
            foreach (DeserializeDelegate deserializeDelegate in BinarySerializer.CustomConfig.PrimitiveDeserializeDelegates)
            {
                var type = deserializeDelegate.Check();
                if (type != null) DeserializeDelegates[type] = deserializeDelegate;
            }
#endif
        }
    }
}
