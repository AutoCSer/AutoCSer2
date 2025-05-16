using AutoCSer.CodeGenerator;
using AutoCSer.Extensions;
using AutoCSer.Metadata;
using AutoCSer.Reflection;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace AutoCSer.Json
{
    /// <summary>
    /// 公共调用
    /// </summary>
    internal static class Common
    {
#if AOT
        /// <summary>
        /// 获取序列化委托循环引用信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializeDelegateReference"></param>
        /// <returns></returns>
        private static bool getSerializeDelegateReference(Type type, out AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
        {
            var serializeType = typeof(TypeSerializer<>).MakeGenericType(type);
            if (serializeType != null)
            {
                var field = serializeType.GetField("SerializeDelegateReference", BindingFlags.Static | BindingFlags.NonPublic);
                if (field != null)
                {
                    serializeDelegateReference = field.GetValue(null).castValue<AutoCSer.TextSerialize.DelegateReference>();
                    return true;
                }
                else AutoCSer.LogHelper.ExceptionIgnoreException(new MissingMemberException(serializeType.fullName(), "SerializeDelegateReference"));
            }
            else AutoCSer.LogHelper.ExceptionIgnoreException(new MissingMemberException(typeof(TypeSerializer<>).fullName(), type.fullName()));
            serializeDelegateReference = default(AutoCSer.TextSerialize.DelegateReference);
            return false;
        }
        /// <summary>
        /// 获取类型默认序列化委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializeDelegateReference"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        internal static bool GetTypeSerializeDelegate<T>(out AutoCSer.TextSerialize.DelegateReference serializeDelegateReference, out JsonSerializeAttribute attribute)
        {
            attribute = JsonSerializer.AllMemberAttribute;
            Type type = typeof(T);
            if (JsonSerializer.SerializeDelegates.TryGetValue(type, out serializeDelegateReference)) return true;
            AutoCSer.TextSerialize.SerializeDelegate serializeDelegate = JsonSerializer.CustomConfig.GetCustomSerializeDelegate<T>();
            if (serializeDelegate.Delegate != null && serializeDelegate.Check(typeof(JsonSerializer), type, ref serializeDelegateReference)) return true;
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    var elementType = type.GetElementType().notNull();
                    if (!elementType.isSerializeNotSupport())
                    {
                        if (elementType.isNullable())
                        {
                            Type[] referenceTypes = elementType.GetGenericArguments();
                            serializeDelegateReference.SetMember(JsonSerializer.NullableArrayMethod.MakeGenericMethod(referenceTypes[0]).CreateDelegate(typeof(Action<JsonSerializer, T>)), referenceTypes);
                        }
                        else serializeDelegateReference.SetMember(JsonSerializer.ArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<JsonSerializer, T>)), new Type[] { elementType });
                        return true;
                    }
                }
                serializeDelegateReference.SetUnknown(type, JsonSerializer.NotSupportMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<JsonSerializer, T>)));
                return true;
            }
            if (type.IsEnum)
            {
                Delegate enumSerializeDelegate = AutoCSer.Common.EmptyAction;
                switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)))
                {
                    case UnderlyingTypeEnum.Int: enumSerializeDelegate = JsonSerializer.EnumIntMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<JsonSerializer, T>)); break;
                    case UnderlyingTypeEnum.UInt: enumSerializeDelegate = JsonSerializer.EnumUIntMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<JsonSerializer, T>)); break;
                    case UnderlyingTypeEnum.Byte: enumSerializeDelegate = JsonSerializer.EnumByteMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<JsonSerializer, T>)); break;
                    case UnderlyingTypeEnum.ULong: enumSerializeDelegate = JsonSerializer.EnumULongMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<JsonSerializer, T>)); break;
                    case UnderlyingTypeEnum.UShort: enumSerializeDelegate = JsonSerializer.EnumUShortMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<JsonSerializer, T>)); break;
                    case UnderlyingTypeEnum.Long: enumSerializeDelegate = JsonSerializer.EnumLongMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<JsonSerializer, T>)); break;
                    case UnderlyingTypeEnum.Short: enumSerializeDelegate = JsonSerializer.EnumShortMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<JsonSerializer, T>)); break;
                    case UnderlyingTypeEnum.SByte: enumSerializeDelegate = JsonSerializer.EnumSByteMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<JsonSerializer, T>)); break;
                }
                serializeDelegateReference.SetNoLoop(enumSerializeDelegate);
                return true;
            }
            if (type.isSerializeNotSupport())
            {
                serializeDelegateReference.SetUnknown(type, JsonSerializer.NotSupportMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<JsonSerializer, T>)));
                return true;
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Dictionary<,>))
                {
                    Type[] referenceTypes = type.GetGenericArguments();
                    if (referenceTypes[0] == typeof(string)) serializeDelegateReference.SetMember(JsonSerializer.StringDictionaryMethod.MakeGenericMethod(referenceTypes[1]).CreateDelegate(typeof(Action<JsonSerializer, T>)), referenceTypes);
                    else serializeDelegateReference.SetMember(JsonSerializer.DictionaryMethod.MakeGenericMethod(referenceTypes).CreateDelegate(typeof(Action<JsonSerializer, T>)), referenceTypes);
                    return true;
                }
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    Type[] referenceTypes = type.GetGenericArguments();
                    serializeDelegateReference.SetMember(JsonSerializer.NullableMethod.MakeGenericMethod(referenceTypes[0]).CreateDelegate(typeof(Action<JsonSerializer, T>)), referenceTypes);
                    return true;
                }
                if (genericTypeDefinition == typeof(KeyValuePair<,>))
                {
                    Type[] referenceTypes = type.GetGenericArguments();
                    serializeDelegateReference.SetMember(JsonSerializer.KeyValuePairMethod.MakeGenericMethod(referenceTypes).CreateDelegate(typeof(Action<JsonSerializer, T>)), referenceTypes);
                    return true;
                }
            }
            if (DefaultConstructor<T>.Type != DefaultConstructorTypeEnum.None)
            {
                var collectionType = default(Type);
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IDictionary<,>))
                    {
                        Type[] referenceTypes = interfaceType.GetGenericArguments();
                        if (referenceTypes[0] == typeof(string))
                        {
                            serializeDelegateReference.SetMember(JsonSerializer.StringIDictionaryMethod.MakeGenericMethod(type, referenceTypes[1]).CreateDelegate(typeof(Action<JsonSerializer, T>)), new Type[] { referenceTypes[1] });
                        }
                        else
                        {
                            serializeDelegateReference.SetMember(JsonSerializer.IDictionaryMethod.MakeGenericMethod(type, referenceTypes[0], referenceTypes[1]).CreateDelegate(typeof(Action<JsonSerializer, T>)), referenceTypes[0] != referenceTypes[1] ? referenceTypes : new Type[] { referenceTypes[1] });
                        }
                        return true;
                    }
                    if (collectionType == null && genericTypeDefinition == typeof(ICollection<>)) collectionType = interfaceType;
                }
                if (collectionType != null)
                {
                    Type[] referenceTypes = collectionType.GetGenericArguments();
                    serializeDelegateReference.SetMember(JsonSerializer.CollectionMethod.MakeGenericMethod(type, referenceTypes[0]).CreateDelegate(typeof(Action<JsonSerializer, T>)), referenceTypes);
                    return true;
                }
            }
            var baseType = GetBaseAttribute(type, JsonSerializer.AllMemberAttribute, ref attribute);
            if (baseType != null)
            {
                serializeDelegateReference.SetMember(JsonSerializer.BaseMethod.MakeGenericMethod(type, baseType).CreateDelegate(typeof(Action<JsonSerializer, T>)), new Type[] { baseType });
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取类型默认反序列化委托
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        internal static Delegate? GetTypeDeserializeDelegate<T>(out JsonSerializeAttribute attribute)
        {
            Type type = typeof(T);
            attribute = JsonDeserializer.AllMemberAttribute;
            var deserializeDelegate = JsonDeserializer.GetDeserializeDelegate(type) ?? JsonSerializer.CustomConfig.GeteCustomDeserializeDelegate<T>();
            if (deserializeDelegate != null) return deserializeDelegate;
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    var elementType = type.GetElementType().notNull();
                    if (!elementType.isSerializeNotSupport()) return JsonDeserializer.ArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>)); 
                }
                return JsonDeserializer.NotSupportMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
            }
            if (type.IsEnum)
            {
                UnderlyingTypeEnum underlyingType = EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type));
                if (type.IsDefined(typeof(FlagsAttribute), false))
                {
                    switch (underlyingType)
                    {
                        case UnderlyingTypeEnum.Int: return JsonDeserializer.EnumFlagsIntMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                        case UnderlyingTypeEnum.UInt: return JsonDeserializer.EnumFlagsUIntMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                        case UnderlyingTypeEnum.Byte: return JsonDeserializer.EnumFlagsByteMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                        case UnderlyingTypeEnum.ULong: return JsonDeserializer.EnumFlagsULongMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                        case UnderlyingTypeEnum.UShort: return JsonDeserializer.EnumFlagsUShortMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                        case UnderlyingTypeEnum.Long: return JsonDeserializer.EnumFlagsLongMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                        case UnderlyingTypeEnum.Short: return JsonDeserializer.EnumFlagsShortMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                        case UnderlyingTypeEnum.SByte: return JsonDeserializer.EnumFlagsSByteMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                    }
                }
                switch (underlyingType)
                {
                    case UnderlyingTypeEnum.Int: return JsonDeserializer.EnumIntMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                    case UnderlyingTypeEnum.UInt: return JsonDeserializer.EnumUIntMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                    case UnderlyingTypeEnum.Byte: return JsonDeserializer.EnumByteMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                    case UnderlyingTypeEnum.ULong: return JsonDeserializer.EnumULongMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                    case UnderlyingTypeEnum.UShort: return JsonDeserializer.EnumUShortMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                    case UnderlyingTypeEnum.Long: return JsonDeserializer.EnumLongMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                    case UnderlyingTypeEnum.Short: return JsonDeserializer.EnumShortMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                    case UnderlyingTypeEnum.SByte: return JsonDeserializer.EnumSByteMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                }
            }
            if (type.isSerializeNotSupport()) return JsonDeserializer.NotSupportMethod.MakeGenericMethod(type).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Dictionary<,>))
                {
                    return JsonDeserializer.DictionaryMethod.MakeGenericMethod(type.GetGenericArguments()).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                }
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    return JsonDeserializer.NullableMethod.MakeGenericMethod(type.GetGenericArguments()[0]).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                }
                if (genericTypeDefinition == typeof(KeyValuePair<,>))
                {
                    return JsonDeserializer.KeyValuePairMethod.MakeGenericMethod(type.GetGenericArguments()).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                }
            }
            if (DefaultConstructor<T>.Type != DefaultConstructorTypeEnum.None)
            {
                var collectionType = default(Type);
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IDictionary<,>))
                    {
                        Type[] referenceTypes = interfaceType.GetGenericArguments();
                        return JsonDeserializer.IDictionaryMethod.MakeGenericMethod(type, referenceTypes[0], referenceTypes[1]).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
                    }
                    if (collectionType == null && genericTypeDefinition == typeof(ICollection<>)) collectionType = interfaceType;
                }
                if (collectionType != null) return JsonDeserializer.CollectionMethod.MakeGenericMethod(type, collectionType.GetGenericArguments()[0]).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>)); 
            }
            var baseType = GetBaseAttribute(type, JsonDeserializer.AllMemberAttribute, ref attribute);
            if (baseType != null) return JsonDeserializer.BaseMethod.MakeGenericMethod(type, baseType).CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T?>));
            return null;
        }
