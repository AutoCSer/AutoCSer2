using AutoCSer.Extensions;
using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace AutoCSer.Xml
{
    /// <summary>
    /// 类型反序列化
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    internal static class TypeDeserializer<T>
    {
#if AOT
        /// <summary>
        /// 成员解析
        /// </summary>
        private static readonly XmlDeserializer.MemberDeserializeDelegate<T> memberIndexDeserializer;
        /// <summary>
        /// JSON 反序列化
        /// </summary>
        /// <param name="deserializer"></param>
        /// <param name="value"></param>
        /// <param name="memberIndex"></param>
        private static void nullMemberIndex(XmlDeserializer deserializer, ref T value, int memberIndex) { }
#endif
        /// <summary>
        /// 成员解析器过滤
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct TryDeserializeFilter
        {
            /// <summary>
            /// 集合子节点名称
            /// </summary>
#if NetStandard21
            private string? itemName;
#else
            private string itemName;
#endif
            /// <summary>
            /// 成员位图索引
            /// </summary>
            private int memberMapIndex;
#if AOT
            /// <summary>
            /// Set the data
        /// 设置数据
            /// </summary>
            /// <param name="member"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Set(KeyValue<int, string?> member)
            {
                memberMapIndex = member.Key;
                itemName = member.Value;
            }
            /// <summary>
            /// 成员解析器
            /// </summary>
            /// <param name="deserializer">XML 反序列化</param>
            /// <param name="value">Target data</param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Call(XmlDeserializer deserializer, ref T value)
            {
                deserializer.ItemName = itemName;
                memberIndexDeserializer(deserializer, ref value, memberMapIndex);
            }
            /// <summary>
            /// 成员解析器
            /// </summary>
            /// <param name="deserializer">XML 反序列化</param>
            /// <param name="memberMap">成员位图</param>
            /// <param name="value">Target data</param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Call(XmlDeserializer deserializer, MemberMap<T> memberMap, ref T value)
            {
                deserializer.ItemName = itemName;
                memberIndexDeserializer(deserializer, ref value, memberMapIndex);
                if (deserializer.State == DeserializeStateEnum.Success) memberMap.MemberMapData.SetMember(memberMapIndex);
            }
            /// <summary>
            /// 成员解析器
            /// </summary>
            /// <param name="deserializer">XML 反序列化</param>
            /// <param name="memberMap">成员位图</param>
            /// <param name="value">Target data</param>
            /// <returns></returns>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public int TryCall(XmlDeserializer deserializer, MemberMap<T> memberMap, ref T value)
            {
                deserializer.ItemName = itemName;
                memberIndexDeserializer(deserializer, ref value, memberMapIndex);
                if (deserializer.State == DeserializeStateEnum.Success)
                {
                    memberMap.MemberMapData.SetMember(memberMapIndex);
                    return 1;
                }
                return 0;
            }
#else
            /// <summary>
            /// 成员解析器
            /// </summary>
            private XmlDeserializer.DeserializeDelegate<T> deserialize;
            /// <summary>
            /// Set the data
            /// 设置数据
            /// </summary>
            /// <param name="method"></param>
            /// <param name="member"></param>
            /// <param name="memberAttribute"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
            public void Set(MethodInfo method, MemberIndexInfo member, XmlSerializeMemberAttribute? memberAttribute)
#else
            public void Set(MethodInfo method, MemberIndexInfo member, XmlSerializeMemberAttribute memberAttribute)
#endif
            {
                deserialize = (XmlDeserializer.DeserializeDelegate<T>)method.CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T>));
                itemName = memberAttribute?.ItemName;
                memberMapIndex = member.MemberIndex;
            }
            /// <summary>
            /// 成员解析器
            /// </summary>
            /// <param name="deserializer">XML 反序列化</param>
            /// <param name="value">Target data</param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Call(XmlDeserializer deserializer, ref T value)
            {
                deserializer.ItemName = itemName;
                deserialize(deserializer, ref value);
            }
            /// <summary>
            /// 成员解析器
            /// </summary>
            /// <param name="deserializer">XML 反序列化</param>
            /// <param name="memberMap">成员位图</param>
            /// <param name="value">Target data</param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Call(XmlDeserializer deserializer, MemberMap<T> memberMap, ref T value)
            {
                deserializer.ItemName = itemName;
                deserialize(deserializer, ref value);
                if (deserializer.State == DeserializeStateEnum.Success) memberMap.MemberMapData.SetMember(memberMapIndex);
            }
            /// <summary>
            /// 成员解析器
            /// </summary>
            /// <param name="deserializer">XML 反序列化</param>
            /// <param name="memberMap">成员位图</param>
            /// <param name="value">Target data</param>
            /// <returns></returns>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public int TryCall(XmlDeserializer deserializer, MemberMap<T> memberMap, ref T value)
            {
                deserializer.ItemName = itemName;
                deserialize(deserializer, ref value);
                if (deserializer.State == DeserializeStateEnum.Success)
                {
                    memberMap.MemberMapData.SetMember(memberMapIndex);
                    return 1;
                }
                return 0;
            }
#endif
        }
        /// <summary>
        /// 解析委托
        /// </summary>
