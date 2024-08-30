using AutoCSer.Metadata;
using System;
using System.Collections.Generic;
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
        /// <summary>
        /// 成员解析器过滤
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
        private struct TryDeserializeFilter
        {
            /// <summary>
            /// 成员解析器
            /// </summary>
            private XmlDeserializer.DeserializeDelegate<T> deserialize;
            /// <summary>
            /// 集合子节点名称
            /// </summary>
            private string itemName;
            /// <summary>
            /// 成员位图索引
            /// </summary>
            private int memberMapIndex;
            /// <summary>
            /// 设置数据
            /// </summary>
            /// <param name="dynamicMethod"></param>
            /// <param name="member"></param>
            /// <param name="memberAttribute"></param>
            [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Set(DynamicMethod dynamicMethod, MemberIndexInfo member, XmlSerializeMemberAttribute memberAttribute)
            {
                deserialize = (XmlDeserializer.DeserializeDelegate<T>)dynamicMethod.CreateDelegate(typeof(XmlDeserializer.DeserializeDelegate<T>));
                itemName = memberAttribute?.ItemName;
                memberMapIndex = member.MemberIndex;
            }
            /// <summary>
            /// 成员解析器
            /// </summary>
            /// <param name="deserializer">XML 反序列化</param>
            /// <param name="value">目标数据</param>
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
            /// <param name="value">目标数据</param>
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
            /// <param name="value">目标数据</param>
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
        }
        /// <summary>
        /// 解析委托
        /// </summary>
        internal static readonly XmlDeserializer.DeserializeDelegate<T> DefaultDeserializer;
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
        /// <param name="value">目标数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void deserializeValue(XmlDeserializer deserializer, ref T value)
        {
            if (deserializer.IsValue() != 0) DeserializeMembers(deserializer, ref value);
            else value = default(T);
        }
        /// <summary>
        /// 引用类型对象解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        private static void deserializeClass(XmlDeserializer deserializer, ref T value)
        {
            if (deserializer.IsValue() != 0)
            {
                if (value == null)
                {
                    if (AutoCSer.Metadata.DefaultConstructor<T>.Type != Metadata.DefaultConstructorTypeEnum.None)
                    {
                        value = AutoCSer.Metadata.DefaultConstructor<T>.Constructor();
                    }
                    else if (!AutoCSer.XmlSerializer.CustomConfig.CallCustomConstructor(out value))
                    {
                        deserializer.IgnoreValue();
                        return;
                    }
                }
                DeserializeMembers(deserializer, ref value);
            }
            else value = AutoCSer.Common.GetDefault<T>();
        }
        /// <summary>
        /// 数据成员解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        internal static unsafe void DeserializeMembers(XmlDeserializer deserializer, ref T value)
        {
            byte* names = memberNames.Byte;
            XmlDeserializeConfig config = deserializer.Config;
            MemberMap memberMap = deserializer.MemberMap;
            int index = 0;
            if (memberMap == null)
            {
                while (deserializer.IsName(names, ref index))
                {
                    if (index == -1) return;
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
            MemberMap<T> memberMapObject = memberMap as MemberMap<T>;
            if (memberMapObject != null)
            {
                try
                {
                    memberMapObject.MemberMapData.Empty();
                    deserializer.MemberMap = null;
                    while (deserializer.IsName(names, ref index))
                    {
                        if (index == -1) return;
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
        internal static void Array(XmlDeserializer deserializer, ref T[] array)
        {
            int count = ArrayIndex(deserializer, ref array);
            if (count != -1 && count != array.Length) System.Array.Resize(ref array, count);
        }
        /// <summary>
        /// 数组解析
        /// </summary>
        /// <param name="deserializer">XML解析器</param>
        /// <param name="array">目标数据</param>
        /// <returns>数据数量,-1表示失败</returns>
        internal static unsafe int ArrayIndex(XmlDeserializer deserializer, ref T[] array)
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
                            T value = default(T);
                            if (deserializer.IsArrayItem(itemFixed, arrayItemName.Length) != 0)
                            {
                                DefaultDeserializer(deserializer, ref value);
                                if (deserializer.State != DeserializeStateEnum.Success) return -1;
                                if (deserializer.CheckNameEnd(itemFixed, name.ByteSize) == 0) break;
                            }
                            if (index == 0) array = new T[deserializer.Config.NewArraySize];
                            else array = AutoCSer.Common.Config.GetCopyArray(array, index << 1);
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
                            else array = AutoCSer.Common.Config.GetCopyArray(array, index << 1);
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
        /// <returns>目标数据</returns>
        internal static IEnumerable<T> Enumerable(XmlDeserializer deserializer, AutoCSer.Memory.Pointer arrayItemName)
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
                        T value = default(T);
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
        /// <param name="value">目标数据</param>
        [MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void noMemberValue(XmlDeserializer deserializer, ref T value)
        {
            value = default(T);
            deserializer.IgnoreValue();
        }
        /// <summary>
        /// 无成员对象解析
        /// </summary>
        /// <param name="deserializer">XML 反序列化</param>
        /// <param name="value">目标数据</param>
        private static void noMember(XmlDeserializer deserializer, ref T value)
        {
            if (deserializer.IsValue() == 0)
            {
                if (value == null) value = AutoCSer.Metadata.DefaultConstructor<T>.Constructor();
            }
            else value = default(T);
            deserializer.IgnoreValue();
        }

        unsafe static TypeDeserializer()
        {
            XmlSerializeAttribute attribute;
            AutoCSer.Extensions.Metadata.GenericType<T> genericType = new AutoCSer.Extensions.Metadata.GenericType<T>();
            Delegate deserializeDelegate = Common.GetTypeDeserializeDelegate(genericType, out attribute);
            if (deserializeDelegate != null) DefaultDeserializer = (XmlDeserializer.DeserializeDelegate<T>)deserializeDelegate;
            else
            {
                Type type = typeof(T);
                LeftArray<KeyValue<FieldIndex, XmlSerializeMemberAttribute>> fields = AutoCSer.TextSerialize.Common.GetDeserializeFields<XmlSerializeMemberAttribute>(MemberIndexGroup<T>.GetFields(attribute.MemberFilters), attribute);
                LeftArray<AutoCSer.TextSerialize.PropertyMethod<XmlSerializeMemberAttribute>> properties = AutoCSer.TextSerialize.Common.GetDeserializeProperties<XmlSerializeMemberAttribute>(MemberIndexGroup<T>.GetProperties(attribute.MemberFilters), attribute);
                int count = fields.Length + properties.Length;
                if (count != 0)
                {
                    TryDeserializeFilter[] deserializers = new TryDeserializeFilter[count];
                    string[] names = new string[count];
                    int index = 0;
                    foreach (KeyValue<FieldIndex, XmlSerializeMemberAttribute> member in fields)
                    {
                        deserializers[index].Set(Common.CreateDynamicMethod(type, member.Key.Member), member.Key, member.Value);
                        names[index++] = member.Key.AnonymousName;
                    }
                    foreach (AutoCSer.TextSerialize.PropertyMethod<XmlSerializeMemberAttribute> member in properties)
                    {
                        deserializers[index].Set(Common.CreateDynamicMethod(type, member.Property.Member, member.Method), member.Property, member.MemberAttribute);
                        names[index++] = member.Property.Member.Name;
                    }
                    DefaultDeserializer = type.IsValueType ? (XmlDeserializer.DeserializeDelegate<T>)deserializeValue : (XmlDeserializer.DeserializeDelegate<T>)deserializeClass;
                    memberDeserializers = deserializers;
                    MemberNameSearcher searcher = MemberNameSearcher.Get(type, names);
                    memberNames = searcher.Names;
                    memberSearcher = searcher.Searcher;
                    return;
                }
                DefaultDeserializer = type.IsValueType ? (XmlDeserializer.DeserializeDelegate<T>)noMemberValue : (XmlDeserializer.DeserializeDelegate<T>)noMember;
            }
            memberDeserializers = EmptyArray<TryDeserializeFilter>.Array;
            memberNames = MemberNameSearcher.Null.Names;
            memberSearcher = MemberNameSearcher.Null.Searcher;
        }
    }
}