#else
        /// <summary>
        /// 获取类型默认序列化委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType"></param>
        /// <param name="serializeDelegateReference"></param>
        /// <returns></returns>
#if NetStandard21
        internal static bool GetTypeSerializeDelegate(Type type, ref GenericType? genericType, out AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
#else
        internal static bool GetTypeSerializeDelegate(Type type, ref GenericType genericType, out AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
#endif
        {
            if (JsonSerializer.SerializeDelegates.TryGetValue(type, out serializeDelegateReference)) return true;
            AutoCSer.TextSerialize.SerializeDelegate serializeDelegate = JsonSerializer.CustomConfig.GetCustomSerializeDelegate(type);
            if (serializeDelegate.Delegate != null && serializeDelegate.Check(typeof(JsonSerializer), type, ref serializeDelegateReference)) return true;
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    var elementType = type.GetElementType().notNull();
                    if (!elementType.isSerializeNotSupport())
                    {
                        if (elementType.isNullable())
                        {
                            Type[] referenceTypes = elementType.GetGenericArguments();
                            serializeDelegateReference.SetMember(StructGenericType.Get(referenceTypes[0]).JsonSerializeNullableArrayDelegate, referenceTypes);
                        }
                        else GenericType.Get(elementType).GetJsonSerializeArrayDelegate(ref serializeDelegateReference);
                        return true;
                    }
                }
                if (genericType == null) genericType = GenericType.Get(type);
                serializeDelegateReference.SetUnknown(type, genericType.JsonSerializeNotSupportDelegate);
                return true;
            }
            if (type.IsEnum)
            {
                serializeDelegateReference.SetNoLoop(EnumGenericType.Get(type).JsonSerializeEnumDelegate);
                return true;
            }
            if (type.isSerializeNotSupport())
            {
                if (genericType == null) genericType = GenericType.Get(type);
                serializeDelegateReference.SetUnknown(type, genericType.JsonSerializeNotSupportDelegate);
                return true;
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Dictionary<,>))
                {
                    Type[] referenceTypes = type.GetGenericArguments();
                    serializeDelegateReference.SetMember(DictionaryGenericType2.Get(referenceTypes).JsonSerializeDictionaryDelegate, referenceTypes);
                    return true;
                }
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    Type[] referenceTypes = type.GetGenericArguments();
                    serializeDelegateReference.SetMember(StructGenericType.Get(referenceTypes[0]).JsonSerializeNullableDelegate, referenceTypes);
                    return true;
                }
                if (genericTypeDefinition == typeof(KeyValuePair<,>))
                {
                    Type[] referenceTypes = type.GetGenericArguments();
                    serializeDelegateReference.SetMember(GenericType2.Get(referenceTypes).JsonSerializeKeyValuePairDelegate, referenceTypes);
                    return true;
                }
            }

            if (genericType == null && !type.IsGenericTypeDefinition) genericType = GenericType.Get(type);
            if (genericType == null || genericType.IsSerializeConstructor)
            {
                var collectionType = default(Type);
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IDictionary<,>))
                    {
                        DictionaryGenericType.Get(type, interfaceType).GetJsonSerializeDictionaryDelegate(ref serializeDelegateReference);
                        return true;
                    }
                    if (collectionType == null && genericTypeDefinition == typeof(ICollection<>)) collectionType = interfaceType;
                }
                if(collectionType != null)
                {
                    CollectionGenericType.Get(type, collectionType).GetJsonSerializeCollectionDelegate(ref serializeDelegateReference);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取类型默认序列化委托
        /// </summary>
        /// <param name="genericType"></param>
        /// <param name="serializeDelegateReference"></param>
        /// <param name="attribute"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
#if NetStandard21
        internal static bool GetTypeSerializeDelegate(GenericType genericType, out AutoCSer.TextSerialize.DelegateReference serializeDelegateReference, out JsonSerializeAttribute attribute, out Type? baseType)
#else
        internal static bool GetTypeSerializeDelegate(GenericType genericType, out AutoCSer.TextSerialize.DelegateReference serializeDelegateReference, out JsonSerializeAttribute attribute, out Type baseType)
#endif
        {
            attribute = JsonSerializer.AllMemberAttribute;
            Type type = genericType.CurrentType;
            var genericTypeParameter = genericType;
            if (GetTypeSerializeDelegate(type, ref genericTypeParameter, out serializeDelegateReference))
            {
                baseType = null;
                return true;
            }
            baseType = GetBaseAttribute(type, JsonSerializer.AllMemberAttribute, ref attribute);
            if (baseType != null)
            {
                BaseGenericType.Get(type, baseType).GetJsonSerializeBaseDelegate(ref serializeDelegateReference);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 成员序列化委托集合
        /// </summary>
        private static readonly Dictionary<HashObject<Type>, Delegate> memberSerializeDelegates = DictionaryCreator.CreateHashObject<Type, Delegate>();
        /// <summary>
        /// 成员序列化委托集合访问锁
        /// </summary>
        private static readonly object memberSerializeDelegateLock = new object();
        /// <summary>
        /// 获取成员序列化委托
        /// </summary>
        /// <param name="type">成员类型</param>
        internal static Delegate GetMemberSerializeDelegate(Type type)
        {
            var memberSerializeDelegate = default(Delegate);
            HashObject<Type> hashType = type;
            Monitor.Enter(memberSerializeDelegateLock);
            try
            {
                if (!memberSerializeDelegates.TryGetValue(hashType, out memberSerializeDelegate))
                {
                    memberSerializeDelegates.Add(hashType, memberSerializeDelegate = getMemberSerializeDelegate(type));
                }
            }
            finally { Monitor.Exit(memberSerializeDelegateLock); }
            return memberSerializeDelegate;
        }
        /// <summary>
        /// 获取成员序列化委托
        /// </summary>
        /// <param name="type">成员类型</param>
        private static Delegate getMemberSerializeDelegate(Type type)
        {
            var genericType = default(GenericType);
            AutoCSer.TextSerialize.DelegateReference serializeDelegateReference;
            if (GetTypeSerializeDelegate(type, ref genericType, out serializeDelegateReference)) return serializeDelegateReference.Delegate.Delegate.notNull();

            return (genericType ?? GenericType.Get(type)).JsonSerializeDelegate;
        }
        /// <summary>
        /// 成员反序列化委托集合
        /// </summary>
        private static readonly Dictionary<HashObject<Type>, Delegate> memberDeserializeDelegates = DictionaryCreator.CreateHashObject<Type, Delegate>();
        /// <summary>
        /// 成员反序列化委托集合访问锁
        /// </summary>
        private static readonly object memberDeserializeDelegateLock = new object();
        /// <summary>
        /// 获取成员反序列化委托
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <returns>成员反序列化委托</returns>
        internal static Delegate GetMemberDeserializeDelegate(Type type)
        {
            var memberDeserializeDelegate = default(Delegate);
            HashObject<Type> hashType = type;
            Monitor.Enter(memberDeserializeDelegateLock);
            try
            {
                if (!memberDeserializeDelegates.TryGetValue(hashType, out memberDeserializeDelegate))
                {
                    memberDeserializeDelegates.Add(hashType, memberDeserializeDelegate = getMemberDeserializeDelegate(type));
                }
            }
            finally { Monitor.Exit(memberDeserializeDelegateLock); }
            return memberDeserializeDelegate;
        }
        /// <summary>
        /// 获取成员反序列化委托
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <returns>成员反序列化委托</returns>
        private static Delegate getMemberDeserializeDelegate(Type type)
        {
            var genericType = default(GenericType);
            return getTypeDeserializeDelegate(type, ref genericType) ?? (genericType ?? GenericType.Get(type)).JsonDeserializeDelegate;
        }
        /// <summary>
        /// 获取类型默认反序列化委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
#if NetStandard21
        private static Delegate? getTypeDeserializeDelegate(Type type, ref GenericType? genericType)
#else
        private static Delegate getTypeDeserializeDelegate(Type type, ref GenericType genericType)
#endif
        {
            var deserializeDelegate = JsonDeserializer.GetDeserializeDelegate(type);
            if (deserializeDelegate != null) return deserializeDelegate;
            deserializeDelegate = JsonSerializer.CustomConfig.GeteCustomDeserializeDelegate(type);
            if (deserializeDelegate != null)
            {
                var checkType = AutoCSer.Common.CheckDeserializeType(typeof(JsonDeserializer), deserializeDelegate);
                if (type == checkType) return deserializeDelegate;
                if (checkType != null) AutoCSer.LogHelper.ErrorIgnoreException($"自定义类型反序列化函数数据类型不匹配 {type.fullName()} <> {checkType.fullName()}", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    var elementType = type.GetElementType().notNull();
                    if (!elementType.isSerializeNotSupport()) return GenericType.Get(elementType).JsonDeserializeArrayDelegate;
                }
                return GenericType.Get(type).JsonDeserializeNotSupportDelegate;
            }
            if (type.IsEnum)
            {
                if (type.IsDefined(typeof(FlagsAttribute), false)) return EnumGenericType.Get(type).JsonDeserializeEnumFlagsDelegate;
                return EnumGenericType.Get(type).JsonDeserializeEnumDelegate;
            }
            if (type.isSerializeNotSupport()) return GenericType.Get(type).JsonDeserializeNotSupportDelegate;
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Dictionary<,>))
                {
                    return DictionaryGenericType2.Get(type.GetGenericArguments()).JsonDeserializeDictionaryDelegate;
                }
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    return StructGenericType.Get(type.GetGenericArguments()[0]).JsonDeserializeNullableDelegate;
                }
                if (genericTypeDefinition == typeof(KeyValuePair<,>))
                {
                    return GenericType2.Get(type.GetGenericArguments()).JsonDeserializeKeyValuePairDelegate;
                }
            }
            if (genericType == null) genericType = GenericType.Get(type);
            if (genericType.IsSerializeConstructor)
            {
                var collectionType = default(Type);
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IDictionary<,>)) return DictionaryGenericType.Get(type, interfaceType).JsonDeserializeDictionaryDelegate;
                    if (collectionType == null && genericTypeDefinition == typeof(ICollection<>)) collectionType = interfaceType;
                }
                if (collectionType != null) return CollectionGenericType.Get(type, collectionType).JsonDeserializeCollectionDelegate;
            }
            return null;
        }
        /// <summary>
        /// 获取类型默认反序列化委托
        /// </summary>
        /// <param name="genericType"></param>
        /// <param name="attribute"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
