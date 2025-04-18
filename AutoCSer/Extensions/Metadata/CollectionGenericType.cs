using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Extensions.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class CollectionGenericType : AutoCSer.Metadata.GenericTypeCache<CollectionGenericType>
    {
        /// <summary>
        /// 获取 XML 集合序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal abstract void GetXmlSerializeCollectionDelegate(ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference);
        /// <summary>
        /// XML 集合反序列化委托
        /// </summary>
        internal abstract Delegate XmlDeserializeCollectionDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static CollectionGenericType create<T, VT>()
#if NetStandard21
            where T : ICollection<VT?>
#else
            where T : ICollection<VT>
#endif
        {
            return new CollectionGenericType<T, VT>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static CollectionGenericType? lastGenericType;
#else
        protected static CollectionGenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType">ICollection 类型</param>
        /// <returns></returns>
        public static CollectionGenericType Get(Type type, Type interfaceType)
        {
            var value = lastGenericType;
            if (value?.CurrentType == type) return value;
            value = getCollection(type, interfaceType);
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="VT"></typeparam>
    internal sealed class CollectionGenericType<T, VT> : CollectionGenericType
#if NetStandard21
        where T : ICollection<VT?>
#else
        where T : ICollection<VT>
#endif
    {
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }
        /// <summary>
        /// 获取 XML 集合序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal override void GetXmlSerializeCollectionDelegate(ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
        {
#if NetStandard21
            serializeDelegateReference.SetMember((Action<XmlSerializer, T>)XmlSerializer.Collection<T, VT>, AutoCSer.Metadata.CollectionGenericType<T, VT>.ReferenceTypes);
#else
            serializeDelegateReference.SetMember((Action<XmlSerializer, T>)XmlSerializer.Collection<T, VT>, AutoCSer.Metadata.CollectionGenericType<T, VT>.ReferenceTypes);
#endif
        }
        /// <summary>
        /// XML 集合反序列化委托
        /// </summary>
#if NetStandard21
        internal override Delegate XmlDeserializeCollectionDelegate { get { return (XmlDeserializer.DeserializeDelegate<T?>)XmlDeserializer.Collection<T, VT>; } }
#else
        internal override Delegate XmlDeserializeCollectionDelegate { get { return (XmlDeserializer.DeserializeDelegate<T>)XmlDeserializer.Collection<T, VT>; } }
#endif
    }
}