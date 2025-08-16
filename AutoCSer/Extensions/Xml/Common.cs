using AutoCSer.CodeGenerator;
using AutoCSer.Extensions;
using AutoCSer.Metadata;
using AutoCSer.Threading;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AutoCSer.Xml
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
        /// <param name="serializeMethodInfo"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        internal static bool GetTypeSerializeDelegate<T>(out AutoCSer.TextSerialize.DelegateReference serializeDelegateReference, ref TextSerializeMethodInfo serializeMethodInfo, out XmlSerializeAttribute attribute)
        {
            attribute = XmlSerializer.AllMemberAttribute;
            Type type = typeof(T);
            if (XmlSerializer.SerializeDelegates.TryGetValue(type, out serializeDelegateReference)) return true;
            AutoCSer.TextSerialize.SerializeDelegate serializeDelegate = XmlSerializer.CustomConfig.GetCustomSerializeDelegate<T>();
            if (serializeDelegate.Delegate != null && serializeDelegate.Check(typeof(XmlSerializer), type, ref serializeDelegateReference)) return true;
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    var elementType = type.GetElementType().notNull();
                    if (!elementType.isSerializeNotSupport())
                    {
                        serializeDelegateReference.SetMember(XmlSerializer.ArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<XmlSerializer, T>)), new Type[] { elementType });
                        return true;
                    }
                }
                serializeDelegateReference.SetUnknown(type, XmlSerializer.NotSupportMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<XmlSerializer, T>)));
                return true;
            }
            if (type.IsEnum)
            {
                Delegate enumSerializeDelegate = AutoCSer.Common.EmptyAction;
                switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)))
                {
                    case UnderlyingTypeEnum.Int: enumSerializeDelegate = XmlSerializer.EnumIntMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<XmlSerializer, T>)); break;
                    case UnderlyingTypeEnum.UInt: enumSerializeDelegate = XmlSerializer.EnumUIntMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<XmlSerializer, T>)); break;
                    case UnderlyingTypeEnum.Byte: enumSerializeDelegate = XmlSerializer.EnumByteMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<XmlSerializer, T>)); break;
                    case UnderlyingTypeEnum.ULong: enumSerializeDelegate = XmlSerializer.EnumULongMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<XmlSerializer, T>)); break;
                    case UnderlyingTypeEnum.UShort: enumSerializeDelegate = XmlSerializer.EnumUShortMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<XmlSerializer, T>)); break;
                    case UnderlyingTypeEnum.Long: enumSerializeDelegate = XmlSerializer.EnumLongMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<XmlSerializer, T>)); break;
                    case UnderlyingTypeEnum.Short: enumSerializeDelegate = XmlSerializer.EnumShortMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<XmlSerializer, T>)); break;
                    case UnderlyingTypeEnum.SByte: enumSerializeDelegate = XmlSerializer.EnumSByteMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<XmlSerializer, T>)); break;
                }
                serializeDelegateReference.SetNoLoop(enumSerializeDelegate);
                return true;
            }
            if (type.isSerializeNotSupport())
            {
                serializeDelegateReference.SetUnknown(type, XmlSerializer.NotSupportMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<XmlSerializer, T>)));
                return true;
            }
            if (type.IsGenericType && type.IsValueType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    Type[] referenceTypes = type.GetGenericArguments();
                    serializeDelegateReference.SetMember(XmlSerializer.NullableMethod.MakeGenericMethod(referenceTypes[0]).CreateDelegate(typeof(Action<XmlSerializer, T>)), referenceTypes);
                    return true;
                }
                if (genericTypeDefinition == typeof(KeyValue<,>))
                {
                    Type[] referenceTypes = type.GetGenericArguments();
                    serializeDelegateReference.SetMember(XmlSerializer.KeyValueMethod.MakeGenericMethod(referenceTypes).CreateDelegate(typeof(Action<XmlSerializer, T>)), referenceTypes);
                    return serializeMethodInfo.IsEmptyString = true;
                }
                if (genericTypeDefinition == typeof(KeyValuePair<,>))
                {
                    Type[] referenceTypes = type.GetGenericArguments();
                    serializeDelegateReference.SetMember(XmlSerializer.KeyValuePairMethod.MakeGenericMethod(referenceTypes).CreateDelegate(typeof(Action<XmlSerializer, T>)), referenceTypes);
                    return serializeMethodInfo.IsEmptyString = true;
                }
            }
            if (DefaultConstructor<T>.Type != DefaultConstructorTypeEnum.None)
            {
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(ICollection<>))
                    {
                        Type[] referenceTypes = interfaceType.GetGenericArguments();
                        serializeDelegateReference.SetMember(XmlSerializer.CollectionMethod.MakeGenericMethod(type, referenceTypes[0]).CreateDelegate(typeof(Action<XmlSerializer, T>)), referenceTypes);
                        return true;
                    }
                }
            }
            var baseType = GetBaseAttribute(type, XmlSerializer.AllMemberAttribute, ref attribute);
            if (baseType != null)
            {
                serializeDelegateReference.SetMember(XmlSerializer.BaseMethod.MakeGenericMethod(type, baseType).CreateDelegate(typeof(Action<XmlSerializer, T>)), new Type[] { baseType });
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取类型默认反序列化委托
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        internal static Delegate? GetTypeDeserializeDelegate<T>(out XmlSerializeAttribute attribute)
        {
            Type type = typeof(T);
            attribute = XmlDeserializer.AllMemberAttribute;
            var deserializeDelegate = XmlDeserializer.GetDeserializeDelegate(type);
            if (deserializeDelegate != null) return deserializeDelegate;
            if (typeof(ICustomSerialize<T>).IsAssignableFrom(type)) return XmlDeserializer.ICustomMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    var elementType = type.GetElementType().notNull();
                    if (!elementType.isSerializeNotSupport()) return XmlDeserializer.ArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                }
                return XmlDeserializer.NotSupportMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
            }
            if (type.IsEnum)
            {
                UnderlyingTypeEnum underlyingType = EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type));
                if (type.IsDefined(typeof(FlagsAttribute), false))
                {
                    switch (underlyingType)
                    {
                        case UnderlyingTypeEnum.Int: return XmlDeserializer.EnumFlagsIntMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                        case UnderlyingTypeEnum.UInt: return XmlDeserializer.EnumFlagsUIntMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                        case UnderlyingTypeEnum.Byte: return XmlDeserializer.EnumFlagsByteMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                        case UnderlyingTypeEnum.ULong: return XmlDeserializer.EnumFlagsULongMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                        case UnderlyingTypeEnum.UShort: return XmlDeserializer.EnumFlagsUShortMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                        case UnderlyingTypeEnum.Long: return XmlDeserializer.EnumFlagsLongMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                        case UnderlyingTypeEnum.Short: return XmlDeserializer.EnumFlagsShortMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                        case UnderlyingTypeEnum.SByte: return XmlDeserializer.EnumFlagsSByteMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                    }
                }
                switch (underlyingType)
                {
                    case UnderlyingTypeEnum.Int: return XmlDeserializer.EnumIntMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                    case UnderlyingTypeEnum.UInt: return XmlDeserializer.EnumUIntMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                    case UnderlyingTypeEnum.Byte: return XmlDeserializer.EnumByteMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                    case UnderlyingTypeEnum.ULong: return XmlDeserializer.EnumULongMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                    case UnderlyingTypeEnum.UShort: return XmlDeserializer.EnumUShortMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                    case UnderlyingTypeEnum.Long: return XmlDeserializer.EnumLongMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                    case UnderlyingTypeEnum.Short: return XmlDeserializer.EnumShortMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                    case UnderlyingTypeEnum.SByte: return XmlDeserializer.EnumSByteMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                }
            }
            if (type.isSerializeNotSupport()) return XmlDeserializer.NotSupportMethod.MakeGenericMethod(type).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (type.IsValueType)
                {
                    if (genericTypeDefinition == typeof(Nullable<>))
                    {
                        return XmlDeserializer.NullableMethod.MakeGenericMethod(type.GetGenericArguments()[0]).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                    }
                    if (genericTypeDefinition == typeof(KeyValue<,>))
                    {
                        return XmlDeserializer.KeyValueMethod.MakeGenericMethod(type.GetGenericArguments()).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                    }
                    if (genericTypeDefinition == typeof(KeyValuePair<,>))
                    {
                        return XmlDeserializer.KeyValuePairMethod.MakeGenericMethod(type.GetGenericArguments()).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                    }
                    if (genericTypeDefinition == typeof(LeftArray<>))
                    {
                        return XmlDeserializer.LeftArrayMethod.MakeGenericMethod(type.GetGenericArguments()[0]).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                    }
                }
                else
                {
                    if (genericTypeDefinition == typeof(ListArray<>))
                    {
                        return XmlDeserializer.ListArrayMethod.MakeGenericMethod(type.GetGenericArguments()[0]).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                    }
                }
            }
            if (DefaultConstructor<T>.Type != DefaultConstructorTypeEnum.None)
            {
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(ICollection<>))
                    {
                        return XmlDeserializer.CollectionMethod.MakeGenericMethod(type, interfaceType.GetGenericArguments()[0]).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
                    }
                }
            }
            var baseType = GetBaseAttribute(type, XmlDeserializer.AllMemberAttribute, ref attribute);
            if (baseType != null) return XmlDeserializer.BaseMethod.MakeGenericMethod(type, baseType).CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T?>));
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
        internal static bool GetTypeSerializeDelegate(Type type, ref AutoCSer.Extensions.Metadata.GenericType? genericType, out AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
#else
        internal static bool GetTypeSerializeDelegate(Type type, ref AutoCSer.Extensions.Metadata.GenericType genericType, out AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
#endif
        {
            if (XmlSerializer.SerializeDelegates.TryGetValue(type, out serializeDelegateReference)) return true;
            AutoCSer.TextSerialize.SerializeDelegate serializeDelegate = XmlSerializer.CustomConfig.GetCustomSerializeDelegate(type);
            if (serializeDelegate.Delegate != null && serializeDelegate.Check(typeof(XmlSerializer), type, ref serializeDelegateReference)) return true;
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    var elementType = type.GetElementType().notNull();
                    if (!elementType.isSerializeNotSupport())
                    {
                        AutoCSer.Extensions.Metadata.GenericType.Get(elementType).GetXmlSerializeArrayDelegate(ref serializeDelegateReference);
                        return true;
                    }
                }
                if (genericType == null) genericType = AutoCSer.Extensions.Metadata.GenericType.Get(type);
                serializeDelegateReference.SetUnknown(type, genericType.XmlSerializeNotSupportDelegate);
                return true;
            }
            if (type.IsEnum)
            {
                serializeDelegateReference.SetNoLoop(AutoCSer.Extensions.Metadata.EnumGenericType.Get(type).XmlSerializeEnumDelegate);
                return true;
            }
            if (type.isSerializeNotSupport())
            {
                if (genericType == null) genericType = AutoCSer.Extensions.Metadata.GenericType.Get(type);
                serializeDelegateReference.SetUnknown(type, genericType.XmlSerializeNotSupportDelegate);
                return true;
            }
            if (type.IsGenericType && type.IsValueType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    Type[] referenceTypes = type.GetGenericArguments();
                    serializeDelegateReference.SetMember(AutoCSer.Extensions.Metadata.StructGenericType.Get(referenceTypes[0]).XmlSerializeNullableDelegate, referenceTypes);
                    return true;
                }
                //if (genericTypeDefinition == typeof(KeyValuePair<,>) || genericTypeDefinition == typeof(KeyValue<,>)) return IsEmptyString = true;
            }
            if (genericType == null && !type.IsGenericTypeDefinition) genericType = AutoCSer.Extensions.Metadata.GenericType.Get(type);
            if (genericType == null || genericType.IsSerializeConstructor)
            {
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(ICollection<>))
                    {
                        AutoCSer.Extensions.Metadata.CollectionGenericType.Get(type, interfaceType).GetXmlSerializeCollectionDelegate(ref serializeDelegateReference);
                        return true;
                    }
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
        internal static bool GetTypeSerializeDelegate(AutoCSer.Extensions.Metadata.GenericType genericType, out AutoCSer.TextSerialize.DelegateReference serializeDelegateReference, out XmlSerializeAttribute attribute, out Type? baseType)