#if NetStandard21
        internal static Delegate? GetTypeDeserializeDelegate(GenericType genericType, out JsonSerializeAttribute attribute, out Type? baseType)
#else
        internal static Delegate GetTypeDeserializeDelegate(GenericType genericType, out JsonSerializeAttribute attribute, out Type baseType)
#endif
        {
            attribute = JsonDeserializer.AllMemberAttribute;
            var genericTypeParameter = genericType;
            var deserializeDelegate = getTypeDeserializeDelegate(genericType.CurrentType, ref genericTypeParameter);
            if (deserializeDelegate != null)
            {
                baseType = null;
                return deserializeDelegate;
            }
            baseType = GetBaseAttribute(genericType.CurrentType, JsonDeserializer.AllMemberAttribute, ref attribute);
            if (baseType != null) return BaseGenericType.Get(genericType.CurrentType, baseType).JsonDeserializeBaseDelegate;
            return null;
        }
#endif
        /// <summary>
        /// 获取 JSON 序列化类型配置
        /// </summary>
        /// <param name="type"></param>
        /// <param name="defaultAttribute"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
#if NetStandard21
        internal static Type? GetBaseAttribute(Type type, JsonSerializeAttribute defaultAttribute, ref JsonSerializeAttribute attribute)
#else
        internal static Type GetBaseAttribute(Type type, JsonSerializeAttribute defaultAttribute, ref JsonSerializeAttribute attribute)
