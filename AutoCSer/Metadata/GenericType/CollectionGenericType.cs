using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class CollectionGenericType : GenericTypeCache<CollectionGenericType>
    {
        /// <summary>
        /// 元素类型
        /// </summary>
        internal abstract Type ElementType { get; }
        /// <summary>
        /// 获取 JSON 集合序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal abstract void GetJsonSerializeCollectionDelegate(ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference);
        /// <summary>
        /// JSON 集合反序列化委托
        /// </summary>
        internal abstract Delegate JsonDeserializeCollectionDelegate { get; }

        /// <summary>
        /// 获取集合二进制序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal abstract void GetBinarySerializeCollectionDelegate(ref AutoCSer.BinarySerialize.SerializeDelegateReference serializeDelegateReference);
        /// <summary>
        /// 获取集合二进制反序列化委托
        /// </summary>
        internal abstract Delegate BinaryDeserializeCollectionDelegate { get; }

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
        /// 引用类型数组
        /// </summary>
        internal static readonly Type[] ReferenceTypes = new Type[] { typeof(VT) };
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }
        /// <summary>
        /// 元素类型
        /// </summary>
        internal override Type ElementType { get { return typeof(VT); } }

        /// <summary>
        /// 获取 JSON 集合序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal override void GetJsonSerializeCollectionDelegate(ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
        {
            serializeDelegateReference.SetMember((Action<JsonSerializer, T>)JsonSerializer.Collection<T, VT>, ReferenceTypes);
        }
        /// <summary>
        /// JSON 集合反序列化委托
        /// </summary>
#if NetStandard21
        internal override Delegate JsonDeserializeCollectionDelegate { get { return (JsonDeserializer.DeserializeDelegate<T?>)JsonDeserializer.Collection<T, VT>; } }
#else
        internal override Delegate JsonDeserializeCollectionDelegate { get { return (JsonDeserializer.DeserializeDelegate<T>)JsonDeserializer.Collection<T, VT>; } }
#endif

        /// <summary>
        /// 获取集合二进制序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal override void GetBinarySerializeCollectionDelegate(ref AutoCSer.BinarySerialize.SerializeDelegateReference serializeDelegateReference)
        {
            serializeDelegateReference.SetMember((Action<BinarySerializer, T>)BinarySerializer.Collection<T, VT>, ReferenceTypes, BinarySerialize.SerializePushTypeEnum.Primitive, true);
        }
        /// <summary>
        /// 获取集合二进制反序列化委托
        /// </summary>
#if NetStandard21
        internal override Delegate BinaryDeserializeCollectionDelegate { get { return (BinaryDeserializer.DeserializeDelegate<T?>)BinaryDeserializer.Collection<T, VT>; } }
#else
        internal override Delegate BinaryDeserializeCollectionDelegate { get { return (BinaryDeserializer.DeserializeDelegate<T>)BinaryDeserializer.Collection<T, VT>; } }
#endif
    }
}