#else
        internal static bool GetTypeSerializeDelegate(AutoCSer.Extensions.Metadata.GenericType genericType, out AutoCSer.TextSerialize.DelegateReference serializeDelegateReference, out XmlSerializeAttribute attribute, out Type baseType)
#endif
        {
            attribute = XmlSerializer.AllMemberAttribute;
            Type type = genericType.CurrentType;
            var genericTypeParameter = genericType;
            if (GetTypeSerializeDelegate(type, ref genericTypeParameter, out serializeDelegateReference))
            {
                baseType = null;
                return true;
            }
            baseType = GetBaseAttribute(type, XmlSerializer.AllMemberAttribute, ref attribute);
            if (baseType != null)
            {
                AutoCSer.Extensions.Metadata.BaseGenericType.Get(type, baseType).GetXmlSerializeBaseDelegate(ref serializeDelegateReference);
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
            var genericType = default(AutoCSer.Extensions.Metadata.GenericType);
            AutoCSer.TextSerialize.DelegateReference serializeDelegateReference;
            if (GetTypeSerializeDelegate(type, ref genericType, out serializeDelegateReference)) return serializeDelegateReference.Delegate.Delegate.notNull();

            return (genericType ?? AutoCSer.Extensions.Metadata.GenericType.Get(type)).XmlSerializeDelegate;
        }
        /// <summary>
        /// 是否输出字符串函数信息
        /// </summary>
        private static readonly Delegate isOutputSubStringMethod = (Func<XmlSerializer, SubString, bool>)XmlSerializer.IsOutputSubString;
        /// <summary>
        /// 是否输出字符串函数信息
        /// </summary>
        private static readonly Delegate isOutputStringMethod = (Func<XmlSerializer, string, bool>)XmlSerializer.IsOutputString;
        /// <summary>
        /// 是否输出对象函数信息
        /// </summary>
        private static readonly Delegate isOutputMethod = (Func<XmlSerializer, object, bool>)XmlSerializer.IsOutput;
        /// <summary>
        /// 获取是否输出对象函数信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static Delegate? GetIsOutputDelegate(Type type)
#else
        internal static Delegate GetIsOutputDelegate(Type type)
#endif
        {
            if (type.IsValueType) return type == typeof(SubString) ? isOutputSubStringMethod : GetIsOutputNullable(type);
            return type == typeof(string) ? isOutputStringMethod : isOutputMethod;
        }
        /// <summary>
        /// 获取是否输出可空对象函数信息
        /// </summary>
        /// <param name="type">数组类型</param>
        /// <returns>数组转换委托调用函数信息</returns>
#if NetStandard21
        public static Delegate? GetIsOutputNullable(Type type)
#else
        public static Delegate GetIsOutputNullable(Type type)
#endif
        {
            if (type.IsGenericType && type.IsValueType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return AutoCSer.Extensions.Metadata.StructGenericType.Get(type.GetGenericArguments()[0]).XmlSerializeIsOutputNullableMethod;
            }
            return null;
        }
        /// <summary>
        /// 获取类型默认反序列化委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
#if NetStandard21
        private static Delegate? getTypeDeserializeDelegate(Type type, ref AutoCSer.Extensions.Metadata.GenericType? genericType)
#else
        private static Delegate getTypeDeserializeDelegate(Type type, ref AutoCSer.Extensions.Metadata.GenericType genericType)
#endif
        {
            var deserializeDelegate = XmlDeserializer.GetDeserializeDelegate(type);
            if (deserializeDelegate != null) return deserializeDelegate;
            deserializeDelegate = XmlSerializer.CustomConfig.GeteCustomDeserializDelegate(type);
            if (deserializeDelegate != null)
            {
                var checkType = AutoCSer.Common.CheckDeserializeType(typeof(XmlDeserializer), deserializeDelegate);
                if (type == checkType) return deserializeDelegate;
                if (checkType != null) AutoCSer.LogHelper.ErrorIgnoreException($"自定义类型反序列化函数数据类型不匹配 {type.fullName()} <> {checkType.fullName()}", LogLevelEnum.AutoCSer | LogLevelEnum.Error);
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    var elementType = type.GetElementType().notNull();
                    if (!elementType.isSerializeNotSupport()) return AutoCSer.Extensions.Metadata.GenericType.Get(elementType).XmlDeserializeArrayDelegate;
                }
                if(genericType == null) genericType = AutoCSer.Extensions.Metadata.GenericType.Get(type);
                return genericType.XmlDeserializeNotSupportDelegate;
            }
            if (type.IsEnum)
            {
                if (type.IsDefined(typeof(FlagsAttribute), false)) return AutoCSer.Extensions.Metadata.EnumGenericType.Get(type).XmlDeserializeEnumFlagsDelegate;
                return AutoCSer.Extensions.Metadata.EnumGenericType.Get(type).XmlDeserializeEnumDelegate;
            }
            if (type.isSerializeNotSupport())
            {
                if (genericType == null) genericType = AutoCSer.Extensions.Metadata.GenericType.Get(type);
                return genericType.XmlDeserializeNotSupportDelegate;
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (type.IsValueType)
                {
                    if (genericTypeDefinition == typeof(Nullable<>))
                    {
                        return AutoCSer.Extensions.Metadata.StructGenericType.Get(type.GetGenericArguments()[0]).XmlDeserializeNullableDelegate;
                    }
                    if (genericTypeDefinition == typeof(KeyValuePair<,>))
                    {
                        return AutoCSer.Extensions.Metadata.GenericType2.Get(type.GetGenericArguments()).XmlDeserializeKeyValuePairDelegate;
                    }
                    if (genericTypeDefinition == typeof(LeftArray<>))
                    {
                        return AutoCSer.Extensions.Metadata.GenericType.Get(type.GetGenericArguments()[0]).XmlDeserializeLeftArrayDelegate;
                    }
                }
                else
                {
                    if (genericTypeDefinition == typeof(ListArray<>))
                    {
                        return AutoCSer.Extensions.Metadata.GenericType.Get(type.GetGenericArguments()[0]).XmlDeserializeListArrayDelegate;
                    }
                }
            }
            if (genericType == null) genericType = AutoCSer.Extensions.Metadata.GenericType.Get(type);
            if (genericType.IsSerializeConstructor)
            {
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(ICollection<>))
                    {
                        return AutoCSer.Extensions.Metadata.CollectionGenericType.Get(type, interfaceType).XmlDeserializeCollectionDelegate;
                    }
                }
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
        internal static Delegate? GetTypeDeserializeDelegate(AutoCSer.Extensions.Metadata.GenericType genericType, out XmlSerializeAttribute attribute, out Type? baseType)
