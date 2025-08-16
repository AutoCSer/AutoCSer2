using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Security.AccessControl;
using System.Threading;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 公共调用
    /// </summary>
    internal static class Common
    {
        /// <summary>
        /// 获取二进制数据序列化类型配置
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
#if NetStandard21
        internal static Type? GetBaseAttribute(Type type, ref BinarySerializeAttribute attribute)
#else
        internal static Type GetBaseAttribute(Type type, ref BinarySerializeAttribute attribute)
#endif
        {
            for (var baseType = type; baseType != typeof(object);)
            {
                var baseAttribute = baseType.GetCustomAttribute(typeof(BinarySerializeAttribute), false);
                if (baseAttribute != null)
                {
                    attribute = (BinarySerializeAttribute)baseAttribute;
                    if (type != baseType && attribute.IsBaseType)
                    {
                        return baseType;
                    }
                    break;
                }
                if (!object.ReferenceEquals(attribute, BinarySerializer.DefaultAttribute)) break;
                if ((baseType = baseType.BaseType) == null) break;
            }
            return null;
        }
        /// <summary>
        /// 成员序列化委托集合访问锁
        /// </summary>
        private static readonly object memberSerializeDelegateLock = new object();
        /// <summary>
        /// 成员反序列化委托集合访问锁
        /// </summary>
        private static readonly object memberDeserializeDelegateLock = new object();

#if AOT
        /// <summary>
        /// 获取类型默认序列化委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializeDelegateReference"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        internal static bool GetTypeSerializeDelegate<T>(out SerializeDelegateReference serializeDelegateReference, out BinarySerializeAttribute attribute)
        {
            attribute = BinarySerializer.DefaultAttribute;
            Type type = typeof(T);
            if (BinarySerializer.SerializeDelegates.TryGetValue(type, out serializeDelegateReference)) return true;
            if (typeof(ICustomSerialize<T>).IsAssignableFrom(type))
            {
                Type?[]? referenceTypes = null;
                var customAttribute = (BinarySerializeAttribute?)type.GetCustomAttribute(typeof(BinarySerializeAttribute), false);
                if (customAttribute != null)
                {
                    referenceTypes = customAttribute.CustomReferenceTypes;
                    if (referenceTypes != null && type.IsGenericType)
                    {
                        int referenceTypeIndex = 0;
                        foreach (var referenceType in referenceTypes)
                        {
                            if (referenceType == null) referenceTypes[referenceTypeIndex] = type.GetGenericArguments()[0];
                            else if (referenceType.IsGenericTypeDefinition)
                            {
                                referenceTypes[referenceTypeIndex] = referenceType.MakeGenericType(type.GetGenericArguments());
                            }
                            ++referenceTypeIndex;
                        }
                    }
                    attribute = customAttribute;
                }
                SerializeDelegate serializeDelegate = new SerializeDelegate(BinarySerializer.ICustomMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<BinarySerializer, T>)), referenceTypes);
                if (serializeDelegate.Check(type, ref serializeDelegateReference)) return true;
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    Type elementType = type.GetElementType().notNull();
                    if (!elementType.IsAbstract && !elementType.isSerializeNotSupport())
                    {
                        if (elementType.IsValueType)
                        {
                            if (elementType.IsEnum)
                            {
                                MethodInfo method = AutoCSer.Common.EmptyAction.Method;
                                switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                                {
                                    case UnderlyingTypeEnum.Int: method = BinarySerializer.EnumIntArrayMethod; break;
                                    case UnderlyingTypeEnum.UInt: method = BinarySerializer.EnumUIntArrayMethod; break;
                                    case UnderlyingTypeEnum.Byte: method = BinarySerializer.EnumByteArrayMethod; break;
                                    case UnderlyingTypeEnum.ULong: method = BinarySerializer.EnumULongArrayMethod; break;
                                    case UnderlyingTypeEnum.UShort: method = BinarySerializer.EnumUShortArrayMethod; break;
                                    case UnderlyingTypeEnum.Long: method = BinarySerializer.EnumLongArrayMethod; break;
                                    case UnderlyingTypeEnum.Short: method = BinarySerializer.EnumShortArrayMethod; break;
                                    case UnderlyingTypeEnum.SByte: method = BinarySerializer.EnumSByteArrayMethod; break;
                                }
                                serializeDelegateReference.SetPrimitive(method.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, T>)), type);
                            }
                            else if (elementType.isValueTypeNullable())
                            {
                                Type[] referenceTypes = elementType.GetGenericArguments();
                                serializeDelegateReference.SetMember(BinarySerializer.NullableArrayMethod.MakeGenericMethod(referenceTypes[0]).CreateDelegate(typeof(Action<BinarySerializer, T>)), referenceTypes, SerializePushTypeEnum.Primitive, true);
                            }
                            else serializeDelegateReference.SetMember(BinarySerializer.StructArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, T>)), new Type[] { elementType }, SerializePushTypeEnum.Primitive, true);
                        }
                        else serializeDelegateReference.SetMember(BinarySerializer.ArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, T>)), new Type[] { elementType }, SerializePushTypeEnum.Primitive, true);
                        return true;
                    }
                }
                serializeDelegateReference.SetUnknown(type, BinarySerializer.NotSupportMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<BinarySerializer, T>)));
                return true;
            }
            if (type.IsEnum)
            {
                MethodInfo method = AutoCSer.Common.EmptyAction.Method;
                switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)))
                {
                    case UnderlyingTypeEnum.Int: method = BinarySerializer.EnumIntMethod; break;
                    case UnderlyingTypeEnum.UInt: method = BinarySerializer.EnumUIntMethod; break;
                    case UnderlyingTypeEnum.Byte: method = BinarySerializer.EnumByteMethod; break;
                    case UnderlyingTypeEnum.ULong: method = BinarySerializer.EnumULongMethod; break;
                    case UnderlyingTypeEnum.UShort: method = BinarySerializer.EnumUShortMethod; break;
                    case UnderlyingTypeEnum.Long: method = BinarySerializer.EnumLongMethod; break;
                    case UnderlyingTypeEnum.Short: method = BinarySerializer.EnumShortMethod; break;
                    case UnderlyingTypeEnum.SByte: method = BinarySerializer.EnumSByteMethod; break;
                }
                serializeDelegateReference.SetPrimitive(method.MakeGenericMethod(type).CreateDelegate(typeof(Action<BinarySerializer, T>)));
                return true;
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(LeftArray<>))
                {
                    Type[] elementTypeArray = type.GetGenericArguments();
                    Type elementType = elementTypeArray[0];
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum)
                        {
                            MethodInfo method = AutoCSer.Common.EmptyAction.Method;
                            switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                            {
                                case UnderlyingTypeEnum.Int: method = BinarySerializer.EnumIntLeftArrayMethod; break;
                                case UnderlyingTypeEnum.UInt: method = BinarySerializer.EnumUIntLeftArrayMethod; break;
                                case UnderlyingTypeEnum.Byte: method = BinarySerializer.EnumByteLeftArrayMethod; break;
                                case UnderlyingTypeEnum.ULong: method = BinarySerializer.EnumULongLeftArrayMethod; break;
                                case UnderlyingTypeEnum.UShort: method = BinarySerializer.EnumUShortLeftArrayMethod; break;
                                case UnderlyingTypeEnum.Long: method = BinarySerializer.EnumLongLeftArrayMethod; break;
                                case UnderlyingTypeEnum.Short: method = BinarySerializer.EnumShortLeftArrayMethod; break;
                                case UnderlyingTypeEnum.SByte: method = BinarySerializer.EnumSByteLeftArrayMethod; break;
                            }
                            serializeDelegateReference.SetNotReference(method.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, T>)));
                            return true;
                        }
                        if (elementType.isValueTypeNullable())
                        {
                            Type[] referenceTypes = elementType.GetGenericArguments();
                            serializeDelegateReference.SetMember(BinarySerializer.NullableLeftArrayMethod.MakeGenericMethod(referenceTypes[0]).CreateDelegate(typeof(Action<BinarySerializer, T>)), referenceTypes, SerializePushTypeEnum.Primitive, true);
                        }
                        else serializeDelegateReference.SetMember(BinarySerializer.StructLeftArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, T>)), elementTypeArray, SerializePushTypeEnum.Primitive, true);
                    }
                    else serializeDelegateReference.SetMember(BinarySerializer.LeftArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, T>)), elementTypeArray, SerializePushTypeEnum.Primitive, true);
                    return true;
                }
                if (genericTypeDefinition == typeof(ListArray<>))
                {
                    Type[] elementTypeArray = type.GetGenericArguments();
                    Type elementType = elementTypeArray[0];
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum)
                        {
                            MethodInfo method = AutoCSer.Common.EmptyAction.Method;
                            switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                            {
                                case UnderlyingTypeEnum.Int: method = BinarySerializer.EnumIntListArrayMethod; break;
                                case UnderlyingTypeEnum.UInt: method = BinarySerializer.EnumUIntListArrayMethod; break;
                                case UnderlyingTypeEnum.Byte: method = BinarySerializer.EnumByteListArrayMethod; break;
                                case UnderlyingTypeEnum.ULong: method = BinarySerializer.EnumULongListArrayMethod; break;
                                case UnderlyingTypeEnum.UShort: method = BinarySerializer.EnumUShortListArrayMethod; break;
                                case UnderlyingTypeEnum.Long: method = BinarySerializer.EnumLongListArrayMethod; break;
                                case UnderlyingTypeEnum.Short: method = BinarySerializer.EnumShortListArrayMethod; break;
                                case UnderlyingTypeEnum.SByte: method = BinarySerializer.EnumSByteListArrayMethod; break;
                            }
                            serializeDelegateReference.SetPrimitive(method.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, T>)), type);
                            return true;
                        }
                        if (elementType.isValueTypeNullable())
                        {
                            Type[] referenceTypes = elementType.GetGenericArguments();
                            serializeDelegateReference.SetMember(BinarySerializer.NullableListArrayMethod.MakeGenericMethod(referenceTypes[0]).CreateDelegate(typeof(Action<BinarySerializer, T>)), referenceTypes, SerializePushTypeEnum.Primitive, true);
                        }
                        else serializeDelegateReference.SetMember(BinarySerializer.StructListArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, T>)), elementTypeArray, SerializePushTypeEnum.Primitive, true);
                    }
                    else serializeDelegateReference.SetMember(BinarySerializer.ListArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, T>)), elementTypeArray, SerializePushTypeEnum.Primitive, true);
                    return true;
                }
            }
            if (type.IsAbstract || type.isSerializeNotSupport())
            {
                serializeDelegateReference.SetUnknown(type, BinarySerializer.NotSupportMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<BinarySerializer, T>)));
                return true;
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    Type[] referenceTypes = type.GetGenericArguments();
                    serializeDelegateReference.SetMember(BinarySerializer.NullableMethod.MakeGenericMethod(referenceTypes[0]).CreateDelegate(typeof(Action<BinarySerializer, T>)), referenceTypes, SerializePushTypeEnum.Primitive);
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
                        serializeDelegateReference.SetMember(BinarySerializer.DictionaryMethod.MakeGenericMethod(type, referenceTypes[0], referenceTypes[1]).CreateDelegate(typeof(Action<BinarySerializer, T>)), referenceTypes, SerializePushTypeEnum.Primitive, true);
                        return true;
                    }
                    if (collectionType == null && genericTypeDefinition == typeof(ICollection<>)) collectionType = interfaceType;
                }
                if (collectionType != null)
                {
                    Type[] referenceTypes = collectionType.GetGenericArguments();
                    serializeDelegateReference.SetMember(BinarySerializer.CollectionMethod.MakeGenericMethod(type, referenceTypes[0]).CreateDelegate(typeof(Action<BinarySerializer, T>)), referenceTypes, SerializePushTypeEnum.Primitive, true);
                    return true;
                }
            }
            var baseType = GetBaseAttribute(type, ref attribute);
            if (baseType != null)
            {
                serializeDelegateReference.SetMember(BinarySerializer.BaseMethod.MakeGenericMethod(type, baseType).CreateDelegate(typeof(Action<BinarySerializer, T>)), new Type[] { baseType }, SerializePushTypeEnum.Primitive);
                return true;
            }
            if (!object.ReferenceEquals(attribute, BinarySerializer.DefaultAttribute) && attribute.IsJsonMix)
            {
                if (type.IsValueType) serializeDelegateReference = new SerializeDelegateReference(BinarySerializer.StructJsonMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<BinarySerializer, T>)));
                else serializeDelegateReference = new SerializeDelegateReference(BinarySerializer.JsonMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<BinarySerializer, T>)), type);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 成员序列化委托集合
        /// </summary>
        private static readonly Dictionary<HashObject<Type>, Action<BinarySerializer, object?>> memberSerializeDelegates = DictionaryCreator.CreateHashObject<Type, Action<BinarySerializer, object?>>();
        /// <summary>
        /// 获取成员序列化委托
        /// </summary>
        /// <param name="type">成员类型</param>
        internal static Action<BinarySerializer, object?> GetMemberSerializeDelegate(Type type)
        {
            var memberSerializeDelegate = default(Action<BinarySerializer, object?>);
            HashObject<Type> hashType = type;
            Monitor.Enter(memberSerializeDelegateLock);
            try
            {
                if (!memberSerializeDelegates.TryGetValue(hashType, out memberSerializeDelegate))
                {
                    memberSerializeDelegates.Add(hashType, memberSerializeDelegate = (Action<BinarySerializer, object?>)getMemberSerializeDelegate(type));
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
            SerializeDelegateReference serializeDelegateReference;
            if (BinarySerializer.SerializeDelegates.TryGetValue(type, out serializeDelegateReference)) return serializeDelegateReference.Delegate.MemberDelegate.notNull();
            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type))
            {
                return BinarySerializer.ICustomReflectionMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<BinarySerializer, object?>));
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    Type elementType = type.GetElementType().notNull();
                    if (!elementType.IsAbstract && !elementType.isSerializeNotSupport())
                    {
                        if (elementType.IsValueType)
                        {
                            if (elementType.IsEnum)
                            {
                                MethodInfo method = AutoCSer.Common.EmptyAction.Method;
                                switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                                {
                                    case UnderlyingTypeEnum.Int: method = BinarySerializer.EnumIntArrayReflectionMethod; break;
                                    case UnderlyingTypeEnum.UInt: method = BinarySerializer.EnumUIntArrayReflectionMethod; break;
                                    case UnderlyingTypeEnum.Byte: method = BinarySerializer.EnumByteArrayReflectionMethod; break;
                                    case UnderlyingTypeEnum.ULong: method = BinarySerializer.EnumULongArrayReflectionMethod; break;
                                    case UnderlyingTypeEnum.UShort: method = BinarySerializer.EnumUShortArrayReflectionMethod; break;
                                    case UnderlyingTypeEnum.Long: method = BinarySerializer.EnumLongArrayReflectionMethod; break;
                                    case UnderlyingTypeEnum.Short: method = BinarySerializer.EnumShortArrayReflectionMethod; break;
                                    case UnderlyingTypeEnum.SByte: method = BinarySerializer.EnumSByteArrayReflectionMethod; break;
                                }
                                return method.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, object?>));
                            }
                            if (elementType.isValueTypeNullable())
                            {
                                return BinarySerializer.NullableArrayReflectionMethod.MakeGenericMethod(elementType.GetGenericArguments()[0]).CreateDelegate(typeof(Action<BinarySerializer, object?>));
                            }
                            return BinarySerializer.StructArrayReflectionMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, object?>));
                        }
                        return BinarySerializer.ArrayReflectionMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, object?>));
                    }
                }
                return BinarySerializer.NotSupportReflectionMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<BinarySerializer, object?>));
            }
            if (type.IsEnum)
            {
                MethodInfo method = AutoCSer.Common.EmptyAction.Method;
                switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)))
                {
                    case UnderlyingTypeEnum.Int: method = BinarySerializer.PrimitiveMemberIntReflectionMethod; break;
                    case UnderlyingTypeEnum.UInt: method = BinarySerializer.PrimitiveMemberUIntReflectionMethod; break;
                    case UnderlyingTypeEnum.Byte: method = BinarySerializer.PrimitiveMemberByteReflectionMethod; break;
                    case UnderlyingTypeEnum.ULong: method = BinarySerializer.PrimitiveMemberULongReflectionMethod; break;
                    case UnderlyingTypeEnum.UShort: method = BinarySerializer.PrimitiveMemberUShortReflectionMethod; break;
                    case UnderlyingTypeEnum.Long: method = BinarySerializer.PrimitiveMemberLongReflectionMethod; break;
                    case UnderlyingTypeEnum.Short: method = BinarySerializer.PrimitiveMemberShortReflectionMethod; break;
                    case UnderlyingTypeEnum.SByte: method = BinarySerializer.PrimitiveMemberSByteReflectionMethod; break;
                }
                return method.MakeGenericMethod(type).CreateDelegate(typeof(Action<BinarySerializer, object?>));
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(LeftArray<>))
                {
                    Type elementType = type.GetGenericArguments()[0];
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum)
                        {
                            MethodInfo method = AutoCSer.Common.EmptyAction.Method;
                            switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                            {
                                case UnderlyingTypeEnum.Int: method = BinarySerializer.EnumIntLeftArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.UInt: method = BinarySerializer.EnumUIntLeftArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.Byte: method = BinarySerializer.EnumByteLeftArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.ULong: method = BinarySerializer.EnumULongLeftArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.UShort: method = BinarySerializer.EnumUShortLeftArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.Long: method = BinarySerializer.EnumLongLeftArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.Short: method = BinarySerializer.EnumShortLeftArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.SByte: method = BinarySerializer.EnumSByteLeftArrayReflectionMethod; break;
                            }
                            return method.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, object>));
                        }
                        if (elementType.isValueTypeNullable())
                        {
                            return BinarySerializer.NullableLeftArrayReflectionMethod.MakeGenericMethod(elementType.GetGenericArguments()[0]).CreateDelegate(typeof(Action<BinarySerializer, object>));
                        }
                        return BinarySerializer.StructLeftArrayReflectionMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, object>));
                    }
                    return BinarySerializer.LeftArrayReflectionMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, object>));
                }
                if (genericTypeDefinition == typeof(ListArray<>))
                {
                    Type elementType = type.GetGenericArguments()[0];
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum)
                        {
                            MethodInfo method = AutoCSer.Common.EmptyAction.Method;
                            switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                            {
                                case UnderlyingTypeEnum.Int: method = BinarySerializer.EnumIntListArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.UInt: method = BinarySerializer.EnumUIntListArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.Byte: method = BinarySerializer.EnumByteListArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.ULong: method = BinarySerializer.EnumULongListArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.UShort: method = BinarySerializer.EnumUShortListArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.Long: method = BinarySerializer.EnumLongListArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.Short: method = BinarySerializer.EnumShortListArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.SByte: method = BinarySerializer.EnumSByteListArrayReflectionMethod; break;
                            }
                            return method.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, object?>));
                        }
                        if (elementType.isValueTypeNullable())
                        {
                            return BinarySerializer.NullableListArrayReflectionMethod.MakeGenericMethod(elementType.GetGenericArguments()[0]).CreateDelegate(typeof(Action<BinarySerializer, object?>));
                        }
                        return BinarySerializer.StructListArrayReflectionMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, object?>));
                    }
                    return BinarySerializer.ListArrayReflectionMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Action<BinarySerializer, object?>));
                }
            }
            if (type.IsAbstract || type.isSerializeNotSupport()) return BinarySerializer.NotSupportReflectionMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<BinarySerializer, object?>));
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>)) return BinarySerializer.NullableReflectionMethod.MakeGenericMethod(type.GetGenericArguments()[0]).CreateDelegate(typeof(Action<BinarySerializer, object?>));
            }
            if (type.IsValueType || DefaultConstructor.GetIsSerializeConstructorMethod.MakeGenericMethod(type).Invoke(null, null) != null)
            {
                var collectionType = default(Type);
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IDictionary<,>))
                    {
                        Type[] referenceTypes = interfaceType.GetGenericArguments();
                        return BinarySerializer.DictionaryReflectionMethod.MakeGenericMethod(type, referenceTypes[0], referenceTypes[1]).CreateDelegate(typeof(Action<BinarySerializer, object?>));
                    }
                    if (collectionType == null && genericTypeDefinition == typeof(ICollection<>)) collectionType = interfaceType;
                }
                if (collectionType != null) return BinarySerializer.CollectionReflectionMethod.MakeGenericMethod(type, collectionType.GetGenericArguments()[0]).CreateDelegate(typeof(Action<BinarySerializer, object?>));
            }
            BinarySerializeAttribute attribute = BinarySerializer.DefaultAttribute;
            var baseType = GetBaseAttribute(type, ref attribute);
            if (baseType != null) return BinarySerializer.BaseReflectionMethod.MakeGenericMethod(type, baseType).CreateDelegate(typeof(Action<BinarySerializer, object?>));
            if (!object.ReferenceEquals(attribute, BinarySerializer.DefaultAttribute) && attribute.IsJsonMix)
            {
                //GenericType genericType = GenericType.Get(type);
                if (type.IsValueType) return BinarySerializer.StructJsonReflectionMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<BinarySerializer, object>));
                return BinarySerializer.JsonReflectionMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<BinarySerializer, object?>));
            }
            return BinarySerializer.SerializeReflectionMethod.MakeGenericMethod(type).CreateDelegate(typeof(Action<BinarySerializer, object?>));
        }
        /// <summary>
        /// 获取类型默认反序列化委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializeDelegate"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        internal static bool GetTypeDeserializeDelegate<T>(out DeserializeDelegate deserializeDelegate, out BinarySerializeAttribute attribute)
        {
            Type type = typeof(T);
            attribute = BinarySerializer.DefaultAttribute;
            if (BinaryDeserializer.DeserializeDelegates.TryGetValue(type, out deserializeDelegate)) return true;
            if (typeof(ICustomSerialize<T>).IsAssignableFrom(type))
            {
                deserializeDelegate.Set(BinaryDeserializer.ICustomMethod.MakeGenericMethod(type).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T?>)), null, true);
                return true;
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    Type elementType = type.GetElementType().notNull();
                    if (!elementType.IsAbstract && !elementType.isSerializeNotSupport())
                    {
                        if (elementType.IsValueType)
                        {
                            if (elementType.IsEnum)
                            {
                                MethodInfo method = AutoCSer.Common.EmptyAction.Method;
                                switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                                {
                                    case UnderlyingTypeEnum.Int: method = BinaryDeserializer.EnumIntArrayMethod; break;
                                    case UnderlyingTypeEnum.UInt: method = BinaryDeserializer.EnumUIntArrayMethod; break;
                                    case UnderlyingTypeEnum.Byte: method = BinaryDeserializer.EnumByteArrayMethod; break;
                                    case UnderlyingTypeEnum.ULong: method = BinaryDeserializer.EnumULongArrayMethod; break;
                                    case UnderlyingTypeEnum.UShort: method = BinaryDeserializer.EnumUShortArrayMethod; break;
                                    case UnderlyingTypeEnum.Long: method = BinaryDeserializer.EnumLongArrayMethod; break;
                                    case UnderlyingTypeEnum.Short: method = BinaryDeserializer.EnumShortArrayMethod; break;
                                    case UnderlyingTypeEnum.SByte: method = BinaryDeserializer.EnumSByteArrayMethod; break;
                                }
                                deserializeDelegate.Set(method.MakeGenericMethod(elementType).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T?>)), null, true);
                            }
                            else if (elementType.isValueTypeNullable())
                            {
                                deserializeDelegate.Set(BinaryDeserializer.NullableArrayMethod.MakeGenericMethod(elementType.GetGenericArguments()[0]).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T?>)), null, true);
                            }
                            else deserializeDelegate.Set(BinaryDeserializer.StructArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T?>)), null, true);
                        }
                        else deserializeDelegate.Set(BinaryDeserializer.ArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T?>)), null, true);
                        return true;
                    }
                }
                deserializeDelegate.Set(BinaryDeserializer.NotSupportMethod.MakeGenericMethod(type).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T?>)), null, true);
                return true;
            }
            if (type.IsEnum)
            {
                MethodInfo method = AutoCSer.Common.EmptyAction.Method;
                switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)))
                {
                    case UnderlyingTypeEnum.Int: method = BinaryDeserializer.EnumIntMethod; break;
                    case UnderlyingTypeEnum.UInt: method = BinaryDeserializer.EnumUIntMethod; break;
                    case UnderlyingTypeEnum.Byte: method = BinaryDeserializer.EnumByteMethod; break;
                    case UnderlyingTypeEnum.ULong: method = BinaryDeserializer.EnumULongMethod; break;
                    case UnderlyingTypeEnum.UShort: method = BinaryDeserializer.EnumUShortMethod; break;
                    case UnderlyingTypeEnum.Long: method = BinaryDeserializer.EnumLongMethod; break;
                    case UnderlyingTypeEnum.Short: method = BinaryDeserializer.EnumShortMethod; break;
                    case UnderlyingTypeEnum.SByte: method = BinaryDeserializer.EnumSByteMethod; break;
                }
                deserializeDelegate.Set(method.MakeGenericMethod(type).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T>)), null, true);
                return true;
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(LeftArray<>))
                {
                    Type elementType = type.GetGenericArguments()[0];
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum)
                        {
                            MethodInfo method = AutoCSer.Common.EmptyAction.Method;
                            switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                            {
                                case UnderlyingTypeEnum.Int: method = BinaryDeserializer.EnumIntLeftArrayMethod; break;
                                case UnderlyingTypeEnum.UInt: method = BinaryDeserializer.EnumUIntLeftArrayMethod; break;
                                case UnderlyingTypeEnum.Byte: method = BinaryDeserializer.EnumByteLeftArrayMethod; break;
                                case UnderlyingTypeEnum.ULong: method = BinaryDeserializer.EnumULongLeftArrayMethod; break;
                                case UnderlyingTypeEnum.UShort: method = BinaryDeserializer.EnumUShortLeftArrayMethod; break;
                                case UnderlyingTypeEnum.Long: method = BinaryDeserializer.EnumLongLeftArrayMethod; break;
                                case UnderlyingTypeEnum.Short: method = BinaryDeserializer.EnumShortLeftArrayMethod; break;
                                case UnderlyingTypeEnum.SByte: method = BinaryDeserializer.EnumSByteLeftArrayMethod; break;
                            }
                            deserializeDelegate.Set(method.MakeGenericMethod(elementType).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T>)), null, true);
                        }
                        else if (elementType.isValueTypeNullable())
                        {
                            deserializeDelegate.Set(BinaryDeserializer.NullableLeftArrayMethod.MakeGenericMethod(elementType.GetGenericArguments()[0]).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T>)), null, true);
                        }
                        else deserializeDelegate.Set(BinaryDeserializer.StructLeftArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T>)), null, true);
                    }
                    else deserializeDelegate.Set(BinaryDeserializer.LeftArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T>)), null, true);
                    return true;
                }
                if (genericTypeDefinition == typeof(ListArray<>))
                {
                    Type elementType = type.GetGenericArguments()[0];
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum)
                        {
                            MethodInfo method = AutoCSer.Common.EmptyAction.Method;
                            switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                            {
                                case UnderlyingTypeEnum.Int: method = BinaryDeserializer.EnumIntListArrayMethod; break;
                                case UnderlyingTypeEnum.UInt: method = BinaryDeserializer.EnumUIntListArrayMethod; break;
                                case UnderlyingTypeEnum.Byte: method = BinaryDeserializer.EnumByteListArrayMethod; break;
                                case UnderlyingTypeEnum.ULong: method = BinaryDeserializer.EnumULongListArrayMethod; break;
                                case UnderlyingTypeEnum.UShort: method = BinaryDeserializer.EnumUShortListArrayMethod; break;
                                case UnderlyingTypeEnum.Long: method = BinaryDeserializer.EnumLongListArrayMethod; break;
                                case UnderlyingTypeEnum.Short: method = BinaryDeserializer.EnumShortListArrayMethod; break;
                                case UnderlyingTypeEnum.SByte: method = BinaryDeserializer.EnumSByteListArrayMethod; break;
                            }
                            deserializeDelegate.Set(method.MakeGenericMethod(elementType).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T?>)), null, true);
                        }
                        else if (elementType.isValueTypeNullable())
                        {
                            deserializeDelegate.Set(BinaryDeserializer.NullableListArrayMethod.MakeGenericMethod(elementType.GetGenericArguments()[0]).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T?>)), null, true);
                        }
                        else deserializeDelegate.Set(BinaryDeserializer.StructListArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T?>)), null, true);
                    }
                    else deserializeDelegate.Set(BinaryDeserializer.ListArrayMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T?>)), null, true);
                    return true;
                }
            }
            if (type.IsAbstract || type.isSerializeNotSupport())
            {
                deserializeDelegate.Set(BinaryDeserializer.NotSupportMethod.MakeGenericMethod(type).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T?>)), null, true);
                return true;
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    deserializeDelegate.Set(BinaryDeserializer.NullableMethod.MakeGenericMethod(type.GetGenericArguments()[0]).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T>)), null, true);
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
                        deserializeDelegate.Set(BinaryDeserializer.DictionaryMethod.MakeGenericMethod(type, referenceTypes[0], referenceTypes[1]).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T?>)), null, true);
                        return true;
                    }
                    if (collectionType == null && genericTypeDefinition == typeof(ICollection<>)) collectionType = interfaceType;
                }
                if (collectionType != null)
                {
                    deserializeDelegate.Set(BinaryDeserializer.CollectionMethod.MakeGenericMethod(type, collectionType.GetGenericArguments()[0]).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T?>)), null, true);
                    return true;
                }
            }
            var baseType = GetBaseAttribute(type, ref attribute);
            if (baseType != null)
            {
                deserializeDelegate.Set(BinaryDeserializer.BaseMethod.MakeGenericMethod(type, baseType).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T?>)), null, true);
                return true;
            }
            if (!object.ReferenceEquals(attribute, BinarySerializer.DefaultAttribute) && attribute.IsJsonMix)
            {
                if (type.IsValueType) deserializeDelegate.Set(BinaryDeserializer.StructJsonMethod.MakeGenericMethod(type).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T>)), null, true);
                else deserializeDelegate.Set(BinaryDeserializer.JsonMethod.MakeGenericMethod(type).CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T?>)), null, true);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 成员反序列化委托集合
        /// </summary>
        private static readonly Dictionary<HashObject<Type>, Func<BinaryDeserializer, object?>> memberDeserializeDelegates = DictionaryCreator.CreateHashObject<Type, Func<BinaryDeserializer, object?>>();
        /// <summary>
        /// 获取成员反序列化委托
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <returns>成员反序列化委托</returns>
        internal static Func<BinaryDeserializer, object?> GetMemberDeserializeDelegate(Type type)
        {
            var memberDeserializeDelegate = default(Func<BinaryDeserializer, object?>);
            HashObject<Type> hashType = type;
            Monitor.Enter(memberDeserializeDelegateLock);
            try
            {
                if (!memberDeserializeDelegates.TryGetValue(hashType, out memberDeserializeDelegate))
                {
                    memberDeserializeDelegates.Add(hashType, memberDeserializeDelegate = (Func<BinaryDeserializer, object?>)getMemberDeserializeDelegate(type));
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
            DeserializeDelegate deserializeDelegate;
            if (BinaryDeserializer.DeserializeDelegates.TryGetValue(type, out deserializeDelegate)) return deserializeDelegate.MemberDelegate.notNull();
            if (typeof(ICustomSerialize).IsAssignableFrom(type) && typeof(ICustomSerialize<>).MakeGenericType(type).IsAssignableFrom(type))
            {
                return BinaryDeserializer.ICustomReflectionMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<BinaryDeserializer, object?>));
            }
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    Type elementType = type.GetElementType().notNull();
                    if (!elementType.IsAbstract && !elementType.isSerializeNotSupport())
                    {
                        if (elementType.IsValueType)
                        {
                            if (elementType.IsEnum)
                            {
                                MethodInfo method = AutoCSer.Common.EmptyAction.Method;
                                switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                                {
                                    case UnderlyingTypeEnum.Int: method = BinaryDeserializer.EnumIntArrayReflectionMethod; break;
                                    case UnderlyingTypeEnum.UInt: method = BinaryDeserializer.EnumUIntArrayReflectionMethod; break;
                                    case UnderlyingTypeEnum.Byte: method = BinaryDeserializer.EnumByteArrayReflectionMethod; break;
                                    case UnderlyingTypeEnum.ULong: method = BinaryDeserializer.EnumULongArrayReflectionMethod; break;
                                    case UnderlyingTypeEnum.UShort: method = BinaryDeserializer.EnumUShortArrayReflectionMethod; break;
                                    case UnderlyingTypeEnum.Long: method = BinaryDeserializer.EnumLongArrayReflectionMethod; break;
                                    case UnderlyingTypeEnum.Short: method = BinaryDeserializer.EnumShortArrayReflectionMethod; break;
                                    case UnderlyingTypeEnum.SByte: method = BinaryDeserializer.EnumSByteArrayReflectionMethod; break;
                                }
                                return method.MakeGenericMethod(elementType).CreateDelegate(typeof(Func<BinaryDeserializer, object?>));
                            }
                            if (elementType.isValueTypeNullable())
                            {
                                return BinaryDeserializer.NullableArrayReflectionMethod.MakeGenericMethod(elementType.GetGenericArguments()[0]).CreateDelegate(typeof(Func<BinaryDeserializer, object?>));
                            }
                            return BinaryDeserializer.StructArrayReflectionMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Func<BinaryDeserializer, object?>));
                        }
                        return BinaryDeserializer.ArrayReflectionMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Func<BinaryDeserializer, object?>));
                    }
                }
                return BinaryDeserializer.NotSupportReflectionMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<BinaryDeserializer, object?>));
            }
            if (type.IsEnum)
            {
                MethodInfo method = AutoCSer.Common.EmptyAction.Method;
                switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(type)))
                {
                    case UnderlyingTypeEnum.Int: method = BinaryDeserializer.PrimitiveMemberIntReflectionMethod; break;
                    case UnderlyingTypeEnum.UInt: method = BinaryDeserializer.PrimitiveMemberUIntReflectionMethod; break;
                    case UnderlyingTypeEnum.Byte: method = BinaryDeserializer.PrimitiveMemberByteReflectionMethod; break;
                    case UnderlyingTypeEnum.ULong: method = BinaryDeserializer.PrimitiveMemberULongReflectionMethod; break;
                    case UnderlyingTypeEnum.UShort: method = BinaryDeserializer.PrimitiveMemberUShortReflectionMethod; break;
                    case UnderlyingTypeEnum.Long: method = BinaryDeserializer.PrimitiveMemberLongReflectionMethod; break;
                    case UnderlyingTypeEnum.Short: method = BinaryDeserializer.PrimitiveMemberShortReflectionMethod; break;
                    case UnderlyingTypeEnum.SByte: method = BinaryDeserializer.PrimitiveMemberSByteReflectionMethod; break;
                }
                return method.MakeGenericMethod(type).CreateDelegate(typeof(Func<BinaryDeserializer, object>));
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(LeftArray<>))
                {
                    Type elementType = type.GetGenericArguments()[0];
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum)
                        {
                            MethodInfo method = AutoCSer.Common.EmptyAction.Method;
                            switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                            {
                                case UnderlyingTypeEnum.Int: method = BinaryDeserializer.EnumIntLeftArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.UInt: method = BinaryDeserializer.EnumUIntLeftArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.Byte: method = BinaryDeserializer.EnumByteLeftArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.ULong: method = BinaryDeserializer.EnumULongLeftArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.UShort: method = BinaryDeserializer.EnumUShortLeftArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.Long: method = BinaryDeserializer.EnumLongLeftArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.Short: method = BinaryDeserializer.EnumShortLeftArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.SByte: method = BinaryDeserializer.EnumSByteLeftArrayReflectionMethod; break;
                            }
                            return method.MakeGenericMethod(elementType).CreateDelegate(typeof(Func<BinaryDeserializer, object>));
                        }
                        if (elementType.isValueTypeNullable())
                        {
                            return BinaryDeserializer.NullableLeftArrayReflectionMethod.MakeGenericMethod(elementType.GetGenericArguments()[0]).CreateDelegate(typeof(Func<BinaryDeserializer, object>));
                        }
                        return BinaryDeserializer.StructLeftArrayReflectionMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Func<BinaryDeserializer, object>));
                    }
                    return BinaryDeserializer.LeftArrayReflectionMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Func<BinaryDeserializer, object>));
                }
                if (genericTypeDefinition == typeof(ListArray<>))
                {
                    Type elementType = type.GetGenericArguments()[0];
                    if (elementType.IsValueType)
                    {
                        if (elementType.IsEnum)
                        {
                            MethodInfo method = AutoCSer.Common.EmptyAction.Method;
                            switch (EnumGenericType.GetUnderlyingType(System.Enum.GetUnderlyingType(elementType)))
                            {
                                case UnderlyingTypeEnum.Int: method = BinaryDeserializer.EnumIntListArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.UInt: method = BinaryDeserializer.EnumUIntListArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.Byte: method = BinaryDeserializer.EnumByteListArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.ULong: method = BinaryDeserializer.EnumULongListArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.UShort: method = BinaryDeserializer.EnumUShortListArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.Long: method = BinaryDeserializer.EnumLongListArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.Short: method = BinaryDeserializer.EnumShortListArrayReflectionMethod; break;
                                case UnderlyingTypeEnum.SByte: method = BinaryDeserializer.EnumSByteListArrayReflectionMethod; break;
                            }
                            return method.MakeGenericMethod(elementType).CreateDelegate(typeof(Func<BinaryDeserializer, object?>));
                        }
                        if (elementType.isValueTypeNullable())
                        {
                            return BinaryDeserializer.NullableListArrayReflectionMethod.MakeGenericMethod(elementType.GetGenericArguments()[0]).CreateDelegate(typeof(Func<BinaryDeserializer, object?>));
                        }
                        return BinaryDeserializer.StructListArrayReflectionMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Func<BinaryDeserializer, object?>));
                    }
                    return BinaryDeserializer.ListArrayReflectionMethod.MakeGenericMethod(elementType).CreateDelegate(typeof(Func<BinaryDeserializer, object?>));
                }
            }
            if (type.IsAbstract || type.isSerializeNotSupport()) return BinaryDeserializer.NotSupportReflectionMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<BinaryDeserializer, object?>));
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    return BinaryDeserializer.NullableReflectionMethod.MakeGenericMethod(type.GetGenericArguments()[0]).CreateDelegate(typeof(Func<BinaryDeserializer, object>));
                }
            }
            if (type.IsValueType || DefaultConstructor.GetIsSerializeConstructorMethod.MakeGenericMethod(type).Invoke(null, null) != null)
            {
                var collectionType = default(Type);
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IDictionary<,>))
                    {
                        Type[] referenceTypes = interfaceType.GetGenericArguments();
                        return BinaryDeserializer.DictionaryReflectionMethod.MakeGenericMethod(type, referenceTypes[0], referenceTypes[1]).CreateDelegate(typeof(Func<BinaryDeserializer, object?>));
                    }
                    if (collectionType == null && genericTypeDefinition == typeof(ICollection<>)) collectionType = interfaceType;
                }
                if (collectionType != null)
                {
                    return BinaryDeserializer.CollectionReflectionMethod.MakeGenericMethod(type, collectionType.GetGenericArguments()[0]).CreateDelegate(typeof(Func<BinaryDeserializer, object?>));
                }
            }
            BinarySerializeAttribute attribute = BinarySerializer.DefaultAttribute;
            var baseType = GetBaseAttribute(type, ref attribute);
            if (baseType != null) return BinaryDeserializer.BaseReflectionMethod.MakeGenericMethod(type, baseType).CreateDelegate(typeof(Func<BinaryDeserializer, object?>));
            if (!object.ReferenceEquals(attribute, BinarySerializer.DefaultAttribute) && attribute.IsJsonMix)
            {
                if (type.IsValueType) return BinaryDeserializer.StructJsonReflectionMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<BinaryDeserializer, object>));
                return BinaryDeserializer.JsonReflectionMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<BinaryDeserializer, object?>));
            }
            return BinaryDeserializer.DeserializeReflectionMethod.MakeGenericMethod(type).CreateDelegate(typeof(Func<BinaryDeserializer, object?>));
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
        internal static bool GetTypeSerializeDelegate(Type type, ref GenericType? genericType, out SerializeDelegateReference serializeDelegateReference)
