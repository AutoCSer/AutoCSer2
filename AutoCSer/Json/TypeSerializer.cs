using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoCSer.Extensions;
using AutoCSer.Memory;
using AutoCSer.Metadata;
using AutoCSer.Reflection;

namespace AutoCSer.Json
{
    /// <summary>
    /// Type serialization
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
#if AOT
    public unsafe static class TypeSerializer<T>
#else
    internal unsafe static class TypeSerializer<T>
#endif
    {
        /// <summary>
        /// 转换委托
        /// </summary>
        internal static readonly Action<JsonSerializer, T> DefaultSerializer;
        /// <summary>
        /// 成员转换
        /// </summary>
        private static readonly Action<JsonSerializer, T> memberSerializer;
        /// <summary>
        /// 成员转换
        /// </summary>
        private static readonly Action<MemberMap<T>, JsonSerializer, T, CharStream> memberMapSerializer;
        /// <summary>
        /// JSON 序列化委托循环引用信息
        /// </summary>
        internal static AutoCSer.TextSerialize.DelegateReference SerializeDelegateReference;

        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void SerializeNull(JsonSerializer jsonSerializer, ref T? value)
#else
        private static void SerializeNull(JsonSerializer jsonSerializer, ref T value)
#endif
        {
            if (value != null) Serialize(jsonSerializer, ref value);
            else jsonSerializer.CharStream.WriteJsonNull();
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">Data object</param>
        internal static void Serialize(JsonSerializer jsonSerializer, ref T value)
        {
            switch (jsonSerializer.Check(SerializeDelegateReference.PushType))
            {
                case AutoCSer.TextSerialize.PushTypeEnum.DepthCount:
                    DefaultSerializer(jsonSerializer, value);
                    ++jsonSerializer.CheckDepth;
                    return;
                case AutoCSer.TextSerialize.PushTypeEnum.UnknownNode:
                    int unknownCount = jsonSerializer.PushUnknownNode(value.castObject());
                    if (unknownCount != 0)
                    {
                        DefaultSerializer(jsonSerializer, value);
                        jsonSerializer.PopUnknownNode(unknownCount);
                    }
                    return;
                case AutoCSer.TextSerialize.PushTypeEnum.UnknownDepthCount:
                    DefaultSerializer(jsonSerializer, value);
                    jsonSerializer.PopUnknownDepthCount();
                    return;
                case AutoCSer.TextSerialize.PushTypeEnum.Push:
                    if (jsonSerializer.Push(value.castObject()))
                    {
                        DefaultSerializer(jsonSerializer, value);
                        jsonSerializer.Pop();
                    }
                    return;
            }
            jsonSerializer.CharStream.WriteJsonObject();
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static void SerializeNull(JsonSerializer jsonSerializer, T? value)
#else
        internal static void SerializeNull(JsonSerializer jsonSerializer, T value)
#endif
        {
            if (value != null) Serialize(jsonSerializer, value);
            else jsonSerializer.CharStream.WriteJsonNull();
        }
        /// <summary>
        /// 对象转换JSON字符串
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">Data object</param>
        internal static void Serialize(JsonSerializer jsonSerializer, T value)
        {
            switch (jsonSerializer.Check(SerializeDelegateReference.PushType))
            {
                case AutoCSer.TextSerialize.PushTypeEnum.DepthCount:
                    DefaultSerializer(jsonSerializer, value);
                    ++jsonSerializer.CheckDepth;
                    return;
                case AutoCSer.TextSerialize.PushTypeEnum.UnknownNode:
                    int unknownCount = jsonSerializer.PushUnknownNode(value.castObject());
                    if (unknownCount != 0)
                    {
                        DefaultSerializer(jsonSerializer, value);
                        jsonSerializer.PopUnknownNode(unknownCount);
                    }
                    return;
                case AutoCSer.TextSerialize.PushTypeEnum.UnknownDepthCount:
                    DefaultSerializer(jsonSerializer, value);
                    jsonSerializer.PopUnknownDepthCount();
                    return;
                case AutoCSer.TextSerialize.PushTypeEnum.Push:
                    if (jsonSerializer.Push(value.castObject()))
                    {
                        DefaultSerializer(jsonSerializer, value);
                        jsonSerializer.Pop();
                    }
                    return;
            }
            jsonSerializer.CharStream.WriteJsonObject();
        }
        /// <summary>
        /// 对象成员序列化
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">Data object</param>
        internal static void MemberSerialize(JsonSerializer jsonSerializer, T value)
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            JsonSerializeConfig config = jsonSerializer.Config;
            jsonStream.Write('{');
            var memberMap = config.MemberMap;
            if (memberMap == null) memberSerializer(jsonSerializer, value);
            else
            {
                var memberMapObject = memberMap as MemberMap<T>;
                if (memberMapObject != null)
                {
                    config.MemberMap = null;
                    try
                    {
                        memberMapSerializer(memberMapObject, jsonSerializer, value, jsonStream);
                    }
                    finally { config.MemberMap = memberMap; }
                }
                else
                {
                    jsonSerializer.Warning |= AutoCSer.TextSerialize.WarningEnum.MemberMap;
                    if (config.IsMemberMapErrorToDefault) memberSerializer(jsonSerializer, value);
                }
            }
            jsonStream.Write('}');
        }
        /// <summary>
        /// 命令服务对象成员序列化
        /// </summary>
        /// <param name="jsonSerializer">对象转换JSON字符串</param>
        /// <param name="value">Data object</param>
        internal static void SerializeCommandServer(JsonSerializer jsonSerializer, ref T value)
        {
            if (DefaultSerializer == null)
            {
                CharStream jsonStream = jsonSerializer.CharStream;
                jsonStream.Write('{');
                memberSerializer(jsonSerializer, value);
                jsonStream.Write('}');
            }
            else DefaultSerializer(jsonSerializer, value);
        }

//        /// <summary>
//        /// 数组序列化
//        /// </summary>
//        /// <param name="jsonSerializer"></param>
//        /// <param name="array">Array object</param>
//        /// <param name="count"></param>
//#if NetStandard21
//        internal static void Array(JsonSerializer jsonSerializer, T?[] array, int count)
//#else
//        internal static void Array(JsonSerializer jsonSerializer, T[] array, int count)
//#endif
//        {
//            CharStream jsonStream = jsonSerializer.CharStream;
//            for (int index = 0; index != count; ++index)
//            {
//                if (index != 0) jsonStream.Write(',');
//                SerializeNull(jsonSerializer, ref array[index]);
//            }
//        }
        /// <summary>
        /// 集合序列化
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="array"></param>
#if NetStandard21
        internal static void Collection(JsonSerializer jsonSerializer, ICollection<T?> array)
#else
        internal static void Collection(JsonSerializer jsonSerializer, ICollection<T> array)
#endif
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            bool isNext = false;
            foreach (var value in array)
            {
                if (isNext) jsonStream.Write(',');
                else isNext = true;
                SerializeNull(jsonSerializer, value);
            }
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="dictionary"></param>
        internal static void StringDictionary(JsonSerializer jsonSerializer, IDictionary<string, T> dictionary)
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            bool isNext = false;
            foreach (KeyValuePair<string, T> value in dictionary)
            {
                if (isNext) jsonStream.Write(',');
                else isNext = true;
                jsonSerializer.CharStream.WriteJsonName(value.Key);
                SerializeNull(jsonSerializer, value.Value);
            }
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="dictionary"></param>
        internal static void StringDictionaryToArray(JsonSerializer jsonSerializer, IDictionary<string, T> dictionary)
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            bool isNext = false;
            foreach (KeyValuePair<string, T> value in dictionary)
            {
                if (isNext) jsonStream.Write(',');
                else isNext = true;

                string key = value.Key.notNull();
                //jsonStream.WriteJsonKeyValueKey(21 + (key != null ? key.Length + 2 : 4));
                jsonStream.WriteJsonKeyValueKey(21 + key.Length + 2);
                jsonSerializer.JsonSerializeNull(key);
                jsonStream.WriteJsonKeyValueValue();
                SerializeNull(jsonSerializer, value.Value);
                jsonStream.Write('}');
            }
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="dictionary"></param>
#if NetStandard21
        internal static void Dictionary<KT>(JsonSerializer jsonSerializer, IDictionary<KT, T?> dictionary)
#else
        internal static void Dictionary<KT>(JsonSerializer jsonSerializer, IDictionary<KT, T> dictionary)
#endif
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            bool isNext = false;
            foreach (var value in dictionary)
            {
                if (isNext) jsonStream.Write(',');
                else isNext = true;
                TypeSerializer<KT>.SerializeNull(jsonSerializer, value.Key);
                jsonStream.Write(':');
                SerializeNull(jsonSerializer, value.Value);
            }
        }
        /// <summary>
        /// 字典序列化
        /// </summary>
        /// <typeparam name="KT"></typeparam>
        /// <param name="jsonSerializer"></param>
        /// <param name="dictionary"></param>
#if NetStandard21
        internal static void DictionaryToArray<KT>(JsonSerializer jsonSerializer, IDictionary<KT, T?> dictionary)
#else
        internal static void DictionaryToArray<KT>(JsonSerializer jsonSerializer, IDictionary<KT, T> dictionary)
#endif
        {
            CharStream jsonStream = jsonSerializer.CharStream;
            bool isNext = false;
            foreach (var value in dictionary)
            {
                if (isNext) jsonStream.Write(',');
                else isNext = true;

                jsonStream.WriteJsonKeyValueKey(21);
                TypeSerializer<KT>.SerializeNull(jsonSerializer, value.Key);
                jsonStream.WriteJsonKeyValueValue();
                SerializeNull(jsonSerializer, value.Value);
                jsonStream.Write('}');
            }
        }

        /// <summary>
        /// JSON 序列化
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void nullMember(JsonSerializer jsonSerializer, T value) { }
        /// <summary>
        /// JSON 序列化
        /// </summary>
        /// <param name="memberMap"></param>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        /// <param name="charStream"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void nullMemberMap(MemberMap<T> memberMap, JsonSerializer jsonSerializer, T value, CharStream charStream) { }
        /// <summary>
        /// 输出无成员对象
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void noMemberValue(JsonSerializer jsonSerializer, T value)
        {
            jsonSerializer.CharStream.WriteJsonObject();
        }
        /// <summary>
        /// 输出无成员对象
        /// </summary>
        /// <param name="jsonSerializer"></param>
        /// <param name="value"></param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void noMember(JsonSerializer jsonSerializer, T value)
        {
            if (value == null) jsonSerializer.CharStream.WriteJsonNull();
            else jsonSerializer.CharStream.WriteJsonObject();
        }

        static TypeSerializer()
        {
            Type type = typeof(T);
            JsonSerializeAttribute attribute;
#if AOT
            if (Common.GetTypeSerializeDelegate<T>(out SerializeDelegateReference, out attribute))
#else
            var baseType = typeof(T);
            GenericType<T> genericType = new GenericType<T>();
            if (Common.GetTypeSerializeDelegate(genericType, out SerializeDelegateReference, out attribute, out baseType))
#endif
            {
                DefaultSerializer = (Action<JsonSerializer, T>)SerializeDelegateReference.Delegate.GetRemoveDelegate();
                Common.CheckCompleted(type, ref SerializeDelegateReference);
            }
            else
            {
#if AOT
                DefaultSerializer = MemberSerialize;
                var method = type.GetMethod(AutoCSer.CodeGenerator.JsonSerializeAttribute.JsonSerializeMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, new Type[] { typeof(JsonSerializer), type });
                if (method != null && !method.IsGenericMethod && method.ReturnType == typeof(void))
                {
                    var memberMapMethod = type.GetMethod(AutoCSer.CodeGenerator.JsonSerializeAttribute.JsonSerializeMemberMapMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, new Type[] { typeof(MemberMap<T>), typeof(JsonSerializer), type, typeof(CharStream) });
                    if (memberMapMethod != null && !memberMapMethod.IsGenericMethod && memberMapMethod.ReturnType == typeof(void))
                    {
                        memberSerializer = (Action<JsonSerializer, T>)method.CreateDelegate(typeof(Action<JsonSerializer, T>));
                        memberMapSerializer = (Action<MemberMap<T>, JsonSerializer, T, CharStream>)memberMapMethod.CreateDelegate(typeof(Action<MemberMap<T>, JsonSerializer, T, CharStream>));
                        if (attribute.CheckLoopReference)
                        {
                            var memberTypeMethod = type.GetMethod(AutoCSer.CodeGenerator.JsonSerializeAttribute.JsonSerializeMemberTypeMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, EmptyArray<Type>.Array);
                            if (memberTypeMethod != null && !memberTypeMethod.IsGenericMethod && memberTypeMethod.ReturnType == typeof(AutoCSer.LeftArray<Type>))
                            {
                                SerializeDelegateReference.SetMember(DefaultSerializer, memberTypeMethod.Invoke(null, null).castValue<AutoCSer.LeftArray<Type>>().ToArray());
                                Common.CheckCompleted(type, ref SerializeDelegateReference);
                                return;
                            }
                        }
                        SerializeDelegateReference.SetNoLoop(DefaultSerializer);
                        return;
                    }
                    throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.JsonSerializeAttribute.JsonSerializeMemberMapMethodName);
                }
                throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.JsonSerializeAttribute.JsonSerializeMethodName);
#else
                var fields = AutoCSer.TextSerialize.Common.GetSerializeFields<JsonSerializeMemberAttribute>(MemberIndexGroup.GetFields(baseType ?? type, attribute.MemberFilters), attribute);
                LeftArray<AutoCSer.TextSerialize.PropertyMethod<JsonSerializeMemberAttribute>> properties = AutoCSer.TextSerialize.Common.GetSerializeProperties<JsonSerializeMemberAttribute>(MemberIndexGroup.GetProperties(baseType ?? type, attribute.MemberFilters), attribute);
                if ((fields.Length | properties.Length) != 0)
                {
                    DefaultSerializer = MemberSerialize;
                    if (attribute.CheckLoopReference)
                    {
                        HashSet<HashObject<System.Type>> referenceTypes = HashSetCreator.CreateHashObject<System.Type>();
                        foreach (var member in fields) referenceTypes.Add(member.Key.Member.FieldType);
                        foreach (AutoCSer.TextSerialize.PropertyMethod<JsonSerializeMemberAttribute> member in properties) referenceTypes.Add(member.Property.Member.PropertyType);
                        SerializeDelegateReference.SetMember(DefaultSerializer, referenceTypes.getArray(p => p.Value));
                        Common.CheckCompleted(type, ref SerializeDelegateReference);
                    }
                    else SerializeDelegateReference.SetNoLoop(DefaultSerializer);
                    SerializeMemberDynamicMethod dynamicMethod = new SerializeMemberDynamicMethod(type);
                    SerializeMemberMapDynamicMethod memberMapDynamicMethod = new SerializeMemberMapDynamicMethod(genericType);
                    foreach (var member in fields)
                    {
                        MethodInfo serializeMethod = Common.GetMemberSerializeDelegate(member.Key.Member.FieldType).Method;
                        dynamicMethod.Push(member.Key, serializeMethod);
                        memberMapDynamicMethod.Push(member.Key, serializeMethod);
                    }
                    foreach (AutoCSer.TextSerialize.PropertyMethod<JsonSerializeMemberAttribute> member in properties)
                    {
                        MethodInfo serializeMethod = Common.GetMemberSerializeDelegate(member.Property.Member.PropertyType).Method;
                        if (member.Method == null)
                        {
                            dynamicMethod.Push(member.Property.AnonymousField.notNull(), serializeMethod);
                            memberMapDynamicMethod.Push(member.Property.AnonymousField.notNull(), serializeMethod);
                        }
                        else
                        {
                            dynamicMethod.Push(member.Property, member.Method, serializeMethod);
                            memberMapDynamicMethod.Push(member.Property, member.Method, serializeMethod);
                        }
                    }
                    memberSerializer = (Action<JsonSerializer, T>)dynamicMethod.Create(typeof(Action<JsonSerializer, T>));
                    memberMapSerializer = (Action<MemberMap<T>, JsonSerializer, T, CharStream>)memberMapDynamicMethod.Create(typeof(Action<MemberMap<T>, JsonSerializer, T, CharStream>));
                    return;
                }
                else SerializeDelegateReference.SetNoLoop(DefaultSerializer = type.IsValueType ? (Action<JsonSerializer, T>)noMemberValue : (Action<JsonSerializer, T>)noMember);
#endif
            }
            memberSerializer = nullMember;
            memberMapSerializer = nullMemberMap;
        }
    }
}
