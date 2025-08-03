using AutoCSer.Extensions;
using AutoCSer.Metadata;
using AutoCSer.Net.CommandServer;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.BinarySerialize
{
    /// <summary>
    /// 二进制数据反序列化
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    internal unsafe static class TypeDeserializer<T>
    {
        /// <summary>
        /// 二进制数据反序列化委托
        /// </summary>
        /// <param name="memberMap">成员位图</param>
        /// <param name="deserializer">二进制数据反序列化</param>
        /// <param name="value">Target data</param>
        private delegate void memberMapDeserialize(MemberMap<T> memberMap, BinaryDeserializer deserializer, ref T value);

        /// <summary>
        /// 成员序列化
        /// </summary>
        private static readonly BinaryDeserializer.DeserializeDelegate<T> memberDeserializer;
        /// <summary>
        /// 成员位图序列化
        /// </summary>
        private static readonly memberMapDeserialize memberMapDeserializer;
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
        private static readonly int[] jsonMemberIndexs;
        /// <summary>
        /// 当没有 JSON 序列化成员时是否预留 JSON 序列化标记
        /// </summary>
        private static readonly bool isJson;
#endif
        /// <summary>
        /// 序列化成员数量
        /// </summary>
        private static readonly int memberCountVerify;

        /// <summary>
        /// 成员反序列化
        /// </summary>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void nullMember(BinaryDeserializer binarySerializer, ref T value) { }
        /// <summary>
        /// 成员位图反序列化
        /// </summary>
        /// <param name="memberMap"></param>
        /// <param name="binarySerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void nullMemberMap(MemberMap<T> memberMap, BinaryDeserializer binarySerializer, ref T value) { }

        /// <summary>
        /// 二进制数据序列化类型配置
        /// </summary>
        private static readonly BinarySerializeAttribute attribute;
        /// <summary>
        /// 反序列化委托
        /// </summary>
#if NetStandard21
        internal static readonly BinaryDeserializer.DeserializeDelegate<T?> DefaultDeserializer;
#else
        internal static readonly BinaryDeserializer.DeserializeDelegate<T> DefaultDeserializer;
#endif
        /// <summary>
        /// 引用执行类型
        /// </summary>
        private static readonly SerializePushTypeEnum pushType;
        /// <summary>
        /// 是否处理成员位图
        /// </summary>
        private static readonly bool isMemberMap;
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
#if NetStandard21
        internal static void Deserialize(BinaryDeserializer deserializer, ref T? value)
#else
        internal static void Deserialize(BinaryDeserializer deserializer, ref T value)
#endif
        {
            switch (pushType)
            {
                case SerializePushTypeEnum.Primitive: DefaultDeserializer(deserializer, ref value); return;
                case SerializePushTypeEnum.DepthCount:
                    if (deserializer.CheckNotNull(ref value)) DefaultDeserializer(deserializer, ref value);
                    return;
                case SerializePushTypeEnum.TryReference:
                    bool isRealType;
                    if (deserializer.CheckTryPush(ref value, out isRealType)) DefaultDeserializer(deserializer, ref value);
                    else if(isRealType) deserializer.RealType(ref value);
                    return;
            }
        }
        /// <summary>
        /// Object serialization
        /// </summary>
        /// <param name="deserializer">二进制数据反序列化</param>
        /// <param name="value">Data object</param>
        internal static void MemberDeserialize(BinaryDeserializer deserializer, ref T value)
        {
            if (deserializer.CheckMemberCount(memberCountVerify))
            {
                memberDeserializer(deserializer, ref value);
#if !AOT
                if (isJson || jsonMemberMap != null) deserializer.DeserializeJsonNotNullCheckZore(ref value);
#endif
            }
            else if (isMemberMap)
            {
                var memberMap = deserializer.GetMemberMap<T>();
                if (memberMap != null)
                {
                    memberMapDeserializer(memberMap, deserializer, ref value);
#if !AOT
                    if (isJson) deserializer.DeserializeJsonNotNullCheckZore(ref value);
                    else if (jsonMemberMap != null)
                    {
                        foreach (int memberIndex in jsonMemberIndexs)
                        {
                            if (memberMap.MemberMapData.IsMember(memberIndex))
                            {
                                deserializer.DeserializeJsonNotNullCheckZore(ref value);
                                return;
                            }
                        }
                    }
#endif
                    if (object.ReferenceEquals(memberMap, MemberMap<T>.Default)) deserializer.MemberMap = null;
                }
            }
            else deserializer.State = DeserializeStateEnum.MemberMap;
        }

#pragma warning disable CS8618
        static TypeDeserializer()
#pragma warning restore CS8618
        {
            DeserializeDelegate deserializeDelegate;
#if AOT
            if (Common.GetTypeDeserializeDelegate<T>(out deserializeDelegate, out attribute))
#else
            GenericType genericType = new GenericType<T>();
            var baseType = default(Type);
            if (Common.GetTypeDeserializeDelegate(genericType, out deserializeDelegate, out attribute, out baseType))
#endif
            {
#pragma warning disable CS8619
                DefaultDeserializer = (BinaryDeserializer.DeserializeDelegate<T>)deserializeDelegate.Delegate;
#pragma warning restore CS8619
                pushType = deserializeDelegate.IsPrimitive ? SerializePushTypeEnum.Primitive : (typeof(T).IsValueType ? SerializePushTypeEnum.DepthCount : SerializePushTypeEnum.TryReference);
            }
            else
            {
                Type type = typeof(T);
#if AOT
                isMemberMap = attribute.GetIsMemberMap(type);
#pragma warning disable CS8622
                DefaultDeserializer = MemberDeserialize;
#pragma warning restore CS8622
                if (!type.IsGenericType)
                {
                    Type refType = type.MakeByRefType();
                    var method = type.GetMethod(AutoCSer.CodeGenerator.BinarySerializeAttribute.BinaryDeserializeMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, new Type[] { typeof(BinaryDeserializer), refType });
                    if (method != null && !method.IsGenericMethod && method.ReturnType == typeof(void))
                    {
                        var getMemberTypeMethod = type.GetMethod(AutoCSer.CodeGenerator.BinarySerializeAttribute.BinarySerializeMemberTypeMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, EmptyArray<Type>.Array);
                        if (getMemberTypeMethod != null && !getMemberTypeMethod.IsGenericMethod && getMemberTypeMethod.ReturnType == typeof(TypeInfo))
                        {
                            memberDeserializer = (BinaryDeserializer.DeserializeDelegate<T>)method.CreateDelegate(typeof(BinaryDeserializer.DeserializeDelegate<T>));
                            memberCountVerify = getMemberTypeMethod.Invoke(null, null).castValue<TypeInfo>().MemberCountVerify;
                            if (isMemberMap)
                            {
                                var memberMapMethod = type.GetMethod(AutoCSer.CodeGenerator.BinarySerializeAttribute.BinaryDeserializeMemberMapMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, new Type[] { typeof(MemberMap<T>), typeof(BinaryDeserializer), refType });
                                if (memberMapMethod != null && !memberMapMethod.IsGenericMethod && memberMapMethod.ReturnType == typeof(void))
                                {
                                    memberMapDeserializer = (memberMapDeserialize)memberMapMethod.CreateDelegate(typeof(memberMapDeserialize));
                                }
                                else throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.BinarySerializeAttribute.BinaryDeserializeMemberMapMethodName);
                            }
                            else memberMapDeserializer = nullMemberMap;
                            pushType = (type.IsValueType ? SerializePushTypeEnum.DepthCount : SerializePushTypeEnum.TryReference);
                            return;
                        }
                        throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.BinarySerializeAttribute.BinarySerializeMemberTypeMethodName);
                    }
                    throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.BinarySerializeAttribute.BinaryDeserializeMethodName);
                }
                FieldSizeArray fields = new FieldSizeArray(MemberIndexGroup.GetAnonymousFields(type, attribute.MemberFilters), false, out memberCountVerify);
                if ((fields.FixedFields.Length | fields.FieldArray.Length) != 0)
                {
                    memberDeserializer = (BinaryDeserializer.DeserializeDelegate<T>)new DeserializeMember(ref fields).Deserialize<T>;
                    if (isMemberMap) memberMapDeserializer = (memberMapDeserialize)new DeserializeMemberMap(ref fields).Deserialize<T>;
                    else memberMapDeserializer = nullMemberMap;
                }
                else
                {
                    memberDeserializer = nullMember;
                    memberMapDeserializer = nullMemberMap;
                }
#else
                FieldSizeArray fields = new FieldSizeArray(MemberIndexGroup.GetAnonymousFields(type, attribute.MemberFilters), isJson = attribute.GetIsJsonMember(type), out memberCountVerify);
                if (type.IsValueType && !attribute.IsMemberMap && fields.IsSimpleSerialize(type, attribute.IsReferenceMember))
                {
#pragma warning disable CS8619
                    DefaultDeserializer = (BinaryDeserializer.DeserializeDelegate<T>)StructGenericType.Get(type).BinaryDeserializeSimpleDelegate;
#pragma warning restore CS8619
                    pushType = SerializePushTypeEnum.Primitive;
                    return;
                }
                isMemberMap = attribute.GetIsMemberMap(type);
#pragma warning disable CS8622
                DefaultDeserializer = MemberDeserialize;
#pragma warning restore CS8622
                int fixedFillSize = -fields.FixedSize & 3;
                if ((fields.FixedFields.Length | fields.FieldArray.Length) != 0)
                {
                    DeserializeMemberDynamicMethod dynamicMethod = new DeserializeMemberDynamicMethod(type, AutoCSer.Common.NamePrefix + "BinaryDeserializer");
                    DeserializeMemberMapDynamicMethod memberMapDynamicMethod = isMemberMap ? new DeserializeMemberMapDynamicMethod(genericType, AutoCSer.Common.NamePrefix + "MemberMapBinaryDeserializer", (fields.AnyFixedSize & 3) != 0) : default(DeserializeMemberMapDynamicMethod);
                    foreach (FieldSize member in fields.FixedFields)
                    {
                        dynamicMethod.Push(member);
                        if (isMemberMap) memberMapDynamicMethod.Push(member);
                    }
                    if (fixedFillSize != 0) dynamicMethod.FixedFillSize(fixedFillSize);
                    if (isMemberMap) memberMapDynamicMethod.SetFixedCurrentEnd();
                    foreach (FieldSize member in fields.FieldArray)
                    {
                        dynamicMethod.Push(member);
                        if (isMemberMap) memberMapDynamicMethod.Push(member);
                    }
                    memberDeserializer = (BinaryDeserializer.DeserializeDelegate<T>)dynamicMethod.Create(typeof(BinaryDeserializer.DeserializeDelegate<T>));
                    if (isMemberMap) memberMapDeserializer = (memberMapDeserialize)memberMapDynamicMethod.Create(typeof(memberMapDeserialize));
                    else memberMapDeserializer = nullMemberMap;
                }
                else
                {
                    memberDeserializer = nullMember;
                    memberMapDeserializer = nullMemberMap;
                }
                if (fields.JsonFields.Length != 0)
                {
                    jsonMemberMap = new MemberMap<T>();
                    jsonMemberIndexs = AutoCSer.Common.GetUninitializedArray<int>(fields.JsonFields.Length);
                    int index = 0;
                    foreach (Metadata.FieldIndex field in fields.JsonFields) jsonMemberMap.MemberMapData.SetMember(jsonMemberIndexs[index++] = field.MemberIndex);
                }
                else jsonMemberIndexs = EmptyArray<int>.Array;
#endif
                pushType = (type.IsValueType ? SerializePushTypeEnum.DepthCount : SerializePushTypeEnum.TryReference);
            }
        }
    }
}