#else
        internal static Delegate GetTypeDeserializeDelegate(AutoCSer.Extensions.Metadata.GenericType genericType, out XmlSerializeAttribute attribute, out Type baseType)
#endif
        {
            attribute = XmlDeserializer.AllMemberAttribute;
            Type type = genericType.CurrentType;
            var genericTypeParameter = genericType;
            var deserializeDelegate = getTypeDeserializeDelegate(type, ref genericTypeParameter);
            if (deserializeDelegate != null)
            {
                baseType = null;
                return deserializeDelegate;
            }
            baseType = GetBaseAttribute(type, XmlDeserializer.AllMemberAttribute, ref attribute);
            if (baseType != null) return AutoCSer.Extensions.Metadata.BaseGenericType.Get(type, baseType).XmlDeserializeBaseDelegate;
            return null;
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
        /// <returns></returns>
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
        /// <returns></returns>
        private static Delegate getMemberDeserializeDelegate(Type type)
        {
            XmlSerializeAttribute attribute;
            AutoCSer.Extensions.Metadata.GenericType genericType = AutoCSer.Extensions.Metadata.GenericType.Get(type);
            var baseType = default(Type);
            var deserializeDelegate = GetTypeDeserializeDelegate(genericType, out attribute, out baseType);
            return deserializeDelegate ?? genericType.XmlDeserializeDelegate;
        }
        /// <summary>
        /// 创建成员反序列化委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        internal static DynamicMethod CreateDynamicMethod(Type type, FieldInfo field)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "XmlDeserializer" + field.Name, null, new Type[] { typeof(XmlDeserializer), type.MakeByRefType() }, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            Delegate deserializeDelegate = GetMemberDeserializeDelegate(field.FieldType);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            if (!type.IsValueType) generator.Emit(OpCodes.Ldind_Ref);
            generator.Emit(OpCodes.Ldflda, field);
            generator.call(deserializeDelegate.Method);
            generator.ret();
            return dynamicMethod;
        }
        /// <summary>
        /// 创建成员反序列化委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="property"></param>
        /// <param name="propertyMethod"></param>
        /// <returns></returns>
        internal static DynamicMethod CreateDynamicMethod(Type type, PropertyInfo property, MethodInfo propertyMethod)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(AutoCSer.Common.NamePrefix + "XmlDeserializer" + property.Name, null, new Type[] { typeof(XmlDeserializer), type.MakeByRefType() }, type, true);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            Type memberType = property.PropertyType;
            LocalBuilder loadMember = generator.DeclareLocal(memberType);
            generator.initobjShort(memberType, loadMember);
            Delegate deserializeDelegate = GetMemberDeserializeDelegate(memberType);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldloca_S, loadMember);
            generator.call(deserializeDelegate.Method);

            generator.Emit(OpCodes.Ldarg_1);
            if (!type.IsValueType) generator.Emit(OpCodes.Ldind_Ref);
            generator.Emit(OpCodes.Ldloc_0);
            generator.call(propertyMethod);
            generator.ret();
            return dynamicMethod;
        }
