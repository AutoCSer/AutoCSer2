﻿using AutoCSer.Extensions;
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
        /// <summary>
        /// 获取类型默认序列化委托
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType"></param>
        /// <param name="serializeDelegateReference"></param>
        /// <returns></returns>
#if NetStandard21
        private static bool getTypeSerializeDelegate(Type type, ref AutoCSer.Extensions.Metadata.GenericType? genericType, out AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
#else
        private static bool getTypeSerializeDelegate(Type type, ref AutoCSer.Extensions.Metadata.GenericType genericType, out AutoCSer.TextSerialize.DelegateReference serializeDelegateReference)
#endif
        {
            if (XmlSerializer.SerializeDelegates.TryGetValue(type, out serializeDelegateReference)) return true;
            if (XmlSerializer.CustomConfig.GetCustomSerializeDelegate(type).Check(typeof(XmlSerializer), type, ref serializeDelegateReference)) return true;
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
            }
            if (genericType == null) genericType = AutoCSer.Extensions.Metadata.GenericType.Get(type);
            if (genericType.IsSerializeConstructor)
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
        /// <returns></returns>
        internal static bool GetTypeSerializeDelegate(AutoCSer.Extensions.Metadata.GenericType genericType, out AutoCSer.TextSerialize.DelegateReference serializeDelegateReference, out XmlSerializeAttribute attribute)
        {
            attribute = XmlSerializer.AllMemberAttribute;
            Type type = genericType.CurrentType;
            var genericTypeParameter = genericType;
            if (getTypeSerializeDelegate(type, ref genericTypeParameter, out serializeDelegateReference)) return true;
            for (var baseType = type; baseType != typeof(object);)
            {
                var baseAttribute = baseType.GetCustomAttribute(typeof(XmlSerializeAttribute), false);
                if (baseAttribute != null)
                {
                    attribute = (XmlSerializeAttribute)baseAttribute;
                    if (type != baseType && attribute.IsBaseType)
                    {
                        AutoCSer.Extensions.Metadata.BaseGenericType.Get(type, baseType).GetXmlSerializeBaseDelegate(ref serializeDelegateReference);
                        return true;
                    }
                }
                if (!object.ReferenceEquals(attribute, XmlSerializer.AllMemberAttribute)) break;
                if ((baseType = baseType.BaseType) == null) break;
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
            if (getTypeSerializeDelegate(type, ref genericType, out serializeDelegateReference)) return serializeDelegateReference.Delegate.Delegate.notNull();

            return (genericType ?? AutoCSer.Extensions.Metadata.GenericType.Get(type)).XmlSerializeDelegate;
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
                reference.ReferenceGenericTypes = new AutoCSer.Metadata.GenericType[referenceTypes.Length];
                foreach (var memberType in referenceTypes)
                {
                    AutoCSer.Extensions.Metadata.GenericType genericType = AutoCSer.Extensions.Metadata.GenericType.Get(memberType.notNull());
                    AutoCSer.TextSerialize.DelegateReference serializeDelegateReference = genericType.XmlSerializeDelegateReference;
                    reference.IsUnknownMember |= serializeDelegateReference.IsUnknownMember;
                    if (!serializeDelegateReference.IsCompleted || serializeDelegateReference.IsCheckMember)
                    {
                        reference.ReferenceGenericTypes[memberIndex++] = AutoCSer.Metadata.GenericType.Get(memberType.notNull());
                    }
                }
                if (memberIndex == 0)
                {
                    reference.ReferenceGenericTypes = null;
                    reference.PushType = AutoCSer.TextSerialize.PushTypeEnum.DepthCount;
                    reference.IsCheckMember = false;
                }
                else
                {
                    if (memberIndex != reference.ReferenceGenericTypes.Length) System.Array.Resize(ref reference.ReferenceGenericTypes, memberIndex);
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
                            else if (genericType.CurrentType == type)
                            {
                                reference.PushType = AutoCSer.TextSerialize.PushTypeEnum.Push;
                                break;
                            }
                            else
                            {
                                typeArray.PrepLength(1);
                                typeArray.Array[typeArray.Length++].Set(AutoCSer.Extensions.Metadata.GenericType.Get(genericType.CurrentType).XmlSerializeDelegateReference);
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
#if NetStandard21
        internal static AutoCSer.Metadata.GenericType? Check(ref AutoCSer.TextSerialize.LoopTypeArray array, Type type, HashSet<HashObject<System.Type>> types)
#else
        internal static AutoCSer.Metadata.GenericType Check(ref AutoCSer.TextSerialize.LoopTypeArray array, Type type, HashSet<HashObject<System.Type>> types)
#endif
        {
            do
            {
            START:
                if (array.ReferenceGenericTypes != null)
                {
                    AutoCSer.Metadata.GenericType genericType = array.ReferenceGenericTypes[array.Index++];
                    Type currentType = genericType.CurrentType;
                    bool isType;
                    if (currentType.IsValueType) isType = true;
                    else
                    {
                        if (type == currentType) return genericType;
                        isType = types.Add(currentType);
                    }
                    if (isType)
                    {
                        AutoCSer.TextSerialize.DelegateReference serializeDelegateReference = AutoCSer.Extensions.Metadata.GenericType.Get(genericType.CurrentType).XmlSerializeDelegateReference;
                        if (!serializeDelegateReference.IsCompleted || serializeDelegateReference.IsCheckMember)
                        {
                            if (array.Index != array.ReferenceGenericTypes.Length) return genericType;
                            array.Set(ref serializeDelegateReference);
                            goto START;
                        }
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
                        if (type == currentType) return AutoCSer.Metadata.GenericType.Get(type);
                        isType = types.Add(currentType);
                    }
                    if (isType)
                    {
                        AutoCSer.Extensions.Metadata.GenericType genericType = AutoCSer.Extensions.Metadata.GenericType.Get(currentType);
                        AutoCSer.TextSerialize.DelegateReference serializeDelegateReference = genericType.XmlSerializeDelegateReference;
                        if (!serializeDelegateReference.IsCompleted || serializeDelegateReference.IsCheckMember)
                        {
                            if (array.Index != referenceTypes.Length) return AutoCSer.Metadata.GenericType.Get(currentType);
                            array.Set(ref serializeDelegateReference);
                            goto START;
                        }
                    }
                    if (array.Index == referenceTypes.Length) return null;
                }
            }
            while (true);
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
        /// <returns></returns>
#if NetStandard21
        internal static Delegate? GetTypeDeserializeDelegate(AutoCSer.Extensions.Metadata.GenericType genericType, out XmlSerializeAttribute attribute)
#else
        internal static Delegate GetTypeDeserializeDelegate(AutoCSer.Extensions.Metadata.GenericType genericType, out XmlSerializeAttribute attribute)
#endif
        {
            attribute = XmlDeserializer.AllMemberAttribute;
            Type type = genericType.CurrentType;
            var genericTypeParameter = genericType;
            var deserializeDelegate = getTypeDeserializeDelegate(type, ref genericTypeParameter);
            if (deserializeDelegate != null) return deserializeDelegate;
            for (var baseType = type; baseType != typeof(object);)
            {
                var baseAttribute = baseType.GetCustomAttribute(typeof(XmlSerializeAttribute), false);
                if (baseAttribute != null)
                {
                    attribute = (XmlSerializeAttribute)baseAttribute;
                    if (type != baseType && attribute.IsBaseType) return AutoCSer.Extensions.Metadata.BaseGenericType.Get(type, baseType).XmlDeserializeBaseDelegate;
                }
                if (!object.ReferenceEquals(attribute, XmlDeserializer.AllMemberAttribute)) break;
                if ((baseType = baseType.BaseType) == null) break;
            }
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
            var deserializeDelegate = GetTypeDeserializeDelegate(genericType, out attribute);
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
            generator.Emit(OpCodes.Ret);
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
            generator.Emit(OpCodes.Ret);
            return dynamicMethod;
        }
    }
}
