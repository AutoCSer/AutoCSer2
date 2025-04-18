using AutoCSer.Extensions;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 自定义 XML 序列化泛型类型元数据
    /// </summary>
    internal abstract class CustomSerializeGenericType
    {
        /// <summary>
        /// 自定义序列化委托
        /// </summary>
        internal abstract Delegate SerializeDelegate { get; }
        /// <summary>
        /// 自定义反序列化委托
        /// </summary>
        internal abstract Delegate DeserializeDelegate { get; }

        /// <summary>
        /// 创建自定义 XML 序列化泛型类型元数据
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
        /// 获取自定义 XML 序列化泛型类型元数据
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
        /// 自定义序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Serialize(AutoCSer.XmlSerializer serializer, T value)
        {
            value.Serialize(serializer);
        }
        /// <summary>
        /// 自定义序列化委托
        /// </summary>
        internal override Delegate SerializeDelegate { get { return (Action<AutoCSer.XmlSerializer, T>)Serialize; } }
        /// <summary>
        /// 自定义反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void Deserialize(AutoCSer.XmlDeserializer deserializer, ref T value)
        {
            value.Deserialize(deserializer);
        }
        /// <summary>
        /// 自定义反序列化委托
        /// </summary>
        internal override Delegate DeserializeDelegate { get { return (AutoCSer.XmlDeserializer.DeserializeDelegate<T>)Deserialize; } }
    }
}
