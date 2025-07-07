using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class StructGenericType : GenericTypeCache<StructGenericType>
    {
#if !AOT
        /// <summary>
        /// JSON 序列化数组
        /// </summary>
        internal abstract Delegate JsonSerializeNullableArrayDelegate { get; }
        /// <summary>
        /// JSON 序列化可空类型
        /// </summary>
        internal abstract Delegate JsonSerializeNullableDelegate { get; }
        /// <summary>
        /// JSON 反序列化可空类型
        /// </summary>
        internal abstract Delegate JsonDeserializeNullableDelegate { get; }

        /// <summary>
        /// 二进制序列化可空类型
        /// </summary>
        internal abstract Delegate BinarySerializeNullableDelegate { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinarySerializeNullableArrayDelegate { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        internal abstract Delegate BinarySerializeNullableLeftArrayDelegate { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        internal abstract Delegate BinarySerializeNullableListArrayDelegate { get; }
        /// <summary>
        /// 二进制序列化转简单序列化
        /// </summary>
        internal abstract Delegate BinarySerializeSimpleDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeserializeNullableLeftArrayDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeserializeStructLeftArrayDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeserializeNullableListArrayDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeserializeStructListArrayDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeserializeNullableArrayDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeserializeStructArrayDelegate { get; }
        /// <summary>
        /// 二进制反序列化转简单反序列化
        /// </summary>
        internal abstract Delegate BinaryDeserializeSimpleDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Delegate BinaryDeserializeNullableDelegate { get; }
#endif

        /// <summary>
        /// The server queue task sends data
        /// 服务端执行队列任务发送数据
        /// </summary>
        internal abstract Delegate CommandServerCallQueueSend { get; }

        /// <summary>
        /// The server queue task sends data
        /// 服务端执行队列任务发送数据
        /// </summary>
        internal abstract Delegate CommandServerCallReadWriteQueueSend { get; }
        /// <summary>
        /// The server queue task sends data
        /// 服务端执行队列任务发送数据
        /// </summary>
        internal abstract Delegate CommandServerCallConcurrencyReadQueueSend { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static StructGenericType create<T>() where T : struct
        {
            return new StructGenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static StructGenericType? lastGenericType;
#else
        protected static StructGenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static StructGenericType Get(Type type)
        {
            var value = lastGenericType;
            if (value?.CurrentType == type) return value;
            value = get(type);
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class StructGenericType<T> : StructGenericType
        where T : struct
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

#if !AOT
        /// <summary>
        /// JSON 序列化数组
        /// </summary>
        internal override Delegate JsonSerializeNullableArrayDelegate { get { return (Action<AutoCSer.JsonSerializer, T?[]>)AutoCSer.JsonSerializer.NullableArray<T>; } }
        /// <summary>
        /// JSON 序列化可空类型
        /// </summary>
        internal override Delegate JsonSerializeNullableDelegate { get { return (Action<AutoCSer.JsonSerializer, T?>)AutoCSer.JsonSerializer.Nullable<T>; } }
        /// <summary>
        /// JSON 反序列化可空类型
        /// </summary>
        internal override Delegate JsonDeserializeNullableDelegate { get { return (JsonDeserializer.DeserializeDelegate<T?>)AutoCSer.JsonDeserializer.Nullable<T>; } }

        /// <summary>
        /// 二进制序列化可空类型
        /// </summary>
        internal override Delegate BinarySerializeNullableDelegate { get { return (Action<AutoCSer.BinarySerializer, T?>)AutoCSer.BinarySerializer.Nullable<T>; } }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        internal override Delegate BinarySerializeNullableArrayDelegate { get { return (Action<BinarySerializer, T?[]>)BinarySerializer.NullableArray<T>; } }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        internal override Delegate BinarySerializeNullableLeftArrayDelegate { get { return (Action<BinarySerializer, LeftArray<T?>>)BinarySerializer.NullableLeftArray<T>; } }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        internal override Delegate BinarySerializeNullableListArrayDelegate { get { return (Action<BinarySerializer, ListArray<T?>>)BinarySerializer.NullableListArray<T>; } }
        /// <summary>
        /// 二进制序列化转简单序列化
        /// </summary>
        internal override Delegate BinarySerializeSimpleDelegate { get { return (Action<AutoCSer.BinarySerializer, T>)AutoCSer.BinarySerializer.Simple<T>; } }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinaryDeserializeNullableLeftArrayDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<LeftArray<T?>>)BinaryDeserializer.NullableLeftArray<T>; } }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinaryDeserializeStructLeftArrayDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<LeftArray<T>>)BinaryDeserializer.StructLeftArray<T>; } }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal override Delegate BinaryDeserializeNullableListArrayDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T?>?>)BinaryDeserializer.NullableListArray<T>; } }
