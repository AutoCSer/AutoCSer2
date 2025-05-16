using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using AutoCSer.Net.CommandServer;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据序列化
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
#if AOT
    public static class TypeSerializer<T>
#else
    internal static class TypeSerializer<T>
#endif
    {
        /// <summary>
        /// 序列化委托
        /// </summary>
        internal static readonly Action<BinarySerializer, T> DefaultSerializer;
        /// <summary>
        /// 成员序列化
        /// </summary>
        private static readonly Action<BinarySerializer, T> memberSerializer;
        /// <summary>
        /// 成员位图序列化
        /// </summary>
        private static readonly Action<MemberMap<T>, BinarySerializer, T> memberMapSerializer;
        /// <summary>
        /// 序列化委托引用检查信息
        /// </summary>
        internal static SerializeDelegateReference SerializeDelegateReference;
#if !AOT
        /// <summary>
        /// JSON混合序列化位图
        /// </summary>
#if NetStandard21
        private static readonly MemberMap<T>? jsonMemberMap;
#else
        private static readonly MemberMap<T> jsonMemberMap;
#endif
        /// <summary>
        /// JSON混合序列化成员索引集合
        /// </summary>
        private static AutoCSer.Memory.Pointer jsonMemberIndexs;
        /// <summary>
        /// 固定分组字节数
        /// </summary>
        private static readonly int fixedSize;
        /// <summary>
        /// 序列化成员数量
        /// </summary>
        private static readonly int memberCountVerify;
        /// <summary>
        /// 当没有 JSON 序列化成员时是否预留 JSON 序列化标记
        /// </summary>
        private static readonly bool isJson;
#endif
        /// <summary>
        /// 是否处理成员位图
        /// </summary>
        private static readonly bool isMemberMap;

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value">数据对象</param>
        internal static void Serialize(BinarySerializer binarySerializer, ref T value)
        {
            if (SerializeDelegateReference.PushType != SerializePushTypeEnum.Primitive)
            {
                switch (binarySerializer.CheckDepthWriteNotNull(SerializeDelegateReference.PushType))
                {
                    case SerializePushTypeEnum.DepthCount:
                        DefaultSerializer(binarySerializer, value);
                        ++binarySerializer.CurrentDepth;
                        return;
                    case SerializePushTypeEnum.NotReferenceCount:
                        DefaultSerializer(binarySerializer, value);
                        binarySerializer.ClearNotReferenceCount();
                        return;
                    case SerializePushTypeEnum.TryReference:
                        if (binarySerializer.CheckPoint(value))
                        {
                            binarySerializer.Stream.Write(BinarySerializer.NotNullValue);
                            DefaultSerializer(binarySerializer, value);
                        }
                        ++binarySerializer.CurrentDepth;
                        return;
                }
            }
            else DefaultSerializer(binarySerializer, value);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value">数据对象</param>
        internal static void Serialize(BinarySerializer binarySerializer, T value)
        {
            if (SerializeDelegateReference.PushType != SerializePushTypeEnum.Primitive)
            {
                switch (binarySerializer.CheckDepthWriteNotNull(SerializeDelegateReference.PushType))
                {
                    case SerializePushTypeEnum.DepthCount:
                        DefaultSerializer(binarySerializer, value);
                        ++binarySerializer.CurrentDepth;
                        return;
                    case SerializePushTypeEnum.NotReferenceCount:
                        DefaultSerializer(binarySerializer, value);
                        binarySerializer.ClearNotReferenceCount();
                        return;
                    case SerializePushTypeEnum.TryReference:
                        if (binarySerializer.CheckPoint(value))
                        {
                            binarySerializer.Stream.Write(BinarySerializer.NotNullValue);
                            DefaultSerializer(binarySerializer, value);
                        }
                        ++binarySerializer.CurrentDepth;
                        return;
                }
            }
            else DefaultSerializer(binarySerializer, value);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value">数据对象</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SerializeNullable(BinarySerializer binarySerializer, T value)
        {
            if (SerializeDelegateReference.PushType == SerializePushTypeEnum.Primitive)
            {
                binarySerializer.Stream.Write(BinarySerializer.NotNullValue);
                DefaultSerializer(binarySerializer, value);
            }
            else
            {
                switch (binarySerializer.CheckDepthWriteNotNull(SerializeDelegateReference.PushType))
                {
                    case SerializePushTypeEnum.DepthCount:
                        DefaultSerializer(binarySerializer, value);
                        ++binarySerializer.CurrentDepth;
                        return;
                    case SerializePushTypeEnum.NotReferenceCount:
                        DefaultSerializer(binarySerializer, value);
                        binarySerializer.ClearNotReferenceCount();
                        return;
                }
            }
        }
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        private static void MemberSerialize(BinarySerializer serializer, T value)
        {
            var memberMap = isMemberMap ? serializer.SerializeMemberMap<T>() : null;
#if AOT
            if (memberMap == null) memberSerializer(serializer, value);
            else memberMapSerializer(memberMap, serializer, value);
#else
            UnmanagedStream stream = serializer.Stream;
            if (stream.PrepSize(fixedSize))
            {
                if (memberMap == null)
                {
                    stream.Data.Pointer.Write(memberCountVerify);
                    memberSerializer(serializer, value);
                    if (jsonMemberMap == null)
                    {
                        if (isJson) stream.Write(0);
                    }
                    else serializer.JsonSerialize(ref value, jsonMemberMap);
                }
                else
                {
                    memberMapSerializer(memberMap, serializer, value);
                    if (jsonMemberMap == null || (memberMap = serializer.GetJsonMemberMap(memberMap, ref jsonMemberIndexs)) == null)
                    {
                        if (isJson) stream.Write(0);
                    }
                    else serializer.JsonSerialize(ref value, memberMap);
                }
            }
#endif
        }
        /// <summary>
        /// 命令服务序列化
        /// </summary>
        /// <param name="serializer">二进制数据序列化</param>
        /// <param name="value">数据对象</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void SerializeCommandServer(BinarySerializer serializer, ref T value)
        {
#if AOT
            memberSerializer(serializer, value);
#else
            UnmanagedStream stream = serializer.Stream;
            if (stream.PrepSize(fixedSize))
            {
                stream.Data.Pointer.Write(memberCountVerify);
                memberSerializer(serializer, value);
            }
#endif
        }

        /// <summary>
        /// 成员序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void nullMember(BinarySerializer binarySerializer, T value) { }
        /// <summary>
        /// 成员位图序列化
        /// </summary>
        /// <param name="memberMap"></param>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void nullMemberMap(MemberMap<T> memberMap, BinarySerializer binarySerializer, T value) { }

        unsafe static TypeSerializer()
        {
            BinarySerializeAttribute attribute;
#if AOT
            if (Common.GetTypeSerializeDelegate<T>(out SerializeDelegateReference, out attribute))
#else
            GenericType genericType = new GenericType<T>();
            var baseType = default(Type);
            if (Common.GetTypeSerializeDelegate(genericType, out SerializeDelegateReference, out attribute, out baseType))
#endif
            {
                DefaultSerializer = (Action<BinarySerializer, T>)SerializeDelegateReference.Delegate.Delegate;
#if AOT
                SerializeDelegateReference.CheckCompleted(typeof(T));
#else
                SerializeDelegateReference.CheckCompleted(genericType);
#endif
                memberSerializer = nullMember;
                memberMapSerializer = nullMemberMap;
            }
            else
            {
                Type type = typeof(T);
#if AOT
                isMemberMap = attribute.GetIsMemberMap(type);
                if (!type.IsGenericType)
                {
                    var method = type.GetMethod(AutoCSer.CodeGenerator.BinarySerializeAttribute.BinarySerializeMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, new Type[] { typeof(BinarySerializer), type });
                    if (method != null && !method.IsGenericMethod && method.ReturnType == typeof(void))
                    {
                        var getMemberTypeMethod = type.GetMethod(AutoCSer.CodeGenerator.BinarySerializeAttribute.BinarySerializeMemberTypeMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, EmptyArray<Type>.Array);
                        if (getMemberTypeMethod != null && !getMemberTypeMethod.IsGenericMethod && getMemberTypeMethod.ReturnType == typeof(TypeInfo))
                        {
                            TypeInfo typeInfo = getMemberTypeMethod.Invoke(null, null).castValue<TypeInfo>();
                            if (typeInfo.IsSimpleSerialize)
                            {
                                SerializeDelegateReference = new SerializeDelegateReference(DefaultSerializer = (Action<BinarySerializer, T>)method.CreateDelegate(typeof(Action<BinarySerializer, T>)));
                                memberSerializer = nullMember;
                                memberMapSerializer = nullMemberMap;
                                return;
                            }
                            DefaultSerializer = MemberSerialize;
                            memberSerializer = (Action<BinarySerializer, T>)method.CreateDelegate(typeof(Action<BinarySerializer, T>));
                            if (isMemberMap)
                            {
                                var memberMapMethod = type.GetMethod(AutoCSer.CodeGenerator.BinarySerializeAttribute.BinarySerializeMemberMapMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, new Type[] { typeof(MemberMap<T>), typeof(BinarySerializer), type });
                                if (memberMapMethod != null && !memberMapMethod.IsGenericMethod && memberMapMethod.ReturnType == typeof(void))
                                {
                                    memberMapSerializer = (Action<MemberMap<T>, BinarySerializer, T>)memberMapMethod.CreateDelegate(typeof(Action<MemberMap<T>, BinarySerializer, T>));
                                }
                                else throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.BinarySerializeAttribute.BinarySerializeMemberMapMethodName);
                            }
                            else memberMapSerializer = nullMemberMap;
                            if (typeInfo.MemberTypes.Length != 0)
                            {
                                SerializeDelegateReference.SetMember(DefaultSerializer, typeInfo.MemberTypes.ToArray());
                                SerializeDelegateReference.CheckCompleted(type);
                                return;
                            }
                            if (attribute.IsReferenceMember && !type.IsValueType) SerializeDelegateReference.SetTryReference(DefaultSerializer);
                            else SerializeDelegateReference.SetNotReference(DefaultSerializer);
                            return;
                        }
                        throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.BinarySerializeAttribute.BinarySerializeMemberTypeMethodName);
                    }
                    throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.BinarySerializeAttribute.BinarySerializeMethodName);
                }
                int memberCountVerify;
                DefaultSerializer = MemberSerialize;
                FieldSizeArray fields = new FieldSizeArray(MemberIndexGroup.GetAnonymousFields(type, attribute.MemberFilters), false, out memberCountVerify);
                if (fields.FieldArray.Length == 0)
                {
                    if (attribute.IsReferenceMember && !type.IsValueType) SerializeDelegateReference.SetTryReference(DefaultSerializer);
                    else SerializeDelegateReference.SetNotReference(DefaultSerializer);
                }
                else
                {
                    if (attribute.IsReferenceMember)
                    {
                        HashSet<HashObject<System.Type>> referenceTypes = HashSetCreator.CreateHashObject<System.Type>();
                        foreach (FieldSize member in fields.FieldArray) referenceTypes.Add(member.Field.FieldType);
                        SerializeDelegateReference.SetMember(DefaultSerializer, referenceTypes.getArray(p => p.Value));
                        SerializeDelegateReference.CheckCompleted(type);
                    }
                    else SerializeDelegateReference.SetNotReference(DefaultSerializer);
                }
                if ((fields.FixedFields.Length | fields.FieldArray.Length) != 0)
                {
                    memberSerializer = (Action<BinarySerializer, T>)new SerializeMember(ref fields, memberCountVerify).Serialize<T>;
                    if (isMemberMap) memberMapSerializer = (Action<MemberMap<T>, BinarySerializer, T>)new SerializeMemberMap(ref fields).Serialize<T>;
                    else memberMapSerializer = nullMemberMap;
                }
                else
                {
                    memberSerializer = nullMember;
                    memberMapSerializer = nullMemberMap;
                }
#else
                FieldSizeArray fields = new FieldSizeArray(MemberIndexGroup.GetAnonymousFields(type, attribute.MemberFilters), isJson = attribute.GetIsJsonMember(type), out memberCountVerify);
                if (type.IsValueType && !attribute.IsMemberMap && fields.IsSimpleSerialize(type, attribute.IsReferenceMember))
                {
                    SerializeDelegateReference = new SerializeDelegateReference(StructGenericType.Get(type).BinarySerializeSimpleDelegate);
                    DefaultSerializer = (Action<BinarySerializer, T>)SerializeDelegateReference.Delegate.Delegate;
                    memberSerializer = nullMember;
                    memberMapSerializer = nullMemberMap;
                    return;
                }
                isMemberMap = attribute.GetIsMemberMap(type);
                DefaultSerializer = MemberSerialize;
                int fixedFillSize = -fields.FixedSize & 3;
                fixedSize = (fields.FixedSize + (sizeof(int) + 3)) & (int.MaxValue - 3);
                if ((fields.FixedFields.Length | fields.FieldArray.Length) != 0)
                {
                    SerializeMemberDynamicMethod dynamicMethod = new SerializeMemberDynamicMethod(type, AutoCSer.Common.NamePrefix + "BinarySerializer");
                    SerializeMemberMapDynamicMethod memberMapDynamicMethod = isMemberMap ? new SerializeMemberMapDynamicMethod(genericType, AutoCSer.Common.NamePrefix + "MemberMapBinarySerializer", (fields.AnyFixedSize & 3) != 0) : default(SerializeMemberMapDynamicMethod);
                    if (fields.FixedFields.Length != 0)
                    {
                        foreach (FieldSize member in fields.FixedFields)
                        {
                            dynamicMethod.Push(member);
                            if (isMemberMap) memberMapDynamicMethod.Push(member);
                        }
                        if (fixedFillSize != 0) dynamicMethod.FixedFillSize(fixedFillSize);
                        if (isMemberMap) memberMapDynamicMethod.SerializeFill();
                    }
                    if (fields.FieldArray.Length != 0)
                    {
                        if (attribute.IsReferenceMember)
                        {
                            HashSet<HashObject<System.Type>> referenceTypes = HashSetCreator.CreateHashObject<System.Type>();
                            foreach (FieldSize member in fields.FieldArray) referenceTypes.Add(member.Field.FieldType);
                            SerializeDelegateReference.SetMember(DefaultSerializer, referenceTypes.getArray(p => p.Value));
                            SerializeDelegateReference.CheckCompleted(genericType);
                        }
                        else SerializeDelegateReference.SetNotReference(DefaultSerializer);

                        foreach (FieldSize member in fields.FieldArray)
                        {
                            dynamicMethod.Push(member);
                            if (isMemberMap) memberMapDynamicMethod.Push(member);
                        }
                    }
                    else
                    {
                        if (attribute.IsReferenceMember && !type.IsValueType) SerializeDelegateReference.SetTryReference(DefaultSerializer);
                        else SerializeDelegateReference.SetNotReference(DefaultSerializer);
                    }
                    memberSerializer = (Action<BinarySerializer, T>)dynamicMethod.Create(typeof(Action<BinarySerializer, T>));
                    memberMapSerializer = isMemberMap ? (Action<MemberMap<T>, BinarySerializer, T>)memberMapDynamicMethod.Create(typeof(Action<MemberMap<T>, BinarySerializer, T>)) : nullMemberMap;
                }
                else
                {
                    if (attribute.IsReferenceMember && !type.IsValueType) SerializeDelegateReference.SetTryReference(DefaultSerializer);
                    else SerializeDelegateReference.SetNotReference(DefaultSerializer);
                    memberSerializer = nullMember;
                    memberMapSerializer = nullMemberMap;
                }
                if (fields.JsonFields.Length != 0)
                {
                    jsonMemberMap = new MemberMap<T>();
                    jsonMemberIndexs = AutoCSer.Memory.Unmanaged.GetStaticPointer(fields.JsonFields.Length * sizeof(int), false);
                    int index = 0;
                    int* jsonMemberIndex = jsonMemberIndexs.Int;
                    foreach (FieldIndex field in fields.JsonFields) jsonMemberMap.MemberMapData.SetMember(jsonMemberIndex[index++] = field.MemberIndex);
                }
#endif
            }
        }
    }
}