#else
        internal static bool GetTypeSerializeDelegate(Type type, ref GenericType genericType, out SerializeDelegateReference serializeDelegateReference)
#endif
        {
            if (BinarySerializer.SerializeDelegates.TryGetValue(type, out serializeDelegateReference)) return true;
            if (BinarySerializer.CustomConfig.GetCustomSerializeDelegate(type).Check(type, ref serializeDelegateReference)) return true;
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    Type elementType = type.GetElementType().notNull();
                    if (!elementType.IsAbstract && !elementType.isSerializeNotSupport())
                    {
                        if (elementType.IsValueType)
                        {
                            if (elementType.IsEnum) serializeDelegateReference.SetPrimitive(EnumGenericType.Get(elementType).BinarySerializeEnumArrayDelegate, genericType ?? GenericType.Get(type));
                            else if (elementType.isValueTypeNullable())
                            {
                                Type[] referenceTypes = elementType.GetGenericArguments();
                                serializeDelegateReference.SetMember(StructGenericType.Get(referenceTypes[0]).BinarySerializeNullableArrayDelegate, referenceTypes, SerializePushTypeEnum.Primitive, true);
                            }
                            else GenericType.Get(elementType).GetBinarySerializeStructArrayDelegate(ref serializeDelegateReference);
                        }
                        else ClassGenericType.Get(elementType).GetBinarySerializeArrayDelegate(ref serializeDelegateReference);
                        return true;
                    }
                }
                if (genericType == null) genericType = GenericType.Get(type);
                serializeDelegateReference.SetUnknown(type, genericType.BinarySerializeNotSupportDelegate);
                return true;
            }
            if (type.IsEnum)
            {
                EnumGenericType.Get(type).GetBinarySerializeEnumDelegate(ref serializeDelegateReference);
                return true;
            }
            if (type.IsAbstract || type.isSerializeNotSupport())
            {
                if (genericType == null) genericType = GenericType.Get(type);
                serializeDelegateReference.SetUnknown(type, genericType.BinarySerializeNotSupportDelegate);
                return true;
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    Type[] referenceTypes = type.GetGenericArguments();
                    serializeDelegateReference.SetMember(StructGenericType.Get(referenceTypes[0]).BinarySerializeNullableDelegate, referenceTypes, SerializePushTypeEnum.Primitive);
                    return true;
                }
            }
            if (BinarySerializer.CustomConfig.IsJsonSerialize(type))
            {
                if (genericType == null) genericType = GenericType.Get(type);
                if (type.IsValueType) serializeDelegateReference = new SerializeDelegateReference(genericType.BinarySerializeStructJsonDelegate);
                else serializeDelegateReference = new SerializeDelegateReference(genericType.BinarySerializeJsonDelegate, genericType);
                return true;
            }
            if ((genericType ?? GenericType.Get(type)).IsSerializeConstructor)
            {
                var collectionType = default(Type);
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IDictionary<,>))
                    {
                        DictionaryGenericType.Get(type, interfaceType).GetBinarySerializeDictionaryDelegate(ref serializeDelegateReference);
                        return true;
                    }
                    if (collectionType == null && genericTypeDefinition == typeof(ICollection<>)) collectionType = interfaceType;
                }
                if (collectionType != null)
                {
                    CollectionGenericType.Get(type, collectionType).GetBinarySerializeCollectionDelegate(ref serializeDelegateReference);
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
        internal static bool GetTypeSerializeDelegate(GenericType genericType, out SerializeDelegateReference serializeDelegateReference, out BinarySerializeAttribute attribute, out Type? baseType)
#else
        internal static bool GetTypeSerializeDelegate(GenericType genericType, out SerializeDelegateReference serializeDelegateReference, out BinarySerializeAttribute attribute, out Type baseType)
#endif
        {
            attribute = BinarySerializer.DefaultAttribute;
            Type type = genericType.CurrentType;
            var genericTypeParametere = genericType;
            if (GetTypeSerializeDelegate(type, ref genericTypeParametere, out serializeDelegateReference))
            {
                baseType = null;
                return true;
            }
            baseType = GetBaseAttribute(type, ref attribute);
            if (baseType != null)
            {
                BaseGenericType.Get(type, baseType).GetBinarySerializeBaseDelegate(ref serializeDelegateReference);
                return true;
            }
            if (!object.ReferenceEquals(attribute, BinarySerializer.DefaultAttribute) && attribute.IsJsonMix)
            {
                if (type.IsValueType) serializeDelegateReference = new SerializeDelegateReference(genericType.BinarySerializeStructJsonDelegate);
                else serializeDelegateReference = new SerializeDelegateReference(genericType.BinarySerializeJsonDelegate, genericType);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 成员序列化委托集合
        /// </summary>
        private static readonly Dictionary<HashObject<Type>, Delegate> memberSerializeDelegates = DictionaryCreator.CreateHashObject<Type, Delegate>();
        /// <summary>
        /// 获取成员序列化委托
        /// </summary>
        /// <param name="type">成员类型</param>
        internal static Delegate GetMemberSerializeDelegate(Type type)
        {
            var memberSerializeDelegate = default(Delegate);
            HashObject<Type> hashType = type;
            var baseType = default(Type);
            Monitor.Enter(memberSerializeDelegateLock);
            try
            {
                if (!memberSerializeDelegates.TryGetValue(hashType, out memberSerializeDelegate))
                {
                    memberSerializeDelegates.Add(hashType, memberSerializeDelegate = GetMemberSerializeDelegate(type, out baseType));
                }
            }
            finally { Monitor.Exit(memberSerializeDelegateLock); }
            return memberSerializeDelegate;
        }
        /// <summary>
        /// 获取成员序列化委托
        /// </summary>
        /// <param name="type">成员类型</param>
        /// <param name="baseType"></param>
#if NetStandard21
        internal static Delegate GetMemberSerializeDelegate(Type type, out Type? baseType)
#else
        internal static Delegate GetMemberSerializeDelegate(Type type, out Type baseType)
#endif
        {
            var genericType = default(GenericType);
            SerializeDelegateReference serializeDelegateReference;
            if (GetTypeSerializeDelegate(type, ref genericType, out serializeDelegateReference))
            {
                baseType = null;
                return serializeDelegateReference.Delegate.GetMemberDelegate();
            }
            BinarySerializeAttribute attribute = BinarySerializer.DefaultAttribute;
            baseType = GetBaseAttribute(type, ref attribute);
            if (baseType != null) return BaseGenericType.Get(type, baseType).BinarySerializeBaseDelegate;
            if (!object.ReferenceEquals(attribute, BinarySerializer.DefaultAttribute) && attribute.IsJsonMix)
            {
                if (genericType == null) genericType = GenericType.Get(type);
                if (type.IsValueType) return genericType.BinarySerializeMemberStructJsonDelegate;
                return genericType.BinarySerializeMemberJsonDelegate;
            }
            if (type.IsValueType && !attribute.IsMemberMap)
            {
                int memberCountVerify;
                FieldSizeArray fields = new FieldSizeArray(MemberIndexGroup.GetAnonymousFields(type, attribute.MemberFilters), attribute.GetIsJsonMember(type), out memberCountVerify);
                if (fields.IsSimpleSerialize(type, attribute.IsReferenceMember)) return StructGenericType.Get(type).BinarySerializeSimpleDelegate;
            }
            return (genericType ?? GenericType.Get(type)).BinarySerializeDelegate;
        }
        
        /// <summary>
        /// 获取类型默认反序列化委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType"></param>
        /// <param name="deserializeDelegate"></param>
        /// <returns></returns>
#if NetStandard21
        private static bool getTypeDeserializeDelegate(Type type, ref GenericType? genericType, out DeserializeDelegate deserializeDelegate)
#else
        private static bool getTypeDeserializeDelegate(Type type, ref GenericType genericType, out DeserializeDelegate deserializeDelegate)
#endif
        {
            if (BinaryDeserializer.DeserializeDelegates.TryGetValue(type, out deserializeDelegate)) return true;
            deserializeDelegate = BinarySerializer.CustomConfig.GetCustomDeserializeDelegate(type);
            if (deserializeDelegate.Check(type)) return true;
            if (type.IsArray)
            {
                if (type.GetArrayRank() == 1)
                {
                    Type elementType = type.GetElementType().notNull();
                    if (!elementType.IsAbstract && !elementType.isSerializeNotSupport())
                    {
                        if (elementType.IsValueType)
                        {
                            if (elementType.IsEnum)
                            {
                                deserializeDelegate.Set(EnumGenericType.Get(elementType).BinaryDeserializeEnumArrayDelegate, null, true);
                            }
                            else if (elementType.isValueTypeNullable())
                            {
                                deserializeDelegate.Set(StructGenericType.Get(elementType.GetGenericArguments()[0]).BinaryDeserializeNullableArrayDelegate, null, true);
                            }
                            else deserializeDelegate.Set(StructGenericType.Get(elementType).BinaryDeserializeStructArrayDelegate, null, true);
                        }
                        else deserializeDelegate.Set(ClassGenericType.Get(elementType).BinaryDeserializeArrayDelegate, null, true);
                        return true;
                    }
                }
                if (genericType == null) genericType = GenericType.Get(type);
                deserializeDelegate.Set(genericType.BinaryDeserializeNotSupportDelegate);
                return true;
            }
            if (type.IsEnum)
            {
                EnumGenericType.Get(type).GetBinaryDeserializeEnumDelegate(ref deserializeDelegate);
                return true;
            }
            if (type.IsAbstract || type.isSerializeNotSupport())
            {
                if (genericType == null) genericType = GenericType.Get(type);
                deserializeDelegate.Set(genericType.BinaryDeserializeNotSupportDelegate);
                return true;
            }
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    deserializeDelegate.Set(StructGenericType.Get(type.GetGenericArguments()[0]).BinaryDeserializeNullableDelegate, null, true);
                    return true;
                }
            }
            if (genericType == null) genericType = GenericType.Get(type);
            if (BinarySerializer.CustomConfig.IsJsonSerialize(type))
            {
                if (type.IsValueType) deserializeDelegate = new DeserializeDelegate(genericType.BinaryDeserializeStructJsonDelegate, true);
                else deserializeDelegate = new DeserializeDelegate(genericType.BinaryDeserializeJsonDelegate, true);
                return true;
            }
            if (genericType.IsSerializeConstructor)
            {
                var collectionType = default(Type);
                foreach (Type interfaceType in type.getGenericInterface())
                {
                    Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IDictionary<,>))
                    {
                        deserializeDelegate.Set(DictionaryGenericType.Get(type, interfaceType).BinaryDeserializeDictionaryDelegate, null, true);
                        return true;
                    }
                    if (collectionType == null && genericTypeDefinition == typeof(ICollection<>)) collectionType = interfaceType;
                }
                if (collectionType != null)
                {
                    deserializeDelegate.Set(CollectionGenericType.Get(type, collectionType).BinaryDeserializeCollectionDelegate, null, true);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取类型默认反序列化委托
        /// </summary>
        /// <param name="genericType"></param>
        /// <param name="deserializeDelegate"></param>
        /// <param name="attribute"></param>
        /// <param name="baseType"></param>
        /// <returns></returns>
#if NetStandard21
        internal static bool GetTypeDeserializeDelegate(GenericType genericType, out DeserializeDelegate deserializeDelegate, out BinarySerializeAttribute attribute, out Type? baseType)
#else
        internal static bool GetTypeDeserializeDelegate(GenericType genericType, out DeserializeDelegate deserializeDelegate, out BinarySerializeAttribute attribute, out Type baseType)
#endif
        {
            attribute = BinarySerializer.DefaultAttribute;
            Type type = genericType.CurrentType;
            var genericTypeNull = genericType;
            if (getTypeDeserializeDelegate(type, ref genericTypeNull, out deserializeDelegate))
            {
                baseType = null;
                return true;
            }
            baseType = GetBaseAttribute(type, ref attribute);
            if (baseType != null)
            {
                deserializeDelegate = new DeserializeDelegate(BaseGenericType.Get(type, baseType).BinaryDeserializeBaseDelegate, true);
                return true;
            }
            if (!object.ReferenceEquals(attribute, BinarySerializer.DefaultAttribute) && attribute.IsJsonMix)
            {
                if (type.IsValueType) deserializeDelegate = new DeserializeDelegate(genericType.notNull().BinaryDeserializeStructJsonDelegate, true);
                else deserializeDelegate = new DeserializeDelegate(genericType.notNull().BinaryDeserializeJsonDelegate, true);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 成员反序列化委托集合
        /// </summary>
        private static readonly Dictionary<HashObject<Type>, Delegate> memberDeserializeDelegates = DictionaryCreator.CreateHashObject<Type, Delegate>();
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
            DeserializeDelegate deserializeDelegate;
            if (getTypeDeserializeDelegate(type, ref genericType, out deserializeDelegate)) return deserializeDelegate.GetMemberDelegate();
            BinarySerializeAttribute attribute = BinarySerializer.DefaultAttribute;
            var baseType = GetBaseAttribute(type, ref attribute);
            if (baseType != null) return BaseGenericType.Get(type, baseType).BinaryDeserializeBaseDelegate;
            if (genericType == null) genericType = GenericType.Get(type);
            if (!object.ReferenceEquals(attribute, BinarySerializer.DefaultAttribute) && attribute.IsJsonMix)
            {
                if (type.IsValueType) return genericType.BinaryDeserializeStructJsonDelegate;
                return genericType.BinaryDeserializeJsonDelegate;
            }
            if (type.IsValueType && !attribute.IsMemberMap)
            {
                int memberCountVerify;
                FieldSizeArray fields = new FieldSizeArray(MemberIndexGroup.GetAnonymousFields(type, attribute.MemberFilters), attribute.GetIsJsonMember(type), out memberCountVerify);
                if (fields.IsSimpleSerialize(type, attribute.IsReferenceMember)) return StructGenericType.Get(type).BinaryDeserializeSimpleDelegate;
            }
            return genericType.BinaryDeserializeDelegate;
        }
#endif
    }
}