#else
        internal override Delegate BinaryDeserializeNullableListArrayDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T?>>)BinaryDeserializer.NullableListArray<T>; } }
#endif
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal override Delegate BinaryDeserializeStructListArrayDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>?>)BinaryDeserializer.StructListArray<T>; } }
#else
        internal override Delegate BinaryDeserializeStructListArrayDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<ListArray<T>>)BinaryDeserializer.StructListArray<T>; } }
#endif
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal override Delegate BinaryDeserializeNullableArrayDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T?[]?>)BinaryDeserializer.NullableArray<T>; } }
#else
        internal override Delegate BinaryDeserializeNullableArrayDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T?[]>)BinaryDeserializer.NullableArray<T>; } }
#endif
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal override Delegate BinaryDeserializeStructArrayDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]?>)BinaryDeserializer.StructArray<T>; } }
#else
        internal override Delegate BinaryDeserializeStructArrayDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T[]>)BinaryDeserializer.StructArray<T>; } }
#endif
        /// <summary>
        /// 二进制反序列化转简单反序列化
        /// </summary>
        internal override Delegate BinaryDeserializeSimpleDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.Simple<T>; } }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Delegate BinaryDeserializeNullableDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T?>)BinaryDeserializer.Nullable<T>; } }
#endif

        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="node"></param>
        /// <param name="queue"></param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        public delegate bool CommandServerCallQueueSendDelegate(AutoCSer.Net.CommandServerCallQueueNode node, AutoCSer.Net.CommandServerCallQueue queue, AutoCSer.Net.CommandServer.ServerInterfaceMethod method, ref T outputParameter);
        /// <summary>
        /// The server queue task sends data
        /// 服务端执行队列任务发送数据
        /// </summary>
        internal override Delegate CommandServerCallQueueSend { get { return (CommandServerCallQueueSendDelegate)AutoCSer.Net.CommandServerCallQueueNode.Send; } }
        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        public delegate bool CommandServerCallReadWriteQueueSendDelegate(AutoCSer.Net.CommandServerCallReadWriteQueueNode node, AutoCSer.Net.CommandServer.ServerInterfaceMethod method, ref T outputParameter);
        /// <summary>
        /// The server queue task sends data
        /// 服务端执行队列任务发送数据
        /// </summary>
        internal override Delegate CommandServerCallReadWriteQueueSend { get { return (CommandServerCallReadWriteQueueSendDelegate)AutoCSer.Net.CommandServerCallReadWriteQueueNode.Send; } }
        /// <summary>
        /// Send data
        /// </summary>
        /// <param name="node"></param>
        /// <param name="method">Server interface method information
        /// 服务端接口方法信息</param>
        /// <param name="outputParameter">Return value output parameters</param>
        /// <returns>Whether the addition to the output queue was successful
        /// 添加到输出队列是否成功</returns>
        public delegate bool CommandServerCallConcurrencyReadQueueSendDelegate(AutoCSer.Net.CommandServerCallConcurrencyReadQueueNode node, AutoCSer.Net.CommandServer.ServerInterfaceMethod method, ref T outputParameter);
        /// <summary>
        /// The server queue task sends data
        /// 服务端执行队列任务发送数据
        /// </summary>
        internal override Delegate CommandServerCallConcurrencyReadQueueSend { get { return (CommandServerCallConcurrencyReadQueueSendDelegate)AutoCSer.Net.CommandServerCallConcurrencyReadQueueNode.Send; } }
    }
}