#if NetStandard21
        internal static readonly XmlDeserializer.DeserializeDelegate<T?> DefaultDeserializer;
#else
        internal static readonly XmlDeserializer.DeserializeDelegate<T> DefaultDeserializer;
#endif
        /// <summary>
        /// 成员解析器集合
        /// </summary>
        private static readonly TryDeserializeFilter[] memberDeserializers;
        /// <summary>
        /// 成员名称查找数据
        /// </summary>
        private static AutoCSer.Memory.Pointer memberSearcher;
        /// <summary>
        /// 默认顺序成员名称数据
        /// </summary>
        private static AutoCSer.Memory.Pointer memberNames;
        /// <summary>
        /// 值类型对象解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">Target data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void deserializeValue(XmlDeserializer deserializer, ref T? value)
#else
        private static void deserializeValue(XmlDeserializer deserializer, ref T value)
#endif
        {
#pragma warning disable CS8601
            if (deserializer.IsValue() != 0) DeserializeMembers(deserializer, ref value);
#pragma warning restore CS8601
            else value = default(T);
        }
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">Target data</param>
#if NetStandard21
        private static void deserializeClass(XmlDeserializer deserializer, ref T? value)
#else
        private static void deserializeClass(XmlDeserializer deserializer, ref T value)
#endif
        {
            if (deserializer.IsValue() != 0)
            {
                if (value == null)
                {
                    if (AutoCSer.Metadata.DefaultConstructor<T>.Type != Metadata.DefaultConstructorTypeEnum.None)
                    {
                        if (!deserializer.Constructor(out value)) return;
                    }
                    else if (!AutoCSer.XmlSerializer.CustomConfig.CallCustomConstructor(out value))
                    {
                        deserializer.IgnoreValue();
                        return;
                    }
                }
#pragma warning disable CS8601
                DeserializeMembers(deserializer, ref value);
#pragma warning restore CS8601
            }
            else value = default(T);
        }
        /// <summary>
        /// 数据成员解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">Target data</param>
        internal static unsafe void DeserializeMembers(XmlDeserializer deserializer, ref T value)
        {
            byte* names = memberNames.Byte;
            XmlDeserializeConfig config = deserializer.Config;
            var memberMap = deserializer.MemberMap;
            int index = 0;
            if (memberMap == null)
            {
                while (deserializer.IsName(names, ref index))
                {
                    if (index == -1)
                    {
                        if (deserializer.IsValue() != 0) deserializer.IgnoreValue();
                        return;
                    }
                    memberDeserializers[index].Call(deserializer, ref value);
                    if (deserializer.State != DeserializeStateEnum.Success) return;
                    if (deserializer.IsNameEnd(names) == 0)
                    {
                        if (deserializer.CheckNameEnd((char*)(names + (sizeof(short) + sizeof(char))), (*(short*)names >> 1) - 2) == 0) return;
                        break;
                    }
                    ++index;
                    names += *(short*)names + sizeof(short);
                }
                AutoCSer.StateSearcher.CharSearcher searcher = new AutoCSer.StateSearcher.CharSearcher(memberSearcher);
                AutoCSer.Memory.Pointer name = default(AutoCSer.Memory.Pointer);
                byte isTagEnd = 0;
                do
                {
                    if (deserializer.GetName(ref name, ref isTagEnd)) return;
                    if (isTagEnd == 0)
                    {
                        if ((index = searcher.UnsafeSearch(name.Char, name.ByteSize)) == -1)
                        {
                            if (deserializer.IgnoreValue() == 0) return;
                        }
                        else
                        {
                            memberDeserializers[index].Call(deserializer, ref value);
                            if(deserializer.State != DeserializeStateEnum.Success) return;
                        }
                        if (deserializer.CheckNameEnd(name.Char, name.ByteSize) == 0) return;
                    }
                }
                while (true);
            }
            var memberMapObject = memberMap as MemberMap<T>;
            if (memberMapObject != null)
            {
                try
                {
                    memberMapObject.MemberMapData.Empty();
                    deserializer.MemberMap = null;
                    while (deserializer.IsName(names, ref index))
                    {
                        if (index == -1)
                        {
                            if (deserializer.IsValue() != 0) deserializer.IgnoreValue();
                            return;
                        }
                        memberDeserializers[index].Call(deserializer, memberMapObject, ref value);
                        if (deserializer.State != DeserializeStateEnum.Success) return;
                        if (deserializer.IsNameEnd(names) == 0)
                        {
                            if (deserializer.CheckNameEnd((char*)(names + (sizeof(short) + sizeof(char))), (*(short*)names >> 1) - 2) == 0) return;
                            break;
                        }
                        ++index;
                        names += *(short*)names + sizeof(short);
                    }
                    AutoCSer.StateSearcher.CharSearcher searcher = new AutoCSer.StateSearcher.CharSearcher(memberSearcher);
                    AutoCSer.Memory.Pointer name = default(AutoCSer.Memory.Pointer);
                    byte isTagEnd = 0;
                    do
                    {
                        if (deserializer.GetName(ref name, ref isTagEnd)) return;
                        if (isTagEnd == 0)
                        {
                            if ((index = searcher.UnsafeSearch(name.Char, name.ByteSize)) == -1)
                            {
                                if (deserializer.IgnoreValue() == 0) return;
                            }
                            else if (memberDeserializers[index].TryCall(deserializer, memberMapObject, ref value) == 0) return;
                            if (deserializer.CheckNameEnd(name.Char, name.ByteSize) == 0) return;
                        }
                    }
                    while (true);
                }
                finally { deserializer.MemberMap = memberMap; }
            }
            deserializer.State = DeserializeStateEnum.MemberMap;
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="deserializer">XML解析器</param>
        /// <param name="array">目标数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        internal static void Array(XmlDeserializer deserializer, ref T?[]? array)
#else
        internal static void Array(XmlDeserializer deserializer, ref T[] array)
#endif
        {
            int count = ArrayIndex(deserializer, ref array);
            if (count != -1 && count != array.notNull().Length) System.Array.Resize(ref array, count);
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="deserializer">XML解析器</param>
        /// <param name="array">目标数据</param>
        /// <returns>数据数量,-1表示失败</returns>
#if NetStandard21
        internal static unsafe int ArrayIndex(XmlDeserializer deserializer, ref T?[]? array)
#else
        internal static unsafe int ArrayIndex(XmlDeserializer deserializer, ref T[] array)
#endif
        {
            if (array == null) array = EmptyArray<T>.Array;
            string arrayItemName = deserializer.ArrayItemName;
            AutoCSer.Memory.Pointer name = default(AutoCSer.Memory.Pointer);
            int index = 0;
            byte isTagEnd = 0;
            fixed (char* itemFixed = arrayItemName)
            {
                do
                {
                    if (deserializer.GetName(ref name, ref isTagEnd)) break;
                    if (isTagEnd == 0)
                    {
                        if (arrayItemName.Length != name.ByteSize || !AutoCSer.Memory.Common.SimpleEqualNotNull((byte*)itemFixed, name.Byte, name.ByteSize << 1))
                        {
                            deserializer.State = DeserializeStateEnum.NotArrayItem;
                            return -1;
                        }
                        if (index == array.Length)
                        {
                            var value = default(T);
                            if (deserializer.IsArrayItem(itemFixed, arrayItemName.Length) != 0)
                            {
                                DefaultDeserializer(deserializer, ref value);
                                if (deserializer.State != DeserializeStateEnum.Success) return -1;
                                if (deserializer.CheckNameEnd(itemFixed, name.ByteSize) == 0) break;
                            }
                            if (index == 0) array = new T[deserializer.Config.NewArraySize];
                            else array = AutoCSer.Common.GetCopyArray(array, index << 1);
                            array[index++] = value;
                        }
                        else
                        {
                            if (deserializer.IsArrayItem(itemFixed, arrayItemName.Length) != 0)
                            {
                                DefaultDeserializer(deserializer, ref array[index]);
                                if (deserializer.State != DeserializeStateEnum.Success) return -1;
                                if (deserializer.CheckNameEnd(itemFixed, name.ByteSize) == 0) break;
                            }
                            ++index;
                        }
                    }
                    else
                    {
                        if (index == array.Length)
                        {
                            if (index == 0) array = new T[deserializer.Config.NewArraySize];
                            else array = AutoCSer.Common.GetCopyArray(array, index << 1);
                        }
                        array[index++] = default(T);
                    }
                }
                while (true);
            }
            return deserializer.State == DeserializeStateEnum.Success ? index : -1;
        }
        /// <summary>
        /// 集合解析
        /// </summary>
        /// <param name="deserializer">XML解析器</param>
        /// <param name="arrayItemName">集合子节点名称</param>
        /// <returns>Target data
        /// 目标数据</returns>
#if NetStandard21
        internal static IEnumerable<T?> Enumerable(XmlDeserializer deserializer, AutoCSer.Memory.Pointer arrayItemName)
#else
        internal static IEnumerable<T> Enumerable(XmlDeserializer deserializer, AutoCSer.Memory.Pointer arrayItemName)
#endif
        {
            AutoCSer.Memory.Pointer name = default(AutoCSer.Memory.Pointer);
            byte isTagEnd = 0;
            do
            {
                if (deserializer.GetName(ref name, ref isTagEnd)) break;
                if (isTagEnd == 0)
                {
                    if (arrayItemName.ByteSize != name.ByteSize || !AutoCSer.Memory.Common.SimpleEqualNotNull(ref arrayItemName, ref name, name.ByteSize << 1))
                    {
                        deserializer.State = DeserializeStateEnum.NotArrayItem;
                        break;
                    }
                    if (deserializer.IsArrayItem(ref arrayItemName) != 0)
                    {
                        var value = default(T);
                        DefaultDeserializer(deserializer, ref value);
                        if (deserializer.State != DeserializeStateEnum.Success) break;
                        yield return value;
                        if (deserializer.CheckNameEnd(ref arrayItemName) == 0) break;
                        continue;
                    }
                }
                yield return default(T);
            }
            while (true);
        }
        /// <summary>
        /// 无成员对象解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">Target data</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if NetStandard21
        private static void noMemberValue(XmlDeserializer deserializer, ref T? value)
#else
        private static void noMemberValue(XmlDeserializer deserializer, ref T value)
#endif
        {
            value = default(T);
            deserializer.IgnoreValue();
        }
        /// <summary>
        /// 无成员对象解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">Target data</param>
#if NetStandard21
        private static void noMember(XmlDeserializer deserializer, ref T? value)
#else
        private static void noMember(XmlDeserializer deserializer, ref T value)
#endif
        {
            if (deserializer.IsValue() == 0)
            {
                if (value == null && !deserializer.Constructor(out value)) return;
            }
            else value = default(T);
            deserializer.IgnoreValue();
        }

        unsafe static TypeDeserializer()
        {
            XmlSerializeAttribute attribute;
#if AOT
            var deserializeDelegate = Common.GetTypeDeserializeDelegate<T>(out attribute);
#else
            AutoCSer.Extensions.Metadata.GenericType<T> genericType = new AutoCSer.Extensions.Metadata.GenericType<T>();
            var baseType = default(Type);
            var deserializeDelegate = Common.GetTypeDeserializeDelegate(genericType, out attribute, out baseType);
#endif
            if (deserializeDelegate != null)
            {
#if NetStandard21
                DefaultDeserializer = (XmlDeserializer.DeserializeDelegate<T?>)deserializeDelegate;
#else
                DefaultDeserializer = (XmlDeserializer.DeserializeDelegate<T>)deserializeDelegate;
#endif
            }
            else
            {
                Type type = typeof(T);
#if AOT
                Type refType = type.MakeByRefType(), pointerRefType = typeof(AutoCSer.Memory.Pointer).MakeByRefType();
                var memberMethod = type.GetMethod(AutoCSer.CodeGenerator.XmlSerializeAttribute.XmlDeserializeMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, new Type[] { typeof(XmlDeserializer), refType, typeof(int) });
                if (memberMethod != null && !memberMethod.IsGenericMethod && memberMethod.ReturnType == typeof(void))
                {
                    var memberNameMethod = type.GetMethod(AutoCSer.CodeGenerator.XmlSerializeAttribute.XmlDeserializeMemberNameMethodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, EmptyArray<Type>.Array);
                    if (memberNameMethod != null && !memberNameMethod.IsGenericMethod && memberNameMethod.ReturnType == typeof(AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string?>>>))
                    {
                        AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string?>>> names = memberNameMethod.Invoke(null, null).castValue<AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string?>>>>();
                        if (names.Key.Length != 0)
                        {
                            int index = 0;
                            TryDeserializeFilter[] deserializers = new TryDeserializeFilter[names.Key.Length];
                            foreach (KeyValue<int, string?> member in names.Value) deserializers[index++].Set(member);
                            memberDeserializers = deserializers;
                            MemberNameSearcher searcher = MemberNameSearcher.Get(type, names.Key);
                            memberNames = searcher.Names;
                            memberSearcher = searcher.Searcher;
                        }
                        else
                        {
                            memberDeserializers = EmptyArray<TryDeserializeFilter>.Array;
                            memberNames = MemberNameSearcher.Null.Names;
                            memberSearcher = MemberNameSearcher.Null.Searcher;
                        }
                        memberIndexDeserializer = (XmlDeserializer.MemberDeserializeDelegate<T>)memberMethod.CreateDelegate(typeof(XmlDeserializer.MemberDeserializeDelegate<T>));
                        DefaultDeserializer = type.IsValueType ? (XmlDeserializer.DeserializeDelegate<T?>)deserializeValue : (XmlDeserializer.DeserializeDelegate<T?>)deserializeClass;
                        return;
                    }
                    throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.XmlSerializeAttribute.XmlDeserializeMemberNameMethodName);
                }
                throw new MissingMethodException(type.fullName(), AutoCSer.CodeGenerator.XmlSerializeAttribute.XmlDeserializeMethodName);
#else
                var  fields = AutoCSer.TextSerialize.Common.GetDeserializeFields<XmlSerializeMemberAttribute>(MemberIndexGroup.GetFields(baseType ?? type, attribute.MemberFilters), attribute);
                LeftArray<AutoCSer.TextSerialize.PropertyMethod<XmlSerializeMemberAttribute>> properties = AutoCSer.TextSerialize.Common.GetDeserializeProperties<XmlSerializeMemberAttribute>(MemberIndexGroup.GetProperties(baseType ?? type, attribute.MemberFilters), attribute);
                int count = fields.Length + properties.Length;
                if (count != 0)
                {
                    TryDeserializeFilter[] deserializers = new TryDeserializeFilter[count];
                    string[] names = new string[count];
                    int index = 0;
                    foreach (var member in fields)
                    {
                        deserializers[index].Set(Common.CreateDynamicMethod(type, member.Key.Member), member.Key, member.Value);
                        names[index++] = member.Key.Member.Name;
                    }
                    foreach (AutoCSer.TextSerialize.PropertyMethod<XmlSerializeMemberAttribute> member in properties)
                    {
                        if (member.Method == null)
                        {
                            FieldIndex field = member.Property.AnonymousField.notNull();
                            deserializers[index].Set(Common.CreateDynamicMethod(type, field.Member), field, member.MemberAttribute);
                            names[index++] = field.AnonymousName;
                        }
                        else
                        {
                            deserializers[index].Set(Common.CreateDynamicMethod(type, member.Property.Member, member.Method), member.Property, member.MemberAttribute);
                            names[index++] = member.Property.Member.Name;
                        }
                    }
#if NetStandard21
                    DefaultDeserializer = type.IsValueType ? (XmlDeserializer.DeserializeDelegate<T?>)deserializeValue : (XmlDeserializer.DeserializeDelegate<T?>)deserializeClass;
#else
                    DefaultDeserializer = type.IsValueType ? (XmlDeserializer.DeserializeDelegate<T>)deserializeValue : (XmlDeserializer.DeserializeDelegate<T>)deserializeClass;
#endif
                    memberDeserializers = deserializers;
                    MemberNameSearcher searcher = MemberNameSearcher.Get(type, names);
                    memberNames = searcher.Names;
                    memberSearcher = searcher.Searcher;
                    return;
                }
#if NetStandard21
                DefaultDeserializer = type.IsValueType ? (XmlDeserializer.DeserializeDelegate<T?>)noMemberValue : (XmlDeserializer.DeserializeDelegate<T?>)noMember;
#else
                DefaultDeserializer = type.IsValueType ? (XmlDeserializer.DeserializeDelegate<T>)noMemberValue : (XmlDeserializer.DeserializeDelegate<T>)noMember;
#endif
#endif
            }
#if AOT
            memberIndexDeserializer = nullMemberIndex;
#endif
            memberDeserializers = EmptyArray<TryDeserializeFilter>.Array;
            memberNames = MemberNameSearcher.Null.Names;
            memberSearcher = MemberNameSearcher.Null.Searcher;
        }
    }
}
