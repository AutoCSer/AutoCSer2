using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer.Json
{
    /// <summary>
    /// 类型反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    internal unsafe static class TypeDeserializer<T>
    {
#if AOT
        /// <summary>
        /// 成员解析
        /// </summary>
        private static readonly JsonDeserializer.MemberDeserializeDelegate<T> memberIndexDeserializer;
        /// <summary>
        /// 成员索引集合
        /// </summary>
        private static int[] memberIndexs;
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        /// <param name="memberIndex"></param>
        private static void nullMemberIndex(JsonDeserializer jsonDeserializer, ref T value, int memberIndex) { }
#else
        /// <summary>
        /// 成员解析器过滤
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct TryDeserializeFilter
        {
            /// <summary>
            /// 成员解析器
            /// </summary>
            internal JsonDeserializer.DeserializeDelegate<T> Deserialize;
            /// <summary>
            /// 成员位图索引
            /// </summary>
            private int memberMapIndex;
            /// <summary>
            /// Set the data
            /// 设置数据
            /// </summary>
            /// <param name="method"></param>
            /// <param name="member"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Set(MethodInfo method, MemberIndexInfo member)
            {
                Deserialize = (JsonDeserializer.DeserializeDelegate<T>)method.CreateDelegate(typeof(JsonDeserializer.DeserializeDelegate<T>));
                memberMapIndex = member.MemberIndex;
            }
            /// <summary>
            /// 成员解析器
            /// </summary>
            /// <param name="jsonDeserializer">JSON 反序列化</param>
            /// <param name="value">Target data</param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Call(JsonDeserializer jsonDeserializer, ref T value)
            {
                Deserialize(jsonDeserializer, ref value);
            }
            /// <summary>
            /// 成员解析器
            /// </summary>
            /// <param name="jsonDeserializer">JSON 反序列化</param>
            /// <param name="memberMap">成员位图</param>
            /// <param name="value">Target data</param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Call(JsonDeserializer jsonDeserializer, MemberMap<T> memberMap, ref T value)
            {
                Deserialize(jsonDeserializer, ref value);
                memberMap.MemberMapData.SetMember(memberMapIndex);
            }
        }
        /// <summary>
        /// 成员解析器集合
        /// </summary>
        private static readonly TryDeserializeFilter[] memberDeserializers;
#endif
        /// <summary>
        /// 默认名称解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        /// <param name="names"></param>
        private delegate void DeserializeMember(JsonDeserializer jsonDeserializer, ref T value, ref AutoCSer.Memory.Pointer names);
        /// <summary>
        /// 默认名称解析
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        /// <param name="names"></param>
        /// <param name="memberMap"></param>
        private delegate void DeserializeMemberMap(JsonDeserializer jsonDeserializer, ref T value, ref AutoCSer.Memory.Pointer names, MemberMap<T> memberMap);
        /// <summary>
        /// 解析委托
        /// </summary>
#if NetStandard21
        internal static readonly JsonDeserializer.DeserializeDelegate<T?> DefaultDeserializer;
#else
        internal static readonly JsonDeserializer.DeserializeDelegate<T> DefaultDeserializer;
#endif
        /// <summary>
        /// 默认名称解析
        /// </summary>
        private static readonly DeserializeMember memberDeserializer;
        /// <summary>
        /// 默认名称解析
        /// </summary>
        private static readonly DeserializeMemberMap memberMapDeserializer;
        /// <summary>
        /// 成员名称查找数据
        /// </summary>
        private static StateSearcher memberSearcher;
        /// <summary>
        /// 默认顺序成员名称数据
        /// </summary>
        private static AutoCSer.Memory.Pointer memberNames;

        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">Target data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void deserializeValue(JsonDeserializer jsonDeserializer, ref T? value)
#else
        private static void deserializeValue(JsonDeserializer jsonDeserializer, ref T value)
#endif
        {
#pragma warning disable CS8601
            if (jsonDeserializer.SearchObject()) DeserializeMembers(jsonDeserializer, ref value);
#pragma warning restore CS8601
            else value = default(T);
        }
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">Target data</param>
#if NetStandard21
        private static void deserializeClass(JsonDeserializer jsonDeserializer, ref T? value)
#else
        private static void deserializeClass(JsonDeserializer jsonDeserializer, ref T value)
#endif
        {
            if (jsonDeserializer.SearchObject())
            {
                if (value == null)
                {
                    if (AutoCSer.Metadata.DefaultConstructor<T>.Type != Metadata.DefaultConstructorTypeEnum.None)
                    {
                        if (!jsonDeserializer.Constructor(out value)) return;
                    }
                    else if (!AutoCSer.JsonSerializer.CustomConfig.CallCustomConstructor(out value))
                    {
                        jsonDeserializer.NoConstructorIgnoreObject();
                        return;
                    }
                }
#pragma warning disable CS8601
                DeserializeMembers(jsonDeserializer, ref value);
#pragma warning restore CS8601
            }
            else value = default(T);
        }
        /// <summary>
        /// 数据成员解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">Target data</param>
        internal static unsafe void DeserializeMembers(JsonDeserializer jsonDeserializer, ref T value)
        {
            JsonDeserializeConfig config = jsonDeserializer.Config;
            var memberMap = jsonDeserializer.MemberMap;
            if (memberMap == null)
            {
                AutoCSer.Memory.Pointer names = new AutoCSer.Memory.Pointer(memberNames.Data, 0);
                memberDeserializer(jsonDeserializer, ref value, ref names);
                int index = names.ByteSize;
                if (index == 0 ? jsonDeserializer.IsFirstObject() : (index != -1 && jsonDeserializer.IsNextObject()))
                {
                    bool isQuote;
                    do
                    {
                        if ((index = memberSearcher.SearchName(jsonDeserializer, out isQuote)) != -1)
                        {
#if AOT
                            if (jsonDeserializer.SearchColon() != 0) memberIndexDeserializer(jsonDeserializer, ref value, memberIndexs[index]);
#else
                            if (jsonDeserializer.SearchColon() != 0) memberDeserializers[index].Call(jsonDeserializer, ref value);
#endif
                            else return;
                        }
                        else if (jsonDeserializer.State == DeserializeStateEnum.Success)
                        {
                            if (isQuote) jsonDeserializer.SearchStringEnd();
                            else jsonDeserializer.SearchNameEnd();
                            if (jsonDeserializer.State == DeserializeStateEnum.Success && jsonDeserializer.SearchColon() != 0) jsonDeserializer.Ignore();
                            else return;
                        }
                        else return;
                    }
                    while (jsonDeserializer.State == DeserializeStateEnum.Success && jsonDeserializer.IsNextObject());
                }
                return;
            }
            var memberMapObject = memberMap as MemberMap<T>;
            if (memberMapObject != null)
            {
                try
                {
                    memberMapObject.MemberMapData.Empty();
                    jsonDeserializer.MemberMap = null;
                    AutoCSer.Memory.Pointer names = new AutoCSer.Memory.Pointer(memberNames.Data, 0);
                    memberMapDeserializer(jsonDeserializer, ref value, ref names, memberMapObject);
                    int index = names.ByteSize;
                    if (index == 0 ? jsonDeserializer.IsFirstObject() : (index != -1 && jsonDeserializer.IsNextObject()))
                    {
                        bool isQuote;
                        do
                        {
                            if ((index = memberSearcher.SearchName(jsonDeserializer, out isQuote)) != -1)
                            {
                                if (jsonDeserializer.SearchColon() != 0)
                                {
#if AOT
                                    int memberMapIndex = memberIndexs[index];
                                    memberIndexDeserializer(jsonDeserializer, ref value, memberMapIndex);
                                    memberMapObject.MemberMapData.SetMember(memberMapIndex);
#else
                                    memberDeserializers[index].Call(jsonDeserializer, memberMapObject, ref value);
#endif
                                }
                                else return;
                            }
                            else if (jsonDeserializer.State == DeserializeStateEnum.Success)
                            {
                                if (isQuote) jsonDeserializer.SearchStringEnd();
                                else jsonDeserializer.SearchNameEnd();
                                if (jsonDeserializer.State == DeserializeStateEnum.Success && jsonDeserializer.SearchColon() != 0) jsonDeserializer.Ignore();
                                else return;
                            }
                            else return;
                        }
                        while (jsonDeserializer.State == DeserializeStateEnum.Success && jsonDeserializer.IsNextObject());
                    }
                }
                finally { jsonDeserializer.MemberMap = memberMap; }
            }
            else jsonDeserializer.State = DeserializeStateEnum.MemberMap;
        }

        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        /// <param name="names"></param>
        private static void nullMember(JsonDeserializer jsonDeserializer, ref T value, ref AutoCSer.Memory.Pointer names) { }
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <param name="jsonDeserializer"></param>
        /// <param name="value"></param>
        /// <param name="names"></param>
        /// <param name="memberMap"></param>
        private static void nullMemberMap(JsonDeserializer jsonDeserializer, ref T value, ref AutoCSer.Memory.Pointer names, MemberMap<T> memberMap) { }
        /// <summary>
        /// 无成员对象解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">Target data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void noMemberValue(JsonDeserializer jsonDeserializer, ref T? value)
#else
        private static void noMemberValue(JsonDeserializer jsonDeserializer, ref T value)
#endif
        {
            value = default(T);
            jsonDeserializer.Ignore();
        }
        /// <summary>
        /// 无成员对象解析
        /// </summary>
        /// <param name="jsonDeserializer">JSON 反序列化</param>
        /// <param name="value">Target data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void noMember(JsonDeserializer jsonDeserializer, ref T? value)
#else
        private static void noMember(JsonDeserializer jsonDeserializer, ref T value)
#endif
        {
            if (jsonDeserializer.IsNull()) value = default(T);
            else if (value == null && !jsonDeserializer.Constructor(out value)) return;
            jsonDeserializer.Ignore();
        }

        unsafe static TypeDeserializer()
        {
            JsonSerializeAttribute attribute;
#if AOT
            var deserializeDelegate = Common.GetTypeDeserializeDelegate<T>(out attribute);
#else
            GenericType genericType = new GenericType<T>();
            var baseType = typeof(T);
            var deserializeDelegate = Common.GetTypeDeserializeDelegate(genericType, out attribute, out baseType);
#endif
            if (deserializeDelegate != null)
            {
#if NetStandard21
                DefaultDeserializer = (JsonDeserializer.DeserializeDelegate<T?>)deserializeDelegate;
#else
                DefaultDeserializer = (JsonDeserializer.DeserializeDelegate<T>)deserializeDelegate;
#endif
            }
            else
            {
                Type type = typeof(T);
#if AOT
                Type refType = type.MakeByRefType(), pointerRefType = typeof(AutoCSer.Memory.Pointer).MakeByRefType();
                var method = type.GetMethod(AutoCSer.CodeGenerator.JsonSerializeAttribute.JsonDeserializeMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, new Type[] { typeof(JsonDeserializer), refType, pointerRefType });
                if (method != null && !method.IsGenericMethod && method.ReturnType == typeof(void))
                {
                    var memberMapMethod = type.GetMethod(AutoCSer.CodeGenerator.JsonSerializeAttribute.JsonDeserializeMemberMapMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, new Type[] { typeof(JsonDeserializer), refType, pointerRefType, typeof(AutoCSer.Metadata.MemberMap<T>) });
                    if (memberMapMethod != null && !memberMapMethod.IsGenericMethod && memberMapMethod.ReturnType == typeof(void))
                    {
                        var memberMethod = type.GetMethod(AutoCSer.CodeGenerator.JsonSerializeAttribute.JsonDeserializeMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, new Type[] { typeof(JsonDeserializer), refType, typeof(int) });
                        if (memberMethod != null && !memberMethod.IsGenericMethod && memberMethod.ReturnType == typeof(void))
                        {
                            var memberNameMethod = type.GetMethod(AutoCSer.CodeGenerator.JsonSerializeAttribute.JsonDeserializeMemberNameMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, EmptyArray<Type>.Array);
                            if (memberNameMethod != null && !memberNameMethod.IsGenericMethod && memberNameMethod.ReturnType == typeof(AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>))
                            {
                                memberDeserializer = (DeserializeMember)method.CreateDelegate(typeof(DeserializeMember));
                                memberMapDeserializer = (DeserializeMemberMap)memberMapMethod.CreateDelegate(typeof(DeserializeMemberMap));
                                AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>> names = memberNameMethod.Invoke(null, null).castValue<AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<int>>>();
                                if (names.Key.Length != 0)
                                {
                                    memberIndexs = names.Value.ToArray();
                                    MemberNameSearcher searcher = MemberNameSearcher.Get(type, names.Key);
                                    memberNames = searcher.Names;
                                    memberSearcher = searcher.Searcher;
                                }
                                else
                                {
                                    memberIndexs = EmptyArray<int>.Array;
                                    memberNames = MemberNameSearcher.Null.Names;
                                    memberSearcher = MemberNameSearcher.Null.Searcher;
                                }
                                memberIndexDeserializer = (JsonDeserializer.MemberDeserializeDelegate<T>)memberMethod.CreateDelegate(typeof(JsonDeserializer.MemberDeserializeDelegate<T>));
                                DefaultDeserializer = type.IsValueType ? (JsonDeserializer.DeserializeDelegate<T?>)deserializeValue : (JsonDeserializer.DeserializeDelegate<T?>)deserializeClass;
                                return;
                            }
                            throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.JsonSerializeAttribute.JsonDeserializeMemberNameMethodName);
                        }
                        throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.JsonSerializeAttribute.JsonDeserializeMethodName);
                    }
                    throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.JsonSerializeAttribute.JsonDeserializeMemberMapMethodName);
                }
                throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.JsonSerializeAttribute.JsonDeserializeMethodName);
