﻿using AutoCSer.Memory;
using System;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class GenericType : AutoCSer.Metadata.GenericTypeCache<GenericType>
    {
        /// <summary>
        /// 判断构造函数是否支持数据反序列化
        /// </summary>
        internal abstract bool IsSerializeConstructor { get; }
        /// <summary>
        /// 获取 XML 序列化数组委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal abstract void GetXmlSerializeArrayDelegate(ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference);
        /// <summary>
        /// XML 序列化
        /// </summary>
        internal abstract Delegate XmlSerializeDelegate { get; }
        /// <summary>
        /// XML 自定义序列化不支持类型
        /// </summary>
        internal abstract Delegate XmlSerializeNotSupportDelegate { get; }
        /// <summary>
        /// XML 序列化委托循环引用信息
        /// </summary>
        internal abstract AutoCSer.TextSerialize.DelegateReference XmlSerializeDelegateReference { get; }
        /// <summary>
        /// XML 序列化
        /// </summary>
        internal abstract Func<object, XmlSerializeConfig, KeyValue<string, AutoCSer.TextSerialize.WarningEnum>> XmlSerializeObjectGenericDelegate { get; }
        /// <summary>
        /// XML 序列化
        /// </summary>
        internal abstract Func<object, CharStream, XmlSerializeConfig, AutoCSer.TextSerialize.WarningEnum> XmlSerializeStreamObjectDelegate { get; }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal abstract Action<AutoCSer.XmlSerializer, object> XmlSerializeObjectDelegate { get; }
        /// <summary>
        /// XML 反序列化数组
        /// </summary>
        internal abstract Delegate XmlDeserializeArrayDelegate { get; }
        /// <summary>
        /// XML 反序列化数组
        /// </summary>
        internal abstract Delegate XmlDeserializeLeftArrayDelegate { get; }
        /// <summary>
        /// XML 反序列化数组
        /// </summary>
        internal abstract Delegate XmlDeserializeListArrayDelegate { get; }
        /// <summary>
        /// XML 自定义反序列化不支持类型
        /// </summary>
        internal abstract Delegate XmlDeserializeNotSupportDelegate { get; }
        /// <summary>
        /// XML 反序列化类型
        /// </summary>
        internal abstract Delegate XmlDeserializeDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [AutoCSer.AOT.Preserve(Conditional = true)]
        private static GenericType create<T>()
        {
            return new GenericType<T>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
        protected static GenericType lastGenericType;
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static GenericType Get(Type type)
        {
            GenericType value = lastGenericType;
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
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }

        /// <summary>
        /// 是否存在默认构造函数
        /// </summary>
        internal override bool IsSerializeConstructor { get { return AutoCSer.Metadata.GenericType<T>.GetIsSerializeConstructor(); } }
        /// <summary>
        /// XML 序列化数组
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal override void GetXmlSerializeArrayDelegate(ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
        {
            serializeDelegateReference.SetMember((Action<AutoCSer.XmlSerializer, T[]>)AutoCSer.XmlSerializer.Array<T>, AutoCSer.Metadata.GenericType<T>.ReferenceTypes);
        }
        /// <summary>
        /// XML 自定义序列化引用类型
        /// </summary>
        internal override Delegate XmlSerializeDelegate { get { return (Action<AutoCSer.XmlSerializer, T>)AutoCSer.XmlSerializer.Serialize<T>; } }
        /// <summary>
        /// XML 自定义序列化不支持类型
        /// </summary>
        internal override Delegate XmlSerializeNotSupportDelegate { get { return (Action<AutoCSer.XmlSerializer, T>)AutoCSer.XmlSerializer.NotSupport<T>; } }

        /// <summary>
        /// XML 序列化委托循环引用信息
        /// </summary>
        internal override AutoCSer.TextSerialize.DelegateReference XmlSerializeDelegateReference { get { return AutoCSer.Xml.TypeSerializer<T>.SerializeDelegateReference; } }
        /// <summary>
        /// XML 序列化
        /// </summary>
        internal override Func<object, XmlSerializeConfig, KeyValue<string, AutoCSer.TextSerialize.WarningEnum>> XmlSerializeObjectGenericDelegate { get { return XmlSerializer.Serialize<T>; } }
        /// <summary>
        /// XML 序列化
        /// </summary>
        internal override Func<object, CharStream, XmlSerializeConfig, AutoCSer.TextSerialize.WarningEnum> XmlSerializeStreamObjectDelegate { get { return XmlSerializer.Serialize<T>; } }
        /// <summary>
        /// 获取 XML 序列化函数信息
        /// </summary>
        internal override Action<AutoCSer.XmlSerializer, object> XmlSerializeObjectDelegate { get { return (Action<AutoCSer.XmlSerializer, object>)AutoCSer.XmlSerializer.Object<T>; } }
        /// <summary>
        /// XML 反序列化数组
        /// </summary>
        internal override Delegate XmlDeserializeArrayDelegate { get { return (AutoCSer.XmlDeserializer.DeserializeDelegate<T[]>)AutoCSer.XmlDeserializer.Array<T>; } }
        /// <summary>
        /// XML 反序列化数组
        /// </summary>
        internal override Delegate XmlDeserializeLeftArrayDelegate { get { return (AutoCSer.XmlDeserializer.DeserializeDelegate<LeftArray<T>>)AutoCSer.XmlDeserializer.LeftArray<T>; } }
        /// <summary>
        /// XML 反序列化数组
        /// </summary>
        internal override Delegate XmlDeserializeListArrayDelegate { get { return (AutoCSer.XmlDeserializer.DeserializeDelegate<ListArray<T>>)AutoCSer.XmlDeserializer.ListArray<T>; } }
        /// <summary>
        /// XML 自定义反序列化不支持类型
        /// </summary>
        internal override Delegate XmlDeserializeNotSupportDelegate { get { return (AutoCSer.XmlDeserializer.DeserializeDelegate<T>)AutoCSer.XmlDeserializer.NotSupport<T>; } }
        /// <summary>
        /// XML 反序列化类型
        /// </summary>
        internal override Delegate XmlDeserializeDelegate { get { return (XmlDeserializer.DeserializeDelegate<T>)AutoCSer.XmlDeserializer.Deserialize<T>; } }
    }
}