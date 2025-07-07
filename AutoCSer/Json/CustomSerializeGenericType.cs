using AutoCSer.Extensions;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Json
{
    /// <summary>
    /// 自定义 JSON 序列化泛型类型元数据
    /// </summary>
    internal abstract class CustomSerializeGenericType
    {
        /// <summary>
        /// Custom serialization委托
        /// </summary>
        internal abstract Delegate SerializeDelegate { get; }
        /// <summary>
        /// 自定义反序列化委托
        /// </summary>
        internal abstract Delegate DeserializeDelegate { get; }

        /// <summary>
        /// 创建自定义 JSON 序列化泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static CustomSerializeGenericType create<T>()
            where T : ICustomSerialize<T>
        {
            return new CustomSerializeGenericType<T>();
        }
        /// <summary>
        /// 创建泛型类型元数据 函数信息
        /// </summary>
        protected static readonly MethodInfo createMethod = typeof(CustomSerializeGenericType).GetMethod(nameof(create), BindingFlags.Static | BindingFlags.NonPublic).notNull();
        /// <summary>
        /// 获取自定义 JSON 序列化泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static CustomSerializeGenericType Get(Type type)
        {
            return (CustomSerializeGenericType)createMethod.MakeGenericMethod(type).Invoke(null, null).notNull();
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class CustomSerializeGenericType<T> : CustomSerializeGenericType
        where T : ICustomSerialize<T>
    {
        /// <summary>
        /// Custom serialization委托
        /// </summary>
        internal override Delegate SerializeDelegate { get { return (Action<JsonSerializer, T>)JsonSerializer.ICustom<T>; } }
        /// <summary>
        /// 自定义反序列化委托
        /// </summary>
#if NetStandard21
        internal override Delegate DeserializeDelegate { get { return (AutoCSer.JsonDeserializer.DeserializeDelegate<T?>)JsonDeserializer.ICustom<T>; } }
#else
        internal override Delegate DeserializeDelegate { get { return (AutoCSer.JsonDeserializer.DeserializeDelegate<T>)JsonDeserializer.ICustom<T>; } }
#endif
    }
}