#endif
        {
            for (var baseType = type; baseType != typeof(object);)
            {
                var baseAttribute = baseType.GetCustomAttribute(typeof(JsonSerializeAttribute), false);
                if (baseAttribute != null)
                {
                    attribute = (JsonSerializeAttribute)baseAttribute;
                    if (type != baseType && attribute.IsBaseType) return baseType;
                }
                if (!object.ReferenceEquals(attribute, defaultAttribute)) break;
                if ((baseType = baseType.BaseType) == null) break;
            }
            return null;
        }

        /// <summary>
        /// 计算状态完成检查
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reference"></param>
        internal static void CheckCompleted(Type type, ref AutoCSer.TextSerialize.DelegateReference reference)
        {
            if (!reference.IsCompleted)
            {
                int memberIndex = 0;
                var referenceTypes = reference.Delegate.ReferenceTypes.notNull();
#if AOT
                reference.ReferenceTypes = new Type[referenceTypes.Length];
#else
                reference.ReferenceTypes = new GenericType[referenceTypes.Length];
#endif
                foreach (var memberType in referenceTypes)
                {
#if AOT
                    Type genericType = memberType.notNull();
                    AutoCSer.TextSerialize.DelegateReference serializeDelegateReference;
                    if (getSerializeDelegateReference(genericType, out serializeDelegateReference))
                    {
                        if (!serializeDelegateReference.IsCompleted || serializeDelegateReference.IsCheckMember)
                        {
                            reference.ReferenceTypes[memberIndex++] = genericType;
                        }
                    }
                    else
                    {
                        reference.IsUnknownMember = true;
                        reference.ReferenceTypes[memberIndex++] = genericType;
                    }
#else
                    GenericType genericType = GenericType.Get(memberType.notNull());
                    AutoCSer.TextSerialize.DelegateReference serializeDelegateReference = genericType.JsonSerializeDelegateReference;
                    reference.IsUnknownMember |= serializeDelegateReference.IsUnknownMember;
                    if (!serializeDelegateReference.IsCompleted || serializeDelegateReference.IsCheckMember)
                    {
                        reference.ReferenceTypes[memberIndex++] = genericType;
                    }
#endif
                }
                if (memberIndex == 0)
                {
                    reference.ReferenceTypes = null;
                    reference.PushType = AutoCSer.TextSerialize.PushTypeEnum.DepthCount;
                    reference.IsCheckMember = false;
                }
                else
                {
                    if (memberIndex != reference.ReferenceTypes.Length) System.Array.Resize(ref reference.ReferenceTypes, memberIndex);
                    if (type.IsValueType) reference.PushType = AutoCSer.TextSerialize.PushTypeEnum.DepthCount;
                    else
                    {
                        HashSet<HashObject<System.Type>> types = HashSetCreator.CreateHashObject<System.Type>();
                        LeftArray<AutoCSer.TextSerialize.LoopTypeArray> typeArray = new LeftArray<AutoCSer.TextSerialize.LoopTypeArray>(sizeof(int));
                        typeArray.Array[0].Set(ref reference);
                        typeArray.Length = 1;
                        do
                        {
                            var genericType = Check(ref typeArray.Array[typeArray.Length - 1], type, types);
                            if (genericType == null)
                            {
                                if (--typeArray.Length == 0)
                                {
                                    reference.PushType = reference.IsUnknownMember ? AutoCSer.TextSerialize.PushTypeEnum.UnknownNode : AutoCSer.TextSerialize.PushTypeEnum.DepthCount;
                                    break;
                                }
                            }
#if AOT
                            else if (genericType == type)
#else
                            else if (genericType.CurrentType == type)
#endif
                            {
                                reference.PushType = AutoCSer.TextSerialize.PushTypeEnum.Push;
                                break;
                            }
                            else
                            {
#if AOT
                                AutoCSer.TextSerialize.DelegateReference serializeDelegateReference;
                                if (getSerializeDelegateReference(genericType, out serializeDelegateReference))
                                {
                                    typeArray.PrepLength(1);
                                    typeArray.Array[typeArray.Length++].Set(serializeDelegateReference);
                                }
                                else
                                {
                                    reference.PushType = AutoCSer.TextSerialize.PushTypeEnum.Push;
                                    break;
                                }
#else
                                typeArray.PrepLength(1);
                                typeArray.Array[typeArray.Length++].Set(genericType.JsonSerializeDelegateReference);
#endif
                            }
                        }
                        while (true);
                    }
                }
                reference.IsCompleted = true;
                reference.Delegate.ReferenceTypes = null;
            }
        }
        /// <summary>
        /// 循环引用检查
        /// </summary>
        /// <param name="array"></param>
        /// <param name="type"></param>
        /// <param name="types"></param>
        /// <returns></returns>
