//本文件由程序自动生成，请不要自行修改
using System;
using System.Numerics;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer.Diagnostics
{
        /// <summary>
        /// 服务端时间戳
        /// </summary>
    public partial struct ServerTimestamp
    {
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.Diagnostics.ServerTimestamp value)
            {
                serializer.Simple(value);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.Diagnostics.ServerTimestamp value)
            {
                deserializer.Simple(ref value);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                return new AutoCSer.BinarySerialize.TypeInfo(true, 0, 1073741827);
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.Diagnostics.ServerTimestamp value = default(AutoCSer.Diagnostics.ServerTimestamp);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.Diagnostics.ServerTimestamp));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Diagnostics.ServerTimestamp));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.Diagnostics.ServerTimestamp>();
            }
    }
}namespace AutoCSer
{
        /// <summary>
        /// 索引范围
        /// </summary>
    internal partial struct Range
    {
            /// <summary>
            /// 二进制序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void BinarySerialize(AutoCSer.BinarySerializer serializer, AutoCSer.Range value)
            {
                serializer.Simple(value);
            }
            /// <summary>
            /// 二进制反序列化
            /// </summary>
            /// <param name="deserializer"></param>
            /// <param name="value"></param>
            internal static void BinaryDeserialize(AutoCSer.BinaryDeserializer deserializer, ref AutoCSer.Range value)
            {
                deserializer.Simple(ref value);
            }
            /// <summary>
            /// 获取二进制序列化类型信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.BinarySerialize.TypeInfo BinarySerializeMemberTypes()
            {
                return new AutoCSer.BinarySerialize.TypeInfo(true, 0, 1073741826);
            }
            /// <summary>
            /// 二进制序列化代码生成调用激活 AOT 反射
            /// </summary>
            internal static void BinarySerialize()
            {
                AutoCSer.Range value = default(AutoCSer.Range);
                BinarySerialize(null, value);
                BinaryDeserialize(null, ref value);
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.Range));
                BinarySerializeMemberTypes();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Range));
                AutoCSer.Metadata.DefaultConstructor.GetIsSerializeConstructor<AutoCSer.Range>();
            }
    }
}namespace AutoCSer
{
        /// <summary>
        /// 键值对（用于二进制序列化屏蔽引用操作）
        /// </summary>
    public partial struct BinarySerializeKeyValue<KT,VT>
    {
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void XmlSerialize(AutoCSer.XmlSerializer serializer, AutoCSer.BinarySerializeKeyValue<KT,VT> value)
            {
                value.xmlSerialize(serializer);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void XmlSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.BinarySerializeKeyValue<KT,VT>> memberMap, XmlSerializer serializer, AutoCSer.BinarySerializeKeyValue<KT,VT> value, AutoCSer.Memory.CharStream stream)
            {
                value.xmlSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void xmlSerialize(AutoCSer.XmlSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                if (AutoCSer.XmlSerializer.IsOutputGenericParameter(__serializer__, Key))
                {
                    __stream__.SimpleWrite(@"<Key>");
                    if (Key != null) __serializer__.XmlSerializeType(Key);
                    __stream__.SimpleWrite(@"</Key>");
                }
                if (AutoCSer.XmlSerializer.IsOutputGenericParameter(__serializer__, Value))
                {
                    __stream__.SimpleWrite(@"<Value>");
                    if (Value != null) __serializer__.XmlSerializeType(Value);
                    __stream__.SimpleWrite(@"</Value>");
                }
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void xmlSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.BinarySerializeKeyValue<KT,VT>> __memberMap__, XmlSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                if (__memberMap__.IsMember(0) && AutoCSer.XmlSerializer.IsOutputGenericParameter(__serializer__, Key))
                {
                    __stream__.SimpleWrite(@"<Key>");
                    if (Key != null) __serializer__.XmlSerializeType(Key);
                    __stream__.SimpleWrite(@"</Key>");
                }
                if (__memberMap__.IsMember(1) && AutoCSer.XmlSerializer.IsOutputGenericParameter(__serializer__, Value))
                {
                    __stream__.SimpleWrite(@"<Value>");
                    if (Value != null) __serializer__.XmlSerializeType(Value);
                    __stream__.SimpleWrite(@"</Value>");
                }
            }
            /// <summary>
            /// 成员 XML 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void XmlDeserialize(AutoCSer.XmlDeserializer __deserializer__, ref AutoCSer.BinarySerializeKeyValue<KT,VT> __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.XmlDeserialize(ref __value__.Key);
                        return;
                    case 1:
                        __deserializer__.XmlDeserialize(ref __value__.Value);
                        return;
                }
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> XmlDeserializeMemberNames()
            {
                return xmlDeserializeMemberName();
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> xmlDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(2);
                AutoCSer.LeftArray<KeyValue<int, string>> indexs = new AutoCSer.LeftArray<KeyValue<int, string>>(2);
                names.Add(nameof(Key));
                indexs.Add(new KeyValue<int, string>(0, null));
                names.Add(nameof(Value));
                indexs.Add(new KeyValue<int, string>(1, null));
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>(names, indexs);
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            public static void XmlSerialize()
            {
                AutoCSer.BinarySerializeKeyValue<KT,VT> value = default(AutoCSer.BinarySerializeKeyValue<KT,VT>);
                XmlSerialize(null, value);
                XmlSerializeMemberMap(null, null, value, null);
                XmlDeserialize(null, ref value, 0);
                XmlDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.BinarySerializeKeyValue<KT,VT>));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.BinarySerializeKeyValue<KT,VT>));
            }
    }
}namespace AutoCSer.Extensions
{
        /// <summary>
        /// System.Numerics.Complex
        /// </summary>
    internal partial struct SerializeComplex
    {
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void XmlSerialize(AutoCSer.XmlSerializer serializer, AutoCSer.Extensions.SerializeComplex value)
            {
                value.xmlSerialize(serializer);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void XmlSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.Extensions.SerializeComplex> memberMap, XmlSerializer serializer, AutoCSer.Extensions.SerializeComplex value, AutoCSer.Memory.CharStream stream)
            {
                value.xmlSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void xmlSerialize(AutoCSer.XmlSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                {
                    __stream__.SimpleWrite(@"<Imaginary>");
                    __serializer__.XmlSerialize(Imaginary);
                    __stream__.SimpleWrite(@"</Imaginary>");
                }
                {
                    __stream__.SimpleWrite(@"<Real>");
                    __serializer__.XmlSerialize(Real);
                    __stream__.SimpleWrite(@"</Real>");
                }
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void xmlSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.Extensions.SerializeComplex> __memberMap__, XmlSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                if (__memberMap__.IsMember(0))
                {
                    __stream__.SimpleWrite(@"<Imaginary>");
                    __serializer__.XmlSerialize(Imaginary);
                    __stream__.SimpleWrite(@"</Imaginary>");
                }
                if (__memberMap__.IsMember(1))
                {
                    __stream__.SimpleWrite(@"<Real>");
                    __serializer__.XmlSerialize(Real);
                    __stream__.SimpleWrite(@"</Real>");
                }
            }
            /// <summary>
            /// 成员 XML 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void XmlDeserialize(AutoCSer.XmlDeserializer __deserializer__, ref AutoCSer.Extensions.SerializeComplex __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.XmlDeserialize(ref __value__.Imaginary);
                        return;
                    case 1:
                        __deserializer__.XmlDeserialize(ref __value__.Real);
                        return;
                }
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> XmlDeserializeMemberNames()
            {
                return xmlDeserializeMemberName();
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> xmlDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(2);
                AutoCSer.LeftArray<KeyValue<int, string>> indexs = new AutoCSer.LeftArray<KeyValue<int, string>>(2);
                names.Add(nameof(Imaginary));
                indexs.Add(new KeyValue<int, string>(0, null));
                names.Add(nameof(Real));
                indexs.Add(new KeyValue<int, string>(1, null));
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>(names, indexs);
            }
            /// <summary>
            /// 获取 Xml 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> XmlSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(double));
                return types;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void XmlSerialize()
            {
                AutoCSer.Extensions.SerializeComplex value = default(AutoCSer.Extensions.SerializeComplex);
                XmlSerialize(null, value);
                XmlSerializeMemberMap(null, null, value, null);
                XmlDeserialize(null, ref value, 0);
                XmlDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Extensions.SerializeComplex));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.Extensions.SerializeComplex));
                XmlSerializeMemberTypes();
            }
    }
}namespace AutoCSer.Extensions
{
        /// <summary>
        /// System.Numerics.Matrix3x2
        /// </summary>
    internal partial struct SerializeMatrix3x2
    {
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void XmlSerialize(AutoCSer.XmlSerializer serializer, AutoCSer.Extensions.SerializeMatrix3x2 value)
            {
                value.xmlSerialize(serializer);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void XmlSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.Extensions.SerializeMatrix3x2> memberMap, XmlSerializer serializer, AutoCSer.Extensions.SerializeMatrix3x2 value, AutoCSer.Memory.CharStream stream)
            {
                value.xmlSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void xmlSerialize(AutoCSer.XmlSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                {
                    __stream__.SimpleWrite(@"<M11>");
                    __serializer__.XmlSerialize(M11);
                    __stream__.SimpleWrite(@"</M11>");
                }
                {
                    __stream__.SimpleWrite(@"<M12>");
                    __serializer__.XmlSerialize(M12);
                    __stream__.SimpleWrite(@"</M12>");
                }
                {
                    __stream__.SimpleWrite(@"<M21>");
                    __serializer__.XmlSerialize(M21);
                    __stream__.SimpleWrite(@"</M21>");
                }
                {
                    __stream__.SimpleWrite(@"<M22>");
                    __serializer__.XmlSerialize(M22);
                    __stream__.SimpleWrite(@"</M22>");
                }
                {
                    __stream__.SimpleWrite(@"<M31>");
                    __serializer__.XmlSerialize(M31);
                    __stream__.SimpleWrite(@"</M31>");
                }
                {
                    __stream__.SimpleWrite(@"<M32>");
                    __serializer__.XmlSerialize(M32);
                    __stream__.SimpleWrite(@"</M32>");
                }
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void xmlSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.Extensions.SerializeMatrix3x2> __memberMap__, XmlSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                if (__memberMap__.IsMember(0))
                {
                    __stream__.SimpleWrite(@"<M11>");
                    __serializer__.XmlSerialize(M11);
                    __stream__.SimpleWrite(@"</M11>");
                }
                if (__memberMap__.IsMember(1))
                {
                    __stream__.SimpleWrite(@"<M12>");
                    __serializer__.XmlSerialize(M12);
                    __stream__.SimpleWrite(@"</M12>");
                }
                if (__memberMap__.IsMember(2))
                {
                    __stream__.SimpleWrite(@"<M21>");
                    __serializer__.XmlSerialize(M21);
                    __stream__.SimpleWrite(@"</M21>");
                }
                if (__memberMap__.IsMember(3))
                {
                    __stream__.SimpleWrite(@"<M22>");
                    __serializer__.XmlSerialize(M22);
                    __stream__.SimpleWrite(@"</M22>");
                }
                if (__memberMap__.IsMember(4))
                {
                    __stream__.SimpleWrite(@"<M31>");
                    __serializer__.XmlSerialize(M31);
                    __stream__.SimpleWrite(@"</M31>");
                }
                if (__memberMap__.IsMember(5))
                {
                    __stream__.SimpleWrite(@"<M32>");
                    __serializer__.XmlSerialize(M32);
                    __stream__.SimpleWrite(@"</M32>");
                }
            }
            /// <summary>
            /// 成员 XML 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void XmlDeserialize(AutoCSer.XmlDeserializer __deserializer__, ref AutoCSer.Extensions.SerializeMatrix3x2 __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.XmlDeserialize(ref __value__.M11);
                        return;
                    case 1:
                        __deserializer__.XmlDeserialize(ref __value__.M12);
                        return;
                    case 2:
                        __deserializer__.XmlDeserialize(ref __value__.M21);
                        return;
                    case 3:
                        __deserializer__.XmlDeserialize(ref __value__.M22);
                        return;
                    case 4:
                        __deserializer__.XmlDeserialize(ref __value__.M31);
                        return;
                    case 5:
                        __deserializer__.XmlDeserialize(ref __value__.M32);
                        return;
                }
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> XmlDeserializeMemberNames()
            {
                return xmlDeserializeMemberName();
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> xmlDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(6);
                AutoCSer.LeftArray<KeyValue<int, string>> indexs = new AutoCSer.LeftArray<KeyValue<int, string>>(6);
                names.Add(nameof(M11));
                indexs.Add(new KeyValue<int, string>(0, null));
                names.Add(nameof(M12));
                indexs.Add(new KeyValue<int, string>(1, null));
                names.Add(nameof(M21));
                indexs.Add(new KeyValue<int, string>(2, null));
                names.Add(nameof(M22));
                indexs.Add(new KeyValue<int, string>(3, null));
                names.Add(nameof(M31));
                indexs.Add(new KeyValue<int, string>(4, null));
                names.Add(nameof(M32));
                indexs.Add(new KeyValue<int, string>(5, null));
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>(names, indexs);
            }
            /// <summary>
            /// 获取 Xml 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> XmlSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(float));
                return types;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void XmlSerialize()
            {
                AutoCSer.Extensions.SerializeMatrix3x2 value = default(AutoCSer.Extensions.SerializeMatrix3x2);
                XmlSerialize(null, value);
                XmlSerializeMemberMap(null, null, value, null);
                XmlDeserialize(null, ref value, 0);
                XmlDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Extensions.SerializeMatrix3x2));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.Extensions.SerializeMatrix3x2));
                XmlSerializeMemberTypes();
            }
    }
}namespace AutoCSer.Extensions
{
        /// <summary>
        /// .NET8 类型定义（用于二进制序列化兼容操作）
        /// </summary>
    internal partial struct SerializeMatrix4x4
    {
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void XmlSerialize(AutoCSer.XmlSerializer serializer, AutoCSer.Extensions.SerializeMatrix4x4 value)
            {
                value.xmlSerialize(serializer);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void XmlSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.Extensions.SerializeMatrix4x4> memberMap, XmlSerializer serializer, AutoCSer.Extensions.SerializeMatrix4x4 value, AutoCSer.Memory.CharStream stream)
            {
                value.xmlSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void xmlSerialize(AutoCSer.XmlSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                {
                    __stream__.SimpleWrite(@"<M11>");
                    __serializer__.XmlSerialize(M11);
                    __stream__.SimpleWrite(@"</M11>");
                }
                {
                    __stream__.SimpleWrite(@"<M12>");
                    __serializer__.XmlSerialize(M12);
                    __stream__.SimpleWrite(@"</M12>");
                }
                {
                    __stream__.SimpleWrite(@"<M13>");
                    __serializer__.XmlSerialize(M13);
                    __stream__.SimpleWrite(@"</M13>");
                }
                {
                    __stream__.SimpleWrite(@"<M14>");
                    __serializer__.XmlSerialize(M14);
                    __stream__.SimpleWrite(@"</M14>");
                }
                {
                    __stream__.SimpleWrite(@"<M21>");
                    __serializer__.XmlSerialize(M21);
                    __stream__.SimpleWrite(@"</M21>");
                }
                {
                    __stream__.SimpleWrite(@"<M22>");
                    __serializer__.XmlSerialize(M22);
                    __stream__.SimpleWrite(@"</M22>");
                }
                {
                    __stream__.SimpleWrite(@"<M23>");
                    __serializer__.XmlSerialize(M23);
                    __stream__.SimpleWrite(@"</M23>");
                }
                {
                    __stream__.SimpleWrite(@"<M24>");
                    __serializer__.XmlSerialize(M24);
                    __stream__.SimpleWrite(@"</M24>");
                }
                {
                    __stream__.SimpleWrite(@"<M31>");
                    __serializer__.XmlSerialize(M31);
                    __stream__.SimpleWrite(@"</M31>");
                }
                {
                    __stream__.SimpleWrite(@"<M32>");
                    __serializer__.XmlSerialize(M32);
                    __stream__.SimpleWrite(@"</M32>");
                }
                {
                    __stream__.SimpleWrite(@"<M33>");
                    __serializer__.XmlSerialize(M33);
                    __stream__.SimpleWrite(@"</M33>");
                }
                {
                    __stream__.SimpleWrite(@"<M34>");
                    __serializer__.XmlSerialize(M34);
                    __stream__.SimpleWrite(@"</M34>");
                }
                {
                    __stream__.SimpleWrite(@"<M41>");
                    __serializer__.XmlSerialize(M41);
                    __stream__.SimpleWrite(@"</M41>");
                }
                {
                    __stream__.SimpleWrite(@"<M42>");
                    __serializer__.XmlSerialize(M42);
                    __stream__.SimpleWrite(@"</M42>");
                }
                {
                    __stream__.SimpleWrite(@"<M43>");
                    __serializer__.XmlSerialize(M43);
                    __stream__.SimpleWrite(@"</M43>");
                }
                {
                    __stream__.SimpleWrite(@"<M44>");
                    __serializer__.XmlSerialize(M44);
                    __stream__.SimpleWrite(@"</M44>");
                }
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void xmlSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.Extensions.SerializeMatrix4x4> __memberMap__, XmlSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                if (__memberMap__.IsMember(0))
                {
                    __stream__.SimpleWrite(@"<M11>");
                    __serializer__.XmlSerialize(M11);
                    __stream__.SimpleWrite(@"</M11>");
                }
                if (__memberMap__.IsMember(1))
                {
                    __stream__.SimpleWrite(@"<M12>");
                    __serializer__.XmlSerialize(M12);
                    __stream__.SimpleWrite(@"</M12>");
                }
                if (__memberMap__.IsMember(2))
                {
                    __stream__.SimpleWrite(@"<M13>");
                    __serializer__.XmlSerialize(M13);
                    __stream__.SimpleWrite(@"</M13>");
                }
                if (__memberMap__.IsMember(3))
                {
                    __stream__.SimpleWrite(@"<M14>");
                    __serializer__.XmlSerialize(M14);
                    __stream__.SimpleWrite(@"</M14>");
                }
                if (__memberMap__.IsMember(4))
                {
                    __stream__.SimpleWrite(@"<M21>");
                    __serializer__.XmlSerialize(M21);
                    __stream__.SimpleWrite(@"</M21>");
                }
                if (__memberMap__.IsMember(5))
                {
                    __stream__.SimpleWrite(@"<M22>");
                    __serializer__.XmlSerialize(M22);
                    __stream__.SimpleWrite(@"</M22>");
                }
                if (__memberMap__.IsMember(6))
                {
                    __stream__.SimpleWrite(@"<M23>");
                    __serializer__.XmlSerialize(M23);
                    __stream__.SimpleWrite(@"</M23>");
                }
                if (__memberMap__.IsMember(7))
                {
                    __stream__.SimpleWrite(@"<M24>");
                    __serializer__.XmlSerialize(M24);
                    __stream__.SimpleWrite(@"</M24>");
                }
                if (__memberMap__.IsMember(8))
                {
                    __stream__.SimpleWrite(@"<M31>");
                    __serializer__.XmlSerialize(M31);
                    __stream__.SimpleWrite(@"</M31>");
                }
                if (__memberMap__.IsMember(9))
                {
                    __stream__.SimpleWrite(@"<M32>");
                    __serializer__.XmlSerialize(M32);
                    __stream__.SimpleWrite(@"</M32>");
                }
                if (__memberMap__.IsMember(10))
                {
                    __stream__.SimpleWrite(@"<M33>");
                    __serializer__.XmlSerialize(M33);
                    __stream__.SimpleWrite(@"</M33>");
                }
                if (__memberMap__.IsMember(11))
                {
                    __stream__.SimpleWrite(@"<M34>");
                    __serializer__.XmlSerialize(M34);
                    __stream__.SimpleWrite(@"</M34>");
                }
                if (__memberMap__.IsMember(12))
                {
                    __stream__.SimpleWrite(@"<M41>");
                    __serializer__.XmlSerialize(M41);
                    __stream__.SimpleWrite(@"</M41>");
                }
                if (__memberMap__.IsMember(13))
                {
                    __stream__.SimpleWrite(@"<M42>");
                    __serializer__.XmlSerialize(M42);
                    __stream__.SimpleWrite(@"</M42>");
                }
                if (__memberMap__.IsMember(14))
                {
                    __stream__.SimpleWrite(@"<M43>");
                    __serializer__.XmlSerialize(M43);
                    __stream__.SimpleWrite(@"</M43>");
                }
                if (__memberMap__.IsMember(15))
                {
                    __stream__.SimpleWrite(@"<M44>");
                    __serializer__.XmlSerialize(M44);
                    __stream__.SimpleWrite(@"</M44>");
                }
            }
            /// <summary>
            /// 成员 XML 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void XmlDeserialize(AutoCSer.XmlDeserializer __deserializer__, ref AutoCSer.Extensions.SerializeMatrix4x4 __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.XmlDeserialize(ref __value__.M11);
                        return;
                    case 1:
                        __deserializer__.XmlDeserialize(ref __value__.M12);
                        return;
                    case 2:
                        __deserializer__.XmlDeserialize(ref __value__.M13);
                        return;
                    case 3:
                        __deserializer__.XmlDeserialize(ref __value__.M14);
                        return;
                    case 4:
                        __deserializer__.XmlDeserialize(ref __value__.M21);
                        return;
                    case 5:
                        __deserializer__.XmlDeserialize(ref __value__.M22);
                        return;
                    case 6:
                        __deserializer__.XmlDeserialize(ref __value__.M23);
                        return;
                    case 7:
                        __deserializer__.XmlDeserialize(ref __value__.M24);
                        return;
                    case 8:
                        __deserializer__.XmlDeserialize(ref __value__.M31);
                        return;
                    case 9:
                        __deserializer__.XmlDeserialize(ref __value__.M32);
                        return;
                    case 10:
                        __deserializer__.XmlDeserialize(ref __value__.M33);
                        return;
                    case 11:
                        __deserializer__.XmlDeserialize(ref __value__.M34);
                        return;
                    case 12:
                        __deserializer__.XmlDeserialize(ref __value__.M41);
                        return;
                    case 13:
                        __deserializer__.XmlDeserialize(ref __value__.M42);
                        return;
                    case 14:
                        __deserializer__.XmlDeserialize(ref __value__.M43);
                        return;
                    case 15:
                        __deserializer__.XmlDeserialize(ref __value__.M44);
                        return;
                }
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> XmlDeserializeMemberNames()
            {
                return xmlDeserializeMemberName();
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> xmlDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(16);
                AutoCSer.LeftArray<KeyValue<int, string>> indexs = new AutoCSer.LeftArray<KeyValue<int, string>>(16);
                names.Add(nameof(M11));
                indexs.Add(new KeyValue<int, string>(0, null));
                names.Add(nameof(M12));
                indexs.Add(new KeyValue<int, string>(1, null));
                names.Add(nameof(M13));
                indexs.Add(new KeyValue<int, string>(2, null));
                names.Add(nameof(M14));
                indexs.Add(new KeyValue<int, string>(3, null));
                names.Add(nameof(M21));
                indexs.Add(new KeyValue<int, string>(4, null));
                names.Add(nameof(M22));
                indexs.Add(new KeyValue<int, string>(5, null));
                names.Add(nameof(M23));
                indexs.Add(new KeyValue<int, string>(6, null));
                names.Add(nameof(M24));
                indexs.Add(new KeyValue<int, string>(7, null));
                names.Add(nameof(M31));
                indexs.Add(new KeyValue<int, string>(8, null));
                names.Add(nameof(M32));
                indexs.Add(new KeyValue<int, string>(9, null));
                names.Add(nameof(M33));
                indexs.Add(new KeyValue<int, string>(10, null));
                names.Add(nameof(M34));
                indexs.Add(new KeyValue<int, string>(11, null));
                names.Add(nameof(M41));
                indexs.Add(new KeyValue<int, string>(12, null));
                names.Add(nameof(M42));
                indexs.Add(new KeyValue<int, string>(13, null));
                names.Add(nameof(M43));
                indexs.Add(new KeyValue<int, string>(14, null));
                names.Add(nameof(M44));
                indexs.Add(new KeyValue<int, string>(15, null));
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>(names, indexs);
            }
            /// <summary>
            /// 获取 Xml 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> XmlSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(float));
                return types;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void XmlSerialize()
            {
                AutoCSer.Extensions.SerializeMatrix4x4 value = default(AutoCSer.Extensions.SerializeMatrix4x4);
                XmlSerialize(null, value);
                XmlSerializeMemberMap(null, null, value, null);
                XmlDeserialize(null, ref value, 0);
                XmlDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Extensions.SerializeMatrix4x4));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.Extensions.SerializeMatrix4x4));
                XmlSerializeMemberTypes();
            }
    }
}namespace AutoCSer.Extensions
{
        /// <summary>
        /// .NET8 类型定义（用于二进制序列化兼容操作）
        /// </summary>
    internal partial struct SerializePlane
    {
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void XmlSerialize(AutoCSer.XmlSerializer serializer, AutoCSer.Extensions.SerializePlane value)
            {
                value.xmlSerialize(serializer);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void XmlSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.Extensions.SerializePlane> memberMap, XmlSerializer serializer, AutoCSer.Extensions.SerializePlane value, AutoCSer.Memory.CharStream stream)
            {
                value.xmlSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void xmlSerialize(AutoCSer.XmlSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                {
                    __stream__.SimpleWrite(@"<D>");
                    __serializer__.XmlSerialize(D);
                    __stream__.SimpleWrite(@"</D>");
                }
                {
                    __stream__.SimpleWrite(@"<Normal>");
                    __serializer__.XmlSerializeType(Normal);
                    __stream__.SimpleWrite(@"</Normal>");
                }
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void xmlSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.Extensions.SerializePlane> __memberMap__, XmlSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                if (__memberMap__.IsMember(0))
                {
                    __stream__.SimpleWrite(@"<D>");
                    __serializer__.XmlSerialize(D);
                    __stream__.SimpleWrite(@"</D>");
                }
                if (__memberMap__.IsMember(1))
                {
                    __stream__.SimpleWrite(@"<Normal>");
                    __serializer__.XmlSerializeType(Normal);
                    __stream__.SimpleWrite(@"</Normal>");
                }
            }
            /// <summary>
            /// 成员 XML 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void XmlDeserialize(AutoCSer.XmlDeserializer __deserializer__, ref AutoCSer.Extensions.SerializePlane __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.XmlDeserialize(ref __value__.D);
                        return;
                    case 1:
                        __deserializer__.XmlDeserialize(ref __value__.Normal);
                        return;
                }
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> XmlDeserializeMemberNames()
            {
                return xmlDeserializeMemberName();
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> xmlDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(2);
                AutoCSer.LeftArray<KeyValue<int, string>> indexs = new AutoCSer.LeftArray<KeyValue<int, string>>(2);
                names.Add(nameof(D));
                indexs.Add(new KeyValue<int, string>(0, null));
                names.Add(nameof(Normal));
                indexs.Add(new KeyValue<int, string>(1, null));
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>(names, indexs);
            }
            /// <summary>
            /// 获取 Xml 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> XmlSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(2);
                types.Add(typeof(float));
                types.Add(typeof(AutoCSer.Extensions.SerializeVector3));
                return types;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void XmlSerialize()
            {
                AutoCSer.Extensions.SerializePlane value = default(AutoCSer.Extensions.SerializePlane);
                XmlSerialize(null, value);
                XmlSerializeMemberMap(null, null, value, null);
                XmlDeserialize(null, ref value, 0);
                XmlDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Extensions.SerializePlane));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.Extensions.SerializePlane));
                XmlSerializeMemberTypes();
            }
    }
}namespace AutoCSer.Extensions
{
        /// <summary>
        /// .NET8 类型定义（用于二进制序列化兼容操作）
        /// </summary>
    internal partial struct SerializeQuaternion
    {
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void XmlSerialize(AutoCSer.XmlSerializer serializer, AutoCSer.Extensions.SerializeQuaternion value)
            {
                value.xmlSerialize(serializer);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void XmlSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.Extensions.SerializeQuaternion> memberMap, XmlSerializer serializer, AutoCSer.Extensions.SerializeQuaternion value, AutoCSer.Memory.CharStream stream)
            {
                value.xmlSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void xmlSerialize(AutoCSer.XmlSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                {
                    __stream__.SimpleWrite(@"<W>");
                    __serializer__.XmlSerialize(W);
                    __stream__.SimpleWrite(@"</W>");
                }
                {
                    __stream__.SimpleWrite(@"<X>");
                    __serializer__.XmlSerialize(X);
                    __stream__.SimpleWrite(@"</X>");
                }
                {
                    __stream__.SimpleWrite(@"<Y>");
                    __serializer__.XmlSerialize(Y);
                    __stream__.SimpleWrite(@"</Y>");
                }
                {
                    __stream__.SimpleWrite(@"<Z>");
                    __serializer__.XmlSerialize(Z);
                    __stream__.SimpleWrite(@"</Z>");
                }
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void xmlSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.Extensions.SerializeQuaternion> __memberMap__, XmlSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                if (__memberMap__.IsMember(0))
                {
                    __stream__.SimpleWrite(@"<W>");
                    __serializer__.XmlSerialize(W);
                    __stream__.SimpleWrite(@"</W>");
                }
                if (__memberMap__.IsMember(1))
                {
                    __stream__.SimpleWrite(@"<X>");
                    __serializer__.XmlSerialize(X);
                    __stream__.SimpleWrite(@"</X>");
                }
                if (__memberMap__.IsMember(2))
                {
                    __stream__.SimpleWrite(@"<Y>");
                    __serializer__.XmlSerialize(Y);
                    __stream__.SimpleWrite(@"</Y>");
                }
                if (__memberMap__.IsMember(3))
                {
                    __stream__.SimpleWrite(@"<Z>");
                    __serializer__.XmlSerialize(Z);
                    __stream__.SimpleWrite(@"</Z>");
                }
            }
            /// <summary>
            /// 成员 XML 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void XmlDeserialize(AutoCSer.XmlDeserializer __deserializer__, ref AutoCSer.Extensions.SerializeQuaternion __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.XmlDeserialize(ref __value__.W);
                        return;
                    case 1:
                        __deserializer__.XmlDeserialize(ref __value__.X);
                        return;
                    case 2:
                        __deserializer__.XmlDeserialize(ref __value__.Y);
                        return;
                    case 3:
                        __deserializer__.XmlDeserialize(ref __value__.Z);
                        return;
                }
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> XmlDeserializeMemberNames()
            {
                return xmlDeserializeMemberName();
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> xmlDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(4);
                AutoCSer.LeftArray<KeyValue<int, string>> indexs = new AutoCSer.LeftArray<KeyValue<int, string>>(4);
                names.Add(nameof(W));
                indexs.Add(new KeyValue<int, string>(0, null));
                names.Add(nameof(X));
                indexs.Add(new KeyValue<int, string>(1, null));
                names.Add(nameof(Y));
                indexs.Add(new KeyValue<int, string>(2, null));
                names.Add(nameof(Z));
                indexs.Add(new KeyValue<int, string>(3, null));
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>(names, indexs);
            }
            /// <summary>
            /// 获取 Xml 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> XmlSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(float));
                return types;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void XmlSerialize()
            {
                AutoCSer.Extensions.SerializeQuaternion value = default(AutoCSer.Extensions.SerializeQuaternion);
                XmlSerialize(null, value);
                XmlSerializeMemberMap(null, null, value, null);
                XmlDeserialize(null, ref value, 0);
                XmlDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Extensions.SerializeQuaternion));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.Extensions.SerializeQuaternion));
                XmlSerializeMemberTypes();
            }
    }
}namespace AutoCSer.Extensions
{
        /// <summary>
        /// .NET8 类型定义（用于二进制序列化兼容操作）
        /// </summary>
    internal partial struct SerializeVector2
    {
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void XmlSerialize(AutoCSer.XmlSerializer serializer, AutoCSer.Extensions.SerializeVector2 value)
            {
                value.xmlSerialize(serializer);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void XmlSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.Extensions.SerializeVector2> memberMap, XmlSerializer serializer, AutoCSer.Extensions.SerializeVector2 value, AutoCSer.Memory.CharStream stream)
            {
                value.xmlSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void xmlSerialize(AutoCSer.XmlSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                {
                    __stream__.SimpleWrite(@"<X>");
                    __serializer__.XmlSerialize(X);
                    __stream__.SimpleWrite(@"</X>");
                }
                {
                    __stream__.SimpleWrite(@"<Y>");
                    __serializer__.XmlSerialize(Y);
                    __stream__.SimpleWrite(@"</Y>");
                }
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void xmlSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.Extensions.SerializeVector2> __memberMap__, XmlSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                if (__memberMap__.IsMember(0))
                {
                    __stream__.SimpleWrite(@"<X>");
                    __serializer__.XmlSerialize(X);
                    __stream__.SimpleWrite(@"</X>");
                }
                if (__memberMap__.IsMember(1))
                {
                    __stream__.SimpleWrite(@"<Y>");
                    __serializer__.XmlSerialize(Y);
                    __stream__.SimpleWrite(@"</Y>");
                }
            }
            /// <summary>
            /// 成员 XML 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void XmlDeserialize(AutoCSer.XmlDeserializer __deserializer__, ref AutoCSer.Extensions.SerializeVector2 __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.XmlDeserialize(ref __value__.X);
                        return;
                    case 1:
                        __deserializer__.XmlDeserialize(ref __value__.Y);
                        return;
                }
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> XmlDeserializeMemberNames()
            {
                return xmlDeserializeMemberName();
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> xmlDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(2);
                AutoCSer.LeftArray<KeyValue<int, string>> indexs = new AutoCSer.LeftArray<KeyValue<int, string>>(2);
                names.Add(nameof(X));
                indexs.Add(new KeyValue<int, string>(0, null));
                names.Add(nameof(Y));
                indexs.Add(new KeyValue<int, string>(1, null));
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>(names, indexs);
            }
            /// <summary>
            /// 获取 Xml 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> XmlSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(float));
                return types;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void XmlSerialize()
            {
                AutoCSer.Extensions.SerializeVector2 value = default(AutoCSer.Extensions.SerializeVector2);
                XmlSerialize(null, value);
                XmlSerializeMemberMap(null, null, value, null);
                XmlDeserialize(null, ref value, 0);
                XmlDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Extensions.SerializeVector2));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.Extensions.SerializeVector2));
                XmlSerializeMemberTypes();
            }
    }
}namespace AutoCSer.Extensions
{
        /// <summary>
        /// .NET8 类型定义（用于二进制序列化兼容操作）
        /// </summary>
    internal partial struct SerializeVector3
    {
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void XmlSerialize(AutoCSer.XmlSerializer serializer, AutoCSer.Extensions.SerializeVector3 value)
            {
                value.xmlSerialize(serializer);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void XmlSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.Extensions.SerializeVector3> memberMap, XmlSerializer serializer, AutoCSer.Extensions.SerializeVector3 value, AutoCSer.Memory.CharStream stream)
            {
                value.xmlSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void xmlSerialize(AutoCSer.XmlSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                {
                    __stream__.SimpleWrite(@"<X>");
                    __serializer__.XmlSerialize(X);
                    __stream__.SimpleWrite(@"</X>");
                }
                {
                    __stream__.SimpleWrite(@"<Y>");
                    __serializer__.XmlSerialize(Y);
                    __stream__.SimpleWrite(@"</Y>");
                }
                {
                    __stream__.SimpleWrite(@"<Z>");
                    __serializer__.XmlSerialize(Z);
                    __stream__.SimpleWrite(@"</Z>");
                }
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void xmlSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.Extensions.SerializeVector3> __memberMap__, XmlSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                if (__memberMap__.IsMember(0))
                {
                    __stream__.SimpleWrite(@"<X>");
                    __serializer__.XmlSerialize(X);
                    __stream__.SimpleWrite(@"</X>");
                }
                if (__memberMap__.IsMember(1))
                {
                    __stream__.SimpleWrite(@"<Y>");
                    __serializer__.XmlSerialize(Y);
                    __stream__.SimpleWrite(@"</Y>");
                }
                if (__memberMap__.IsMember(2))
                {
                    __stream__.SimpleWrite(@"<Z>");
                    __serializer__.XmlSerialize(Z);
                    __stream__.SimpleWrite(@"</Z>");
                }
            }
            /// <summary>
            /// 成员 XML 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void XmlDeserialize(AutoCSer.XmlDeserializer __deserializer__, ref AutoCSer.Extensions.SerializeVector3 __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.XmlDeserialize(ref __value__.X);
                        return;
                    case 1:
                        __deserializer__.XmlDeserialize(ref __value__.Y);
                        return;
                    case 2:
                        __deserializer__.XmlDeserialize(ref __value__.Z);
                        return;
                }
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> XmlDeserializeMemberNames()
            {
                return xmlDeserializeMemberName();
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> xmlDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(3);
                AutoCSer.LeftArray<KeyValue<int, string>> indexs = new AutoCSer.LeftArray<KeyValue<int, string>>(3);
                names.Add(nameof(X));
                indexs.Add(new KeyValue<int, string>(0, null));
                names.Add(nameof(Y));
                indexs.Add(new KeyValue<int, string>(1, null));
                names.Add(nameof(Z));
                indexs.Add(new KeyValue<int, string>(2, null));
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>(names, indexs);
            }
            /// <summary>
            /// 获取 Xml 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> XmlSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(float));
                return types;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void XmlSerialize()
            {
                AutoCSer.Extensions.SerializeVector3 value = default(AutoCSer.Extensions.SerializeVector3);
                XmlSerialize(null, value);
                XmlSerializeMemberMap(null, null, value, null);
                XmlDeserialize(null, ref value, 0);
                XmlDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Extensions.SerializeVector3));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.Extensions.SerializeVector3));
                XmlSerializeMemberTypes();
            }
    }
}namespace AutoCSer.Extensions
{
        /// <summary>
        /// .NET8 类型定义（用于二进制序列化兼容操作）
        /// </summary>
    internal partial struct SerializeVector4
    {
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            internal static void XmlSerialize(AutoCSer.XmlSerializer serializer, AutoCSer.Extensions.SerializeVector4 value)
            {
                value.xmlSerialize(serializer);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="memberMap"></param>
            /// <param name="serializer"></param>
            /// <param name="value"></param>
            /// <param name="stream"></param>
            internal static void XmlSerializeMemberMap(AutoCSer.Metadata.MemberMap<AutoCSer.Extensions.SerializeVector4> memberMap, XmlSerializer serializer, AutoCSer.Extensions.SerializeVector4 value, AutoCSer.Memory.CharStream stream)
            {
                value.xmlSerialize(memberMap, serializer, stream);
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__serializer__"></param>
            private void xmlSerialize(AutoCSer.XmlSerializer __serializer__)
            {
                AutoCSer.Memory.CharStream __stream__ = __serializer__.CharStream;
                {
                    __stream__.SimpleWrite(@"<W>");
                    __serializer__.XmlSerialize(W);
                    __stream__.SimpleWrite(@"</W>");
                }
                {
                    __stream__.SimpleWrite(@"<X>");
                    __serializer__.XmlSerialize(X);
                    __stream__.SimpleWrite(@"</X>");
                }
                {
                    __stream__.SimpleWrite(@"<Y>");
                    __serializer__.XmlSerialize(Y);
                    __stream__.SimpleWrite(@"</Y>");
                }
                {
                    __stream__.SimpleWrite(@"<Z>");
                    __serializer__.XmlSerialize(Z);
                    __stream__.SimpleWrite(@"</Z>");
                }
            }
            /// <summary>
            /// XML 序列化
            /// </summary>
            /// <param name="__memberMap__"></param>
            /// <param name="__serializer__"></param>
            /// <param name="__stream__"></param>
            private void xmlSerialize(AutoCSer.Metadata.MemberMap<AutoCSer.Extensions.SerializeVector4> __memberMap__, XmlSerializer __serializer__, AutoCSer.Memory.CharStream __stream__)
            {
                if (__memberMap__.IsMember(0))
                {
                    __stream__.SimpleWrite(@"<W>");
                    __serializer__.XmlSerialize(W);
                    __stream__.SimpleWrite(@"</W>");
                }
                if (__memberMap__.IsMember(1))
                {
                    __stream__.SimpleWrite(@"<X>");
                    __serializer__.XmlSerialize(X);
                    __stream__.SimpleWrite(@"</X>");
                }
                if (__memberMap__.IsMember(2))
                {
                    __stream__.SimpleWrite(@"<Y>");
                    __serializer__.XmlSerialize(Y);
                    __stream__.SimpleWrite(@"</Y>");
                }
                if (__memberMap__.IsMember(3))
                {
                    __stream__.SimpleWrite(@"<Z>");
                    __serializer__.XmlSerialize(Z);
                    __stream__.SimpleWrite(@"</Z>");
                }
            }
            /// <summary>
            /// 成员 XML 反序列化
            /// </summary>
            /// <param name="__deserializer__"></param>
            /// <param name="__value__"></param>
            /// <param name="__memberIndex__"></param>
            internal static void XmlDeserialize(AutoCSer.XmlDeserializer __deserializer__, ref AutoCSer.Extensions.SerializeVector4 __value__, int __memberIndex__)
            {
                switch (__memberIndex__)
                {
                    case 0:
                        __deserializer__.XmlDeserialize(ref __value__.W);
                        return;
                    case 1:
                        __deserializer__.XmlDeserialize(ref __value__.X);
                        return;
                    case 2:
                        __deserializer__.XmlDeserialize(ref __value__.Y);
                        return;
                    case 3:
                        __deserializer__.XmlDeserialize(ref __value__.Z);
                        return;
                }
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> XmlDeserializeMemberNames()
            {
                return xmlDeserializeMemberName();
            }
            /// <summary>
            /// 获取 XML 反序列化成员名称
            /// </summary>
            /// <returns></returns>
            private static AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>> xmlDeserializeMemberName()
            {
                AutoCSer.LeftArray<string> names = new AutoCSer.LeftArray<string>(4);
                AutoCSer.LeftArray<KeyValue<int, string>> indexs = new AutoCSer.LeftArray<KeyValue<int, string>>(4);
                names.Add(nameof(W));
                indexs.Add(new KeyValue<int, string>(0, null));
                names.Add(nameof(X));
                indexs.Add(new KeyValue<int, string>(1, null));
                names.Add(nameof(Y));
                indexs.Add(new KeyValue<int, string>(2, null));
                names.Add(nameof(Z));
                indexs.Add(new KeyValue<int, string>(3, null));
                return new AutoCSer.KeyValue<AutoCSer.LeftArray<string>, AutoCSer.LeftArray<KeyValue<int, string>>>(names, indexs);
            }
            /// <summary>
            /// 获取 Xml 序列化成员类型
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.LeftArray<Type> XmlSerializeMemberTypes()
            {
                AutoCSer.LeftArray<Type> types = new LeftArray<Type>(1);
                types.Add(typeof(float));
                return types;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal static void XmlSerialize()
            {
                AutoCSer.Extensions.SerializeVector4 value = default(AutoCSer.Extensions.SerializeVector4);
                XmlSerialize(null, value);
                XmlSerializeMemberMap(null, null, value, null);
                XmlDeserialize(null, ref value, 0);
                XmlDeserializeMemberNames();
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Extensions.SerializeVector4));
                AutoCSer.AotReflection.ConstructorNonPublicMethods(typeof(AutoCSer.Extensions.SerializeVector4));
                XmlSerializeMemberTypes();
            }
    }
}namespace AutoCSer.Diagnostics
{
        /// <summary>
        /// 服务端时间戳
        /// </summary>
    public partial struct ServerTimestamp
    {
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.Diagnostics.ServerTimestamp value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(24))
                {
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.Time);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.Timestamp);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.TimestampPerSecond);
                }
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.Diagnostics.ServerTimestamp value, byte* end)
            {
                return value.simpleDeserialize(start, end);
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="__start__"></param>
            /// <param name="__end__"></param>
            /// <returns></returns>
            private unsafe byte* simpleDeserialize(byte* __start__, byte* __end__)
            {
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.Time);
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.Timestamp);
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.TimestampPerSecond);
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.Diagnostics.ServerTimestamp value = default(AutoCSer.Diagnostics.ServerTimestamp);
                SimpleSerialize(null, ref value);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Diagnostics.ServerTimestamp));
            }
    }
}namespace AutoCSer
{
        /// <summary>
        /// 索引范围
        /// </summary>
    internal partial struct Range
    {
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="stream"></param>
            /// <param name="value"></param>
            internal static void SimpleSerialize(AutoCSer.Memory.UnmanagedStream stream, ref AutoCSer.Range value)
            {
                value.simpleSerialize(stream);
            }
            /// <summary>
            /// 简单序列化
            /// </summary>
            /// <param name="__stream__"></param>
            private void simpleSerialize(AutoCSer.Memory.UnmanagedStream __stream__)
            {
                if (__stream__.TryPrepSize(8))
                {
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.EndIndex);
                    AutoCSer.SimpleSerialize.Serializer.Serialize(__stream__, this.StartIndex);
                }
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="start"></param>
            /// <param name="value"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            internal unsafe static byte* SimpleDeserialize(byte* start, ref AutoCSer.Range value, byte* end)
            {
                return value.simpleDeserialize(start, end);
            }
            /// <summary>
            /// 简单反序列化
            /// </summary>
            /// <param name="__start__"></param>
            /// <param name="__end__"></param>
            /// <returns></returns>
            private unsafe byte* simpleDeserialize(byte* __start__, byte* __end__)
            {
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.EndIndex);
                __start__ = AutoCSer.SimpleSerialize.Deserializer.Deserialize(__start__, ref this.StartIndex);
                if (__start__ == null || __start__ > __end__) return null;
                return __start__;
            }
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            internal unsafe static void SimpleSerialize()
            {
                AutoCSer.Range value = default(AutoCSer.Range);
                SimpleSerialize(null, ref value);
                SimpleDeserialize(null, ref value, null);
                AutoCSer.AotReflection.NonPublicMethods(typeof(AutoCSer.Range));
            }
    }
}namespace AutoCSer.Extensions
{
    /// <summary>
    /// 触发 AOT 编译
    /// </summary>
    public static class AotMethod
    {
            /// <summary>
            /// 代码生成调用激活 AOT 反射
            /// </summary>
            /// <returns></returns>
            public static bool Call()
            {
                if (AutoCSer.Date.StartTimestamp == long.MinValue)
                {
                    AutoCSer.Diagnostics.ServerTimestamp/**/.BinarySerialize();
                    AutoCSer.Range/**/.BinarySerialize();
                    AutoCSer.Extensions.SerializeComplex/**/.XmlSerialize();
                    AutoCSer.Extensions.SerializeMatrix3x2/**/.XmlSerialize();
                    AutoCSer.Extensions.SerializeMatrix4x4/**/.XmlSerialize();
                    AutoCSer.Extensions.SerializePlane/**/.XmlSerialize();
                    AutoCSer.Extensions.SerializeQuaternion/**/.XmlSerialize();
                    AutoCSer.Extensions.SerializeVector2/**/.XmlSerialize();
                    AutoCSer.Extensions.SerializeVector3/**/.XmlSerialize();
                    AutoCSer.Extensions.SerializeVector4/**/.XmlSerialize();
                    AutoCSer.Diagnostics.ServerTimestamp/**/.SimpleSerialize();
                    AutoCSer.Range/**/.SimpleSerialize();



                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<double>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<float>));
                    AutoCSer.AotReflection.NonPublicFields(typeof(AutoCSer.Xml.TypeSerializer<AutoCSer.Extensions.SerializeVector3>));

                    return true;
                }
                return false;
            }
    }
}
#endif