using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AutoCSer.Metadata
{
    /// <summary>
    /// 泛型类型元数据
    /// </summary>
    internal abstract class DictionaryGenericType : GenericTypeCache<DictionaryGenericType>
    {
        /// <summary>
        /// 关键字类型
        /// </summary>
        internal abstract Type KeyType { get; }
        /// <summary>
        /// 数据类型
        /// </summary>
        internal abstract Type ValueType { get; }
        /// <summary>
        /// 获取 JSON 字典序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal abstract void GetJsonSerializeDictionaryDelegate(ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference);
        /// <summary>
        /// JSON 字典反序列化委托
        /// </summary>
        internal abstract Delegate JsonDeserializeDictionaryDelegate { get; }
        /// <summary>
        /// 获取字典二进制序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal abstract void GetBinarySerializeDictionaryDelegate(ref AutoCSer.BinarySerialize.SerializeDelegateReference serializeDelegateReference);
        /// <summary>
        /// 获取字典二进制反序列化委托
        /// </summary>
        internal abstract Delegate BinaryDeserializeDictionaryDelegate { get; }

        /// <summary>
        /// 创建泛型类型元数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="KT"></typeparam>
        /// <typeparam name="VT"></typeparam>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static DictionaryGenericType create<T, KT, VT>()
            where T : IDictionary<KT, VT>
        {
            return new DictionaryGenericType<T, KT, VT>();
        }
        /// <summary>
        /// 最后一次访问的泛型类型元数据
        /// </summary>
#if NetStandard21
        protected static DictionaryGenericType? lastGenericType;
#else
        protected static DictionaryGenericType lastGenericType;
#endif
        /// <summary>
        /// 获取泛型类型元数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType">IDictionary 类型</param>
        /// <returns></returns>
        public static DictionaryGenericType Get(Type type, Type interfaceType)
        {
            var value = lastGenericType;
            if (value?.CurrentType == type) return value;
            value = getDictionary(type, interfaceType);
            lastGenericType = value;
            return value;
        }
    }
    /// <summary>
    /// 泛型代理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="KT"></typeparam>
    /// <typeparam name="VT"></typeparam>
    internal sealed class DictionaryGenericType<T, KT, VT> : DictionaryGenericType
        where T : IDictionary<KT, VT>
    {
        /// <summary>
        /// 引用类型数组
        /// </summary>
        private static readonly Type[] valueReferenceTypes = new Type[] { typeof(VT) };
        /// <summary>
        /// 引用类型数组
        /// </summary>
        private static readonly Type[] referenceTypes = new Type[] { typeof(KT), typeof(VT) };
        /// <summary>
        /// 获取当前泛型类型
        /// </summary>
        internal override Type CurrentType { get { return typeof(T); } }
        /// <summary>
        /// 关键字类型
        /// </summary>
        internal override Type KeyType { get { return typeof(KT); } }
        /// <summary>
        /// 数据类型
        /// </summary>
        internal override Type ValueType { get { return typeof(VT); } }

        /// <summary>
        /// 获取 JSON 字典序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal override void GetJsonSerializeDictionaryDelegate(ref AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
        {
            if (typeof(KT) == typeof(string))
            {
                serializeDelegateReference.SetMember((Action<JsonSerializer, T>)JsonSerializer.StringIDictionary<T, VT>, valueReferenceTypes);
            }
            else
            {
                serializeDelegateReference.SetMember((Action<JsonSerializer, T>)JsonSerializer.IDictionary<T, KT, VT>, typeof(KT) != typeof(VT) ? referenceTypes : valueReferenceTypes);
            }
        }
        /// <summary>
        /// JSON 字典反序列化委托
        /// </summary>
#if NetStandard21
        internal override Delegate JsonDeserializeDictionaryDelegate { get { return (JsonDeserializer.DeserializeDelegate<T?>)JsonDeserializer.IDictionary<T, KT, VT>; } }
#else
        internal override Delegate JsonDeserializeDictionaryDelegate { get { return (JsonDeserializer.DeserializeDelegate<T>)JsonDeserializer.IDictionary<T, KT, VT>; } }
#endif
        /// <summary>
        /// 获取字典二进制序列化委托
        /// </summary>
        /// <param name="serializeDelegateReference"></param>
        internal override void GetBinarySerializeDictionaryDelegate(ref AutoCSer.BinarySerialize.SerializeDelegateReference serializeDelegateReference)
        {
            serializeDelegateReference.SetMember((Action<BinarySerializer, T>)BinarySerializer.Dictionary<T, KT, VT>, referenceTypes, BinarySerialize.SerializePushTypeEnum.Primitive, true);
        }
        /// <summary>
        /// 获取字典二进制反序列化委托
        /// </summary>
#if NetStandard21
        internal override Delegate BinaryDeserializeDictionaryDelegate { get { return (BinaryDeserializer.DeserializeDelegate<T?>)BinaryDeserializer.Dictionary<T, KT, VT>; } }
#else
        internal override Delegate BinaryDeserializeDictionaryDelegate { get { return (BinaryDeserializer.DeserializeDelegate<T>)BinaryDeserializer.Dictionary<T, KT, VT>; } }
#endif
    }
}