#endif
        /// <summary>
        /// 获取 XML 序列化类型配置
        /// </summary>
        /// <param name="type"></param>
        /// <param name="defaultAttribute"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
#if NetStandard21
        internal static Type? GetBaseAttribute(Type type, XmlSerializeAttribute defaultAttribute, ref XmlSerializeAttribute attribute)
#else
        internal static Type GetBaseAttribute(Type type, XmlSerializeAttribute defaultAttribute, ref XmlSerializeAttribute attribute)
#endif
        {
            for (var baseType = type; baseType != typeof(object);)
            {
                var baseAttribute = baseType.GetCustomAttribute(typeof(XmlSerializeAttribute), false);
                if (baseAttribute != null)
                {
                    attribute = (XmlSerializeAttribute)baseAttribute;
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
                reference.ReferenceTypes = new AutoCSer.Metadata.GenericType[referenceTypes.Length];
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
                    AutoCSer.Extensions.Metadata.GenericType genericType = AutoCSer.Extensions.Metadata.GenericType.Get(memberType.notNull());
                    AutoCSer.TextSerialize.DelegateReference serializeDelegateReference = genericType.XmlSerializeDelegateReference;
                    reference.IsUnknownMember |= serializeDelegateReference.IsUnknownMember;
                    if (!serializeDelegateReference.IsCompleted || serializeDelegateReference.IsCheckMember)
                    {
                        reference.ReferenceTypes[memberIndex++] = AutoCSer.Metadata.GenericType.Get(memberType.notNull());
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
                                typeArray.Array[typeArray.Length++].Set(AutoCSer.Extensions.Metadata.GenericType.Get(genericType.CurrentType).XmlSerializeDelegateReference);
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
        internal static AutoCSer.Metadata.GenericType? Check(ref AutoCSer.TextSerialize.LoopTypeArray array, Type type, HashSet<HashObject<System.Type>> types)
#else
        internal static AutoCSer.Metadata.GenericType Check(ref AutoCSer.TextSerialize.LoopTypeArray array, Type type, HashSet<HashObject<System.Type>> types)
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
                        AutoCSer.TextSerialize.DelegateReference serializeDelegateReference = AutoCSer.Extensions.Metadata.GenericType.Get(genericType.CurrentType).XmlSerializeDelegateReference;
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
                        if (type == currentType) return AutoCSer.Metadata.GenericType.Get(type);
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
                        AutoCSer.Extensions.Metadata.GenericType genericType = AutoCSer.Extensions.Metadata.GenericType.Get(currentType);
                        AutoCSer.TextSerialize.DelegateReference serializeDelegateReference = genericType.XmlSerializeDelegateReference;
                        if (!serializeDelegateReference.IsCompleted || serializeDelegateReference.IsCheckMember)
                        {
                            if (array.Index != referenceTypes.Length) return AutoCSer.Metadata.GenericType.Get(currentType);
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
