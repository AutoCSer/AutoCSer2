using AutoCSer.Memory;
using AutoCSer.Net;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class GenericType : GenericTypeCache<GenericType>
    {
#if !AOT
        /// <summary>
        /// 判断成员索引是否有效
        /// </summary>
        internal abstract Delegate GetMemberMapIsMemberDelegate { get; }
        /// <summary>
        /// 设置成员索引
        /// </summary>
        internal abstract Delegate GetMemberMapSetMemberDelegate { get; }
        /// <summary>
        /// 获取成员委托类型
        /// </summary>
        internal abstract Type GetMemberMapType { get; }
        /// <summary>
        /// 判断构造函数是否支持数据反序列化
        /// </summary>
        internal abstract bool IsSerializeConstructor { get; }
        /// <summary>
        /// 数组字串构造函数
        /// </summary>
        internal abstract Delegate LeftArrayDefaultConstructorDelegate { get; }
        /// <summary>
        /// JSON 序列化委托循环引用信息
        /// </summary>
        internal abstract AutoCSer.TextSerialize.DelegateReference JsonSerializeDelegateReference { get; }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal abstract Action<AutoCSer.JsonSerializer, object> JsonSerializeObjectDelegate { get; }
        /// <summary>
        /// JSON 序列化
        /// </summary>
        internal abstract Delegate JsonSerializeDelegate { get; }
        /// <summary>
        /// JSON 自定义序列化不支持类型
        /// </summary>
        internal abstract Delegate JsonSerializeNotSupportDelegate { get; }
        /// <summary>
        /// 获取 JSON 序列化数组委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal abstract void GetJsonSerializeArrayDelegate(ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference);
        /// <summary>
        /// JSON 反序列化数组
        /// </summary>
        internal abstract Delegate JsonDeserializeArrayDelegate { get; }
        /// <summary>
        /// JSON 自定义反序列化不支持类型
        /// </summary>
        internal abstract Delegate JsonDeserializeNotSupportDelegate { get; }
        /// <summary>
        /// JSON 反序列化类型
        /// </summary>
        internal abstract Delegate JsonDeserializeDelegate { get; }

        /// <summary>
        /// JSON 序列化数组
        /// </summary>
        internal abstract AutoCSer.TextSerialize.SerializeDelegate JsonSerializeLeftArrayDelegate { get; }
        /// <summary>
        /// JSON 序列化数组
        /// </summary>
        internal abstract AutoCSer.TextSerialize.SerializeDelegate JsonSerializeListArrayDelegate { get; }
        /// <summary>
        /// JSON 反序列化数组
        /// </summary>
        internal abstract Delegate JsonDeserializeLeftArrayDelegate { get; }
        /// <summary>
        /// JSON 反序列化数组
        /// </summary>
        internal abstract Delegate JsonDeserializeListArrayDelegate { get; }

        /// <summary>
        /// 二进制序列化委托循环引用信息
        /// </summary>
        internal abstract AutoCSer.BinarySerialize.SerializeDelegateReference BinarySerializeDelegateReference { get; }
        /// <summary>
        /// 二进制序列化
        /// </summary>
        internal abstract Delegate BinarySerializeDelegate { get; }
        /// <summary>
        /// 二进制序列化数组
        /// </summary>
        internal abstract Delegate BinarySerializeStructLeftArrayDelegate { get; }
        /// <summary>
        /// 二进制序列化数组
        /// </summary>
        internal abstract Delegate BinarySerializeStructListArrayDelegate { get; }
        /// <summary>
        /// 二进制自定义序列化不支持类型
        /// </summary>
        internal abstract Delegate BinarySerializeNotSupportDelegate { get; }
        /// <summary>
        /// 二进制自定义反序列化不支持类型
        /// </summary>
        internal abstract Delegate BinaryDeserializeNotSupportDelegate { get; }
        /// <summary>
        /// 二进制混杂 JSON 反序列化
        /// </summary>
        internal abstract Delegate BinaryDeserializeStructJsonDelegate { get; } 
        /// <summary>
        /// 二进制混杂 JSON 反序列化
        /// </summary>
        internal abstract Delegate BinaryDeserializeJsonDelegate { get; }
        /// <summary>
        /// 二进制反序列化类型
        /// </summary>
        internal abstract Delegate BinaryDeserializeDelegate { get; }

        /// <summary>
        /// JSON 序列化
        /// </summary>
#if NetStandard21
        internal abstract Func<object, JsonSerializeConfig?, KeyValue<string, AutoCSer.TextSerialize.WarningEnum>> JsonSerializeObjectGenericDelegate { get; }
#else
        internal abstract Func<object, JsonSerializeConfig, KeyValue<string, AutoCSer.TextSerialize.WarningEnum>> JsonSerializeObjectGenericDelegate { get; }
#endif
        /// <summary>
        /// JSON 序列化
        /// </summary>
#if NetStandard21
        internal abstract Func<object, CharStream, JsonSerializeConfig?, AutoCSer.TextSerialize.WarningEnum> JsonSerializeStreamObjectDelegate { get; }
#else
        internal abstract Func<object, CharStream, JsonSerializeConfig, AutoCSer.TextSerialize.WarningEnum> JsonSerializeStreamObjectDelegate { get; }
#endif
        /// <summary>
        /// 二进制序列化数组
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal abstract void GetBinarySerializeStructArrayDelegate(ref AutoCSer.BinarySerialize.SerializeDelegateReference serializeDelegateReference);
        /// <summary>
        /// 二进制混杂 JSON 序列化
        /// </summary>
        internal abstract Delegate BinarySerializeJsonDelegate { get; }
        /// <summary>
        /// 二进制混杂 JSON 序列化
        /// </summary>
        internal abstract Delegate BinarySerializeMemberJsonDelegate { get; }
        /// <summary>
        /// 二进制混杂 JSON 序列化
        /// </summary>
        internal abstract Delegate BinarySerializeStructJsonDelegate { get; }
        /// <summary>
        /// 二进制混杂 JSON 序列化
        /// </summary>
        internal abstract Delegate BinarySerializeMemberStructJsonDelegate { get; }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal abstract Action<AutoCSer.BinarySerializer, object> BinarySerializeRealTypeObjectDelegate { get; }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal abstract Func<AutoCSer.BinaryDeserializer, object?> BinaryDeserializeRealTypeObjectDelegate { get; }
#else
        internal abstract Func<AutoCSer.BinaryDeserializer, object> BinaryDeserializeRealTypeObjectDelegate { get; }
#endif
        /// <summary>
        /// 获取客户端返回值委托
        /// </summary>
        internal abstract Delegate GetCommandClientReturnValueDelegate { get; }
        /// <summary>
        /// 获取客户端错误返回值委托
        /// </summary>
        internal abstract Delegate GetCommandClientReturnTypeDelegate { get; }
        /// <summary>
        /// 命令客户端回调委托
        /// </summary>
        internal abstract Delegate CommandClientControllerCallbackDelegate { get; }
        /// <summary>
        /// 命令客户端保持回调委托
        /// </summary>
        internal abstract Delegate CommandClientControllerKeepCallbackDelegate { get; }
        /// <summary>
        /// 命令客户端队列回调委托
        /// </summary>
        internal abstract Delegate CommandClientControllerCallbackQueueDelegate { get; }
        /// <summary>
        /// 命令客户端队列保持回调委托
        /// </summary>
        internal abstract Delegate CommandClientControllerKeepCallbackQueueDelegate { get; }
        /// <summary>
        /// 命令客户端返回值委托
        /// </summary>
        internal abstract Delegate CommandClientControllerReturnValueDelegate { get; }
        /// <summary>
        /// 获取 Task 委托
        /// </summary>
        internal abstract Delegate CommandClientReturnCommandGetTaskDelegate { get; }
        /// <summary>
        /// 命令客户端队列返回值委托
        /// </summary>
        internal abstract Delegate CommandClientControllerReturnValueQueueDelegate { get; }
        /// <summary>
        /// 命令客户端枚举返回值委托
        /// </summary>
        internal abstract Delegate CommandClientControllerEnumeratorDelegate { get; }
        /// <summary>
        /// 命令客户端队列枚举返回值委托
        /// </summary>
        internal abstract Delegate CommandClientControllerEnumeratorQueueDelegate { get; }
        /// <summary>
        /// 获取客户端回调委托
        /// </summary>
        internal abstract Delegate GetCommandClientCallbackDelegate { get; }
        /// <summary>
        /// 获取客户端回调委托
        /// </summary>
        internal abstract Delegate GetCommandClientKeepCallbackDelegate { get; }
        /// <summary>
        /// 获取客户端回调委托
        /// </summary>
        internal abstract Delegate GetCommandClientCallbackQueueDelegate { get; }
        /// <summary>
        /// 获取客户端回调委托
        /// </summary>
        internal abstract Delegate GetCommandClientKeepCallbackQueueDelegate { get; }
#endif

        /// <summary>
        /// Task 类型
        /// </summary>
        internal abstract Type TaskType { get; }

        /// <summary>
        /// 获取控制器创建器
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
#if NetStandard21
        internal abstract AutoCSer.Net.CommandServerInterfaceControllerCreator GetCommandServerInterfaceControllerCreator(object controller, string? controllerName);
#else
        internal abstract AutoCSer.Net.CommandServerInterfaceControllerCreator GetCommandServerInterfaceControllerCreator(object controller, string controllerName);
#endif
        /// <summary>
        /// 发送返回值数据
        /// </summary>
        internal abstract Delegate CommandServerSocketSendReturnValueDelegate { get; }
        /// <summary>
        /// TCP 服务器端异步回调类型
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal abstract Type GetCommandServerCallbackType(ServerInterfaceMethod method);
        /// <summary>
        /// TCP 服务器端异步成功回调
        /// </summary>
        internal abstract Delegate CommandServerCallbackDelegate { get; }
        /// <summary>
        /// TCP 服务器端异步成功回调
        /// </summary>
        internal abstract Delegate CommandServerSynchronousCallbackDelegate { get; }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal abstract Delegate CreateCommandServerKeepCallbackDelegate { get; }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal abstract Delegate CreateCommandServerKeepCallbackCountDelegate { get; }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal abstract Delegate CreateCommandServerKeepCallbackQueueNodeDelegate { get; }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal abstract Delegate CreateCommandServerKeepCallbackReadWriteQueueNodeDelegate { get; }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal abstract Delegate CreateCommandServerKeepCallbackConcurrencyReadQueueNodeDelegate { get; }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal abstract Delegate CreateCommandServerKeepCallbackCountQueueNodeDelegate { get; }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal abstract Delegate CreateCommandServerKeepCallbackCountReadWriteQueueNodeDelegate { get; }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal abstract Delegate CreateCommandServerKeepCallbackCountConcurrencyReadQueueNodeDelegate { get; }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal abstract Delegate CreateServerKeepCallbackTaskDelegate { get; }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal abstract Delegate CreateServerKeepCallbackCountTaskDelegate { get; }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal abstract Delegate CreateServerEnumerableKeepCallbackCountTaskDelegate { get; }
#if NetStandard21
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal abstract Delegate CreateServerAsyncEnumerableTaskDelegate { get; }
        /// <summary>
        /// 获取接口任务以后检查是否完成
        /// </summary>
        internal abstract Delegate CommandServerAsyncEnumerableQueueTaskCheckCallTaskDelegate { get; }
        /// <summary>
        /// 获取命令服务套接字
        /// </summary>
        internal abstract Delegate CommandServerAsyncEnumerableQueueTaskGetSocketDelegate { get; }
        /// <summary>
        /// 服务端异步调用队列任务类型
        /// </summary>
        internal abstract Type CommandServerAsyncEnumerableQueueTaskType { get; }
        /// <summary>
        /// 获取 IAsyncEnumerable
        /// </summary>
        internal abstract Delegate CommandClientEnumeratorCommandGetAsyncEnumerableDelegate { get; }
        /// <summary>
        /// 获取命令服务 Task 队列
        /// </summary>
        internal abstract Delegate AsyncEnumerableQueueTaskGetTaskQueueDelegate { get; }
#endif
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal abstract Delegate CreateCommandServerKeepCallbackTaskQueueDelegate { get; }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal abstract Delegate CreateCommandServerKeepCallbackCountTaskQueueDelegate { get; }
        /// <summary>
        /// 服务端执行队列任务发送数据
        /// </summary>
        internal abstract Delegate CommandServerCallQueueSendReturnValueDelegate { get; }
        /// <summary>
        /// 服务端执行队列任务发送数据
        /// </summary>
        internal abstract Delegate CommandServerCallReadWriteQueueSendReturnValueDelegate { get; }
        /// <summary>
        /// 服务端执行队列任务发送数据
        /// </summary>
        internal abstract Delegate CommandServerCallConcurrencyReadQueueSendReturnValueDelegate { get; }
        /// <summary>
        /// 发送数据
        /// </summary>
        internal abstract Delegate CommandServerCallSendDelegate { get; }

        /// <summary>
        /// 命令服务返回值类型
        /// </summary>
        internal abstract Type CommandServerReturnValueType { get; }
        /// <summary>
        /// 服务端异步调用队列任务类型
        /// </summary>
        internal abstract Type CommandServerCallTaskQueueTaskType { get; }
        /// <summary>
        /// 服务端异步调用队列任务类型
        /// </summary>
        internal abstract Type CommandServerKeepCallbackQueueTaskType { get; }
        /// <summary>
        /// 获取接口任务以后检查是否完成
        /// </summary>
        internal abstract Delegate CommandServerCallTaskQueueTaskCheckCallTaskDelegate { get; }
        /// <summary>
        /// 检查接口任务完成状态
        /// </summary>
        internal abstract Delegate CommandServerSocketCheckTaskDelegate { get; }
        /// <summary>
        /// 检查接口任务完成状态
        /// </summary>
        internal abstract Delegate CommandServerCallbackTaskCheckTaskDelegate { get; }
        /// <summary>
        /// 检查接口任务完成状态
        /// </summary>
        internal abstract Delegate CommandServerKeepCallbackTaskCheckTaskDelegate { get; }
        /// <summary>
        /// 检查接口任务完成状态
        /// </summary>
        internal abstract Delegate CommandServerKeepCallbackTaskCheckTaskAutoCancelKeepDelegate { get; }
        /// <summary>
        /// 检查接口任务完成状态
        /// </summary>
        internal abstract Delegate CommandServerKeepCallbackTaskCheckCountTaskDelegate { get; }
        /// <summary>
        /// 检查接口任务完成状态
        /// </summary>
        internal abstract Delegate CommandServerKeepCallbackTaskCheckCountTaskAutoCancelKeepDelegate { get; }
        /// <summary>
        /// 获取命令服务套接字
        /// </summary>
        internal abstract Delegate CommandServerKeepCallbackQueueTaskGetSocketDelegate { get; }
        /// <summary>
        /// 获取接口任务以后检查是否完成
        /// </summary>
        internal abstract Delegate CommandServerKeepCallbackQueueTaskCheckCallTaskDelegate { get; }
        /// <summary>
        /// Task.Run 异步任务类型
        /// </summary>
        internal abstract Type CommandServerRunTaskType { get; }
        /// <summary>
        /// 设置参数是否反序列化成功
        /// </summary>
        internal abstract Delegate CommandServerRunTaskSetIsDeserializeDelegate { get; }
        /// <summary>
        /// 任务调用
        /// </summary>
        internal abstract Delegate CommandServerRunTaskIsDeserializeDelegate { get; }
        /// <summary>
        /// Task.Run 异步任务类型
        /// </summary>
        internal abstract Type CommandServerCallbackRunTaskType { get; }
        /// <summary>
        /// 设置参数是否反序列化成功
        /// </summary>
        internal abstract Delegate CommandServerCallbackRunTaskSetIsDeserializeDelegate { get; }
        /// <summary>
        /// 任务调用
        /// </summary>
        internal abstract Delegate CommandServerCallbackRunTaskIsDeserializeDelegate { get; }
        /// <summary>
        /// Task.Run 异步任务类型
        /// </summary>
        internal abstract Type CommandServerKeepCallbackRunTaskType { get; }
        /// <summary>
        /// 设置参数是否反序列化成功
        /// </summary>
        internal abstract Delegate CommandServerKeepCallbackRunTaskSetIsDeserializeDelegate { get; }
        /// <summary>
        /// 任务调用
        /// </summary>
        internal abstract Delegate CommandServerKeepCallbackRunTaskIsDeserializeDelegate { get; }
        /// <summary>
        /// 任务调用
        /// </summary>
        internal abstract Delegate CommandServerKeepCallbackRunTaskAutoCancelKeepIsDeserializeDelegate { get; }
        /// <summary>
        /// Task.Run 异步任务类型
        /// </summary>
        internal abstract Type CommandServerKeepCallbackCountRunTaskType { get; }
        /// <summary>
        /// 设置参数是否反序列化成功
        /// </summary>
        internal abstract Delegate CommandServerKeepCallbackCountRunTaskSetIsDeserializeDelegate { get; }
        /// <summary>
        /// 任务调用
        /// </summary>
        internal abstract Delegate CommandServerKeepCallbackCountRunTaskIsDeserializeDelegate { get; }
        /// <summary>
        /// 任务调用
        /// </summary>
        internal abstract Delegate CommandServerKeepCallbackCountRunTaskAutoCancelKeepIsDeserializeDelegate { get; }
        /// <summary>
        /// Task.Run 异步任务类型
        /// </summary>
        internal abstract Type CommandServerEnumerableKeepCallbackCountRunTaskType { get; }
        /// <summary>
        /// 设置参数是否反序列化成功
        /// </summary>
        internal abstract Delegate CommandServerEnumerableKeepCallbackCountRunTaskSetIsDeserializeDelegate { get; }
        /// <summary>
        /// 任务调用
        /// </summary>
        internal abstract Delegate CommandServerEnumerableKeepCallbackCountRunTaskIsDeserializeDelegate { get; }
        /// <summary>
        /// 客户端获取命令服务返回值
        /// </summary>
        internal abstract Delegate GetCommandServerReturnValueDelegate { get; }
        /// <summary>
        /// 设置客户端命令服务返回值
        /// </summary>
        internal abstract Delegate SetCommandServerReturnValueDelegate { get; }
        /// <summary>
        /// 获取命令服务 Task 队列
        /// </summary>
        internal abstract Delegate CommandServerKeepCallbackQueueTaskGetTaskQueueDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static GenericType create<T>()
        {
            return new GenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static GenericType? lastGenericType;
#else
        protected static GenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static GenericType Get(Type type)
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
    internal sealed class GenericType<T> : GenericType
    {
        /// <summary>
        /// 引用类型数组
        /// </summary>
        internal static readonly Type[] ReferenceTypes = new Type[] { typeof(T) };
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }
#if !AOT
        /// <summary>
        /// 判断成员索引是否有效
        /// </summary>
        internal override Delegate GetMemberMapIsMemberDelegate { get { return (Func<MemberMap<T>, int, bool>)MemberMap<T>.IsMember; } }
        /// <summary>
        /// 设置成员索引
        /// </summary>
        internal override Delegate GetMemberMapSetMemberDelegate { get { return (Action<MemberMap<T>, int>)MemberMap<T>.SetMember; } }
        /// <summary>
        /// 获取成员委托类型
        /// </summary>
        internal override Type GetMemberMapType { get { return typeof(MemberMap<T>); } }
        ///// <summary>
        ///// 获取成员索引分组
        ///// </summary>
        //internal override AutoCSer.Metadata.MemberIndexGroup GetMemberIndexGroup { get { return AutoCSer.Metadata.MemberIndexGroup<T>.Get(); } }
        /// <summary>
        /// 是否存在默认构造函数
        /// </summary>
        internal override bool IsSerializeConstructor { get { return GetIsSerializeConstructor(); } }
        /// <summary>
        /// 是否存在默认构造函数
        /// </summary>
        /// <returns></returns>
        internal static bool GetIsSerializeConstructor()
        {
            return DefaultConstructor<T>.Type != DefaultConstructorTypeEnum.None;
        }
        /// <summary>
        /// 数组字串构造函数
        /// </summary>
        internal override Delegate LeftArrayDefaultConstructorDelegate { get { return (Func<LeftArray<T>>)DefaultConstructor.LeftArrayDefaultConstructor<T>; } }
        ///// <summary>
        ///// 获取字段集合（包括匿名字段）
        ///// </summary>
        //internal override GetAnonymousFieldsDelegate GetAnonymousFields { get { return MemberIndexGroup<T>.GetAnonymousFields; } }
        ///// <summary>
        ///// 追加字段信息（包括匿名字段）
        ///// </summary>
        //internal override AppendAnonymousFieldDelegate AppendAnonymousField { get { return AutoCSer.BinarySerialize.Common.AppendAnonymousField<T>; } }

        /// <summary>
        /// JSON 序列化委托循环引用信息
        /// </summary>
        internal override AutoCSer.TextSerialize.DelegateReference JsonSerializeDelegateReference { get { return AutoCSer.Json.TypeSerializer<T>.SerializeDelegateReference; } }
        /// <summary>
        /// 获取 JSON 序列化函数信息
        /// </summary>
        internal override Action<AutoCSer.JsonSerializer, object> JsonSerializeObjectDelegate { get { return (Action<AutoCSer.JsonSerializer, object>)AutoCSer.JsonSerializer.Object<T>; } }
        /// <summary>
        /// JSON 自定义序列化引用类型
        /// </summary>
        internal override Delegate JsonSerializeDelegate { get { return (Action<AutoCSer.JsonSerializer, T>)AutoCSer.JsonSerializer.Serialize<T>; } }
        /// <summary>
        /// JSON 自定义序列化不支持类型
        /// </summary>
        internal override Delegate JsonSerializeNotSupportDelegate { get { return (Action<AutoCSer.JsonSerializer, T>)AutoCSer.JsonSerializer.NotSupport<T>; } }
        /// <summary>
        /// JSON 序列化数组
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal override void GetJsonSerializeArrayDelegate(ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
        {
            serializeDelegateReference.SetMember((Action<AutoCSer.JsonSerializer, T[]>)AutoCSer.JsonSerializer.Array<T>, ReferenceTypes);
        }
        /// <summary>
        /// JSON 反序列化数组
        /// </summary>
#if NetStandard21
        internal override Delegate JsonDeserializeArrayDelegate { get { return (AutoCSer.JsonDeserializer.DeserializeDelegate<T?[]?>)AutoCSer.JsonDeserializer.Array<T>; } }
#else
        internal override Delegate JsonDeserializeArrayDelegate { get { return (AutoCSer.JsonDeserializer.DeserializeDelegate<T[]>)AutoCSer.JsonDeserializer.Array<T>; } }
#endif
        /// <summary>
        /// JSON 自定义反序列化不支持类型
        /// </summary>
#if NetStandard21
        internal override Delegate JsonDeserializeNotSupportDelegate { get { return (AutoCSer.JsonDeserializer.DeserializeDelegate<T?>)AutoCSer.JsonDeserializer.NotSupport<T>; } }
#else
        internal override Delegate JsonDeserializeNotSupportDelegate { get { return (AutoCSer.JsonDeserializer.DeserializeDelegate<T>)AutoCSer.JsonDeserializer.NotSupport<T>; } }
#endif
        /// <summary>
        /// JSON 反序列化类型
        /// </summary>
#if NetStandard21
        internal override Delegate JsonDeserializeDelegate { get { return (JsonDeserializer.DeserializeDelegate<T?>)AutoCSer.JsonDeserializer.Deserialize<T>; } }
#else
        internal override Delegate JsonDeserializeDelegate { get { return (JsonDeserializer.DeserializeDelegate<T>)AutoCSer.JsonDeserializer.Deserialize<T>; } }
#endif

        /// <summary>
        /// JSON 序列化数组
        /// </summary>
        internal override AutoCSer.TextSerialize.SerializeDelegate JsonSerializeLeftArrayDelegate { get { return new AutoCSer.TextSerialize.SerializeDelegate((Action<AutoCSer.JsonSerializer, LeftArray<T>>)JsonSerializer.LeftArray<T>, ReferenceTypes); } }
        /// <summary>
        /// JSON 序列化数组
        /// </summary>
        internal override AutoCSer.TextSerialize.SerializeDelegate JsonSerializeListArrayDelegate { get { return new AutoCSer.TextSerialize.SerializeDelegate((Action<AutoCSer.JsonSerializer, ListArray<T>>)JsonSerializer.ListArray<T>, ReferenceTypes); } }
        /// <summary>
        /// JSON 反序列化数组
        /// </summary>
#if NetStandard21
        internal override Delegate JsonDeserializeLeftArrayDelegate { get { return (AutoCSer.JsonDeserializer.DeserializeDelegate<LeftArray<T?>>)JsonDeserializer.LeftArray<T>; } }
#else
        internal override Delegate JsonDeserializeLeftArrayDelegate { get { return (AutoCSer.JsonDeserializer.DeserializeDelegate<LeftArray<T>>)JsonDeserializer.LeftArray<T>; } }
#endif
        /// <summary>
        /// JSON 反序列化数组
        /// </summary>
#if NetStandard21
        internal override Delegate JsonDeserializeListArrayDelegate { get { return (AutoCSer.JsonDeserializer.DeserializeDelegate<ListArray<T?>?>)JsonDeserializer.ListArray<T>; } }
#else
        internal override Delegate JsonDeserializeListArrayDelegate { get { return (AutoCSer.JsonDeserializer.DeserializeDelegate<ListArray<T>>)JsonDeserializer.ListArray<T>; } }
#endif

        /// <summary>
        /// 二进制序列化委托循环引用信息
        /// </summary>
        internal override AutoCSer.BinarySerialize.SerializeDelegateReference BinarySerializeDelegateReference { get { return AutoCSer.BinarySerialize.TypeSerializer<T>.SerializeDelegateReference; } }
        /// <summary>
        /// 二进制序列化
        /// </summary>
        internal override Delegate BinarySerializeDelegate
        {
            get
            {
                return (Action<AutoCSer.BinarySerializer, T>)AutoCSer.BinarySerializer.Serialize<T>;
            }
        }
        /// <summary>
        /// 二进制序列化数组
        /// </summary>
        internal override Delegate BinarySerializeStructLeftArrayDelegate { get { return (Action<AutoCSer.BinarySerializer, LeftArray<T>>)AutoCSer.BinarySerializer.StructLeftArray<T>; } }
        /// <summary>
        /// 二进制序列化数组
        /// </summary>
        internal override Delegate BinarySerializeStructListArrayDelegate { get { return (Action<AutoCSer.BinarySerializer, ListArray<T>>)AutoCSer.BinarySerializer.StructListArray<T>; } }
        /// <summary>
        /// 二进制自定义序列化不支持类型
        /// </summary>
        internal override Delegate BinarySerializeNotSupportDelegate { get { return (Action<AutoCSer.BinarySerializer, T>)AutoCSer.BinarySerializer.NotSupport<T>; } }
        /// <summary>
        /// 二进制自定义反序列化不支持类型
        /// </summary>
#if NetStandard21
        internal override Delegate BinaryDeserializeNotSupportDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T?>)BinaryDeserializer.NotSupport<T>; } }
