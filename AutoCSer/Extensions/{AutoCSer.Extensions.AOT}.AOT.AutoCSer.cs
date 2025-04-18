//本文件由程序自动生成，请不要自行修改
using System;
using AutoCSer;

#if NoAutoCSer
#else
#pragma warning disable
namespace AutoCSer
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
                AutoCSer.XmlSerializer.XmlSerialize<AutoCSer.BinarySerializeKeyValue<KT,VT>>();
                AutoCSer.XmlDeserializer.XmlDeserialize<AutoCSer.BinarySerializeKeyValue<KT,VT>>();
            }
    }
}
#endif