#if AOT
        internal static Type? Check(ref AutoCSer.TextSerialize.LoopTypeArray array, Type type, HashSet<HashObject<System.Type>> types)
#else
#if NetStandard21
        internal static GenericType? Check(ref AutoCSer.TextSerialize.LoopTypeArray array, Type type, HashSet<HashObject<System.Type>> types)
#else
        internal static GenericType Check(ref AutoCSer.TextSerialize.LoopTypeArray array, Type type, HashSet<HashObject<System.Type>> types)
#endif
#endif
        {
            do
            {
            START:
                if (array.ReferenceGenericTypes != null)
                {
                    var genericType = array.ReferenceGenericTypes[array.Index++];
#if AOT
                    Type currentType = genericType;
#else
                    Type currentType = genericType.CurrentType;
#endif
                    bool isType;
                    if (currentType.IsValueType) isType = true;
                    else
                    {
                        if (type == currentType) return genericType;
                        isType = types.Add(currentType);
                    }
                    if (isType)
                    {
#if AOT
                        AutoCSer.TextSerialize.DelegateReference serializeDelegateReference;
                        if (getSerializeDelegateReference(genericType, out serializeDelegateReference))
                        {
                            if (!serializeDelegateReference.IsCompleted || serializeDelegateReference.IsCheckMember)
                            {
                                if (array.Index != array.ReferenceGenericTypes.Length) return genericType;
                                array.Set(ref serializeDelegateReference);
                                goto START;
                            }
                        }
                        else return type;
#else
                        AutoCSer.TextSerialize.DelegateReference serializeDelegateReference = genericType.JsonSerializeDelegateReference;
                        if (!serializeDelegateReference.IsCompleted || serializeDelegateReference.IsCheckMember)
                        {
                            if (array.Index != array.ReferenceGenericTypes.Length) return genericType;
                            array.Set(ref serializeDelegateReference);
                            goto START;
                        }
#endif

                    }
                    if (array.Index == array.ReferenceGenericTypes.Length) return null;
                }
                else
                {
                    var referenceTypes = array.ReferenceTypes.notNull();
                    var currentType = referenceTypes[array.Index++].notNull();
                    bool isType;
                    if (currentType.IsValueType) isType = true;
                    else
                    {
#if AOT
                        if (type == currentType) return type;
#else
                        if (type == currentType) return GenericType.Get(type);
#endif
                        isType = types.Add(currentType);
                    }
                    if (isType)
                    {
#if AOT
                        AutoCSer.TextSerialize.DelegateReference serializeDelegateReference;
                        if (getSerializeDelegateReference(currentType, out serializeDelegateReference))
                        {
                            if (!serializeDelegateReference.IsCompleted || serializeDelegateReference.IsCheckMember)
                            {
                                if (array.Index != referenceTypes.Length) return currentType;
                                array.Set(ref serializeDelegateReference);
                                goto START;
                            }
                        }
                        else return type;
#else
                        GenericType genericType = GenericType.Get(currentType);
                        AutoCSer.TextSerialize.DelegateReference serializeDelegateReference = genericType.JsonSerializeDelegateReference;
                        if (!serializeDelegateReference.IsCompleted || serializeDelegateReference.IsCheckMember)
                        {
                            if (array.Index != referenceTypes.Length) return genericType;
                            array.Set(ref serializeDelegateReference);
                            goto START;
                        }
#endif
                    }
                    if (array.Index == referenceTypes.Length) return null;
                }
            }
            while (true);
        }
    }
}