#else
                var fields = AutoCSer.TextSerialize.Common.GetDeserializeFields<JsonSerializeMemberAttribute>(MemberIndexGroup.GetFields(baseType ?? type, attribute.MemberFilters), attribute);
                LeftArray<AutoCSer.TextSerialize.PropertyMethod<JsonSerializeMemberAttribute>> properties = AutoCSer.TextSerialize.Common.GetDeserializeProperties<JsonSerializeMemberAttribute>(MemberIndexGroup.GetProperties(baseType ?? type, attribute.MemberFilters), attribute);
                int count = fields.Length + properties.Length;
                if (count != 0)
                {
                    TryDeserializeFilter[] deserializers = new TryDeserializeFilter[count];
                    string[] names = new string[count];
                    int index = 0;
                    DeserializeDynamicMethod dynamicMethod = new DeserializeDynamicMethod(genericType, false), memberMapDynamicMethod = new DeserializeDynamicMethod(genericType, true);
                    foreach (var member in fields)
                    {
                        deserializers[index].Set(DeserializeDynamicMethod.CreateDynamicMethod(type, member.Key.Member), member.Key);
                        dynamicMethod.Push(member.Key);
                        memberMapDynamicMethod.Push(member.Key);
                        names[index++] = member.Key.Member.Name;
                    }
                    foreach (AutoCSer.TextSerialize.PropertyMethod<JsonSerializeMemberAttribute> member in properties)
                    {
                        if (member.Method == null)
                        {
                            FieldIndex field = member.Property.AnonymousField.notNull();
                            deserializers[index].Set(DeserializeDynamicMethod.CreateDynamicMethod(type, field.Member), field);
                            dynamicMethod.Push(field);
                            memberMapDynamicMethod.Push(field);
                            names[index++] = field.AnonymousName;
                        }
                        else
                        {
                            deserializers[index].Set(DeserializeDynamicMethod.CreateDynamicMethod(type, member.Property.Member, member.Method), member.Property);
                            dynamicMethod.Push(member.Property, member.Method);
                            memberMapDynamicMethod.Push(member.Property, member.Method);
                            names[index++] = member.Property.Member.Name;
                        }
                    }
                    memberDeserializer = (DeserializeMember)dynamicMethod.Create(typeof(DeserializeMember));
                    memberMapDeserializer = (DeserializeMemberMap)memberMapDynamicMethod.Create(typeof(DeserializeMemberMap));
                    memberDeserializers = deserializers;
#if NetStandard21
                    DefaultDeserializer = type.IsValueType ? (JsonDeserializer.DeserializeDelegate<T?>)deserializeValue : (JsonDeserializer.DeserializeDelegate<T?>)deserializeClass;
#else
                    DefaultDeserializer = type.IsValueType ? (JsonDeserializer.DeserializeDelegate<T>)deserializeValue : (JsonDeserializer.DeserializeDelegate<T>)deserializeClass;
#endif
                    MemberNameSearcher searcher = MemberNameSearcher.Get(type, names);
                    memberNames = searcher.Names;
                    memberSearcher = searcher.Searcher;
                    return;
                }
#if NetStandard21
                DefaultDeserializer = type.IsValueType ? (JsonDeserializer.DeserializeDelegate<T?>)noMemberValue : (JsonDeserializer.DeserializeDelegate<T?>)noMember;
#else
                DefaultDeserializer = type.IsValueType ? (JsonDeserializer.DeserializeDelegate<T>)noMemberValue : (JsonDeserializer.DeserializeDelegate<T>)noMember;
#endif
#endif
            }
#if AOT
            memberIndexDeserializer = nullMemberIndex;
            memberIndexs = EmptyArray<int>.Array;
#else
            memberDeserializers = EmptyArray<TryDeserializeFilter>.Array;
#endif
            memberNames = MemberNameSearcher.Null.Names;
            memberSearcher = MemberNameSearcher.Null.Searcher;
            memberDeserializer = nullMember;
            memberMapDeserializer = nullMemberMap;
        }
    }
}
