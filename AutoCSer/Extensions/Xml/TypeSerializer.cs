using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 类型序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    internal unsafe static class TypeSerializer<T>
    {
        /// <summary>
        /// 转换委托
        /// </summary>
        internal static readonly Action<XmlSerializer, T> DefaultSerializer;
        /// <summary>
        /// 成员转换
        /// </summary>
        private static readonly Action<XmlSerializer, T> memberSerializer;
        /// <summary>
        /// 成员转换
        /// </summary>
        private static readonly Action<MemberMap<T>, XmlSerializer, T, CharStream> memberMapSerializer;
        /// <summary>
        /// XML 序列化委托循环引用信息
        /// </summary>
        internal static AutoCSer.TextSerialize.DelegateReference SerializeDelegateReference;
        /// <summary>
        /// 空节点输出字符串
        /// </summary>
        private static readonly string emptyString;

        /// <summary>
        /// 对象转换XML字符串
        /// </summary>
        /// <param name="serializer">对象转换XML字符串</param>
        /// <param name="value">数据对象</param>
        internal static void Serialize(XmlSerializer serializer, ref T value)
        {
            switch (serializer.Check(SerializeDelegateReference.PushType))
            {
                case AutoCSer.TextSerialize.PushTypeEnum.DepthCount:
                    DefaultSerializer(serializer, value);
                    ++serializer.CheckDepth;
                    return;
                case AutoCSer.TextSerialize.PushTypeEnum.UnknownNode:
                    int unknownCount = serializer.PushUnknownNode(value);
                    if (unknownCount != 0)
                    {
                        DefaultSerializer(serializer, value);
                        serializer.PopUnknownNode(unknownCount);
                    }
                    return;
                case AutoCSer.TextSerialize.PushTypeEnum.UnknownDepthCount:
                    DefaultSerializer(serializer, value);
                    serializer.PopUnknownDepthCount();
                    return;
                case AutoCSer.TextSerialize.PushTypeEnum.Push:
                    if (serializer.Push(value))
                    {
                        DefaultSerializer(serializer, value);
                        serializer.Pop();
                    }
                    return;
            }
        }
        /// <summary>
        /// 对象转换XML字符串
        /// </summary>
        /// <param name="serializer">对象转换XML字符串</param>
        /// <param name="value">数据对象</param>
        internal static void Serialize(XmlSerializer serializer, T value)
        {
            switch (serializer.Check(SerializeDelegateReference.PushType))
            {
                case AutoCSer.TextSerialize.PushTypeEnum.DepthCount:
                    DefaultSerializer(serializer, value);
                    ++serializer.CheckDepth;
                    return;
                case AutoCSer.TextSerialize.PushTypeEnum.UnknownNode:
                    int unknownCount = serializer.PushUnknownNode(value);
                    if (unknownCount != 0)
                    {
                        DefaultSerializer(serializer, value);
                        serializer.PopUnknownNode(unknownCount);
                    }
                    return;
                case AutoCSer.TextSerialize.PushTypeEnum.UnknownDepthCount:
                    DefaultSerializer(serializer, value);
                    serializer.PopUnknownDepthCount();
                    return;
                case AutoCSer.TextSerialize.PushTypeEnum.Push:
                    if (serializer.Push(value))
                    {
                        DefaultSerializer(serializer, value);
                        serializer.Pop();
                    }
                    return;
            }
        }
        /// <summary>
        /// 对象成员序列化
        /// </summary>
        /// <param name="serializer">对象转换XML字符串</param>
        /// <param name="value">数据对象</param>
        internal static void MemberSerialize(XmlSerializer serializer, T value)
        {
            CharStream charStream = serializer.CharStream;
            XmlSerializeConfig config = serializer.Config;
            MemberMap memberMap = config.MemberMap;
            int streamIndex = charStream.Data.Pointer.CurrentIndex;
            if (memberMap == null) memberSerializer(serializer, value);
            else
            {
                MemberMap<T> memberMapObject = memberMap as MemberMap<T>;
                if (memberMapObject != null)
                {
                    config.MemberMap = null;
                    try
                    {
                        memberMapSerializer(memberMapObject, serializer, value, charStream);
                    }
                    finally { config.MemberMap = memberMap; }
                }
                else
                {
                    serializer.Warning |= AutoCSer.TextSerialize.WarningEnum.MemberMap;
                    if (config.IsMemberMapErrorToDefault) memberSerializer(serializer, value);
                }
            }
            if (streamIndex == charStream.Data.Pointer.CurrentIndex && !typeof(T).IsValueType) WriteEmptyString(charStream);
        }

        /// <summary>
        /// 写入空节点输出字符串
        /// </summary>
        /// <param name="charStream"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static void WriteEmptyString(CharStream charStream)
        {
            if(emptyString.Length != 0) charStream.SimpleWrite(emptyString);
        }
        /// <summary>
        /// XML 序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void emptyMember(XmlSerializer serializer, T value)
        {
            serializer.CharStream.SimpleWrite(emptyString);
        }
        /// <summary>
        /// XML 序列化
        /// </summary>
        /// <param name="memberMap"></param>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        /// <param name="charStream"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void emptyMemberMap(MemberMap<T> memberMap, XmlSerializer serializer, T value, CharStream charStream)
        {
            charStream.SimpleWrite(emptyString);
        }
        /// <summary>
        /// XML 序列化
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void nullMember(XmlSerializer serializer, T value) { }
        /// <summary>
        /// XML 序列化
        /// </summary>
        /// <param name="memberMap"></param>
        /// <param name="serializer"></param>
        /// <param name="value"></param>
        /// <param name="charStream"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void nullMemberMap(MemberMap<T> memberMap, XmlSerializer serializer, T value, CharStream charStream) { }

        static TypeSerializer()
        {
            Type type = typeof(T);
            AutoCSer.Extensions.Metadata.GenericType<T> genericType = new AutoCSer.Extensions.Metadata.GenericType<T>();
            XmlSerializeAttribute attribute;
            if (Common.GetTypeSerializeDelegate(genericType, out SerializeDelegateReference, out attribute))
            {
                DefaultSerializer = (Action<XmlSerializer, T>)SerializeDelegateReference.Delegate.GetRemoveDelegate();
                Common.CheckCompleted(type, ref SerializeDelegateReference);
                memberSerializer = nullMember;
                memberMapSerializer = nullMemberMap;
                emptyString = string.Empty;
            }
            else
            {
                if (object.ReferenceEquals(attribute, XmlSerializer.AllMemberAttribute) && type.Name[0] == '<') attribute = XmlSerializeAttribute.AnonymousTypeMember;
                emptyString = @"<" + type.Name + @"></" + type.Name + @">";
                LeftArray<KeyValue<FieldIndex, XmlSerializeMemberAttribute>> fields = AutoCSer.TextSerialize.Common.GetSerializeFields<XmlSerializeMemberAttribute>(MemberIndexGroup<T>.GetFields(attribute.MemberFilters), attribute);
                LeftArray<AutoCSer.TextSerialize.PropertyMethod<XmlSerializeMemberAttribute>> properties = AutoCSer.TextSerialize.Common.GetSerializeProperties<XmlSerializeMemberAttribute>(MemberIndexGroup<T>.GetProperties(attribute.MemberFilters), attribute);
                if ((fields.Length | properties.Length) != 0)
                {
                    DefaultSerializer = MemberSerialize;
                    if (attribute.CheckLoopReference)
                    {
                        HashSet<HashObject<System.Type>> referenceTypes = HashSetCreator.CreateHashObject<System.Type>();
                        foreach (KeyValue<FieldIndex, XmlSerializeMemberAttribute> member in fields) referenceTypes.Add(member.Key.Member.FieldType);
                        foreach (AutoCSer.TextSerialize.PropertyMethod<XmlSerializeMemberAttribute> member in properties) referenceTypes.Add(member.Property.Member.PropertyType);
                        SerializeDelegateReference.SetMember(DefaultSerializer, referenceTypes.getArray(p => p.Value));
                        Common.CheckCompleted(type, ref SerializeDelegateReference);
                    }
                    else SerializeDelegateReference.SetNoLoop(DefaultSerializer);

                    SerializeMemberDynamicMethod dynamicMethod = new SerializeMemberDynamicMethod(type);
                    SerializeMemberMapDynamicMethod memberMapDynamicMethod = new SerializeMemberMapDynamicMethod(new AutoCSer.Metadata.GenericType<T>());
                    foreach (KeyValue<FieldIndex, XmlSerializeMemberAttribute> member in fields)
                    {
                        MethodInfo serializeMethod = Common.GetMemberSerializeDelegate(member.Key.Member.FieldType).Method;
                        dynamicMethod.Push(member.Key, serializeMethod, member.Value);
                        memberMapDynamicMethod.Push(member.Key, serializeMethod, member.Value);
                    }
                    foreach (AutoCSer.TextSerialize.PropertyMethod<XmlSerializeMemberAttribute> member in properties)
                    {
                        MethodInfo serializeMethod = Common.GetMemberSerializeDelegate(member.Property.Member.PropertyType).Method;
                        dynamicMethod.Push(member.Property, member.Method, serializeMethod, member.MemberAttribute);
                        memberMapDynamicMethod.Push(member.Property, member.Method, serializeMethod, member.MemberAttribute);
                    }
                    memberSerializer = (Action<XmlSerializer, T>)dynamicMethod.Create(typeof(Action<XmlSerializer, T>));
                    memberMapSerializer = (Action<MemberMap<T>, XmlSerializer, T, CharStream>)memberMapDynamicMethod.Create(typeof(Action<MemberMap<T>, XmlSerializer, T, CharStream>));
                }
                else
                {
                    if (type.IsValueType)
                    {
                        memberSerializer = DefaultSerializer = nullMember;
                        memberMapSerializer = nullMemberMap;
                    }
                    else
                    {
                        memberSerializer = DefaultSerializer = emptyMember;
                        memberMapSerializer = emptyMemberMap;
                    }
                    SerializeDelegateReference.SetNoLoop(DefaultSerializer);
                }
            }
        }
    }
}