#else
        internal override Delegate BinaryDeserializeNotSupportDelegate { get { return (AutoCSer.BinaryDeserializer.DeserializeDelegate<T>)BinaryDeserializer.NotSupport<T>; } }
#endif
        /// <summary>
        /// 二进制混杂 JSON 反序列化
        /// </summary>
        internal override Delegate BinaryDeserializeStructJsonDelegate { get { return (BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.StructJson<T>; } }
        /// <summary>
        /// 二进制混杂 JSON 反序列化
        /// </summary>
#if NetStandard21
        internal override Delegate BinaryDeserializeJsonDelegate { get { return (BinaryDeserializer.DeserializeDelegate<T?>)AutoCSer.BinaryDeserializer.Json<T>; } }
#else
        internal override Delegate BinaryDeserializeJsonDelegate { get { return (BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.Json<T>; } }
#endif
        /// <summary>
        /// 二进制反序列化类型
        /// </summary>
#if NetStandard21
        internal override Delegate BinaryDeserializeDelegate { get { return (BinaryDeserializer.DeserializeDelegate<T?>)AutoCSer.BinaryDeserializer.Deserialize<T>; } }
#else
        internal override Delegate BinaryDeserializeDelegate { get { return (BinaryDeserializer.DeserializeDelegate<T>)AutoCSer.BinaryDeserializer.Deserialize<T>; } }
#endif

        /// <summary>
        /// JSON 序列化
        /// </summary>
#if NetStandard21
        internal override Func<object, JsonSerializeConfig?, KeyValue<string, AutoCSer.TextSerialize.WarningEnum>> JsonSerializeObjectGenericDelegate { get { return JsonSerializer.Serialize<T>; } }
#else
        internal override Func<object, JsonSerializeConfig, KeyValue<string, AutoCSer.TextSerialize.WarningEnum>> JsonSerializeObjectGenericDelegate { get { return JsonSerializer.Serialize<T>; } }
#endif
        /// <summary>
        /// JSON 序列化
        /// </summary>
#if NetStandard21
        internal override Func<object, CharStream, JsonSerializeConfig?, AutoCSer.TextSerialize.WarningEnum> JsonSerializeStreamObjectDelegate { get { return JsonSerializer.Serialize<T>; } }
#else
        internal override Func<object, CharStream, JsonSerializeConfig, AutoCSer.TextSerialize.WarningEnum> JsonSerializeStreamObjectDelegate { get { return JsonSerializer.Serialize<T>; } }
#endif
        /// <summary>
        /// 二进制序列化数组
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal override void GetBinarySerializeStructArrayDelegate(ref AutoCSer.BinarySerialize.SerializeDelegateReference serializeDelegateReference)
        {
            serializeDelegateReference.SetMember((Action<AutoCSer.BinarySerializer, T[]>)AutoCSer.BinarySerializer.StructArray<T>, ReferenceTypes, BinarySerialize.SerializePushTypeEnum.Primitive, true);
        }
        /// <summary>
        /// 二进制混杂 JSON 序列化
        /// </summary>
        internal override Delegate BinarySerializeJsonDelegate { get { return (Action<AutoCSer.BinarySerializer, T>)AutoCSer.BinarySerializer.Json<T>; } }
        /// <summary>
        /// 二进制混杂 JSON 序列化
        /// </summary>
        internal override Delegate BinarySerializeMemberJsonDelegate
        {
            get
            {
                return BinarySerializeJsonDelegate;
            }
        }
        /// <summary>
        /// 二进制混杂 JSON 序列化
        /// </summary>
        internal override Delegate BinarySerializeStructJsonDelegate { get { return (Action<AutoCSer.BinarySerializer, T>)AutoCSer.BinarySerializer.StructJson<T>; } }
        /// <summary>
        /// 二进制混杂 JSON 序列化
        /// </summary>
        internal override Delegate BinarySerializeMemberStructJsonDelegate
        {
            get
            {
                return BinarySerializeStructJsonDelegate;
            }
        }
        /// <summary>
        /// 获取二进制序列化函数信息
        /// </summary>
        /// <returns></returns>
        internal override Action<AutoCSer.BinarySerializer, object> BinarySerializeRealTypeObjectDelegate { get { return AutoCSer.BinarySerializer.RealTypeObject<T>; } }
        /// <summary>
        /// 获取二进制反序列化函数信息
        /// </summary>
        /// <returns></returns>
#if NetStandard21
        internal override Func<AutoCSer.BinaryDeserializer, object?> BinaryDeserializeRealTypeObjectDelegate { get { return AutoCSer.BinaryDeserializer.RealTypeObject<T>; } }
#else
        internal override Func<AutoCSer.BinaryDeserializer, object> BinaryDeserializeRealTypeObjectDelegate { get { return AutoCSer.BinaryDeserializer.RealTypeObject<T>; } }
#endif
        /// <summary>
        /// 获取命令客户端返回值委托
        /// </summary>
        internal override Delegate GetCommandClientReturnValueDelegate { get { return (Func<T, AutoCSer.Net.CommandClientReturnValue<T>>)AutoCSer.Net.CommandClientReturnValue<T>.GetReturnValue; } }
        /// <summary>
        /// 获取命令客户端错误返回值委托
        /// </summary>
        internal override Delegate GetCommandClientReturnTypeDelegate { get { return (Func<AutoCSer.Net.CommandClientReturnValue, AutoCSer.Net.CommandClientReturnValue<T>>)AutoCSer.Net.CommandClientReturnValue<T>.GetReturnValue; } }
        /// <summary>
        /// 命令客户端回调委托
        /// </summary>
        internal override Delegate CommandClientControllerCallbackDelegate { get { return (Func<AutoCSer.Net.CommandClientController, int, AutoCSer.Net.CommandClientCallback<T>, AutoCSer.Net.CallbackCommand>)AutoCSer.Net.CommandClientController.Callback<T>; } }
        /// <summary>
        /// 命令客户端保持回调委托
        /// </summary>
        internal override Delegate CommandClientControllerKeepCallbackDelegate { get { return (Func<AutoCSer.Net.CommandClientController, int, AutoCSer.Net.CommandClientKeepCallback<T>, AutoCSer.Net.KeepCallbackCommand>)AutoCSer.Net.CommandClientController.KeepCallback<T>; } }
        /// <summary>
        /// 命令客户端队列回调委托
        /// </summary>
        internal override Delegate CommandClientControllerCallbackQueueDelegate { get { return (Func<AutoCSer.Net.CommandClientController, int, AutoCSer.Net.CommandClientCallbackQueueNode<T>, AutoCSer.Net.CallbackCommand>)AutoCSer.Net.CommandClientController.CallbackQueue<T>; } }
        /// <summary>
        /// 命令客户端队列保持回调委托
        /// </summary>
        internal override Delegate CommandClientControllerKeepCallbackQueueDelegate { get { return (Func<AutoCSer.Net.CommandClientController, int, AutoCSer.Net.CommandClientKeepCallbackQueue<T>, AutoCSer.Net.KeepCallbackCommand>)AutoCSer.Net.CommandClientController.KeepCallbackQueue<T>; } }
        /// <summary>
        /// 命令客户端返回值委托
        /// </summary>
        internal override Delegate CommandClientControllerReturnValueDelegate { get { return (Func<AutoCSer.Net.CommandClientController, int, AutoCSer.Net.ReturnCommand<T>>)AutoCSer.Net.CommandClientController.ReturnValue<T>; } }
        /// <summary>
        /// 获取 Task 委托
        /// </summary>
        internal override Delegate CommandClientReturnCommandGetTaskDelegate { get { return (Func<AutoCSer.Net.ReturnCommand<T>, Task<T>>)AutoCSer.Net.ReturnCommand<T>.GetTask; } }
        /// <summary>
        /// 命令客户端队列返回值委托
        /// </summary>
        internal override Delegate CommandClientControllerReturnValueQueueDelegate { get { return (Func<AutoCSer.Net.CommandClientController, int, AutoCSer.Net.ReturnQueueCommand<T>>)AutoCSer.Net.CommandClientController.ReturnValueQueue<T>; } }
        /// <summary>
        /// 命令客户端枚举返回值委托
        /// </summary>
        internal override Delegate CommandClientControllerEnumeratorDelegate { get { return (Func<AutoCSer.Net.CommandClientController, int, AutoCSer.Net.EnumeratorCommand<T>>)AutoCSer.Net.CommandClientController.Enumerator<T>; } }
        /// <summary>
        /// 命令客户端队列枚举返回值委托
        /// </summary>
        internal override Delegate CommandClientControllerEnumeratorQueueDelegate { get { return (Func<AutoCSer.Net.CommandClientController, int, AutoCSer.Net.EnumeratorQueueCommand<T>>)AutoCSer.Net.CommandClientController.EnumeratorQueue<T>; } }
        /// <summary>
        /// 获取客户端回调委托
        /// </summary>
        internal override Delegate GetCommandClientCallbackDelegate { get { return (Func<Action<AutoCSer.Net.CommandClientReturnValue<T>>, AutoCSer.Net.CommandClientCallback<T>>)AutoCSer.Net.CommandClientCallback<T>.Get; } }
        /// <summary>
        /// 获取客户端回调委托
        /// </summary>
        internal override Delegate GetCommandClientKeepCallbackDelegate { get { return (Func<Action<AutoCSer.Net.CommandClientReturnValue<T>, AutoCSer.Net.KeepCallbackCommand>, AutoCSer.Net.CommandClientKeepCallback<T>>)AutoCSer.Net.CommandClientKeepCallback<T>.Get; } }
        /// <summary>
        /// 获取客户端回调委托
        /// </summary>
        internal override Delegate GetCommandClientCallbackQueueDelegate { get { return (Func<Action<AutoCSer.Net.CommandClientReturnValue<T>, AutoCSer.Net.CommandClientCallQueue>, AutoCSer.Net.CommandClientCallbackQueueNode<T>>)AutoCSer.Net.CommandClientCallbackQueueNode<T>.Get; } }
        /// <summary>
        /// 获取客户端回调委托
        /// </summary>
        internal override Delegate GetCommandClientKeepCallbackQueueDelegate { get { return (Func<Action<AutoCSer.Net.CommandClientReturnValue<T>, AutoCSer.Net.CommandClientCallQueue, AutoCSer.Net.KeepCallbackCommand>, AutoCSer.Net.CommandClientKeepCallbackQueue<T>>)AutoCSer.Net.CommandClientKeepCallbackQueue<T>.Get; } }
#endif

        /// <summary>
        /// Task 类型
        /// </summary>
        internal override Type TaskType { get { return typeof(Task<T>); } }

        /// <summary>
        /// 获取控制器创建器
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
#if NetStandard21
        internal override AutoCSer.Net.CommandServerInterfaceControllerCreator GetCommandServerInterfaceControllerCreator(object controller, string? controllerName)
#else
        internal override AutoCSer.Net.CommandServerInterfaceControllerCreator GetCommandServerInterfaceControllerCreator(object controller, string controllerName)
#endif
        {
            return AutoCSer.Net.CommandServerInterfaceControllerCreator.GetCreator((T)controller, controllerName);
        }
        /// <summary>
        /// 发送返回值数据
        /// </summary>
        internal override Delegate CommandServerSocketSendReturnValueDelegate { get { return (Func<AutoCSer.Net.CommandServerSocket, AutoCSer.Net.CommandServer.ServerInterfaceMethod, T, bool>)AutoCSer.Net.CommandServerSocket.SendReturnValue; } }
        /// <summary>
        /// TCP 服务器端异步回调类型
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal override Type GetCommandServerCallbackType(ServerInterfaceMethod method)
        {
            switch (method.MethodType)
            {
                case ServerMethodTypeEnum.CallbackTask: return typeof(AutoCSer.Net.CommandServerCallbackTask<T>);
                default: return typeof(AutoCSer.Net.CommandServerCallback<T>);
            }
        }
        /// <summary>
        /// TCP 服务器端异步成功回调
        /// </summary>
        internal override Delegate CommandServerCallbackDelegate { get { return (Func<AutoCSer.Net.CommandServerCallback<T>, AutoCSer.Net.CommandServer.ServerInterfaceMethod, T, bool>)AutoCSer.Net.CommandServerCallback<T>.Callback; } }
        /// <summary>
        /// TCP 服务器端异步成功回调
        /// </summary>
        internal override Delegate CommandServerSynchronousCallbackDelegate { get { return (Func<AutoCSer.Net.CommandServerCallQueue, AutoCSer.Net.CommandServerCallback<T>, AutoCSer.Net.CommandServer.ServerInterfaceMethod, T, bool>)AutoCSer.Net.CommandServerCallback<T>.SynchronousCallback; } }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal override Delegate CreateCommandServerKeepCallbackDelegate { get { return (Func<AutoCSer.Net.CommandServerSocket, AutoCSer.Net.CommandServer.ServerInterfaceMethod, AutoCSer.Net.CommandServerKeepCallback<T>>)AutoCSer.Net.CommandServerKeepCallback<T>.CreateServerKeepCallback; } }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal override Delegate CreateCommandServerKeepCallbackCountDelegate { get { return (Func<AutoCSer.Net.CommandServerSocket, AutoCSer.Net.CommandServer.ServerInterfaceMethod, AutoCSer.Net.CommandServerKeepCallbackCount<T>>)AutoCSer.Net.CommandServerKeepCallbackCount<T>.CreateServerKeepCallback; } }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal override Delegate CreateCommandServerKeepCallbackQueueNodeDelegate { get { return (Func<AutoCSer.Net.CommandServerCallQueueNode, AutoCSer.Net.CommandServer.ServerInterfaceMethod, AutoCSer.Net.CommandServerKeepCallback<T>>)AutoCSer.Net.CommandServerKeepCallback<T>.CreateServerKeepCallback; } }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal override Delegate CreateCommandServerKeepCallbackReadWriteQueueNodeDelegate { get { return (Func<AutoCSer.Net.CommandServerCallReadWriteQueueNode, AutoCSer.Net.CommandServer.ServerInterfaceMethod, AutoCSer.Net.CommandServerKeepCallback<T>>)AutoCSer.Net.CommandServerKeepCallback<T>.CreateServerKeepCallback; } }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal override Delegate CreateCommandServerKeepCallbackConcurrencyReadQueueNodeDelegate { get { return (Func<AutoCSer.Net.CommandServerCallConcurrencyReadQueueNode, AutoCSer.Net.CommandServer.ServerInterfaceMethod, AutoCSer.Net.CommandServerKeepCallback<T>>)AutoCSer.Net.CommandServerKeepCallback<T>.CreateServerKeepCallback; } }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal override Delegate CreateCommandServerKeepCallbackCountQueueNodeDelegate { get { return (Func<AutoCSer.Net.CommandServerCallQueueNode, AutoCSer.Net.CommandServer.ServerInterfaceMethod, AutoCSer.Net.CommandServerKeepCallbackCount<T>>)AutoCSer.Net.CommandServerKeepCallbackCount<T>.CreateServerKeepCallback; } }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal override Delegate CreateCommandServerKeepCallbackCountReadWriteQueueNodeDelegate { get { return (Func<AutoCSer.Net.CommandServerCallReadWriteQueueNode, AutoCSer.Net.CommandServer.ServerInterfaceMethod, AutoCSer.Net.CommandServerKeepCallbackCount<T>>)AutoCSer.Net.CommandServerKeepCallbackCount<T>.CreateServerKeepCallback; } }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal override Delegate CreateCommandServerKeepCallbackCountConcurrencyReadQueueNodeDelegate { get { return (Func<AutoCSer.Net.CommandServerCallConcurrencyReadQueueNode, AutoCSer.Net.CommandServer.ServerInterfaceMethod, AutoCSer.Net.CommandServerKeepCallbackCount<T>>)AutoCSer.Net.CommandServerKeepCallbackCount<T>.CreateServerKeepCallback; } }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal override Delegate CreateServerKeepCallbackTaskDelegate { get { return (Func<AutoCSer.Net.CommandServerSocket, AutoCSer.Net.CommandServer.ServerInterfaceMethod, AutoCSer.Net.CommandServerKeepCallbackTask<T>>)AutoCSer.Net.CommandServerKeepCallbackTask<T>.CreateServerKeepCallbackTask; } }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal override Delegate CreateServerKeepCallbackCountTaskDelegate { get { return (Func<AutoCSer.Net.CommandServerSocket, AutoCSer.Net.CommandServer.ServerInterfaceMethod, AutoCSer.Net.CommandServerKeepCallbackCountTask<T>>)AutoCSer.Net.CommandServerKeepCallbackCountTask<T>.CreateServerKeepCallbackTask; } }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal override Delegate CreateServerEnumerableKeepCallbackCountTaskDelegate { get { return (Action<AutoCSer.Net.CommandServerSocket, AutoCSer.Net.CommandServer.ServerInterfaceMethod, Task<IEnumerable<T>>>)AutoCSer.Net.CommandServerEnumerableKeepCallbackCountTask<T>.CreateServerKeepCallbackTask; } }
#if NetStandard21
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal override Delegate CreateServerAsyncEnumerableTaskDelegate { get { return (Action<AutoCSer.Net.CommandServerSocket, AutoCSer.Net.CommandServer.ServerInterfaceMethod, IAsyncEnumerable<T>>)AutoCSer.Net.CommandServerAsyncEnumerableTask<T>.CreateServerKeepCallbackTask; } }
        /// <summary>
        /// 获取接口任务以后检查是否完成
        /// </summary>
        internal override Delegate CommandServerAsyncEnumerableQueueTaskCheckCallTaskDelegate { get { return (Func<AutoCSer.Net.AsyncEnumerableQueueTask<T>, IAsyncEnumerable<T>, bool>)AutoCSer.Net.AsyncEnumerableQueueTask<T>.CheckCallTask; } }
        /// <summary>
        /// 获取命令服务套接字
        /// </summary>
        internal override Delegate CommandServerAsyncEnumerableQueueTaskGetSocketDelegate { get { return (AutoCSer.Net.AsyncEnumerableQueueTask<T>.GetSocketDelegate)AutoCSer.Net.AsyncEnumerableQueueTask<T>.GetSocket; } }
        /// <summary>
        /// 服务端异步调用队列任务类型
        /// </summary>
        internal override Type CommandServerAsyncEnumerableQueueTaskType { get { return typeof(AutoCSer.Net.AsyncEnumerableQueueTask<T>); } }
        /// <summary>
        /// 获取 IAsyncEnumerable
        /// </summary>
        internal override Delegate CommandClientEnumeratorCommandGetAsyncEnumerableDelegate { get { return (Func<AutoCSer.Net.EnumeratorCommand<T>, IAsyncEnumerable<T>>)AutoCSer.Net.EnumeratorCommand<T>.GetAsyncEnumerable; } }
        /// <summary>
        /// 获取命令服务 Task 队列
        /// </summary>
        internal override Delegate AsyncEnumerableQueueTaskGetTaskQueueDelegate { get { return (Func<AutoCSer.Net.AsyncEnumerableQueueTask<T>, AutoCSer.Net.CommandServer.ServerInterfaceMethod, AutoCSer.Net.CommandServerTaskQueueService?>)AutoCSer.Net.AsyncEnumerableQueueTask<T>.GetTaskQueue; } }
#endif
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal override Delegate CreateCommandServerKeepCallbackTaskQueueDelegate { get { return (Func<AutoCSer.Net.CommandServerCallTaskQueueNode, AutoCSer.Net.CommandServer.ServerInterfaceMethod, AutoCSer.Net.CommandServerKeepCallback<T>>)AutoCSer.Net.CommandServerKeepCallback<T>.CreateServerKeepCallback; } }
        /// <summary>
        /// 创建 TCP 服务器端异步回调对象
        /// </summary>
        internal override Delegate CreateCommandServerKeepCallbackCountTaskQueueDelegate { get { return (Func<AutoCSer.Net.CommandServerCallTaskQueueNode, AutoCSer.Net.CommandServer.ServerInterfaceMethod, AutoCSer.Net.CommandServerKeepCallbackCount<T>>)AutoCSer.Net.CommandServerKeepCallbackCount<T>.CreateServerKeepCallback; } }
        /// <summary>
        /// 服务端执行队列任务发送数据
        /// </summary>
        internal override Delegate CommandServerCallQueueSendReturnValueDelegate { get { return (Func<AutoCSer.Net.CommandServerCallQueueNode, AutoCSer.Net.CommandServerCallQueue, AutoCSer.Net.CommandServer.ServerInterfaceMethod, T, bool>)AutoCSer.Net.CommandServerCallQueueNode.SendReturnValue; } }
        /// <summary>
        /// 服务端执行队列任务发送数据
        /// </summary>
        internal override Delegate CommandServerCallReadWriteQueueSendReturnValueDelegate { get { return (Func<AutoCSer.Net.CommandServerCallReadWriteQueueNode, AutoCSer.Net.CommandServer.ServerInterfaceMethod, T, bool>)AutoCSer.Net.CommandServerCallReadWriteQueueNode.SendReturnValue; } }
        /// <summary>
        /// 服务端执行队列任务发送数据
        /// </summary>
        internal override Delegate CommandServerCallConcurrencyReadQueueSendReturnValueDelegate { get { return (Func<AutoCSer.Net.CommandServerCallConcurrencyReadQueueNode, AutoCSer.Net.CommandServer.ServerInterfaceMethod, T, bool>)AutoCSer.Net.CommandServerCallConcurrencyReadQueueNode.SendReturnValue; } }
        /// <summary>
        /// 发送数据
        /// </summary>
        internal override Delegate CommandServerCallSendDelegate { get { return (Func<AutoCSer.Net.CommandServerCall, AutoCSer.Net.CommandServer.ServerInterfaceMethod, T, bool>)AutoCSer.Net.CommandServerCall.Send<T>; } }

        /// <summary>
        /// 命令服务返回值类型
        /// </summary>
        internal override Type CommandServerReturnValueType { get { return typeof(AutoCSer.Net.CommandServer.ServerReturnValue<T>); } }
        /// <summary>
        /// 服务端异步调用队列任务类型
        /// </summary>
        internal override Type CommandServerCallTaskQueueTaskType { get { return typeof(AutoCSer.Net.CommandServerCallTaskQueueTask<T>); } }
        /// <summary>
        /// 服务端异步调用队列任务类型
        /// </summary>
        internal override Type CommandServerKeepCallbackQueueTaskType { get { return typeof(AutoCSer.Net.CommandServerKeepCallbackQueueTask<T>); } }
        /// <summary>
        /// 获取接口任务以后检查是否完成
        /// </summary>
        internal override Delegate CommandServerCallTaskQueueTaskCheckCallTaskDelegate { get { return (Func<AutoCSer.Net.CommandServerCallTaskQueueTask<T>, Task<T>, bool>)AutoCSer.Net.CommandServerCallTaskQueueTask<T>.CheckCallTask; } }
        /// <summary>
        /// 检查接口任务完成状态
        /// </summary>
        internal override Delegate CommandServerSocketCheckTaskDelegate { get { return (Action<AutoCSer.Net.CommandServerSocket, AutoCSer.Net.CommandServer.ServerInterfaceMethod, Task<T>>)AutoCSer.Net.CommandServerSocket.CheckTask<T>; } }
        /// <summary>
        /// 检查接口任务完成状态
        /// </summary>
        internal override Delegate CommandServerCallbackTaskCheckTaskDelegate { get { return (Action<AutoCSer.Net.CommandServerCallbackTask<T>, Task>)AutoCSer.Net.CommandServerCallbackTask<T>.CheckTask; } }
        /// <summary>
        /// 检查接口任务完成状态
        /// </summary>
        internal override Delegate CommandServerKeepCallbackTaskCheckTaskDelegate { get { return (Action<AutoCSer.Net.CommandServerKeepCallbackTask<T>, Task>)AutoCSer.Net.CommandServerKeepCallbackTask<T>.CheckTask; } }
        /// <summary>
        /// 检查接口任务完成状态
        /// </summary>
        internal override Delegate CommandServerKeepCallbackTaskCheckTaskAutoCancelKeepDelegate { get { return (Action<AutoCSer.Net.CommandServerKeepCallbackTask<T>, Task>)AutoCSer.Net.CommandServerKeepCallbackTask<T>.CheckTaskAutoCancelKeep; } }
        /// <summary>
        /// 检查接口任务完成状态
        /// </summary>
        internal override Delegate CommandServerKeepCallbackTaskCheckCountTaskDelegate { get { return (Action<AutoCSer.Net.CommandServerKeepCallbackCountTask<T>, Task>)AutoCSer.Net.CommandServerKeepCallbackCountTask<T>.CheckTask; } }
        /// <summary>
        /// 检查接口任务完成状态
        /// </summary>
        internal override Delegate CommandServerKeepCallbackTaskCheckCountTaskAutoCancelKeepDelegate { get { return (Action<AutoCSer.Net.CommandServerKeepCallbackCountTask<T>, Task>)AutoCSer.Net.CommandServerKeepCallbackCountTask<T>.CheckTaskAutoCancelKeep; } }
        /// <summary>
        /// 获取命令服务套接字
        /// </summary>
        internal override Delegate CommandServerKeepCallbackQueueTaskGetSocketDelegate { get { return (AutoCSer.Net.CommandServerKeepCallbackQueueTask<T>.GetSocketDelegate)AutoCSer.Net.CommandServerKeepCallbackQueueTask<T>.GetSocket; } }
        /// <summary>
        /// 获取接口任务以后检查是否完成
        /// </summary>
        internal override Delegate CommandServerKeepCallbackQueueTaskCheckCallTaskDelegate { get { return (Func<AutoCSer.Net.CommandServerKeepCallbackQueueTask<T>, Task<IEnumerable<T>>, bool>)AutoCSer.Net.CommandServerKeepCallbackQueueTask<T>.CheckCallTask; } }
        /// <summary>
        /// Task.Run 异步任务类型
        /// </summary>
        internal override Type CommandServerRunTaskType { get { return typeof(AutoCSer.Net.CommandServerRunTask<T>); } }
        /// <summary>
        /// 设置参数是否反序列化成功
        /// </summary>
        internal override Delegate CommandServerRunTaskSetIsDeserializeDelegate { get { return (Action<AutoCSer.Net.CommandServerRunTask<T>, bool>)AutoCSer.Net.CommandServerRunTask<T>.SetIsDeserialize; } }
        /// <summary>
        /// 任务调用
        /// </summary>
        internal override Delegate CommandServerRunTaskIsDeserializeDelegate { get { return (Func<AutoCSer.Net.CommandServerRunTask<T>, CommandClientReturnTypeEnum>)AutoCSer.Net.CommandServerRunTask<T>.RunTaskIsDeserialize; } }
        /// <summary>
        /// Task.Run 异步任务类型
        /// </summary>
        internal override Type CommandServerCallbackRunTaskType { get { return typeof(AutoCSer.Net.CommandServerCallbackRunTask<T>); } }
        /// <summary>
        /// 设置参数是否反序列化成功
        /// </summary>
        internal override Delegate CommandServerCallbackRunTaskSetIsDeserializeDelegate { get { return (Action<AutoCSer.Net.CommandServerCallbackRunTask<T>, bool>)AutoCSer.Net.CommandServerCallbackRunTask<T>.SetIsDeserialize; } }
        /// <summary>
        /// 任务调用
        /// </summary>
        internal override Delegate CommandServerCallbackRunTaskIsDeserializeDelegate { get { return (Func<AutoCSer.Net.CommandServerCallbackRunTask<T>, CommandClientReturnTypeEnum>)AutoCSer.Net.CommandServerCallbackRunTask<T>.RunTaskIsDeserialize; } }
        /// <summary>
        /// Task.Run 异步任务类型
        /// </summary>
        internal override Type CommandServerKeepCallbackRunTaskType { get { return typeof(AutoCSer.Net.CommandServerKeepCallbackRunTask<T>); } }
        /// <summary>
        /// 设置参数是否反序列化成功
        /// </summary>
        internal override Delegate CommandServerKeepCallbackRunTaskSetIsDeserializeDelegate { get { return (Action<AutoCSer.Net.CommandServerKeepCallbackRunTask<T>, bool>)AutoCSer.Net.CommandServerKeepCallbackRunTask<T>.SetIsDeserialize; } }
        /// <summary>
        /// 任务调用
        /// </summary>
        internal override Delegate CommandServerKeepCallbackRunTaskIsDeserializeDelegate { get { return (Func<AutoCSer.Net.CommandServerKeepCallbackRunTask<T>, CommandClientReturnTypeEnum>)AutoCSer.Net.CommandServerKeepCallbackRunTask<T>.RunTaskIsDeserialize; } }
        /// <summary>
        /// 任务调用
        /// </summary>
        internal override Delegate CommandServerKeepCallbackRunTaskAutoCancelKeepIsDeserializeDelegate { get { return (Func<AutoCSer.Net.CommandServerKeepCallbackRunTask<T>, CommandClientReturnTypeEnum>)AutoCSer.Net.CommandServerKeepCallbackRunTask<T>.RunTaskAutoCancelKeepIsDeserialize; } }
        /// <summary>
        /// Task.Run 异步任务类型
        /// </summary>
        internal override Type CommandServerKeepCallbackCountRunTaskType { get { return typeof(AutoCSer.Net.CommandServerKeepCallbackCountRunTask<T>); } }
        /// <summary>
        /// 设置参数是否反序列化成功
        /// </summary>
        internal override Delegate CommandServerKeepCallbackCountRunTaskSetIsDeserializeDelegate { get { return (Action<AutoCSer.Net.CommandServerKeepCallbackCountRunTask<T>, bool>)AutoCSer.Net.CommandServerKeepCallbackCountRunTask<T>.SetIsDeserialize; } }
        /// <summary>
        /// 任务调用
        /// </summary>
        internal override Delegate CommandServerKeepCallbackCountRunTaskIsDeserializeDelegate { get { return (Func<AutoCSer.Net.CommandServerKeepCallbackCountRunTask<T>, CommandClientReturnTypeEnum>)AutoCSer.Net.CommandServerKeepCallbackCountRunTask<T>.RunTaskIsDeserialize; } }
        /// <summary>
        /// 任务调用
        /// </summary>
        internal override Delegate CommandServerKeepCallbackCountRunTaskAutoCancelKeepIsDeserializeDelegate { get { return (Func<AutoCSer.Net.CommandServerKeepCallbackCountRunTask<T>, CommandClientReturnTypeEnum>)AutoCSer.Net.CommandServerKeepCallbackCountRunTask<T>.RunTaskAutoCancelKeepIsDeserialize; } }
        /// <summary>
        /// Task.Run 异步任务类型
        /// </summary>
        internal override Type CommandServerEnumerableKeepCallbackCountRunTaskType { get { return typeof(AutoCSer.Net.CommandServerEnumerableKeepCallbackCountRunTask<T>); } }
        /// <summary>
        /// 设置参数是否反序列化成功
        /// </summary>
        internal override Delegate CommandServerEnumerableKeepCallbackCountRunTaskSetIsDeserializeDelegate { get { return (Action<AutoCSer.Net.CommandServerEnumerableKeepCallbackCountRunTask<T>, bool>)AutoCSer.Net.CommandServerEnumerableKeepCallbackCountRunTask<T>.SetIsDeserialize; } }
        /// <summary>
        /// 任务调用
        /// </summary>
        internal override Delegate CommandServerEnumerableKeepCallbackCountRunTaskIsDeserializeDelegate { get { return (Func<AutoCSer.Net.CommandServerEnumerableKeepCallbackCountRunTask<T>, CommandClientReturnTypeEnum>)AutoCSer.Net.CommandServerEnumerableKeepCallbackCountRunTask<T>.RunTaskIsDeserialize; } }

        /// <summary>
        /// 命令客户端获取命令服务返回值
        /// </summary>
#if NetStandard21
        internal override Delegate GetCommandServerReturnValueDelegate { get { return (Func<AutoCSer.Net.CommandServer.ServerReturnValue<T>, T?>)AutoCSer.Net.CommandServer.ServerReturnValue<T>.GetReturnValue; } }
#else
        internal override Delegate GetCommandServerReturnValueDelegate { get { return (Func<AutoCSer.Net.CommandServer.ServerReturnValue<T>, T>)AutoCSer.Net.CommandServer.ServerReturnValue<T>.GetReturnValue; } }
#endif
        /// <summary>
        /// 设置客户端命令服务返回值
        /// </summary>
        internal override Delegate SetCommandServerReturnValueDelegate { get { return (AutoCSer.Net.CommandServer.ServerReturnValue<T>.SetReturnValueDelegate)AutoCSer.Net.CommandServer.ServerReturnValue<T>.SetReturnValue; } }
        /// <summary>
        /// 获取命令服务 Task 队列
        /// </summary>
#if NetStandard21
        internal override Delegate CommandServerKeepCallbackQueueTaskGetTaskQueueDelegate { get { return (Func<AutoCSer.Net.CommandServerKeepCallbackQueueTask<T>, AutoCSer.Net.CommandServer.ServerInterfaceMethod, AutoCSer.Net.CommandServerTaskQueueService?>)AutoCSer.Net.CommandServerKeepCallbackQueueTask<T>.GetTaskQueue; } }
#else
        internal override Delegate CommandServerKeepCallbackQueueTaskGetTaskQueueDelegate { get { return (Func<AutoCSer.Net.CommandServerKeepCallbackQueueTask<T>, AutoCSer.Net.CommandServer.ServerInterfaceMethod, AutoCSer.Net.CommandServerTaskQueueService>)AutoCSer.Net.CommandServerKeepCallbackQueueTask<T>.GetTaskQueue; } }
#endif
    }
}
